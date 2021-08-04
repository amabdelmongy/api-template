using System;
using System.Linq;
using Domain.Payment.Projection;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/payment-details")]
    public class PaymentDetailsController : ControllerBase
    { 
        private readonly IPaymentProjectionRepository _paymentProjectionRepository;

        public PaymentDetailsController( 
            IPaymentProjectionRepository paymentProjectionRepository
        )
        {
            _paymentProjectionRepository = paymentProjectionRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var paymentResult =
                _paymentProjectionRepository.Get();

            if (paymentResult.HasErrors)
                return new BadRequestObjectResult(
                    paymentResult.Errors
                        .Select(error => new
                        {
                            subject = error.Subject,
                            message = error.Message
                        }));

            if (paymentResult.Value == null)
                return new NotFoundResult();

            var payments = paymentResult.Value;

            return Ok(
                payments.ToList().Select(payment =>
                    new
                    {
                        paymentId = payment.PaymentId,
                        Number = payment.CardNumber,
                        Expiry = payment.CardExpiry,
                        Cvv = payment.CardCvv,
                        LastUpdatedDate = payment.LastUpdatedDate
                    })
                );
        }

        [HttpGet]
        [Route("{paymentId}")]
        public ActionResult Get(Guid paymentId)
        {
            var paymentResult =
                _paymentProjectionRepository.Get(paymentId);

            if (paymentResult.HasErrors)
                return new BadRequestObjectResult(
                    paymentResult.Errors
                        .Select(error => new
                        {
                            subject = error.Subject,
                            message = error.Message
                        }));

            if (paymentResult.Value == null)
                return new NotFoundResult();

            var payment = paymentResult.Value;

            return Ok(
                new
                {
                    paymentId = payment.PaymentId,
                    Number = payment.CardNumber,
                    Expiry = payment.CardExpiry,
                    Cvv = payment.CardCvv,
                    LastUpdatedDate = payment.LastUpdatedDate
                }
            );
        }
    }
}