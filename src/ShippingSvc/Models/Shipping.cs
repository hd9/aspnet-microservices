using System;
using Microservices.Core.Contracts.Shipping;
using Microservices.Core.Infrastructure.Extensions;

namespace ShippingSvc.Models
{
    public class Shipping
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public int AccountId { get; set; }
        
        public int OrderId { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }
        
        public string Currency { get; set; }

        public string Street { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public ShippingStatus Status { get; set; }

        public ShippingProvider Provider { get; set; }

        public static Shipping Create(ShippingRequest sr)
        {
            if (sr == null)
                return null;

            // todo :: automapper
            return new Shipping
            {
                Number = NewNumber(),
                AccountId = sr.AccountId,
                Name = sr.Name,
                Currency = sr.Currency,
                OrderId = sr.OrderId,
                Amount = sr.Amount,
                Street = sr.Street,
                PostalCode = sr.PostalCode,
                City = sr.City,
                Region = sr.Region,
                Country = sr.Country,
                Status = sr.Status.Parse<ShippingStatus>(),
                Provider = sr.Provider.Parse<ShippingProvider>()
            };
        }

        private static string NewNumber()
        {
            var td = DateTime.UtcNow;
            return $"S-{td.ToString("yyyy-MM")}-{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToUpper()}";
        }
    }
}
