using System;
using Domain.Payment.Aggregate;
using Domain.Payment.CommandHandlers;
using Domain.Payment.Commands;
using Domain.Payment.Events;
using Domain.Payment.InputValidator;

namespace Domain.Payment
{
    public interface IPaymentWorkflow
    {
        Result<Event> Run(Card card);
    }
    public class PaymentWorkflow : IPaymentWorkflow
    {
        private readonly IPaymentCommandHandler _paymentCommandHandler;
        private readonly IPaymentInputValidator _paymentInputValidator;

        public PaymentWorkflow(
            IPaymentCommandHandler paymentCommandHandler,
            IPaymentInputValidator paymentInputValidator
        )
        {
            _paymentCommandHandler = paymentCommandHandler;
            _paymentInputValidator = paymentInputValidator;
        }

        public Result<Event> Run(Card card)
        {
            var validateStatus =
                _paymentInputValidator.Validate(
                    card
                );

            if (validateStatus.HasErrors)
                return Result.Failed<Event>(validateStatus.Errors);

            var paymentRequestedEvent =
                _paymentCommandHandler.Handle(
                    new RequestPaymentCommand(card)
                );

            if (paymentRequestedEvent.HasErrors)
                return Result.Failed<Event>(paymentRequestedEvent.Errors);

            return paymentRequestedEvent;
        }
    }
}
