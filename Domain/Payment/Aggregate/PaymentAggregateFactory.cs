using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Payment.Events;

namespace Domain.Payment.Aggregate
{
    public static class PaymentAggregateFactory
    {
        public static Result<PaymentAggregate> CreateFrom(IEnumerable<Event> events)
        {
            var resultPayment =
                events
                    .OrderBy(x => x.Version)
                    .ToList()
                    .Aggregate(new PaymentAggregate(), (paymentAggregate, e) =>
                    {
                        switch (e)
                        {
                            case PaymentRequestedEvent @event:
                                paymentAggregate =
                                    paymentAggregate.With(
                                        @event.AggregateId,
                                        @event.Card,
                                        @event.Version
                                    );
                                break;

                            default:
                                throw new NotSupportedException();
                        }

                        return paymentAggregate;
                    });

            return Result.Ok(resultPayment);
        }
    }
}
