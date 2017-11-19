namespace HealthManager.Common
{
    /// <summary>
    /// 共通処理クラス(ViewModeCommonUtilに移行する)
    /// </summary>
    internal class CommonUtil
    {
        /// <summary>小数箇所の規定桁数</summary>
        public static readonly string DecimalStringFormtRule="n2";

        /// <summary>
        /// 小数箇所を規定桁数に変換した文字列を返却
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDecimalFormatString(double value)
        {
            return value.ToString(DecimalStringFormtRule);
        }

        /// <summary>
        /// 小数箇所を規定桁数に変換した文字列を返却
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDecimalFormatString(float value)
        {
            return value.ToString(DecimalStringFormtRule);
        }
    }
}
