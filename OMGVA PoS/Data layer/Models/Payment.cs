﻿using OMGVA_PoS.Data_layer.Enums;

namespace OMGVA_PoS.Data_layer.Models
{
    public class Payment
    {
        public string Id { get; set; }
        public PaymentMethod Method { get; set; }
        public long CustomerId { get; set; }
        public long? GiftCardPaymentId { get; set; }
    }
}
