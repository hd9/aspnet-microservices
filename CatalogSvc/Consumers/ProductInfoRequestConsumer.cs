using CatalogSvc.Services;
using Core.Commands.Catalog;
using Core.Infrastructure.Extentions;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogSvc.Consumers
{
    public class ProductInfoRequestConsumer : IConsumer<ProductInfoRequest>
    {

        private readonly ICatalogSvc _svc;

        public ProductInfoRequestConsumer(ICatalogSvc svc)
        {
            _svc = svc;
        }

        public async Task Consume(ConsumeContext<ProductInfoRequest> context)
        {
            if (context == null || context.Message == null || !context.Message.Slugs.HasAny())
            {
                await context.RespondAsync(null);
                return;
            }

            await context.RespondAsync(
                _svc.GetProducts(context.Message.Slugs)
            );
        }
    }
}
