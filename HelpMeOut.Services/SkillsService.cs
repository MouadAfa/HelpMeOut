using HelpMeOut.Repository.DTOs;
using HelpMeOut.Repository.Repositories;
using Microsoft.Extensions.Logging;

namespace HelpMeOut.Services;

public interface ISkillsService
{
    void AddSkills(HelperDto helperDto, SkillDto[] skills);
    SkillDto[] GetSkillsByUserId(int helperDtoId);
}

public class SkillsService : ISkillsService
{
    private readonly ILogger<SkillsService> _logger;
    private readonly ISkillRepository _skillRepository;
    private readonly IHelperSkillRepository _helperSkillRepository;
    private readonly IUserRepository _userRepository;

    public SkillsService(
        ILogger<SkillsService> logger,
        ISkillRepository skillRepository,
        IHelperSkillRepository helperSkillRepository,
        IUserRepository userRepository)
    {
        _logger = logger;
        _skillRepository = skillRepository;
        _helperSkillRepository = helperSkillRepository;
        _userRepository = userRepository;
    }

    public void AddSkills(HelperDto helperDto, params SkillDto[] skills)
    {
        var user = _userRepository.GetBy(u => u.Email == helperDto.Email).SingleOrDefault();

        if (user == null)
        {
            return;
        }
        
        foreach (var skill in skills)
        {
            _skillRepository.Create(skill);
            var s = _skillRepository.GetAll().FirstOrDefault(s => s.SkillName == skill.SkillName && s.YearsOfExperience == skill.YearsOfExperience && s.Certified == skill.Certified);
            if (s != null)
            {
                var helperSkillDto = new HelperSkillDto
                {
                    UserId = user.Id,
                    SkillId = s.Id
                };
                _helperSkillRepository.Create(helperSkillDto);
            }
        }
    }

    public SkillDto[] GetSkillsByUserId(int helperDtoId)
    {
        var skillIds = _helperSkillRepository.GetAll()
            .Where(s => s.UserId == helperDtoId)
            .Select(s => s.SkillId)
            .ToArray();

        return skillIds.Select(_skillRepository.Get).ToArray();
    }
}