using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OxideClassLibrary.DTOs;
using OxideClassLibrary.Models;
using OxideWebApi.Services;

[Route("api/[controller]")]
[ApiController]
public class NotesController(INotesService service) : ControllerBase
{
    private readonly INotesService _notesService = service;

    //[Authorize]
    [HttpGet("Active/{id}")]
    public async Task<ActionResult<IEnumerable<NoteDto>>> GetActiveNotes(Guid id)
    {
        var activeNotes = await _notesService.GetActiveNotesAsync(id);

        return Ok(activeNotes);
    }

    //[Authorize]
    [HttpGet("Archive/{id}")]
    public async Task<ActionResult<IEnumerable<NoteDto>>> GetArchiveNotes(Guid id)
    {
        var archiveNotes = await _notesService.GetArchiveNotesAsync(id);

        return Ok(archiveNotes);
    }

    //[Authorize]
    [HttpGet("Trash/{id}")]
    public async Task<ActionResult<IEnumerable<NoteDto>>> GetTrashNotes(Guid id)
    {
        var trashNotes = await _notesService.GetTrashNotesAsync(id);

        return Ok(trashNotes);
    }

    //[Authorize]
    [HttpGet("{noteid}")]
    public async Task<ActionResult<Note>> GetNote(Guid noteid)
    {
        var note = await _notesService.GetNoteAsync(noteid);

        return note is null ? NotFound() : Ok(note);
    }

    //[Authorize]
    [HttpPut("{noteid}")]
    public async Task<IActionResult> PutNote(Guid? noteid, Note note)
    {
        var result = await _notesService.PutNoteAsync(noteid, note);

        if (result is null)
            return NotFound();

        return result.Value ? NoContent() : BadRequest();
    }

    //[Authorize]
    [HttpPost]
    public async Task<ActionResult<Note>> PostNote(Note note)
    {
        await _notesService.PostNoteAsync(note);

        return CreatedAtAction(nameof(_notesService.GetNoteAsync), new { noteid = note.NoteId }, note);
    }

    //[Authorize]
    [HttpDelete("Archive/{noteid}")]
    public async Task<IActionResult> ArchiveNote(Guid? noteid)
    {
        var result = await _notesService.ArchiveNoteAsync(noteid);

        if (result is null)
            return NotFound();

        return result.Value ? NoContent() : BadRequest();
    }

    //[Authorize]
    [HttpDelete("Trash/{noteid}")]
    public async Task<IActionResult> TrashNote(Guid? noteid)
    {
        var result = await _notesService.TrashNoteAsync(noteid);

        if (result is null)
            return NotFound();

        return result.Value ? NoContent() : BadRequest();
    }

    //[Authorize]
    [HttpPost("Return/{noteid}")]
    public async Task<IActionResult> ReturnNote(Guid? noteid)
    {
        var result = await _notesService.ReturnNoteAsync(noteid);

        if (result is null)
            return NotFound();

        return result.Value ? NoContent() : BadRequest();
    }

    //[Authorize]
    [HttpDelete("{noteid}")]
    public async Task<IActionResult> DeleteNote(Guid? noteid)
    {
        var result = await _notesService.DeleteNoteAsync(noteid);

        if (result is null)
            return NotFound();

        return result.Value ? NoContent() : BadRequest();
    }
}
