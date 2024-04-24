using Microsoft.AspNetCore.Mvc;
using OA.Api.Interfaces;
using OA.Api.Requests;
using OA.Api.Securities;
using OA.Common;


namespace OA.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ItemController : BaseController
    {
        private readonly IItemRepository _repository;

        public ItemController(IItemRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _repository.GetAllAsync();

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdItemAsync(id, CurrentUser.Role, CurrentUser.Id);
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CreateItemRequest model)
        {
            try
            {
                model.AuctionStartDate = model.AuctionStartDate.ToLocalTime();
                model.AuctionEndDate = model.AuctionEndDate.ToLocalTime();
                var result = await _repository.CreateAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(CreateItemRequest model)
        {
            try
            {
                var result = await _repository.UpdateAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchItem(string keyword)
        {
            try
            {
                var result = await _repository.SearchItemAsync(keyword);
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

        [HttpGet("myitems")]
        public async Task<IActionResult> GetItemsByUserId()
        {
            try
            {
                var result = await _repository.GetItemsByUserIdAsync(CurrentUser.Id);
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

        [HttpPost("report")]
        public async Task<IActionResult> ExportReportItems(ReportItemRequest model)
        {
            try
            {
                if (CurrentUser?.Role == (byte)RoleEnums.Seller)
                {
                    model.StartDate = model.StartDate.ToLocalTime();
                    model.EndDate = model.EndDate.ToLocalTime();
                    var result = await _repository.ExportReportItemsAsync(model, CurrentUser.Id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
