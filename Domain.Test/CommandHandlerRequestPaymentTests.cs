using System.Linq;
using Domain.Payment.CommandHandlers;
using Domain.Payment.Commands;
using Domain.Payment.Events;
using Moq;
using NUnit.Framework;

namespace Domain.Test
{
    public class CommandHandlerRequestPaymentTests
    {

        [Test]
        public void WHEN_Handle_RequestPaymentCommand_THEN_Create_PaymentRequestedEvent_Event_and_return_Ok()
        {
            var eventRepository = new Mock<IEventRepository>();
            eventRepository
                    .Setup(repository => repository.Add(It.IsAny<Event>()))
                    .Returns(Result.Ok<object>());
             
            var requestProcessPaymentCommandHandler = new RequestPaymentCommandHandler(eventRepository.Object);

            var requestPaymentCommand = new RequestPaymentCommand(
                PaymentStubsTests.CardTest
            );

            var actualResult = requestProcessPaymentCommandHandler.Handle(requestPaymentCommand);

            Assert.IsTrue(actualResult.IsOk);
            var actual = actualResult.Value;
            Assert.AreEqual("PaymentRequestedEvent", actual.Type);
            Assert.AreEqual(requestPaymentCommand.PaymentId, actual.AggregateId);
            Assert.AreEqual(1, actual.Version);
            eventRepository.Verify(mock => mock.Add(It.IsAny<Event>()), Times.Once());
        }

        [Test]
        public void WHEN_Handle_RequestPaymentCommand_And_eventRepository_return_error_THEN_return_Error()
        {
            var expectedError =
                Error.CreateFrom("Error Subject", "Error Message");

            var eventRepositoryMock =
                new Mock<IEventRepository>();

            eventRepositoryMock
                .Setup(repository =>
                    repository.Add(It.IsAny<Event>())
                )
                .Returns(Result.Failed<object>(expectedError));

            var requestProcessPaymentCommandHandler =
                new RequestPaymentCommandHandler(eventRepositoryMock.Object);

            var requestPaymentCommand = new RequestPaymentCommand(
                PaymentStubsTests.CardTest
            );

            var actualResult = requestProcessPaymentCommandHandler.Handle(requestPaymentCommand);

            Assert.IsTrue(actualResult.HasErrors);
            Assert.AreEqual(1, actualResult.Errors.Count());
            var actualError = actualResult.Errors.First();

            Assert.AreEqual(expectedError.Subject, actualError.Subject);
            Assert.AreEqual(expectedError.Message, actualError.Message);

            eventRepositoryMock.Verify(mock => mock.Add(It.IsAny<Event>()), Times.Once());
        }
    }
}