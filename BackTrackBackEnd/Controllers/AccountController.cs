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
        public ActionResult<Account> Register(Account account)
        {
            // Add password constraints check here

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