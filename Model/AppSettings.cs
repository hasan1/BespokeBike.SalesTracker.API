using Microsoft.Extensions.Configuration;

namespace BespokeBike.SalesTracker.API.Model
{
    public interface IAppSettings
    {
        string BespokeBikeDBconn { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        public AppSettings(string _bespokeBikeDBconn)
        {
            BespokeBikeDBconn = _bespokeBikeDBconn;
        }
        public string BespokeBikeDBconn { get; set; }
    }
}
