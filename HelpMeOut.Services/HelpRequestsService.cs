using HelpMeOut.Repository.DTOs;
using HelpMeOut.Repository.Repositories;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace HelpMeOut.Services
{
    public interface IHelpRequestsService
    {
        Either<Error, HelpRequestDto> Create(HelpRequestDto request);
    }

    public class HelpRequestsService : IHelpRequestsService
    {
        private readonly ILogger<HelpRequestsService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IHelpRequestRepository _helpRequestRepository;

        public HelpRequestsService(
            ILogger<HelpRequestsService> logger,
            IUserRepository userRepository,
            IHelpRequestRepository helpRequestRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _helpRequestRepository = helpRequestRepository;
        }

        public Either<Error, HelpRequestDto> Create(HelpRequestDto request)
        {
            var users = _userRepository.GetAll().ToList();
            var user = users.SingleOrDefault(u => u.Id == request.UserId);
            if (user == null)
            {
                var msg = $"User could not be found.";
                _logger.LogError(msg);
                return Either<Error, HelpRequestDto>.Left(new Error
                    { Message = msg, Exception = new Exception(msg) });
            }

            _helpRequestRepository.Create(request);

            var helpRequestDto = _helpRequestRepository.GetAll().FirstOrDefault(hr => hr.UserId == request.UserId);

            if (helpRequestDto == null)
            {
                var msg = $"Help Request {request.Title} could not be added.";
                _logger.LogError(msg);
                return Either<Error, HelpRequestDto>.Left(new Error
                    { Message = msg, Exception = new Exception(msg) });
            }

            return Either<Error, HelpRequestDto>.Right(helpRequestDto);
        }
    }
}