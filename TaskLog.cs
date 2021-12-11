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
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using Pastel;

using static Tasker.TaskerVariables;

namespace Tasker
{
	public sealed class TaskLog
	{
		private readonly Queue<string> taskLogBuffer = new Queue<string>();

		private readonly string taskLogFileTimestamp;

		private readonly BackgroundWorker taskLogBufferHandler;
		private readonly object bufferLock = new object();

		private readonly FileStream? taskLogFile;

		public TaskLog()
		{
			taskLogBufferHandler = new BackgroundWorker() { WorkerSupportsCancellation = true };
			taskLogBufferHandler.DoWork += new DoWorkEventHandler(TaskLogBufferHandler_DoWork);

			taskLogFileTimestamp = DateTime.Now.ToLongDateString() + "-" + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString();

			string taskLogPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			taskLogPath = Path.Combine(taskLogPath, "TaskApp\\TaskLog");
			string taskLogFilePath = Path.Combine(taskLogPath, "TaskLog-" + taskLogFileTimestamp + ".log");

			if(!File.Exists(taskLogFilePath)) {
				if(!Directory.Exists(taskLogPath)) {
					Directory.CreateDirectory(taskLogPath);
				}

				taskLogFile = new FileStream(taskLogFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4, FileOptions.SequentialScan);
				taskLogFile.Seek(0, SeekOrigin.Begin);
			}
			else {
				taskLogFile = new FileStream(taskLogFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4, FileOptions.SequentialScan);
				taskLogFile.Seek(0, SeekOrigin.End);
			}
		}

		~TaskLog()
		{
			FlushTaskLogBuffer();

			taskLogFile!.Close();
			taskLogFile!.Dispose();
		}

		private void TaskLogBufferHandler_DoWork(object? sender, DoWorkEventArgs e)
		{
			BackgroundWorker worker = (BackgroundWorker)sender!;

			int bufferSize = GetTaskLogBufferSize();
			if(bufferSize > 0) {
				int i = 0;
				while(i < bufferSize) {
					if(worker.CancellationPending) {
						e.Cancel = true;
						break;
					}

					string nextMessage = GetNextMessageFromLogBuffer();

					if(!string.IsNullOrEmpty(nextMessage)) {
						if (IsConsoleEnabled)
							Console.WriteLine(nextMessage.Pastel(Color.Turquoise).PastelBg(Color.Black));

						byte[] msgBuffer = Encoding.UTF8.GetBytes(nextMessage);
						taskLogFile!.Write(msgBuffer, 0, msgBuffer.Length);
						taskLogFile!.Seek(0, SeekOrigin.End);
					}

					i++;
				}
			}

			e.Cancel = true;
			return;
		}

		public void UpdateTaskLogBuffer(string inputMsg)
		{
			if(IsTaskLoggingEnabled) {
				string taskLogMsg = DateTime.Now.ToLongDateString() + " -- TASKLOG -- " + inputMsg;
				AddTaskLogMsgToBuffer(taskLogMsg);

				if (!taskLogBufferHandler.IsBusy)
					taskLogBufferHandler.RunWorkerAsync();
			}
		}

		private void AddTaskLogMsgToBuffer(string msg)
		{
			lock(bufferLock) {
				taskLogBuffer.Enqueue(msg);
			}
		}

		private string GetNextMessageFromLogBuffer()
		{
			string taskLogMsg;
			lock(bufferLock) {
				if (taskLogBuffer.Count != 0)
					taskLogMsg = taskLogBuffer.Dequeue();
				else
					taskLogMsg = String.Empty;
			}

			return taskLogMsg;
		}

		private int GetTaskLogBufferSize()
		{
			int size;
			lock(bufferLock) {
				size = taskLogBuffer.Count;
			}

			return size;
		}

		private void FlushTaskLogBuffer()
		{
			if (taskLogBufferHandler.IsBusy) {
				taskLogBufferHandler.CancelAsync();
				taskLogBufferHandler.Dispose();
			}
			else
				taskLogBufferHandler.Dispose();

			int remainingLogs = GetTaskLogBufferSize();
			if(remainingLogs != 0) {
				int i = 0;
				while(i < remainingLogs) {
					string message = GetNextMessageFromLogBuffer();
					if(!string.IsNullOrEmpty(message)) {
						if (IsConsoleEnabled)
							Console.WriteLine(message.Pastel(Color.Turquoise).PastelBg(Color.Black));

						byte[] msgBuffer = Encoding.UTF8.GetBytes(message);
						taskLogFile!.Write(msgBuffer, 0, msgBuffer.Length);
						taskLogFile!.Seek(0, SeekOrigin.End);

						i++;
					}
					else {
						i = remainingLogs;
					}
				}
			}
		}
	}
}