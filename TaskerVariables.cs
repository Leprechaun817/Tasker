#if DEBUG
#define TASKER_DEBUG
#endif

using static Tasker.TaskerObjects;

namespace Tasker
{
	public sealed class ProgramSwitches
	{
		//PLEASE NOTE - Once these variables are set when the program is first run, they are not to be changed again.
		public SetOnInit<bool> isDebugModeEnabled = new SetOnInit<bool>("IsDebugModeEnabled");
		public SetOnInit<bool> isLoggingEnabled = new SetOnInit<bool>("IsLoggingEnabled");
		public SetOnInit<bool> isLoggingLevelArgSet = new SetOnInit<bool>("IsLoggingLevelArgSet");
		public SetOnInit<bool> isLogLevelSet = new SetOnInit<bool>("IsLogLevelSet");
		public SetOnInit<bool> isConsoleEnabled = new SetOnInit<bool>("IsConsoleEnabled");
		public SetOnInit<bool> isLogFileEnabled = new SetOnInit<bool>("IsLogFileEnabled");
		public SetOnInit<bool> isLVMouseTrackDebugEnabled = new SetOnInit<bool>("IsLVMouseTrackDebugEnabled");
		public SetOnInit<bool> isTaskEntryDebugEnabled = new SetOnInit<bool>("IsTaskEntryDebugEnabled");
		public SetOnInit<bool> isTaskListTestEnabled = new SetOnInit<bool>("IsTaskListTestEnabled");
		public SetOnInit<bool> isTaskLoggingEnabled = new SetOnInit<bool>("IsTaskLoggingEnabled");
		public SetOnInit<bool> isHelpSwitchEnabled = new SetOnInit<bool>("IsHelpSwitchEnabled");
	}

	public static class TaskerObjects
	{
		//Logger related objects
		public static Logger? LoggerObj { get; set; }

		//Program switch related objects
		public static ProgramSwitches? Switches { get; set; }
	}

	public static class TaskerVariables
	{ 
		public static bool IsDebugModeEnabled 
		{
			get
			{
				return Switches!.isDebugModeEnabled.Value;
			}

			set
			{
				Switches!.isDebugModeEnabled.Value = value;
			}
		}

		public static bool IsLoggingEnabled 
		{
			get
			{
				return Switches!.isLoggingEnabled.Value;
			}

			set
			{
				Switches!.isLoggingEnabled.Value = value;
			}
		}

		public static bool IsLoggingLevelArgSet 
		{
			get
			{
				return Switches!.isLoggingLevelArgSet.Value;
			}

			set
			{
				Switches!.isLoggingLevelArgSet.Value = value;
			}
		}
		
		public static bool IsLogLevelSet 
		{
			get
			{
				return Switches!.isLogLevelSet.Value;
			}

			set
			{
				Switches!.isLogLevelSet.Value = value;
			}
		}

		public static bool IsConsoleEnabled 
		{
			get
			{
				return Switches!.isConsoleEnabled.Value;
			}

			set
			{
				Switches!.isConsoleEnabled.Value = value;
			}
		}
		
		public static bool IsLogFileEnabled 
		{
			get
			{
				return Switches!.isLogFileEnabled.Value;
			}

			set
			{
				Switches!.isLogFileEnabled.Value = value;
			}
		}
		
		public static bool IsLVMouseTrackDebugEnabled 
		{
			get
			{
				return Switches!.isLVMouseTrackDebugEnabled.Value;
			}

			set
			{
				Switches!.isLVMouseTrackDebugEnabled.Value = value;
			}
		}
		
		public static bool IsTaskEntryDebugEnabled 
		{
			get
			{
				return Switches!.isTaskEntryDebugEnabled.Value;
			}

			set
			{
				Switches!.isTaskEntryDebugEnabled.Value = value;
			}
		}
		
		public static bool IsTaskListTestEnabled 
		{
			get
			{
				return Switches!.isTaskListTestEnabled.Value;
			}

			set
			{
				Switches!.isTaskListTestEnabled.Value = value;
			}
		}
		
		public static bool IsTaskLoggingEnabled 
		{
			get
			{
				return Switches!.isTaskLoggingEnabled.Value;
			}

			set
			{
				Switches!.isTaskLoggingEnabled.Value = value;
			}
		}
		
		public static bool IsHelpSwitchEnabled 
		{
			get
			{
				return Switches!.isHelpSwitchEnabled.Value;
			}

			set
			{
				Switches!.isHelpSwitchEnabled.Value = value;
			}
		}

		public static readonly string taskerStatusStarted = "Tasker v1.0 -- Tracking Started";
		public static readonly string taskerStatusRunning = "Tasker v1.0 -- Tracking Running";
		public static readonly string taskerStatusPaused = "Tasker v1.0 -- Tracking Paused";
		public static readonly string taskerStatusStopped = "Tasker v1.0 -- Tracking Stopped";
	}
}