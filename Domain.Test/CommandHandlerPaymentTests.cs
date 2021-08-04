using System; 
using System.Linq;
using Domain.Payment;
using Domain.Payment.Aggregate;
using Domain.Payment.CommandHandlers;
using Domain.Payment.Commands;
using Domain.Payment.Events;
using Moq;
using NUnit.Framework;

namespace Domain.Test
{
    public class CommandHandlerPaymentTests
    {
        private Mock<IPaymentService> paymentServiceMock()
        { 
            var paymentServiceMock = new Mock<IPaymentService>();

            paymentServiceMock
                .Setup(service =>
                    service.Get(It.IsAny<Guid>()))
                .Returns(Result.Ok<PaymentAggregate>(PaymentStubsTests.PaymentAggregateTest()));
            return paymentServiceMock;
        }
        private Mock<IRequestProcessPaymentCommandHandler> requestProcessPaymentCommandHandlerMock()
        {
            var requestProcessPaymentCommandHandlerMock = new Mock<IRequestProcessPaymentCommandHandler>();
            requestProcessPaymentCommandHandlerMock
                .Setup(commandHandler =>
                    commandHandler.Handle(It.IsAny<RequestPaymentCommand>())
                )
                .Returns(It.IsAny<Result<Event>>());
            return requestProcessPaymentCommandHandlerMock;
        }

        [Test]
        public void WHEN_handle_RequestPaymentCommand_THEN_should_call_requestProcessPaymentCommandHandler()
        {  
            var requestProcessPaymentCommandHandler = requestProcessPaymentCommandHandlerMock();

            var paymentCommandHandler =
                new PaymentCommandHandler(
                    paymentServiceMock().Object,
                    requestProcessPaymentCommandHandler.Object
                );

            var requestPaymentCommand = new RequestPaymentCommand(
                PaymentStubsTests.CardTest
            );

            paymentCommandHandler.Handle(requestPaymentCommand);

            requestProcessPaymentCommandHandler.Verify(
                mock =>
                    mock.Handle(
                        requestPaymentCommand),
                Times.Once());
        }
    }
}