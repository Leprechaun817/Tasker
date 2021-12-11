using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


using static Tasker.TaskerVariables;
using static Tasker.LogCalls;
using static Tasker.TextBoxTypes;
using static Tasker.TextBoxLibrary;

namespace Tasker
{
	public partial class TaskerMainWindow
	{
		//-- Start Tracking Button
		private void StartTrackButton_MouseDown(object sender, MouseEventArgs e)
		{
			if (startTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Start tracking button mouse down event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(startTrackButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				startTrackButton.Focus();

				StartTasker(ButtonType.FORM);
			}
		}

		private void StartTrackButton_MouseHover(object sender, EventArgs e)
		{
			if (startTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Start Tracking Button mouse hover event has been triggered");

				taskerMainWinToolTip.Show("Clicking on this will start time tracking", startTrackButton, toolTipTimerShowInterval);

				isToolTipShown = true;
				toolTipTimer.Enabled = true;
				toolTipTimer.Start();
			}
		}

		private void StartTrackButton_MouseMove(object sender, MouseEventArgs e)
		{
			if (startTrackButton.Enabled) {
				if (mouseMoveEnabled) {
					if (IsDebugModeEnabled)
						EventLog("Start tracking button mouse move event has been triggered");

					if (isToolTipShown) {
						taskerMainWinToolTip.Hide(startTrackButton);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void StartTrackButton_MouseEnter(object sender, EventArgs e)
		{
			if (startTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Start tracking button mouse enter event has been triggered");

				startTrackButton.BackColor = Color.FromArgb(192, 255, 192);
			}
		}

		private void StartTrackButton_MouseLeave(object sender, EventArgs e)
		{
			if (startTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Start Tracking Button mouse leave event has been triggered");

				if (startTrackButton.Enabled) {
					taskerMainWinToolTip.Hide(remindIntervTextBox);
					toolTipTimer.Stop();
					isToolTipShown = false;
					mouseMoveEnabled = false;
				}

				startTrackButton.BackColor = Color.White;
			}
		}

		private void StartTrackButton_Leave(object sender, EventArgs e)
		{
			if (startTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Start tracking button leave event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(startTrackButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void StartTrackButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if(startTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Start track button key down event has been triggered");

				if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
					if(isToolTipShown) {
						taskerMainWinToolTip.Hide(startTrackButton);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}

					StartTasker(ButtonType.FORM);
				}
			}
		}


		///////////////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////


		//-- Start Tracking Menu Button
		private void StartTrackingMenuButton_MouseDown(object sender, MouseEventArgs e)
		{
			if (startTrackingMenuButton.Enabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Start tracking menu button mouse down event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				taskerMenuActionsItem.HideDropDown();

				StartTasker(ButtonType.MENU);
			}
		}

		private void StartTrackingMenuButton_MouseHover(object sender, EventArgs e)
		{
			if(startTrackingMenuButton.Enabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Start tracking menu button mouse hover event has been triggered");

				taskerMainWinToolTip.Show("Clicking on this will start time tracking", tempItem.Owner, toolTipTimerShowInterval);

				isToolTipShown = true;
				toolTipTimer.Enabled = true;
				toolTipTimer.Start();
			}
		}

		private void StartTrackingMenuButton_MouseMove(object sender, MouseEventArgs e)
		{
			if(startTrackingMenuButton.Enabled) {
				if(mouseMoveEnabled) {
					ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

					if (IsDebugModeEnabled)
						EventLog("Start tracking menu button mouse move event has been triggered");

					if (isToolTipShown) {
						taskerMainWinToolTip.Hide(tempItem.Owner);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void StartTrackingMenuButton_MouseEnter(object sender, EventArgs e)
		{
			if(startTrackingMenuButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Start tracking menu button mouse enter event has been triggered");

				startTrackingMenuButton.ForeColor = SystemColors.ControlDarkDark;
				startTrackingMenuButton.BackColor = SystemColors.Control;
				startTrackingMenuButton.Font = menuMouseEnterFont;
			}
		}

		private void StartTrackingMenuButton_MouseLeave(object sender, EventArgs e)
		{
			if(startTrackingMenuButton.Enabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Start tracking menu button mouse leave event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				if (tS == TrackerStates.Start || tS == TrackerStates.Running) {
					startTrackingMenuButton.ForeColor = SystemColors.ControlText;
					startTrackingMenuButton.BackColor = Color.MediumSeaGreen;
				}
				else {
					startTrackingMenuButton.ForeColor = SystemColors.ControlText;
					startTrackingMenuButton.BackColor = SystemColors.Control;
				}

				startTrackingMenuButton.Font = menuMouseLeaveFont;
			}
		}

		private void StartTrackingMenuButton_DropDownClosed(object sender, EventArgs e)
		{
			if(startTrackingMenuButton.Enabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Start tracking menu button drop down closed event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}



		/////////////////////////////////////////////////////////////////////////////////////////////
		/////////////////////////////////////////////////////////////////////////////////////////////



		//-- Pause Tracking Button
		private void PauseTrackButton_MouseDown(object sender, MouseEventArgs e)
		{
			if (pauseTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Pause tracking button mouse down event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(pauseTrackButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				pauseTrackButton.Focus();
								
				PauseTasker(ButtonType.FORM);
			}
		}

		private void PauseTrackButton_MouseHover(object sender, EventArgs e)
		{
			if (pauseTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Pause tracking button mouse hover event has been triggered");
								
				taskerMainWinToolTip.Show("Clicking on this will pause time tracking", pauseTrackButton, toolTipTimerShowInterval);
					
				isToolTipShown = true;
				toolTipTimer.Enabled = true;
				toolTipTimer.Start();
			}
		}

		private void PauseTrackButton_MouseMove(object sender, MouseEventArgs e)
		{
			if(pauseTrackButton.Enabled) {
				if(mouseMoveEnabled) {
					if (IsDebugModeEnabled)
						EventLog("Pause track button mouse move event has been triggered");

					if(isToolTipShown) {
						taskerMainWinToolTip.Hide(pauseTrackButton);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void PauseTrackButton_MouseEnter(object sender, EventArgs e)
		{
			if (pauseTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Pause track button mouse enter event has been triggered");

				pauseTrackButton.BackColor = Color.FromArgb(255, 255, 192);
			}
		}

		private void PauseTrackButton_MouseLeave(object sender, EventArgs e)
		{
			if (pauseTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Pause tracking button mouse leave event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(pauseTrackButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				pauseTrackButton.BackColor = Color.White;
			}
		}

		private void PauseTrackButton_Leave(object sender, EventArgs e)
		{
			if(pauseTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Pause tracking button leave event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(pauseTrackButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void PauseTrackButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if(pauseTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Pause tracking button key down event has been triggered");

				if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
					if (isToolTipShown) {
						taskerMainWinToolTip.Hide(pauseTrackButton);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}

					PauseTasker(ButtonType.FORM);
				}
			}
		}


		/////////////////////////////////////////////////////////////////////////////////////////////
		/////////////////////////////////////////////////////////////////////////////////////////////


		//-- Pause Tracking Menu Button
		private void PauseTrackingMenuButton_MouseDown(object sender, MouseEventArgs e)
		{
			if (pauseTrackingMenuButton.Enabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Pause tracking menu button mouse down event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				taskerMenuActionsItem.HideDropDown();

				PauseTasker(ButtonType.MENU);
			}
		}

		private void PauseTrackingMenuButton_MouseHover(object sender, EventArgs e)
		{
			if(pauseTrackingMenuButton.Enabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Pause tracking menu button mouse hover event has been triggered");

				taskerMainWinToolTip.Show("Clicking on this will pause time tracking", tempItem.Owner, toolTipTimerShowInterval);

				isToolTipShown = true;
				toolTipTimer.Enabled = true;
				toolTipTimer.Start();
			}
		}

		private void PauseTrackingMenuButton_MouseMove(object sender, MouseEventArgs e)
		{
			if(pauseTrackingMenuButton.Enabled) {
				if(mouseMoveEnabled) {
					ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

					if (IsDebugModeEnabled)
						EventLog("Pause tracking menu button mouse move event has been triggered");

					if (isToolTipShown) {
						taskerMainWinToolTip.Hide(tempItem.Owner);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void PauseTrackingMenuButton_MouseEnter(object sender, EventArgs e)
		{
			if(pauseTrackingMenuButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Pause tracking menu button mouse enter event has been triggered");

				pauseTrackingMenuButton.ForeColor = SystemColors.ControlDarkDark;
				pauseTrackingMenuButton.BackColor = SystemColors.Control;
				pauseTrackingMenuButton.Font = menuMouseEnterFont;
			}
		}

		private void PauseTrackingMenuButton_MouseLeave(object sender, EventArgs e)
		{
			if(pauseTrackingMenuButton.Enabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Pause tracking menu button leave event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				if(tS == TrackerStates.Pause) {
					pauseTrackingMenuButton.ForeColor = SystemColors.ControlText;
					pauseTrackingMenuButton.BackColor = Color.Gold;
				}
				else {
					pauseTrackingMenuButton.ForeColor = SystemColors.ControlText;
					pauseTrackingMenuButton.BackColor = SystemColors.Control;
				}

				pauseTrackingMenuButton.Font = menuMouseLeaveFont;
			}
		}

		private void PauseTrackingMenuButton_DropDownClosed(object sender, EventArgs e)
		{
			if(pauseTrackingMenuButton.Enabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Pause tracking menu button drop down closewd event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}



		///////////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////



		//-- Stop Tracking Button
		private void StopTrackButton_MouseDown(object sender, MouseEventArgs e)
		{
			if(stopTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Stop tracking button mouse down event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(stopTrackButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				stopTrackButton.Focus();
				
				StopTasker(ButtonType.FORM);
			}
		}

		private void StopTrackButton_MouseHover(object sender, EventArgs e)
		{
			if(stopTrackButton.Enabled) { 
				if (IsDebugModeEnabled)
					EventLog("Stop Tracking Button mouse hover event has been triggered");

				taskerMainWinToolTip.Show("Clicking on this will stop time tracking", stopTrackButton, toolTipTimerShowInterval);

				isToolTipShown = true;
				toolTipTimer.Enabled = true;
				toolTipTimer.Start();
			}
		}

		private void StopTrackButton_MouseMove(object sender, MouseEventArgs e)
		{
			if(stopTrackButton.Enabled) {
				if(mouseMoveEnabled) {
					if (IsDebugModeEnabled)
						EventLog("Stop tracking button mouse move evnet has been triggered");

					if(isToolTipShown) {
						taskerMainWinToolTip.Hide(stopTrackButton);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void StopTrackButton_MouseEnter(object sender, EventArgs e)
		{
			if (stopTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Stop tracking button mouse enter event has been triggered");

				stopTrackButton.BackColor = Color.FromArgb(255, 192, 192);
			}
		}

		private void StopTrackButton_MouseLeave(object sender, EventArgs e)
		{
			if (stopTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Stop Tracking Button mouse leave event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(stopTrackButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				stopTrackButton.BackColor = Color.White;
			}
		}

		private void StopTrackButton_Leave(object sender, EventArgs e)
		{
			if(stopTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Stop tracking button leave event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(stopTrackButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void StopTrackButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if(stopTrackButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Stop track button key down event has been triggered");

				if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
					if (isToolTipShown) {
						taskerMainWinToolTip.Hide(stopTrackButton);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}

					StopTasker(ButtonType.FORM);
				}
			}
		}



		//////////////////////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////////////////////



		//-- Stop Tracking Menu Button
		private void StopTrackingMenuButton_MouseDown(object sender, MouseEventArgs e)
		{
			if(stopTrackingMenuButton.Enabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Stop tracking menu button mouse down event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				taskerMenuActionsItem.HideDropDown();
				
				StopTasker(ButtonType.MENU);
			}
		}

		private void StopTrackingMenuButton_MouseHover(object sender, EventArgs e)
		{
			if(stopTrackingMenuButton.Enabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Stop tracking menu button mouse hover event has been triggered");

				taskerMainWinToolTip.Show("Clicking on this will stop the time tracking", tempItem.Owner, toolTipTimerShowInterval);

				isToolTipShown = true;
				toolTipTimer.Enabled = true;
				toolTipTimer.Start();
			}
		}

		private void StopTrackingMenuButton_MouseMove(object sender, MouseEventArgs e)
		{
			if(stopTrackingMenuButton.Enabled) {
				if(mouseMoveEnabled) {
					ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

					if (IsDebugModeEnabled)
						EventLog("Stop tracking menu button mouse move event has been triggered");

					if (isToolTipShown) {
						taskerMainWinToolTip.Hide(tempItem.Owner);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void StopTrackingMenuButton_MouseEnter(object sender, EventArgs e)
		{
			if(stopTrackingMenuButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Stop tracking menu button mouse enter event has been triggered");

				stopTrackingMenuButton.ForeColor = SystemColors.ControlDarkDark;
				stopTrackingMenuButton.BackColor = SystemColors.Control;
				stopTrackingMenuButton.Font = menuMouseEnterFont;
			}
		}

		private void StopTrackingMenuButton_MouseLeave(object sender, EventArgs e)
		{
			if(stopTrackingMenuButton.Enabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Stop tracking menu button mouse leave event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				if(tS == TrackerStates.Stop) {
					stopTrackingMenuButton.ForeColor = SystemColors.ControlText;
					stopTrackingMenuButton.BackColor = Color.IndianRed;
				}
				else {
					stopTrackingMenuButton.ForeColor = SystemColors.ControlText;
					stopTrackingMenuButton.BackColor = SystemColors.Control;
				}

				stopTrackingMenuButton.Font = menuMouseLeaveFont;
			}
		}

		private void StopTrackingMenuButton_DropDownClosed(object sender, EventArgs e)
		{
			if(stopTrackingMenuButton.Enabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Stop tracking menu button leave event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}



		///////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////



		//-- Quit Tracking Button
		private void QuitButton_MouseDown(object sender, MouseEventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Quit tasker button mouse down event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(quitButton);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			quitButton.Focus();

			closeType = CloseType.FormQuitButton;
			NormalCloseTasker();
		}

		private void QuitButton_MouseHover(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Quit tasker button mouse hover event has been triggered");

			taskerMainWinToolTip.Show("Clicking on this will quit the tasker", quitButton, toolTipTimerShowInterval);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void QuitButton_MouseMove(object sender, MouseEventArgs e)
		{
			if(mouseMoveEnabled) {
				if (IsDebugModeEnabled)
					EventLog("Quit tasker button mouse move event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(quitButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void QuitButton_MouseEnter(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Quit tasker button mouse enter event has been triggered");

			quitButton.ForeColor = SystemColors.Window;
			quitButton.BackColor = Color.FromArgb(255, 100, 100);
		}

		private void QuitButton_MouseLeave(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Quit tasker button mouse leave event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(quitButton);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			quitButton.ForeColor = SystemColors.ControlText;
			quitButton.BackColor = Color.FromArgb(255, 128, 128);
		}

		private void QuitButton_Leave(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Quit tasker button leave event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(quitButton);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}

		private void QuitButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Quit tasker button key down has been triggered");

			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(quitButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				closeType = CloseType.FormQuitButton;
				NormalCloseTasker();
			}
		}


		///////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////



		//-- Quit Tracking Menu Button
		private void QuitMenuButton_MouseDown(object sender, MouseEventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Quit menu button mouse down event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			taskerMenuActionsItem.HideDropDown();

			closeType = CloseType.MenuQuitButton;		
			NormalCloseTasker();
		}

		private void QuitMenuButton_MouseHover(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Quit menu button mouse hover event has been triggered");

			taskerMainWinToolTip.Show("Clicking on this will quit the tasker", tempItem.Owner, toolTipTimerShowInterval);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void QuitMenuButton_MouseMove(object sender, MouseEventArgs e)
		{
			if(mouseMoveEnabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Quit menu button mouse move event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void QuitMenuButton_MouseEnter(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Quit menu button mouse enter event has been triggered");

			quitMenuButton.ForeColor = Color.FromArgb(255, 128, 128);
			quitMenuButton.Font = menuMouseEnterFont;
		}

		private void QuitMenuButton_MouseLeave(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Quit menu button mouse leave event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			quitMenuButton.ForeColor = SystemColors.ControlText;
			quitMenuButton.Font = menuMouseLeaveFont;
		}

		private void QuitMenuButton_DropDownClosed(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Quit menu button drop down closed event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}



		///////////////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////



		//-- Tasker Actions Menu Item
		private void TaskerMenuActionsItem_MouseDown(object sender, MouseEventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Tasker menu actions item mouse down event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}

		private void TaskerMenuActionsItem_MouseHover(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Tasker menu actions item mouse hover event has been triggered");

			taskerMainWinToolTip.Show("Start, Pause, and Stop tracking", tempItem.Owner, toolTipTimerShowInterval);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void TaskerMenuActionsItem_MouseMove(object sender, MouseEventArgs e)
		{
			if (mouseMoveEnabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Tasker menu actions item mouse move event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void TaskerMenuActionsItem_MouseEnter(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Tasker menu actions item mouse enter event has been triggered");

			taskerMenuActionsItem.ForeColor = SystemColors.ControlDarkDark;
			taskerMenuActionsItem.Font = menuMouseEnterFont;
		}

		private void TaskerMenuActionsItem_MouseLeave(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Tasker menu actions item mouse leave event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			taskerMenuActionsItem.ForeColor = SystemColors.ControlText;
			taskerMenuActionsItem.Font = menuMouseLeaveFont;
		}

		private void TaskerMenuActionsItem_DropDownClosed(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Tasker menu actions item drop down closed event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}

		private void TaskerMenuActionsItem_DropDownOpening(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Tasker menu actions item drop down opening event has been triggered");

			startTrackingMenuButton.Font = menuMouseLeaveFont;
			pauseTrackingMenuButton.Font = menuMouseLeaveFont;
			stopTrackingMenuButton.Font = menuMouseLeaveFont;
			exportLogMenuButton.Font = menuMouseLeaveFont;
			quitMenuButton.Font = menuMouseLeaveFont;
		}



		///////////////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////



		//-- Tasker Settings Menu Item
		private void TaskerMenuSettingsItem_MouseDown(object sender, MouseEventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Tasker menu settings item mouse down event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}

		private void TaskerMenuSettingsItem_MouseHover(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Tasker menu settings item mouse hover event has been triggered");

			taskerMainWinToolTip.Show("Export and Save Settings", tempItem.Owner, toolTipTimerShowInterval);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void TaskerMenuSettingsItem_MouseMove(object sender, MouseEventArgs e)
		{
			if (mouseMoveEnabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Tasker menu settings item mouse move event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void TaskerMenuSettingsItem_MouseEnter(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Tasker menu settings item mouse enter event has been triggered");

			taskerMenuSettingsItem.ForeColor = SystemColors.ControlDarkDark;
			taskerMenuSettingsItem.Font = menuMouseEnterFont;
		}

		private void TaskerMenuSettingsItem_MouseLeave(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Tasker menu settings item mouse leave event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			taskerMenuSettingsItem.ForeColor = SystemColors.ControlText;
			taskerMenuSettingsItem.Font = menuMouseLeaveFont;
		}

		private void TaskerMenuSettingsItem_DropDownClosed(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Tasker menu settings item drop down closed event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}

		private void TaskerMenuSettingsItem_DropDownOpening(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Tasker menu settings item drop down opening event has been triggered");

			settingsSaveOptionMenuButton.Font = menuMouseLeaveFont;
			logExportOptionsMenu.Font = menuMouseLeaveFont;
		}



		///////////////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////



		//-- Export Log Menu Button
		private void ExportLogMenuButton_MouseDown(object sender, MouseEventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Export log menu button mouse down event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			taskerMenuActionsItem.HideDropDown();

			lvOps.ManualExportListViewToExcel();
		}

		private void ExportLogMenuButton_MouseHover(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Export log menu button mouse hover event has been triggered");

			taskerMainWinToolTip.Show("Exports contents of task list to an excel file", tempItem.Owner, toolTipTimerShowInterval);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void ExportLogMenuButton_MouseMove(object sender, MouseEventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Export log menu button mouse move event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}

		private void ExportLogMenuButton_MouseEnter(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Export log menu button mouse enter event has been triggered");

			exportLogMenuButton.ForeColor = SystemColors.ControlDarkDark;
			exportLogMenuButton.Font = menuMouseEnterFont;
		}

		private void ExportLogMenuButton_MouseLeave(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Export log menu button mouse leave event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			exportLogMenuButton.ForeColor = SystemColors.ControlText;
			exportLogMenuButton.Font = menuMouseLeaveFont;
		}

		private void ExportLogMenuButton_DropDownClosed(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Export log menu button drop down closed event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}



		///////////////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////



		//-- Settings Save Option Menu Button
		private void SettingsSaveOptionMenuButton_MouseDown(object sender, MouseEventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Settings save option menu button mouse down event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			if (settingsSaveOptionMenuButton.CheckState == CheckState.Checked) {
				settingsSaveOptionMenuButton.CheckState = CheckState.Unchecked;
				settingsSaveOptionMenuButton.Checked = false;
				taskerSettings.SaveSettings = false;
				if (IsDebugModeEnabled)
					Debug("Menu save defaults button unchecked");
			}
			else {
				settingsSaveOptionMenuButton.CheckState = CheckState.Checked;
				settingsSaveOptionMenuButton.Checked = true;
				taskerSettings.SaveSettings = true;
				if (IsDebugModeEnabled)
					Debug("Menu save defaults button checked");
			}

			taskerMenuSettingsItem.HideDropDown();
		}

		private void SettingsSaveOptionMenuButton_MouseHover(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Settings save option menu button mouse hover event has been triggered");

			taskerMainWinToolTip.Show("If checked, settings will save upon exit", tempItem.Owner, toolTipTimerShowInterval);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void SettingsSaveOptionMenuButton_MouseMove(object sender, MouseEventArgs e)
		{
			if(mouseMoveEnabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Settings save option menu button mouse move event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void SettingsSaveOptionMenuButton_MouseEnter(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Settings save option menu button mouse enter event has been triggered");

			settingsSaveOptionMenuButton.ForeColor = SystemColors.ControlDarkDark;
			settingsSaveOptionMenuButton.Font = menuMouseEnterFont;
		}

		private void SettingsSaveOptionMenuButton_MouseLeave(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Settings save option menu button mouse leave event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			settingsSaveOptionMenuButton.ForeColor = SystemColors.ControlText;
			settingsSaveOptionMenuButton.Font = menuMouseLeaveFont;
		}

		private void SettingsSaveOptionMenuButton_DropDownClosed(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Settings save option menu button drop down closed event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}



		///////////////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////



		//-- Export Log Options Menu
		private void LogExportOptionsMenu_MouseDown(object sender, MouseEventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Log export options menu mouse down event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}

		private void LogExportOptionsMenu_MouseHover(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Log export options menu mouse hover event has been triggered");

			taskerMainWinToolTip.Show("Menu contains options for the log exports", tempItem.Owner, toolTipTimerShowInterval);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void LogExportOptionsMenu_MouseMove(object sender, MouseEventArgs e)
		{
			if (mouseMoveEnabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Log export options menu mouse move event has been triggered");

				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}

		private void LogExportOptionsMenu_MouseEnter(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Log export options menu mouse enter event has been triggered");

			logExportOptionsMenu.ForeColor = SystemColors.ControlDarkDark;
			logExportOptionsMenu.Font = menuMouseEnterFont;
		}

		private void LogExportOptionsMenu_MouseLeave(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Log export options menu mouse leave event has been triggered");

			taskerMainWinToolTip.Hide(tempItem.Owner);
			isToolTipShown = false;
			mouseMoveEnabled = false;
			toolTipTimer.Stop();

			logExportOptionsMenu.ForeColor = SystemColors.ControlText;
			logExportOptionsMenu.Font = menuMouseLeaveFont;
		}

		private void LogExportOptionsMenu_DropDownClosed(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Log export options menu drop down closed event has been triggered");

			taskerMainWinToolTip.Hide(tempItem.Owner);
			isToolTipShown = false;
			mouseMoveEnabled = false;
			toolTipTimer.Stop();
		}

		private void LogExportOptionsMenu_DropDownOpening(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Log export options menu drop down opening event has been triggered");

			autoExportTaskListMenuButton.Font = menuMouseLeaveFont;
			saveLocationExportMenuButton.Font = menuMouseLeaveFont;
		}




		///////////////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////



		//-- Auto Export Task List Menu Button
		private void AutoExportTaskListMenuButton_MouseDown(object sender, MouseEventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Auto export task list menu button mouse down event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			if(autoExportTaskListMenuButton.CheckState == CheckState.Checked) {
				autoExportTaskListMenuButton.CheckState = CheckState.Unchecked;
				autoExportTaskListMenuButton.Checked = false;
				taskerSettings.AutoExportTaskList = false;
				if (IsDebugModeEnabled)
					Debug("Menu auto export task list button unchecked");
			}
			else {
				autoExportTaskListMenuButton.CheckState = CheckState.Checked;
				autoExportTaskListMenuButton.Checked = true;
				taskerSettings.AutoExportTaskList = true;
				if (IsDebugModeEnabled)
					Debug("Menu auto export task list button checked");
			}

			logExportOptionsMenu.HideDropDown();
			taskerMenuSettingsItem.HideDropDown();
		}

		private void AutoExportTaskListMenuButton_MouseHover(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Auto export task list menu button mouse hover event has been triggered");

			taskerMainWinToolTip.Show("Enables auto export of task list to excel when tracking has ended", tempItem.Owner, toolTipTimerShowInterval);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void AutoExportTaskListMenuButton_MouseMove(object sender, MouseEventArgs e)
		{
			if (mouseMoveEnabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Auto export task list menu button mouse move event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void AutoExportTaskListMenuButton_MouseEnter(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Auto export task list menu button mouse enter has been triggered");

			autoExportTaskListMenuButton.ForeColor = SystemColors.ControlDarkDark;
			autoExportTaskListMenuButton.Font = menuMouseEnterFont;
		}

		private void AutoExportTaskListMenuButton_MouseLeave(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Auto export task list menu button mouse leave has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			autoExportTaskListMenuButton.ForeColor = SystemColors.ControlText;
			autoExportTaskListMenuButton.Font = menuMouseLeaveFont;
		}

		private void AutoExportTaskListMenuButton_DropDownClosed(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Auto export task list menu button drop down closed has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}




		///////////////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////



		//-- Set Export Save Location Menu Button
		private void SaveLocationExportMenuButton_MouseDown(object sender, MouseEventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Save location export menu button mouse down event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			logExportOptionsMenu.HideDropDown();
			taskerMenuSettingsItem.HideDropDown();

			if(saveLocationExportMenuButton.CheckState == CheckState.Checked) {
				saveLocationExportMenuButton.CheckState = CheckState.Unchecked;
				saveLocationExportMenuButton.Checked = false;
				taskerSettings.ExportSaveLocation = TaskerSettings.DefaultSettings.defaultExportSaveLocation;
				if (IsDebugModeEnabled)
					Debug("Auto export save location has been reset to default and setting unchecked");
			}
			else {
				saveLocationExportMenuButton.CheckState = CheckState.Checked;
				saveLocationExportMenuButton.Checked = true;

				SaveFileDialog saveDialog = new SaveFileDialog
				{
					Filter = "All files (*.*)|*.*",
					FilterIndex = 2,
					RestoreDirectory = true
				};

				DialogResult result = saveDialog.ShowDialog();
				if (result == DialogResult.OK) {
					if (IsDebugModeEnabled)
						Debug("Auto export save location is set to: " + saveDialog.FileName);

					taskerSettings.ExportSaveLocation = saveDialog.FileName;
				}
				else {
					if (IsDebugModeEnabled)
						Warn("User hit the cancel button. Setting export save location to default");

					taskerSettings.ExportSaveLocation = TaskerSettings.DefaultSettings.defaultExportSaveLocation;
				}
			}
		}

		private void SaveLocationExportMenuButton_MouseHover(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Save location export menu button mouse hover event has been triggered");

			taskerMainWinToolTip.Show("Sets the save location for the task list export", tempItem.Owner, toolTipTimerShowInterval);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void SaveLocationExportMenuButton_MouseMove(object sender, MouseEventArgs e)
		{
			if (mouseMoveEnabled) {
				ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

				if (IsDebugModeEnabled)
					EventLog("Save location export menu button mouse move event has been triggered");

				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(tempItem.Owner);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void SaveLocationExportMenuButton_MouseEnter(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Save location export menu button mouse enter event has been triggered");

			saveLocationExportMenuButton.ForeColor = SystemColors.ControlDarkDark;
			saveLocationExportMenuButton.Font = menuMouseEnterFont;
		}

		private void SaveLocationExportMenuButton_MouseLeave(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Save location export menu button mouse leave event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			saveLocationExportMenuButton.ForeColor = SystemColors.ControlText;
			saveLocationExportMenuButton.Font = menuMouseLeaveFont;
		}

		private void SaveLocationExportMenuButton_DropDownClosed(object sender, EventArgs e)
		{
			ToolStripMenuItem tempItem = (ToolStripMenuItem)sender;

			if (IsDebugModeEnabled)
				EventLog("Save location export menu button drop down closed event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(tempItem.Owner);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}
	}

	public partial class TaskEntryWindow
	{
		///////////////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////


		//-- Ok Button
		private void OkButton_MouseDown(object sender, MouseEventArgs e)
		{
			if (okButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Ok button mouse down event has been triggered");

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(okButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				okButton.Focus();

				Dictionary<TextBoxTypes, string> timeEntryTextBoxList = new Dictionary<TextBoxTypes, string>();
				timeEntryTextBoxList.Add(KeyText, keyEntryTextBox.Text);
				timeEntryTextBoxList.Add(TimeSpentText, timeSpentTextBox.Text);
				timeEntryTextBoxList.Add(DescText, descriptionTextBox.Text);

				(int textBoxState, Dictionary<TextBoxTypes, string> errorMsgList, Dictionary<TextBoxTypes, string> errorMsgTypeList) = GetTaskEntryTextBoxValidationStatus(timeEntryTextBoxList);
				ResetTimeEntryTextBoxErrorStates(this, textBoxState, errorMsgList, errorMsgTypeList);

				bool isTimeEntryGood;
				if (IsTimerEntryGood(textBoxState)) {
					isTimeEntryGood = true;
					AlertUserOfTimeEntryWindowErrors(textBoxState, errorMsgList, errorMsgTypeList);
				}
				else {
					isTimeEntryGood = false;
					AlertUserOfTimeEntryWindowErrors(textBoxState, errorMsgList, errorMsgTypeList);
				}

				if (isTimeEntryGood) {
					okButtonPressed = true;
					if (IsDebugModeEnabled)
						Debug("Hiding and closing task entry window");

					this.Hide();
					this.Close();
				}
			}
		}

		private void OkButton_MouseHover(object sender, EventArgs e)
		{
			if (okButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Ok Button mouse hover event has been triggered");

				taskEntryWindowToolTip.Show("Click on Ok Button to finish the task entry", okButton, toolTipTimerShowInterval);

				isToolTipShown = true;
				toolTipTimer.Enabled = true;
				toolTipTimer.Start();
			}
		}

		private void OkButton_MouseMove(object sender, MouseEventArgs e)
		{
			if (okButton.Enabled) {
				if (mouseMoveEnabled) {
					if (IsDebugModeEnabled)
						EventLog("Ok button mouse move event has been triggered");

					if(isToolTipShown) {
						taskEntryWindowToolTip.Hide(okButton);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void OkButton_MouseEnter(object sender, EventArgs e)
		{
			if(okButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Ok button mouse enter event has been triggered");
			}
		}

		private void OkButton_MouseLeave(object sender, EventArgs e)
		{
			if (okButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Ok Button mouse leave event has been triggered");

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(okButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void OkButton_Leave(object sender, EventArgs e)
		{
			if(okButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Ok button leave event has been triggered");

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(okButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void OkButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if(okButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Ok button preview key down has been triggered");

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(okButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				Dictionary<TextBoxTypes, string> timeEntryTextBoxList = new Dictionary<TextBoxTypes, string>();
				timeEntryTextBoxList.Add(KeyText, keyEntryTextBox.Text);
				timeEntryTextBoxList.Add(TimeSpentText, timeSpentTextBox.Text);
				timeEntryTextBoxList.Add(DescText, descriptionTextBox.Text);

				(int textBoxState, Dictionary<TextBoxTypes, string> errorMsgList, Dictionary<TextBoxTypes, string> errorMsgTypeList) = GetTaskEntryTextBoxValidationStatus(timeEntryTextBoxList);
				ResetTimeEntryTextBoxErrorStates(this, textBoxState, errorMsgList, errorMsgTypeList);

				bool isTimeEntryGood;
				if (IsTimerEntryGood(textBoxState)) {
					isTimeEntryGood = true;
					AlertUserOfTimeEntryWindowErrors(textBoxState, errorMsgList, errorMsgTypeList);
				}
				else {
					isTimeEntryGood = false;
					AlertUserOfTimeEntryWindowErrors(textBoxState, errorMsgList, errorMsgTypeList);
				}

				if(isTimeEntryGood) {
					okButtonPressed = true;
					if (IsDebugModeEnabled)
						Debug("Cancelling task entry tracking loop");

					entryTrackingLoop.CancelAsync();

					if (IsDebugModeEnabled)
						Debug("Hiding and closing task entry window");

					this.Hide();
					this.Close();
				}
			}
		}



		///////////////////////////////////////////////////////////////////////////////////////////////
		///////////////////////////////////////////////////////////////////////////////////////////////



		//-- Set Export Save Location Menu Button
		private void ClearEntryButton_MouseDown(object sender, MouseEventArgs e)
		{
			if (clearEntryButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Clear entry button mouse down event has been triggered");

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(clearEntryButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				clearEntryButton.Focus();
				
				keyEntryTextBox.Clear();
				timeSpentTextBox.Clear();
				descriptionTextBox.Clear();

				if (IsDebugModeEnabled)
					Debug("All text boxes have been cleared");
			}
		}

		private void ClearEntryButton_MouseHover(object sender, EventArgs e)
		{
			if (clearEntryButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Clear Entry Button mouse hover event has been triggered");

				taskEntryWindowToolTip.Show("Click on the Clear Entry Button to clear all the text boxes", clearEntryButton, toolTipTimerShowInterval);

				isToolTipShown = true;
				toolTipTimer.Enabled = true;
				toolTipTimer.Stop();
			}
		}

		private void ClearEntryButton_MouseMove(object sender, MouseEventArgs e)
		{
			if(clearEntryButton.Enabled) {
				if(mouseMoveEnabled) {
					if (IsDebugModeEnabled)
						EventLog("Clear entry button mouse move event has been triggered");

					if(isToolTipShown) {
						taskEntryWindowToolTip.Hide(clearEntryButton);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void ClearEntryButton_MouseEnter(object sender, EventArgs e)
		{
			if(clearEntryButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Clear entry button mouse move event has been triggered");

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(clearEntryButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void ClearEntryButton_MouseLeave(object sender, EventArgs e)
		{
			if (clearEntryButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Clear Entry Button mouse leave event has been triggered");

				if (isToolTipShown) {
					taskEntryWindowToolTip.Hide(clearEntryButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void ClearEntryButton_Leave(object sender, EventArgs e)
		{
			if(clearEntryButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Clear entry button leave event has been triggered");

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(clearEntryButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void ClearEntryButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if(clearEntryButton.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Clear entry button preview key down event has been triggered");

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(clearEntryButton);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				keyEntryTextBox.Clear();
				timeSpentTextBox.Clear();
				descriptionTextBox.Clear();

				if (IsDebugModeEnabled)
					Debug("All text boxes has been cleared");
			}
		}
	}
}