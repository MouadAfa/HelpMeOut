using HelpMeOut.Models;
using HelpMeOut.Repository.DTOs;
using HelpMeOut.Services;
using HelpMeOut.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HelpMeOut.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class SkillController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ISkillsService _skillsService;
    private readonly IUsersService _usersService;
    private readonly IAddressesService _addressesService;

    public SkillController(
        ILogger<UserController> logger,
        ISkillsService skillsService, 
        IUsersService usersService,
        IAddressesService addressesService)
    {
        _logger = logger;
        _skillsService = skillsService;
        _usersService = usersService;
        _addressesService = addressesService;
    }

    [HttpPost("AddSkills")]
    public IActionResult AddSkills(AddSkillsRequest request)
    {
        _logger.LogInformation($"Adding skills for Email : {request.Email}");

        if (!ModelState.IsValid)
        {
            var msg = $"Invalid data received: {ModelState}";
            _logger.LogWarning(msg, ModelState);
            return ValidationProblem(ModelState);
        }

        var either = from u in _usersService.Get(request.Email)
            where u.IsHelper == true
            let helper = new HelperDto
            {
                Id = u.Id,
                Email = u.Email,
                Birthday = u.Birthday,
                AddressId = u.AddressId,
                FirstName = u.FirstName,
                LastName = u.LastName
            }
            from a in _addressesService.GetAddressById(u.AddressId)
            let skillsDtos = request.Skills.Select(s => new SkillDto
            {
                SkillName = s.SkillName,
                YearsOfExperience = s.YearsOfExperience,
                Certified = s.Certified
            }).ToArray()
            select AddSkillsToUser(helper, a, skillsDtos);

        Func<Helper, IActionResult> right = (Helper h) => CreatedAtAction(nameof(AddSkills), new { id = h.Email }, h);
        Func<Error, IActionResult> left = (Error e) => BadRequest(e.Exception);
        
        return either.Match(right, left);
    }

    private Helper AddSkillsToUser(HelperDto helperDto, AddressDto addressDto, SkillDto[] skillDtos)
    {
        _skillsService.AddSkills(helperDto, skillDtos);
        var allSkills = _skillsService.GetSkillsByUserId(helperDto.Id);
        return new Helper
        {
            FirstName = helperDto.FirstName,
            LastName = helperDto.LastName,
            Birthday = helperDto.Birthday,
            Address = ToAddress(addressDto),
            Email = helperDto.Email,
            Skills = ToSkill(allSkills)
        };
    }

    private List<Skill> ToSkill(params SkillDto[] skillDtos)
    {
        return skillDtos.Select(s => new Skill
        {
            SkillName = s.SkillName,
            YearsOfExperience = s.YearsOfExperience,
            Certified = s.Certified
        }).ToList();
    }
    
    private Address ToAddress(AddressDto address)
    {
        return new Address
        {
            StreetNumber = address.StreetNumber,
            StreetName = address.StreetName,
            City = address.City,
            State = address.State,
            ZipCode = address.ZipCode,
            Country = address.Country
        };
    }
}