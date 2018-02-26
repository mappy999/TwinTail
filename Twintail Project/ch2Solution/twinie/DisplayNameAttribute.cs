using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Twin
{

	public class EnumDisplayNameConverter : EnumConverter
	{
		public EnumDisplayNameConverter(Type enumType)
			: base(enumType)
		{
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;

			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string valueString = value.ToString();

				foreach (FieldInfo field in base.EnumType.GetFields())
				{
					DescriptionAttribute attr = GetDescriptionAttribute(field);

					if (attr != null && attr.Description == valueString)
					{
						return field.GetValue(null);
					}
				}
			}

			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context,
			CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				DescriptionAttribute attr = GetDescriptionAttribute(value.GetType());
				
				if (attr != null)
				{
					return attr.Description;
				}
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		protected virtual DescriptionAttribute GetDescriptionAttribute(MemberInfo member)
		{
			object[] attributes = member.GetCustomAttributes(typeof(DescriptionAttribute), true);

			if (attributes.Length == 0)
				return null;

			return attributes[0] as DescriptionAttribute;
		}

	}
}
