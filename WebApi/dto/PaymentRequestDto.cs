using System;

namespace WebApi.dto
{
    public class CardDto
    {
        public string Number { get; set; }
        public string Expiry { get; set; }
        public string Cvv { get; set; }
    }
}
