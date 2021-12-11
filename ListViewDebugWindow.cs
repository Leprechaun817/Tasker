using System;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

using static Tasker.TaskerVariables;
using static Tasker.LogCalls;

namespace Tasker
{
	public class ListViewDebugWindow
	{
		private int curXCoord = 0;
		private int curYCoord = 0;
		private int currentRow = 0;
		private int curRowLength = 0;
		private int curRowHeight = 0;
		private int currentColumn = 0;
		private int curColumnWidth = 0;
		private int curColumnHeight = 0;

		private bool quit = false;

		private readonly Form lvDebugForm = new Form();
		private readonly Label lvDebugLabel = new Label();

		private readonly object debugLock = new object();
		private readonly ThreadStart lvDebugWindowDelegate;
		private readonly Thread lvDebugWindowUpdateThread;

		private readonly ToolTip lvDebugWindowToolTip = new ToolTip();
		private bool isToolTipShown = false;
		private bool mouseMoveEnabled = false;
		private readonly System.Windows.Forms.Timer toolTipTimer = new System.Windows.Forms.Timer();
		private const int toolTipAutoPopDelay = 5000;
		private const int toolTipInitialDelay = 2500;
		private const int toolTipReshowDelay = 3000;
		private const int toolTipTimerInterval = 2600;
		private const int toolTipTimerShowInterval = 5000;

		public int XCoord
		{
			set
			{
				lock(debugLock) {
					curXCoord = value;
				}
			}
		}

		public int YCoord
		{
			set
			{
				lock(debugLock) {
					curYCoord = value;
				}
			}
		}

		public int CurrentRow
		{
			set
			{
				lock(debugLock) {
					currentRow = value;
				}
			}
		}

		public int CurRowLength
		{
			set
			{
				lock(debugLock) {
					curRowLength = value;
				}
			}
		}

		public int CurRowHeight
		{
			set
			{
				lock(debugLock) {
					curRowHeight = value;
				}
			}
		}

		public int CurrentColumn
		{
			set
			{
				lock(debugLock) {
					currentColumn = value;
				}
			}
		}

		public int CurColumnWidth
		{
			set
			{
				lock(debugLock) {
					curColumnWidth = value;
				}
			}
		}

		public int CurColumnHeight
		{
			set
			{
				lock(debugLock) {
					curColumnHeight = value;
				}
			}
		}

		public bool QuitLVDebug
		{
			set
			{
				lock(debugLock) {
					quit = value;
				}
			}
		}

		public ListViewDebugWindow()
		{
			SetupListViewDebugWindowToolTips();

			lvDebugLabel.CausesValidation = false;
			lvDebugLabel.FlatStyle = FlatStyle.Flat;
			lvDebugLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			lvDebugLabel.Location = new Point(1, 1);
			lvDebugLabel.Name = "LVDebugWindow";
			lvDebugLabel.Size = new Size(155, 181);
			lvDebugLabel.TabIndex = 0;
			lvDebugLabel.Visible = true;
			lvDebugLabel.Text = "X Coordinate: 0" + "\n" +
								"Y Coordinate: 0" + "\n" +
								"Current Row: 0" + "\n" +
								"Row Length: 0" + "\n" +
								"Row Height: 0" + "\n" +
								"Current Column: 0" + "\n" +
								"Column Width: 0" + "\n" +
								"Column Height: 0" + "\n";
			lvDebugLabel.MouseDown += new MouseEventHandler(LVDebugLabel_MouseDown);
			lvDebugLabel.MouseHover += new EventHandler(LVDebugLabel_MouseHover);
			lvDebugLabel.MouseMove += new MouseEventHandler(LVDebugLabel_MouseMove);
			lvDebugLabel.MouseLeave += new EventHandler(LVDebugLabel_MouseLeave);
			lvDebugLabel.Leave += new EventHandler(LVDebugLabel_Leave);


			lvDebugForm.SuspendLayout();
			lvDebugForm.Name = "LVDebugForm";
			lvDebugForm.Text = "List View Visual Debugger";
			lvDebugForm.AutoScaleDimensions = new SizeF(7F, 15F);
			lvDebugForm.AutoScaleMode = AutoScaleMode.Font;
			lvDebugForm.ClientSize = new Size(160, 185);
			lvDebugForm.BackColor = SystemColors.Menu;
			lvDebugForm.ForeColor = SystemColors.ControlText;
			lvDebugForm.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			lvDebugForm.MaximizeBox = false;
			lvDebugForm.MinimizeBox = false;
			lvDebugForm.ControlBox = false;
			lvDebugForm.ShowInTaskbar = false;
			lvDebugForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			lvDebugForm.StartPosition = FormStartPosition.CenterParent;
			lvDebugForm.CausesValidation = false;
			lvDebugForm.ShowIcon = false;
			lvDebugForm.Controls.Add(lvDebugLabel);
			lvDebugForm.ResumeLayout(false);
			lvDebugForm.PerformLayout();
			lvDebugForm.Show();

			lvDebugWindowDelegate = new ThreadStart(delegate ()
			{
				UpdateListViewDebugWindow();
			});

			lvDebugWindowUpdateThread = new Thread(lvDebugWindowDelegate);
			lvDebugWindowUpdateThread.Start();
		}

		private void UpdateListViewDebugWindow()
		{
			bool varQuit;
			int oldXCoord;
			int oldYCoord;
			int oldCurrentRow;
			int oldCurRowLength;
			int oldCurRowHeight;
			int oldCurrentColumn;
			int oldCurColumnWidth;
			int oldCurColumnHeight;
			
			lock(debugLock) {
				varQuit = quit;
				oldXCoord = curXCoord;
				oldYCoord = curYCoord;
				oldCurrentRow = currentRow;
				oldCurRowLength = curRowLength;
				oldCurRowHeight = curRowHeight;
				oldCurrentColumn = currentColumn;
				oldCurColumnWidth = curColumnWidth;
				oldCurColumnHeight = curColumnHeight;
			}

			while(!varQuit) {
				if(lvDebugLabel.InvokeRequired) {
					lvDebugLabel.BeginInvoke((MethodInvoker)delegate ()
					{
						lvDebugLabel.Text = "X Coordinate: " + oldXCoord.ToString() + "\n" +
											"Y Coordinate: " + oldYCoord.ToString() + "\n" +
											"Current Row: " + oldCurrentRow.ToString() + "\n" +
											"Row Length: " + oldCurRowLength.ToString() + "\n" +
											"Row Height: " + oldCurRowHeight.ToString() + "\n" +
											"Current Column: " + oldCurrentColumn.ToString() + "\n" +
											"Column Width: " + oldCurColumnWidth.ToString() + "\n" +
											"Column Height: " + oldCurColumnHeight.ToString() + "\n";
					});
				}
				else {
					lvDebugLabel.Text = "X Coordinate: " + oldXCoord.ToString() + "\n" +
											"Y Coordinate: " + oldYCoord.ToString() + "\n" +
											"Current Row: " + oldCurrentRow.ToString() + "\n" +
											"Row Length: " + oldCurRowLength.ToString() + "\n" +
											"Row Height: " + oldCurRowHeight.ToString() + "\n" +
											"Current Column: " + oldCurrentColumn.ToString() + "\n" +
											"Column Width: " + oldCurColumnWidth.ToString() + "\n" +
											"Column Height: " + oldCurColumnHeight.ToString() + "\n";
				}

				lock (debugLock) {
					if (!quit) {

						if (oldXCoord != curXCoord)
							oldXCoord = curXCoord;

						if (oldYCoord != curYCoord)
							oldYCoord = curYCoord;

						if (oldCurrentRow != currentRow) {
							oldCurrentRow = currentRow;
							oldCurRowLength = curRowLength;
							oldCurRowHeight = curRowHeight;
						}

						if (oldCurrentColumn != currentColumn) {
							oldCurrentColumn = currentColumn;
							oldCurColumnWidth = curColumnWidth;
							oldCurColumnHeight = curColumnHeight;
						}
					}
					else
						varQuit = quit;
				}

				Thread.Sleep(50);
			}
		}

		private void ToolTipTimer_Tick(object? sender, EventArgs e)
		{
			toolTipTimer.Stop();
			mouseMoveEnabled = true;
		}

		private void SetupListViewDebugWindowToolTips()
		{
			lvDebugWindowToolTip.Active = true;
			lvDebugWindowToolTip.IsBalloon = false;
			lvDebugWindowToolTip.ToolTipIcon = ToolTipIcon.None;
			lvDebugWindowToolTip.AutoPopDelay = toolTipAutoPopDelay;
			lvDebugWindowToolTip.InitialDelay = toolTipInitialDelay;
			lvDebugWindowToolTip.ReshowDelay = toolTipReshowDelay;
			lvDebugWindowToolTip.BackColor = SystemColors.Info;
			lvDebugWindowToolTip.ForeColor = SystemColors.InfoText;

			toolTipTimer.Interval = toolTipTimerInterval;
			toolTipTimer.Tick += new EventHandler(ToolTipTimer_Tick);

			if (IsDebugModeEnabled)
				Debug("List view debug window tool tips have now been setup");
		}

		private void LVDebugLabel_MouseDown(object? sender, MouseEventArgs e)
		{
			if (IsDebugModeEnabled)
				Info("LV debug label mouse down event has been triggered");

			if(isToolTipShown) {
				lvDebugWindowToolTip.Hide(lvDebugLabel);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}

		private void LVDebugLabel_MouseHover(object? sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				Info("LV debug label mouse hover event has been triggered");

			lvDebugWindowToolTip.Show("Displays the list view mouse data in debug mode", lvDebugLabel, toolTipTimerShowInterval);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void LVDebugLabel_MouseMove(object? sender, MouseEventArgs e)
		{
			if(mouseMoveEnabled) {
				if (IsDebugModeEnabled)
					Info("LV debug label mouse move event has been triggered");

				if(isToolTipShown) {
					lvDebugWindowToolTip.Hide(lvDebugLabel);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void LVDebugLabel_MouseLeave(object? sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				Info("LV debug label mouse leave event has been triggered");

			if(isToolTipShown) {
				lvDebugWindowToolTip.Hide(lvDebugLabel);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}

		private void LVDebugLabel_Leave(object? sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				Info("LV debug label leave event has been triggered");

			if(isToolTipShown) {
				lvDebugWindowToolTip.Hide(lvDebugLabel);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}
		}
	}
}