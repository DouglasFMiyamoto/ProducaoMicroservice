namespace ProducaoMicroservice.Adapters.Controllers.Request
{
    public class ProducaoUpdateRequest
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
