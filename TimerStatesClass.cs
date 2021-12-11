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


using static Tasker.TaskerVariables;
using static Tasker.LogCalls;

namespace Tasker
{
	public partial class TaskerMainWindow
	{
		//Used to control the timer state
		private class TimerStates
		{
			private struct StateCompare
			{
				public readonly bool compareStart;
				public readonly bool compareRunning;
				public readonly bool comparePaused;
				public readonly bool compareStopped;

				public StateCompare(bool cSt, bool cR, bool cP, bool cSp)
				{
					compareStart = cSt;
					compareRunning = cR;
					comparePaused = cP;
					compareStopped = cSp;
				}
			}

			private bool timerStarted;
			private bool timerRunning;
			private bool timerPaused;
			private bool timerStopped;

			public bool TimerStarted
			{
				get => timerStarted;
				set => timerStarted = value;
			}

			public bool TimerRunning
			{
				get => timerRunning;
				set => timerRunning = value;
			}

			public bool TimerPaused
			{
				get => timerPaused;
				set => timerPaused = value;
			}

			public bool TimerStopped
			{
				get => timerStopped;
				set => timerStopped = value;
			}

			private readonly StateCompare startCompare = new StateCompare(true, false, false, false);
			private readonly StateCompare runningCompare = new StateCompare(false, true, false, false);
			private readonly StateCompare pausedCompare = new StateCompare(false, true, true, false);
			private readonly StateCompare stoppedCompare = new StateCompare(false, false, false, true);
			private readonly StateCompare closeCompare = new StateCompare(false, false, false, false);

			public TimerStates()
			{
				timerStarted = false;
				timerRunning = false;
				timerPaused = false;
				timerStopped = false;
			}

			public TimerStates(TrackerStates tS)
			{
				if (tS == TrackerStates.Start) {
					timerStarted = startCompare.compareStart;
					timerRunning = startCompare.compareRunning;
					timerPaused = startCompare.comparePaused;
					timerStopped = startCompare.compareStopped;
				}
				else if (tS == TrackerStates.Running) {
					timerStarted = runningCompare.compareStart;
					timerRunning = runningCompare.compareRunning;
					timerPaused = runningCompare.comparePaused;
					timerStopped = runningCompare.compareStopped;
				}
				else if (tS == TrackerStates.Pause) {
					timerStarted = pausedCompare.compareStart;
					timerRunning = pausedCompare.compareRunning;
					timerPaused = pausedCompare.comparePaused;
					timerStopped = pausedCompare.compareStopped;
				}
				else if (tS == TrackerStates.Stop) {
					timerStarted = stoppedCompare.compareStart;
					timerRunning = stoppedCompare.compareRunning;
					timerPaused = stoppedCompare.comparePaused;
					timerStopped = stoppedCompare.compareStopped;
				}
				else if (tS == TrackerStates.NormalClose || tS == TrackerStates.ForceClose) {
					timerStarted = closeCompare.compareStart;
					timerRunning = closeCompare.compareRunning;
					timerPaused = closeCompare.comparePaused;
					timerStopped = closeCompare.compareStopped;
				}
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			public override bool Equals(object? obj)
			{
				if (obj == null)
					return false;

				return this.Equals((TrackerStates)obj);
			}

			private bool Equals(TrackerStates state)
			{
				if (state == TrackerStates.Start) {
					if (true.EqualsAny((timerStarted != startCompare.compareStart),
									  (timerRunning != startCompare.compareRunning),
									  (timerPaused != startCompare.comparePaused),
									  (timerStopped != startCompare.compareStopped))) {
						return false;
					}
				}
				else if (state == TrackerStates.Running) {
					if (true.EqualsAny((timerStarted != runningCompare.compareStart),
									  (timerRunning != runningCompare.compareRunning),
									  (timerPaused != runningCompare.comparePaused),
									  (timerStopped != runningCompare.compareStopped))) {
						return false;
					}
				}
				else if (state == TrackerStates.Pause) {
					if (true.EqualsAny((timerStarted != pausedCompare.compareStart),
									  (timerRunning != pausedCompare.compareRunning),
									  (timerPaused != pausedCompare.comparePaused),
									  (timerStopped != pausedCompare.compareStopped))) {
						return false;
					}
				}
				else if (state == TrackerStates.Stop) {
					if (true.EqualsAny((timerStarted != stoppedCompare.compareStart),
									  (timerRunning != stoppedCompare.compareRunning),
									  (timerPaused != stoppedCompare.comparePaused),
									  (timerStopped != stoppedCompare.compareStopped))) {
						return false;
					}
				}
				else if (state == TrackerStates.NormalClose || state == TrackerStates.ForceClose) {
					if (true.EqualsAny((timerStarted != closeCompare.compareStart),
									  (timerRunning != closeCompare.compareRunning),
									  (timerPaused != closeCompare.comparePaused),
									  (timerPaused != closeCompare.compareStopped))) {
						return false;
					}
				}

				return true;
			}

			public void AssignNewState(TrackerStates state)
			{
				if (state == TrackerStates.Start) {
					timerStarted = startCompare.compareStart;
					timerRunning = startCompare.compareRunning;
					timerPaused = startCompare.comparePaused;
					timerStopped = startCompare.compareStopped;
				}
				else if (state == TrackerStates.Running) {
					timerStarted = runningCompare.compareStart;
					timerRunning = runningCompare.compareRunning;
					timerPaused = runningCompare.comparePaused;
					timerStopped = runningCompare.compareStopped;
				}
				else if (state == TrackerStates.Pause) {
					timerStarted = pausedCompare.compareStart;
					timerRunning = pausedCompare.compareRunning;
					timerPaused = pausedCompare.comparePaused;
					timerStopped = pausedCompare.compareStopped;
				}
				else if (state == TrackerStates.Stop) {
					timerStarted = stoppedCompare.compareStart;
					timerRunning = stoppedCompare.compareRunning;
					timerPaused = stoppedCompare.comparePaused;
					timerStopped = stoppedCompare.compareStopped;
				}
				else if(state == TrackerStates.NormalClose || state == TrackerStates.ForceClose) {
					timerStarted = closeCompare.compareStart;
					timerRunning = closeCompare.compareRunning;
					timerPaused = closeCompare.comparePaused;
					timerStopped = closeCompare.compareStopped;
				}
			}

			public static bool operator ==(TimerStates tS, TrackerStates state)
			{
				if (tS == null)
					return false;

				return tS.Equals(state);
			}

			public static bool operator !=(TimerStates tS, TrackerStates state)
			{
				if (tS == null)
					return true;

				return !tS.Equals(state);
			}

			public bool EqualsAll(params TrackerStates[] collection)
			{
				if (collection.Rank > 1)
					throw new Exception("Multidimensional arrays can not be used with TimerStates extension functions - Error: TimerStates EqualsAll");
				bool boolTrack = true;
				int i = 0;
				while (i < collection.GetLength(0)) {
					if (this != (TrackerStates)collection.GetValue(i)!) {
						boolTrack = false;
						break;
					}

					i++;
				}

				return boolTrack;
			}

			public bool EqualsAny(params TrackerStates[] collection)
			{
				if (collection.Rank > 1)
					throw new Exception("Multidimensional arrays cannot be used with TimerStates extension functions - Error: TimerStates EqualsAny");

				foreach (TrackerStates i in collection) {
					if (this == i)
						return true;
				}

				return false;
			}
		}
	}
}