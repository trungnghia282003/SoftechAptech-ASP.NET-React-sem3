using Microsoft.AspNetCore.Mvc;
using OA.Api.Interfaces;
using OA.Api.Requests;

namespace OA.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : BaseController
    {
        private readonly IRatingRepository _repository;

        public RatingController(IRatingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(RatingRequest model)
        {
            try
            {
                var result = await _repository.UpdateAsync(model, CurrentUser.Role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllRatingAsync();
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
