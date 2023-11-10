using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShoesStore.Models.Authentication
{
    public class AuthenicationAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("TaiKhoan") == null || context.HttpContext.Session.GetInt32("Role") != 0)
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
