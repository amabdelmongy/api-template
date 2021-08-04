using WebApi.dto;

namespace WebApi.Integration.Test
{
    public static class CardDtoTests
    {
        public static CardDto Card = new CardDto
        {
            Number = "5105105105105100",
            Expiry = "8/22",
            Cvv = "123"
        };
    }
}
