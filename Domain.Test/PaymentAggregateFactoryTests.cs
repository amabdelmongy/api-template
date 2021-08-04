using System.Collections.Generic;
using Domain.Payment.Aggregate;
using Domain.Payment.Events;
using NUnit.Framework;

namespace Domain.Test
{
    public class PaymentAggregateFactoryTests
    {
        [Test]
        public void WHEN_pass_PaymentRequestedEvent_THEN_return_correct_Aggregate()
        { 
            var expectedEvent = PaymentStubsTests.PaymentRequestedEventTest;
            var paymentAggregateResult =
                PaymentAggregateFactory.CreateFrom(
                    new List<Event>
                    {
                        expectedEvent
                    }
                );

            Assert.True(paymentAggregateResult.IsOk);
            var actualAggregate = paymentAggregateResult.Value;

            Assert.AreEqual(expectedEvent.AggregateId, actualAggregate.PaymentId);
            Assert.AreEqual(expectedEvent.Card, actualAggregate.Card);
            Assert.AreEqual(expectedEvent.Version, actualAggregate.Version);
        }
    }
}
