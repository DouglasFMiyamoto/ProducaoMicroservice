using System.ComponentModel.DataAnnotations;

namespace ProducaoMicroservice.Core.Entities
{
    public class Producao
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int PedidoId { get; set; }
        [Required]
        public string Cliente { get; set; } = string.Empty;
        [Required]
        public DateTime Data { get; set; }
    }
}
