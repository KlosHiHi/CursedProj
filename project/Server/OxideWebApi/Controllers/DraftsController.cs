using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OxideClassLibrary.Models;
using OxideClassLibrary.Contexts;

[Route("api/[controller]")]
[ApiController]
public class DraftsController : ControllerBase
{
    private readonly OxideDbContext _context;
    public DraftsController(OxideDbContext context)
    {
        _context = context;
    }

    // GET: api/Draft
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Draft>>> GetDraft()
    {
        return await _context.Drafts.ToListAsync();
    }

    // GET: api/Draft/5
    [HttpGet("{draftid}")]
    public async Task<ActionResult<Draft>> GetDraft(System.Guid draftid)
    {
        var draft = await _context.Drafts.FindAsync(draftid);

        if (draft == null)
        {
            return NotFound();
        }

        return draft;
    }

    // PUT: api/Draft/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{draftid}")]
    public async Task<IActionResult> PutDraft(System.Guid? draftid, Draft draft)
    {
        if (draftid != draft.DraftId)
        {
            return BadRequest();
        }

        _context.Entry(draft).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DraftExists(draftid))
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

    // POST: api/Draft
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Draft>> PostDraft(Draft draft)
    {
        _context.Drafts.Add(draft);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDraft", new { draftid = draft.DraftId }, draft);
    }

    // DELETE: api/Draft/5
    [HttpDelete("{draftid}")]
    public async Task<IActionResult> DeleteDraft(System.Guid? draftid)
    {
        var draft = await _context.Drafts.FindAsync(draftid);
        if (draft == null)
        {
            return NotFound();
        }

        _context.Drafts.Remove(draft);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DraftExists(System.Guid? draftid)
    {
        return _context.Drafts.Any(e => e.DraftId == draftid);
    }
}
