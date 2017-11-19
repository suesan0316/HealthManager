namespace HealthManager.DependencyInterface
{
    /// <summary>
    /// SqliteDBのファイルパスを取得するための依存インターフェース
    /// </summary>
    public interface ISqliteDeviceInform
    {
        /// <summary>
        /// データベースのパスを取得
        /// </summary>
        /// <returns></returns>
        string GetDbPath();
    }
}
