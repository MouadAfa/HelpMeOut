using HelpMeOut.Models;
using HelpMeOut.Repository.DTOs;
using HelpMeOut.Services;
using HelpMeOut.WebApi.Requests;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

namespace HelpMeOut.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUsersService _usersService;
    private readonly IAddressesService _addressesService;
    private readonly IAccountsService _accountsService;
    private readonly ISkillsService _skillsService;

    public UserController(
        ILogger<UserController> logger,
        IUsersService usersService,
        IAddressesService addressesService,
        IAccountsService accountsService,
        ISkillsService skillsService)
    {
        _logger = logger;
        _usersService = usersService;
        _addressesService = addressesService;
        _accountsService = accountsService;
        _skillsService = skillsService;
    }

    [HttpPost("Login")]
    public IActionResult Login(LoginRequest request)
    {
        _logger.LogInformation($"Login in for Email : {request.Email}");

        if (!ModelState.IsValid)
        {
            var msg = $"Invalid data received: {ModelState}";
            _logger.LogWarning(msg, ModelState);
            return ValidationProblem(ModelState);
        }

        var either = from user in _usersService.Login(request.Email, request.Password)
                     from address in _addressesService.GetAddressById(user.Id)
                     select ToAUser(user, address);
        
        Func<AUser, IActionResult> right = (AUser u) => CreatedAtAction(nameof(Login), new { id = u.Email }, u);
        Func<Error, IActionResult> left = (Error e) => BadRequest(e.Exception);

        return either.Match(right, left);
    }

    [HttpPost("Register")]
    public IActionResult Register(RegisterRequest request)
    {
        _logger.LogInformation($"Registration...");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid data received: {ModelState}", ModelState);
            return ValidationProblem(ModelState);
        }

        var addressDto = new AddressDto
        {
            StreetNumber = request.Address.StreetNumber,
            StreetName = request.Address.StreetName,
            City = request.Address.City,
            State = request.Address.State,
            ZipCode = request.Address.ZipCode,
            Country = request.Address.Country
        };

        var either = from address in _addressesService.Create(addressDto)
                     let userDto = new UserDto
                     {
                         FirstName = request.FirstName,
                         LastName = request.LastName,
                         Email = request.Email,
                         AddressId = address.Id,
                         Birthday = request.Birthday,
                         IsHelper = request.IsHelper
                     }
                     from user in _usersService.CreateUser(userDto)
                     let accDto = new AccountDto
                     {
                         Email = request.Email,
                         Password = request.Password,
                         Active = false
                     }
                     from acc in _accountsService.Create(accDto)
                     select ToAUser(user, addressDto);

        Func<AUser, IActionResult> right = (AUser u) => CreatedAtAction(nameof(Register), new { id = u.Email }, u);
        Func<Error, IActionResult> left = (Error e) => BadRequest(e.Exception);
        
        return either.Match(right, left);
    }
    
    [HttpPost("Activate")]
    public IActionResult Activate(ActivateRequest request)
    {
        _logger.LogInformation($"Activation {request.Email}...");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid data received: {ModelState}", ModelState);
            return ValidationProblem(ModelState);
        }

        var either = from account in _accountsService.GetAccountByEmail(request.Email)
            let accDto = new AccountDto
            {
                Id = account.Id,
                Email = request.Email,
                Password = account.Password,
                Active = true
            }
            from acc in _accountsService.Update(accDto)
            select acc;

        Func<AccountDto, IActionResult> right = (AccountDto a) => CreatedAtAction(nameof(Activate), new { id = a.Email }, a);
        Func<Error, IActionResult> left = (Error e) => BadRequest(e.Exception);
        
        return either.Match(right, left);
    }

    #region Private methods

    private AUser ToAUser(UserDto userDto, AddressDto address)
    {
        if (userDto.IsHelper)
        {
            var skillDtos = _skillsService.GetSkillsByUserId(userDto.Id);
            return new Helper
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Address = ToAddress(address),
                Birthday = userDto.Birthday,
                Skills = ToSkill(skillDtos)
            };
        }

        return new Seeker
        {
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Address = ToAddress(address),
            Birthday = userDto.Birthday
        };
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

    private List<Skill> ToSkill(params SkillDto[] skillDtos)
    {
        return skillDtos.Select(s => new Skill
        {
            SkillName = s.SkillName,
            YearsOfExperience = s.YearsOfExperience,
            Certified = s.Certified
        }).ToList();
    }
    
    #endregion
}