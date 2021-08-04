using System;

namespace Domain.Payment.Aggregate
{
    public class PaymentAggregate
    {
        public PaymentAggregate()
        {

        }
        PaymentAggregate(
            Guid id,
            Card card,
            int version
        )
        {
            PaymentId = id;
            Card = card;
            Version = version;
        }

        public Guid PaymentId { get; }

        public Card Card { get; }

        public int Version { get; }

        public PaymentAggregate With(
            Guid id,
            Card card,
            int version
        )
        {
            return
                new PaymentAggregate(
                    id,
                    card,
                    version
                );
        }
    }
}