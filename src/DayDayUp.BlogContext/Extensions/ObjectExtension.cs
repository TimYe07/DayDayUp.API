using System;
using System.Linq;

namespace DayDayUp.BlogContext.Extensions
{
    public static class ObjectExtension
    {
        public static bool TryGetOwnPropertyName(Type type, string name, out string propertyName)
        {
            var props = type.GetProperties();
            var propertyNames = props.Select(p => p.Name);

            foreach (var item in propertyNames)
            {
                if (!item.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                propertyName = item;
                return true;
            }

            propertyName = string.Empty;
            return false;
        }
    }
}