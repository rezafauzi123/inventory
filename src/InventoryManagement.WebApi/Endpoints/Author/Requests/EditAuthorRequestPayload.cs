namespace InventoryManagement.WebApi.Endpoints.Author.Requests
{
    public class EditAuthorRequestPayload
    {
        public string? Name { get; set; }
        public string? Biography { get; set; }
        public IFormFile Image { get; set; } = null!;
    }
}