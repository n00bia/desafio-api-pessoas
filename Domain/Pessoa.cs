namespace Api_Pessoas.Domain
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Uf { get; set;}
        public DateTime? Nascimento { get; set; }
    }
}
