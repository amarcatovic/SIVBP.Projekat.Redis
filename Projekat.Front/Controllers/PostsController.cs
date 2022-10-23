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

        [HttpGet("search")]
        public async Task<IActionResult> SearchPostsAsync([FromQuery] string term)
        {
            var result = await _context
                .Posts
                .Where(p => p.Title != null && EF.Functions.Like(p.Title, $"%{term}%"))
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("search/cache")]
        public async Task<IActionResult> SearchCachedPostsAsync([FromQuery] string term)
        {
            var posts = await _cache.Get<List<PostTitleCache>>(Constants.POSTS_TITLE_ID);
            var result = posts
                .Where(p => p.Title != null && p.Title.Contains(term))
                .ToList();

            return Ok(result);
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

        [HttpGet("cache-names")]
        public async Task<IActionResult> CacheAllNamesAsync(int numberOfPosts = 10)
        {
            var result = await _context
                .Posts
                .Where(p => p.Title != null)
                .Select(p => new PostTitleCache
                {
                    Id = p.Id,
                    Title = p.Title
                })
                .ToListAsync();

            await _cache.Create(result, Constants.POSTS_TITLE_ID);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPostByIdAsync(int id)
        {
            var post = await _context
                .Posts
                .SingleOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound("This post does not exist!");
            }

            var result = _mapper.Map<PostReadDto>(post);
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
