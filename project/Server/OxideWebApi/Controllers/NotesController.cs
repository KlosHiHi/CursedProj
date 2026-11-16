using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OxideClassLibrary.Models;
using OxideClassLibrary.Contexts;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly OxideDbContext _context;
    public NotesController(OxideDbContext context)
    {
        _context = context;
    }

    // GET: api/Note
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Note>>> GetNote()
    {
        return await _context.Notes.ToListAsync();
    }

    // GET: api/Note/5
    [HttpGet("{noteid}")]
    public async Task<ActionResult<Note>> GetNote(System.Guid noteid)
    {
        var note = await _context.Notes.FindAsync(noteid);

        if (note == null)
        {
            return NotFound();
        }

        return note;
    }

    // PUT: api/Note/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
