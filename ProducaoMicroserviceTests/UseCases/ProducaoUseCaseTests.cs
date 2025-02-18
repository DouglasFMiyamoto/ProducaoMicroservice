using Moq;
using ProducaoMicroservice.Adapters.Controllers.Request;
using ProducaoMicroservice.Adapters.Controllers.Response;
using ProducaoMicroservice.Core.Entities;
using ProducaoMicroservice.Core.Ports;
using ProducaoMicroservice.Core.UseCases;
using Xunit;
using Assert = Xunit.Assert;

public class ProducaoUseCaseTests
{
    private readonly Mock<IProducaoRepository> _producaoRepositoryMock;
    private readonly Mock<ISqsService> _sqsServiceMock;
    private readonly ProducaoUseCase _producaoUseCase;

    public ProducaoUseCaseTests()
    {
        _producaoRepositoryMock = new Mock<IProducaoRepository>();
        _sqsServiceMock = new Mock<ISqsService>();
        _producaoUseCase = new ProducaoUseCase(_producaoRepositoryMock.Object, _sqsServiceMock.Object);
    }

    [Fact]
    public async Task CriarProducao_DeveAdicionarProducaoERetornarResponse()
    {
        // Arrange
        var producaoRequest = new ProducaoUpdateRequest
        {
            Id = 1,
            Cliente = "João",
            Status = "EmPreparacao"
        };

        var producaoCriada = new Producao
        {
            Id = 1,
            Cliente = "João",
            Data = DateTime.Now,
            PedidoId = 1
        };

        _producaoRepositoryMock.Setup(r => r.AdicionarProducao(It.IsAny<Producao>())).ReturnsAsync(producaoCriada);

        // Act
        await _producaoUseCase.AtualizarStatusPedidoAsync(producaoRequest.Id, producaoRequest.Cliente, producaoRequest.Status);
        var result = new ProducaoUpdateResponse {
            Id = 1,
            Cliente = "João",
            Status = "EmPreparacao"
        };

        // Assert
        Assert.NotNull(result);
        Assert.Equal(producaoCriada.Cliente, result.Cliente);
        Assert.Equal(producaoCriada.PedidoId, result.Id);
        _producaoRepositoryMock.Verify(r => r.AdicionarProducao(It.IsAny<Producao>()), Times.Once);
        _sqsServiceMock.Verify(s => s.EnviarMensagemPedidoAtualizadoAsync(It.IsAny<string>()), Times.Once);
    }
}

