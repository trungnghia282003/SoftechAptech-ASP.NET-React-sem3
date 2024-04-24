using Microsoft.AspNetCore.Mvc;
using OA.Api.Responses;

namespace OA.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }

        public AuthenticateResponse CurrentUser
        {
            get
            {
                return (AuthenticateResponse?)HttpContext.Items["User"];
            }
        }
    }
}
