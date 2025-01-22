using HelpMeOut.Repository.DTOs;
using HelpMeOut.Services;
using HelpMeOut.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HelpMeOut.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelpRequestController : ControllerBase
{
    private readonly ILogger<HelpRequestController> _logger;
    private readonly IHelpRequestsService _helpRequestsService;
    private readonly IUsersService _usersService;

    public HelpRequestController(
        ILogger<HelpRequestController> logger, 
        IHelpRequestsService helpRequestsService,
        IUsersService usersService)
    {
        _logger = logger;
        _helpRequestsService = helpRequestsService;
        _usersService = usersService;
    }

    [HttpPost("SubmitHelpRequest")]
    public IActionResult SubmitHelpRequest(SubmitHelpRequest request)
    {
        _logger.LogInformation($"Submit Help Request {request}");

        var either = from user in _usersService.Get(request.SeekerEmail)
                     let req = new HelpRequestDto
                     {
                         Title = request.Title,
                         Description = request.Description,
                         Type = (int)request.Type,
                         UserId = user.Id
                     }
                     from helpRequest in _helpRequestsService.Create(req)
                     select helpRequest;

        Func<HelpRequestDto, IActionResult> right = (HelpRequestDto r) => CreatedAtAction(nameof(SubmitHelpRequest), new { id = r.Id }, r);
        Func<Error, IActionResult> left = (Error e) => BadRequest(e.Exception);

        return either.Match(right, left);
    }
}