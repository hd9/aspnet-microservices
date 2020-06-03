using Microservices.Core.Contracts.Orders;
using MassTransit;
using RecommendationSvc.Services;
using System.Threading.Tasks;

namespace RecommendationSvc.Consumers
{
    public class OrderSubmittedConsumer : IConsumer<OrderSubmitted>
    {

        readonly IRecommendationSvc _svc;

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
