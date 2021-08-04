using System;
using Domain.Payment.Aggregate;

namespace Domain.Payment.Commands
{
    public class RequestPaymentCommand : PaymentCommand
    {
        public RequestPaymentCommand(
            Card card
        )
            : base(Guid.NewGuid())
        {

            Card = card;
        }

        public Card Card { get; }
    }
}

