using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reflection
{
    class Serializer<T>
    {
		public string Serialize(T obj)
		{
			var stringBuilder = new StringBuilder();
			
			var fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			
			foreach (var field in fields)
			{
				stringBuilder.Append($"{field.Name}:{field.GetValue(obj)};");
			}

			return stringBuilder.ToString();
		}

		public T Deserialize(string csv)
		{
			var type = typeof(T);

			var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			var fieldStr = csv.Split(';',StringSplitOptions.RemoveEmptyEntries);

			if (fields.Length != fieldStr.Length)
                throw new ArgumentOutOfRangeException("Incorrect format to deserialize.");

			var instance = Activator.CreateInstance(type);

			for (int i = 0; i < fields.Length; i++)
			{
				fields[i].SetValue(instance, Convert.ChangeType(fieldStr[i].Split(':')[1], fields[i].FieldType));
			}

			return (T)instance;
		}

	}
}
