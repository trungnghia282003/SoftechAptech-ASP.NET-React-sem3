namespace OA.Api.Responses
{
    public class CategoryResponse
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }

        public List<ItemResponse>? ItemResponses { get; set; }
    }
}
