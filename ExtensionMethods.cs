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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Tasker
{
	public static class ConsoleExtensions
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool AllocConsole();

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FreeConsole();

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool AttachConsole(int dwProcessId);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

		public static void StartConsoleEx()
		{
			IntPtr ptr = GetForegroundWindow();

			_ = GetWindowThreadProcessId(ptr, out int u);

			Process process = Process.GetProcessById(u);

			if (process.ProcessName == "cmd")
				AttachConsole(process.Id);
			else
				AllocConsole();
		}

		public static void FreeConsoleEx()
		{
			FreeConsole();
		}
	}

	public static class ExtensionMethods
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////
		// -- Window Flashing -- //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////
		[DllImport("user32.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

		//Setting to enable flashing of both caption and taskbar button
		private const uint FLASHW_ALL = 3;

		//Setting to enable flashing continuously until it goes to foreground
		private const uint FLASHW_TIMERNOFG = 12;

		[StructLayout(LayoutKind.Sequential)]
		private struct FLASHWINFO
		{
			public uint cbSize;
			public IntPtr hwnd;
			public uint dwFlags;
			public uint uCount;
			public uint dwTimeout;
		}

		//Actual function that handles window flashing
		public static bool FlashNotification(this Form form)
		{
			IntPtr hWnd = form.Handle;
			FLASHWINFO fInfo = new();

			fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
			fInfo.hwnd = hWnd;
			fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
			fInfo.uCount = uint.MaxValue;
			fInfo.dwTimeout = 0;

			return FlashWindowEx(ref fInfo);
		}
	}

	public static class ErrorProviderExtensions
	{
		public static void ResetError(this ErrorProvider eP, Control ctrl, string msg)
		{
			if (msg != string.Empty) {
				if (eP.GetError(ctrl) != string.Empty) {
					eP.SetError(ctrl, string.Empty);
					eP.SetError(ctrl, msg);
				}
				else {
					eP.SetError(ctrl, msg);
				}
			}
			else
				eP.SetError(ctrl, string.Empty);
		}
	}

	public static class BooleanExtensions
	{
		public static bool EqualsAll(this bool obj, params bool[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with boolean extension functions - Error: Bool EqualsAll");

			bool boolTrack = true;
			int i = 0;
			while (i < collection.GetLength(0)) {
				if (obj != (bool)collection.GetValue(i)!) {
					boolTrack = false;
					break;
				}

				i++;
			}

			return boolTrack;
		}

		public static bool EqualsAny(this bool obj, params bool[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with boolean extension functions - Error: Bool EqualsAny");

			foreach (bool i in collection) {
				if (obj == i)
					return true;
			}

			return false;
		}

		public static int ToInt(this bool obj)
		{
			if (obj == true)
				return 1;

			return 0;
		}
	}

	public static class StringExtensions
	{
		//Used when you want to see if a string contains any of the possible string values in the collection
		public static bool ContainsAny(this string obj, params string[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with boolean extension functions - Error: String ContainsAny - String Based Collection");

			foreach (string i in collection) {
				if (obj.Contains(i))
					return true;
			}

			return false;
		}

		//Used when you want to see if a string contains any of the possible character values in the collection
		public static bool ContainsAny(this string obj, params char[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with boolean extension functions - Error: String ContainsAny - Char Based Collection");

			foreach (char i in collection) {
				if (obj.Contains(i))
					return true;
			}

			return false;
		}

		//Used when you want to see if a string contains all of the possible string values in the collection
		public static bool ContainsAll(this string obj, params string[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with boolean extension functions - Error: String ContainsAll - String Based Collection");

			bool boolTrack = true;
			int i = 0;
			while (i < collection.GetLength(0)) {
				if (!obj.Contains((string)collection.GetValue(i)!)) {
					boolTrack = false;
					break;
				}

				i++;
			}

			return boolTrack;
		}

		//Used when you want to see if a string contains all of the possible character values in the collection
		public static bool ContainsAll(this string obj, params char[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with boolean extension functions - Error: String ContainsAll - Char Based Collection");

			bool boolTrack = true;
			int i = 0;
			while (i < collection.GetLength(0)) {
				if (!obj.Contains((char)collection.GetValue(i)!)) {
					boolTrack = false;
					break;
				}

				i++;
			}

			return boolTrack;
		}

		//Used when you want to see if the string is an exact match on any of the string values in the collection
		public static bool MatchesAny(this string obj, params string[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with boolean extension functions - Error: String MatchesAny");

			foreach (string i in collection) {
				if (obj.Contains(i) && obj.Length == i.Length) {
					return true;
				}
			}

			return false;
		}

		//Used when you want to see if the string is an exact match on all of the string values in the collection
		public static bool MatchesAll(this string obj, params string[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with boolean extension functions - Error: String MatchesAll");

			bool boolTrack = true;
			int i = 0;
			while (i < collection.GetLength(0)) {
				string match = (string)collection.GetValue(i)!;
				if (!(obj.Contains(match) && obj.Length == match.Length)) {
					boolTrack = false;
					break;
				}

				i++;
			}

			return boolTrack;
		}
	}

	public static class CharacterExtensions
	{
		//USed when you want to see if a character matches any of the characters in the collection
		public static bool EqualsAny(this char obj, params char[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with boolean extension functions - Error: Char EqualsAny - Char Based Collection");

			foreach (char i in collection) {
				if (obj == i)
					return true;
			}

			return false;
		}

		public static bool EqualsAny(this char obj, params string[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with boolean extension functions - Error: Char EqualsAny - String Based Collection");

			foreach (string i in collection) {
				if (i.Length > 1)
					throw new Exception("Can't use EqualsAny with strings larger than one character when comparison is char - Error: Char EqualsAny");

				if (obj == i[0])
					return true;
			}

			return false;
		}

		public static bool EqualsAll(this char obj, params char[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with boolean extension functions - Error: Char EqualsAll - Char Based Collection");

			bool boolTrack = true;
			int i = 0;
			while (i < collection.GetLength(0)) {
				if (obj != (char)collection.GetValue(i)!) {
					boolTrack = false;
					break;
				}

				i++;
			}

			return boolTrack;
		}

		public static bool EqualsAll(this char obj, params string[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with boolean extension functions - Error: Char EqualsAll - String Based Collection");

			bool boolTrack = true;
			int i = 0;
			while (i < collection.GetLength(0)) {
				string value = (string)collection.GetValue(i)!;
				if (value.Length > 1)
					throw new Exception("Can't use EqualsAny with strings larger than one character when comparison is char - Error: Char EqualsAll");

				if (obj != (char)collection.GetValue(i)!) {
					boolTrack = false;
					break;
				}

				i++;
			}

			return boolTrack;
		}
	}

	public static class IntegerExtensions
	{
		public static bool EqualsAny(this int obj, params int[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with integer extension functions - Error: Int EqualsAny");

			foreach (int i in collection) {
				if (obj == i)
					return true;
			}

			return false;
		}

		public static bool EqualsAll(this int obj, params int[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with integer extension functions - Error: Int EqualsAny");

			bool boolTrack = true;
			int i = 0;
			while (i < collection.GetLength(0)) {
				if (obj != (int)collection.GetValue(i)!) {
					boolTrack = false;
					break;
				}

				i++;
			}

			return boolTrack;
		}

		public static bool ToBool(this int obj)
		{
			if (obj != 0 && obj != 1)
				throw new Exception("Cannot convert integers other than 1 or 0 (1, 0 - true/false) into a boolean value - Error: ToBool");

			if (obj == 1)
				return true;

			return false;
		}

		public static string ToTimeString(this int obj)
		{
			int h = obj / 60;
			int m = obj % 60;
			string hours;
			string minutes;
			if (m == 0 || m < 10)
				minutes = "0" + m.ToString();
			else
				minutes = m.ToString();

			if (h == 0 || h < 10)
				hours = "0" + h.ToString();
			else
				hours = h.ToString();

			return hours + ":" + minutes;
		}
	}

	public static class LogLevelExtensions
	{
		public static bool EqualsAny(this LogLevels obj, params LogLevels[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with integer extension functions - Error: LogLevel EqualsAny");

			foreach (LogLevels i in collection) {
				if (i == obj)
					return true;
			}

			return false;
		}

		public static bool EqualsAll(this LogLevels obj, params LogLevels[] collection)
		{
			if (collection.Rank > 1)
				throw new Exception("Multidimensional arrays cannot be used with integer extension functions - Error: LogLevel EqualsAll");

			bool boolTrack = false;
			int i = 0;
			while (i < collection.GetLength(0)) {
				if (obj != (LogLevels)collection.GetValue(i)!) {
					boolTrack = false;
					break;
				}

				i++;
			}

			return boolTrack;
		}
	}

	public static class ObjectExtensions
	{
		public static bool IsNull(this object? obj)
		{
			if (obj! == null)
				return true;
			else
				return false;
		}
	}

	public class SetOnInit<T>
	{
		private readonly object setLock = new object();
		private readonly bool throwIfNotSet = false;
		private readonly string valueName;
		private bool set = false;
		private T? value;

		public SetOnInit(string vN)
		{
			valueName = vN;
			throwIfNotSet = true;
		}

		public SetOnInit(string vN, T defaultV)
		{
			valueName = vN;
			value = defaultV;
		}

		public T Value
		{
			get
			{
				lock(setLock) {
					if (!set && throwIfNotSet)
						throw new ValueNotSetException(valueName);

					return value!;
				}
			}

			set
			{
				lock(setLock) {
					if (set)
						throw new AlreadySetException(valueName);

					set = true;
					this.value = value;
				}
			}
		}

		public static implicit operator T(SetOnInit<T> toConvert)
		{
			return toConvert.value!;
		}
	}

	public class NamedValueException : InvalidOperationException
	{
		private readonly string valueName;

		public NamedValueException(string vN, string msgFrmt) : base(string.Format(msgFrmt, vN))
		{
			valueName = vN;
		}

		public string ValueName
		{
			get
			{
				return valueName;
			}
		}
	}

	public class AlreadySetException : NamedValueException
	{
		private const string MESSAGE = "The value \"{0}\" has already been set";

		public AlreadySetException(string vN) : base(vN, MESSAGE)
		{}
	}

	public class ValueNotSetException : NamedValueException
	{
		private const string MESSAGE = "The value \"{0}\" has not been set yet";

		public ValueNotSetException(string vN) : base(vN, MESSAGE)
		{}
	}
}