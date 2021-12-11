namespace Tasker
{
	partial class TaskEntryWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.startTimeLabel = new System.Windows.Forms.Label();
			this.timeBlockLabel = new System.Windows.Forms.Label();
			this.timeSpentLabel = new System.Windows.Forms.Label();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.keyEntryTextBox = new System.Windows.Forms.TextBox();
			this.descriptionTextBox = new System.Windows.Forms.TextBox();
			this.timeSpentTextBox = new System.Windows.Forms.TextBox();
			this.clearEntryButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			//
			//	startTimeLabel
			//
			this.startTimeLabel.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.startTimeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.startTimeLabel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.startTimeLabel.Location = new System.Drawing.Point(34, 27);
			this.startTimeLabel.Name = "startTimeLabel";
			this.startTimeLabel.Size = new System.Drawing.Size(101, 30);
			this.startTimeLabel.TabIndex = 41;
			this.startTimeLabel.Text = "Key";
			this.startTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//	timeBlockLabel
			//
			this.timeBlockLabel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.timeBlockLabel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.timeBlockLabel.Location = new System.Drawing.Point(251, 26);
			this.timeBlockLabel.Name = "timeBlockLabel";
			this.timeBlockLabel.Size = new System.Drawing.Size(179, 31);
			this.timeBlockLabel.TabIndex = 44;
			this.timeBlockLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//	timeSpentLabel
			//
			this.timeSpentLabel.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.timeSpentLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.timeSpentLabel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.timeSpentLabel.Location = new System.Drawing.Point(15, 69);
			this.timeSpentLabel.Name = "timeSpentLabel";
			this.timeSpentLabel.Size = new System.Drawing.Size(122, 30);
			this.timeSpentLabel.TabIndex = 43;
			this.timeSpentLabel.Text = "Time Spent";
			this.timeSpentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//	descriptionLabel
			//
			this.descriptionLabel.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.descriptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.descriptionLabel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.descriptionLabel.Location = new System.Drawing.Point(13, 109);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(122, 30);
			this.descriptionLabel.TabIndex = 43;
			this.descriptionLabel.Text = "Description";
			this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//	keyEntryTextBox
			//
			this.keyEntryTextBox.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.keyEntryTextBox.Location = new System.Drawing.Point(141, 27);
			this.keyEntryTextBox.MaxLength = 30;
			this.keyEntryTextBox.Name = "keyEntryTextBox";
			this.keyEntryTextBox.Size = new System.Drawing.Size(84, 29);
			this.keyEntryTextBox.TabIndex = 0;
			this.keyEntryTextBox.WordWrap = false;
			this.keyEntryTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.KeyEntryTextBox_Validating);
			this.keyEntryTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyEntryTextBox_MouseDown);
			this.keyEntryTextBox.MouseHover += new System.EventHandler(this.KeyEntryTextBox_MouseHover);
			this.keyEntryTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.KeyEntryTextBox_MouseMove);
			this.keyEntryTextBox.MouseLeave += new System.EventHandler(this.KeyEntryTextBox_MouseLeave);
			this.keyEntryTextBox.Leave += new System.EventHandler(this.KeyEntryTextBox_Leave);
			this.keyEntryTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.KeyEntryTextBox_PreviewKeyDown);
			//
			//	timeSpentTextBox
			//
			this.timeSpentTextBox.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.timeSpentTextBox.Location = new System.Drawing.Point(141, 69);
			this.timeSpentTextBox.MaxLength = 5;
			this.timeSpentTextBox.Name = "timeSpentTextBox";
			this.timeSpentTextBox.Size = new System.Drawing.Size(84, 29);
			this.timeSpentTextBox.TabIndex = 1;
			this.timeSpentTextBox.WordWrap = false;
			this.timeSpentTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TimeSpentTextBox_Validating);
			this.timeSpentTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TimeSpentTextBox_MouseDown);
			this.timeSpentTextBox.MouseHover += new System.EventHandler(this.TimeSpentTextBox_MouseHover);
			this.timeSpentTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimeSpentTextBox_MouseMove);
			this.timeSpentTextBox.MouseLeave += new System.EventHandler(this.TimeSpentTextBox_MouseLeave);
			this.timeSpentTextBox.Leave += new System.EventHandler(this.TimeSpentTextBox_Leave);
			this.timeSpentTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TimeSpentTextBox_PreviewKeyDown);
			//
			//	descriptionTextBox
			//
			this.descriptionTextBox.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.descriptionTextBox.Location = new System.Drawing.Point(141, 109);
			this.descriptionTextBox.MaxLength = 500;
			this.descriptionTextBox.Multiline = true;
			this.descriptionTextBox.Name = "descriptionTextBox";
			this.descriptionTextBox.Size = new System.Drawing.Size(289, 75);
			this.descriptionTextBox.TabIndex = 2;
			this.descriptionTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DescriptionTextBox_Validating);
			this.descriptionTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DescriptionTextBox_MouseDown);
			this.descriptionTextBox.MouseHover += new System.EventHandler(this.DescriptionTextBox_MouseHover);
			this.descriptionTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DescriptionTextBox_MouseMove);
			this.descriptionTextBox.MouseLeave += new System.EventHandler(this.DescriptionTextBox_MouseLeave);
			this.descriptionTextBox.Leave += new System.EventHandler(this.DescriptionTextBox_Leave);
			this.descriptionTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.DescriptionTextBox_PreviewKeyDown);

			//
			//	clearEntryButton
			//
			this.clearEntryButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.clearEntryButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.clearEntryButton.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.clearEntryButton.Location = new System.Drawing.Point(466, 109);
			this.clearEntryButton.Name = "clearEntryButton";
			this.clearEntryButton.Size = new System.Drawing.Size(96, 76);
			this.clearEntryButton.TabIndex = 4;
			this.clearEntryButton.Text = "Clear Entry";
			this.clearEntryButton.UseVisualStyleBackColor = false;
			this.clearEntryButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ClearEntryButton_PreviewKeyDown);
			this.clearEntryButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ClearEntryButton_MouseDown);
			this.clearEntryButton.MouseHover += new System.EventHandler(this.ClearEntryButton_MouseHover);
			this.clearEntryButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ClearEntryButton_MouseMove);
			this.clearEntryButton.MouseEnter += new System.EventHandler(this.ClearEntryButton_MouseEnter);
			this.clearEntryButton.MouseLeave += new System.EventHandler(this.ClearEntryButton_MouseLeave);
			this.clearEntryButton.Leave += new System.EventHandler(this.ClearEntryButton_Leave);
			//
			// okButton
			//
			this.okButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.okButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.okButton.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.okButton.Location = new System.Drawing.Point(466, 22);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(96, 76);
			this.okButton.TabIndex = 3;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = false;
			this.okButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OkButton_PreviewKeyDown);
			this.okButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OkButton_MouseDown);
			this.okButton.MouseHover += new System.EventHandler(this.OkButton_MouseHover);
			this.okButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OkButton_MouseMove);
			this.okButton.MouseEnter += new System.EventHandler(this.OkButton_MouseEnter);
			this.okButton.MouseLeave += new System.EventHandler(this.OkButton_MouseLeave);
			this.okButton.Leave += new System.EventHandler(this.OkButton_Leave);
			//
			// taskEntryWindow
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);			
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(577, 198);
			this.Controls.Add(this.timeBlockLabel);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.clearEntryButton);
			this.Controls.Add(this.timeSpentTextBox);
			this.Controls.Add(this.descriptionTextBox);
			this.Controls.Add(this.keyEntryTextBox);
			this.Controls.Add(this.timeSpentLabel);
			this.Controls.Add(this.descriptionLabel);
			this.Controls.Add(startTimeLabel);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(593, 237);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(593, 237);
			this.Name = "taskEntryWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Task Entry";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskEntryWindow_FormClosing);
			this.Shown += new System.EventHandler(this.TaskEntryWindow_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label startTimeLabel;
		private System.Windows.Forms.Label timeBlockLabel;
		private System.Windows.Forms.Label timeSpentLabel;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.TextBox keyEntryTextBox;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.TextBox timeSpentTextBox;
		private System.Windows.Forms.Button clearEntryButton;
		private System.Windows.Forms.Button okButton;

	}
}