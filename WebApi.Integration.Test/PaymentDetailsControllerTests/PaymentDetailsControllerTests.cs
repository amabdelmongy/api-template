using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Domain;
using Domain.Payment.Aggregate;
using Domain.Payment.Projection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace WebApi.Integration.Test.PaymentDetailsControllerTests
{
    public class PaymentDetailsControllerTests
    {
        private const string UrlPaymentGet = "/api/v1/payment-details/";

        private HttpClient CreateClient(List<PaymentProjection> paymentProjections)
        {
            var inMemoryPaymentProjectionRepository = new InMemoryPaymentProjectionRepository();
            paymentProjections.ForEach(paymentProjection =>
                inMemoryPaymentProjectionRepository.Add(paymentProjection)
            );

            var factory =
                new WebApplicationFactory<Startup>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureTestServices(services =>
                        {
                            services.AddScoped(a =>
                                (IPaymentProjectionRepository) inMemoryPaymentProjectionRepository);
                        });
                    });

            return factory.CreateClient();
        }

        [Test]
        public async Task WHEN_Get_by_payment_id_THEN_return_correct_Payment()
        {
            var expectPaymentProjection = new PaymentProjection
            {
                PaymentId = Guid.Parse("f00526d2-cfe6-4a34-8ea4-f54ccc00ae7d"),
                CardNumber = "5105105105105100",
                CardExpiry = "10/24",
                CardCvv = "321",
                LastUpdatedDate = DateTime.Now
            };
            var expectedCardNumber = "5105105105105100";

            var client = CreateClient(
                new List<PaymentProjection>()
                {
                    expectPaymentProjection
                });

            var response =
                await client.GetAsync(
                    UrlPaymentGet + expectPaymentProjection.PaymentId);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var outputDefinition = new
            {
                paymentId = "",
                CardNumber = "",
                CardExpiry = "",
                CardCvv = "",
                LastUpdatedDate = ""
            };

            var output = JsonConvert.DeserializeAnonymousType(result, outputDefinition);
            Assert.AreEqual(expectPaymentProjection.PaymentId.ToString(), output.paymentId);
            Assert.AreEqual(expectedCardNumber, output.CardNumber);
            Assert.AreEqual(expectPaymentProjection.CardExpiry, output.CardExpiry);
            Assert.AreEqual(expectPaymentProjection.CardCvv, output.CardCvv);
            Assert.AreEqual(expectPaymentProjection.LastUpdatedDate, DateTime.Parse(output.LastUpdatedDate));
        } 

        [Test]
        public async Task WHEN_Repository_return_Exception_THEN_return_Error()
        {
            var expectedError = Error.CreateFrom(
                "Failed calling Data base",
                new Exception("Test Exception from PaymentProjection Repository"));

            Result<PaymentProjection> expectedResult = Result.Failed<PaymentProjection>(expectedError);

            var inMemoryPaymentProjectionRepository = new InMemoryPaymentProjectionRepository();
            inMemoryPaymentProjectionRepository.WithNewGetResult(expectedResult);

            var factory =
                new WebApplicationFactory<Startup>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureTestServices(services =>
                        {
                            services.AddScoped(a =>
                                (IPaymentProjectionRepository) inMemoryPaymentProjectionRepository);
                        });
                    });

            var client = factory.CreateClient();

            var response =
                await client.GetAsync(
                    UrlPaymentGet + Guid.NewGuid());


            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();

            var outputErrorDefinition = new[]
            {
                new
                {
                    subject = "",
                    message = ""
                }
            };

            var output = JsonConvert.DeserializeAnonymousType(result, outputErrorDefinition);
            Assert.AreEqual(expectedError.Subject, output[0].subject);
            Assert.AreEqual(expectedError.Message, output[0].message);
        }
    }
}