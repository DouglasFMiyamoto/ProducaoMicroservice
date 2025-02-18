namespace ProducaoMicroservice.Adapters.Messaging.Sqs.DTO
{
    public class PedidoPagoMessage
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public bool Pago { get; set; }
    }
}
