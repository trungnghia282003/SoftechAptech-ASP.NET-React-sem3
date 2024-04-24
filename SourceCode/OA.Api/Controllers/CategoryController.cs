using Microsoft.AspNetCore.Mvc;
using OA.Api.Interfaces;
using OA.Api.Requests;
using OA.Api.Securities;

namespace OA.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepository _repository;

        public CategoryController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("role")]
        public async Task<IActionResult> GetAllAsync(int role)
        {
            var response = await _repository.GetAllAsync(role);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllCategoryActiveAsync()
        {
            var response = await _repository.GetAllCategoryAsync(true);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("{id}/{pageNumber}")]
        public async Task<IActionResult> GetAllItemByCategoryIdAsync(int id, int pageNumber = 1)
        {
            var response = await _repository.GetAllItemByCategoryIdAsync(id, pageNumber);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategoryByRoleAdminAsync(CreateCategoryRequest model)
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CreateCategoryRequest model)
        {
            try
            {
                var result = await _repository.CreateAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
