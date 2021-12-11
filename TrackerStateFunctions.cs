using System;
using System.Windows.Forms;
using System.Drawing;


using static Tasker.TaskerVariables;
using static Tasker.LogCalls;

namespace Tasker
{
	public partial class TaskerMainWindow
	{
		private void SetTrackerState(TrackerStates trs)
		{
			if(IsDebugModeEnabled)
				Debug("Setting tracker state");

			switch(trs) 
			{
				case TrackerStates.Start:
					TrackerStateStart();
					
					break;
				case TrackerStates.Running:
					TrackerStateRunning();

					break;
				case TrackerStates.Pause:
					TrackerStatePaused();

					break;
				case TrackerStates.Stop:
					TrackerStateStopped();

					break;
				case TrackerStates.ForceClose:
					TrackerStateForceClose();

					break;
				case TrackerStates.NormalClose:
					TrackerStateNormalClose();

					break;
			}
		}
		
		private void TrackerStateStart()
		{
			lock(entryLock) {
				tS.AssignNewState(TrackerStates.Start);
			}
			
			//Setting text box states
			SetStartTimeTextBoxEnabledState(false);
			SetEndTimeTextBoxEnabledState(false);
			SetRemindIntervTextBoxEnabledState(false);

			SetButtonsState(TrackerStates.Start);

			if (this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)delegate ()
				{
					this.Text = taskerStatusStarted;
				});
			}
			else
				this.Text = taskerStatusStarted;

			SendTaskerStateStatusMessage(TrackerStates.Start);
		}

		private void TrackerStateRunning()
		{
			lock(entryLock) {
				tS.AssignNewState(TrackerStates.Running);
			}

			SetButtonsState(TrackerStates.Running);

			if (this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)delegate ()
				{
					this.Text = taskerStatusRunning;
				});
			}
			else
				this.Text = taskerStatusRunning;

			SendTaskerStateStatusMessage(TrackerStates.Running);
		}

		private void TrackerStatePaused()
		{
			lock(entryLock) {
				tS.AssignNewState(TrackerStates.Pause);
			}

			SetButtonsState(TrackerStates.Pause);

			if (this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)delegate ()
				{
					this.Text = taskerStatusPaused;
				});
			}
			else
				this.Text = taskerStatusPaused;

			SendTaskerStateStatusMessage(TrackerStates.Pause);
		}

		private void TrackerStateStopped()
		{
			lock(entryLock) {
				tS.AssignNewState(TrackerStates.Stop);
			}

			//Setting text box states
			SetStartTimeTextBoxEnabledState(true);
			SetEndTimeTextBoxEnabledState(true);
			SetRemindIntervTextBoxEnabledState(true);

			//Set Button States
			SetButtonsState(TrackerStates.Stop);

			if (this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)delegate ()
				{
					this.Text = taskerStatusStopped;
				});
			}
			else
				this.Text = taskerStatusStopped;

			SendTaskerStateStatusMessage(TrackerStates.Stop);
		}

		private void TrackerStateForceClose()
		{
			lock(entryLock) {
				tS.AssignNewState(TrackerStates.ForceClose);
			}

			//Setting text box states
			SetStartTimeTextBoxEnabledState(false);
			SetEndTimeTextBoxEnabledState(false);
			SetRemindIntervTextBoxEnabledState(false);

			//Set button states
			SetButtonsState(TrackerStates.ForceClose);

			if (this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)delegate ()
				{
					this.Text = String.Empty;
				});
			}
			else
				this.Text = String.Empty;

			SendTaskerStateStatusMessage(TrackerStates.ForceClose);
		}

		private void TrackerStateNormalClose()
		{
			lock(entryLock) {
				tS.AssignNewState(TrackerStates.NormalClose);
			}

			//Setting text box states
			SetStartTimeTextBoxEnabledState(false);
			SetEndTimeTextBoxEnabledState(false);
			SetRemindIntervTextBoxEnabledState(false);

			//Set button states
			SetButtonsState(TrackerStates.NormalClose);

			if (this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)delegate ()
				{
					this.Text = String.Empty;
				});
			}
			else
				this.Text = String.Empty;

			SendTaskerStateStatusMessage(TrackerStates.NormalClose);
		}
		
		private void SetStartTimeTextBoxEnabledState(bool state)
		{
			if (startTimeTextBox.InvokeRequired) {
				startTimeTextBox.BeginInvoke((MethodInvoker)delegate ()
				{
					startTimeTextBox.Enabled = state;
				});
			}
			else
				startTimeTextBox.Enabled = state;
		}

		private void SetEndTimeTextBoxEnabledState(bool state)
		{
			if (endTimeTextBox.InvokeRequired) {
				endTimeTextBox.BeginInvoke((MethodInvoker)delegate ()
				{
					endTimeTextBox.Enabled = state;
				});
			}
			else
				endTimeTextBox.Enabled = state;
		}

		private void SetRemindIntervTextBoxEnabledState(bool state)
		{
			if (remindIntervTextBox.InvokeRequired) {
				remindIntervTextBox.BeginInvoke((MethodInvoker)delegate ()
				{
					remindIntervTextBox.Enabled = state;
				});
			}
			else
				remindIntervTextBox.Enabled = state;
		}

		private void SetButtonsState(TrackerStates state)
		{
			if(this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)delegate ()
				{
					if (state == TrackerStates.Start) {
						//Start Button
						startTrackButton.Enabled = false;
						startTrackButton.BackColor = Color.MediumSeaGreen;
						startTrackButton.FlatStyle = FlatStyle.Standard;
						startTrackButton.Text = "Time Tracking Started";
						startTrackButton.TextAlign = ContentAlignment.TopCenter;
						startTrackButton.Cursor = Cursors.WaitCursor;

						//Start Menu Button
						startTrackingMenuButton.Enabled = false;
						startTrackingMenuButton.BackColor = Color.MediumSeaGreen;
						startTrackingMenuButton.Text = "Time Tracking Started";

						/////////////////////////////////////////////////////////////
						/////////////////////////////////////////////////////////////

						//Pause Button
						pauseTrackButton.Enabled = false;
						pauseTrackButton.BackColor = Color.White;
						pauseTrackButton.FlatStyle = FlatStyle.Standard;
						pauseTrackButton.Text = "Pause Time Tracker";
						pauseTrackButton.TextAlign = ContentAlignment.MiddleCenter;
						pauseTrackButton.Cursor = Cursors.WaitCursor;

						//Pause Menu Button
						pauseTrackingMenuButton.Enabled = false;
						pauseTrackingMenuButton.BackColor = SystemColors.Control;
						pauseTrackingMenuButton.Text = "Pause Time Tracker";

						/////////////////////////////////////////////////////////////
						/////////////////////////////////////////////////////////////

						//Stop Button
						stopTrackButton.Enabled = false;
						stopTrackButton.BackColor = Color.White;
						stopTrackButton.FlatStyle = FlatStyle.Standard;
						stopTrackButton.Text = "Stop Time Tracker";
						stopTrackButton.TextAlign = ContentAlignment.MiddleCenter;

						//Stop Menu Button
						stopTrackingMenuButton.Enabled = false;
						stopTrackingMenuButton.BackColor = SystemColors.Control;
						stopTrackingMenuButton.Text = "Stop Time Tracker";
					}
					else if (state == TrackerStates.Running) {
						//Start Button
						startTrackButton.Enabled = false;
						startTrackButton.BackColor = Color.MediumSeaGreen;
						startTrackButton.FlatStyle = FlatStyle.Standard;
						startTrackButton.Text = "Time Tracking Running";
						startTrackButton.TextAlign = ContentAlignment.MiddleCenter;
						startTrackButton.Cursor = Cursors.WaitCursor;

						//Start Menu Button
						startTrackingMenuButton.Enabled = false;
						startTrackingMenuButton.BackColor = Color.MediumSeaGreen;
						startTrackingMenuButton.Text = "Time Tracking Running";

						/////////////////////////////////////////////////////////////
						/////////////////////////////////////////////////////////////

						//Pause Button
						pauseTrackButton.Enabled = true;
						pauseTrackButton.BackColor = Color.White;
						pauseTrackButton.FlatStyle = FlatStyle.Popup;
						pauseTrackButton.Text = "Pause Time Tracker";
						pauseTrackButton.TextAlign = ContentAlignment.MiddleCenter;
						pauseTrackButton.Cursor = Cursors.Hand;

						//Pause Menu Button
						pauseTrackingMenuButton.Enabled = true;
						pauseTrackingMenuButton.BackColor = SystemColors.Control;
						pauseTrackingMenuButton.Text = "Pause Time Tracker";

						/////////////////////////////////////////////////////////////
						/////////////////////////////////////////////////////////////

						//Stop Button
						stopTrackButton.Enabled = true;
						stopTrackButton.BackColor = Color.White;
						stopTrackButton.FlatStyle = FlatStyle.Popup;
						stopTrackButton.Text = "Stop Time Tracker";
						stopTrackButton.TextAlign = ContentAlignment.MiddleCenter;
						stopTrackButton.Cursor = Cursors.Hand;

						//Stop Menu Button
						stopTrackingMenuButton.Enabled = true;
						stopTrackingMenuButton.BackColor = SystemColors.Control;
						stopTrackingMenuButton.Text = "Stop Time Tracker";
					}
					else if (state == TrackerStates.Pause) {
						//Start Button
						startTrackButton.Enabled = true;
						startTrackButton.BackColor = Color.White;
						startTrackButton.FlatStyle = FlatStyle.Popup;
						startTrackButton.Text = "Restart Time Tracking";
						startTrackButton.TextAlign = ContentAlignment.MiddleCenter;
						startTrackButton.Cursor = Cursors.Hand;

						//Start Menu Button
						startTrackingMenuButton.Enabled = true;
						startTrackingMenuButton.BackColor = SystemColors.Control;
						startTrackingMenuButton.Text = "Restart Time Tracking";

						/////////////////////////////////////////////////////////////
						/////////////////////////////////////////////////////////////

						//Pause Button
						pauseTrackButton.Enabled = false;
						pauseTrackButton.BackColor = Color.Gold;
						pauseTrackButton.FlatStyle = FlatStyle.Standard;
						pauseTrackButton.Text = "Time Tracker Paused";
						pauseTrackButton.TextAlign = ContentAlignment.TopCenter;
						pauseTrackButton.Cursor = Cursors.WaitCursor;

						//Pause Menu Button
						pauseTrackingMenuButton.Enabled = false;
						pauseTrackingMenuButton.BackColor = Color.Gold;
						pauseTrackingMenuButton.Text = "Time Tracker Paused";

						/////////////////////////////////////////////////////////////
						/////////////////////////////////////////////////////////////

						//Stop Button
						stopTrackButton.Enabled = true;
						stopTrackButton.BackColor = Color.White;
						stopTrackButton.FlatStyle = FlatStyle.Popup;
						stopTrackButton.Text = "Stop Time Tracker";
						stopTrackButton.TextAlign = ContentAlignment.MiddleCenter;
						stopTrackButton.Cursor = Cursors.Hand;

						//Stop Menu Button
						stopTrackingMenuButton.Enabled = true;
						stopTrackingMenuButton.BackColor = SystemColors.Control;
						stopTrackingMenuButton.Text = "Stop Time Tracker";
					}
					else if (state == TrackerStates.Stop) {
						//Start Button
						startTrackButton.Enabled = true;
						startTrackButton.BackColor = Color.White;
						startTrackButton.FlatStyle = FlatStyle.Popup;
						startTrackButton.Text = "Start Time Tracker";
						startTrackButton.TextAlign = ContentAlignment.MiddleCenter;
						startTrackButton.Cursor = Cursors.Hand;

						//Start Menu Button
						startTrackingMenuButton.Enabled = true;
						startTrackingMenuButton.BackColor = SystemColors.Control;
						startTrackingMenuButton.Text = "Start Time Tracker";

						/////////////////////////////////////////////////////////////
						/////////////////////////////////////////////////////////////

						//Pause Button
						pauseTrackButton.Enabled = false;
						pauseTrackButton.BackColor = Color.White;
						pauseTrackButton.FlatStyle = FlatStyle.Standard;
						pauseTrackButton.Text = "Pause Time Tracker";
						pauseTrackButton.TextAlign = ContentAlignment.MiddleCenter;
						pauseTrackButton.Cursor = Cursors.WaitCursor;

						//Pause Menu Button
						pauseTrackingMenuButton.Enabled = false;
						pauseTrackingMenuButton.BackColor = SystemColors.Control;
						pauseTrackingMenuButton.Text = "Pause Time Tracker";

						/////////////////////////////////////////////////////////////
						/////////////////////////////////////////////////////////////

						//Stop Button
						stopTrackButton.Enabled = false;
						stopTrackButton.BackColor = Color.IndianRed;
						stopTrackButton.FlatStyle = FlatStyle.Standard;
						stopTrackButton.Text = "Time Tracker Stopped";
						stopTrackButton.TextAlign = ContentAlignment.MiddleCenter;
						stopTrackButton.Cursor = Cursors.WaitCursor;

						//Stop Menu Button
						stopTrackingMenuButton.Enabled = false;
						stopTrackingMenuButton.BackColor = Color.IndianRed;
						stopTrackingMenuButton.Text = "Time Tracker Stopped";
					}
					else if (state == TrackerStates.NormalClose || state == TrackerStates.ForceClose) {
						//Start Button
						startTrackButton.Enabled = false;
						startTrackButton.FlatStyle = FlatStyle.System;
						startTrackButton.Text = "Start Time Tracker";
						startTrackButton.TextAlign = ContentAlignment.MiddleCenter;
						startTrackButton.Cursor = Cursors.WaitCursor;

						//Start Menu Button
						startTrackingMenuButton.Enabled = false;
						startTrackingMenuButton.BackColor = SystemColors.ControlDarkDark;
						startTrackingMenuButton.Text = "Start Time Tracker";

						/////////////////////////////////////////////////////////////
						/////////////////////////////////////////////////////////////

						//Pause Button
						pauseTrackButton.Enabled = false;
						pauseTrackButton.FlatStyle = FlatStyle.System;
						pauseTrackButton.Text = "Pause Time Tracker";
						pauseTrackButton.TextAlign = ContentAlignment.MiddleCenter;
						pauseTrackButton.Cursor = Cursors.WaitCursor;

						//Pause Menu Button
						pauseTrackingMenuButton.Enabled = false;
						pauseTrackingMenuButton.BackColor = SystemColors.ControlDarkDark;
						pauseTrackingMenuButton.Text = "Pause Time Tracker";

						/////////////////////////////////////////////////////////////
						/////////////////////////////////////////////////////////////

						//Stop Button
						stopTrackButton.Enabled = false;
						stopTrackButton.FlatStyle = FlatStyle.System;
						stopTrackButton.Text = "Stop Time Tracker";
						stopTrackButton.TextAlign = ContentAlignment.MiddleCenter;
						stopTrackButton.Cursor = Cursors.WaitCursor;

						//Stop Menu Button
						stopTrackingMenuButton.Enabled = false;
						stopTrackingMenuButton.BackColor = SystemColors.ControlDarkDark;
						stopTrackingMenuButton.Text = "Stop Time Tracker";
					}
				});
			}
			else {
				if (state == TrackerStates.Start) {
					//Start Button
					startTrackButton.Enabled = false;
					startTrackButton.BackColor = Color.MediumSeaGreen;
					startTrackButton.FlatStyle = FlatStyle.Standard;
					startTrackButton.Text = "Time Tracking Started";
					startTrackButton.TextAlign = ContentAlignment.TopCenter;
					startTrackButton.Cursor = Cursors.WaitCursor;

					//Start Menu Button
					startTrackingMenuButton.Enabled = false;
					startTrackingMenuButton.BackColor = Color.MediumSeaGreen;
					startTrackingMenuButton.Text = "Time Tracking Started";

					/////////////////////////////////////////////////////////////
					/////////////////////////////////////////////////////////////

					//Pause Button
					pauseTrackButton.Enabled = false;
					pauseTrackButton.BackColor = Color.White;
					pauseTrackButton.FlatStyle = FlatStyle.Standard;
					pauseTrackButton.Text = "Pause Time Tracker";
					pauseTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					pauseTrackButton.Cursor = Cursors.WaitCursor;

					//Pause Menu Button
					pauseTrackingMenuButton.Enabled = false;
					pauseTrackingMenuButton.BackColor = SystemColors.Control;
					pauseTrackingMenuButton.Text = "Pause Time Tracker";

					/////////////////////////////////////////////////////////////
					/////////////////////////////////////////////////////////////

					//Stop Button
					stopTrackButton.Enabled = false;
					stopTrackButton.BackColor = Color.White;
					stopTrackButton.FlatStyle = FlatStyle.Standard;
					stopTrackButton.Text = "Stop Time Tracker";
					stopTrackButton.TextAlign = ContentAlignment.MiddleCenter;

					//Stop Menu Button
					stopTrackingMenuButton.Enabled = false;
					stopTrackingMenuButton.BackColor = SystemColors.Control;
					stopTrackingMenuButton.Text = "Stop Time Tracker";
				}
				else if (state == TrackerStates.Running) {
					//Start Button
					startTrackButton.Enabled = false;
					startTrackButton.BackColor = Color.MediumSeaGreen;
					startTrackButton.FlatStyle = FlatStyle.Standard;
					startTrackButton.Text = "Time Tracking Running";
					startTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					startTrackButton.Cursor = Cursors.WaitCursor;

					//Start Menu Button
					startTrackingMenuButton.Enabled = false;
					startTrackingMenuButton.BackColor = Color.MediumSeaGreen;
					startTrackingMenuButton.Text = "Time Tracking Running";

					/////////////////////////////////////////////////////////////
					/////////////////////////////////////////////////////////////

					//Pause Button
					pauseTrackButton.Enabled = true;
					pauseTrackButton.BackColor = Color.White;
					pauseTrackButton.FlatStyle = FlatStyle.Popup;
					pauseTrackButton.Text = "Pause Time Tracker";
					pauseTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					pauseTrackButton.Cursor = Cursors.Hand;

					//Pause Menu Button
					pauseTrackingMenuButton.Enabled = true;
					pauseTrackingMenuButton.BackColor = SystemColors.Control;
					pauseTrackingMenuButton.Text = "Pause Time Tracker";

					/////////////////////////////////////////////////////////////
					/////////////////////////////////////////////////////////////

					//Stop Button
					stopTrackButton.Enabled = true;
					stopTrackButton.BackColor = Color.White;
					stopTrackButton.FlatStyle = FlatStyle.Popup;
					stopTrackButton.Text = "Stop Time Tracker";
					stopTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					stopTrackButton.Cursor = Cursors.Hand;

					//Stop Menu Button
					stopTrackingMenuButton.Enabled = true;
					stopTrackingMenuButton.BackColor = SystemColors.Control;
					stopTrackingMenuButton.Text = "Stop Time Tracker";
				}
				else if (state == TrackerStates.Pause) {
					//Start Button
					startTrackButton.Enabled = true;
					startTrackButton.BackColor = Color.White;
					startTrackButton.FlatStyle = FlatStyle.Popup;
					startTrackButton.Text = "Restart Time Tracking";
					startTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					startTrackButton.Cursor = Cursors.Hand;

					//Start Menu Button
					startTrackingMenuButton.Enabled = true;
					startTrackingMenuButton.BackColor = SystemColors.Control;
					startTrackingMenuButton.Text = "Restart Time Tracking";

					/////////////////////////////////////////////////////////////
					/////////////////////////////////////////////////////////////

					//Pause Button
					pauseTrackButton.Enabled = false;
					pauseTrackButton.BackColor = Color.Gold;
					pauseTrackButton.FlatStyle = FlatStyle.Standard;
					pauseTrackButton.Text = "Time Tracker Paused";
					pauseTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					pauseTrackButton.Cursor = Cursors.WaitCursor;

					//Pause Menu Button
					pauseTrackingMenuButton.Enabled = false;
					pauseTrackingMenuButton.BackColor = Color.Gold;
					pauseTrackingMenuButton.Text = "Time Tracker Paused";

					/////////////////////////////////////////////////////////////
					/////////////////////////////////////////////////////////////

					//Stop Button
					stopTrackButton.Enabled = true;
					stopTrackButton.BackColor = Color.White;
					stopTrackButton.FlatStyle = FlatStyle.Popup;
					stopTrackButton.Text = "Stop Time Tracker";
					stopTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					stopTrackButton.Cursor = Cursors.Hand;

					//Stop Menu Button
					stopTrackingMenuButton.Enabled = true;
					stopTrackingMenuButton.BackColor = SystemColors.Control;
					stopTrackingMenuButton.Text = "Stop Time Tracker";
				}
				else if (state == TrackerStates.Stop) {
					//Start Button
					startTrackButton.Enabled = true;
					startTrackButton.BackColor = Color.White;
					startTrackButton.FlatStyle = FlatStyle.Popup;
					startTrackButton.Text = "Start Time Tracker";
					startTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					startTrackButton.Cursor = Cursors.Hand;

					//Start Menu Button
					startTrackingMenuButton.Enabled = true;
					startTrackingMenuButton.BackColor = SystemColors.Control;
					startTrackingMenuButton.Text = "Start Time Tracker";

					/////////////////////////////////////////////////////////////
					/////////////////////////////////////////////////////////////

					//Pause Button
					pauseTrackButton.Enabled = false;
					pauseTrackButton.BackColor = Color.White;
					pauseTrackButton.FlatStyle = FlatStyle.Standard;
					pauseTrackButton.Text = "Pause Time Tracker";
					pauseTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					pauseTrackButton.Cursor = Cursors.WaitCursor;

					//Pause Menu Button
					pauseTrackingMenuButton.Enabled = false;
					pauseTrackingMenuButton.BackColor = SystemColors.Control;
					pauseTrackingMenuButton.Text = "Pause Time Tracker";

					/////////////////////////////////////////////////////////////
					/////////////////////////////////////////////////////////////

					//Stop Button
					stopTrackButton.Enabled = false;
					stopTrackButton.BackColor = Color.IndianRed;
					stopTrackButton.FlatStyle = FlatStyle.Standard;
					stopTrackButton.Text = "Time Tracker Stopped";
					stopTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					stopTrackButton.Cursor = Cursors.WaitCursor;

					//Stop Menu Button
					stopTrackingMenuButton.Enabled = false;
					stopTrackingMenuButton.BackColor = Color.IndianRed;
					stopTrackingMenuButton.Text = "Time Tracker Stopped";
				}
				else if (state == TrackerStates.NormalClose || state == TrackerStates.ForceClose) {
					//Start Button
					startTrackButton.Enabled = false;
					startTrackButton.FlatStyle = FlatStyle.System;
					startTrackButton.Text = "Start Time Tracker";
					startTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					startTrackButton.Cursor = Cursors.WaitCursor;

					//Start Menu Button
					startTrackingMenuButton.Enabled = false;
					startTrackingMenuButton.BackColor = SystemColors.ControlDarkDark;
					startTrackingMenuButton.Text = "Start Time Tracker";

					/////////////////////////////////////////////////////////////
					/////////////////////////////////////////////////////////////

					//Pause Button
					pauseTrackButton.Enabled = false;
					pauseTrackButton.FlatStyle = FlatStyle.System;
					pauseTrackButton.Text = "Pause Time Tracker";
					pauseTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					pauseTrackButton.Cursor = Cursors.WaitCursor;

					//Pause Menu Button
					pauseTrackingMenuButton.Enabled = false;
					pauseTrackingMenuButton.BackColor = SystemColors.ControlDarkDark;
					pauseTrackingMenuButton.Text = "Pause Time Tracker";

					/////////////////////////////////////////////////////////////
					/////////////////////////////////////////////////////////////

					//Stop Button
					stopTrackButton.Enabled = false;
					stopTrackButton.FlatStyle = FlatStyle.System;
					stopTrackButton.Text = "Stop Time Tracker";
					stopTrackButton.TextAlign = ContentAlignment.MiddleCenter;
					stopTrackButton.Cursor = Cursors.WaitCursor;

					//Stop Menu Button
					stopTrackingMenuButton.Enabled = false;
					stopTrackingMenuButton.BackColor = SystemColors.ControlDarkDark;
					stopTrackingMenuButton.Text = "Stop Time Tracker";
				}
			}
		}

		private void SendTaskerStateStatusMessage(TrackerStates trs)
		{
			if (IsDebugModeEnabled) {
				if (trs == TrackerStates.Start)
					Info("Tracker state is now in Start Mode");
				else if (trs == TrackerStates.Running)
					Info("Tracker state is now in Running Mode");
				else if (trs == TrackerStates.Pause)
					Info("Tracker state is now in Pause Mode");
				else if (trs == TrackerStates.Stop)
					Info("Tracker state is now in Stop Mode");
				else if (trs == TrackerStates.ForceClose)
					Info("Tracker state is now in Force Close Mode");
				else if (trs == TrackerStates.NormalClose)
					Info("Tracker state is now in Normal Close Mode");

				Warn("Start Time Text Box State: " + GetEnableState(startTimeTextBox.Enabled));
				Warn("End Time Text Box State: " + GetEnableState(endTimeTextBox.Enabled));
				Warn("Reminder Interval Text Box State: " + GetEnableState(remindIntervTextBox.Enabled));

				Warn("Start Tracking Button State: " + GetEnableState(startTrackButton.Enabled));
				Warn("Pause Tracking Button State: " + GetEnableState(pauseTrackButton.Enabled));
				Warn("Stop Tracking Button State: " + GetEnableState(stopTrackButton.Enabled));

				Warn("Start Tracking Menu Button State: " + GetEnableState(startTrackingMenuButton.Enabled));
				Warn("Pause Tracking Menu Button State: " + GetEnableState(pauseTrackingMenuButton.Enabled));
				Warn("Stop Tracking Menu Button State: " + GetEnableState(stopTrackingMenuButton.Enabled));
			}
		}
	}
}