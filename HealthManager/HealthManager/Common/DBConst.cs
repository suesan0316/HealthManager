using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthManager.DependencyInterface;
using Xamarin.Forms;

namespace HealthManager.Common
{
    public static class DbConst
    {
        public static readonly string DBPath = DependencyService.Get<ISqliteDeviceInform>().GetDbPath();
    }
}
