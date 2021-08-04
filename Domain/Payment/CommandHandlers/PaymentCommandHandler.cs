using Domain.Payment.Aggregate;
using Domain.Payment.Commands;
using Domain.Payment.Events;

namespace Domain.Payment.CommandHandlers
{
    public interface IPaymentCommandHandler
    {
        Result<Event> Handle(PaymentCommand command);
    }

    public class PaymentCommandHandler : IPaymentCommandHandler
    {
        private readonly IPaymentService _paymentService;
        private readonly IRequestProcessPaymentCommandHandler _requestProcessPaymentCommandHandler;

        public PaymentCommandHandler(
            IPaymentService paymentService,
            IRequestProcessPaymentCommandHandler requestProcessPaymentCommandHandler
        )
        {
            _paymentService = paymentService;
            _requestProcessPaymentCommandHandler = requestProcessPaymentCommandHandler;
        }

        public Result<Event> Handle(PaymentCommand command)
        {
            var paymentResult = command is RequestPaymentCommand
                ? Result.Ok<PaymentAggregate>(null)
                : _paymentService.Get(command.PaymentId);

            if (paymentResult.HasErrors)
                return Result.Failed<Event>(paymentResult.Errors);

            return command switch
            {
                RequestPaymentCommand requestPaymentCommand
                    => _requestProcessPaymentCommandHandler.Handle(requestPaymentCommand),

                _ => Result.Failed<Event>(
                    Error.CreateFrom(
                        "Payment Command Handler",
                        "Command not found")
                )
            };
        }
    }
}
