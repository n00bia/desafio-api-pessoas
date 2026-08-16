using Api_Pessoas.Domain;

namespace Api_Pessoas.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly List<Pessoa> _pessoas = new()
        {
            new Pessoa
            {
                Id = 1,
                Nome = "João Silva",
                Cpf = "12345678900",
                Uf = "GO",
                Nascimento = new DateTime(1990, 5, 10)
            },
            new Pessoa
            {
                Id = 2,
                Nome = "Maria Souza",
                Cpf = "98765432100",
                Uf = "MA",
                Nascimento = new DateTime(1995, 8, 20)
            }
        };

        private int _proximoId = 3;


        public IEnumerable<Pessoa> GetAll()
        {
            return _pessoas;
        }

        public Pessoa? GetById(int id)
        {
            return _pessoas.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Pessoa> GetByUf(string uf)
        {
            return _pessoas.Where(p => p.Uf == uf);
        }

        public Pessoa Add(Pessoa pessoa)
        {
            if (string.IsNullOrWhiteSpace(pessoa.Nome))
                throw new ArgumentException("O nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(pessoa.Cpf))
                throw new ArgumentException("O CPF é obrigatório.");

            if (string.IsNullOrWhiteSpace(pessoa.Uf))
                throw new ArgumentException("A UF é obrigatória.");

            if (!pessoa.Nascimento.HasValue)
                throw new ArgumentException("A data de nascimento é obrigatória.");

            if (!ValidarCpf(pessoa.Cpf))
            {
                throw new ArgumentException("O CPF informado é inválido.");
            }
            pessoa.Id = _proximoId++;

            if (string.IsNullOrWhiteSpace(pessoa.Uf) || pessoa.Uf.Length != 2)
                throw new ArgumentException("A UF deve possuir 2 caracteres.");

            pessoa.Uf = pessoa.Uf.Trim().ToUpperInvariant();

            _pessoas.Add(pessoa);

            return pessoa;
        }

        public Pessoa? Update(int id, Pessoa pessoa)
        {
            var pessoaExistente = _pessoas.FirstOrDefault(p => p.Id == id);

            if (pessoaExistente == null)
                return null;


            pessoaExistente.Nome = pessoa.Nome;            
            pessoaExistente.Uf = pessoa.Uf;
            pessoaExistente.Nascimento = pessoa.Nascimento;

            if (string.IsNullOrWhiteSpace(pessoa.Nome))
                throw new ArgumentException("O nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(pessoa.Cpf))
                throw new ArgumentException("O CPF é obrigatório.");

            if (string.IsNullOrWhiteSpace(pessoa.Uf))
                throw new ArgumentException("A UF é obrigatória.");

            if (!pessoa.Nascimento.HasValue)
                throw new ArgumentException("A data de nascimento é obrigatória.");

            return pessoaExistente;
        }

        public bool DeleteById(int id)
        {
            var pessoa = _pessoas.FirstOrDefault(p => p.Id == id);

            if (pessoa == null)
                return false;

            _pessoas.Remove(pessoa);

            return true;
        }

        private bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;
            
            string cpfLimpo = new string(cpf.Where(char.IsDigit).ToArray());
            
            if (cpfLimpo.Length != 11) return false;
            
            if (cpfLimpo.Distinct().Count() == 1) return false;
            
            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += (cpfLimpo[i] - '0') * multiplicadores1[i];
            }
            int resto = soma % 11;
            int primeiroDigito = resto < 2 ? 0 : 11 - resto;
            
            if ((cpfLimpo[9] - '0') != primeiroDigito) return false;
            
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += (cpfLimpo[i] - '0') * multiplicadores2[i];
            }
            resto = soma % 11;
            int segundoDigito = resto < 2 ? 0 : 11 - resto;
            
            return (cpfLimpo[10] - '0') == segundoDigito;
        }
    }   
}

