using EventCorpModels;
using Microsoft.AspNetCore.Identity;

namespace EventCorp.Services
{
    public class SessionInvalidationService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<SessionInvalidationService> _logger;

        public SessionInvalidationService(UserManager<User> userManager, ILogger<SessionInvalidationService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task InvalidateAllSessionsAsync()
        {
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var securityStampUpdateResult = await _userManager.UpdateSecurityStampAsync(user);
                if (securityStampUpdateResult.Succeeded)
                {
                    _logger.LogInformation($"Sesión invalidada para usuario: {user.Email}");
                }
                else
                {
                    _logger.LogWarning($"Error al invalidar sesión para usuario: {user.Email}");
                }
            }
        }
    }
}
