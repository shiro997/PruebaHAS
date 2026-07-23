using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserESDataLayer;

namespace UserESBusinessLayer
{
    public class UserEsImpl
    {
        private IConfiguration _configuration;
        private ILogger _logger;
        private IUserESData _userData;

        public UserEsImpl(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
            _userData = new UserESData(_configuration, _logger);
        }



    }
}
