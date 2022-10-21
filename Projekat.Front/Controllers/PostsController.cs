using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekat.Front.Dtos;
using Projekat.Front.Infrastructure.Persistence;

namespace Projekat.Front.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly StackOverflow2010Context _context;
        private readonly IMapper _mapper;

        public PostsController(StackOverflow2010Context context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestPostsAsync(int numberOfPosts = 10)
        {
            var posts = await _context
                .Posts
                .Where(p => p.Title != null)
                .OrderByDescending(p => p.CreationDate)
                .Take(numberOfPosts)
                .ToListAsync();

            var result = _mapper.Map<PostReadDto>(posts);
            return Ok(result);
        }
    }
}
