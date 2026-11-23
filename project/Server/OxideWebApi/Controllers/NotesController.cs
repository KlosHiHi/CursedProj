using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OxideClassLibrary.DTOs;
using OxideClassLibrary.Models;
using OxideWebApi.Contexts;

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
    [Authorize]
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
    [Authorize]
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
    [Authorize]
    public async Task<ActionResult<Note>> GetNote(Guid noteid)
    {
        var note = await _context.Notes.FindAsync(noteid);

        return note is null ? NotFound() : note;
    }

    [HttpPut("{noteid}")]
    [Authorize]
    public async Task<IActionResult> PutNote(Guid? noteid, Note note)
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

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Note>> PostNote(Note note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetNote", new { noteid = note.NoteId }, note);
    }

    [HttpDelete("{noteid}")]
    [Authorize]
    public async Task<IActionResult> DeleteNote(Guid? noteid)
    {
        var note = await _context.Notes.FindAsync(noteid);
        if (note is null)
        {
            return NotFound();
        }

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool NoteExists(Guid? noteid)
    {
        return _context.Notes.Any(e => e.NoteId == noteid);
    }
}
