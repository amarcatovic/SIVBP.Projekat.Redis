using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekat.Front.Infrastructure.Persistence;

namespace Projekat.Front.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly StackOverflow2010Context _context;

        public PostsController(StackOverflow2010Context context)
        {
            _context = context;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestPostsAsync(int numberOfPosts = 10)
        {
            return Ok(await _context
                .Posts
                .Where(p => p.Title != null)
                .OrderByDescending(p => p.CreationDate)
                .Take(numberOfPosts)
                .ToListAsync());
        }
    }
}
