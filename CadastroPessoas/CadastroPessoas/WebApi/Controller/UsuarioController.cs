using Aplicacao;
using Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controller
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioServico _usuarioServico;

        public UsuarioController(UsuarioServico usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        //Get: /api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> ListarUsuariosAsync()
        {
            try
            {
                var usuarios = await _usuarioServico.ListarUsuariosAsync();
                return Ok(usuarios);
            }
            catch
            {
                return StatusCode(500, new { mensagem = "Erro ao listar usuários." });
            }
        }

        //Get: /api/usuario/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ListarUsuarioPorIdAsync(int id)
        {
            var usuario = await _usuarioServico.ListarUsuariosPorIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensagem = $"Usuário com ID {id} não encontrado." });

            return Ok(usuario);
        }

        //POST: /api/usuarios
        [HttpPost]
        public async Task<IActionResult> CadastrarUsuarioAsync([FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest(new { mensagem = "Dados inválidos." });
            }

            try
            {
                var resultado = await _usuarioServico.CadastrarUsuarioAsync(new Usuario()
                {
                    Nome = usuario.Nome,
                    CPF = usuario.CPF,
                    Email = usuario.Email,
                    Senha = usuario.Senha,
                    Matricula = usuario.Matricula,
                    Ativo = usuario.Ativo
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao cadastrar o usuário.", detalhe = ex.Message });
            }

            return Ok("Usuário cadastrado.");

        }

        //PUT: /api/usuario/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuarioAsync(int id, [FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest(new { mensagem = "Dados inválidos." });
            }

            try
            {
                usuario.Id = id;

                await _usuarioServico.AtualizarUsuarioAsync(id, usuario);

                return Ok("Usuário atualizado.");
            }

            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao atualizar o usuário.", detalhe = ex.Message });
            }

        }

        //Detele: /api/usuario/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarUsuarioAsync(int id)
        {
            try
            {
                var usuario = await _usuarioServico.ListarUsuariosPorIdAsync(id);

                await _usuarioServico.DeletarUsuarioAsync(id);
                return Ok("Usuário deletado.");
            }

            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao deletar o usuário.", detalhe = ex.Message });
            }
        }
    }
}