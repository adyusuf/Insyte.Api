using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _userService.GetAllAsync(search, page, pageSize);
        return Ok(new ApiResponse<PagedResult<UserDto>>(true, result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound(new ApiError(false, "Kullanıcı bulunamadı"));

        return Ok(new ApiResponse<UserDto>(true, user));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var (success, error, user) = await _userService.CreateAsync(request);
        if (!success) return BadRequest(new ApiError(false, error!));

        return CreatedAtAction(nameof(GetById), new { id = user!.Id },
            new ApiResponse<UserDto>(true, user));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        var (success, error, user) = await _userService.UpdateAsync(id, request);
        if (!success)
        {
            if (error == "Kullanıcı bulunamadı") return NotFound(new ApiError(false, error));
            return BadRequest(new ApiError(false, error!));
        }

        return Ok(new ApiResponse<UserDto>(true, user));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (success, error) = await _userService.DeleteAsync(id, GetCurrentUserId());
        if (!success)
        {
            if (error == "Kullanıcı bulunamadı") return NotFound(new ApiError(false, error));
            return BadRequest(new ApiError(false, error!));
        }

        return Ok(new ApiResponse<object>(true, null, "Kullanıcı deaktif edildi"));
    }
}
