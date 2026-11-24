using Microsoft.EntityFrameworkCore;
using OxideClassLibrary.DTOs;
using OxideClassLibrary.Models;
using OxideWebApi.Contexts;
using System.Diagnostics;

namespace OxideWebApi.Services
{
    public interface INotesService
    {
        Task<bool?> ArchiveNoteAsync(Guid? noteid);
        Task<bool?> DeleteNoteAsync(Guid? noteid);
        Task<IEnumerable<NoteDto>> GetActiveNotesAsync(Guid id);
        Task<IEnumerable<NoteDto>> GetArchiveNotesAsync(Guid id);
        Task<Note?> GetNoteAsync(Guid noteid);
        Task<IEnumerable<NoteDto>> GetTrashNotesAsync(Guid id);
        Task<bool> IsNoteExists(Guid? noteid);
        Task<Note?> PostNoteAsync(Note note);
        Task<bool?> PutNoteAsync(Guid? noteid, Note note);
        Task<bool?> ReturnNoteAsync(Guid? noteid);
        Task<bool?> TrashNoteAsync(Guid? noteid);
    }

    public class NotesService(OxideDbContext context) : INotesService
    {
        private readonly OxideDbContext _context = context;

        public async Task<IEnumerable<NoteDto>> GetActiveNotesAsync(Guid id)
        {
            var activeNotes = await _context.UserNotes
                .Include(un => un.Note)
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

            return activeNotes;
        }

        public async Task<IEnumerable<NoteDto>> GetArchiveNotesAsync(Guid id)
        {
            var archiveNotes = await _context.UserNotes
                .Include(un => un.Note)
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

            return archiveNotes;
        }

        public async Task<IEnumerable<NoteDto>> GetTrashNotesAsync(Guid id)
        {
            var trashNotes = await _context.UserNotes
                .Include(un => un.Note)
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

            return trashNotes;
        }

        public async Task<bool?> PutNoteAsync(Guid? noteid, Note note)
        {
            if (noteid != note.NoteId)
                return null;

            _context.Entry(note).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsNoteExists(noteid))
                    return false;
                else
                    throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public async Task<Note?> PostNoteAsync(Note note)
        {
            _context.Notes.Add(note);

            try
            {
                await _context.SaveChangesAsync();
                return await GetNoteAsync(note.NoteId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool?> ArchiveNoteAsync(Guid? noteid)
        {
            var note = await _context.Notes.FindAsync(noteid);

            if (note is null)
                return null;

            note.IsCommented = false;
            note.IsArhived = true;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool?> TrashNoteAsync(Guid? noteid)
        {
            var note = await _context.Notes.FindAsync(noteid);

            if (note is null)
                return null;

            note.IsCommented = false;
            note.IsArhived = false;
            note.IsDeleted = true;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool?> ReturnNoteAsync(Guid? noteid)
        {
            var note = await _context.Notes.FindAsync(noteid);

            if (note is null)
                return null;

            note.IsCommented = false;
            note.IsDeleted = false;
            note.IsArhived = false;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool?> DeleteNoteAsync(Guid? noteid)
        {
            var note = await _context.Notes.FindAsync(noteid);

            if (note is null)
                return null;

            _context.Notes.Remove(note);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Note?> GetNoteAsync(Guid noteid)
            => await _context.Notes.FindAsync(noteid);

        public async Task<bool> IsNoteExists(Guid? noteid)
            => await _context.Notes.AnyAsync(e => e.NoteId == noteid);
    }
}
