using Moq;
using ProducaoMicroservice.Adapters.Controllers.Request;
using ProducaoMicroservice.Adapters.Controllers.Response;
using ProducaoMicroservice.Core.Ports;
using ProducaoMicroservice.Core.UseCases;
using TechTalk.SpecFlow;
using Assert = Xunit.Assert;
using ProducaoMicroservice.Core.Entities;


[Binding]
public class ProducaoSteps
{
    private readonly Mock<IProducaoRepository> _producaoRepositoryMock;
    private readonly Mock<ISqsService> _sqsServiceMock;
    private readonly ProducaoUseCase _producaoUseCase;
    private ProducaoUpdateRequest _producaoUpdateRequest;
    private ProducaoUpdateResponse _producaoUpdateResponse;

    public ProducaoSteps()
    {
        _producaoRepositoryMock = new Mock<IProducaoRepository>();
        _sqsServiceMock = new Mock<ISqsService>();
        _producaoUseCase = new ProducaoUseCase(_producaoRepositoryMock.Object, _sqsServiceMock.Object);
    }

    [Given(@"uma producao válida com os seguintes dados:")]
    public void DadoUmaProducaoValidaComOsSeguintesDados(Table table)
    {
        var itens = new List<ProducaoUpdateRequest>();

        _producaoUpdateRequest = new ProducaoUpdateRequest
        {
            Cliente = "Cliente Teste",
            Id = 1,
            Status = "EmPreparacao"
        };
    }

    [When(@"eu enviar a solicitação de criação do producao")]
    public async Task QuandoEuEnviarASolicitacaoDeCriacaoDaProducao()
    {
        var producaoCriada = new Producao
        {
            Id = 1,
            Cliente = _producaoUpdateRequest.Cliente,
            PedidoId = _producaoUpdateRequest.Id
        };

        _producaoRepositoryMock.Setup(r => r.AdicionarProducao(It.IsAny<Producao>())).ReturnsAsync(producaoCriada);
        await _producaoUseCase.AtualizarStatusPedidoAsync(_producaoUpdateRequest.Id, _producaoUpdateRequest.Cliente, _producaoUpdateRequest.Status);

        _producaoUpdateResponse = new ProducaoUpdateResponse
        {
            Id = 1,
            Cliente = producaoCriada.Cliente,
            Status = _producaoUpdateResponse.Status
        };
    }

    [Then(@"a producao deve ser criada com sucesso")]
    public void EntaoAProducaoDeveSerCriadaComSucesso()
    {
        Assert.NotNull(_producaoUpdateResponse);
        Assert.Equal(_producaoUpdateRequest.Cliente, _producaoUpdateResponse.Cliente);
    }

    [Then(@"o status do pedido deve ser ""(.*)""")]
    public void EntaoOStatusDoPedidoDeveSer(string status)
    {
        Assert.Equal(status, _producaoUpdateResponse.Status);
    }
}
