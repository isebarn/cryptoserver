using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Helpers;
using Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailUserController : ControllerBase
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IEmailUserService _emailUserService;

        public EmailUserController(Context context, IMapper mapper, IUserService userService, IEmailUserService emailUserService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _emailUserService = emailUserService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateModel model)
        {
            var user = _emailUserService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            string tokenString = _userService.SetToken(user);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Token = tokenString
            });
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterModel model)
        {
            // map model to entity
            var emailUser = _mapper.Map<EmailUser>(model);

            try
            {
                // create user
                _emailUserService.Create(emailUser, model.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        // GET: api/EmailUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmailUser>>> GetEmailUser()
        {
            return await _context.EmailUsers.Include(x => x.User).ToListAsync();
        }

        // GET: api/EmailUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmailUser>> GetEmailUser(int id)
        {
            var emailUser = await _context.EmailUsers.Include(x => x.User).FirstOrDefaultAsync(item => item.Id == id);

            if (emailUser == null)
            {
                return NotFound();
            }

            return emailUser;
        }

        // PUT: api/EmailUser/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmailUser(int id, EmailUser emailUser)
        {
            if (id != emailUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(emailUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailUserExists(id))
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

        // DELETE: api/EmailUser/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmailUser>> DeleteEmailUser(int id)
        {
            var emailUser = await _context.EmailUsers.Include(x => x.User).FirstOrDefaultAsync(item => item.Id == id);
            if (emailUser == null)
            {
                return NotFound();
            }

            _context.Users.Remove(emailUser.User);
            _context.EmailUsers.Remove(emailUser);
            await _context.SaveChangesAsync();

            return emailUser;
        }

        private bool EmailUserExists(int id)
        {
            return _context.EmailUsers.Any(e => e.Id == id);
        }
    }
}
