using System;
using System.Windows.Forms;
using System.Drawing;

using static Tasker.TaskerVariables;
using static Tasker.LogCalls;

namespace Tasker
{
	public partial class TaskerMainWindow
	{
		private void ToolTipTimer_Tick(object? sender, EventArgs e)
		{
			toolTipTimer.Stop();
			mouseMoveEnabled = true;
		}
		
		private void SetupMainWindowToolTips()
		{
			taskerMainWinToolTip.Active = true;
			taskerMainWinToolTip.IsBalloon = false;
			taskerMainWinToolTip.ToolTipIcon = ToolTipIcon.None;
			taskerMainWinToolTip.AutoPopDelay = toolTipAutoPopDelay;
			taskerMainWinToolTip.InitialDelay = toolTipInitialDelay;
			taskerMainWinToolTip.ReshowDelay = toolTipReshowDelay;
			taskerMainWinToolTip.BackColor = SystemColors.Info;
			taskerMainWinToolTip.ForeColor = SystemColors.InfoText;

			toolTipTimer.Interval = toolTipTimerInterval;
			toolTipTimer.Tick += new EventHandler(ToolTipTimer_Tick);
			
			if(IsDebugModeEnabled)
				Debug("Main Window ToolTip has been setup");
		}
	}
}