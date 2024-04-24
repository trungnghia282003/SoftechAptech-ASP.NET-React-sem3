using Microsoft.AspNetCore.Mvc;
using OA.Api.Interfaces;
using OA.Api.Requests;
using OA.Api.Securities;

namespace OA.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class BidController : BaseController
    {
        private readonly IBidRepository _repository;

        public BidController(IBidRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(1);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateBidRequest model)
        {
            try
            {
                var result = await _repository.Create(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetAllBid(int itemId)
        {

            var response = await _repository.GetAllBid(itemId);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
