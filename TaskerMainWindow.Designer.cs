namespace Tasker
{
	partial class TaskerMainWindow
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.startTimeLabel = new System.Windows.Forms.Label();
			this.endTimeLabel = new System.Windows.Forms.Label();
			this.remindIntervLabel = new System.Windows.Forms.Label();
			this.startTimeTextBox = new System.Windows.Forms.TextBox();
			this.endTimeTextBox = new System.Windows.Forms.TextBox();
			this.remindIntervTextBox = new System.Windows.Forms.TextBox();
			this.startTrackButton = new System.Windows.Forms.Button();
			this.stopTrackButton = new System.Windows.Forms.Button();
			this.pauseTrackButton = new System.Windows.Forms.Button();
			this.listView1 = new Tasker.ListViewWithScrollEvent();
			this.keyColumn = new System.Windows.Forms.ColumnHeader();
			this.timeColumn = new System.Windows.Forms.ColumnHeader();
			this.timeLoggedColumn = new System.Windows.Forms.ColumnHeader();
			this.descriptionColumn = new System.Windows.Forms.ColumnHeader();
			this.listViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copySelectCell = new System.Windows.Forms.ToolStripMenuItem();
			this.copySelectLine = new System.Windows.Forms.ToolStripMenuItem();
			this.taskerMenu = new System.Windows.Forms.MenuStrip();
			this.taskerMenuActionsItem = new System.Windows.Forms.ToolStripMenuItem();
			this.startTrackingMenuButton = new System.Windows.Forms.ToolStripMenuItem();
			this.pauseTrackingMenuButton = new System.Windows.Forms.ToolStripMenuItem();
			this.stopTrackingMenuButton = new System.Windows.Forms.ToolStripMenuItem();
			this.exportLogMenuButton = new System.Windows.Forms.ToolStripMenuItem();
			this.quitMenuButton = new System.Windows.Forms.ToolStripMenuItem();
			this.taskerMenuSettingsItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsSaveOptionMenuButton = new System.Windows.Forms.ToolStripMenuItem();
			this.logExportOptionsMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.autoExportTaskListMenuButton = new System.Windows.Forms.ToolStripMenuItem();
			this.saveLocationExportMenuButton = new System.Windows.Forms.ToolStripMenuItem();
			this.quitButton = new System.Windows.Forms.Button();
			this.listViewContextMenu.SuspendLayout();
			this.taskerMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// startTimeLabel
			// 
			this.startTimeLabel.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.startTimeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.startTimeLabel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.startTimeLabel.Location = new System.Drawing.Point(26, 34);
			this.startTimeLabel.MinimumSize = new System.Drawing.Size(165, 30);
			this.startTimeLabel.Name = "startTimeLabel";
			this.startTimeLabel.Size = new System.Drawing.Size(165, 30);
			this.startTimeLabel.TabIndex = 40;
			this.startTimeLabel.Text = "Start Time";
			this.startTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// endTimeLabel
			// 
			this.endTimeLabel.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.endTimeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.endTimeLabel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.endTimeLabel.Location = new System.Drawing.Point(26, 76);
			this.endTimeLabel.MinimumSize = new System.Drawing.Size(165, 30);
			this.endTimeLabel.Name = "endTimeLabel";
			this.endTimeLabel.Size = new System.Drawing.Size(165, 30);
			this.endTimeLabel.TabIndex = 41;
			this.endTimeLabel.Text = "End Time";
			this.endTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// remindIntervLabel
			// 
			this.remindIntervLabel.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.remindIntervLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.remindIntervLabel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.remindIntervLabel.Location = new System.Drawing.Point(26, 117);
			this.remindIntervLabel.MinimumSize = new System.Drawing.Size(165, 30);
			this.remindIntervLabel.Name = "remindIntervLabel";
			this.remindIntervLabel.Size = new System.Drawing.Size(165, 30);
			this.remindIntervLabel.TabIndex = 42;
			this.remindIntervLabel.Text = "Reminder Time";
			this.remindIntervLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// startTimeTextBox
			// 
			this.startTimeTextBox.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.startTimeTextBox.Location = new System.Drawing.Point(211, 34);
			this.startTimeTextBox.MaxLength = 5;
			this.startTimeTextBox.Name = "startTimeTextBox";
			this.startTimeTextBox.Size = new System.Drawing.Size(84, 29);
			this.startTimeTextBox.TabIndex = 0;
			this.startTimeTextBox.WordWrap = false;
			this.startTimeTextBox.TextChanged += new System.EventHandler(this.StartTimeTextBox_TextChanged);
			this.startTimeTextBox.Leave += new System.EventHandler(this.StartTimeTextBox_Leave);
			this.startTimeTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StartTimeTextBox_MouseDown);
			this.startTimeTextBox.MouseLeave += new System.EventHandler(this.StartTimeTextBox_MouseLeave);
			this.startTimeTextBox.MouseHover += new System.EventHandler(this.StartTimeTextBox_MouseHover);
			this.startTimeTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.StartTimeTextBox_MouseMove);
			this.startTimeTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.StartTimeTextBox_PreviewKeyDown);
			this.startTimeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.StartTimeTextBox_Validating);
			// 
			// endTimeTextBox
			// 
			this.endTimeTextBox.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.endTimeTextBox.Location = new System.Drawing.Point(211, 76);
			this.endTimeTextBox.MaxLength = 5;
			this.endTimeTextBox.Name = "endTimeTextBox";
			this.endTimeTextBox.Size = new System.Drawing.Size(84, 29);
			this.endTimeTextBox.TabIndex = 1;
			this.endTimeTextBox.WordWrap = false;
			this.endTimeTextBox.TextChanged += new System.EventHandler(this.EndTimeTextBox_TextChanged);
			this.endTimeTextBox.Leave += new System.EventHandler(this.EndTimeTextBox_Leave);
			this.endTimeTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EndTimeTextBox_MouseDown);
			this.endTimeTextBox.MouseLeave += new System.EventHandler(this.EndTimeTextBox_MouseLeave);
			this.endTimeTextBox.MouseHover += new System.EventHandler(this.EndTimeTextBox_MouseHover);
			this.endTimeTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EndTimeTextBox_MouseMove);
			this.endTimeTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.EndTimeTextBox_PreviewKeyDown);
			this.endTimeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.EndTimeTextBox_Validating);
			// 
			// remindIntervTextBox
			// 
			this.remindIntervTextBox.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.remindIntervTextBox.Location = new System.Drawing.Point(211, 117);
			this.remindIntervTextBox.MaxLength = 5;
			this.remindIntervTextBox.Name = "remindIntervTextBox";
			this.remindIntervTextBox.Size = new System.Drawing.Size(84, 29);
			this.remindIntervTextBox.TabIndex = 2;
			this.remindIntervTextBox.WordWrap = false;
			this.remindIntervTextBox.TextChanged += new System.EventHandler(this.RemindIntervTextBox_TextChanged);
			this.remindIntervTextBox.Leave += new System.EventHandler(this.RemindIntervTextBox_Leave);
			this.remindIntervTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RemindIntervTextBox_MouseDown);
			this.remindIntervTextBox.MouseLeave += new System.EventHandler(this.RemindIntervTextBox_MouseLeave);
			this.remindIntervTextBox.MouseHover += new System.EventHandler(this.RemindIntervTextBox_MouseHover);
			this.remindIntervTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RemindIntervTextBox_MouseMove);
			this.remindIntervTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.RemindIntervTextBox_PreviewKeyDown);
			this.remindIntervTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.RemindIntervTextBox_Validating);
			// 
			// startTrackButton
			// 
			this.startTrackButton.BackColor = System.Drawing.Color.MediumSeaGreen;
			this.startTrackButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.startTrackButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.startTrackButton.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.startTrackButton.Location = new System.Drawing.Point(12, 162);
			this.startTrackButton.Name = "startTrackButton";
			this.startTrackButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.startTrackButton.Size = new System.Drawing.Size(92, 65);
			this.startTrackButton.TabIndex = 3;
			this.startTrackButton.Text = "Time Tracker Running";
			this.startTrackButton.UseVisualStyleBackColor = false;
			this.startTrackButton.Leave += new System.EventHandler(this.StartTrackButton_Leave);
			this.startTrackButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StartTrackButton_MouseDown);
			this.startTrackButton.MouseEnter += new System.EventHandler(this.StartTrackButton_MouseEnter);
			this.startTrackButton.MouseLeave += new System.EventHandler(this.StartTrackButton_MouseLeave);
			this.startTrackButton.MouseHover += new System.EventHandler(this.StartTrackButton_MouseHover);
			this.startTrackButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.StartTrackButton_MouseMove);
			this.startTrackButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.StartTrackButton_PreviewKeyDown);
			// 
			// stopTrackButton
			// 
			this.stopTrackButton.BackColor = System.Drawing.Color.IndianRed;
			this.stopTrackButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.stopTrackButton.Enabled = false;
			this.stopTrackButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.stopTrackButton.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.stopTrackButton.Location = new System.Drawing.Point(227, 162);
			this.stopTrackButton.Name = "stopTrackButton";
			this.stopTrackButton.Size = new System.Drawing.Size(92, 65);
			this.stopTrackButton.TabIndex = 4;
			this.stopTrackButton.Text = "Time Tracker Stopped";
			this.stopTrackButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.stopTrackButton.UseVisualStyleBackColor = false;
			this.stopTrackButton.Leave += new System.EventHandler(this.StopTrackButton_Leave);
			this.stopTrackButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StopTrackButton_MouseDown);
			this.stopTrackButton.MouseEnter += new System.EventHandler(this.StopTrackButton_MouseEnter);
			this.stopTrackButton.MouseLeave += new System.EventHandler(this.StopTrackButton_MouseLeave);
			this.stopTrackButton.MouseHover += new System.EventHandler(this.StopTrackButton_MouseHover);
			this.stopTrackButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.StopTrackButton_MouseMove);
			this.stopTrackButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.StopTrackButton_PreviewKeyDown);
			// 
			// pauseTrackButton
			// 
			this.pauseTrackButton.BackColor = System.Drawing.Color.Gold;
			this.pauseTrackButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pauseTrackButton.Enabled = false;
			this.pauseTrackButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.pauseTrackButton.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.pauseTrackButton.Location = new System.Drawing.Point(120, 162);
			this.pauseTrackButton.Name = "pauseTrackButton";
			this.pauseTrackButton.Size = new System.Drawing.Size(92, 65);
			this.pauseTrackButton.TabIndex = 5;
			this.pauseTrackButton.Text = "Time Tracker Paused";
			this.pauseTrackButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.pauseTrackButton.UseVisualStyleBackColor = false;
			this.pauseTrackButton.Leave += new System.EventHandler(this.PauseTrackButton_Leave);
			this.pauseTrackButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PauseTrackButton_MouseDown);
			this.pauseTrackButton.MouseEnter += new System.EventHandler(this.PauseTrackButton_MouseEnter);
			this.pauseTrackButton.MouseLeave += new System.EventHandler(this.PauseTrackButton_MouseLeave);
			this.pauseTrackButton.MouseHover += new System.EventHandler(this.PauseTrackButton_MouseHover);
			this.pauseTrackButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PauseTrackButton_MouseMove);
			this.pauseTrackButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.PauseTrackButton_PreviewKeyDown);
			// 
			// listView1
			// 
			this.listView1.CausesValidation = false;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.keyColumn,
            this.timeColumn,
            this.timeLoggedColumn,
            this.descriptionColumn});
			this.listView1.ContextMenuStrip = this.listViewContextMenu;
			this.listView1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.listView1.HortScrollPos = 0;
			this.listView1.LabelWrap = false;
			this.listView1.Location = new System.Drawing.Point(339, 34);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(693, 380);
			this.listView1.TabIndex = 45;
			this.listView1.TabStop = false;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.VertScrollPos = 0;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.OnScroll += new System.Windows.Forms.ScrollEventHandler(this.ListView1_OnScroll);
			this.listView1.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.ListView1_ColumnWidthChanged);
			this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListView1_MouseDown);
			this.listView1.MouseEnter += new System.EventHandler(this.ListView1_MouseEnter);
			this.listView1.MouseLeave += new System.EventHandler(this.ListView1_MouseLeave);
			this.listView1.MouseHover += new System.EventHandler(this.ListView1_MouseHover);
			this.listView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListView1_MouseMove);
			this.listView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListView1_MouseUp);
			// 
			// keyColumn
			// 
			this.keyColumn.Name = "keyColumn";
			this.keyColumn.Text = "Key";
			// 
			// timeColumn
			// 
			this.timeColumn.Name = "timeColumn";
			this.timeColumn.Text = "Time Spent";
			this.timeColumn.Width = 90;
			// 
			// timeLoggedColumn
			// 
			this.timeLoggedColumn.Name = "timeLoggedColumn";
			this.timeLoggedColumn.Text = "Time Logged";
			this.timeLoggedColumn.Width = 100;
			// 
			// descriptionColumn
			// 
			this.descriptionColumn.Name = "descriptionColumn";
			this.descriptionColumn.Text = "Description";
			this.descriptionColumn.Width = 400;
			// 
			// listViewContextMenu
			// 
			this.listViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copySelectCell,
            this.copySelectLine});
			this.listViewContextMenu.Name = "contextMenuStrip1";
			this.listViewContextMenu.Size = new System.Drawing.Size(175, 48);
			// 
			// copySelectCell
			// 
			this.copySelectCell.Name = "copySelectCell";
			this.copySelectCell.Size = new System.Drawing.Size(174, 22);
			this.copySelectCell.Text = "Copy Selected Cell";
			this.copySelectCell.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CopySelectCell_MouseDown);
			// 
			// copySelectLine
			// 
			this.copySelectLine.Enabled = false;
			this.copySelectLine.Name = "copySelectLine";
			this.copySelectLine.Size = new System.Drawing.Size(174, 22);
			this.copySelectLine.Text = "Copy Selected Line";
			// 
			// taskerMenu
			// 
			this.taskerMenu.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.taskerMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.taskerMenuActionsItem,
            this.taskerMenuSettingsItem});
			this.taskerMenu.Location = new System.Drawing.Point(0, 0);
			this.taskerMenu.Name = "taskerMenu";
			this.taskerMenu.Size = new System.Drawing.Size(1044, 24);
			this.taskerMenu.TabIndex = 46;
			this.taskerMenu.Text = "Tasker Menu";
			// 
			// taskerMenuActionsItem
			// 
			this.taskerMenuActionsItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startTrackingMenuButton,
            this.pauseTrackingMenuButton,
            this.stopTrackingMenuButton,
            this.exportLogMenuButton,
            this.quitMenuButton});
			this.taskerMenuActionsItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.taskerMenuActionsItem.Name = "taskerMenuActionsItem";
			this.taskerMenuActionsItem.Size = new System.Drawing.Size(59, 20);
			this.taskerMenuActionsItem.Text = "Actions";
			this.taskerMenuActionsItem.DropDownClosed += new System.EventHandler(this.TaskerMenuActionsItem_DropDownClosed);
			this.taskerMenuActionsItem.DropDownOpening += new System.EventHandler(this.TaskerMenuActionsItem_DropDownOpening);
			this.taskerMenuActionsItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TaskerMenuActionsItem_MouseDown);
			this.taskerMenuActionsItem.MouseEnter += new System.EventHandler(this.TaskerMenuActionsItem_MouseEnter);
			this.taskerMenuActionsItem.MouseLeave += new System.EventHandler(this.TaskerMenuActionsItem_MouseLeave);
			this.taskerMenuActionsItem.MouseHover += new System.EventHandler(this.TaskerMenuActionsItem_MouseHover);
			this.taskerMenuActionsItem.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskerMenuActionsItem_MouseMove);
			// 
			// startTrackingMenuButton
			// 
			this.startTrackingMenuButton.Name = "startTrackingMenuButton";
			this.startTrackingMenuButton.Size = new System.Drawing.Size(184, 22);
			this.startTrackingMenuButton.Text = "Start Tracking";
			this.startTrackingMenuButton.DropDownClosed += new System.EventHandler(this.StartTrackingMenuButton_DropDownClosed);
			this.startTrackingMenuButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StartTrackingMenuButton_MouseDown);
			this.startTrackingMenuButton.MouseEnter += new System.EventHandler(this.StartTrackingMenuButton_MouseEnter);
			this.startTrackingMenuButton.MouseLeave += new System.EventHandler(this.StartTrackingMenuButton_MouseLeave);
			this.startTrackingMenuButton.MouseHover += new System.EventHandler(this.StartTrackingMenuButton_MouseHover);
			this.startTrackingMenuButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.StartTrackingMenuButton_MouseMove);
			// 
			// pauseTrackingMenuButton
			// 
			this.pauseTrackingMenuButton.Enabled = false;
			this.pauseTrackingMenuButton.Name = "pauseTrackingMenuButton";
			this.pauseTrackingMenuButton.Size = new System.Drawing.Size(184, 22);
			this.pauseTrackingMenuButton.Text = "Pause Tracking";
			this.pauseTrackingMenuButton.DropDownClosed += new System.EventHandler(this.PauseTrackingMenuButton_DropDownClosed);
			this.pauseTrackingMenuButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PauseTrackingMenuButton_MouseDown);
			this.pauseTrackingMenuButton.MouseEnter += new System.EventHandler(this.PauseTrackingMenuButton_MouseEnter);
			this.pauseTrackingMenuButton.MouseLeave += new System.EventHandler(this.PauseTrackingMenuButton_MouseLeave);
			this.pauseTrackingMenuButton.MouseHover += new System.EventHandler(this.PauseTrackingMenuButton_MouseHover);
			this.pauseTrackingMenuButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PauseTrackingMenuButton_MouseMove);
			// 
			// stopTrackingMenuButton
			// 
			this.stopTrackingMenuButton.Enabled = false;
			this.stopTrackingMenuButton.Name = "stopTrackingMenuButton";
			this.stopTrackingMenuButton.Size = new System.Drawing.Size(184, 22);
			this.stopTrackingMenuButton.Text = "Stop Tracking";
			this.stopTrackingMenuButton.DropDownClosed += new System.EventHandler(this.StopTrackingMenuButton_DropDownClosed);
			this.stopTrackingMenuButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StopTrackingMenuButton_MouseDown);
			this.stopTrackingMenuButton.MouseEnter += new System.EventHandler(this.StopTrackingMenuButton_MouseEnter);
			this.stopTrackingMenuButton.MouseLeave += new System.EventHandler(this.StopTrackingMenuButton_MouseLeave);
			this.stopTrackingMenuButton.MouseHover += new System.EventHandler(this.StopTrackingMenuButton_MouseHover);
			this.stopTrackingMenuButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.StopTrackingMenuButton_MouseMove);
			// 
			// exportLogMenuButton
			// 
			this.exportLogMenuButton.Name = "exportLogMenuButton";
			this.exportLogMenuButton.Size = new System.Drawing.Size(184, 22);
			this.exportLogMenuButton.Text = "Export Log to Excel...";
			this.exportLogMenuButton.DropDownClosed += new System.EventHandler(this.ExportLogMenuButton_DropDownClosed);
			this.exportLogMenuButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ExportLogMenuButton_MouseDown);
			this.exportLogMenuButton.MouseEnter += new System.EventHandler(this.ExportLogMenuButton_MouseEnter);
			this.exportLogMenuButton.MouseLeave += new System.EventHandler(this.ExportLogMenuButton_MouseLeave);
			this.exportLogMenuButton.MouseHover += new System.EventHandler(this.ExportLogMenuButton_MouseHover);
			this.exportLogMenuButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ExportLogMenuButton_MouseMove);
			// 
			// quitMenuButton
			// 
			this.quitMenuButton.Name = "quitMenuButton";
			this.quitMenuButton.Size = new System.Drawing.Size(184, 22);
			this.quitMenuButton.Text = "Quit";
			this.quitMenuButton.DropDownClosed += new System.EventHandler(this.QuitMenuButton_DropDownClosed);
			this.quitMenuButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.QuitMenuButton_MouseDown);
			this.quitMenuButton.MouseEnter += new System.EventHandler(this.QuitMenuButton_MouseEnter);
			this.quitMenuButton.MouseLeave += new System.EventHandler(this.QuitMenuButton_MouseLeave);
			this.quitMenuButton.MouseHover += new System.EventHandler(this.QuitMenuButton_MouseHover);
			this.quitMenuButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.QuitMenuButton_MouseMove);
			// 
			// taskerMenuSettingsItem
			// 
			this.taskerMenuSettingsItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsSaveOptionMenuButton,
            this.logExportOptionsMenu});
			this.taskerMenuSettingsItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.taskerMenuSettingsItem.Name = "taskerMenuSettingsItem";
			this.taskerMenuSettingsItem.Size = new System.Drawing.Size(61, 20);
			this.taskerMenuSettingsItem.Text = "Settings";
			this.taskerMenuSettingsItem.DropDownClosed += new System.EventHandler(this.TaskerMenuSettingsItem_DropDownClosed);
			this.taskerMenuSettingsItem.DropDownOpening += new System.EventHandler(this.TaskerMenuSettingsItem_DropDownOpening);
			this.taskerMenuSettingsItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TaskerMenuSettingsItem_MouseDown);
			this.taskerMenuSettingsItem.MouseEnter += new System.EventHandler(this.TaskerMenuSettingsItem_MouseEnter);
			this.taskerMenuSettingsItem.MouseLeave += new System.EventHandler(this.TaskerMenuSettingsItem_MouseLeave);
			this.taskerMenuSettingsItem.MouseHover += new System.EventHandler(this.TaskerMenuSettingsItem_MouseHover);
			this.taskerMenuSettingsItem.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TaskerMenuSettingsItem_MouseMove);
			// 
			// settingsSaveOptionMenuButton
			// 
			this.settingsSaveOptionMenuButton.Name = "settingsSaveOptionMenuButton";
			this.settingsSaveOptionMenuButton.Size = new System.Drawing.Size(192, 22);
			this.settingsSaveOptionMenuButton.Text = "Save Settings on Close";
			this.settingsSaveOptionMenuButton.DropDownClosed += new System.EventHandler(this.SettingsSaveOptionMenuButton_DropDownClosed);
			this.settingsSaveOptionMenuButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SettingsSaveOptionMenuButton_MouseDown);
			this.settingsSaveOptionMenuButton.MouseEnter += new System.EventHandler(this.SettingsSaveOptionMenuButton_MouseEnter);
			this.settingsSaveOptionMenuButton.MouseLeave += new System.EventHandler(this.SettingsSaveOptionMenuButton_MouseLeave);
			this.settingsSaveOptionMenuButton.MouseHover += new System.EventHandler(this.SettingsSaveOptionMenuButton_MouseHover);
			this.settingsSaveOptionMenuButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SettingsSaveOptionMenuButton_MouseMove);
			// 
			// logExportOptionsMenu
			// 
			this.logExportOptionsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoExportTaskListMenuButton,
            this.saveLocationExportMenuButton});
			this.logExportOptionsMenu.Name = "logExportOptionsMenu";
			this.logExportOptionsMenu.Size = new System.Drawing.Size(192, 22);
			this.logExportOptionsMenu.Text = "Log Export Options";
			this.logExportOptionsMenu.DropDownClosed += new System.EventHandler(this.LogExportOptionsMenu_DropDownClosed);
			this.logExportOptionsMenu.DropDownOpening += new System.EventHandler(this.LogExportOptionsMenu_DropDownOpening);
			this.logExportOptionsMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LogExportOptionsMenu_MouseDown);
			this.logExportOptionsMenu.MouseEnter += new System.EventHandler(this.LogExportOptionsMenu_MouseEnter);
			this.logExportOptionsMenu.MouseLeave += new System.EventHandler(this.LogExportOptionsMenu_MouseLeave);
			this.logExportOptionsMenu.MouseHover += new System.EventHandler(this.LogExportOptionsMenu_MouseHover);
			this.logExportOptionsMenu.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LogExportOptionsMenu_MouseMove);
			// 
			// autoExportTaskListMenuButton
			// 
			this.autoExportTaskListMenuButton.Name = "autoExportTaskListMenuButton";
			this.autoExportTaskListMenuButton.Size = new System.Drawing.Size(203, 22);
			this.autoExportTaskListMenuButton.Text = "Auto Export Task List";
			this.autoExportTaskListMenuButton.DropDownClosed += new System.EventHandler(this.AutoExportTaskListMenuButton_DropDownClosed);
			this.autoExportTaskListMenuButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AutoExportTaskListMenuButton_MouseDown);
			this.autoExportTaskListMenuButton.MouseEnter += new System.EventHandler(this.AutoExportTaskListMenuButton_MouseEnter);
			this.autoExportTaskListMenuButton.MouseLeave += new System.EventHandler(this.AutoExportTaskListMenuButton_MouseLeave);
			this.autoExportTaskListMenuButton.MouseHover += new System.EventHandler(this.AutoExportTaskListMenuButton_MouseHover);
			this.autoExportTaskListMenuButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AutoExportTaskListMenuButton_MouseMove);
			// 
			// saveLocationExportMenuButton
			// 
			this.saveLocationExportMenuButton.Name = "saveLocationExportMenuButton";
			this.saveLocationExportMenuButton.Size = new System.Drawing.Size(203, 22);
			this.saveLocationExportMenuButton.Text = "Set Export Save Location";
			this.saveLocationExportMenuButton.DropDownClosed += new System.EventHandler(this.SaveLocationExportMenuButton_DropDownClosed);
			this.saveLocationExportMenuButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SaveLocationExportMenuButton_MouseDown);
			this.saveLocationExportMenuButton.MouseEnter += new System.EventHandler(this.SaveLocationExportMenuButton_MouseEnter);
			this.saveLocationExportMenuButton.MouseLeave += new System.EventHandler(this.SaveLocationExportMenuButton_MouseLeave);
			this.saveLocationExportMenuButton.MouseHover += new System.EventHandler(this.SaveLocationExportMenuButton_MouseHover);
			this.saveLocationExportMenuButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SaveLocationExportMenuButton_MouseMove);
			// 
			// quitButton
			// 
			this.quitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.quitButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.quitButton.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.quitButton.Location = new System.Drawing.Point(12, 342);
			this.quitButton.Name = "quitButton";
			this.quitButton.Size = new System.Drawing.Size(92, 72);
			this.quitButton.TabIndex = 48;
			this.quitButton.Text = "Quit Tasker";
			this.quitButton.UseVisualStyleBackColor = false;
			this.quitButton.Leave += new System.EventHandler(this.QuitButton_Leave);
			this.quitButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.QuitButton_MouseDown);
			this.quitButton.MouseEnter += new System.EventHandler(this.QuitButton_MouseEnter);
			this.quitButton.MouseLeave += new System.EventHandler(this.QuitButton_MouseLeave);
			this.quitButton.MouseHover += new System.EventHandler(this.QuitButton_MouseHover);
			this.quitButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.QuitButton_MouseMove);
			this.quitButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.QuitButton_PreviewKeyDown);
			// 
			// TaskerMainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Menu;
			this.ClientSize = new System.Drawing.Size(1044, 425);
			this.Controls.Add(this.quitButton);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.pauseTrackButton);
			this.Controls.Add(this.stopTrackButton);
			this.Controls.Add(this.startTrackButton);
			this.Controls.Add(this.remindIntervTextBox);
			this.Controls.Add(this.endTimeTextBox);
			this.Controls.Add(this.startTimeTextBox);
			this.Controls.Add(this.remindIntervLabel);
			this.Controls.Add(this.endTimeLabel);
			this.Controls.Add(this.startTimeLabel);
			this.Controls.Add(this.taskerMenu);
			this.MainMenuStrip = this.taskerMenu;
			this.Name = "TaskerMainWindow";
			this.Text = "Tasker v1.0";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskerMainWindow_FormClosing);
			this.Load += new System.EventHandler(this.TaskerMainwindow_FormLoad);
			this.ResizeEnd += new System.EventHandler(this.TaskerMainWindow_ResizeEnd);
			this.listViewContextMenu.ResumeLayout(false);
			this.taskerMenu.ResumeLayout(false);
			this.taskerMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label startTimeLabel;
		private System.Windows.Forms.Label endTimeLabel;
		private System.Windows.Forms.Label remindIntervLabel;
		private System.Windows.Forms.TextBox startTimeTextBox;
		private System.Windows.Forms.TextBox endTimeTextBox;
		private System.Windows.Forms.TextBox remindIntervTextBox;
		private System.Windows.Forms.Button startTrackButton;
		private System.Windows.Forms.Button stopTrackButton;
		private System.Windows.Forms.Button pauseTrackButton;
		private System.Windows.Forms.ColumnHeader keyColumn;
		private System.Windows.Forms.ColumnHeader timeColumn;
		private System.Windows.Forms.ColumnHeader descriptionColumn;
		private System.Windows.Forms.ColumnHeader timeLoggedColumn;
		private System.Windows.Forms.MenuStrip taskerMenu;
		private System.Windows.Forms.ToolStripMenuItem taskerMenuActionsItem;
		private System.Windows.Forms.ToolStripMenuItem startTrackingMenuButton;
		private System.Windows.Forms.ToolStripMenuItem stopTrackingMenuButton;
		private System.Windows.Forms.ToolStripMenuItem pauseTrackingMenuButton;
		private System.Windows.Forms.ToolStripMenuItem taskerMenuSettingsItem;
		private System.Windows.Forms.ToolStripMenuItem settingsSaveOptionMenuButton;
		private System.Windows.Forms.ContextMenuStrip listViewContextMenu;
		private System.Windows.Forms.ToolStripMenuItem copySelectCell;
		private System.Windows.Forms.ToolStripMenuItem copySelectLine;
		private System.Windows.Forms.ToolStripMenuItem exportLogMenuButton;
		private System.Windows.Forms.ToolStripMenuItem quitMenuButton;
		private System.Windows.Forms.ToolStripMenuItem autoExportTaskListMenuButton;
		private System.Windows.Forms.ToolStripMenuItem saveLocationExportMenuButton;
		private System.Windows.Forms.ToolStripMenuItem logExportOptionsMenu;
		private ListViewWithScrollEvent listView1;
		private System.Windows.Forms.Button quitButton;
	}
}

