using System;

namespace Unity.DeliveryDriver.Editor.Tags
{
    public static class EnumHelper
    {
        public static string GetEnumAsString<T>(this T enumValue) where T: IComparable, IFormattable, IConvertible
        {
            VerifyTypeIsEnum<T>();
            return enumValue.ToString();
        }
        
        public static void VerifyTypeIsEnum<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            } 
        }
    }
}
