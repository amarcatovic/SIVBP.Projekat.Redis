using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Projekat.Front.Dtos;
using Projekat.Front.Infrastructure.Caching;
using Projekat.Front.Infrastructure.Persistence;
using Projekat.Front.Utilities;

namespace Projekat.Front.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly StackOverflow2010Context _context;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public PostsController(StackOverflow2010Context context,
            IMapper mapper,
            ICacheService cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestPostsAsync(int numberOfPosts = 10)
        {
            var result = await GetLatestPostsMappedAsync(numberOfPosts);
            return Ok(result);
        }

        [HttpGet("latest/cache")]
        public async Task<IActionResult> GetLatestPostsFromCacheAsync(int numberOfPosts = 10)
        {
            var result = await _cache.Get<List<PostReadDto>>(Constants.LATEST_POSTS_KEY);

            if (result == null)
            {
                result = await GetLatestPostsMappedAsync(numberOfPosts, true);
            }

            return Ok(result);
        }

        private async Task<List<PostReadDto>> GetLatestPostsMappedAsync(int numberOfPosts, bool cacheResult = false)
        {
            var posts = await _context
                            .Posts
                            .Where(p => p.Title != null)
                            .OrderByDescending(p => p.CreationDate)
                            .Take(numberOfPosts)
                            .ToListAsync();

            var result = _mapper.Map<List<PostReadDto>>(posts);

            if (cacheResult)
            {
                await _cache.Create(result, Constants.LATEST_POSTS_KEY);
            }

            return result;
        }
    }
}
