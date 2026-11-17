using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OxideClassLibrary.Contexts;
using OxideClassLibrary.DTOs;
using OxideClassLibrary.Models;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly OxideDbContext _context;
    public NotesController(OxideDbContext context)
    {
        _context = context;
    }

    [HttpGet("Active/{id}")]
    //[Authorize]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<NoteDto>>> GetActiveNotes(Guid id)
    {
        var activeNotes = _context.UserNotes
            .Where(un => un.UserId == id
                && !un.Note.IsArhived
                && !un.Note.IsDeleted)
            .Select(un => new NoteDto
            {
                NoteId = un.Note.NoteId,
                Title = un.Note.Title,
                Content = un.Note.Content,
                Role = un.Role,
                CategoryId = un.Note.CategoryId,
                IsCommented = un.Note.IsCommented,
                CreationTime = un.Note.CreationTime,
                LastChangeTime = un.Note.LastChangeTime,
            })
            .ToListAsync();

        return await activeNotes;
    }

    [HttpGet("Archive/{id}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<NoteDto>>> GetArchiveNotes(Guid id)
    {
        var archiveNotes = _context.UserNotes
            .Where(un => un.UserId == id
                && un.Note.IsArhived)
            .Select(un => new NoteDto
            {
                NoteId = un.Note.NoteId,
                Title = un.Note.Title,
                Content = un.Note.Content,
                Role = un.Role,
                CategoryId = un.Note.CategoryId,
                IsCommented = un.Note.IsCommented,
                CreationTime = un.Note.CreationTime,
                LastChangeTime = un.Note.LastChangeTime,
            })
            .ToListAsync();

        return await archiveNotes;
    }

    [HttpGet("Trash/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<NoteDto>>> GetTrashNotes(Guid id)
    {
        var trashNotes = _context.UserNotes
            .Where(un => un.UserId == id
                && un.Note.IsDeleted)
            .Select(un => new NoteDto
            {
                NoteId = un.Note.NoteId,
                Title = un.Note.Title,
                Content = un.Note.Content,
                Role = un.Role,
                CategoryId = un.Note.CategoryId,
                IsCommented = un.Note.IsCommented,
                CreationTime = un.Note.CreationTime,
                LastChangeTime = un.Note.LastChangeTime,
            })
            .ToListAsync();

        return await trashNotes;
    }

    [HttpGet("{noteid}")]
    public async Task<ActionResult<Note>> GetNote(System.Guid noteid)
    {
        var note = await _context.Notes.FindAsync(noteid);

        return note is null ? NotFound() : note;
    }

    // PUT: api/Note/5
    [HttpPut("{noteid}")]
    public async Task<IActionResult> PutNote(System.Guid? noteid, Note note)
    {
        if (noteid != note.NoteId)
        {
            return BadRequest();
        }

        _context.Entry(note).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NoteExists(noteid))
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

    // POST: api/Note
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Note>> PostNote(Note note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetNote", new { noteid = note.NoteId }, note);
    }

    // DELETE: api/Note/5
    [HttpDelete("{noteid}")]
    public async Task<IActionResult> DeleteNote(System.Guid? noteid)
    {
        var note = await _context.Notes.FindAsync(noteid);
        if (note == null)
        {
            return NotFound();
        }

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool NoteExists(System.Guid? noteid)
    {
        return _context.Notes.Any(e => e.NoteId == noteid);
    }
}
