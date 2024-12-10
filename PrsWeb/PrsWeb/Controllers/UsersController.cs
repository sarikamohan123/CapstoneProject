using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsWeb.Models;

namespace PrsWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PrsdbContext _context;

        public UsersController(PrsdbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        //Another way greating method for login

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginModel loginModel)
        {
           
            var user = await _context.Users
            .Where(u =>
                       u.Username == loginModel.Username &&
                       u.Password == loginModel.Password)
            .FirstOrDefaultAsync();
            if (user == null)
            {
                 return NotFound();
            }
            return user;
        }



        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        //{
        //    // Fetch the user based on the username
        //    var user = await _context.Users
        //                     .SingleOrDefaultAsync(u => u.Username == loginModel.Username);

        //    // If no user found or password is invalid
        //    if (user == null || !VerifyPassword(loginModel.Password, user.Password))
        //    {
        //        return Unauthorized("Invalid username or password.");
        //    }

        //    // If successful, return OK response
        //    return Ok("Login successful!");
        //}

        private bool VerifyPassword(string storedPassword, string enteredPassword)
        {
            return enteredPassword == storedPassword;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
