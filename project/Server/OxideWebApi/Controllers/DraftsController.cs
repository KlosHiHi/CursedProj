using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OxideClassLibrary.DTOs;
using OxideClassLibrary.Models;
using OxideWebApi.Contexts;

[Route("api/[controller]")]
[ApiController]
public class DraftsController : ControllerBase
{
    private readonly OxideDbContext _context;
    public DraftsController(OxideDbContext context)
    {
        _context = context;
    }

    [HttpGet("Active/{id}")]
    public async Task<ActionResult<IEnumerable<DraftDto>>> GetActiveDraft(Guid id)
    {
        var user = await _context.Users
            .Include(u => u.Drafts)
            .FirstOrDefaultAsync(un => un.UserId == id);

        if (user is null)
            return NotFound();

        var userDrafts = user.Drafts
            .Where(d => !d.IsArhived && !d.IsDeleted)
            .Select(d => new DraftDto
            {
                DraftId = d.DraftId,
                Title = d.Title,
                CategoryId = d.CategoryId,
                CreationTime = d.CreationTime,
                LastChangeTime = d.LastChangeTime,
            })
            .ToList();

        return userDrafts;
    }


    [HttpGet("Archive/{id}")]
    public async Task<ActionResult<IEnumerable<DraftDto>>> GetArchiveDraft(Guid id)
    {
        var user = await _context.Users
            .Include(u => u.Drafts)
            .FirstOrDefaultAsync(un => un.UserId == id);

        if (user is null)
            return NotFound();

        var userDrafts = user.Drafts
            .Where(d => d.IsArhived)
            .Select(d => new DraftDto
            {
                DraftId = d.DraftId,
                Title = d.Title,
                CategoryId = d.CategoryId,
                CreationTime = d.CreationTime,
                LastChangeTime = d.LastChangeTime,
            })
            .ToList();

        return userDrafts;
    }

    [HttpGet("Trash/{id}")]
    public async Task<ActionResult<IEnumerable<DraftDto>>> GetTrashDraft(Guid id)
    {
        var user = await _context.Users
            .Include(u => u.Drafts)
            .FirstOrDefaultAsync(un => un.UserId == id);

        if (user is null)
            return NotFound();

        var userDrafts = user.Drafts
            .Where(d => d.IsDeleted)
            .Select(d => new DraftDto
            {
                DraftId = d.DraftId,
                Title = d.Title,
                CategoryId = d.CategoryId,
                CreationTime = d.CreationTime,
                LastChangeTime = d.LastChangeTime,
            })
            .ToList();

        return userDrafts;
    }

    [HttpGet("{draftid}")]
    public async Task<ActionResult<Draft>> GetDraft(Guid draftid)
    {
        var draft = await _context.Drafts.FindAsync(draftid);

        return draft is null ? NotFound() : draft;
    }

    [HttpPut("{draftid}")]
    public async Task<IActionResult> PutDraft(Guid? draftid, Draft draft)
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

    [HttpPost]
    public async Task<ActionResult<Draft>> PostDraft(Draft draft)
    {
        _context.Drafts.Add(draft);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDraft", new { draftid = draft.DraftId }, draft);
    }

    [HttpDelete("{draftid}")]
    public async Task<IActionResult> DeleteDraft(Guid? draftid)
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

    private bool DraftExists(Guid? draftid)
    {
        return _context.Drafts.Any(e => e.DraftId == draftid);
    }
}
