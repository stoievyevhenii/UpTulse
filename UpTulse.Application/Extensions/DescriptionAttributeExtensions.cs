using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace UpTulse.Application.Extensions
{
    public static class DescriptionAttributeExtensions
    {
        public static string GetEnumDescription(this Enum e)
        {
            var name = e.ToString();
            var memberInfo = e.GetType().GetMember(name)[0];
            var descriptionAttributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);

            if (descriptionAttributes.Length == 0)
                return name;

            var descriptionAttribute = descriptionAttributes[0] as DescriptionAttribute;

            return descriptionAttribute?.Description ?? string.Empty;
        }
    }
}