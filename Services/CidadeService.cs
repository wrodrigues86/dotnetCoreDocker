using ApiWebGeradorPessoa.Models;

namespace ApiWebGeradorPessoa.Services
{
    public class CidadeService
    {
        public List<Cidade> listaCidades = new List<Cidade>();
        public CidadeService()
        {
            this.listaCidades = new List<Cidade>
            {
                new Cidade("São Paula", "SP", "11", 01000000, 19999999),
                new Cidade("Rio de Janeiro", "RJ", "21", 20000000, 28999999),
                new Cidade("Espírito Santo", "ES", "27", 29000000, 29999999),
                new Cidade("Bahia", "BA", "77", 40000000, 48999999),
                new Cidade("Minas Gerais", "MG", "77", 30000000, 39999999),
            };
        }
    }
}
