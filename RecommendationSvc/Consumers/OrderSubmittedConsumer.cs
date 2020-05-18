using Core.Events.Orders;
using MassTransit;
using MassTransit.RabbitMqTransport;
using RecommendationSvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationSvc.Consumers
{
    public class OrderSubmittedConsumer : IConsumer<OrderSubmitted>
    {

        private readonly IRecommendationSvc _svc;


        public OrderSubmittedConsumer(IRecommendationSvc svc)
        {
            _svc = svc;
        }

        public async Task Consume(ConsumeContext<OrderSubmitted> context)
        {
            await _svc.BuildRecommendation(context.Message);
        }
    }
}
