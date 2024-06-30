using ApiWebGeradorPessoa.Models;
using System.Collections.Concurrent;

namespace ApiWebGeradorPessoa.Services
{
    public class GerarPessoas
    {
        public bool _formate = false;
        public int _quantidadeDeNomes = 0;
        public string _selecionadaselecionada = "";
        public Cidade? _cidade;

        public CidadeService _cidadeService = new();
        public NomeSobrenome _nomeSobrenome = new();
        public HashSet<string> nomesCompletosSelecionados = new();
        public ConcurrentBag<Pessoa> listaPessoas = new();
        private static Random random = new();

        private static readonly List<string> nomesRuas = Enumerable.Range('A', 26).Select(i => $"Rua {(char)i}").ToList();
        private static readonly List<string> sufixosEmail = Enumerable.Range('A', 26).Select(i => $"@teste{(char)i}.com").ToList();

        public void SetQuantidadeDeNomes(int quantidadeDeNomes, bool formate, string selecionadaselecionada)
        {
            this._formate = formate;
            this._quantidadeDeNomes = quantidadeDeNomes;
            this._selecionadaselecionada = selecionadaselecionada;
        }

        public HashSet<Pessoa> GetJson()
        {
            return new HashSet<Pessoa>(this.listaPessoas);
        }

        public void Execute()
        {
            MontarJson();
        }

        private void MontarJson()
        {

            if (this._selecionadaselecionada != null || this._selecionadaselecionada != "")
            {
                this._cidade = _cidadeService.listaCidades.Find(x => x.Estado == this._selecionadaselecionada);
            }

            for (int i = 0; i < this._quantidadeDeNomes; i++)
            {
                if (this._cidade == null)
                {
                    this._cidade = GerarCidade();
                }

                int indice = random.Next(_nomeSobrenome.nomes.Count);
                string nome = _nomeSobrenome.nomes[indice];

                Pessoa pessoa = new Pessoa
                {
                    Id = Guid.NewGuid().ToString(),
                    Nome = nome,
                    Email = removerAcentos(nome) + GerarEmail(),
                    Cpf = GerarCpf(),
                    Rua = GerarRua(),
                    Numero = GerarNumero(),
                    Cidade = this._cidade.Nome,
                    Estado = this._cidade.Estado,
                    Celular = GerarNumeroCelular(this._cidade.DDD),
                    Cep = GerarCep(this._cidade.FaixaCepInicio, this._cidade.FaixaCepFim)
                };
                this.listaPessoas.Add(pessoa);

                // remove o nomes da lista para não ter mais repetição
                _nomeSobrenome.nomes.Remove(nome);
            }
        }

        private Cidade GerarCidade()
        {
            List<Cidade> cidade = _cidadeService.listaCidades;
            return cidade[random.Next(cidade.Count)];
        }

        static string GerarNumero()
        {
            return random.Next(1, 1000).ToString();
        }

        static string GerarCep(int faixaInicial, int faixaInFinal)
        {
            return random.Next(faixaInicial, faixaInFinal + 1).ToString();
        }

        public static string GerarNumeroCelular(string ddd)
        {
            // Gerar 9 números aleatórios para o número do celular
            string numero = new string(Enumerable.Range(0, 9).Select(_ => random.Next(0, 10).ToString()[0]).ToArray());

            // Formatar e retornar o número de celular
            return "55" + ddd + numero;
        }

        static string GerarEmail()
        {
            return sufixosEmail[random.Next(sufixosEmail.Count)];
        }

        static string GerarRua()
        {
            return nomesRuas[random.Next(nomesRuas.Count)];
        }

        private string GerarCpf()
        {
            int[] cpf = new int[11];

            // Gerar os primeiros 9 dígitos
            for (int i = 0; i < 9; i++)
            {
                cpf[i] = random.Next(0, 10);
            }

            // Calcular os dígitos verificadores
            cpf[9] = CalcularDigitoVerificador(cpf, 9);
            cpf[10] = CalcularDigitoVerificador(cpf, 10);

            if (_formate)
            {
                return string.Format("{0}{1}{2}.{3}{4}{5}.{6}{7}{8}-{9}{10}",
                cpf[0], cpf[1], cpf[2], cpf[3], cpf[4], cpf[5], cpf[6], cpf[7], cpf[8], cpf[9], cpf[10]);
            }
            else
            {
                return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
               cpf[0], cpf[1], cpf[2], cpf[3], cpf[4], cpf[5], cpf[6], cpf[7], cpf[8], cpf[9], cpf[10]);
            }

        }

        private int CalcularDigitoVerificador(int[] cpf, int posicao)
        {
            int soma = 0;
            int peso = posicao == 9 ? 10 : 11;

            for (int i = 0; i < posicao; i++)
            {
                soma += cpf[i] * peso--;
            }

            int digito = 11 - (soma % 11);
            return digito >= 10 ? 0 : digito;
        }

        private string removerAcentos(string texto)
        {
            Dictionary<char, char> mapAcentos = new Dictionary<char, char>
            {
                {'Ä', 'A'}, {'Å', 'A'}, {'Á', 'A'}, {'Â', 'A'}, {'À', 'A'}, {'Ã', 'A'},
                {'ä', 'a'}, {'á', 'a'}, {'â', 'a'}, {'à', 'a'}, {'ã', 'a'},
                {'É', 'E'}, {'Ê', 'E'}, {'Ë', 'E'}, {'È', 'E'},
                {'é', 'e'}, {'ê', 'e'}, {'ë', 'e'}, {'è', 'e'},
                {'Í', 'I'}, {'Î', 'I'}, {'Ï', 'I'}, {'Ì', 'I'},
                {'í', 'i'}, {'î', 'i'}, {'ï', 'i'}, {'ì', 'i'},
                {'Ö', 'O'}, {'Ó', 'O'}, {'Ô', 'O'}, {'Ò', 'O'}, {'Õ', 'O'},
                {'ö', 'o'}, {'ó', 'o'}, {'ô', 'o'}, {'ò', 'o'}, {'õ', 'o'},
                {'Ü', 'U'}, {'Ú', 'U'}, {'Û', 'U'}, {'ü', 'u'}, {'ú', 'u'}, {'û', 'u'}, {'ù', 'u'},
                {'Ç', 'C'}, {'ç', 'c'}
            };

            char[] array = texto.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (mapAcentos.ContainsKey(array[i]))
                {
                    array[i] = mapAcentos[array[i]];
                }
            }

            // Remover espaços
            string resultado = new string(array);
            resultado = resultado.Replace(" ", "").ToLower();

            return resultado;
        }
    }
}
