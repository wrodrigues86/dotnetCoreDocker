namespace ApiWebGeradorPessoa.Models
{
    public class Cidade
    {
        public string? Nome { get; set; }
        public string? Estado { get; set; }
        public string? DDD { get; set; }
        public int FaixaCepInicio { get; set; }
        public int FaixaCepFim { get; set; }

        public Cidade(
            string Nome, string Estado, string DDD,
            int FaixaCepInicio, int FaixaCepFim)
        {
            this.Nome = Nome;
            this.Estado = Estado;
            this.DDD = DDD;
            this.FaixaCepInicio = FaixaCepInicio;
            this.FaixaCepFim = FaixaCepFim;
        }
    }
}
