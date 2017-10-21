using HealthManager.DependencyInterface;
using Xamarin.Forms;

namespace HealthManager.Common
{
    public static class DbConst
    {
        public static readonly string DBPath = DependencyService.Get<ISqliteDeviceInform>().GetDbPath();
    }
}
