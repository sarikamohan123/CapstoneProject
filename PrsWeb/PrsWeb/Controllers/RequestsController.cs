using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using PrsWeb.Models;
using System.Linq;
using System.Text;

namespace PrsWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly PrsdbContext _context;

        public RequestsController(PrsdbContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }
        // PUT: api/Requests/review/{id}
        [HttpPut("review/{id}")]
        public async Task<ActionResult<Request>> ReviewRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound($" Request with ID {id} not found");
            }
            //Update the status to "REVIEW"
            request.Status = "REVIEW";
            //Save the changes to the database
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            // Return a success response
            return NoContent();
        }



        // PUT: api/Requests/approve/{id}
        [HttpPut("approve/{id}")]
        public async Task<ActionResult<Request>> ApproveRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound($" Request with ID {id} not found");
            }
            //Update the status to "Approved"
            request.Status = "APPROVED";
            //Save the changes to the database
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            // Return a success response
            return NoContent();
        }

        // PUT: api/Requests/reject/{id}
        [HttpPut("reject/{id}")]
        public async Task<ActionResult<Request>> RejectRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound($" Request with ID {id} not found");
            }
            //Update the status to "Rejected"
            request.Status = "REJECTED";
            request.ReasonForRejection = "Not on the approved list";
            //Save the changes to the database
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            // Return a success response
            return NoContent();
        }





        // GET: api/Requests/list-review/id
        [HttpGet("list-review/{userId}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsForReview(int userId)
        {
            //Check if the user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if(!userExists)
            {
                return NotFound($"User with ID {userId} not found");
            }

          // Fetch requests with Status = "REVIEW" and UserId != id as path variable
            var requests = await _context.Requests
                .Where(r => r.Status == "REVIEW" && r.UserId != userId)
                .ToListAsync();
            if (requests == null)
            {
                return NotFound();
            }
            return Ok(requests);
        }

        // GET: api/Requests/approved 
        [HttpGet("approved")]
        public async Task<ActionResult<IEnumerable<Request>>> GetApprovedRequests()
        {
            // Get the signed-in user's ID
            var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");

            // Fetch requests with Status = "REVIEW" and UserId != currentUserId
            var requests = await _context.Requests
                .Where(r => r.Status == "APPROVED" && r.UserId != currentUserId)
                .ToListAsync();

            return requests;
        }

        // GET: api/Requests/rejected
        [HttpGet("rejected")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRejectedRequests()
        {
            // Get the signed-in user's ID
            var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");

            // Fetch requests with Status = "REVIEW" and UserId != currentUserId
            var requests = await _context.Requests
                .Where(r => r.Status == "REJECTED")
                .ToListAsync();

            return requests;
        }


        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            request.Status = "NEW";
            request.SubmittedDate = DateTime.Now;
            request.Total = 0.0m;
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        //  PUT: api/Requests/submit-review/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("submit-review/{id}")]
        public async Task<IActionResult> SubmitForReview(int id)
        {
            //Fetch the request from the datbase using the id
            var request = await _context.Requests.FindAsync(id);
            // if the request is not found
            if (id != request.Id)
            {
                return BadRequest();
            }
            //Set the intial status and submitted date
            request.Status = "NEW";
            request.SubmittedDate = DateTime.Now;
        //Set the status based on value
            if (request.Total <= 50)
            {
                request.Status = "APPROVED";
            }
            else
            {
                request.Status = "REVIEW";
            }

            //Mark the entity as Modified
            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(request);
        }


        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(RequestForm requestForm)
        {
            Request request =new Request();

           //retrieve the maximum request number from the datbase
            string maxReqNbr = _context.Requests.Max(r =>r.RequestNumber);
            request.UserId =requestForm.UserId;
             request.RequestNumber = incrementRequestNumber(maxReqNbr); 
            request.Description = requestForm.Description;
            request.Justification = requestForm.Justification;
            request.DateNeeded = requestForm.DateNeeded;
            request.DeliveryMode = requestForm.DeliveryMode;
             request.Status = "NEW";
            request.SubmittedDate = DateTime.Now;
            request.Total = 0.0m;
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }
        //Method for generating incremental request number
        private string incrementRequestNumber(string maxReqNbr)
        {
            StringBuilder nextReqNbr = new StringBuilder("");
            //Extract the numeric part from the 7th character onward
            int nbr = Int32.Parse(maxReqNbr.Substring(7));
            // increment the numeric part
            nbr++;
            //Create the next request numberby combining the prefix and the new incremented number
            nextReqNbr.Append(maxReqNbr.Substring(0, 1));
            string dateStr = String.Format("{0:yyMMdd}", DateTime.Now);

            string nbrStr = nbr.ToString();
            nbrStr = nbrStr.PadLeft(4, '0');
            nextReqNbr.Append(dateStr).Append(nbrStr);
            return nextReqNbr.ToString();
        }




        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }

        
    }
