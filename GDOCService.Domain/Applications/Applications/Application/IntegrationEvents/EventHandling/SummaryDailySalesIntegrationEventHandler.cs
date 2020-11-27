using EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using POSService.Domain.Application.Commands;
using POSService.Domain.Application.IntegrationEvents.Events;
using Serilog.Context;
using SharedService.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSService.Domain.Application.IntegrationEvents.EventHandling
{
    public class SummaryDailySalesIntegrationEventHandler : IIntegrationEventHandler<SummaryDailySalesIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SummaryDailySalesIntegrationEventHandler> _logger;

        public SummaryDailySalesIntegrationEventHandler(IMediator mediator,
            ILogger<SummaryDailySalesIntegrationEventHandler> logger) => (_mediator, _logger) = (mediator ?? throw new ArgumentNullException(nameof(mediator)), logger ?? throw new ArgumentNullException(nameof(logger)));


        /// <summary>
        /// Integration event handler which starts the create order process
        /// </summary>
        /// <param name="@event">
        /// Integration event message which is sent by the
        /// basket.api once it has successfully process the 
        /// order items.
        /// </param>
        /// <returns></returns>
        public async Task Handle(SummaryDailySalesIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{"POSService"}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "POSService", JsonFormatting.Formated(@event));

                var result = false;

                if (@event.RequestId != Guid.Empty)
                {
                    using (LogContext.PushProperty("IdentifiedCommandId", @event.RequestId))
                    {
                        var createDailySalesCommand = new CreateDailySalesCommand(@event.summaryDailySales);

                        var requestCreateDailySales = new IdentifiedCommand<CreateDailySalesCommand, bool>(createDailySalesCommand, @event.RequestId);

                        //_logger.LogInformation(
                        //    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                        //    "CreateDailySalesCommand",
                        //    nameof(requestCreateDailySales.Id),
                        //    requestCreateDailySales.Id,
                        //    requestCreateDailySales);

                        //result = await _mediator.Send(requestCreateDailySales);

                        //if (result)
                        //{
                        //    _logger.LogInformation("----- CreateDailySalesCommand suceeded - RequestId: {RequestId}", @event.RequestId);
                        //}
                        //else
                        //{
                        //    _logger.LogWarning("CreateDailySalesCommand failed - RequestId: {RequestId}", @event.RequestId);
                        //}
                    }
                }
                else
                {
                    _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", @event);
                }
            }
        }

    }
}
