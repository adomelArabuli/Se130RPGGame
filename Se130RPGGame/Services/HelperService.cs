using Se130RPGGame.Interfaces;
using System.Security.Claims;

namespace Se130RPGGame.Services
{
    public class HelperService : IHelperService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HelperService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public int GetUserId()
        {
            return int.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
