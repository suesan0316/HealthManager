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
        public static readonly string DbPath = DependencyService.Get<ISqliteDeviceInform>().GetDbPath();
        
        /// <summary>データベース更新処理成功</summary>
        public static readonly int Success = 0;

        /// <summary>データベース更新処理失敗</summary>
        public static readonly int Failed = 1;
    }
}
