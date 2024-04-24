using Microsoft.AspNetCore.Mvc;
using OA.Api.Interfaces;
using OA.Api.Securities;

namespace OA.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class NotificationController : BaseController
    {
        private readonly INotificationRepository _repository;

        public NotificationController(INotificationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllAsync(int userId)
        {
            var response = await _repository.GetAllNotificationsByIdUserAsync(userId);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync()
        {
            try
            {
                var result = await _repository.UpdateMaskReadAsync(CurrentUser.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
