/////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////
//		Name of Program/Application: Tasker
//		Copyright (C) 2021  Aaron Gagern
//
//		This program is free software: you can redistribute it and/or modify
//		it under the terms of the GNU General Public License as published by
//		the Free Software Foundation, either version 3 of the License, or
//		(at your option) any later version.
//
//		This program is distributed in the hope that it will be useful,
//		but WITHOUT ANY WARRANTY; without even the implied warranty of
//		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//		GNU General Public License for more details.
//
//		You should have received a copy of the GNU General Public License
//		along with this program.  If not, see <https://www.gnu.org/licenses/>.
/////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Tasker
{
	public abstract class Enumeration : IComparable
	{
		private readonly int enumValue;
		private readonly string enumDisplayName;

		protected Enumeration()
		{
			enumDisplayName = String.Empty;
		}

		protected Enumeration(int eV, string eDN)
		{
			enumValue = eV;
			enumDisplayName = eDN;
		}

		public int Value => enumValue;
		public string DisplayName => enumDisplayName;
		public override string ToString() => enumDisplayName;

		public static IEnumerable<T> GetAll<T>() where T : Enumeration
		{
			FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

			return fields.Select(fields => fields.GetValue(null)).Cast<T>();
		}

		public override bool Equals(object? obj)
		{
			if (obj.IsNull())
				throw new ArgumentNullException(nameof(obj), "Enumeration comparisons cannot be done on a null Enum object");

			Enumeration? otherValue = obj as Enumeration;
			
			bool typeMatches = GetType().Equals(obj!.GetType());
			bool valueMatches = enumValue.Equals(otherValue!.Value);

			return typeMatches && valueMatches;
		}

		public override int GetHashCode() => enumValue.GetHashCode();

		public static int AbsoluteDifference(Enumeration leftValue, Enumeration rightValue)
		{
			int absoluteDifference = Math.Abs(leftValue.Value - rightValue.Value);
			return absoluteDifference;
		}

		public static T FromValue<T>(int value) where T : Enumeration
		{
			T matchingItem = Parse<T, int>(value, "value", item => item.Value == value);

			return matchingItem;
		}

		public static T FromDisplayName<T>(string displayName) where T : Enumeration
		{
			T matchingItem = Parse<T, string>(displayName, "display Name", item => item.DisplayName == displayName);

			return matchingItem;
		}

		private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
		{
			T matchingItem = GetAll<T>().FirstOrDefault(predicate)!;

			if (matchingItem == null) {
				var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
				throw new ApplicationException(message);
			}

			return matchingItem;
		}

		public int CompareTo(object? other)
		{
			if (other.IsNull())
				throw new ArgumentNullException(nameof(other), "Object cannot be null when comparing it to this object");

			return enumValue.CompareTo(((Enumeration)other!).Value);
		}
	}
}