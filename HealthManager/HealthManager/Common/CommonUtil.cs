namespace HealthManager.Common
{
    class CommonUtil
    {
        private static string _decimalStringFormtRule="n2";

        public static string GetDecimalFormatString(double value)
        {
            return value.ToString(_decimalStringFormtRule);
        }
        public static string GetDecimalFormatString(float value)
        {
            return value.ToString(_decimalStringFormtRule);
        }
    }
}
