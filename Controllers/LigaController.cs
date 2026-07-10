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
    public class LigaController : ControllerBase
    {
        private readonly JogadorContext _context;
        private readonly IMapper _mapper;

        public LigaController(JogadorContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<Liga> ObterLigas([FromQuery] int pula = 0, [FromQuery] int pega = 5)
        {
            return _context.Ligas.Skip(pula).Take(pega);
        }

        [HttpGet("{id}")]
        public IActionResult ObterLigaPorId(int id)
        {
            var liga = _context.Ligas.FirstOrDefault(liga => liga.Id == id);
            if (liga == null)
            {
                return NotFound();
            }
            return Ok(liga);
        }

        [HttpPost]
        public IActionResult AdicionarLiga([FromBody] CriarLigaDto ligaDto)
        {
            var liga = _mapper.Map<Liga>(ligaDto);
            _context.Ligas.Add(liga);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterLigaPorId), new { id = liga.Id }, liga);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarLiga(int id, [FromBody] AtualizarLigaDto ligaDto)
        {
            var ligaExistente = _context.Ligas.FirstOrDefault(liga => liga.Id == id);
            if (ligaExistente == null)
            {
                return NotFound();
            }
            _mapper.Map(ligaDto, ligaExistente);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarLigaParcialmente(int id, [FromBody] JsonPatchDocument<Liga> patch)
        {
            var ligaExistente = _context.Ligas.FirstOrDefault(liga => liga.Id == id);
            if (ligaExistente == null)
            {
                return NotFound();
            }

            patch.ApplyTo(ligaExistente);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarLiga(int id)
        {
            var liga = _context.Ligas.FirstOrDefault(liga => liga.Id == id);
            if (liga == null)
            {
                return NotFound();
            }
            _context.Ligas.Remove(liga);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
