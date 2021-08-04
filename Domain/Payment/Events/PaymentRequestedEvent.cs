using System;
using Domain.Payment.Aggregate;

namespace Domain.Payment.Events
{
    public class PaymentRequestedEvent : Event
    {
        public PaymentRequestedEvent(
            Guid aggregateId,
            DateTime timeStamp,
            int version,
            Card card
        )
            : base(
                aggregateId,
                timeStamp,
                version,
                typeof(PaymentRequestedEvent)
            )
        {
            Card = card;
        }

        public Card Card { get; }

    }
}