using Microsoft.EntityFrameworkCore;
using ProducaoMicroservice.Adapters.Database.PostgreSQL;
using ProducaoMicroservice.Core.Entities;
using ProducaoMicroservice.Core.Ports;

namespace ProducaoMicroservice.Adapters.Persistence
{
    public class ProducaoRepository : IProducaoRepository
    {
        private readonly ProducaoContext _context;

        public ProducaoRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task<Producao> AdicionarProducao(Producao producao)
        {
            _context.Producoes.Add(producao);
            await _context.SaveChangesAsync();  // Aqui o pedido.Id é gerado.

            return producao;
        }

        // Método para listar todos os pedidos
        public async Task<List<Producao>> ListarProducoes()
        {
            return await _context.Producoes.ToListAsync();
        }

        public async Task<Producao> ObterPorId(int id)
        {
            return await _context.Producoes
                            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Producao?> AtualizarProducao(Producao producao)
        {
            _context.Producoes.Update(producao);
            await _context.SaveChangesAsync();

            return producao;
        }
    }
}
