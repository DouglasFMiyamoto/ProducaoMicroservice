using ProducaoMicroservice.Adapters.Messaging.Sqs.DTO;
using ProducaoMicroservice.Core.Entities;
using ProducaoMicroservice.Core.Ports;

namespace ProducaoMicroservice.Core.UseCases
{
    public class ProducaoUseCase
    {
        private readonly IProducaoRepository _producaoRepository;
        private readonly ISqsService _sqsService;

        public ProducaoUseCase(IProducaoRepository producaoRepository, ISqsService sqsService)
        {
            _producaoRepository = producaoRepository;
            _sqsService = sqsService;
        }

        public async Task StartListeningAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(30000);
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _sqsService.ReceiveMessageAsync(cancellationToken);

                    foreach (var message in response.Messages)
                    {
                        var pedidoMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<PedidoPagoMessage>(message.Body);
                        await ProcessarProducaoAsync(pedidoMessage.Id, pedidoMessage.Cliente.ToString(), pedidoMessage.Pago);
                        await _sqsService.ApagarMensagemAsync(message.ReceiptHandle);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao consumir mensagens da fila: {ex.Message}");
                    await Task.Delay(5000);
                }
            }
        }

        public async Task ProcessarProducaoAsync(int pedidoId, string cliente, bool pago)
        {
            var producao = new Producao
            {
                PedidoId = pedidoId,
                Cliente = cliente,
                Data = DateTime.UtcNow
            };

            await _producaoRepository.AdicionarProducao(producao);

            var mensagem = $"{{ \"id\": {producao.PedidoId}, \"cliente\": {producao.Cliente.ToString()}, \"status\": EmPreparacao }}";
            await _sqsService.EnviarMensagemPedidoAtualizadoAsync(mensagem);
        }

        public async Task AtualizarStatusPedidoAsync(int pedidoId, string cliente, string status)
        {
            var mensagem = $"{{ \"id\": {pedidoId}, \"cliente\": {cliente.ToString()}, \"status\": {status} }}";
            await _sqsService.EnviarMensagemPedidoAtualizadoAsync(mensagem);
        }
    }
}
