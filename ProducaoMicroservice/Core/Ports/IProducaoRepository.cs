using ProducaoMicroservice.Core.Entities;

namespace ProducaoMicroservice.Core.Ports
{
    public interface IProducaoRepository
    {
        Task<Producao> AdicionarProducao(Producao producao);
        Task<List<Producao>> ListarProducoes();
        Task<Producao> ObterPorId(int id);
        Task<Producao?> AtualizarProducao(Producao producao);
    }
}
