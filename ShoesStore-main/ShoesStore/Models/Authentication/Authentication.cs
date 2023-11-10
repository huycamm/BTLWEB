using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShoesStore.Models.Authentication
{
    public class AuthenticationAdmin:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("TaiKhoan") == null)
            {
                context.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "Controller", "Access" },
                    { "Action", "Login" }
                });
            }
        }
    }
}
