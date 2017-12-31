using HealthManager.Common.Enum;
using HealthManager.Common.Language;

namespace HealthManager.Common.Extention
{
    /// <summary>
    ///     基本データ列挙の各拡張ラス
    /// </summary>
    public static class BasicDataEnumExt
    {
        /// <summary>
        ///     基本データ列挙に対応する単位記号を返却
        /// </summary>
        /// <param name="basicData"></param>
        /// <returns></returns>
        public static string DisplayUnit(this BasicDataEnum basicData)
        {
            string[] names = {"", "", "才", "cm", "Kg", "%", "", "", ""};
            return names[(int) basicData];
        }

        public static string DisplayString(this BasicDataEnum basicData)
        {
            string[] names =
            {
                LanguageUtils.Get(LanguageKeys.Name),
                LanguageUtils.Get(LanguageKeys.Gender),
                LanguageUtils.Get(LanguageKeys.Age),
                LanguageUtils.Get(LanguageKeys.Height),
                LanguageUtils.Get(LanguageKeys.BodyWeight),
                LanguageUtils.Get(LanguageKeys.BodyFatPercentage),
                LanguageUtils.Get(LanguageKeys.MaxBloodPressure),
                LanguageUtils.Get(LanguageKeys.MinBloodPressure),
                LanguageUtils.Get(LanguageKeys.BasicMetabolism)
            };
            return names[(int) basicData];
        }
    }
}