using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace BGC.WebAPI.Models
{
	public class LocalizationDictionary
	{
		private static readonly string LocalizationSuffix = "Localization";

		private static void SetStaticFieldsRecursively(Type rootType)
		{
			Queue<Type> nestedTypesQueue = new Queue<Type>();
			string localizationRootTypeName = rootType.Name;
			nestedTypesQueue.Enqueue(rootType);

			Regex suffixReplacer = new Regex(string.Format(@"({0}\.)|({0}$)", Regex.Escape(LocalizationSuffix)));
			while (nestedTypesQueue.Count > 0)
			{
				Type currentType = nestedTypesQueue.Dequeue();
				foreach (FieldInfo localizationKeyField in currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Static).Where(fi => fi.FieldType == typeof(string)))
				{
					string pathToDeclaringType = currentType.FullName.Substring(currentType.FullName.IndexOf(localizationRootTypeName)).Replace('+', '.');
					string pathWithoutSuffix = suffixReplacer.Replace(pathToDeclaringType, string.Empty);
					string fieldToPascalCase = char.ToUpperInvariant(localizationKeyField.Name[0]) + localizationKeyField.Name.Substring(1);
					localizationKeyField.SetValue(null, string.Format("{0}.{1}", pathWithoutSuffix, fieldToPascalCase));
				}

				foreach (Type nestedType in currentType.GetNestedTypes())
				{
					nestedTypesQueue.Enqueue(nestedType);
				}
			}
		}

		static LocalizationDictionary()
		{
			Type rootClassType = typeof(LocalizationDictionary);
			SetStaticFieldsRecursively(rootClassType);
		}

		public class AdministrationAreaLocalization
		{
			public class AdministrationLocalization
			{
				public class UsersLocalization
				{
					private static string ok;
					private static string cancel;
				}
			}
		}

		public class GenericTextLocalization
		{

		}
	}
}