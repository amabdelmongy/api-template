using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using WebApi.Controllers.v1;
using WebApi.dto;

namespace WebApi.Integration.Test.PaymentControllerTests
{
    public class InputTests
    {
        private readonly HttpClient _client;
        private const string UrlRequestPayment = "/api/v1/payment/request-payment/";

        public InputTests()
        {

            var factory =
                new WebApplicationFactory<Startup>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureTestServices(services =>
                        {
                            IEventRepository eventRepository = new InMemoryEventRepository(); 
                            services.AddScoped(a => eventRepository);
                        });
                    });

            _client = factory.CreateClient();
        }

        [Test]
        public async Task WHEN_PaymentRequestDto_is_correct_THEN_return_OK()
        {
            var cardDto = new CardDto
                {
                    Number = "5105105105105100",
                    Expiry = "9/22",
                    Cvv = "123"
                };

            var response =
                await _client.PostAsJsonAsync(
                    UrlRequestPayment,
                    cardDto);
             
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var outputDefinition = new
            {
                acquiringBankId = "",
                merchantId = "",
                paymentId = ""
            };
                
            var output = JsonConvert.DeserializeAnonymousType(result, outputDefinition);
        }

        [Test]
        public async Task WHEN_card_number_empty_THEN_return_Error()
        {
            var cardDto = new CardDto
                {
                    Number = "",
                    Expiry = "10/24",
                    Cvv = "123"
                };

            var response =
                await _client.PostAsJsonAsync(
                    UrlRequestPayment,
                    cardDto
                );
             
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode );

            var result = await response.Content.ReadAsStringAsync();

            var outputErrorDefinition = new []
            {
                new
                {
                    subject = "",
                    message = ""
                }
            };

            var output = JsonConvert.DeserializeAnonymousType(result, outputErrorDefinition);
            Assert.AreEqual("Invalid Card Number", output[0].subject);
            Assert.AreEqual("Card Number is Empty", output[0].message);
        }

        [Test]
        public async Task WHEN_card_Expiry_empty_THEN_return_Error()
        {
            var cardDto = new CardDto
                {
                    Number = "5105105105105100",
                    Expiry = "",
                    Cvv = "123"
                };

            var response =
                await _client.PostAsJsonAsync(
                    UrlRequestPayment,
                    cardDto
                );
             
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
            Assert.AreEqual("Invalid Expiry Date", output[0].subject);
            Assert.AreEqual("Expire date is Empty", output[0].message);
        }

        [Test]
        public async Task WHEN_card_Cvv_empty_THEN_return_Error()
        {
            var cardDto = new CardDto
                {
                    Number = "5105105105105100",
                    Expiry = "10/24",
                    Cvv = ""
                };

            var response =
                await _client.PostAsJsonAsync(
                    UrlRequestPayment,
                    cardDto
                );
             
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
            Assert.AreEqual("Invalid CVV", output[0].subject);
            Assert.AreEqual("Card CVV is Empty", output[0].message);
        }
    }
}