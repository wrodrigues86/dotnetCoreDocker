using ApiWebGeradorPessoa.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebGeradorPessoa.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class PessoasController : ControllerBase
    {
        private readonly CidadeService _cidade = new CidadeService();
        private GerarPessoas _gerarPessoas = new GerarPessoas();

        [HttpGet("GetPessoas")]
        public async Task<ActionResult> GetPessoas(int quantidade, bool formatacao, string cidade)
        {
            try
            {
                if (quantidade > 300)
                {
                    throw new PessoasControllerException("A quantidade não pode ser maior que 300");
                }

                _gerarPessoas.SetQuantidadeDeNomes(quantidade, formatacao, cidade);
                _gerarPessoas.Execute();
                var resultado = _gerarPessoas.GetJson();
                return Ok(resultado);
            }
            catch (PessoasControllerException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Registro de log da exceção aqui, se necessário
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor." });
            }
        }

        public class PessoasControllerException : Exception
        {
            public PessoasControllerException(string message) : base(message)
            {
            }
        }
    }
}
