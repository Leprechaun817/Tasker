using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

using static Tasker.TaskerVariables;
using static Tasker.LogCalls;
using static Tasker.TextBoxTypes;
using static Tasker.TextBoxLibrary;

namespace Tasker
{
	public struct Entry
	{
		public string key;
		public string time;
		public int startTimestamp;
		public int endTimestamp;
		public string description;
		public TaskEntryWindow entryWindow;
	}

	public partial class TaskerMainWindow : Form
	{
		//-----------Enumerations---------------
		private enum TrackerStates : ushort
		{
			Start = 0,
			Running = 1,
			Pause = 2,
			Stop = 3,
			ForceClose = 4,
			NormalClose = 5
		}

		private enum ButtonType : ushort
		{
			FORM = 10,
			MENU = 20,
			NOTYPE = 30
		}

		private enum CloseType : ushort
		{ 
			FormClose = 0,
			FormQuitButton = 2,
			MenuQuitButton = 4,
			WinShutDown = 6,
			AppExitCall = 8,
			TskManClose = 10,
			NoCloseType = 12,
			NullCloseValue = 255
		}

		//------------Variables----------------
		//Error providers for text entry validation
		private readonly ErrorProvider startTimeErrorProvider = new ErrorProvider()
		{
			Icon = SystemIcons.Warning,
			BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError
		};
		private readonly ErrorProvider endTimeErrorProvider = new ErrorProvider()
		{
			Icon = SystemIcons.Warning,
			BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError
		};
		private readonly ErrorProvider remindIntervErrorProvider = new ErrorProvider()
		{
			Icon = SystemIcons.Warning,
			BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError
		};

		//Background worker that controls the time tracking loop that runs in the background of the main window
		private readonly BackgroundWorker timeTrackingLoop = new BackgroundWorker()
		{
			WorkerSupportsCancellation = true
		};

		private readonly TaskerSettings taskerSettings = new TaskerSettings();

		private readonly TimerStates tS = new TimerStates();

		private readonly ListViewOperations lvOps;
		private ListViewDebugWindow? lvDbgWin = null;

		private TaskEntryDebugButton? taskEntryDebug = null;

		private readonly object entryLock = new object();
		private readonly Queue<Entry> entryQueue = new Queue<Entry>();

		private Dictionary<TextBoxTypes, string> mainWindowTextBoxList = new Dictionary<TextBoxTypes, string>();
		private Dictionary<TextBoxTypes, string> errorMsgList = new Dictionary<TextBoxTypes, string>();
		private Dictionary<TextBoxTypes, string> errorTypeMsgList = new Dictionary<TextBoxTypes, string>();

		//Used for window resizing
		private int oldFormWidth;
		private int oldFormHeight;

		//Time tracking variables (Recorded in minutes)
		private int currentTimeInMins;
		private int startTimeInMins;
		private int endTimeInMins;
		private int timeIntervalInMins;

		//Helps to ensure that only one task entry window is generated during the minute time period that the current
		//time is cleanly divisible by the time interval
		private int entryTimeInMins;

		//Record and control each task entry
		private bool isTaskEntryDone;

		//Used when closing the application
		private CloseType closeType;
		private bool forceCloseActive;
		private bool normalCloseActive;

		private readonly ToolTip taskerMainWinToolTip = new ToolTip();
		private bool isToolTipShown;
		private bool mouseMoveEnabled;
		private readonly System.Windows.Forms.Timer toolTipTimer = new System.Windows.Forms.Timer();
		private const int toolTipAutoPopDelay = 5000;
		private const int toolTipInitialDelay = 2500;
		private const int toolTipReshowDelay = 3000;
		private const int toolTipTimerInterval = 2600;
		private const int toolTipTimerShowInterval = 5000;

		private bool mouseMoveDebugMsgTriggered = false;

		private readonly Font menuMouseEnterFont = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
		private readonly Font menuMouseLeaveFont = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

		private readonly TaskLog taskLog = new TaskLog();

		//-------------Get Functions-----------------
		//For use with validation functions contained in sparate file
		public ref readonly ErrorProvider GetStartTimeErrorProviderRef() => ref startTimeErrorProvider;
		public ref readonly ErrorProvider GetEndTimeErrorProviderRef() => ref endTimeErrorProvider;
		public ref readonly ErrorProvider GetRemindIntervErrorProviderRef() => ref remindIntervErrorProvider;
		public ref readonly TextBox GetStartTimeTextBoxRef() => ref startTimeTextBox;
		public ref readonly TextBox GetEndTimeTextBoxRef() => ref endTimeTextBox;
		public ref readonly TextBox GetRemindIntervTextBoxRef() => ref remindIntervTextBox;

		public bool IsForceCloseActive
		{
			get
			{
				lock(entryLock) {
					return forceCloseActive;
				}
			}
		}

		public bool IsNormalCloseActive
		{
			get
			{
				lock(entryLock) {
					return normalCloseActive;
				}
			}
		}

		public TaskerMainWindow()
		{
			InitializeComponent();

			if(IsDebugModeEnabled)
				Debug("Initial setup of main window");

			this.AutoValidate = AutoValidate.EnableAllowFocusChange;

			listView1.SetupListViewColumnHeaders();

			if(IsDebugModeEnabled)
				Debug("Setting up start time error provider");
			
			startTimeErrorProvider.SetIconAlignment(this.startTimeTextBox, ErrorIconAlignment.MiddleRight);
			startTimeErrorProvider.SetIconPadding(this.startTimeTextBox, 2);
			startTimeErrorProvider.SetError(this.startTimeTextBox, String.Empty);
			
			endTimeErrorProvider.SetIconAlignment(this.endTimeTextBox, ErrorIconAlignment.MiddleRight);
			endTimeErrorProvider.SetIconPadding(this.endTimeTextBox, 2);
			endTimeErrorProvider.SetError(this.endTimeTextBox, String.Empty);
			
			remindIntervErrorProvider.SetIconAlignment(this.remindIntervTextBox, ErrorIconAlignment.MiddleRight);
			remindIntervErrorProvider.SetIconPadding(this.remindIntervTextBox, 2);
			remindIntervErrorProvider.SetError(this.remindIntervTextBox, String.Empty);
			
			timeTrackingLoop.DoWork += new DoWorkEventHandler(this.TimeTrackingLoop_DoWork);

			taskerSettings.InitializeTaskerSettings();

			//Main Window Values
			this.Width = taskerSettings.MainWindowLength;
			this.Height = taskerSettings.MainWindowHeight;
			if(IsDebugModeEnabled)
				Debug("Default main window values have been set");

			listView1.Width = taskerSettings.ListViewLength;
			listView1.Height = taskerSettings.ListViewHeight;
			listView1.KeyColumnHeader!.Width = taskerSettings.KeyColumnWidth;
			listView1.TimeColumnHeader!.Width = taskerSettings.TimeColumnWidth;
			listView1.TimeLoggedColumnHeader!.Width = taskerSettings.TimeLoggedColumnWidth;
			listView1.DescriptionColumnHeader!.Width = taskerSettings.DescriptionColumnWidth;

			//Intialize mouse tracking for listView
			lvOps = new ListViewOperations(ref listView1);

			isToolTipShown = false;
			mouseMoveEnabled = false;

			closeType = CloseType.NullCloseValue;
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if ((e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.FormOwnerClosing) && closeType == CloseType.NullCloseValue) {
				closeType = CloseType.FormClose;
				NormalCloseTasker();
			}

			base.OnFormClosing(e);
		}

		private void TaskerMainwindow_FormLoad(object sender, EventArgs e)
		{
			SetupMainWindowToolTips();

			lvOps.IntitializeMouseTracking();

			//Save current width and height of window
			oldFormWidth = this.Width;
			oldFormHeight = this.Height;
			
			if(IsTaskListTestEnabled) {
				int i = 0;
				while(i < 25) {
					Entry entry = new Entry
					{
						key = "LSTVW0" + i.ToString(),
						time = ".5",
						description = "List View Debug Test Entry - " + i.ToString()
					};

					UpdateListView(entry);
					i++;
				}
			}

			if(IsLVMouseTrackDebugEnabled)
				lvDbgWin = new ListViewDebugWindow();

			if (IsTaskEntryDebugEnabled)
				taskEntryDebug = new TaskEntryDebugButton(this);
				

			//Set save settings option in menu
			if (IsDebugModeEnabled)
				Debug("Setting Save Times option in menu");

			if (taskerSettings.SaveSettings) {
				settingsSaveOptionMenuButton.CheckState = CheckState.Checked;
				settingsSaveOptionMenuButton.Checked = true;
				
				if (IsDebugModeEnabled)
					Debug("Save Times option is checked");
			}
			else {
				settingsSaveOptionMenuButton.CheckState = CheckState.Unchecked;
				settingsSaveOptionMenuButton.Checked = false;
				
				if (IsDebugModeEnabled)
					Debug("SaveTimes option is unchecked");
			}
			if (IsDebugModeEnabled)
				Debug("Setting save option on menu set");


			//Set AutoExportTaskList Setting in menu
			if (IsDebugModeEnabled)
				Debug("Setting Auto Export Task List option in menu");

			if(taskerSettings.AutoExportTaskList) {
				autoExportTaskListMenuButton.CheckState = CheckState.Checked;
				autoExportTaskListMenuButton.Checked = true;

				if (IsDebugModeEnabled)
					Debug("Auto Export Task List option is checked");
			}
			else {
				autoExportTaskListMenuButton.CheckState = CheckState.Unchecked;
				autoExportTaskListMenuButton.Checked = false;

				if (IsDebugModeEnabled)
					Debug("Auto Export Task List option is unchecked");
			}
			if (IsDebugModeEnabled)
				Debug("Auto Export Task List option on menu set");


			//Set SaveLocationExport setting in menu
			if (IsDebugModeEnabled)
				Debug("Setting Save Location Export option in menu");

			if(taskerSettings.ExportSaveLocation != TaskerSettings.DefaultSettings.defaultExportSaveLocation) {
				saveLocationExportMenuButton.CheckState = CheckState.Checked;
				saveLocationExportMenuButton.Checked = true;

				if (IsDebugModeEnabled) {
					Debug("Save Location export option is valid and shows checked on menu");
					Debug("Current Save Location export setting is: " + taskerSettings.ExportSaveLocation);
				}
			}
			else {
				saveLocationExportMenuButton.CheckState = CheckState.Unchecked;
				saveLocationExportMenuButton.Checked = false;
				taskerSettings.ExportSaveLocation = TaskerSettings.DefaultSettings.defaultExportSaveLocation;

				if(IsDebugModeEnabled)
					Debug("Save location export option is empty or not valid and doesn't show checked on menu");
			}
			if (IsDebugModeEnabled)
				Debug("Save Location Export option on menu is set");

			//Set start, end, and interval text boxes to current values
			startTimeTextBox.Text = taskerSettings.StartTimeValue;
			endTimeTextBox.Text = taskerSettings.EndTimeValue;
			remindIntervTextBox.Text = taskerSettings.IntervalTimeValue;

			//Set Initial Tracker State
			SetTrackerState(TrackerStates.Stop);

			closeType = CloseType.NullCloseValue;
			forceCloseActive = false;
			normalCloseActive = false;
			isTaskEntryDone = true;
		}

		private void TaskerMainWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.FormOwnerClosing || e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = false;
			}
			else if (e.CloseReason == CloseReason.ApplicationExitCall) {
				closeType = CloseType.AppExitCall;

				if (IsDebugModeEnabled)
					Fatal("Form is being forcibly closed by an application exit call");

				ForceFlushTaskEntryQueue();
				e.Cancel = false;
				ForceCloseTasker();

				if (lvDbgWin != null)
					lvDbgWin.QuitLVDebug = true;
			}
			else if (e.CloseReason == CloseReason.TaskManagerClosing) {
				closeType = CloseType.TskManClose;

				if (IsDebugModeEnabled)
					Fatal("Form is being forcibly closed by a call from the task manager");

				ForceFlushTaskEntryQueue();
				e.Cancel = false;
				ForceCloseTasker();

				if (lvDbgWin != null)
					lvDbgWin.QuitLVDebug = true;
			}
			else if (e.CloseReason == CloseReason.WindowsShutDown) {
				closeType = CloseType.WinShutDown;

				if (IsDebugModeEnabled)
					Fatal("Form is being forcibly closed by Windows due to Windows shut-down event");

				ForceFlushTaskEntryQueue();
				e.Cancel = false;
				ForceCloseTasker();

				if (lvDbgWin != null)
					lvDbgWin.QuitLVDebug = true;
			}
			else if (e.CloseReason == CloseReason.None) {
				closeType = CloseType.NoCloseType;

				if (IsDebugModeEnabled)
					Fatal("Form is being forcibly closed, but no reason can be determined");

				ForceFlushTaskEntryQueue();
				e.Cancel = false;
				ForceCloseTasker();

				if (lvDbgWin != null)
					lvDbgWin.QuitLVDebug = true;
			}
		}

		private void CopySelectCell_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left) {
				if(lvOps.IsRightClickActive.ToBool()) {
					(List<(int, int)> cells, int numOfCells) = lvOps.GetSelectedCells();

					if(numOfCells == 1) {
						(int cY, int cX) = cells[0];
						Clipboard.SetText(lvOps.GetCellValue(cY, cX));
					}

					//Reset right click active
					lvOps.SetRightClickActive = false;
				}
			}
		}

		private void TaskerMainWindow_ResizeEnd(object sender, EventArgs e)
		{
			if (this.Width != oldFormWidth || this.Height != oldFormHeight) {
				int newFormWidth = this.Width;
				int newFormHeight = this.Height;

				int listViewWidInc = newFormWidth - oldFormWidth;
				int listViewHgtInc = newFormHeight - oldFormHeight;

				listView1.BeginUpdate();
				{
					listView1.Width += listViewWidInc;
					listView1.Height += listViewHgtInc;
				}
				listView1.EndUpdate();

				oldFormWidth = newFormWidth;
				oldFormHeight = newFormHeight;

				lvOps.UpdateAfterColumnHeightResize(listView1.Height);

				taskerSettings.MainWindowLength = newFormWidth;
				taskerSettings.MainWindowHeight = newFormHeight;
				taskerSettings.ListViewLength = listView1.Width;
				taskerSettings.ListViewHeight = listView1.Height;
			}
			else {
				if(IsDebugModeEnabled)
					Warn("Resize event was triggered, but no actual change in size of window occurred");
			}
		}

		private void StartTimeTextBox_Validating(object sender, CancelEventArgs e)
		{
			(bool isErrored, string errorMsg, string errorTypeMsg) = ValidateStartTimeTextEntry(startTimeTextBox.Text);

			if(isErrored) {
				e.Cancel = true;
				startTimeTextBox.Select(0, startTimeTextBox.Text.Length);
				startTimeErrorProvider.ResetError(startTimeTextBox, errorMsg);

				if (IsDebugModeEnabled) {
					Warn("Start time error provider triggered: ");
					Error(errorMsg + errorTypeMsg + " - " + startTimeTextBox.Text);
				}
			}
			else {
				e.Cancel = false;
				startTimeErrorProvider.ResetError(startTimeTextBox, String.Empty);
				if(IsDebugModeEnabled)
					Warn("Start time error provider canceled");
			}

			Application.DoEvents();
		}

		private void EndTimeTextBox_Validating(object sender, CancelEventArgs e)
		{
			(bool isErrored, string errorMsg, string errorTypeMsg) = ValidateEndTimeTextEntry(endTimeTextBox.Text);

			if(isErrored) {
				e.Cancel = true;
				endTimeTextBox.Select(0, endTimeTextBox.Text.Length);
				endTimeErrorProvider.ResetError(endTimeTextBox, errorMsg);

				if (IsDebugModeEnabled) {
					Warn("End time error provider triggered: ");
					Error(errorMsg + errorTypeMsg + " - " + endTimeTextBox.Text);
				}
			}
			else {
				e.Cancel = false;
				endTimeErrorProvider.ResetError(endTimeTextBox, String.Empty);
				if(IsDebugModeEnabled)
					Warn("End time error provider canceled");
			}

			Application.DoEvents();
		}

		private void RemindIntervTextBox_Validating(object sender, CancelEventArgs e)
		{
			(bool isErrored, string errorMsg, string errorTypeMsg) = ValidateReminderIntervTextEntry(remindIntervTextBox.Text);

			if(isErrored) {
				e.Cancel = true;
				remindIntervTextBox.Select(0, remindIntervTextBox.Text.Length);
				remindIntervErrorProvider.ResetError(remindIntervTextBox, errorMsg);

				if (IsDebugModeEnabled) {
					Warn("Remind time error provider triggered: ");
					Error(errorMsg + errorTypeMsg + " - " + remindIntervTextBox.Text);
				}
			}
			else {
				e.Cancel = false;
				remindIntervErrorProvider.ResetError(remindIntervTextBox, String.Empty);
				if(IsDebugModeEnabled)
					Warn("Remind time error provider canceled");
			}

			Application.DoEvents();
		}

		private void TimeTrackingLoop_DoWork(object? sender, DoWorkEventArgs e)
		{
			BackgroundWorker worker = (BackgroundWorker)sender!;

			bool endBackgroundLoop = false;
			int numOfMinutes;
			if (IsDebugModeEnabled) {
				Info("Start Time: " + startTimeInMins.ToString());
				Info("Current Time: " + currentTimeInMins.ToString());
			}

			while(!endBackgroundLoop) {
				if (tS == TrackerStates.Start && currentTimeInMins < startTimeInMins) {
					if (IsDebugModeEnabled) {
						if (DateTime.Now.Second % 60 == 0)
							Heartbeat("Heartbeat - Tracking Starting In: " + (startTimeInMins - currentTimeInMins).ToString() + " minutes");
					}
				}
				else if (tS == TrackerStates.Start && currentTimeInMins >= startTimeInMins) {
					SetTrackerState(TrackerStates.Running);
				}
				else if (tS == TrackerStates.Running) {
					while (tS.TimerRunning) {
						numOfMinutes = currentTimeInMins - startTimeInMins;

						if (numOfMinutes % timeIntervalInMins == 0 && entryTimeInMins != numOfMinutes && tS != TrackerStates.Pause) {
							if (IsDebugModeEnabled) {
								Info("New task entry event was triggered");
								Debug("Task Entry triggered at: " + numOfMinutes.ToTimeString());
								Heartbeat("Task Heartbeat: " + DateTime.Now.ToLongTimeString());
							}
							entryTimeInMins = numOfMinutes;

							Entry newEntry = new();

							newEntry.startTimestamp = currentTimeInMins - timeIntervalInMins;
							newEntry.endTimestamp = currentTimeInMins;
							newEntry.entryWindow = new TaskEntryWindow(newEntry.startTimestamp.ToTimeString() + " - " + newEntry.endTimestamp.ToTimeString(), this);

							if (IsDebugModeEnabled) {
								Info("New task entry generated");
								Warn("Task Entry Start: " + newEntry.startTimestamp.ToString());
								Warn("Task Entry End: " + newEntry.endTimestamp.ToString());

								Debug("Sending task entry to entry queue");
							}
							AddEntryToEntryQueue(newEntry);

							if(IsDebugModeEnabled)
								Debug("Starting task entry background worker to process queue");

							if (CheckTaskEntryState() == true && !forceCloseActive && !normalCloseActive) {
								ThreadStart taskEntryDelegate = new ThreadStart(delegate ()
								{
									TaskEntryWorker();
								});

								Thread taskEntryThread = new Thread(taskEntryDelegate);
								taskEntryThread.Start();
							}
							else if(forceCloseActive || normalCloseActive) {
								e.Cancel = true;
								endBackgroundLoop = true;
								break;
							}
						}
						else if (tS == TrackerStates.Pause) {
							if (IsDebugModeEnabled) {
								if (DateTime.Now.Second % 60 == 0)
									Heartbeat("Heartbeat - Tracking Paused - " + DateTime.Now.ToLongTimeString());
							}
						}
						else if(tS.EqualsAny(TrackerStates.Running, TrackerStates.Pause) && currentTimeInMins > endTimeInMins) {
							if (IsDebugModeEnabled)
								Debug("Current time is greater than end time, stopping tracker");

							SetTrackerState(TrackerStates.Stop);
						}
						else {
							if (IsDebugModeEnabled) {
								if (DateTime.Now.Second % 60 == 0)
									Heartbeat("Heartbeat: " + DateTime.Now.ToLongTimeString());
							}
						}

						Thread.Sleep(1000);

						currentTimeInMins = GetCurrentTime();
					}
				}
				else if(tS == TrackerStates.Stop) {
					e.Cancel = true;
					endBackgroundLoop = true;

				}
			}
		}

		private void TaskEntryWorker()
		{
			SetTaskEntryState(false);

			int entryQueueSize = GetEntryQueueSize();
			if(entryQueueSize > 0) {
				while(entryQueueSize > 0) {
					Entry currentEntry = GetEntryFromEntryQueue();
					
					ref TaskEntryWindow entryWin = ref currentEntry.entryWindow;
					entryWin.Enabled = true;
					entryWin.ShowInTaskbar = true;

					entryWin.FlashNotification();
					entryWin.ShowDialog();

					currentEntry.key = entryWin.GetKeyEntryText();
					currentEntry.time = entryWin.GetTimeSpentText();
					currentEntry.description = entryWin.GetDescriptionText();

					if (entryWin.WasTimeEntryForceClosed()) {
						entryWin.Close();
						entryWin.Dispose();
					}
					else
						entryWin.Dispose();

					if (IsDebugModeEnabled) {
						Warn("Entry Validation: ");
						Warn("Current Entry Key: " + currentEntry.key);
						Warn("Current Entry Time: " + currentEntry.time);
						Warn("Current Entry Description: " + currentEntry.description);
					}

					UpdateListView(currentEntry);

					if (IsTaskLoggingEnabled) {
						taskLog.UpdateTaskLogBuffer(currentEntry.key);
						taskLog.UpdateTaskLogBuffer(currentEntry.time);
						taskLog.UpdateTaskLogBuffer(currentEntry.description);
					}

					entryQueueSize--;
					if(forceCloseActive || normalCloseActive) {
						entryQueueSize = -1;
					}
					else if (entryQueueSize < GetEntryQueueSize()) {
						entryQueueSize += GetEntryQueueSize() - entryQueueSize;
					}
					else if (entryQueueSize > 0 && GetEntryQueueSize() == 0)
						entryQueueSize = 0;
				}
			}

			SetTaskEntryState(true);
		}

		private void StartTasker(ButtonType bT)
		{
			if (IsDebugModeEnabled) {
				switch (bT) {
					case ButtonType.FORM:
						Info("Start tasker event triggered using form button");
						break;
					case ButtonType.MENU:
						Info("Start tasker event triggered using menu button");
						break;
				}

				Info("Setting tracker state - Starting");
			}

			bool previousPaused = false;
			bool timerEnabled;
			if (tS != TrackerStates.Pause) {
				SetTrackerState(TrackerStates.Start);

				//Checking text boxes for issues with input
				if (mainWindowTextBoxList.Count != 0)
					mainWindowTextBoxList.Clear();

				mainWindowTextBoxList.Add(StartText, taskerSettings.StartTimeValue);
				mainWindowTextBoxList.Add(EndText, taskerSettings.EndTimeValue);
				mainWindowTextBoxList.Add(RemindText, taskerSettings.IntervalTimeValue);

				int textBoxState;
				ResetMainWindowTextBoxErrorStates(this, ((textBoxState, errorMsgList, errorTypeMsgList) = GetMainWindowTextBoxValidationStatus(mainWindowTextBoxList)).ToTuple());

				if (CanTimerBeStarted(textBoxState)) {
					timerEnabled = true;
					AlertUserOfMainWindowErrors(textBoxState, errorMsgList, errorTypeMsgList);
				}
				else {
					timerEnabled = false;
					AlertUserOfMainWindowErrors(textBoxState, errorMsgList, errorTypeMsgList);
				}
			}
			else {
				previousPaused = true;
				timerEnabled = true;
				SetTrackerState(TrackerStates.Start);
			}

			//If all text boxes were filled in properly, then begin timer loop
			if(timerEnabled) {
				if (IsDebugModeEnabled) {
					Info("No text box input errors");
					Debug("Ensuring time loop hasn't been started and isn't running");
				}
				if(tS == TrackerStates.Start && !previousPaused) {
					if (IsDebugModeEnabled) {
						Warn("Timer is preparing to start");
						Warn("Getting current time values");
					}

					//Get current time in integer form
					currentTimeInMins = GetCurrentTime();

					//Get the start and stop times, and timer interval from the text boxes
					startTimeInMins = GetStartTimeInMinutes();
					endTimeInMins = GetEndTimeInMinutes();
					timeIntervalInMins = GetTimeIntervalInMinutes();

					//Set entry time
					entryTimeInMins = currentTimeInMins;

					if(IsDebugModeEnabled)
						Debug("Checking current time against end time");

					if(currentTimeInMins < endTimeInMins) {
						if (IsDebugModeEnabled) {
							Debug("Attempting to start timer loop. Setting up times");
							Debug("Current time values: ");
							Debug("Start time in integer form: " + startTimeInMins.ToString());
							Debug("End time in integer form: " + endTimeInMins.ToString());
							Debug("Time interval in integer form: " + timeIntervalInMins.ToString());
							Debug("Current time is: " + entryTimeInMins.ToString());
						}

						//Starting the asynchronous loop
						if(timeTrackingLoop.IsBusy != true) {
							if(IsDebugModeEnabled)
								Warn("Starting time tracking loop background worker");

							timeTrackingLoop.RunWorkerAsync();
						}
						else {
							//If time tracking was already running, cancel it, and start it again
							if(IsDebugModeEnabled)
								Warn("Restarting time tracking loop background worker");

							timeTrackingLoop.CancelAsync();

							if(timeTrackingLoop.IsBusy == true) {
								if (IsDebugModeEnabled) {
									Warn("Previous time tracking loop session is still running");
									Warn("Waiting until previous session ends before starting new one");
								}

								timeTrackingLoop.CancelAsync();
								while (timeTrackingLoop.IsBusy) {
									System.Threading.Thread.Sleep(1);
									Application.DoEvents();
								}

								timeTrackingLoop.RunWorkerAsync();
							}
						}

						if(IsDebugModeEnabled)
							Warn("\nTask Tracker start Timestamp (in Minutes): " + currentTimeInMins.ToString());
					}
					else {
						if (IsDebugModeEnabled) {
							Error("Current time is past the stated end time");
							Error("Current Time: " + currentTimeInMins.ToString());
							Error("End Time: " + GetEndTimeInMinutes());
						}

						SetTrackerState(TrackerStates.Stop);
						mainWindowTextBoxList.Clear();
						MessageBox.Show("Current time is already past the end time. Plase enter a later stop time", "Tasker Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}
				else if(tS == TrackerStates.Start && previousPaused) {
					if(currentTimeInMins < endTimeInMins) {
						SetTrackerState(TrackerStates.Running);
						if (IsDebugModeEnabled) {
							Info("Tracker has been restarted");

							Warn("\nTasker Restart Timestamp (in Minutes): " + currentTimeInMins.ToString());
						}
					}
				}
			}
			else {
				SetTrackerState(TrackerStates.Stop);
				
				if(IsDebugModeEnabled)
					Error("A null or empty condition exists in one of the text boxes");
			}
		}

		private void PauseTasker(ButtonType bT)
		{
			if (IsDebugModeEnabled) {
				switch (bT) {
					case ButtonType.FORM:
						Info("Pause tasker event triggered using form button");
						break;
					case ButtonType.MENU:
						Info("Pause tasker event triggered using menu button");
						break;
				}
			}

			SetTrackerState(TrackerStates.Pause);

			if (IsDebugModeEnabled)
				Warn("\nTasker Pause Timestamp (in Minutes): " + currentTimeInMins.ToString());
		}

		private void StopTasker(ButtonType bT)
		{
			if (IsDebugModeEnabled) {
				switch (bT) {
					case ButtonType.FORM:
						Info("Stop tasker event triggered using form button");
						break;
					case ButtonType.MENU:
						Info("Stop tasker event triggered using menu button");
						break;
				}
			}

			SetTrackerState(TrackerStates.Stop);

			//Clear main window text box list
			mainWindowTextBoxList.Clear();

			if(IsDebugModeEnabled)
				Warn("\nTasker Stop Timestamp (in Minutes): " + currentTimeInMins.ToString());

			if(taskerSettings.AutoExportTaskList && currentTimeInMins > endTimeInMins) {
				FileStream excelExportStream = new FileStream(taskerSettings.ExportSaveLocation, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4, FileOptions.SequentialScan);
				excelExportStream.Seek(0, SeekOrigin.Begin);
				lvOps.AutoExportListViewToExcel(excelExportStream);
				excelExportStream.Close();
				excelExportStream.Dispose();
			}
		}

		private void NormalCloseTasker()
		{
			if (IsDebugModeEnabled) {
				switch (closeType) {
					case CloseType.FormClose:
						Warn("Normal close event triggered using the form close button");
						break;
					case CloseType.FormQuitButton:
						Warn("Normal close event triggered using the form quit button");
						break;
					case CloseType.MenuQuitButton:
						Warn("Normal close event triggered using the menu quit button");
						break;
				}
			}

			if(IsDebugModeEnabled)
				Info("Confirming exit or quit command with user");

			DialogResult result;
			if (listView1.Items.Count > 0) {
				if(IsDebugModeEnabled)
					Debug("One or more task logs are in the task list");

				result = MessageBox.Show("Contents of Task Log will be lost.\nAre you sure you want to quit?", "Tasker Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
			}
			else {
				if(IsDebugModeEnabled)
					Debug("No task logs were in the task list");

				result = MessageBox.Show("Are you sure you want to quit?", "Tasker Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
			}

			bool resumeClose;
			if(result == DialogResult.Yes) {
				if (GetEntryQueueSize() != 0) {
					if (IsDebugModeEnabled) {
						Warn("There are still " + GetEntryQueueSize().ToString() + " in the queue");
						Warn("Confirming with user that they still want to quit");
					}

					if (MessageBox.Show("You still have tasks in the queue. Do you want to finish entering those in?", "Tasker Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
						resumeClose = false;

						if (IsDebugModeEnabled) {
							Info("User confirms that they want to finish entering in task to task list");
							Info("Will not be continuing with close out");
						}
					}
					else {
						if (IsDebugModeEnabled) {
							Info("User confirms they aren't entering remaining task into list");
							Info("Continuing with close");
						}

						resumeClose = true;
						normalCloseActive = false;
						forceCloseActive = true;
						ForceFlushTaskEntryQueue();

						if(IsDebugModeEnabled)
							Warn("Task entry queue has now been flushed");
					}
				}
				else {
					if(IsDebugModeEnabled)
						Info("No task entries remaining in queue. Continuing with close process");

					resumeClose = true;
					normalCloseActive = true;
					forceCloseActive = false;
				}
			}
			else {
				if(IsDebugModeEnabled)
					Info("User cancelled out of close process. Will not be continuing with close out");

				resumeClose = false;
			}

			if (resumeClose) {
				SetTrackerState(TrackerStates.NormalClose);

				if (IsDebugModeEnabled) {
					Warn("\nTasker Normal Close Timestamp (in Minutes): " + currentTimeInMins.ToString());

					if (IsLVMouseTrackDebugEnabled) {
						if (lvDbgWin != null) {
							Debug("List view debug window is active. Closing debug window out");

							lvDbgWin.QuitLVDebug = true;
						}
					}

					if(IsTaskEntryDebugEnabled) {
						if(taskEntryDebug != null) {
							Debug("Task entry debug button window is active. Closing button window out");

							taskEntryDebug.QuitTaskEntryDbgBtn = true;
						}
					}

					Debug("Cancelling and disposing of resources used by background worker");
				}

				CancelAndDisposeBackgroundWorker();

				if (IsDebugModeEnabled)
					Debug("Checking for change to SaveSettingsOnClose setting");

				if (taskerSettings.SaveSettings) {
					taskerSettings.SaveTaskerSettings();
				}

				if (closeType != CloseType.FormClose) {
					this.Hide();
					this.Close();
				}
			}
		}

		private void ForceCloseTasker()
		{
			if (IsDebugModeEnabled) {
				switch (closeType) {
					case CloseType.AppExitCall:
						Warn("Force close event triggered by an application exit call");
						break;
					case CloseType.TskManClose:
						Warn("Force close event triggered by a force close call from task manager");
						break;
					case CloseType.WinShutDown:
						Warn("Force close event triggered by a windows shut down event");
						break;
					case CloseType.NoCloseType:
						Warn("Force close event triggered by a call or event that could not be determined");
						break;
				}
			}

			SetTrackerState(TrackerStates.ForceClose);
			if(IsDebugModeEnabled)
				Warn("\nTasker Force Close Timestamp (in Minutes): " + currentTimeInMins.ToString());

			SaveTaskerProgramState();

			CancelAndDisposeBackgroundWorker();

			if(IsDebugModeEnabled)
				Debug("Checking for change to SaveSettingsOnClose setting");
			
			if (taskerSettings.SaveSettings) {
				taskerSettings.SaveTaskerSettings();
			}
		}

		private void CancelAndDisposeBackgroundWorker()
		{
			if (IsDebugModeEnabled)
				Debug("Checking if Time Tracking Loop is still running");

			if(timeTrackingLoop.IsBusy && !timeTrackingLoop.CancellationPending) {
				if (IsDebugModeEnabled)
					Debug("Time Tracking Loop is still running. Sending cancel message to background worker");

				timeTrackingLoop.CancelAsync();

				if (IsDebugModeEnabled)
					Debug("Disposing of resources used by the Time Tracking Loop background worker");

				timeTrackingLoop.Dispose();
			}
			else {
				if (IsDebugModeEnabled)
					Debug("Time Tracking Loop is not running. Disposing of resources used by the Time Tracking Loop background worker");

				timeTrackingLoop.Dispose();
			}
		}

		private void SaveTaskerProgramState()
		{
			//Save timer states
			byte[] timeStateArr = tS.SerializeToByteArray();
			Debug(timeStateArr.ToString()!);

			//Save entry queue
			byte[] entryQueueArr = entryQueue.SerializeToByteArray();
			Debug(entryQueueArr.ToString()!);

			//Save listViewOperations state
			byte[] lvOpsArr = lvOps.SerializeToByteArray();
			Debug(lvOpsArr.ToString()!);

			//Save error provider states
			string startTimeTextBoxErrorState = startTimeErrorProvider.GetError(startTimeTextBox);
			string endTimeTextBoxErrorState = endTimeErrorProvider.GetError(endTimeTextBox);
			string remindTimeTextBoxErrorState = remindIntervErrorProvider.GetError(remindIntervTextBox);
		}

		private int GetStartTimeInMinutes()
		{
			return ((DateTime.Parse(taskerSettings.StartTimeValue).Hour * 60) + DateTime.Parse(taskerSettings.StartTimeValue).Minute);
		}

		private int GetEndTimeInMinutes()
		{
			return ((DateTime.Parse(taskerSettings.EndTimeValue).Hour * 60) + DateTime.Parse(taskerSettings.EndTimeValue).Minute);
		}

		private int GetTimeIntervalInMinutes()
		{
			const float hoursInMinutes = 60.0f;
			if(!Int32.TryParse(taskerSettings.IntervalTimeValue, out int returnValue)) {
				float remindFloatValue = float.Parse(taskerSettings.IntervalTimeValue);
				remindFloatValue *= hoursInMinutes;
				returnValue = (int)Math.Round(remindFloatValue, 0);
			}

			return returnValue;
		}

		private static int GetCurrentTime()
		{
			return ((DateTime.Now.Hour * 60) + DateTime.Now.Minute);
		}

		private void UpdateListView(Entry entry)
		{
			if(listView1.InvokeRequired) {
				listView1.BeginInvoke((MethodInvoker)delegate ()
				{
					if(IsDebugModeEnabled)
						Debug("Adding time entry into list view");

					string startTimeBlock = entry.startTimestamp.ToTimeString();
					string endTimeBlock = entry.endTimestamp.ToTimeString();
					string timeLogged = startTimeBlock + " - " + endTimeBlock;

					listView1.BeginUpdate();
					{
						listView1.Items.Add(new ListViewItem(new String[] { entry.key, entry.time.ToString(), timeLogged, entry.description }));
					}
					listView1.EndUpdate();

					lvOps.AddNewRow();
				});
			}
			else {
				if(IsDebugModeEnabled)
					Debug("Adding time entry into list view");

				string startTimeBlock = entry.startTimestamp.ToTimeString();
				string endTimeBlock = entry.endTimestamp.ToTimeString();
				string timeLogged = startTimeBlock + " - " + endTimeBlock;

				listView1.BeginUpdate();
				{
					listView1.Items.Add(new ListViewItem(new String[] { entry.key, entry.time.ToString(), timeLogged, entry.description }));
				}
				listView1.EndUpdate();

				lvOps.AddNewRow();
			}
		}

		private void AddEntryToEntryQueue(Entry e)
		{
			lock(entryLock) 
			{
				entryQueue.Enqueue(e);
				if(IsDebugModeEnabled)
					Debug("Task Entry -> " + e.startTimestamp.ToString() + " - " + e.endTimestamp.ToString() + " queued");
			}
		}

		private Entry GetEntryFromEntryQueue()
		{
			Entry returnEntry;
			lock(entryLock) 
			{
				returnEntry = entryQueue.Dequeue();
				if(IsDebugModeEnabled)
					Debug("Task Entry -> " + returnEntry.startTimestamp.ToString() + " - " + returnEntry.endTimestamp.ToString() + " dequeued");
			}

			return returnEntry;
		}

		private int GetEntryQueueSize()
		{
			int size;
			lock(entryLock) 
			{
				size = entryQueue.Count;
			}

			return size;
		}

		private void ForceFlushTaskEntryQueue()
		{
			//Close any open task entry screens
			while(GetEntryQueueSize() != 0) {
				Entry entry = GetEntryFromEntryQueue();
				ForceCloseTaskEntryWindow(ref entry.entryWindow);
				entry.entryWindow.Dispose();
				if(IsDebugModeEnabled)
					Warn("Task Entry: " + entry.startTimestamp.ToString() + " - " + entry.endTimestamp.ToString() + " has been force closed with no user input");

				entry.key = "FRCCLOSE";
				entry.time = "0";
				entry.description = "This entry was force closed and flushed from the queue with no user input";

				UpdateListView(entry);
			}
		}

		private static void ForceCloseTaskEntryWindow(ref TaskEntryWindow taskEntryWin)
		{
			if(IsDebugModeEnabled)
				Debug("Checking if current instance of task entry window is valid object");

			if (taskEntryWin != null) {
				if (IsDebugModeEnabled) {
					Debug("Current instance of task entry window is valid");

					Debug("Checking visibility of current task entry window");
				}

				bool wasVisible = false;
				if (taskEntryWin.IsVisible) {
					if(IsDebugModeEnabled)
						Debug("Current task entry window is visible. Setting it to not be visible");

					taskEntryWin.DisableVisibility();
					wasVisible = true;
				}

				if(IsDebugModeEnabled)
					Debug("Checking if entry tracking loop is running");

				if (taskEntryWin.IsEntryTrackingLoopRunning) {
					if(IsDebugModeEnabled)
						Debug("Entry tracking loop is running. Will now attempt to force close tracking loop");

					taskEntryWin.ForceCloseEntryTrackingLoop();
				}

				if (wasVisible)
					taskEntryWin.Hide();

				taskEntryWin.Close();
			}
		}

		private static string GetEnableState(bool state)
		{
			if (state)
				return "Enabled";

			return "Disabled";
		}

		private static bool CanTimerBeStarted(int textBoxState)
		{
			if(textBoxState != 222) {
				if(IsDebugModeEnabled)
					Warn("Timer will not be enabled - Status Code is: " + textBoxState.ToString());

				return false;
			}

			if(IsDebugModeEnabled)
				Warn("Timer will be enabled - Status Code is: 222");

			return true;
		}

		private bool CheckTaskEntryState()
		{
			lock(entryLock) {
				return isTaskEntryDone;
			}
		}

		private void SetTaskEntryState(bool status)
		{
			lock (entryLock) {
				isTaskEntryDone = status;
			}
		}
	}
}
