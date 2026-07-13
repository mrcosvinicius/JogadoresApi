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
    public class TimeController : ControllerBase
    {
        private readonly JogadorContext _context;
        private readonly IMapper _mapper;

        public TimeController(JogadorContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<ReadTimeDto> ObterTimes([FromQuery] int pula = 0, [FromQuery] int pega = 5)
        {
            var times = _context.Times
                .Include(t => t.Elenco)
                .Include(t => t.Ligas)
                .Skip(pula)
                .Take(pega)
                .ToList();

            return _mapper.Map<ICollection<ReadTimeDto>>(times);
        }

        [HttpGet("{id}")]
        public IActionResult ObterTimePorId(int id)
        {
            var time = _context.Times
                .Include(t => t.Elenco)
                .Include(t => t.Ligas)
                .FirstOrDefault(time => time.Id == id);
            if (time == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ReadTimeDto>(time));
        }

        [HttpPost]
        public IActionResult AdicionarTime([FromBody] CriarTimeDto timeDto)
        {
            var time = _mapper.Map<Time>(timeDto);
            AssociarLigas(time, timeDto.LigasIds);
            _context.Times.Add(time);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterTimePorId), new { id = time.Id }, _mapper.Map<ReadTimeDto>(time));
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarTime(int id, [FromBody] AtualizarTimeDto timeDto)
        {
            var timeExistente = _context.Times
                .Include(t => t.Ligas)
                .FirstOrDefault(time => time.Id == id);
            if (timeExistente == null)
            {
                return NotFound();
            }
            _mapper.Map(timeDto, timeExistente);
            timeExistente.Ligas.Clear();
            AssociarLigas(timeExistente, timeDto.LigasIds);
            _context.SaveChanges();
            return NoContent();
        }

        private void AssociarLigas(Time time, List<int>? ligasIds)
        {
            if (ligasIds == null || ligasIds.Count == 0)
            {
                return;
            }
            var ligas = _context.Ligas.Where(liga => ligasIds.Contains(liga.Id)).ToList();
            foreach (var liga in ligas)
            {
                time.Ligas.Add(liga);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarTimeParcialmente(int id, [FromBody] JsonPatchDocument<Time> patch)
        {
            var timeExistente = _context.Times.FirstOrDefault(time => time.Id == id);
            if (timeExistente == null)
            {
                return NotFound();
            }

            patch.ApplyTo(timeExistente);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarTime(int id)
        {
            var time = _context.Times.FirstOrDefault(time => time.Id == id);
            if (time == null)
            {
                return NotFound();
            }
            _context.Times.Remove(time);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
