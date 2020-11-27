using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSService.Domain.Application.IntegrationEvents
{
    public interface IPOSServiceIntegrationEventService
    {
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task SaveEventAndChangesAsync(IntegrationEvent evt);
    }
}
