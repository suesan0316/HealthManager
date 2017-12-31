using HealthManager.Common.Enum;

namespace HealthManager.Common.Extention
{
    /// <summary>
    /// 基本データ列挙の各拡張ラス
    /// </summary>
    public static class BasicDataEnumExt
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

        public static string DisplayString(this BasicDataEnum basicData)
        {
            string[] names = {"名前", "性別", "年齢", "身長", "体重", "体脂肪率", "上の血圧", "下の血圧", "基礎代謝"};
            return names[(int) basicData];
        }
    }
}
