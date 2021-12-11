using System;
using System.Threading;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using static Tasker.TaskerVariables;
using static Tasker.LogCalls;
using static Tasker.TextBoxLibrary;

namespace Tasker
{
	public partial class TaskEntryWindow : Form
	{
		private readonly ErrorProvider keyEntryErrorProvider;
		private readonly ErrorProvider timeSpentErrorProvider;
		private readonly ErrorProvider descriptionErrorProvider;

		private readonly BackgroundWorker entryTrackingLoop;

		private bool isKeyEntryNull;
		private bool isTimeSpentEntryNull;
		private bool isDescriptionEntryNull;

		private bool taskEntryForceClose;
		private bool mainWindowForceClose;
		private bool okButtonPressed;

		private readonly TaskerMainWindow currentMainWin;

		private readonly ThreadStart forceCloseDelegate;
		private readonly Thread forceCloseCheckThread;

		private readonly Object forceCheckLock;

		private readonly ToolTip taskEntryWindowToolTip;
		private bool isToolTipShown;
		private bool mouseMoveEnabled;
		private readonly System.Windows.Forms.Timer toolTipTimer = new System.Windows.Forms.Timer();
		private const int toolTipAutoPopupDelay = 5000;
		private const int toolTipInitialDelay = 2500;
		private const int toolTipReshowDelay = 3000;
		private const int toolTipTimerInterval = 2600;
		private const int toolTipTimerShowInterval = 5000;

		public ref readonly ErrorProvider GetKeyEntryErrorProviderRef() => ref keyEntryErrorProvider;
		public ref readonly ErrorProvider GetTimeSpentErrorProviderRef() => ref timeSpentErrorProvider;
		public ref readonly ErrorProvider GetDescriptionErrorProviderRef() => ref descriptionErrorProvider;
		public ref readonly TextBox GetKeyEntryTextBoxRef() => ref keyEntryTextBox;
		public ref readonly TextBox GetTimeSpentTextBoxRef() => ref timeSpentTextBox;
		public ref readonly TextBox GetDescriptionTextBoxRef() => ref descriptionTextBox;

		public string GetKeyEntryText() => keyEntryTextBox.Text;
		public string GetTimeSpentText() => timeSpentTextBox.Text;
		public string GetDescriptionText() => descriptionTextBox.Text;

		public bool WasTimeEntryForceClosed() => taskEntryForceClose;

		public bool IsVisible
		{
			get
			{
				return this.Visible;
			}
		}

		public bool IsEntryTrackingLoopRunning
		{
			get
			{
				return entryTrackingLoop.IsBusy;
			}
		}

		public TaskEntryWindow(string timeBlock, TaskerMainWindow mainWindow)
		{
			InitializeComponent();

			this.AutoValidate = AutoValidate.EnableAllowFocusChange;

			isKeyEntryNull = true;
			isTimeSpentEntryNull = true;
			isDescriptionEntryNull = true;
			okButtonPressed = false;
			taskEntryForceClose = false;
			mainWindowForceClose = false;
			if(IsDebugModeEnabled)
				Debug("Setting task entry window state");

			entryTrackingLoop = new BackgroundWorker()
			{
				WorkerSupportsCancellation = true
			};
			entryTrackingLoop.DoWork += new DoWorkEventHandler(this.EntryTrackingLoop_DoWork);

			if(IsDebugModeEnabled)
				Debug("Setting up key entry error provider");

			keyEntryErrorProvider = new()
			{
				Icon = SystemIcons.Warning,
				BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError
			};
			keyEntryErrorProvider.SetIconAlignment(this.keyEntryTextBox, ErrorIconAlignment.MiddleRight);
			keyEntryErrorProvider.SetIconPadding(this.keyEntryTextBox, 2);
			keyEntryErrorProvider.SetError(this.keyEntryTextBox, String.Empty);
			
			if (IsDebugModeEnabled) {
				Debug("Key Entry Error Provider setup and started");

				Debug("Setting up time spent error provider");
			}

			timeSpentErrorProvider = new()
			{
				Icon = SystemIcons.Warning,
				BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError
			};
			timeSpentErrorProvider.SetIconAlignment(this.timeSpentTextBox, ErrorIconAlignment.MiddleRight);
			timeSpentErrorProvider.SetIconPadding(this.timeSpentTextBox, 2);
			timeSpentErrorProvider.SetError(this.timeSpentTextBox, String.Empty);
			
			if (IsDebugModeEnabled) {
				Debug("Time Spent Error Provider setup and started");

				Debug("Setting up description error provider");
			}

			descriptionErrorProvider = new()
			{
				Icon = SystemIcons.Warning,
				BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError
			};
			descriptionErrorProvider.SetIconAlignment(this.descriptionTextBox, ErrorIconAlignment.MiddleRight);
			descriptionErrorProvider.SetIconPadding(this.descriptionTextBox, 2);
			descriptionErrorProvider.SetError(this.descriptionTextBox, String.Empty);
			
			if(IsDebugModeEnabled)
				Debug("Description Error Provider setup and started");

			timeBlockLabel.Text = timeBlock;
			currentMainWin = mainWindow;

			forceCloseDelegate = new ThreadStart(delegate ()
			{
				CheckForceClose();
			});
			forceCloseCheckThread = new Thread(forceCloseDelegate);

			forceCheckLock = new object();

			taskEntryWindowToolTip = new ToolTip();
			SetupTaskEntryWindowToolTips();
		}

		private void TaskEntryWindow_Shown(object sender, EventArgs e)
		{
			if(IsDebugModeEnabled)
				Warn("Starting task entry tracking loop");

			entryTrackingLoop.RunWorkerAsync();
			forceCloseCheckThread.Start();
		}

		private void TaskEntryWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			if((e.CloseReason == CloseReason.UserClosing && !okButtonPressed) && !mainWindowForceClose) {
				if (IsDebugModeEnabled) {
					Info("Form close event for time entry has been triggered. Checking with user to make sure this is what they want to do");
				}
				DialogResult result = MessageBox.Show("Closing the Time Entry window prematurely will result in your time not getting posted to the list.\nAre you sure you want to continue?", "Time Entry Window Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

				if(result == DialogResult.No) {
					if(IsDebugModeEnabled)
						Debug("User is not continuing with Time Entry Window Close Procedure");

					e.Cancel = true;
				}
				else {
					if(IsDebugModeEnabled)
						Debug("Time entry loop has been forcefully closed out");

					entryTrackingLoop.CancelAsync();

					if(IsDebugModeEnabled)
						Warn("Entering default cancel values into entry fields");

					keyEntryTextBox.Text = "User Canceled Entry";
					timeSpentTextBox.Text = "N/A";
					descriptionTextBox.Text = "No description provided. User canceled entry";

					if(IsDebugModeEnabled)
						Debug("Hiding task entry window, normal process should close window out automatically");

					this.Hide();

					if(IsDebugModeEnabled)
						Debug("Setting force close flag to true");

					taskEntryForceClose = true;
				}
			}
			else if((e.CloseReason == CloseReason.UserClosing && !okButtonPressed) && mainWindowForceClose) {
				if (IsDebugModeEnabled) {
					Warn("Force closing currently open task entry window");
					Warn("Force close triggered by closing event on main window");
				}

				entryTrackingLoop.CancelAsync();
				entryTrackingLoop.Dispose();
			}
			else if((e.CloseReason == CloseReason.UserClosing && okButtonPressed) && !mainWindowForceClose) {
				if(IsDebugModeEnabled) {
					Info("Normal close initiated. Ok button was pressed with no force close event");
				}

				entryTrackingLoop.CancelAsync();
				entryTrackingLoop.Dispose();
			}
		}

		private void KeyEntryTextBox_Validating(object sender, CancelEventArgs e)
		{
			(bool isErrored, string errorMsg, string errorTypeMsg) = ValidateKeyTextEntry(keyEntryTextBox.Text);

			if (isErrored) {
				e.Cancel = true;
				keyEntryTextBox.Select(0, keyEntryTextBox.Text.Length);
				keyEntryErrorProvider.ResetError(keyEntryTextBox, errorMsg);
				if (IsDebugModeEnabled) {
					Warn("Key entry error provider triggered: ");
					Error(errorMsg + errorTypeMsg + " - " + keyEntryTextBox.Text);
				}
			}
			else {
				e.Cancel = false;
				keyEntryErrorProvider.ResetError(keyEntryTextBox, String.Empty);
				if(IsDebugModeEnabled)
					Warn("Key entry error provider canceled");
			}
		}

		private void TimeSpentTextBox_Validating(object sender, CancelEventArgs e)
		{
			(bool isErrored, string errorMsg, string errorTypeMsg) = ValidateTimeSpentTextEntry(timeSpentTextBox.Text);

			if(isErrored) {
				e.Cancel = true;
				timeSpentTextBox.Select(0, timeSpentTextBox.Text.Length);
				timeSpentErrorProvider.ResetError(timeSpentTextBox, errorMsg);
				if (IsDebugModeEnabled) {
					Warn("Time Spent error provider triggered: ");
					Error(errorMsg + errorTypeMsg + " - " + timeSpentTextBox.Text);
				}
			}
			else {
				e.Cancel = false;
				timeSpentErrorProvider.ResetError(timeSpentTextBox, String.Empty);
				if(IsDebugModeEnabled)
					Warn("Time Spent error provider canceled");
			}
		}

		private void DescriptionTextBox_Validating(object sender, CancelEventArgs e)
		{
			(bool isErrored, string errorMsg, string errorTypeMsg) = ValidateDescriptionTextEntry(descriptionTextBox.Text);

			if(isErrored) {
				e.Cancel = true;
				descriptionTextBox.Select(0, descriptionTextBox.Text.Length);
				descriptionErrorProvider.ResetError(descriptionTextBox, errorMsg);
				if (IsDebugModeEnabled) {
					Warn("Description error provider triggered: ");
					Error(errorMsg + errorTypeMsg + " - " + descriptionTextBox.Text);
				}
			}
			else {
				e.Cancel = false;
				descriptionErrorProvider.ResetError(descriptionTextBox, String.Empty);
				if(IsDebugModeEnabled)
					Warn("Description error provider canceled");
			}
		}

		private void EntryTrackingLoop_DoWork(object? sender, DoWorkEventArgs e)
		{
			if (sender == null)
				return;

			BackgroundWorker worker = (BackgroundWorker)sender;

			bool endBackgroundLoop = false;
			while(!endBackgroundLoop) {
				if(worker.CancellationPending) {
					e.Cancel = true;
					endBackgroundLoop = true;
				}
				else {
					if (true.EqualsAll(!isKeyEntryNull, !isTimeSpentEntryNull, !isDescriptionEntryNull)) {
						EnableDisableOKButton(true);
						EnableDisableClearButton(true);
					}
					else {
						EnableDisableOKButton(false);
						EnableDisableClearButton(false);
					}

					Thread.Sleep(15);
				}
			}
		}

		public void DisableVisibility()
		{
			this.Visible = false;
		}

		public void ForceCloseEntryTrackingLoop()
		{
			if(entryTrackingLoop.IsBusy && !entryTrackingLoop.CancellationPending) {
				entryTrackingLoop.CancelAsync();

				while (entryTrackingLoop.CancellationPending == true)
					System.Threading.Thread.Sleep(1);

				entryTrackingLoop.Dispose();
			}
			else {
				entryTrackingLoop.Dispose();
			}
		}

		private void EnableDisableOKButton(bool ed)
		{
			if(okButton.InvokeRequired) {
			   okButton.BeginInvoke((MethodInvoker)delegate ()
			   {
				   okButton.Enabled = ed;
			   });
			}
			else {
				okButton.Enabled = ed;
			}
		}

		private void EnableDisableClearButton(bool ed)
		{
			if (clearEntryButton.InvokeRequired) {
				clearEntryButton.BeginInvoke((MethodInvoker)delegate ()
				{
					clearEntryButton.Enabled = ed;
				});
			}
			else {
				clearEntryButton.Enabled = ed;
			}
		}

		private static bool IsTimerEntryGood(int textBoxState)
		{
			if(textBoxState != 222) {
				if(IsDebugModeEnabled)
					Warn("Time Entry has errors, process cannot continue - Status code is: " + textBoxState.ToString());

				return false;
			}

			if(IsDebugModeEnabled)
				Warn("Time Entry has no errors, process will continue - Status Code is: 222");

			return true;
		}

		private void CheckForceClose()
		{
			bool forceClose = false;
			while (!forceClose) {
				bool lockCheck;
				lock(forceCheckLock) {
					lockCheck = currentMainWin.IsForceCloseActive || currentMainWin.IsNormalCloseActive;
				}

				if (lockCheck) {
					entryTrackingLoop.CancelAsync();
					entryTrackingLoop.Dispose();

					lock (forceCheckLock) {
						taskEntryForceClose = false;
						mainWindowForceClose = true;
					}

					if (this.InvokeRequired) {
						this.BeginInvoke((MethodInvoker)delegate ()
						{
						   this.Hide();
						   this.Close();
						});
					}
					else {
						this.Hide();
						this.Close();
					}
					forceClose = true;
				}

				lock(forceCheckLock) {
					if (okButtonPressed)
						forceClose = true;
				}
			}
		}
	}
}