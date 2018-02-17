using HealthManager.Common.Constant;

namespace HealthManager.Common
{
    public static class StringUtils
    {

        public static string Empty = StringConst.Empty;

        public static bool IsEmpty(string text)
        {
            return text == null || Empty.Equals(text);
        }
    }
}
