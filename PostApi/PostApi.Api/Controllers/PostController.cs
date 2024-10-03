using Microsoft.AspNetCore.Mvc;
using PostApi.Application.Contracts;
using PostApi.Application.Interfaces;

namespace PostApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPost([FromBody] PostRequest postRequest)
    {
        var postId = await _postService.AddPostAsync(postRequest);
        return Ok(postId);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPost(int id)
    {
        var postResponse = await _postService.GetPostAsync(id);
        return Ok(postResponse);
    }

    [HttpGet]
    public async Task<IActionResult> GetPostsAsync()
    {
        var posts = await _postService.GetPostsAsync();
        return Ok(posts);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdatePostAsync(int id, PostRequest postRequest)
    {
        await _postService.UpdatePostAsync(id, postRequest);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePostAsync(int id)
    {
        await _postService.DeletePostAsync(id);
        return Ok();
    }
}
