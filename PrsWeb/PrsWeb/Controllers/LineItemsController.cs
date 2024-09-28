using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsWeb.Models;

namespace PrsWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineItemsController : ControllerBase
    {
        private readonly PrsdbContext _context;

        public LineItemsController(PrsdbContext context)
        {
            _context = context;
        }

        // GET: api/LineItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LineItem>>> GetLineItems()
        {
            return await _context.LineItems.ToListAsync();
        }

        // GET: api/LineItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LineItem>> GetLineItem(int id)
        {
            var lineItem = await _context.LineItems.FindAsync(id);

            if (lineItem == null)
            {
                return NotFound();
            }

            return lineItem;
        }

        //Get lineitems by request id method
        [HttpGet("lines-for-req/{requestId}")]
        public async Task<ActionResult<IEnumerable<LineItem>>>
        GetLineItemsForRequestId(int requestId)
        {
            var lineitems = await _context.LineItems.Include(l => l.Request)
                    .Include(l => l.Product)
                   .Where(l => l.RequestId == requestId)
                   .ToListAsync();
            return lineitems;
        }


        // PUT: api/LineItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLineItem(int id, LineItem lineItem)
        {
            if (id != lineItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(lineItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await RecalculateRequestTotal(lineItem.RequestId);


            return NoContent();
        }

        // POST: api/LineItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LineItem>> PostLineItem(LineItem lineItem)
        {
            //Add the new line item
            _context.LineItems.Add(lineItem);
            await _context.SaveChangesAsync();
            //Recalculate the total for the assosiated request
            await RecalculateRequestTotal(lineItem.RequestId);

            return CreatedAtAction("GetLineItem", new { id = lineItem.Id }, lineItem);
        }

        // DELETE: api/LineItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLineItem(int id)
        {
            var lineItem = await _context.LineItems.FindAsync(id);
            if (lineItem == null)
            {
                return NotFound();
            }
            //Remove the line item
            _context.LineItems.Remove(lineItem);
            await _context.SaveChangesAsync();
            await RecalculateRequestTotal(lineItem.RequestId);

            return NoContent();
        }

        //Recalculate the total cost
        private async Task RecalculateRequestTotal(int requestId)
        {
            //Get the request and include its line items
            var request = await _context.Requests
                 .Include(r => r.LineItems)
                 .ThenInclude(li => li.Product)
                 .FirstOrDefaultAsync(r => r.Id == requestId);
            if(request == null)
            {
                return;
            }
            //Calculate the total from line items
            request.Total = request.LineItems.Sum(li => li.Quantity * li.Product.Price);
            // Save the changes
            await _context.SaveChangesAsync();
        }

        private bool LineItemExists(int id)
        {
            return _context.LineItems.Any(e => e.Id == id);
        }
    }
}
