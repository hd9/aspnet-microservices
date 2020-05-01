﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface IOrderProxy
    {
        Task Submit(Order order);
        Task <List<Order>> GetOrdersByAccountId(string accountId);
    }
}