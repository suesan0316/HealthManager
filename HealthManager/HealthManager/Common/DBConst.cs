using HealthManager.DependencyInterface;
using Xamarin.Forms;

namespace HealthManager.Common
{
    /// <summary>
    /// DB関連定数クラス
    /// </summary>
    public static class DbConst
    {
        /// <summary>データベース接続パス</summary>
        public static readonly string DBPath = DependencyService.Get<ISqliteDeviceInform>().GetDbPath();
    }
}
