using Api_Pessoas.Domain;

namespace Api_Pessoas.Services
{
    public interface IPessoaService
    {
        IEnumerable<Pessoa> GetAll();
        Pessoa? GetById(int id);
        IEnumerable<Pessoa> GetByUf(string uf);
        Pessoa Add(Pessoa pessoa);
        Pessoa Update(int id, Pessoa pessoa);
        bool DeleteById(int id);
    }
}
