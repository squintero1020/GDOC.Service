using MediatR;
using Microsoft.Extensions.Logging;
using POSService.Domain.Application.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POSService.Domain.Applications.Application.Commands
{
    public class CreateDailySalesCommandHandler
        : IRequestHandler<CreateDailySalesCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateDailySalesCommandHandler> _logger;

        // Using DI to inject infrastructure persistence Repositories
        public CreateDailySalesCommandHandler(IMediator mediator,
            ILogger<CreateDailySalesCommandHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateDailySalesCommand message, CancellationToken cancellationToken)
        {
            //// Add Integration event to clean the basket
            //var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(message.UserId);
            //await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent);

            //// Add/Update the Buyer AggregateRoot
            //// DDD patterns comment: Add child entities and value-objects through the Order Aggregate-Root
            //// methods and constructor so validations, invariants and business logic 
            //// make sure that consistency is preserved across the whole aggregate
            //var address = new Address(message.Street, message.City, message.State, message.Country, message.ZipCode);
            //var order = new Order(message.UserId, message.UserName, address, message.CardTypeId, message.CardNumber, message.CardSecurityNumber, message.CardHolderName, message.CardExpiration);

            //foreach (var item in message.OrderItems)
            //{
            //    order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
            //}

            //_logger.LogInformation("----- Creating Order - Order: {@Order}", order);

            //_orderRepository.Add(order);

            //return await _orderRepository.UnitOfWork
            //    .SaveEntitiesAsync(cancellationToken);

            return true;
        }
    }
}
