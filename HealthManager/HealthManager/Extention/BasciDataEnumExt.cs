using HealthManager.Common;

namespace HealthManager.Extention
{
    static class BasciDataEnumExt
    {
        public static string DisplayUnit(this BasicDataEnum basicData)
        {
            string[] names = { "", "", "才", "cm", "Kg", "%", "", "", "" };
            return names[(int)basicData];
        }
    }
}
