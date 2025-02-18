using ProducaoMicroservice.Core.UseCases;
using FastEndpoints;
using ProducaoMicroservice.Adapters.Controllers.Request;
using ProducaoMicroservice.Adapters.Controllers.Response;

namespace ProducaoMicroservice.Adapters.Controllers
{
    public class ProducaoUpdateEndpoint : Endpoint<ProducaoUpdateRequest, ProducaoUpdateResponse>
    {
        private readonly ProducaoUseCase _producaoUseCase;

        public ProducaoUpdateEndpoint(ProducaoUseCase producaoUseCase)
        {
            _producaoUseCase = producaoUseCase;
        }

        public override void Configure()
        {
            Put("/producao");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ProducaoUpdateRequest req, CancellationToken ct)
        {
            await _producaoUseCase.AtualizarStatusPedidoAsync(req.Id, req.Cliente, req.Status);

            var resposta = new ProducaoUpdateResponse { 
                Id = req.Id,
                Cliente = req.Cliente,
                Status = req.Status
            };

            await SendAsync(resposta);
        }
    }
}
