using System;

namespace Tasker
{
	public enum BoolState : short
	{
		Indeterminate = -1,
		False = 0,
		True = 1
	}

	public class SpecialBoolean
	{
		private BoolState bState;

		public SpecialBoolean()
		{
			bState = BoolState.Indeterminate;
		}

		public SpecialBoolean(BoolState bS)
		{
			bState = bS;
		}

		public SpecialBoolean(bool v)
		{
			if (v)
				bState = BoolState.True;
			else
				bState = BoolState.False;
		}

		public bool GetValue()
		{
			if (bState == BoolState.True)
				return true;
			else if (bState == BoolState.False)
				return false;
			else
				return false;
		}

		public void SetValue(bool v)
		{
			if (v)
				bState = BoolState.True;
			else
				bState = BoolState.False;
		}

		public BoolState GetValue(int v)
		{
			if (bState == BoolState.True)
				return BoolState.True;
			else if (bState == BoolState.False)
				return BoolState.False;
			else
				return BoolState.Indeterminate;
		}

		public void SetValue(BoolState v)
		{
			bState = v;
		}

		public override int GetHashCode()
		{
			return bState.GetHashCode();
		}

		public override bool Equals(object? obj)
		{
			if (obj.IsNull())
				throw new ArgumentNullException(nameof(obj), "Cannot use comparison on null object");

			SpecialBoolean? compValue = obj as SpecialBoolean;

			bool typeMatches = GetType().Equals(obj!.GetType());
			bool valueMatches = bState.Equals(compValue!.GetValue(0));

			return typeMatches && valueMatches;
		}

		public override string ToString()
		{
			if (bState == BoolState.True)
				return "true";
			else if (bState == BoolState.False)
				return "false";
			else
				return "indeterminate";
		}

		public static bool operator ==(SpecialBoolean spBool, BoolState bState)
		{
			if (spBool.IsNull())
				throw new ArgumentNullException(nameof(spBool), "Cannot use comparison on null SpecialBoolean object");

			return (spBool.GetValue(0) == bState);
		}

		public static bool operator !=(SpecialBoolean spBool, BoolState bState)
		{
			if (spBool.IsNull())
				throw new ArgumentNullException(nameof(spBool), "Cannot use comparison on null SpecialBoolean object");

			return !(spBool.GetValue(0) == bState);
		}

		public static bool operator ==(SpecialBoolean spBoolLeft, SpecialBoolean spBoolRight)
		{
			if (spBoolLeft.IsNull())
				throw new ArgumentNullException(nameof(spBoolLeft), "Cannot use comparison on null SpecialBoolean object");
			if (spBoolRight.IsNull())
				throw new ArgumentNullException(nameof(spBoolRight), "Cannot use comparison on null SpecialBoolean object");

			return (spBoolLeft.GetValue(0) == spBoolRight.GetValue(0));
		}

		public static bool operator !=(SpecialBoolean spBoolLeft, SpecialBoolean spBoolRight)
		{
			if (spBoolLeft.IsNull())
				throw new ArgumentNullException(nameof(spBoolLeft), "Cannot use comparison on null SpecialBoolean object");
			if (spBoolRight.IsNull())
				throw new ArgumentNullException(nameof(spBoolRight), "Cannot use comparison on null SpecialBoolean object");

			return !(spBoolLeft.GetValue(0) == spBoolRight.GetValue(0));
		}

		public static bool operator ==(SpecialBoolean boolLeft, bool boolRight)
		{
			if (boolLeft.IsNull())
				throw new ArgumentNullException(nameof(boolLeft), "Cannot use comparison on null SpecialBoolean object");

			return (boolLeft.GetValue() == boolRight);
		}

		public static bool operator !=(SpecialBoolean boolLeft, bool boolRight)
		{
			if (boolLeft.IsNull())
				throw new ArgumentNullException(nameof(boolLeft), "Cannot use comparison on null SpecialBoolean object");

			return !(boolLeft.GetValue() == boolRight);
		}
	}

	
}