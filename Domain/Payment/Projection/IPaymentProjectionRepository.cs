using System;
using System.Collections.Generic;
using Domain.Payment.Events;

namespace Domain.Payment.Projection
{
    public interface IPaymentProjectionRepository
    {
        Result<PaymentProjection> Get(Guid id);
        Result<IEnumerable<PaymentProjection>> Get();
        Result<object> Add(PaymentProjection paymentProjection);
    }
}