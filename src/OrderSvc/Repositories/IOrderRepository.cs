﻿using OrderSvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderSvc.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId);
        Task Insert(Order order);
        Task<Order> GetById(int id, bool fullGraph = false);
        Task<Order> GetByNumber(string number, bool fullGraph = false);
        Task Update(int id, OrderStatus orderStatus, PaymentStatus paymentStatus, ShippingStatus shippingStatus);
    }
}