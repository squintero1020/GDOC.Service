using EventBus.Events;
using POSService.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace POSService.Domain.Application.IntegrationEvents.Events
{
    public class SummaryDailySalesIntegrationEvent : IntegrationEvent
    {
        public Guid RequestId { get; set; }

        public SummaryDailySales summaryDailySales { get; set; }

        public SummaryDailySalesIntegrationEvent(Guid requestId,
            SummaryDailySales summary) => (RequestId, summaryDailySales) = (requestId, summary);
    }
}
