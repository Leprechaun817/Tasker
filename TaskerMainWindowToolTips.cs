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