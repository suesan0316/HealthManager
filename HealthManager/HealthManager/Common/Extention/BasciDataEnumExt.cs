using HealthManager.Common.Enum;

namespace HealthManager.Common.Extention
{
    /// <summary>
    /// 基本データ列挙の各拡張ラス
    /// </summary>
    public static class BasciDataEnumExt
    {
        /// <summary>
        /// 基本データ列挙に対応する単位記号を返却
        /// </summary>
        /// <param name="basicData"></param>
        /// <returns></returns>
        public static string DisplayUnit(this BasicDataEnum basicData)
        {
            string[] names = { "", "", "才", "cm", "Kg", "%", "", "", "" };
            return names[(int)basicData];
        }
    }
}
