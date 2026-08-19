using Api_Pessoas.Domain;
using Api_Pessoas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_Pessoas.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaService _pessoaService;

        public PessoaController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var pessoas = _pessoaService.GetAll();

            return Ok(pessoas);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var pessoa = _pessoaService.GetById(id);

            if (pessoa == null)
                return NotFound();

            return Ok(pessoa);
        }

        [HttpGet("uf/{uf}")]
        public IActionResult GetByUf(string uf)
        {
            var pessoas = _pessoaService.GetByUf(uf);           

            return Ok(pessoas);
        }

        [HttpPost]
        public IActionResult Add(Pessoa pessoa)
        {
            try
            {
                var novaPessoa = _pessoaService.Add(pessoa);
                return CreatedAtAction(nameof(GetById), new { id = novaPessoa.Id }, novaPessoa);
            }
            catch (ArgumentException ex)
            {               
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception)
            {               
                return StatusCode(500, new { mensagem = "Ocorreu um erro interno no servidor." });
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Pessoa pessoa)
        {
            try
            {
                var pessoaAtualizada = _pessoaService.Update(id, pessoa);

                if (pessoaAtualizada == null)
                    return NotFound(new
                    {
                        mensagem = "Pessoa não encontrada."
                    });

                return Ok(pessoaAtualizada);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    mensagem = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensagem = "Ocorreu um erro interno no servidor."
                });
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var removida = _pessoaService.DeleteById(id);

            if (!removida)
                return NotFound();

            return NoContent();
        }
    }
}