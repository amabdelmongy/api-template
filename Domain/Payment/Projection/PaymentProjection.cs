using System;

namespace Domain.Payment.Projection
{
    public class PaymentProjection
    {
        public Guid PaymentId { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiry { get; set; }
        public string CardCvv { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
