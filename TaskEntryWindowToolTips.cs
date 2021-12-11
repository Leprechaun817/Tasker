using System;
using System.Windows.Forms;
using System.Drawing;

using static Tasker.TaskerVariables;
using static Tasker.LogCalls;

namespace Tasker
{
	public partial class TaskEntryWindow
	{
		private void ToolTipTimer_Tick(object? sender, EventArgs e)
		{
			toolTipTimer.Stop();
			mouseMoveEnabled = true;
		}
		
		private void SetupTaskEntryWindowToolTips()
		{
			taskEntryWindowToolTip.Active = true;
			taskEntryWindowToolTip.IsBalloon = false;
			taskEntryWindowToolTip.ToolTipIcon = ToolTipIcon.None;
			taskEntryWindowToolTip.AutoPopDelay = toolTipAutoPopupDelay;
			taskEntryWindowToolTip.InitialDelay = toolTipInitialDelay;
			taskEntryWindowToolTip.ReshowDelay = toolTipReshowDelay;
			taskEntryWindowToolTip.BackColor = SystemColors.Info;
			taskEntryWindowToolTip.ForeColor = SystemColors.InfoText;

			toolTipTimer.Interval = toolTipTimerInterval;
			toolTipTimer.Tick += new EventHandler(ToolTipTimer_Tick);

			if (IsDebugModeEnabled)
				Debug("Task Entry ToolTip has been setup");
		}
	}
}