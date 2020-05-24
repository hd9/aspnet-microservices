﻿using HildenCo.Core.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HildenCo.Core.Contracts.Payment
{
    public class PaymentResponse : CommandBase
    {
        public int AccountId { get; set; }

        public int OrderId { get; set; }

        public PaymentStatus Status { get; set; }
    }
}
