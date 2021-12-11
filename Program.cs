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

#if DEBUG
#define TASKER_DEBUG
#endif

using System;
using System.Windows.Forms;


using static Tasker.TaskerObjects;
using static Tasker.TaskerVariables;
using static Tasker.LogCalls;

namespace Tasker
{
	internal static class Program
	{
		//Program arguments are available for use with this application to help with debugging any issues
		//The potential program switches/arguments that can be fed into the exe are as follows:
		//'-dm'   -- This enables debug mode. If this switch is not included with any of the other switches, the program will error out.
		//'-lge'  -- This enables logging. Debug mode must also be enabled for this switch to work.
		//'-lvl'  -- This argument must be included with the '-lge' switch. It tells the program what logging level you want.
		//        -- Logging levels are as follows: --
		//        ----  FULLDEBUG,u,U,-10 ----> Lowest level possible, all messages, including internal logger messages will appear in the console and/or log file
		//        ----  DEBUG,d,D,0 ----------> Will enable all messages with the exception of the internal logger messages
		//        ----  INFO,i,I,1 -----------> Will enable all messages that are at least at the INFO level or higher
		//        ----  WARN,w,W,2 -----------> Will enable all messages that are at least at the WARN level or higher
		//        ----  ERROR,e,E,3 ----------> Will enable all messages that are at least at the ERROR level or higher
		//        ----  FATAL,f,F,4 ----------> Will enable all messages that are at least at the FATAL level or higher
		//        ----  NONE,n,N,-25 ---------> This will stop all messages appearing in the log file/console regardless of severity. If this level argument is used,
		//                                      and logging is enabled, the program will ask to make sure you meant to do this.
		//        ---- The logging level argument must follow the '-lvl' switch. Example: tasker.exe -dm -lge -lvl D
		//'-lgf'  -- This enables a log file to be created during program execution. All log messages generated during execution will get dumped into this log file.
		//'-c'    -- This enables a console window to be created during program execution. All log messages generated during execution will get output to this window.
		//'-lmtd' -- This turns on list view mouse tracking debugging system
		//'-tlt'  -- This causes test entries to appear in the task list on the main window
		//'-ted'  -- This turns on the task entry debugging. A button is made visible in the interface which cause a task entry screen to appear when clicked, allowing
		//			 for quick debugging of the task entry window
		//'-tgd'  -- This switch is used to explicitly turn off the task logging that will happen automatically
		//'-h' -- This will cause a block of text to appear in the console which will list all available debug switches and what they are used for
		
		[STAThread]
#if TASKER_DEBUG
		static void Main(string[] args)
		{
			Switches = new ProgramSwitches();

			LogLevels logLevelSetting = LogLevels.NullLogValue;

			{
				SpecialBoolean debugModeSet = new SpecialBoolean();
				SpecialBoolean loggingEnabledSet = new SpecialBoolean();
				SpecialBoolean loggingLevelArgFound = new SpecialBoolean();
				SpecialBoolean loggingLevelSet = new SpecialBoolean();
				SpecialBoolean consoleEnabled = new SpecialBoolean();
				SpecialBoolean logFileEnabled = new SpecialBoolean();
				SpecialBoolean lvMouseTrackingDebugEnabled = new SpecialBoolean();
				SpecialBoolean taskEntryDebugEnabled = new SpecialBoolean();
				SpecialBoolean taskListTestEnabled = new SpecialBoolean();
				SpecialBoolean taskLoggingDisableSet = new SpecialBoolean();
				SpecialBoolean helpSwitchEnabled = new SpecialBoolean();
				
				ProcessProgramArguments(args, ref logLevelSetting, ref debugModeSet, ref loggingEnabledSet, ref loggingLevelArgFound, ref loggingLevelSet, ref consoleEnabled, ref logFileEnabled, ref lvMouseTrackingDebugEnabled, ref taskEntryDebugEnabled, ref taskListTestEnabled, ref taskLoggingDisableSet, ref helpSwitchEnabled);

				CheckProgramConfigurationConsistency(ref logLevelSetting, ref debugModeSet, ref loggingEnabledSet, ref loggingLevelArgFound, ref loggingLevelSet, ref consoleEnabled, ref logFileEnabled, ref lvMouseTrackingDebugEnabled, ref taskEntryDebugEnabled, ref taskListTestEnabled, ref taskLoggingDisableSet, ref helpSwitchEnabled);

				IsDebugModeEnabled = debugModeSet.GetValue();
				IsLoggingEnabled = loggingEnabledSet.GetValue();
				IsLoggingLevelArgSet = loggingLevelArgFound.GetValue();
				IsLogLevelSet = loggingLevelSet.GetValue();
				IsConsoleEnabled = consoleEnabled.GetValue();
				IsLogFileEnabled = logFileEnabled.GetValue();
				IsLVMouseTrackDebugEnabled = lvMouseTrackingDebugEnabled.GetValue();
				IsTaskEntryDebugEnabled = taskEntryDebugEnabled.GetValue();
				IsTaskListTestEnabled = taskEntryDebugEnabled.GetValue();
				IsTaskLoggingEnabled = !taskLoggingDisableSet.GetValue();
				IsHelpSwitchEnabled = helpSwitchEnabled.GetValue();
			}

			if(IsHelpSwitchEnabled) {
				ConsoleExtensions.StartConsoleEx();

				DisplayHelpText();

				Console.Write("Press any key to continue...");
				Console.ReadKey();
				ConsoleExtensions.FreeConsoleEx();
			}

			if (!IsHelpSwitchEnabled) {
				//Create logger object and check if console is enabled
				if (IsLoggingEnabled) {
					LoggerObj = new Logger();
					LoggerObj.SetLogLevelSetting(logLevelSetting);
				}

				if (IsConsoleEnabled)
					ConsoleExtensions.StartConsoleEx();

				if (IsDebugModeEnabled)
					Warn("Log level has been set to: " + LoggerObj!.Level.ToString());

				if (IsDebugModeEnabled)
					Debug("Loading main application window");

				//Console Tests
				if (IsDebugModeEnabled) {
					FullDebug("This is a FullDebug test message");
					Debug("This is a Debug test message");
					Info("This is a Info test message");
					Warn("This is a Warn test message");
					Error("This is a Error test message");
					Fatal("This is a Fatal test message");
					Heartbeat("This is a Heartbeat test message");

					Info("Main Window Loaded");
				}
			}

#else
		static void Main()
		{
#endif
			if (!IsHelpSwitchEnabled) {
				Application.SetHighDpiMode(HighDpiMode.SystemAware);
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);


				Application.Run(new TaskerMainWindow());
			}

#if TASKER_DEBUG
			if (!IsHelpSwitchEnabled) {
				if (IsDebugModeEnabled)
					Info("Exiting Application");


				if (IsConsoleEnabled) {
					if (IsDebugModeEnabled)
						Info("Press any key to continue...");

					Console.ReadKey();
					ConsoleExtensions.FreeConsoleEx();
				}
			}
#endif
		}

#if TASKER_DEBUG
		private static void ProcessProgramArguments(string[] pArgs, ref LogLevels levelSetting, ref SpecialBoolean debugModeSet, ref SpecialBoolean loggingEnabledSet, ref SpecialBoolean loggingLevelArgFound, ref SpecialBoolean loggingLevelSet, ref SpecialBoolean consoleEnabled, ref SpecialBoolean logFileEnabled, ref SpecialBoolean lvMouseTrackingDebugEnabled, ref SpecialBoolean taskEntryDebugEnabled, ref SpecialBoolean taskListTestEnabled, ref SpecialBoolean taskLoggingDisableSet, ref SpecialBoolean helpSwitchEnabled)
		{
			foreach (string i in pArgs) {
				//Check for debug program switch
				if(debugModeSet == BoolState.Indeterminate) {
					debugModeSet.SetValue(DebugMode(i));

					if (debugModeSet != BoolState.Indeterminate)
						continue;
				}

				//Check for logging switch
				if(loggingEnabledSet == BoolState.Indeterminate) {
					loggingEnabledSet.SetValue(LoggingEnabled(i));

					if (loggingEnabledSet != BoolState.Indeterminate)
						continue;
				}

				//Checking for logging level argument and then getting the log level setting
				if(loggingLevelArgFound == BoolState.Indeterminate) {
					(BoolState llaf, BoolState lls, levelSetting) = SettingLoggingLevel(i, pArgs);
					loggingLevelArgFound.SetValue(llaf);
					loggingLevelSet.SetValue(lls);

					if (loggingLevelArgFound != BoolState.Indeterminate)
						continue;
				}

				//Checking for console switch
				if(consoleEnabled == BoolState.Indeterminate) {
					consoleEnabled.SetValue(ConsoleEnabled(i));

					if (consoleEnabled != BoolState.Indeterminate)
						continue;
				}

				//Checking for log enabled switch
				if(logFileEnabled == BoolState.Indeterminate) {
					logFileEnabled.SetValue(LogFileEnabled(i));

					if (logFileEnabled != BoolState.Indeterminate)
						continue;
				}

				//Checking for list view visual debug switch
				if(lvMouseTrackingDebugEnabled == BoolState.Indeterminate) {
					lvMouseTrackingDebugEnabled.SetValue(LVMouseTrackingDebug(i));

					if (lvMouseTrackingDebugEnabled != BoolState.Indeterminate)
						continue;
				}

				if(taskEntryDebugEnabled == BoolState.Indeterminate) {
					taskEntryDebugEnabled.SetValue(TaskEntryDebug(i));

					if (taskEntryDebugEnabled != BoolState.Indeterminate)
						continue;
				}

				if(taskListTestEnabled == BoolState.Indeterminate) {
					taskListTestEnabled.SetValue(TaskListTest(i));

					if (taskListTestEnabled != BoolState.Indeterminate)
						continue;
				}

				//Checking for task log disabled
				if(taskLoggingDisableSet == BoolState.Indeterminate) {
					taskLoggingDisableSet.SetValue(TaskLogDisable(i));

					if (taskLoggingDisableSet != BoolState.Indeterminate)
						continue;
				}

				if(helpSwitchEnabled == BoolState.Indeterminate) {
					helpSwitchEnabled.SetValue(HelpSwitchEnabled(i));

					if (helpSwitchEnabled != BoolState.Indeterminate)
						continue;
				}
			}

			if (debugModeSet == BoolState.Indeterminate)
				debugModeSet.SetValue(false);
			if (loggingEnabledSet == BoolState.Indeterminate)
				loggingEnabledSet.SetValue(false);
			if (loggingLevelArgFound == BoolState.Indeterminate)
				loggingLevelArgFound.SetValue(false);
			if (loggingLevelSet == BoolState.Indeterminate)
				loggingLevelSet.SetValue(false);
			if (consoleEnabled == BoolState.Indeterminate)
				consoleEnabled.SetValue(false);
			if (logFileEnabled == BoolState.Indeterminate)
				logFileEnabled.SetValue(false);
			if (lvMouseTrackingDebugEnabled == BoolState.Indeterminate)
				lvMouseTrackingDebugEnabled.SetValue(false);
			if (taskEntryDebugEnabled == BoolState.Indeterminate)
				taskEntryDebugEnabled.SetValue(false);
			if (taskListTestEnabled == BoolState.Indeterminate)
				taskListTestEnabled.SetValue(false);
			if (taskLoggingDisableSet == BoolState.Indeterminate)
				taskLoggingDisableSet.SetValue(false);
			if (helpSwitchEnabled == BoolState.Indeterminate)
				helpSwitchEnabled.SetValue(false);
		}

		private static void CheckProgramConfigurationConsistency(ref LogLevels levelSetting, ref SpecialBoolean debugModeSet, ref SpecialBoolean loggingEnabledSet, ref SpecialBoolean loggingLevelArgSet, ref SpecialBoolean loggingLevelSet, ref SpecialBoolean consoleEnabled, ref SpecialBoolean logFileEnabled, ref SpecialBoolean lvMouseTrackingDebugEnabled, ref SpecialBoolean taskEntryDebugEnable, ref SpecialBoolean taskListTestEnabled, ref SpecialBoolean taskLoggingDisable, ref SpecialBoolean helpSwitchEnabled)
		{
			//If the help switch is enabled, everything else will be automatically disabled. No message will be shown to alert the user to this
			if (helpSwitchEnabled == true) {
				debugModeSet.SetValue(false);
				loggingEnabledSet.SetValue(false);
				loggingLevelArgSet.SetValue(false);
				loggingLevelSet.SetValue(false);
				consoleEnabled.SetValue(false);
				logFileEnabled.SetValue(false);
				lvMouseTrackingDebugEnabled.SetValue(false);
				taskEntryDebugEnable.SetValue(false);
				taskListTestEnabled.SetValue(false);
				taskLoggingDisable.SetValue(true);

				levelSetting = LogLevels.NullLogValue;
			}
			else {

				//Debug mode must be enabled for any log or console debug messages, or the various debug modes to be enabled
				if (!debugModeSet.GetValue() && true.EqualsAny(loggingEnabledSet.GetValue(), consoleEnabled.GetValue(), logFileEnabled.GetValue(), 
					lvMouseTrackingDebugEnabled.GetValue(), taskEntryDebugEnable.GetValue(), taskListTestEnabled.GetValue())) {
					MessageBox.Show("Debug mode must be enabled for logging and console, log file functionality, and the list view visual debug", "Debug Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					loggingEnabledSet.SetValue(false);
					consoleEnabled.SetValue(false);
					logFileEnabled.SetValue(false);
					loggingLevelArgSet.SetValue(false);
					loggingLevelSet.SetValue(false);
					lvMouseTrackingDebugEnabled.SetValue(false);
					taskEntryDebugEnable.SetValue(false);
					taskListTestEnabled.SetValue(false);

					levelSetting = LogLevels.NullLogValue;
				}

				//Debug mode enabled, but logging, console and log file is disabled
				if (debugModeSet.GetValue() && false.EqualsAll(loggingEnabledSet.GetValue(), consoleEnabled.GetValue(), logFileEnabled.GetValue(), loggingLevelArgSet.GetValue())) {
					MessageBox.Show("Debug mode has been enabled, but logging, console, log file output, and list view visual debug have been disabled. If this is unexpected, please restart with the correct program arguments", "Debug Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					debugModeSet.SetValue(false);

					levelSetting = LogLevels.NullLogValue;
				}

				//If logging level was set, make sure that logging was enabled
				if (loggingLevelArgSet.GetValue() && !loggingEnabledSet.GetValue()) {
					MessageBox.Show("Logging must be enabled for logging levels to be set. No logging for rest of session", "Logging Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

					//Reset log levels to none and set isLogLevelset to false
					loggingLevelSet.SetValue(false);
					loggingLevelArgSet.SetValue(false);
					logFileEnabled.SetValue(false);
					consoleEnabled.SetValue(false);
					loggingEnabledSet.SetValue(false);

					levelSetting = LogLevels.None;
				}

				//If logging was enabled and logging level wasn't set, set default log level to Fatal and notify user
				if (loggingEnabledSet.GetValue() && !loggingLevelSet.GetValue()) {
					MessageBox.Show("Logging level was not set. Log level will default to Fatal", "Logging Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

					loggingLevelArgSet.SetValue(true);
					levelSetting = LogLevels.Fatal;
				}
				else if (loggingEnabledSet.GetValue() && loggingLevelSet.GetValue() && loggingLevelArgSet.GetValue() && levelSetting == LogLevels.None) {
					DialogResult result = MessageBox.Show("You have enabled logging but set the logging level to None. Did you mean to do that?", "Logging Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

					if (result == DialogResult.No) {
						levelSetting = LogLevels.Fatal;
						MessageBox.Show("Logging level has been set to the default level Fatal", "Logging Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}

				//If console is enabled but logging has been disabled or set to -1, then alert user - No output to console with logging disabled
				if (consoleEnabled.GetValue() && !loggingEnabledSet.GetValue()) {
					DialogResult result = MessageBox.Show("You have enabled the console without enabling logging.\nThere will be no output to console.\nDid you mean to do that?", "Logging Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

					if (result == DialogResult.No) {
						loggingEnabledSet.SetValue(true);
						loggingLevelSet.SetValue(true);
						loggingLevelArgSet.SetValue(true);
						levelSetting = LogLevels.Fatal;
						MessageBox.Show("Logging has been enabled and set to the default level Fatal", "Logging Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else {
						//Set log level to None and turn console off
						consoleEnabled.SetValue(false);
						levelSetting = LogLevels.None;
						MessageBox.Show("Console has been disabled as there will be no output to it with logging disabled", "Logging Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}

				//If log file is enabled but logging has been disabled or set to -1, then alert user - No output to log file with logging disabled
				if (logFileEnabled.GetValue() && !loggingEnabledSet.GetValue()) {
					DialogResult result = MessageBox.Show("You have enabled the log file without enabling logging.\nThere will be no output to console.\nDid you mean to do that?", "Logging Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

					if (result == DialogResult.No) {
						loggingEnabledSet.SetValue(true);
						loggingLevelSet.SetValue(true);
						loggingLevelArgSet.SetValue(true);
						levelSetting = LogLevels.Fatal;
						MessageBox.Show("Logging has been enabled and set to the default level Fatal", "Logging Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else {
						//Set log level to None and turn log file off
						logFileEnabled.SetValue(false);
						levelSetting = LogLevels.None;
						MessageBox.Show("Log file has been disabled as there will be no output to it with logging disabled", "Logging Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}

				//If no program switches/arguments were provided at all
				if (false.EqualsAll(debugModeSet.GetValue(), loggingEnabledSet.GetValue(), loggingLevelArgSet.GetValue(), loggingLevelSet.GetValue(), 
									consoleEnabled.GetValue(), logFileEnabled.GetValue(), lvMouseTrackingDebugEnabled.GetValue(), taskEntryDebugEnable.GetValue(),
									taskListTestEnabled.GetValue()))
					levelSetting = LogLevels.NullLogValue;
			}
		}

		private static BoolState DebugMode(string dM)
		{
			BoolState isDebugModeEnabled = BoolState.Indeterminate;
			if(!dM.Contains("--")) {
				//Case where whole word is not being used
				if(dM.ContainsAny("-dm", "-Dm", "-dM", "-DM", "-dbg", "-DBG", "-Dbg", "-dBg", "-dbG", "-DBg", "-dBG", "-DbG")) {
					isDebugModeEnabled = BoolState.True;
				}
			}
			else {
				//Case where whole word was used
				if(dM.Substring(2, 9).ToLower() == "debugmode") {
					isDebugModeEnabled = BoolState.True;
				}
			}

			return isDebugModeEnabled;
		}

		private static BoolState LoggingEnabled(string lE)
		{
			BoolState isLoggingEnabled = BoolState.Indeterminate;
			if(!lE.Contains("--")) {
				if(lE.ContainsAny("-lge", "-LGE", "-Lge", "-lGe", "-lgE", "-LGe", "-lGE", "-LgE")) {
					isLoggingEnabled = BoolState.True;
				}
			}
			else {
				if (lE.Substring(2, 14).ToLower() == "loggingenabled") {
					isLoggingEnabled = BoolState.True;
				}
			}

			return isLoggingEnabled;
		}

		private static (BoolState, BoolState, LogLevels) SettingLoggingLevel(string llvl, string[] mpArgs)
		{
			LogLevels logLevelSettingArg = LogLevels.NullLogValue;

			BoolState isLoggingLevelArgSet = BoolState.Indeterminate;
			BoolState isLogLevelSet = BoolState.Indeterminate;
			int llIndexPos = -99;
			int llArgStringLength = -99;
			if(!llvl.Contains("--")) {
				if(llvl.ContainsAny("-lvl", "-LVL", "-Lvl", "-lVl", "-lvL", "-LVl", "-lVL", "-LvL")) {
					//Level argument was found
					isLoggingLevelArgSet = BoolState.True;

					//Get the index position of the declared log level from mpArgs
					//It is always assumed that the log level will come after the program argument "-lvl", if it's not there we either
					// set the log level to null or assume a default depending on the situation
					llIndexPos = Array.FindIndex(mpArgs, element => element.ContainsAny("-lvl", "-LVL", "-Lvl", "-lVl", "-lvL", "-LVl", "-lVL", "-LvL")) + 1;
					llArgStringLength = mpArgs[llIndexPos].ToString().Length;
				}
			}
			else {
				if(llvl.Substring(2, 8).ToLower() == "loglevel") {
					isLoggingLevelArgSet = BoolState.True;

					llIndexPos = Array.FindIndex(mpArgs, element => element.ToLower() == "loglevel") + 1;
					llArgStringLength = mpArgs[llIndexPos].ToString().Length;
				}
			}

			if(isLoggingLevelArgSet == BoolState.True) {
				(isLogLevelSet, logLevelSettingArg) = ProcessLogLevelArg(llArgStringLength, mpArgs[llIndexPos]);
			}

			return (isLoggingLevelArgSet, isLogLevelSet, logLevelSettingArg);
		}

		private static (BoolState, LogLevels) ProcessLogLevelArg(int argStringLength, string llString)
		{
			//Setting this to false, if the logging level is actually set this will become true
			BoolState isLogLevelSet = BoolState.Indeterminate;
			LogLevels level;

			//If string can be converted into number, then log level was defined using a number
			if(Int32.TryParse(llString, out int llNum)) {
				(isLogLevelSet, level) = GetLogLevelFromNumber(llNum);
			}
			else {
				//If string was unable to be converted into a number, then it's a character or string based
				if (argStringLength == 1)
					//Log level defined using single characters
					(isLogLevelSet, level) = GetLogLevelFromChar(llString.ToCharArray()[0]);
				else
					//Log level is defined using the spellings
					(isLogLevelSet, level) = GetLogLevelFromString(llString.ToLower());
			}

			return (isLogLevelSet, level);
		}

		private static BoolState ConsoleEnabled(string cE)
		{
			BoolState isConsoleEnabled = BoolState.Indeterminate;
			if(!cE.Contains("--")) {
				if(cE.ContainsAny("-c", "-C")) {
					isConsoleEnabled = BoolState.True;
				}
			}
			else {
				if(cE.Substring(2, 7).ToLower() == "console") {
					isConsoleEnabled = BoolState.True;
				}
			}

			return isConsoleEnabled;
		}

		private static BoolState LogFileEnabled(string lfE)
		{
			BoolState isLogFileEnabled = BoolState.Indeterminate;
			if(!lfE.Contains("--")) {
				if(lfE.ContainsAny("-lgf", "-LGF", "-Lgf", "-lGf", "-lgF", "-LGf", "-lGF", "-LgF")) {
					isLogFileEnabled = BoolState.True;
				}
			}
			else {
				if(lfE.Substring(2, 7).ToLower() == "logfile") {
					isLogFileEnabled = BoolState.True;
				}
			}

			return isLogFileEnabled;
		}

		private static BoolState LVMouseTrackingDebug(string vd)
		{
			BoolState isLVMouseTrackDebugEnabled = BoolState.Indeterminate;
			if(!vd.Contains("--")) {
				if(vd.ContainsAny("-lmtd", "-LMTD", "-lMtD", "-LmTd", "-LMtd", "-lmTD", "-LmtD", "-lMTd", "-Lmtd", "-lMtd", "-lmTd", "-lmtD")) {
					isLVMouseTrackDebugEnabled = BoolState.True;
				}
			}
			else {
				if(vd.Substring(2, 19).ToLower() == "lvmousetrackdebug") {
					isLVMouseTrackDebugEnabled = BoolState.True;
				}
			}

			return isLVMouseTrackDebugEnabled;
		}

		private static BoolState TaskEntryDebug(string ted)
		{
			BoolState isTaskEntryDebugEnabled = BoolState.Indeterminate;
			if(!ted.Contains("--")) {
				if(ted.ContainsAny("-ted", "-TED", "-Ted", "-tEd", "-teD", "-TEd", "-tED", "-TeD")) {
					isTaskEntryDebugEnabled = BoolState.True;
				}
			}
			else {
				if(ted.Substring(2, 14).ToLower() == "taskentrydebug") {
					isTaskEntryDebugEnabled = BoolState.True;
				}
			}

			return isTaskEntryDebugEnabled;
		}

		private static BoolState TaskListTest(string tlt)
		{
			BoolState isTaskListTestEnabled = BoolState.Indeterminate;
			if(!tlt.Contains("--")) {
				if(tlt.ContainsAny("-tlt", "-TLT", "-Tlt", "-tLt", "-tlT", "-tLT", "-TLt", "-TlT")) {
					isTaskListTestEnabled = BoolState.True;
				}
			}
			else {
				if(tlt.Substring(2, 14).ToLower() == "tasklisttest") {
					isTaskListTestEnabled = BoolState.True;
				}
			}

			return isTaskListTestEnabled;
		}

		private static BoolState TaskLogDisable(string tld)
		{
			BoolState isTaskLoggingEnabled = BoolState.Indeterminate;
			if(!tld.Contains("--")) {
				if(tld.ContainsAny("-tgd", "-TGD", "-Tgd", "-tGd", "-tgD", "-tGD", "-TGd", "-TgD")) {
					isTaskLoggingEnabled = BoolState.True;
				}
			}
			else {
				if(tld.Substring(2, 14).ToLower() == "tasklogdisable") {
					isTaskLoggingEnabled = BoolState.True;
				}
			}

			return isTaskLoggingEnabled;
		}

		private static BoolState HelpSwitchEnabled(string h)
		{
			BoolState isHelpSwitchEnabled = BoolState.Indeterminate;
			if(!h.Contains("--")) {
				if(h.ContainsAny("-h", "-H")) {
					isHelpSwitchEnabled = BoolState.True;
				}
			}
			else {
				if(h.Substring(2, 6).ToLower() == "help") {
					isHelpSwitchEnabled = BoolState.True;
				}
			}

			return isHelpSwitchEnabled;
		}

		private static (BoolState, LogLevels) GetLogLevelFromNumber(int lgLVL)
		{
			BoolState isLevelSet = BoolState.Indeterminate;
			LogLevels level;
			switch(lgLVL) 
			{
				case -25:
					isLevelSet = BoolState.True;
					level = LogLevels.None;
					break;
				case -10:
					isLevelSet = BoolState.True;
					level = LogLevels.FullDebug;
					break;
				case 0:
					isLevelSet = BoolState.True;
					level = LogLevels.Debug;
					break;
				case 1:
					isLevelSet = BoolState.True;
					level = LogLevels.Info;
					break;
				case 2:
					isLevelSet = BoolState.True;
					level = LogLevels.Warn;
					break;
				case 3:
					isLevelSet = BoolState.True;
					level = LogLevels.Error;
					break;
				case 4:
					isLevelSet = BoolState.True;
					level = LogLevels.Fatal;
					break;
				default:
					isLevelSet = BoolState.False;
					level = LogLevels.NullLogValue;
					break;
			}

			return (isLevelSet, level);
		}

		private static (BoolState, LogLevels) GetLogLevelFromChar(int lgLVL)
		{
			BoolState isLevelSet = BoolState.Indeterminate;
			LogLevels level;
			if(lgLVL.EqualsAny('u', 'U')) {
				isLevelSet = BoolState.True;
				level = LogLevels.FullDebug;
			}
			else if(lgLVL.EqualsAny('d', 'D')) {
				isLevelSet = BoolState.True;
				level = LogLevels.Debug;
			}
			else if (lgLVL.EqualsAny('i', 'I')) {
				isLevelSet = BoolState.True;
				level = LogLevels.Info;
			}
			else if (lgLVL.EqualsAny('w', 'W')) {
				isLevelSet = BoolState.True;
				level = LogLevels.Warn;
			}
			else if (lgLVL.EqualsAny('e', 'E')) {
				isLevelSet = BoolState.True;
				level = LogLevels.Error;
			}
			else if (lgLVL.EqualsAny('f', 'F')) {
				isLevelSet = BoolState.True;
				level = LogLevels.Fatal;
			}
			else if (lgLVL.EqualsAny('n', 'N')) {
				isLevelSet = BoolState.True;
				level = LogLevels.None;
			}
			else {
				//If we get here than there wasn't anything there or it wasn't where it should of been.
				//In this case we'll set it to default to nullLogLevel and ask about it later if necessary
				isLevelSet = BoolState.False;
				level = LogLevels.NullLogValue;
			}

			return (isLevelSet, level);
		}

		private static (BoolState, LogLevels) GetLogLevelFromString(string lgLVL)
		{
			BoolState isLevelSet;
			LogLevels level;
			if(lgLVL == "fulldebug") {
				isLevelSet = BoolState.True;
				level = LogLevels.FullDebug;
			}
			else if(lgLVL == "debug") {
				isLevelSet = BoolState.True;
				level = LogLevels.Debug;
			}
			else if (lgLVL == "info") {
				isLevelSet = BoolState.True;
				level = LogLevels.Info;
			}
			else if (lgLVL == "warn") {
				isLevelSet = BoolState.True;
				level = LogLevels.Warn;
			}
			else if (lgLVL == "error") {
				isLevelSet = BoolState.True;
				level = LogLevels.Error;
			}
			else if (lgLVL == "fatal") {
				isLevelSet = BoolState.True;
				level = LogLevels.Fatal;
			}
			else if (lgLVL == "none") {
				isLevelSet = BoolState.True;
				level = LogLevels.None;
			}
			else {
				isLevelSet = BoolState.False;
				level = LogLevels.NullLogValue;
			}

			return (isLevelSet, level);
		}

		private static void DisplayHelpText()
		{
			Console.Write("--Program arguments are available for use with this application to help with debugging any issues--\n");
			Console.Write("\'-dm\'   -- This enables debug mode. If this switch is not included with any of the other switches, the program will error out.\n");
			Console.Write("\'-lge\'  -- This enables logging. Debug mode must also be enabled for this switch to work.\n");
			Console.Write("\'-lvl\'  -- This argument must be included with the '-lge' switch. It tells the program what logging level you want.\n");
			Console.Write("          -- Logging levels are as follows: --\n");
			Console.Write("			 ----  FULLDEBUG,u,U,-10 ----> Lowest level possible, all messages, including internal logger messages will appear in the console and/or log file\n");
			Console.Write("          ----  DEBUG,d,D,0 ----------> Will enable all messages with the exception of the internal logger messages\n");
			Console.Write("          ----  INFO,i,I,1 -----------> Will enable all messages that are at least at the INFO level or higher\n");
			Console.Write("          ----  WARN,w,W,2 -----------> Will enable all messages that are at least at the WARN level or higher\n");
			Console.Write("          ----  ERROR,e,E,3 ----------> Will enable all messages that are at least at the ERROR level or higher\n");
			Console.Write("          ----  FATAL,f,F,4 ----------> Will enable all messages that are at least at the FATAL level or higher\n");
			Console.Write("          ----  NONE,n,N,-25 ---------> This will stop all messages appearing in the log file/console regardless of severity. If this level argument is used,\n");
			Console.Write("                                        and logging is enabled, the program will ask to make sure you meant to do this.\n");
			Console.Write("          ---- The logging level argument must follow the '-lvl' switch. Example: tasker.exe -dm -lge -lvl D\n");
			Console.Write("\'-lgf\'  -- This enables a log file to be created during program execution. All log messages generated during execution will get dumped into this log file.\n");
			Console.Write("\'-c\'    -- This enables a console window to be created during program execution. All log messages generated during execution will get output to this window.");
			Console.Write("\'-lmtd\' -- This turns on list view mouse tracking debugging system\n");
			Console.Write("\'-tlt\'  -- This causes test entries to appear in the task list on the main window\n");
			Console.Write("\'-ted\'  -- This turns on the task entry debugging. A button is made visible in the interface which cause a task entry screen to appear when clicked, allowing");
			Console.Write("             for quick debugging of the task entry window\n");
			Console.Write("\'-tgd\'  -- This switch is used to explicitly turn off the task logging that will happen automatically\n");
		}
#endif
	}
}
