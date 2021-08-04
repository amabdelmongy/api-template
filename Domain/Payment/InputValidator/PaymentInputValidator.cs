using System.Collections.Generic;
using Domain.Payment.Aggregate;

namespace Domain.Payment.InputValidator
{
    public interface IPaymentInputValidator
    {
        Result<object> Validate(Card card);
    }

    public class PaymentInputValidator : IPaymentInputValidator
    {   
        public Result<object> Validate(
            Card card
        )
        {
            var validatedCardResult = new CardValidator().Validate(card);

            if (validatedCardResult.IsOk)
                return Result.Ok<object>();

            var errors = new List<Error>();
            errors.AddRange(validatedCardResult.Errors);
            return Result.Failed<object>(errors);
        }
    }
}

