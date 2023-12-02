using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Conventions
{
    public static class PaymentApiConvention
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static void AddAccountMoneyAsync(long accountId)
        {
        }
    }
}
