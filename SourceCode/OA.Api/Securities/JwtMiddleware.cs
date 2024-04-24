
using QLTC.Application.Utils;

namespace OA.Api.Securities
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrWhiteSpace(token))
            {
                var user = JwtUtils.ValidateJwtToken(token);
                if (user != null)
                {
                    context.Items["User"] = user;
                }
            }
            await _next(context);
        }
    }
}
