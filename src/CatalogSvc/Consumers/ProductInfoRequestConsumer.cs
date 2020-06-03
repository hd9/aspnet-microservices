using CatalogSvc.Infrastructure.Extensions;
using CatalogSvc.Models;
using CatalogSvc.Services;
using Microservices.Core.Contracts.Catalog;
using Microservices.Core.Infrastructure.Extensions;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogSvc.Consumers
{
    public class ProductInfoRequestConsumer : IConsumer<ProductInfoRequest>
    {

        readonly ICatalogSvc _svc;

        public ProductInfoRequestConsumer(ICatalogSvc svc)
        {
            _svc = svc;
        }

        public async Task Consume(ConsumeContext<ProductInfoRequest> context)
        {
            if (!context.Message.Slugs.HasAny())
            {
                await context.RespondAsync(null);
                return;
            }

            var pis = await _svc.GetProducts(context.Message.Slugs);
            var productInfoResponse = new ProductInfoResponse
            {
                ProductInfos = ((List<Product>)pis).ConvertAll(pi => pi.ToProductInfo())
            };

            await context.RespondAsync(productInfoResponse);
        }
    }
}
