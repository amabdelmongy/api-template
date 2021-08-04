using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Domain.Payment;
using Domain.Payment.Aggregate;
using WebApi.dto;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentWorkflow _paymentWorkflow;
        private readonly IPaymentService _paymentService; 

        public PaymentController(
            IPaymentWorkflow paymentWorkflow,
            IPaymentService paymentService
        )
        {
            _paymentWorkflow = paymentWorkflow;
            _paymentService = paymentService; 
        }
           
        [HttpPost]
        [Route("request-payment")]
        public ActionResult RequestPayment(
            [FromBody] CardDto cardDto
        )
        {
            var paymentResult =
                _paymentWorkflow.Run(
                    new Card(
                        cardDto.Number,
                        cardDto.Expiry,
                        cardDto.Cvv
                    )
                );

            if (paymentResult.HasErrors)
                return new BadRequestObjectResult(
                    paymentResult.Errors
                        .Select(error => new
                        {
                            subject = error.Subject,
                            message = error.Message
                        }));

            var paymentAggregate =
                _paymentService
                    .Get(paymentResult.Value.AggregateId);

            if (paymentAggregate.HasErrors)
                return new BadRequestObjectResult(
                    paymentAggregate.Errors
                        .Select(error => new
                        {
                            subject = error.Subject,
                            message = error.Message
                        }));

            return Ok(new
                {
                    PaymentId = paymentAggregate.Value.PaymentId,
                }
            );
        }
    }
}