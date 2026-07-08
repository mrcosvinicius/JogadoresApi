using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using JogadoresApi.Model;
using JogadoresApi.Data;
using JogadoresApi.Dtos;


namespace JogadoresApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JogadorController : ControllerBase
    {
        private readonly JogadorContext _context;
        private readonly IMapper _mapper;

        public JogadorController(JogadorContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<Jogador> ObterJogadores([FromQuery] int pula = 0, [FromQuery] int pega = 5)
        {
            return _context.Jogadores.Skip(pula).Take(pega);
        }

        [HttpGet("{id}")]
        public IActionResult ObterJogadorPorId(int id)
        {
            var jogador = _context.Jogadores.FirstOrDefault(jogador => jogador.Id == id);
            if (jogador == null)
            {
                return NotFound();
            }
            return Ok(jogador);
        }

        [HttpPost]
        public IActionResult AdicionarJogador([FromBody] CriarJogadorDto jogadorDto)
        {
            var jogador = _mapper.Map<Jogador>(jogadorDto);
            _context.Jogadores.Add(jogador);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterJogadorPorId), new { id = jogador.Id }, jogador);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarJogador(int id, [FromBody] AtualizarJogadorDto jogadorDto)
        {
            var jogadorExistente = _context.Jogadores.FirstOrDefault(jogador => jogador.Id == id);
            if (jogadorExistente == null)
            {
                return NotFound();
            }
            _mapper.Map(jogadorDto, jogadorExistente);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarJogadorParcialmente(int id, [FromBody] JsonPatchDocument<Jogador> patch)
        {
            var jogadorExistente = _context.Jogadores.FirstOrDefault(jogador => jogador.Id == id);
            if (jogadorExistente == null)
            {
                return NotFound();
            }

            patch.ApplyTo(jogadorExistente);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarJogador(int id)
        {
            var jogador = _context.Jogadores.FirstOrDefault(jogador => jogador.Id == id);
            if (jogador == null)
            {
                return NotFound();
            }
            _context.Jogadores.Remove(jogador);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
