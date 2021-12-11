using System;
using System.IO;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;


using static Tasker.TaskerVariables;
using static Tasker.LogCalls;

namespace Tasker
{
	public class TaskerSettings
	{
		public readonly struct DefaultSettings
		{
			//Default Settings for Main Window
			public const int defaultMainWindowLength = 1060;
			public const int defaultMainWindowHeight = 464;

			//Default Settings for ListView (General Settings)
			public const int defaultListViewLength = 693;
			public const int defaultListViewHeight = 380;

			//Default Settings for ListView Columns
			public const int defaultKeyColumnWidth = 60;
			public const int defaultTimeColumnWidth = 90;
			public const int defaultTimeLoggedColumnWidth = 100;
			public const int defaultDescriptionColumnWidth = 400;

			//Default Settings for Times
			public const string defaultStartTimeValue = "09:00";
			public const string defaultEndTimeValue = "18:00";
			public const string defaultIntervalTimeValue = ".5";
			public const bool defaultSaveSettingsOnCloseValue = false;
			public const bool defaultAutoExportTaskListAtEndTime = false;
			public const string defaultExportSaveLocation = "..\\";

			//Default Other Values
			public const bool firstRunValue = true;
		}

		const string mainElement_MainWindow = "MainWindow";
		const string mainElement_ListView = "ListView";
		const string mainElement_ListViewColumns = "ListViewColumns";
		const string mainElement_TimeValues = "TimeValues";
		const string mainElement_GeneralSettings = "GeneralSettings";

		const string subElement_KeyColumn = "KeyColumn";
		const string subElement_TimeColumn = "TimeColumn";
		const string subElement_TimeLoggedColumn = "TimeLoggedColumn";
		const string subElement_DescriptionColumn = "DescriptionColumn";

		const string xValue_Length = "Length";
		const string xValue_Height = "Height";
		const string xValue_Width = "Width";
		const string xValue_StartTimeValue = "StartTimeValue";
		const string xValue_EndTimeValue = "EndTimeValue";
		const string xValue_IntervalTimeValue = "IntervalTimeValue";
		const string xValue_SettingsSave = "SettingsSave";
		const string xValue_AutoExportTaskList = "AutoExportTaskList";
		const string xValue_ExportSaveLocation = "ExportSaveLocation";
		const string xValue_FirstRun = "FirstRun";

		private int curMainWindowLength;
		private int curMainWindowHeight;
		private int curListViewLength;
		private int curListViewHeight;
		private int curKeyColumnWidth;
		private int curTimeColumnWidth;
		private int curTimeLoggedColumnWidth;
		private int curDescriptionColumnWidth;
		private string curStartTimeValue;
		private string curEndTimeValue;
		private string curIntervalTimeValue;
		private bool curSaveSettingsValue;
		private bool curAutoExportTaskListValue;
		private string curExportSaveLocationValue;
		private bool curFirstRunValue;

		bool isInitialized;

		FileStream? settingsFile;

		XElement? xmlSettings;

		public TaskerSettings()
		{
			isInitialized = false;

			curMainWindowLength = 0;
			curMainWindowHeight = 0;
			curListViewLength = 0;
			curListViewHeight = 0;
			curKeyColumnWidth = 0;
			curTimeColumnWidth = 0;
			curTimeLoggedColumnWidth = 0;
			curDescriptionColumnWidth = 0;
			curStartTimeValue = String.Empty;
			curEndTimeValue = String.Empty;
			curIntervalTimeValue = String.Empty;
			curSaveSettingsValue = false;
			curAutoExportTaskListValue = false;
			curExportSaveLocationValue = String.Empty;
			curFirstRunValue = false;

			settingsFile = null;
			xmlSettings = null;
		}

		~TaskerSettings()
		{
			settingsFile!.Close();
			settingsFile!.Dispose();
		}

		public int MainWindowLength
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				return curMainWindowLength;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				curMainWindowLength = value;
			}
		}

		public int MainWindowHeight
		{

			get
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				return curMainWindowHeight;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				curMainWindowHeight = value;
			}
		}

		public int ListViewLength
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				return curListViewLength;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				curListViewLength = value;
			}
		}

		public int ListViewHeight
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				return curListViewHeight;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				curListViewHeight = value;
			}
		}

		public int KeyColumnWidth
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				return curKeyColumnWidth;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				curKeyColumnWidth = value;
			}
		}

		public int TimeColumnWidth
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				return curTimeColumnWidth;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				curTimeColumnWidth = value;
			}
		}

		public int TimeLoggedColumnWidth
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				return curTimeLoggedColumnWidth;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				curTimeLoggedColumnWidth = value;
			}
		}

		public int DescriptionColumnWidth
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				return curDescriptionColumnWidth;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				curDescriptionColumnWidth = value;
			}
		}

		public string StartTimeValue
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				return curStartTimeValue;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				curStartTimeValue = value;
			}
		}

		public string EndTimeValue
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				return curEndTimeValue;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				curEndTimeValue = value;
			}
		}

		public string IntervalTimeValue
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				return curIntervalTimeValue;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				curIntervalTimeValue = value;
			}
		}

		public bool SaveSettings
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				return curSaveSettingsValue;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker settings haven't been inititalized");

				curSaveSettingsValue = value;
			}
		}

		public bool AutoExportTaskList
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker setting haven't been initialized");

				return curAutoExportTaskListValue;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker setting haven't been initialized");

				curAutoExportTaskListValue = value;
			}
		}

		public string ExportSaveLocation
		{
			get
			{
				if (!isInitialized)
					throw new Exception("Tasker setting haven't been initialized");

				return curExportSaveLocationValue;
			}
			set
			{
				if (!isInitialized)
					throw new Exception("Tasker setting haven't been initialized");

				curExportSaveLocationValue = value;
			}
		}

		public void InitializeTaskerSettings()
		{
			string settingsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			settingsPath = Path.Combine(settingsPath, "TaskerApp");
			string settingsFilePath = Path.Combine(settingsPath, "TaskerApp.config");
			if (IsDebugModeEnabled)
				Debug("Checking if settings file path: " + settingsFilePath + " exists");

			if (!File.Exists(settingsFilePath)) {
				if (IsDebugModeEnabled)
					Debug("Standard file path does not exist. Checking if the directory path exists");

				if (!Directory.Exists(settingsPath)) {
					if (IsDebugModeEnabled)
						Debug("Standard directory path does not exist. Creating the directory.");

					Directory.CreateDirectory(settingsPath);
				}

				Debug("Now creating configuration file");
				settingsFile = new FileStream(settingsFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4, FileOptions.SequentialScan);
				xmlSettings = CreateInitialXMLSettingsTree();
				SetCurrentSettingsToDefault();
				CreateDefaultSettingsFile();
			}
			else {
				Debug("Configuration file already exists. Opening file for reading");
				settingsFile = new FileStream(settingsFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4, FileOptions.SequentialScan);
				settingsFile.Seek(0, SeekOrigin.Begin);
				xmlSettings = XElement.Load(settingsFile);
				LoadSettingsFromFile();
			}

			isInitialized = true;
		}

		public void SaveTaskerSettings()
		{
			foreach (XElement e in xmlSettings!.Elements()) {
				if (e.Name == mainElement_MainWindow) {
					foreach (XElement eValue in e.Elements()) {
						if (eValue.Name == xValue_Length)
							eValue.SetValue(curMainWindowLength.ToString());
						else if (eValue.Name == xValue_Height)
							eValue.SetValue(curMainWindowHeight.ToString());
					}
				}
				else if (e.Name == mainElement_ListView) {
					foreach (XElement eValue in e.Elements()) {
						if (eValue.Name == xValue_Length)
							eValue.SetValue(curListViewLength.ToString());
						else if (eValue.Name == xValue_Height)
							eValue.SetValue(curListViewHeight.ToString());
					}
				}
				else if (e.Name == mainElement_ListViewColumns) {
					foreach (XElement subE in e.Elements()) {
						if (subE.Name == subElement_KeyColumn) {
							foreach (XElement eValue in subE.Elements()) {
								if (eValue.Name == xValue_Width)
									eValue.SetValue(curKeyColumnWidth.ToString());
								else if (eValue.Name == xValue_Height)
									eValue.SetValue(curListViewHeight.ToString());
							}
						}
						else if (subE.Name == subElement_TimeColumn) {
							foreach (XElement eValue in subE.Elements()) {
								if (eValue.Name == xValue_Width)
									eValue.SetValue(curTimeColumnWidth.ToString());
								else if (eValue.Name == xValue_Height)
									eValue.SetValue(curListViewHeight.ToString());
							}
						}
						else if (subE.Name == subElement_TimeLoggedColumn) {
							foreach (XElement eValue in subE.Elements()) {
								if (eValue.Name == xValue_Width)
									eValue.SetValue(curTimeLoggedColumnWidth.ToString());
								else if (eValue.Name == xValue_Height)
									eValue.SetValue(curListViewHeight.ToString());
							}
						}
						else if (subE.Name == subElement_DescriptionColumn) {
							foreach (XElement eValue in subE.Elements()) {
								if (eValue.Name == xValue_Width)
									eValue.SetValue(curDescriptionColumnWidth.ToString());
								else if (eValue.Name == xValue_Height)
									eValue.SetValue(curListViewHeight.ToString());
							}
						}
					}
				}
				else if (e.Name == mainElement_TimeValues) {
					foreach (XElement eValue in e.Elements()) {
						if (eValue.Name == xValue_StartTimeValue)
							eValue.SetValue(curStartTimeValue);
						else if (eValue.Name == xValue_EndTimeValue)
							eValue.SetValue(curEndTimeValue);
						else if (eValue.Name == xValue_IntervalTimeValue)
							eValue.SetValue(curIntervalTimeValue);
					}
				}
				else if (e.Name == mainElement_GeneralSettings) {
					foreach (XElement eValue in e.Elements()) {
						if (eValue.Name == xValue_SettingsSave)
							eValue.SetValue(curSaveSettingsValue);
						else if (eValue.Name == xValue_AutoExportTaskList)
							eValue.SetValue(curAutoExportTaskListValue);
						else if (eValue.Name == xValue_ExportSaveLocation)
							eValue.SetValue(curExportSaveLocationValue);
						else if (eValue.Name == xValue_FirstRun)
							eValue.SetValue(curFirstRunValue);
					}
				}
			}

			byte[] settingsFileBytes = Encoding.UTF8.GetBytes(xmlSettings.ToString());
			settingsFile!.Seek(0, SeekOrigin.Begin);
			settingsFile!.Write(settingsFileBytes, 0, settingsFileBytes.Length);
		}

		public void ResetTaskerSettings()
		{
			SetCurrentSettingsToDefault();
		}

		private XElement CreateInitialXMLSettingsTree()
		{
			return new XElement("TaskerSettings",
												new XElement("MainWindow",
																new XElement("Length", curMainWindowLength.ToString()),
																new XElement("Height", curMainWindowHeight.ToString())),
												new XElement("ListView",
																new XElement("Length", curListViewLength.ToString()),
																new XElement("Height", curListViewHeight.ToString())),
												new XElement("ListViewColumns",
																new XElement("KeyColumn",
																				new XElement("Width", curKeyColumnWidth.ToString()),
																				new XElement("Height", curListViewHeight.ToString())),
																new XElement("TimeColumn",
																				new XElement("Width", curTimeColumnWidth.ToString()),
																				new XElement("Height", curListViewHeight.ToString())),
																new XElement("TimeLoggedColumn",
																				new XElement("Width", curTimeLoggedColumnWidth.ToString()),
																				new XElement("Height", curListViewHeight.ToString())),
																new XElement("DescriptionColumn",
																				new XElement("Width", curDescriptionColumnWidth.ToString()),
																				new XElement("Height", curListViewHeight.ToString()))),
												new XElement("TimeValues",
																new XElement("StartTimeValue", curStartTimeValue),
																new XElement("EndTimeValue", curEndTimeValue),
																new XElement("IntervalTimeValue", curIntervalTimeValue)),
												new XElement("GeneralSettings",
																new XElement("SettingsSave", curSaveSettingsValue.ToString()),
																new XElement("AutoExportTaskList", curAutoExportTaskListValue.ToString()),
																new XElement("ExportSaveLocation", curExportSaveLocationValue.ToString()),
																new XElement("FirstRun", curFirstRunValue.ToString())));
		}

		private void SetCurrentSettingsToDefault()
		{
			curMainWindowLength = DefaultSettings.defaultMainWindowLength;
			curMainWindowHeight = DefaultSettings.defaultMainWindowHeight;
			curListViewLength = DefaultSettings.defaultListViewLength;
			curListViewHeight = DefaultSettings.defaultListViewHeight;
			curKeyColumnWidth = DefaultSettings.defaultKeyColumnWidth;
			curTimeColumnWidth = DefaultSettings.defaultTimeColumnWidth;
			curTimeLoggedColumnWidth = DefaultSettings.defaultTimeLoggedColumnWidth;
			curDescriptionColumnWidth = DefaultSettings.defaultDescriptionColumnWidth;
			curStartTimeValue = DefaultSettings.defaultStartTimeValue;
			curEndTimeValue = DefaultSettings.defaultEndTimeValue;
			curIntervalTimeValue = DefaultSettings.defaultIntervalTimeValue;
			curSaveSettingsValue = DefaultSettings.defaultSaveSettingsOnCloseValue;
			curAutoExportTaskListValue = DefaultSettings.defaultAutoExportTaskListAtEndTime;
			curExportSaveLocationValue = DefaultSettings.defaultExportSaveLocation;
			curFirstRunValue = DefaultSettings.firstRunValue;
		}

		private void LoadSettingsFromFile()
		{
			foreach (XElement e in xmlSettings!.Elements()) {
				if(e.Name == mainElement_MainWindow) {
					foreach(XElement eValue in e.Elements()) {
						if (eValue.Name == xValue_Length)
							curMainWindowLength = Int32.Parse(eValue.Value);
						else if (eValue.Name == xValue_Height)
							curMainWindowHeight = Int32.Parse(eValue.Value);
					}
				}
				else if(e.Name == mainElement_ListView) {
					foreach(XElement eValue in e.Elements()) {
						if (eValue.Name == xValue_Length)
							curListViewLength = Int32.Parse(eValue.Value);
						else if (eValue.Name == xValue_Height)
							curListViewHeight = Int32.Parse(eValue.Value);
					}
				}
				else if(e.Name == mainElement_ListViewColumns) {
					foreach(XElement subE in e.Elements()) {
						if(subE.Name == subElement_KeyColumn) {
							foreach(XElement eValue in subE.Elements()) {
								if (eValue.Name == xValue_Width)
									curKeyColumnWidth = Int32.Parse(eValue.Value);
							}
						}
						else if(subE.Name == subElement_TimeColumn) {
							foreach (XElement eValue in subE.Elements()) {
								if (eValue.Name == xValue_Width)
									curTimeColumnWidth = Int32.Parse(eValue.Value);
							}
						}
						else if(subE.Name == subElement_TimeLoggedColumn) {
							foreach (XElement eValue in subE.Elements()) {
								if (eValue.Name == xValue_Width)
									curTimeLoggedColumnWidth = Int32.Parse(eValue.Value);
							}
						}
						else if(subE.Name == subElement_DescriptionColumn) {
							foreach (XElement eValue in subE.Elements()) {
								if (eValue.Name == xValue_Width)
									curDescriptionColumnWidth = Int32.Parse(eValue.Value);
							}
						}
					}
				}
				else if(e.Name == mainElement_TimeValues) {
					foreach (XElement eValue in e.Elements()) {
						if (eValue.Name == xValue_StartTimeValue)
							curStartTimeValue = eValue.Value;
						else if (eValue.Name == xValue_EndTimeValue)
							curEndTimeValue = eValue.Value;
						else if (eValue.Name == xValue_IntervalTimeValue)
							curIntervalTimeValue = eValue.Value;
					}
				}
				else if(e.Name == mainElement_GeneralSettings) {
					foreach (XElement eValue in e.Elements()) {
						if (eValue.Name == xValue_SettingsSave)
							curSaveSettingsValue = bool.Parse(eValue.Value);
						else if (eValue.Name == xValue_AutoExportTaskList)
							curAutoExportTaskListValue = bool.Parse(eValue.Value);
						else if (eValue.Name == xValue_ExportSaveLocation)
							curExportSaveLocationValue = eValue.Value;
						else if (eValue.Name == xValue_FirstRun)
							curFirstRunValue = bool.Parse(eValue.Value);
					}
				}
			}
		}

		private void CreateDefaultSettingsFile()
		{
			foreach (XElement e in xmlSettings!.Elements()) {
				if (e.Name == mainElement_MainWindow) {
					foreach (XElement eValue in e.Elements()) {
						if (eValue.Name == xValue_Length)
							eValue.SetValue(DefaultSettings.defaultMainWindowLength.ToString());
						else if (eValue.Name == xValue_Height)
							eValue.SetValue(DefaultSettings.defaultMainWindowHeight.ToString());
					}
				}
				else if (e.Name == mainElement_ListView) {
					foreach (XElement eValue in e.Elements()) {
						if (eValue.Name == xValue_Length)
							eValue.SetValue(DefaultSettings.defaultListViewLength.ToString());
						else if (eValue.Name == xValue_Height)
							eValue.SetValue(DefaultSettings.defaultListViewHeight.ToString());
					}
				}
				else if (e.Name == mainElement_ListViewColumns) {
					foreach (XElement subE in e.Elements()) {
						if (subE.Name == subElement_KeyColumn) {
							foreach (XElement eValue in subE.Elements()) {
								if (eValue.Name == xValue_Width)
									eValue.SetValue(DefaultSettings.defaultKeyColumnWidth.ToString());
								else if (eValue.Name == xValue_Height)
									eValue.SetValue(DefaultSettings.defaultListViewHeight.ToString());
							}
						}
						else if (subE.Name == subElement_TimeColumn) {
							foreach (XElement eValue in subE.Elements()) {
								if (eValue.Name == xValue_Width)
									eValue.SetValue(DefaultSettings.defaultTimeColumnWidth.ToString());
								else if (eValue.Name == xValue_Height)
									eValue.SetValue(DefaultSettings.defaultListViewHeight.ToString());
							}
						}
						else if (subE.Name == subElement_TimeLoggedColumn) {
							foreach (XElement eValue in subE.Elements()) {
								if (eValue.Name == xValue_Width)
									eValue.SetValue(DefaultSettings.defaultTimeLoggedColumnWidth.ToString());
								else if (eValue.Name == xValue_Height)
									eValue.SetValue(DefaultSettings.defaultListViewHeight.ToString());
							}
						}
						else if (subE.Name == subElement_DescriptionColumn) {
							foreach (XElement eValue in subE.Elements()) {
								if (eValue.Name == xValue_Width)
									eValue.SetValue(DefaultSettings.defaultDescriptionColumnWidth.ToString());
								else if (eValue.Name == xValue_Height)
									eValue.SetValue(DefaultSettings.defaultListViewHeight.ToString());
							}
						}
					}
				}
				else if (e.Name == mainElement_TimeValues) {
					foreach (XElement eValue in e.Elements()) {
						if (eValue.Name == xValue_StartTimeValue)
							eValue.SetValue(DefaultSettings.defaultStartTimeValue);
						else if (eValue.Name == xValue_EndTimeValue)
							eValue.SetValue(DefaultSettings.defaultEndTimeValue);
						else if (eValue.Name == xValue_IntervalTimeValue)
							eValue.SetValue(DefaultSettings.defaultIntervalTimeValue);
					}
				}
				else if (e.Name == mainElement_GeneralSettings) {
					foreach (XElement eValue in e.Elements()) {
						if (eValue.Name == xValue_SettingsSave)
							eValue.SetValue(DefaultSettings.defaultSaveSettingsOnCloseValue);
						else if (eValue.Name == xValue_AutoExportTaskList)
							eValue.SetValue(DefaultSettings.defaultAutoExportTaskListAtEndTime);
						else if (eValue.Name == xValue_ExportSaveLocation)
							eValue.SetValue(DefaultSettings.defaultExportSaveLocation);
						else if (eValue.Name == xValue_FirstRun)
							eValue.SetValue(DefaultSettings.firstRunValue);
					}
				}
			}
		}
	}
}