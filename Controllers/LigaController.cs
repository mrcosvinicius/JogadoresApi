using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
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
        public IEnumerable<ReadLigaDto> ObterLigas([FromQuery] int pula = 0, [FromQuery] int pega = 5)
        {
            var ligas = _context.Ligas
                .Include(l => l.Times)
                .Skip(pula)
                .Take(pega)
                .ToList();

            return _mapper.Map<ICollection<ReadLigaDto>>(ligas);
        }

        [HttpGet("{id}")]
        public IActionResult ObterLigaPorId(int id)
        {
            var liga = _context.Ligas
                .Include(l => l.Times)
                .FirstOrDefault(liga => liga.Id == id);
            if (liga == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ReadLigaDto>(liga));
        }

        [HttpPost]
        public IActionResult AdicionarLiga([FromBody] CriarLigaDto ligaDto)
        {
            var liga = _mapper.Map<Liga>(ligaDto);
            _context.Ligas.Add(liga);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterLigaPorId), new { id = liga.Id }, _mapper.Map<ReadLigaDto>(liga));
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
