using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OxideClassLibrary.Models;
using OxideWebApi.Contexts;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly OxideDbContext _context;
    public CommentsController(OxideDbContext context)
    {
        _context = context;
    }

    // GET: api/Comment
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Comment>>> GetComment()
    {
        return await _context.Comments.ToListAsync();
    }

    // GET: api/Comment/5
    [HttpGet("{commentid}")]
    public async Task<ActionResult<Comment>> GetComment(System.Guid commentid)
    {
        var comment = await _context.Comments.FindAsync(commentid);

        if (comment == null)
        {
            return NotFound();
        }

        return comment;
    }

    // PUT: api/Comment/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{commentid}")]
    public async Task<IActionResult> PutComment(System.Guid? commentid, Comment comment)
    {
        if (commentid != comment.CommentId)
        {
            return BadRequest();
        }

        _context.Entry(comment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CommentExists(commentid))
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

    // POST: api/Comment
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Comment>> PostComment(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetComment", new { commentid = comment.CommentId }, comment);
    }

    // DELETE: api/Comment/5
    [HttpDelete("{commentid}")]
    public async Task<IActionResult> DeleteComment(System.Guid? commentid)
    {
        var comment = await _context.Comments.FindAsync(commentid);
        if (comment == null)
        {
            return NotFound();
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CommentExists(System.Guid? commentid)
    {
        return _context.Comments.Any(e => e.CommentId == commentid);
    }
}
