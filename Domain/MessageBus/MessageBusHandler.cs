using System.Threading.Tasks;
using Domain.Payment.Events;
using Domain.Payment.Projection;

namespace Domain.MessageBus
{
    public interface IMessageBusHandler
    {
        Task Handle(Event paymentEvent);
    }

    public class MessageBusHandler : IMessageBusHandler
    {
        private readonly IPaymentProjectionRepository _paymentProjectionRepository;

        public MessageBusHandler(IPaymentProjectionRepository paymentProjectionRepository)
        {
            _paymentProjectionRepository = paymentProjectionRepository;
        }

        public async Task Handle(Event paymentEvent)
        {
            switch (paymentEvent)
            {
                case PaymentRequestedEvent paymentRequestedEvent:
                    await Handel(paymentRequestedEvent);
                    break;
            }
        }

        private async Task Handel(PaymentRequestedEvent paymentRequestedEvent)
        {
            var paymentProjection = new PaymentProjection
            {
                PaymentId = paymentRequestedEvent.AggregateId,
                CardNumber = paymentRequestedEvent.Card.Number,
                CardExpiry = paymentRequestedEvent.Card.Expiry,
                CardCvv = paymentRequestedEvent.Card.Cvv,
                LastUpdatedDate = paymentRequestedEvent.TimeStamp
            };
            _paymentProjectionRepository.Add(paymentProjection);
        }
    }
}
