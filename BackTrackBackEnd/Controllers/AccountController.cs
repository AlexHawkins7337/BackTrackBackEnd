using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BackTrackBackEnd.SQLTables;
using Microsoft.EntityFrameworkCore;

namespace BackTrackBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Account/Register
        [HttpPost("Register")]
        public IActionResult Register(Account account)
        {
            // Check for existing username
            var existingUserWithUsername = _context.Accounts
                .Any(a => a.Username == account.Username);
            if (existingUserWithUsername)
            {
                return BadRequest("This username is already taken. Please choose a different one.");
            }

            // Check for existing email
            var existingUserWithEmail = _context.Accounts
                .Any(a => a.Email == account.Email);
            if (existingUserWithEmail)
            {
                return BadRequest("This email is already in use. Please choose a different one.");
            }

            // Add password constraints check here (ensure you hash the password before saving)

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        // POST: api/Account/Login
        [HttpGet("Login")]
        public ActionResult<Guid> Login(string username, string password)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Username == username && a.Password == password);

            if (account == null)
            {
                return NotFound();
            }

            return account.Id;
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(Guid id)
        {
            var account = _context.Accounts.Find(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }
    }
}