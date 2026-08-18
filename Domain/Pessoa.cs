namespace Api_Pessoas.Domain
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public DateTime? Nascimento { get; set; }
    }
}
