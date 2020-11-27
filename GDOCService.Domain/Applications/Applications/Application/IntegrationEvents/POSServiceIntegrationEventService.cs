using EventBus.Abstractions;
using EventBus.Events;
using EventBus.Extensions;
using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using IntegrationEventLogEF.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using POSService.DataAccess.DataContext;
using POSService.DataAccess.Models;
using POSService.Domain.Application.IntegrationEvents;
using SharedService.Functions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace POSService.Domain.Application.IntegrationEvents
{
    public class POSServiceIntegrationEventService : IPOSServiceIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly POSContext _posContext;
        private readonly IntegrationEventLogContext _integrationContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<POSServiceIntegrationEventService> _logger;

        public POSServiceIntegrationEventService(IEventBus eventBus,
            POSContext posContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
            ILogger<POSServiceIntegrationEventService> logger,
            IntegrationEventLogContext integrationContext)
        {
            _posContext = posContext ?? throw new ArgumentNullException(nameof(posContext));
            _integrationContext = integrationContext ?? throw new ArgumentNullException(nameof(integrationContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_integrationContext.Database.GetDbConnection());
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

            foreach (var logEvt in pendingLogEvents)
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", logEvt.EventId, "POSService", logEvt.IntegrationEvent);

                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    _eventBus.Publish(logEvt.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", logEvt.EventId, "POSService");

                    await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {

                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, "POSIntegrationEventService", JsonFormatting.Formated(evt));

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                _eventBus.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, "POSIntegrationEventService", JsonFormatting.Formated(evt));
                //await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task SaveEventAndChangesAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- POSIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);
            await ResilientTransaction.New(_posContext, _logger).ExecuteAsync(async () =>
             {
                // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
                try
                 {
                     if (await _posContext.SaveChangesAsync() <= 0)
                     {
                         _logger.LogError("----- POSIntegrationEventService {0}", "Not can saved in Db");
                     }
                 }
                 catch (DbUpdateException e)
                 {
                     _logger.LogError("***** Error In SaveChangesAsync Process {0}", e.Message);
                 }
                 
                await _eventLogService.SaveEventAsync(@event : evt, transaction: Guid.NewGuid());
            });
        }
    }
}
