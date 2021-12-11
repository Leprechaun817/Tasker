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
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using Pastel;


using static Tasker.TaskerVariables;
using static Tasker.TaskerObjects;

namespace Tasker
{
	public sealed class LogLevels : Enumeration
	{
		//Log levels go in ascending order, in that the "Debug" level will give the most messages and the "Fatal" level will give the
		//fewest messages.
		//None or NullLogValue both have negative values to ensure that they don't get confused
		//Notes:
		//	-- A "NullLogValue" implies that the log level switch may or may not have been included in the program arguments but no actual level was provided
		//  -- A "None" value implies that it was set this way on purpose, or was set this way due to a config error in the program
		//     arguments provided
		//  -- Both None and NullLogValue will result in no logs being sent to the console or Log file

		//Main Log Levels - When setting log leels, these are what will be used
		public static readonly LogLevels NullLogValue = new LogLevels(-9999, "NullValue");
		public static readonly LogLevels None = new LogLevels(-25, "None");
		public static readonly LogLevels FullDebug = new LogLevels(-10, "FullDebug");
		public static readonly LogLevels Debug = new LogLevels(0, "Debug");
		public static readonly LogLevels Info = new LogLevels(10, "Info");
		public static readonly LogLevels Warn = new LogLevels(20, "Warn");
		public static readonly LogLevels Error = new LogLevels(30, "Error");
		public static readonly LogLevels Fatal = new LogLevels(40, "Fatal");

		//LogMsgError will appear when the log level is set to Debug only
		public static readonly LogLevels LogMsgError = new LogLevels(-9, "LogMsgError");

		//Hearbeat will appear when the log level is set to Info or lower
		public static readonly LogLevels Heartbeat = new LogLevels(9, "Heartbeat");

		//EmptyQueue will appear when the log level is set to Info or lower
		public static readonly LogLevels EmptyQueue = new LogLevels(8, "EmptyQueue");

		//AppVersion will appear when the log level is set to Fatal or lower
		public static readonly LogLevels AppVersion = new LogLevels(39, "AppVersion");

		//EventLog will appear when the log level is set to FullDebug - This "log level" is meant to be used with the C# event system
		public static readonly LogLevels EventLog = new LogLevels(-11, "EventLog");

		public readonly int intLogLevel;
		public readonly string logDisplayName;
		
		//Constructor set to private so that only the Enumerations defined in the LogLevels class here can ever exist while the program is running
		private LogLevels() : base(-9999, "NullValue")
		{
			intLogLevel = -9999;
			logDisplayName = "NullValue";
		}

		private LogLevels(int lV, string lDN) : base(lV, lDN) 
		{
			intLogLevel = lV;
			logDisplayName = lDN;
		}

		public static bool operator ==(LogLevels LeftLVL, LogLevels RightLVL)
		{
			if (LeftLVL.IsNull())
				throw new ArgumentNullException(nameof(LeftLVL), "Cannot use a null LogLevel object in a comparison");
			if (RightLVL.IsNull())
				throw new ArgumentNullException(nameof(RightLVL), "Cannot use a null LogLevel object in a comparison");

			return (LeftLVL.intLogLevel == RightLVL.intLogLevel);
		}

		public static bool operator !=(LogLevels LeftLVL, LogLevels RightLVL)
		{
			if (LeftLVL.IsNull())
				throw new ArgumentNullException(nameof(LeftLVL), "Cannot use a null LogLevel object in a comparison");
			if (RightLVL.IsNull())
				throw new ArgumentNullException(nameof(RightLVL), "Cannot use a null LogLevel object in a comparison");

			return !(LeftLVL.intLogLevel == RightLVL.intLogLevel);
		}

		public static bool operator >=(LogLevels LeftLVL, LogLevels RightLVL)
		{
			if (LeftLVL.IsNull())
				throw new ArgumentNullException(nameof(LeftLVL), "Cannot use a null LogLevel object in a comparison");
			if (RightLVL.IsNull())
				throw new ArgumentNullException(nameof(RightLVL), "Cannot use a null LogLevel object in a comparison");

			return (LeftLVL.intLogLevel >= RightLVL.intLogLevel);
		}

		public static bool operator <=(LogLevels LeftLVL, LogLevels RightLVL)
		{
			if (LeftLVL.IsNull())
				throw new ArgumentNullException(nameof(LeftLVL), "Cannot use a null LogLevel object in a comparison");
			if (RightLVL.IsNull())
				throw new ArgumentNullException(nameof(RightLVL), "Cannot use a null LogLevel object in a comparison");

			return (LeftLVL.intLogLevel <= RightLVL.intLogLevel);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object? obj)
		{
			return base.Equals(obj);
		}
	}

	public sealed class Logger
	{
		private LogLevels logLevelSetting = LogLevels.NullLogValue;
		private bool logLevelSet = false;

		private readonly Queue<(string, LogLevels)> logBuffer = new Queue<(string, LogLevels)>();

		private readonly string logFileTimestamp;

		private readonly BackgroundWorker bufferHandler;
		private readonly object bufferLock = new object();
		private readonly int bufferLoopCounter;

		private readonly FileStream? logFile;

		public LogLevels Level
		{
			get
			{
				return logLevelSetting;
			}
		}

		public Logger()
		{
			//Set for async buffer operations
			bufferHandler = new BackgroundWorker() { WorkerSupportsCancellation = true };
			bufferHandler.DoWork += new DoWorkEventHandler(BufferHandler_DoWork);

			if (IsLogFileEnabled)
				logFileTimestamp = DateTime.Now.ToLongDateString() + "-" + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString();
			else
				logFileTimestamp = String.Empty;

			bufferLoopCounter = 1;

			string logPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			logPath = Path.Combine(logPath, "TaskerApp\\Log");
			string logFilePath = Path.Combine(logPath, "Log-" + logFileTimestamp + ".log");

			if(!File.Exists(logFilePath)) {
				if(!Directory.Exists(logPath)) {
					Directory.CreateDirectory(logPath);
				}

				logFile = new FileStream(logFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4, FileOptions.SequentialScan);
				logFile.Seek(0, SeekOrigin.Begin);
			}
			else {
				logFile = new FileStream(logFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4, FileOptions.SequentialScan);
				logFile.Seek(0, SeekOrigin.End);
			}
		}

		~Logger()
		{
			WriteInternalLog("Final flush of log buffer", LogLevels.FullDebug);

			FlushLogBuffer();

			logFile!.Close();
			logFile!.Dispose();
		}

		public void SetLogLevelSetting(LogLevels lvl)
		{
			if(!logLevelSet) {
				logLevelSetting = lvl;
				logLevelSet = true;
			}
			else {
				throw new InvalidOperationException("Log level setting has already been set and cannot be set again once the app has started");
			}
		}

		public void FlushLogBuffer()
		{
			if (bufferHandler.IsBusy) {
				bufferHandler.CancelAsync();
				bufferHandler.Dispose();

				WriteInternalLog("Cancelling async operation on buffer handler", LogLevels.FullDebug);
			}

			WriteInternalLog("Manually flushing log buffer", LogLevels.FullDebug);

			int i = 0;
			int remainingLogs = GetLogBufferSize();
			int logsLeftToProcess = remainingLogs;
			string bufferLoopNumber = bufferLoopCounter.ToString();

			if (remainingLogs != 0) {
				WriteInternalLog("Processing Loop -> " + bufferLoopNumber + " starting", LogLevels.FullDebug);
				string startLoopTimeStamp = GetDateTimestampWithMillis();
				WriteInternalLog("Current Processing Loop->" + bufferLoopNumber + " started at: " + startLoopTimeStamp, LogLevels.FullDebug);
				WriteInternalLog("--------------------------------------------------------------------------------------------------------", LogLevels.FullDebug);
				WriteInternalLog("Processing " + remainingLogs.ToString() + " log messages in current loop", LogLevels.FullDebug);
				WriteInternalLog("--------------------------------------------------------------------------------------------------------", LogLevels.FullDebug);

				while (i < remainingLogs) {
					string message;
					LogLevels level;
					(message, level) = GetNextMessageFromLogBuffer();
					if (!string.IsNullOrEmpty(message)) {
						if (level != LogLevels.None && level != LogLevels.NullLogValue) {
							WriteToConsole(message, level);
							WriteToLogFile(message);
						}

						i++;
						logsLeftToProcess--;
						WriteInternalLog("--------------------------------------------------------------------------------------------------------", LogLevels.FullDebug);
						WriteInternalLog(logsLeftToProcess.ToString() + " log messages left to process in the current loop", LogLevels.FullDebug);
						WriteInternalLog("--------------------------------------------------------------------------------------------------------", LogLevels.FullDebug);
					}
					else {
						//Log buffer is unexpectedly empty, display as log error
						WriteInternalLog(DateTime.Now.ToLongTimeString() + " -- " + LogLevels.LogMsgError.ToString() + " -- " + "Expected message was empty or null", LogLevels.LogMsgError);

						//Stop loop from continuing
						i = remainingLogs;
					}
				}

				string endLoopTimeStamp = GetDateTimestampWithMillis();
				WriteInternalLog("Current Processing Loop->" + bufferLoopNumber + " stopped at: " + endLoopTimeStamp, LogLevels.FullDebug);
			}
			else {
				string message = DateTime.Now.ToLongTimeString() + " -- " + LogLevels.EmptyQueue.ToString() + " -- " + "No flush required, log buffer is already empty";
				WriteToLogFile(message);
				WriteToConsole(message, LogLevels.EmptyQueue);
			}
		}

		public void UpdateMessageLogBuffer(string inputMsg, LogLevels msgLogLevel)
		{
			//Find log level
			//If log level is set to none or null, then no logging will take place
			if (!logLevelSetting.Value.EqualsAny(LogLevels.None.Value, LogLevels.NullLogValue.Value)) {
				string logMessage = BuildLogMessage(inputMsg, msgLogLevel);

				if (logMessage != string.Empty) {
					AddMessageToLogBuffer(logMessage, msgLogLevel);

					if (!bufferHandler.IsBusy) {
						bufferHandler.RunWorkerAsync();
					}
					else {
						bufferHandler.CancelAsync();
						FlushLogBuffer();
					}
				}
			}
		}

		private string BuildLogMessage(string inputMsg, LogLevels msgLogLevel)
		{
			//Create string for log message
			string msgPrefix = DateTime.Now.ToLongTimeString() + " -- ";
			string message = string.Empty;

			//Find the message log level and construct the log message accordingly
			//Main LogLevels
			if (msgLogLevel == LogLevels.FullDebug && LogLevels.FullDebug >= logLevelSetting) {
				message = msgPrefix + LogLevels.FullDebug.ToString() + " -- " + inputMsg;
			}

			else if (msgLogLevel == LogLevels.Debug && LogLevels.Debug >= logLevelSetting) {
				message = msgPrefix + LogLevels.Debug.ToString() + " -- " + inputMsg;
			}

			else if (msgLogLevel == LogLevels.Info && LogLevels.Info >= logLevelSetting) {
				message = msgPrefix + LogLevels.Info.ToString() + " -- " + inputMsg;
			}

			else if (msgLogLevel == LogLevels.Warn && LogLevels.Warn >= logLevelSetting) {
				message = msgPrefix + LogLevels.Warn.ToString() + " -- " + inputMsg;
			}

			else if (msgLogLevel == LogLevels.Error && LogLevels.Error >= logLevelSetting) {
				message = msgPrefix + LogLevels.Error.ToString() + " -- " + inputMsg;
			}

			else if (msgLogLevel == LogLevels.Fatal && LogLevels.Fatal >= logLevelSetting) {
				message = msgPrefix + LogLevels.Fatal.ToString() + " -- " + inputMsg;
			}

			//Other LogLevels
			//Log Message Errors only appear when log is set to DEBUG
			else if (msgLogLevel == LogLevels.LogMsgError && LogLevels.Debug >= logLevelSetting) {
				message = msgPrefix + LogLevels.LogMsgError.ToString() + " -- " + inputMsg;
			}

			//Heartbeat messages only appear when log is set to INFO or lower
			else if (msgLogLevel == LogLevels.Heartbeat && LogLevels.Info >= logLevelSetting) {
				message = msgPrefix + LogLevels.Heartbeat.ToString() + " -- " + inputMsg;
			}

			//Empty Queue meesages only appear when log is set to INFO or lower
			else if (msgLogLevel == LogLevels.EmptyQueue && LogLevels.Info >= logLevelSetting) {
				message = msgPrefix + LogLevels.EmptyQueue.ToString() + " -- " + inputMsg;
			}

			//App Version message only appear when log is set to FATAL or lower
			else if (msgLogLevel == LogLevels.AppVersion && LogLevels.Fatal >= logLevelSetting) {
				message = msgPrefix + LogLevels.AppVersion.ToString() + " -- " + inputMsg;
			}

			//Event Log message only appears when log is set to FULLDEBUG
			else if(msgLogLevel == LogLevels.EventLog && LogLevels.FullDebug >= logLevelSetting) {
				message = msgPrefix + LogLevels.EventLog.ToString() + " -- " + inputMsg;
			}

			return message;
		}

		private void BufferHandler_DoWork(object? sender, DoWorkEventArgs e)
		{
			BackgroundWorker worker = (BackgroundWorker)sender!;

			int logBufferSize = GetLogBufferSize();
			if (logBufferSize > 0) {
				int i = 0;
				while (i < logBufferSize) {
					if (worker.CancellationPending == true) {
						e.Cancel = true;
						break;
					}

					string nextMessage;
					LogLevels lvl;
					(nextMessage, lvl) = GetNextMessageFromLogBuffer();
					//This check to if the buffer comes back empty is most likely not necessary.
					//However, given that this part is multithreaded, there's a chance wherein if it was expected 5 messages, but the
					// flush function was run and now there are less than 5 messages, this prevents the entire program from crashing out.
					//A special error message has been provided for such a case as this.
					//We also cancel the incrementation of the "i" tracking variable and just set it equal to the number of logs it's expecting

					//Check to make sure that the message isn't empty or null
					if (!string.IsNullOrEmpty(nextMessage)) {
						//Log messages where log level isn't set to null and level isn't task log
						if (!lvl.EqualsAny(LogLevels.None, LogLevels.NullLogValue))
							WriteToLogFile(nextMessage);

						//Regardless of log level, as long as console is enabled, we want to output to the console as well
						WriteToConsole(nextMessage, lvl);

						//Increment "i" so that next message, if there are any more, can be output
						i++;
					}
					else {
						//Log buffer is unexpectedly empty, display as log error
						WriteInternalLog("Expected message was empty or null", LogLevels.LogMsgError);

						//Incrementing loop to continue
						i++;
					}
				}
			}

			e.Cancel = true;
			return;
		}

		private void AddMessageToLogBuffer(string message, LogLevels l)
		{
			lock (bufferLock) {
				logBuffer.Enqueue((message, l));
			}
		}

		private (string, LogLevels) GetNextMessageFromLogBuffer()
		{
			string logMsg;
			LogLevels lv;
			lock (bufferLock) {
				if (logBuffer.Count != 0)
					(logMsg, lv) = logBuffer.Dequeue();
				else {
					logMsg = string.Empty;
					lv = LogLevels.NullLogValue;
				}
			}

			return (logMsg, lv);
		}

		private int GetLogBufferSize()
		{
			int size;
			lock (bufferLock) {
				size = logBuffer.Count;
			}

			return size;
		}

		private void WriteToLogFile(string message)
		{
			lock (bufferLock) {
				if (IsLogFileEnabled) {
					byte[] msgBuffer = Encoding.UTF8.GetBytes(message);
					logFile!.Write(msgBuffer, 0, msgBuffer.Length);
					logFile!.Seek(0, SeekOrigin.End);
				}
			}
		}

		private void WriteToLogFileFromLogger(string message, LogLevels curLogLevel)
		{
			lock (bufferLock) {
				if (IsLogFileEnabled) {
					using StreamWriter file = new("TaskerLog-" + logFileTimestamp + ".log", append: true);
					string msgPrefix = string.Empty;

					//Main Log Levels
					if (curLogLevel == LogLevels.FullDebug && LogLevels.FullDebug >= logLevelSetting) {
						msgPrefix = "FULLDEBUG FUNCTION - ";
					}

					else if (curLogLevel == LogLevels.Debug && LogLevels.Debug >= logLevelSetting) {
						msgPrefix = "DEBUG FUNCTION - ";
					}

					else if (curLogLevel == LogLevels.Info && LogLevels.Info >= logLevelSetting) {
						msgPrefix = "INFO FUNCTION - ";
					}

					else if (curLogLevel == LogLevels.Warn && LogLevels.Warn >= logLevelSetting) {
						msgPrefix = "WARN FUNCTION - ";
					}

					else if (curLogLevel == LogLevels.Error && LogLevels.Error >= logLevelSetting) {
						msgPrefix = "ERROR FUNCTION - ";
					}

					else if (curLogLevel == LogLevels.Fatal && LogLevels.Fatal >= logLevelSetting) {
						msgPrefix = "FATAL FUNCTION - ";
					}

					//Other Log Levels
					//Log Message Errors only appear when log is set to DEBUG
					else if (curLogLevel == LogLevels.LogMsgError && LogLevels.Debug >= logLevelSetting) {
						msgPrefix = "LOGMSGERROR FUNCTION - ";
					}

					//Empty Queue meesages only appear when log is set to INFO or lower
					else if (curLogLevel == LogLevels.EmptyQueue && LogLevels.Info >= logLevelSetting) {
						msgPrefix = "EMPTYQUEUE FUNCTION - ";
					}

					//App Version message only appear when log is set to FATAL or lower
					else if (curLogLevel == LogLevels.AppVersion && LogLevels.Fatal >= logLevelSetting) {
						msgPrefix = "APPVERSION FUNCTION - ";
					}

					//EventLog message only appears when log is set to FULLDEBUG
					else if (curLogLevel == LogLevels.EventLog && LogLevels.FullDebug >= logLevelSetting) {
						msgPrefix = "EVENTLOG FUNCTION - ";
					}

					file.WriteLine(msgPrefix + message);
				}
			}
		}

		private void WriteToConsole(string message, LogLevels curLogLevel)
		{
			lock (bufferLock) {
				if (IsConsoleEnabled) {
					//Set color of console
					if (curLogLevel == LogLevels.FullDebug)
						Console.WriteLine(message.Pastel(Color.SpringGreen).PastelBg(Color.Black));
					else if (curLogLevel == LogLevels.Debug)
						Console.WriteLine(message.Pastel(Color.Black).PastelBg(Color.Ivory));
					else if (curLogLevel == LogLevels.Info)
						Console.WriteLine(message.Pastel(Color.Aqua).PastelBg(Color.Black));
					else if (curLogLevel == LogLevels.Warn)
						Console.WriteLine(message.Pastel(Color.Coral).PastelBg(Color.Black));
					else if (curLogLevel == LogLevels.Error)
						Console.WriteLine(message.Pastel(Color.Crimson).PastelBg(Color.Black));
					else if (curLogLevel == LogLevels.Fatal)
						Console.WriteLine(message.Pastel(Color.White).PastelBg(Color.Crimson));
					else if (curLogLevel == LogLevels.Heartbeat)
						Console.WriteLine(message.Pastel(Color.Black).PastelBg(Color.MediumVioletRed));
					else if (curLogLevel == LogLevels.AppVersion)
						Console.WriteLine(message.Pastel(Color.White).PastelBg(Color.Blue));
					else if (curLogLevel == LogLevels.EmptyQueue)
						Console.WriteLine(message.Pastel(Color.Gold).PastelBg(Color.DodgerBlue));
					else if (curLogLevel == LogLevels.LogMsgError)
						Console.WriteLine(message.Pastel(Color.DeepPink).PastelBg(Color.WhiteSmoke));
					else if (curLogLevel == LogLevels.EventLog)
						Console.WriteLine(message.Pastel(Color.Orchid).PastelBg(Color.Khaki));
				}
			}
		}

		private void WriteToConsoleFromLogger(string message, LogLevels curLogLevel)
		{
			lock (bufferLock) {
				if (IsConsoleEnabled) {
					if (curLogLevel == LogLevels.FullDebug && LogLevels.FullDebug >= logLevelSetting) {
						Console.WriteLine("FULLDEBUG FUNCTION - ".Pastel(Color.Black).PastelBg(Color.SpringGreen) +
										  message.Pastel(Color.SpringGreen).PastelBg(Color.Black));
					}

					else if (curLogLevel == LogLevels.Debug && LogLevels.Debug >= logLevelSetting) {
						Console.WriteLine("DEBUG FUNCTION - ".Pastel(Color.Ivory).PastelBg(Color.Black) +
										  message.Pastel(Color.Black).PastelBg(Color.Ivory));
					}

					else if (curLogLevel == LogLevels.Info && LogLevels.Info >= logLevelSetting) {
						Console.WriteLine("INFO FUNCTION - ".Pastel(Color.Black).PastelBg(Color.Aqua) +
										  message.Pastel(Color.Aqua).PastelBg(Color.Black));
					}

					else if (curLogLevel == LogLevels.Warn && LogLevels.Warn >= logLevelSetting) {
						Console.WriteLine("WARN FUNCTION - ".Pastel(Color.Black).PastelBg(Color.Coral) +
										  message.Pastel(Color.Coral).PastelBg(Color.Black));
					}

					else if (curLogLevel == LogLevels.Error && LogLevels.Error >= logLevelSetting) {
						Console.WriteLine("ERROR FUNCTION - ".Pastel(Color.Black).PastelBg(Color.Crimson) +
										  message.Pastel(Color.Crimson).PastelBg(Color.Black));
					}

					else if (curLogLevel == LogLevels.Fatal && LogLevels.Fatal >= logLevelSetting) {
						Console.WriteLine("FATAL FUNCTION - ".Pastel(Color.Crimson).PastelBg(Color.White) +
										  message.Pastel(Color.White).PastelBg(Color.Crimson));
					}

					else if (curLogLevel == LogLevels.AppVersion && LogLevels.Fatal >= logLevelSetting) {
						Console.WriteLine("APPVERSION FUNCTION - ".Pastel(Color.Blue).PastelBg(Color.White) +
										  message.Pastel(Color.White).PastelBg(Color.Blue));
					}

					else if (curLogLevel == LogLevels.EmptyQueue && LogLevels.Info >= logLevelSetting) {
						Console.WriteLine("EMPTYQUEUE FUNCTION - ".Pastel(Color.DodgerBlue).PastelBg(Color.Gold) +
										  message.Pastel(Color.Gold).PastelBg(Color.DodgerBlue));
					}

					else if (curLogLevel == LogLevels.LogMsgError && LogLevels.FullDebug >= logLevelSetting) {
						Console.WriteLine("LOGMSGERROR FUNCTION - ".Pastel(Color.WhiteSmoke).PastelBg(Color.DeepPink) +
										  message.Pastel(Color.DeepPink).PastelBg(Color.WhiteSmoke));
					}

					else if(curLogLevel == LogLevels.EventLog && LogLevels.FullDebug >= logLevelSetting) {
						Console.WriteLine("EVENTLOG FUNCTION - ".Pastel(Color.Khaki).PastelBg(Color.Orchid) +
										  message.Pastel(Color.Orchid).PastelBg(Color.Khaki));
					}
				}
			}
		}

		private void WriteInternalLog(string message, LogLevels lvl)
		{
			string completeMessage = BuildLogMessage(message, lvl);
			WriteToConsoleFromLogger(completeMessage, lvl);
			WriteToLogFileFromLogger(completeMessage, lvl);
		}

		private void WriteToTaskLog(string message)
		{
			lock (bufferLock) {
				if (IsTaskLoggingEnabled) {
					using StreamWriter file = new("TaskLog-" + logFileTimestamp + ".log", append: true);
					file.WriteLine(message);
				}
			}
		}

		private static string GetDateTimestampWithMillis()
		{
			return DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString();
		}
	}

	public static class LogCalls
	{
#if TASKER_DEBUG
		public static void FullDebug(string message)
		{
			if (IsLoggingEnabled)
				LoggerObj!.UpdateMessageLogBuffer(message, LogLevels.FullDebug);
		}

		public static void Debug(string message)
		{
			if (IsLoggingEnabled)
				LoggerObj!.UpdateMessageLogBuffer(message, LogLevels.Debug);
		}

		public static void Info(string message)
		{
			if (IsLoggingEnabled)
				LoggerObj!.UpdateMessageLogBuffer(message, LogLevels.Info);
		}

		public static void Warn(string message)
		{
			if (IsLoggingEnabled)
				LoggerObj!.UpdateMessageLogBuffer(message, LogLevels.Warn);
		}

		public static void Error(string message)
		{
			if (IsLoggingEnabled)
				LoggerObj!.UpdateMessageLogBuffer(message, LogLevels.Error);
		}

		public static void Fatal(string message)
		{
			if (IsLoggingEnabled)
				LoggerObj!.UpdateMessageLogBuffer(message, LogLevels.Fatal);
		}

		public static void Heartbeat(string message)
		{
			if (IsLoggingEnabled)
				LoggerObj!.UpdateMessageLogBuffer(message, LogLevels.Heartbeat);
		}

		public static void AppVersion(string message)
		{
			if (IsLoggingEnabled)
				LoggerObj!.UpdateMessageLogBuffer(message, LogLevels.AppVersion);
		}

		public static void EmptyQueue(string message)
		{
			if (IsLoggingEnabled)
				LoggerObj!.UpdateMessageLogBuffer(message, LogLevels.FullDebug);
		}

		public static void LogMsgError(string message)
		{
			if (IsLoggingEnabled)
				LoggerObj!.UpdateMessageLogBuffer(message, LogLevels.FullDebug);
		}

		public static void EventLog(string message)
		{
			if (IsLoggingEnabled)
				LoggerObj!.UpdateMessageLogBuffer(message, LogLevels.EventLog);
		}
#else
		public static void FullDebug(string message) {}
		public static void Debug(string message) {}
		public static void Info(string message) {}
		public static void Warn(string message) {}
		public static void Error(string message) {}
		public static void Fatal(string message) {}
		public static void Heartbeat(string message) {}
		public static void AppVersion(string message) {}
		public static void EmptyQueue(string message) {}
		public static void LogMsgError(string message) {}
		public static void EventLog(string message) {}
#endif
	}
}