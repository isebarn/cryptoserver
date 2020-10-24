using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Microsoft.AspNetCore.Authorization;
using Helpers;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly Context _context;

        public GameController(Context context)
        {
            _context = context;
        }

        // GET: api/Game
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
            return await _context.Games.Include(x => x.User).ToListAsync();
        }

        // GET: api/Game/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var Game = await _context.Games.Include(x => x.User).FirstOrDefaultAsync(item => item.Id == id);

            if (Game == null)
            {
                return NotFound();
            }

            return Game;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult PostGame(Game model)
        {
            try
            {
                // create game
                _context.Games.Add(model);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Game/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game Game)
        {
            if (id != Game.Id)
            {
                return BadRequest();
            }

            _context.Entry(Game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Game/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(int id)
        {
            var Game = await _context.Games.Include(x => x.User).FirstOrDefaultAsync(item => item.Id == id);
            if (Game == null)
            {
                return NotFound();
            }

            _context.Users.Remove(Game.User);
            _context.Games.Remove(Game);
            await _context.SaveChangesAsync();

            return Game;
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
