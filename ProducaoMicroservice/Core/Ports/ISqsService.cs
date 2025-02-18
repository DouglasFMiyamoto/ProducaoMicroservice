using Amazon.SQS.Model;

namespace ProducaoMicroservice.Core.Ports
{
    public interface ISqsService
    {
        Task<ReceiveMessageResponse> ReceiveMessageAsync(CancellationToken cancellationToken);
        Task EnviarMensagemPedidoAtualizadoAsync(string mensagem);
        Task ApagarMensagemAsync(string receiptHandle);
        Task EsperarLocalStackAsync();
    }
}
