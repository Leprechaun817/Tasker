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
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;


using static Tasker.TaskerVariables;
using static Tasker.LogCalls;

namespace Tasker
{
	public class TaskEntryDebugButton
	{
		private readonly TaskerMainWindow taskerMainWin;
		
		private readonly Form taskEntryDebugTimeBlockForm = new Form();
		private readonly Label timeBlockStartLabel = new Label();
		private readonly Label timeBlockEndLabel = new Label();
		private readonly TextBox timeBlockStartTextBox = new TextBox();
		private readonly TextBox timeBlockEndTextBox = new TextBox();
		private readonly Button continueButton = new Button();
		
		private readonly Form taskEntryDbgFrm = new Form();
		private readonly Button taskEntryDbgBtn = new Button();

		private readonly object debugLock = new object();

		private readonly Queue<Entry> debugEntryQueue = new Queue<Entry>();


		private bool quit = false;

		public bool QuitTaskEntryDbgBtn
		{
			set
			{
				lock(debugLock) {
					quit = value;
				}
			}
		}

		public TaskEntryDebugButton(TaskerMainWindow mWin)
		{
			taskerMainWin = mWin;
			
			SetupTaskEntryDebugTimeBlockForm();

			taskEntryDbgBtn.Name = "taskEntryDbgBtn";
			taskEntryDbgBtn.Text = "Create New Task Entry Window";
			taskEntryDbgBtn.TextAlign = ContentAlignment.MiddleCenter;
			taskEntryDbgBtn.UseVisualStyleBackColor = false;
			taskEntryDbgBtn.BackColor = SystemColors.ButtonShadow;
			taskEntryDbgBtn.ForeColor = SystemColors.ControlText;
			taskEntryDbgBtn.Cursor = Cursors.Hand;
			taskEntryDbgBtn.FlatStyle = FlatStyle.Popup;
			taskEntryDbgBtn.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
			taskEntryDbgBtn.Location = new Point(5, 5);
			taskEntryDbgBtn.Size = new Size(145, 90);
			taskEntryDbgBtn.TabIndex = 0;
			taskEntryDbgBtn.Enabled = true;

			taskEntryDbgBtn.MouseDown += new MouseEventHandler(TaskEntryDbgBtn_MouseDown);
			taskEntryDbgBtn.PreviewKeyDown += new PreviewKeyDownEventHandler(TaskEntryDbgBtn_PreviewKeyDown);

			taskEntryDbgFrm.SuspendLayout();
			taskEntryDbgFrm.Name = "taskEntryDbgFrm";
			taskEntryDbgFrm.Text = "Task Entry Debug Button";
			taskEntryDbgFrm.AutoScaleDimensions = new SizeF(7F, 15F);
			taskEntryDbgFrm.AutoScaleMode = AutoScaleMode.Font;
			taskEntryDbgFrm.ClientSize = new Size(155, 100);
			taskEntryDbgFrm.BackColor = SystemColors.Menu;
			taskEntryDbgFrm.ForeColor = SystemColors.ControlText;
			taskEntryDbgFrm.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			taskEntryDbgFrm.MaximizeBox = false;
			taskEntryDbgFrm.MinimizeBox = false;
			taskEntryDbgFrm.ControlBox = false;
			taskEntryDbgFrm.ShowInTaskbar = false;
			taskEntryDbgFrm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			taskEntryDbgFrm.StartPosition = FormStartPosition.CenterParent;
			taskEntryDbgFrm.CausesValidation = false;
			taskEntryDbgFrm.ShowIcon = false;
			taskEntryDbgFrm.Controls.Add(taskEntryDbgBtn);
			taskEntryDbgFrm.ResumeLayout();
			taskEntryDbgFrm.PerformLayout();
			taskEntryDbgFrm.Show();

			ThreadStart debugTaskEntryDelegate = new ThreadStart(delegate ()
			{
				EntryQueueWorker();
			});

			Thread debugTaskEntryThread = new Thread(debugTaskEntryDelegate);
			debugTaskEntryThread.Start();
		}

		private void SetupTaskEntryDebugTimeBlockForm()
		{
			//Setup for timeBlockStartLabel
			timeBlockStartLabel.AutoSize = true;
			timeBlockStartLabel.Font = new Font("Arial", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
			timeBlockStartLabel.Location = new Point(2, 13);
			timeBlockStartLabel.MinimumSize = new Size(162, 24);
			timeBlockStartLabel.Name = "timeBlockStartLabel";
			timeBlockStartLabel.Size = new Size(162, 24);
			timeBlockStartLabel.TabIndex = 9;
			timeBlockStartLabel.Text = "Time Block Start";
			timeBlockStartLabel.TextAlign = ContentAlignment.MiddleRight;

			//Setup for timeBlockEndLabel
			timeBlockEndLabel.AutoSize = true;
			timeBlockEndLabel.Font = new Font("Arial", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
			timeBlockEndLabel.Location = new Point(2, 48);
			timeBlockEndLabel.MinimumSize = new Size(162, 24);
			timeBlockEndLabel.Name = "timeBlockEndLabel";
			timeBlockEndLabel.Size = new Size(162, 24);
			timeBlockEndLabel.TabIndex = 8;
			timeBlockEndLabel.Text = "Time Block End";
			timeBlockEndLabel.TextAlign = ContentAlignment.MiddleRight;

			//Setup for timeBlockStartTextBox
			timeBlockStartTextBox.Font = new Font("Arial", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
			timeBlockStartTextBox.Location = new Point(170, 11);
			timeBlockStartTextBox.MaxLength = 5;
			timeBlockStartTextBox.Name = "timeBloclStartTextBox";
			timeBlockStartTextBox.Size = new Size(84, 29);
			timeBlockStartTextBox.TabIndex = 0;
			timeBlockStartTextBox.WordWrap = false;

			//Setup for timeBlockEndTextBox
			timeBlockEndTextBox.Font = new Font("Arial", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
			timeBlockEndTextBox.Location = new Point(170, 46);
			timeBlockEndTextBox.MaxLength = 5;
			timeBlockEndTextBox.Name = "timeBlockEndTextBox";
			timeBlockEndTextBox.Size = new Size(84, 29);
			timeBlockEndTextBox.TabIndex = 1;
			timeBlockEndTextBox.WordWrap = false;

			//Setup for Continue Button
			continueButton.BackColor = SystemColors.ButtonShadow;
			continueButton.ForeColor = SystemColors.ControlText;
			continueButton.Cursor = Cursors.Hand;
			continueButton.FlatStyle = FlatStyle.Popup;
			continueButton.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
			continueButton.Location = new Point(273, 11);
			continueButton.Name = "continueButton";
			continueButton.Size = new Size(92, 64);
			continueButton.TabIndex = 2;
			continueButton.Text = "Continue";
			continueButton.UseVisualStyleBackColor = false;
			continueButton.MouseDown += new MouseEventHandler(ContinueButton_MouseDown);
			continueButton.PreviewKeyDown += new PreviewKeyDownEventHandler(ContinueButton_PreviewKeyDown);

			//Setup for Task Entry Debug Time Block Form
			taskEntryDebugTimeBlockForm.SuspendLayout();
			taskEntryDebugTimeBlockForm.Name = "taskEntryDebugTimeBlockForm";
			taskEntryDebugTimeBlockForm.Text = "Task Entry Window Debug Time Block";
			taskEntryDebugTimeBlockForm.AutoScaleDimensions = new SizeF(7F, 15F);
			taskEntryDebugTimeBlockForm.AutoScaleMode = AutoScaleMode.Font;
			taskEntryDebugTimeBlockForm.ClientSize = new Size(375, 83);
			taskEntryDebugTimeBlockForm.BackColor = SystemColors.Window;
			taskEntryDebugTimeBlockForm.ForeColor = SystemColors.ControlText;
			taskEntryDebugTimeBlockForm.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			taskEntryDebugTimeBlockForm.MaximizeBox = false;
			taskEntryDebugTimeBlockForm.MinimizeBox = false;
			taskEntryDebugTimeBlockForm.ControlBox = false;
			taskEntryDebugTimeBlockForm.ShowInTaskbar = false;
			taskEntryDebugTimeBlockForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			taskEntryDebugTimeBlockForm.StartPosition = FormStartPosition.CenterParent;
			taskEntryDebugTimeBlockForm.CausesValidation = false;
			taskEntryDebugTimeBlockForm.ShowIcon = false;
			taskEntryDebugTimeBlockForm.Controls.Add(timeBlockStartLabel);
			taskEntryDebugTimeBlockForm.Controls.Add(timeBlockEndLabel);
			taskEntryDebugTimeBlockForm.Controls.Add(timeBlockStartTextBox);
			taskEntryDebugTimeBlockForm.Controls.Add(timeBlockEndTextBox);
			taskEntryDebugTimeBlockForm.Controls.Add(continueButton);
			taskEntryDebugTimeBlockForm.ResumeLayout(false);
			taskEntryDebugTimeBlockForm.PerformLayout();
		}

		private void TaskEntryDbgBtn_MouseDown(object? sender, MouseEventArgs e)
		{
			Entry newDebugEntry = new Entry();
			AddDebugEntryToQueue(newDebugEntry);
		}

		private void TaskEntryDbgBtn_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
		{
			if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
				Entry newDebugEntry = new Entry();
				AddDebugEntryToQueue(newDebugEntry);
			}
		}

		private void ContinueButton_MouseDown(object? sender, MouseEventArgs e)
		{	
			taskEntryDebugTimeBlockForm.Hide();
			taskEntryDebugTimeBlockForm.Close();
		}

		private void ContinueButton_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
		{
			if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
				taskEntryDebugTimeBlockForm.Hide();
				taskEntryDebugTimeBlockForm.Close();
			}
		}

		private void GetTimestampsForDebugEntry(ref Entry newDebugEntry)
		{
			taskEntryDebugTimeBlockForm.ShowDialog();

			newDebugEntry.startTimestamp = ((DateTime.Parse(timeBlockStartTextBox.Text).Hour * 60) + DateTime.Parse(timeBlockStartTextBox.Text).Minute);
			newDebugEntry.endTimestamp = ((DateTime.Parse(timeBlockStartTextBox.Text).Hour * 60) + DateTime.Parse(timeBlockEndTextBox.Text).Minute);

			newDebugEntry.entryWindow = new TaskEntryWindow(newDebugEntry.startTimestamp.ToTimeString() + " - " + newDebugEntry.endTimestamp.ToTimeString(), taskerMainWin);
		}

		private void AddDebugEntryToQueue(Entry e)
		{
			lock(debugLock) {
				debugEntryQueue.Enqueue(e);
			}
		}

		private Entry GetEntryFromDebugQueue()
		{
			Entry returnEntry;
			lock(debugLock) {
				returnEntry = debugEntryQueue.Dequeue();
			}

			return returnEntry;
		}

		private int GetEntryQueueSize()
		{
			int size;
			lock(debugLock) {
				size = debugEntryQueue.Count;
			}

			return size;
		}

		private void EntryQueueWorker()
		{
			bool varQuit;
			lock(debugLock) {
				varQuit = quit;
			}
			while(!varQuit) {
				Entry e;
				if (GetEntryQueueSize() > 0) {
					Entry entry = GetEntryFromDebugQueue();

					GetTimestampsForDebugEntry(ref entry);

					ref TaskEntryWindow debugEntryWin = ref entry.entryWindow;
					debugEntryWin.Enabled = true;
					debugEntryWin.ShowInTaskbar = true;

					debugEntryWin.FlashNotification();
					debugEntryWin.ShowDialog();

					e.key = debugEntryWin.GetKeyEntryText();
					e.time = debugEntryWin.GetTimeSpentText();
					e.description = debugEntryWin.GetDescriptionText();

					if (debugEntryWin.WasTimeEntryForceClosed()) {
						debugEntryWin.Close();
						debugEntryWin.Dispose();
					}
					else
						debugEntryWin.Dispose();
				}
				else
					Thread.Sleep(1000);

				lock(debugLock) {
					varQuit = quit;
				}
			}
		}
	}
}