using Microsoft.AspNetCore.Mvc;
using OA.Api.Interfaces;
using OA.Api.Requests;
using OA.Api.Securities;
using QLTC.Application.Utils;

namespace OA.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> CreateAsync(CreateUserRequest model)
        {
            try
            {
                var errorMessge = await _repository.CreateAsync(model);
                return Ok(new { Result = errorMessge });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateRequest model)
        {
            try
            {
                var authenticateResponse = await _repository.AuthenticateAsync(model.Username, model.Password);
                if (authenticateResponse == null)
                {
                    return new JsonResult(null);
                }

                authenticateResponse.Token = JwtUtils.GenerateJwtToken(authenticateResponse);
                return Ok(authenticateResponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync()
        {
            try
            {
                var response = await _repository.GetAllAsync();

                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByUserIdAsync(int id)
        {
            try
            {
                var response = await _repository.GetUserByUserIdAsync(id);

                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(UpdateUserRequest model)
        {
            try
            {
                var errorMessge = await _repository.UpdateAsync(model);
                return Ok(new { Result = errorMessge });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut("forgotpassword/{email}")]
        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            try
            {
                var error = await _repository.ForgotPasswordAsync(email);
                return Ok(new { Result = error });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
