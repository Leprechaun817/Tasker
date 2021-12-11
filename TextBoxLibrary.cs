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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;


using static Tasker.TaskerVariables;
using static Tasker.LogCalls;
using static Tasker.TextBoxTypes;

namespace Tasker
{
	public class TextBoxTypes : Enumeration
	{
		public static readonly TextBoxTypes StartText = new TextBoxTypes(0, "Start");
		public static readonly TextBoxTypes EndText = new TextBoxTypes(1, "End");
		public static readonly TextBoxTypes RemindText = new TextBoxTypes(2, "Remind");
		public static readonly TextBoxTypes KeyText = new TextBoxTypes(3, "Key");
		public static readonly TextBoxTypes TimeSpentText = new TextBoxTypes(4, "TimeSpent");
		public static readonly TextBoxTypes DescText = new TextBoxTypes(5, "Description");


		private TextBoxTypes() { }
		private TextBoxTypes(int lV, string lDN) : base(lV, lDN) { }
	}

	//First part of partial class contains functions to be used with the main window
	public static partial class TextBoxLibrary
	{
		public static void AlertUserOfMainWindowErrors(int textBoxState, Dictionary<TextBoxTypes, string> errorMsgList, Dictionary<TextBoxTypes, string> errorTypeMsgList)
		{
			if (textBoxState != 222) {
				if(IsDebugModeEnabled)
					Error("Text box errors were detected. Alerting user");
			}

			switch (textBoxState) {
				case 111:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Start, End, Interval");
						Error("Start Textbox Error -> " + errorMsgList[StartText] + errorTypeMsgList[StartText]);
						Error("End Textbox Error -> " + errorMsgList[EndText] + errorTypeMsgList[EndText]);
						Error("Reminder Interval Textbox Error -> " + errorMsgList[RemindText] + errorTypeMsgList[RemindText]);
					}

					MessageBox.Show("Start Textbox Error: " + errorMsgList[StartText] + errorTypeMsgList[StartText] + "\n" +
									"End Textbox Error: " + errorMsgList[EndText] + errorTypeMsgList[EndText] + "\n" +
									"Interval Textbox Error: " + errorMsgList[RemindText] + errorTypeMsgList[RemindText] + "\n" +
									"Please correct all errors and then press Start Tracking", "Tasker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 112:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Start, End");
						Error("Start Textbox Error -> " + errorMsgList[StartText] + errorTypeMsgList[StartText]);
						Error("End Textbox Error -> " + errorMsgList[EndText] + errorTypeMsgList[EndText]);
						Info("Interval Textbox -> No error found, entry is valid");
					}

					MessageBox.Show("Start Textbox Error: " + errorMsgList[StartText] + errorTypeMsgList[StartText] + "\n" +
									"End Textbox Error: " + errorMsgList[EndText] + errorTypeMsgList[EndText] + "\n" +
									"Please correct all errors and then press Start Tracking", "Tasker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 122:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Start");
						Error("Start Textbox Error -> " + errorMsgList[StartText] + errorTypeMsgList[StartText]);
						Info("End Textbox -> No error found, entry is valid");
						Info("Reminder Interval Textbox -> No error found, entry is valid");
					}

					MessageBox.Show("Start Textbox Error: " + errorMsgList[StartText] + errorTypeMsgList[StartText] + "\n" +
									"Please correct all errors and then press Start Tracking", "Tasker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 121:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Start, Interval");
						Error("Start Textbox Error -> " + errorMsgList[StartText] + errorTypeMsgList[StartText]);
						Info("End Textbox -> No error found, entry is valid");
						Error("Reminder Interval Textbox Error -> " + errorMsgList[RemindText] + errorTypeMsgList[RemindText]);
					}

					MessageBox.Show("Start Textbox Error: " + errorMsgList[StartText] + errorTypeMsgList[StartText] + "\n" +
									"Interval Textbox Error: " + errorMsgList[RemindText] + errorTypeMsgList[RemindText] + "\n" +
									"Please correct all errors and then press Start Tracking", "Tasker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 211:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - End, Interval");
						Info("Start Textbox -> No error found, entry is valid");
						Error("End Textbox Error -> " + errorMsgList[EndText] + errorTypeMsgList[EndText]);
						Error("Reminder Interval Textbox Error -> " + errorMsgList[RemindText] + errorTypeMsgList[RemindText]);
					}

					MessageBox.Show("End Textbox Error: " + errorMsgList[EndText] + errorTypeMsgList[EndText] + "\n" +
									"Interval Textbox Error: " + errorMsgList[RemindText] + errorTypeMsgList[RemindText] + "\n" +
									"Please correct all errors and then press Start Tracking", "Tasker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 221:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Interval");
						Info("Start Textbox -> No error found, entry is valid");
						Info("End Textbox -> No error found, entry is valid");
						Error("Reminder Interval Textbox Error -> " + errorMsgList[RemindText] + errorTypeMsgList[RemindText]);
					}
					MessageBox.Show("Interval Textbox Error: " + errorMsgList[RemindText] + errorTypeMsgList[RemindText] + "\n" +
									"Please correct all errors and then press Start Tracking", "Tasker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 212:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - End");
						Info("Start Textbox -> No error found, entry is valid");
						Error("End Textbox Error -> " + errorMsgList[EndText] + errorTypeMsgList[EndText]);
						Info("Reminder Interval Textbox -> No error found, entry is valid");
					}

					MessageBox.Show("End Textbox Error: " + errorMsgList[EndText] + errorTypeMsgList[EndText] + "\n" +
									"Please correct all errors and then press Start Tracking", "Tasker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				default:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Start, End, Interval");
						Error("Start Textbox -> No error found, entry is valid");
						Error("End Textbox -> No error found, entry is valid");
						Error("Reminder Interval Textbox -> No error found, entry is valid");
					}
					break;
			}
		}

		public static void ResetMainWindowTextBoxErrorStates(TaskerMainWindow mainWinObj, Tuple<int, Dictionary<TextBoxTypes, string>, Dictionary<TextBoxTypes, string>> errorState)
		{
			ErrorProvider step = mainWinObj.GetStartTimeErrorProviderRef();
			ErrorProvider etep = mainWinObj.GetEndTimeErrorProviderRef();
			ErrorProvider rtep = mainWinObj.GetRemindIntervErrorProviderRef();

			TextBox sttbr = mainWinObj.GetStartTimeTextBoxRef();
			TextBox ettbr = mainWinObj.GetEndTimeTextBoxRef();
			TextBox rttbr = mainWinObj.GetRemindIntervTextBoxRef();

			int textBoxState = errorState.Item1;
			Dictionary<TextBoxTypes, string> errorMsgList = errorState.Item2;
			Dictionary<TextBoxTypes, string> errorTypeMsgList = errorState.Item3;

			switch(textBoxState) {
				case 111:
					step.ResetError(sttbr, errorMsgList[StartText]);
					etep.ResetError(ettbr, errorMsgList[EndText]);
					rtep.ResetError(rttbr, errorMsgList[RemindText]);

					if (IsDebugModeEnabled) {
						Error("Start Time Error - " + errorMsgList[StartText] + errorTypeMsgList[StartText]);
						Error("End Time Error - " + errorMsgList[EndText] + errorTypeMsgList[EndText]);
						Error("Reminder Interval Error - " + errorMsgList[RemindText] + errorTypeMsgList[RemindText]);
					}
					break;
				case 112:
					step.ResetError(sttbr, errorMsgList[StartText]);
					etep.ResetError(ettbr, errorMsgList[EndText]);
					rtep.ResetError(rttbr, String.Empty);

					if (IsDebugModeEnabled) {
						Error("Start Time Error - " + errorMsgList[StartText] + errorTypeMsgList[StartText]);
						Error("End Time Error - " + errorMsgList[EndText] + errorTypeMsgList[EndText]);
						Info("Reminder Interval Error Canceled - Valid value has been provided");
					}
					break;
				case 122:
					step.ResetError(sttbr, errorMsgList[StartText]);
					etep.ResetError(ettbr, String.Empty);
					rtep.ResetError(rttbr, String.Empty);

					if (IsDebugModeEnabled) {
						Error("Start Time Error - " + errorMsgList[StartText] + errorTypeMsgList[StartText]);
						Info("End Time Error Cancelled - Valid value has been provided");
						Info("Reminder Interval Error Canceled - Valid value has been provided");
					}
					break;
				case 121:
					step.ResetError(sttbr, errorMsgList[StartText]);
					etep.ResetError(ettbr, String.Empty);
					rtep.ResetError(rttbr, errorMsgList[RemindText]);

					if (IsDebugModeEnabled) {
						Error("Start Time Error - " + errorMsgList[StartText] + errorTypeMsgList[StartText]);
						Error("End Time Error Canceled - Valid value has been provided");
						Error("Reminder Interval Error - " + errorMsgList[RemindText] + errorTypeMsgList[RemindText]);
					}
					break;
				case 211:
					step.ResetError(sttbr, String.Empty);
					etep.ResetError(ettbr, errorMsgList[EndText]);
					rtep.ResetError(rttbr, errorMsgList[RemindText]);

					if (IsDebugModeEnabled) {
						Error("Start Time Error Canceled - Valid value has been provided");
						Error("End Time Error - " + errorMsgList[EndText] + errorTypeMsgList[EndText]);
						Error("Reminder Interval Error - " + errorMsgList[RemindText] + errorTypeMsgList[RemindText]);
					}
					break;
				case 221:
					step.ResetError(sttbr, String.Empty);
					etep.ResetError(ettbr, String.Empty);
					rtep.ResetError(rttbr, errorMsgList[RemindText]);

					if (IsDebugModeEnabled) {
						Error("Start Time Error Canceled - Valid value has been provided");
						Error("End Time Error Canceled - Valid value has been provided");
						Error("Reminder Interval Error - " + errorMsgList[RemindText] + errorTypeMsgList[RemindText]);
					}
					break;
				case 212:
					step.ResetError(sttbr, String.Empty);
					etep.ResetError(ettbr, errorMsgList[EndText]);
					rtep.ResetError(rttbr, String.Empty);

					if (IsDebugModeEnabled) {
						Error("Start Time Error Canceled - Valid value has been provided");
						Error("End Time Error - " + errorMsgList[EndText] + errorTypeMsgList[EndText]);
						Error("Reminder Interval Error Canceled - Valid value has been provided");
					}
					break;
				default:
					step.ResetError(sttbr, String.Empty);
					etep.ResetError(ettbr, String.Empty);
					rtep.ResetError(rttbr, String.Empty);

					if (IsDebugModeEnabled) {
						Error("Start Time Error Canceled - Valid value has been provided");
						Error("End Time Error Canceled - Valid value has been provided");
						Error("Reminder Interval Error Canceled - Valid value has been provided");
					}
					break;
			}
		}

		public static (int, Dictionary<TextBoxTypes, string>, Dictionary<TextBoxTypes, string>) GetMainWindowTextBoxValidationStatus(Dictionary<TextBoxTypes, string> textEntryList)
		{
			if(IsDebugModeEnabled) 
				Debug("Getting statuses of all 3 text boxes");

			(bool startTimeInvalid, string startTimeErrorMsg, string startTimeTypeErrorMsg) = ValidateStartTimeTextEntry(textEntryList[StartText]);
			(bool endTimeInvalid, string endTimeErrorMsg, string endTimeTypeErrorMsg) = ValidateEndTimeTextEntry(textEntryList[EndText]);
			(bool remindIntervalTimeInvalid, string remindIntervalTimeErrorMsg, string remindIntervalTimeTypeErrorMsg) = ValidateReminderIntervTextEntry(textEntryList[RemindText]);

			Dictionary<TextBoxTypes, string> errorMsgList = new Dictionary<TextBoxTypes, string>();
			Dictionary<TextBoxTypes, string> errorTypeMsgList = new Dictionary<TextBoxTypes, string>();

			int returnValue = 0;
			if(startTimeInvalid) {
				returnValue += 100;
				errorMsgList.Add(StartText, startTimeErrorMsg);
				errorTypeMsgList.Add(StartText, startTimeTypeErrorMsg);
			}
			else {
				returnValue += 200;
				errorMsgList.Add(StartText, String.Empty);
				errorTypeMsgList.Add(StartText, String.Empty);
			}

			if(IsDebugModeEnabled)
				Warn("Start Time Text Box Status Code: " + returnValue.ToString());

			if(endTimeInvalid) {
				returnValue += 10;
				errorMsgList.Add(EndText, endTimeErrorMsg);
				errorTypeMsgList.Add(EndText, endTimeTypeErrorMsg);
			}
			else {
				returnValue += 20;
				errorMsgList.Add(EndText, String.Empty);
				errorTypeMsgList.Add(EndText, String.Empty);
			}

			if(IsDebugModeEnabled)
				Warn("End Time Text Box Status Code: " + returnValue.ToString());

			if(remindIntervalTimeInvalid) {
				returnValue += 1;
				errorMsgList.Add(RemindText, remindIntervalTimeErrorMsg);
				errorTypeMsgList.Add(RemindText, remindIntervalTimeTypeErrorMsg);
			}
			else {
				returnValue += 2;
				errorMsgList.Add(RemindText, String.Empty);
				errorTypeMsgList.Add(RemindText, String.Empty);
			}

			if(IsDebugModeEnabled)
				Warn("Reminder Interval Text Box Status Code: " + returnValue.ToString());

			return (returnValue, errorMsgList, errorTypeMsgList);
		}

		public static (bool, string, string) ValidateStartTimeTextEntry(string entryText)
		{
			bool isErrored = false;
			string errorMsg = String.Empty;
			string errorTypeMsg = String.Empty;

			//Check to make sure start time is in a valid format and is not null or empty
			if(String.IsNullOrWhiteSpace(entryText)) {
				isErrored = true;
				if (IsDebugModeEnabled) {
					errorMsg = "The start time text is null or empty";
					errorTypeMsg = "-- Null or Empty Text Box Error";
				}
			}
			else if(!Regex.Match(entryText, @"^[0-2][0-9]:[0-5][0-9]").Success) {
				isErrored = true;
				if (IsDebugModeEnabled) {
					errorMsg = "Start time must be in correct format: HH:MM";
					errorTypeMsg = "-- Formatting Error";
				}
			}
			else {
				int hours = Int32.Parse(entryText.AsSpan(0, 2));
				if(hours > 23) {
					isErrored = true;
					if (IsDebugModeEnabled) {
						errorMsg = "Start time cannot have a hours value greater than 23";
						errorTypeMsg = "-- Time Error";
					}
				}
			}

			return (isErrored, errorMsg, errorTypeMsg);
		}

		public static (bool, string, string) ValidateEndTimeTextEntry(string entryText)
		{
			bool isErrored = false;
			string errorMsg = String.Empty;
			string errorTypeMsg = String.Empty;

			//Check to make sure time is in a valid format
			if(String.IsNullOrWhiteSpace(entryText)) {
				isErrored = true;
				if (IsDebugModeEnabled) {
					errorMsg = "The end time text box is null or empty";
					errorTypeMsg = "-- Null Or Empty Text Box Error";
				}
			}
			else if(!Regex.Match(entryText, @"^[0-2][0-9]:[0-9][0-9]").Success) {
				isErrored = true;
				if (IsDebugModeEnabled) {
					errorMsg = "The end time must be in the correct format: HH:MM";
					errorTypeMsg = "-- Formatting Error";
				}
			}
			else {
				//If the end time is in a valid format, ensure that time is actually valid
				int hours = Int32.Parse(entryText.AsSpan(0, 2));
				if(hours > 23) {
					isErrored = true;
					if (IsDebugModeEnabled) {
						errorMsg = "End time cannot have a hours value greater than 23";
						errorTypeMsg = "-- Time Error";
					}
				}
			}

			return (isErrored, errorMsg, errorTypeMsg);
		}

		public static (bool, string, string) ValidateReminderIntervTextEntry(string entryText)
		{
			bool isErrored;
			string errorMsg = String.Empty;
			string errorTypeMsg = String.Empty;

			if (String.IsNullOrWhiteSpace(entryText)) {
				isErrored = true;
				if (IsDebugModeEnabled) {
					errorMsg = "The reminder interval text box is null or empty";
					errorTypeMsg = "-- Null or Empty Text Box Error";
				}
			}
			else if (!Regex.Match(entryText, @"^[1-4]").Success &&
					!Regex.Match(entryText, @"^[1-3]\.5").Success &&
					!entryText.ContainsAny("10", "15", "30", "60") &&
					!entryText.ContainsAny(".17", ".25", ".5")) {
				//Formatting error detected
				isErrored = true;

				int textBoxCharCount = 0;
				bool isNotChar = Int32.TryParse(entryText, out int timeEnteredInt);

				if (isNotChar) {
					if (textBoxCharCount == 1) {
						//User was entering hour value
						if (timeEnteredInt == 0) {
							if (IsDebugModeEnabled) {
								errorMsg = "You cannot enter 0 minutes as the interval";
								errorTypeMsg = "-- Zero Minute Error";
							}
						}
						else if (timeEnteredInt > 4) {
							if (IsDebugModeEnabled) {
								errorMsg = "You cannot have an interval greater than 4 hours";
								errorTypeMsg = "-- Hour Value Greater Than 4 Error";
							}
						}
					}
					else if (textBoxCharCount == 2) {
						if (entryText.Contains('.')) {
							if (IsDebugModeEnabled) {
								errorMsg = "When entering a decimal for time, it must be .17, .25, or .5";
								errorTypeMsg = "-- Decimal Time Minute Error";
							}
						}
						else {
							if (IsDebugModeEnabled) {
								errorMsg = "When using minute values for interval, interval must be 10, 15, 30, or 60 minutes";
								errorTypeMsg = "-- Bad Minute Value Error";
							}
						}
					}
					else if (textBoxCharCount == 3) {
						if (entryText.Contains('.')) {
							if (entryText.StartsWith('.')) {
								if (IsDebugModeEnabled) {
									errorMsg = "When entering in a decimal for time, it must be .17, .25, or .5";
									errorTypeMsg = "-- Decimal Time Minute Error";
								}
							}
							else {
								if (IsDebugModeEnabled) {
									errorMsg = "When entering in a hours/minutes block time, it must be 1.5, 2.5, or 3.5";
									errorTypeMsg = "-- Decimal Time Hour Error";
								}
							}
						}
						else {
							if (IsDebugModeEnabled) {
								errorMsg = "If entering in time greater than 1 hour, please use single digit hours\nOtherwise, you cannot have a time greater than 60 minutes";
								errorTypeMsg = "-- Invalid Time Or Format Error";
							}
						}
					}
					else {
						if (textBoxCharCount == 0) {
							if (IsDebugModeEnabled) {
								errorMsg = "You must enter a value for the reminder interval";
								errorTypeMsg = "-- Null Value Error";
							}
						}
						else {
							if (IsDebugModeEnabled) {
								errorMsg = "You must enter a valid value for the reminder interval";
								errorTypeMsg = "-- Character Length Error";
							}
						}
					}
				}
				else {
					if (IsDebugModeEnabled) {
						errorMsg = "You can only enter a number or decimal as the interval";
						errorTypeMsg = "-- Alphabetic Character Error";
					}
				}
			}
			else
				isErrored = false;

			return (isErrored, errorMsg, errorTypeMsg);
		}
	}

	public static partial class TextBoxLibrary
	{
		public static void AlertUserOfTimeEntryWindowErrors(int textBoxState, Dictionary<TextBoxTypes, string> errorMsgList, Dictionary<TextBoxTypes, string> errorTypeMsgList)
		{
			if (textBoxState != 222) {
				if(IsDebugModeEnabled)
					Error("Textbox errors were detected. Alerting user");
			}

			switch(textBoxState) {
				case 111:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Key, Time Spent, Description");
						Error("Key Textbox Error -> " + errorMsgList[KeyText] + errorTypeMsgList[KeyText]);
						Error("Time Spent Textbox Error -> " + errorMsgList[TimeSpentText] + errorTypeMsgList[TimeSpentText]);
						Error("Description Textbox Error -> " + errorMsgList[DescText] + errorTypeMsgList[DescText]);
					}

					MessageBox.Show("Key Textbox Error: " + errorMsgList[KeyText] + errorTypeMsgList[KeyText] + "\n" +
									"Time Spent Textbox Error: " + errorMsgList[TimeSpentText] + errorTypeMsgList[TimeSpentText] + "\n" +
									"Description Textbox Error: " + errorMsgList[DescText] + errorTypeMsgList[DescText] + "\n" +
									"Please correct all errors and then press OK.", "Time Entry Window Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 112:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Key, Time Spent");
						Error("Key Textbox Error -> " + errorMsgList[KeyText] + errorTypeMsgList[KeyText]);
						Error("Time Spent Textbox Error -> " + errorMsgList[TimeSpentText] + errorTypeMsgList[TimeSpentText]);
						Info("Description Textbox -> No error found, entry is valid");
					}

					MessageBox.Show("Key Textbox Error: " + errorMsgList[KeyText] + errorTypeMsgList[KeyText] + "\n" +
									"Time Spent Textbox Error: " + errorMsgList[TimeSpentText] + errorTypeMsgList[TimeSpentText] + "\n" +
									"Please correct all errors and then press OK.", "Time Entry Window Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 122:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Key");
						Error("Key Textbox Error -> " + errorMsgList[KeyText] + errorTypeMsgList[KeyText]);
						Info("Time Spent Textbox -> No error found, entry is valid");
						Info("Description Textbox -> No error found, entry is valid");
					}

					MessageBox.Show("Key Textbox Error: " + errorMsgList[KeyText] + errorTypeMsgList[KeyText] + "\n" +
									"Please correct all errors and then press OK.", "Time Entry Window Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 121:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Key, Description");
						Error("Key Textbox Error -> " + errorMsgList[KeyText] + errorTypeMsgList[KeyText]);
						Info("Time Spent Textbox -> No error found, entry is valid");
						Error("Description Textbox Error -> " + errorMsgList[DescText] + errorTypeMsgList[DescText]);
					}

					MessageBox.Show("Key Textbox Error: " + errorMsgList[KeyText] + errorTypeMsgList[KeyText] + "\n" +
									"Description Textbox Error: " + errorMsgList[DescText] + errorTypeMsgList[DescText] + "\n" +
									"Please correct all errors and then press OK.", "Time Entry Window Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 211:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Time Spent, Description");
						Info("Key Textbox -> No error found, entry is valid");
						Error("Time Spent Textbox Error -> " + errorMsgList[TimeSpentText] + errorTypeMsgList[TimeSpentText]);
						Error("Description Textbox Error -> " + errorMsgList[DescText] + errorTypeMsgList[DescText]);
					}

					MessageBox.Show("Time Spent Textbox Error: " + errorMsgList[TimeSpentText] + errorTypeMsgList[TimeSpentText] + "\n" +
									"Description Textbox Error: " + errorMsgList[DescText] + errorTypeMsgList[DescText] + "\n" +
									"Please correct all errors and then press OK.", "Time Entry Window Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 221:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Description");
						Info("Key Textbox -> No error found, entry is valid");
						Info("Time Spent Textbox -> No error found, entry is valid");
						Error("Description Textbox Error -> " + errorMsgList[DescText] + errorTypeMsgList[DescText]);
					}

					MessageBox.Show("Description Textbox Error: " + errorMsgList[DescText] + errorTypeMsgList[DescText] + "\n" +
									"Please correct all errors and then press OK.", "Time Entry Window Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 212:
					if (IsDebugModeEnabled) {
						Warn("Alerting user - Time Spent");
						Info("Key Textbox -> No error found, entry is valid");
						Error("Time Spent Textbox Error -> " + errorMsgList[TimeSpentText] + errorTypeMsgList[TimeSpentText]);
						Info("Description Textbox -> No error found, entry is valid");
					}

					MessageBox.Show("Time Spent Textbox Error: " + errorMsgList[TimeSpentText] + errorTypeMsgList[TimeSpentText] + "\n" +
									"Please correct all errors and then press OK.", "Time Entry Window Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				case 222:
					if (IsDebugModeEnabled) {
						Info("Key Textbox -> No error found, entry is valid");
						Info("Time Spent Textbox -> No error found, entry is valid");
						Info("Description Textbox -> No error found, entry is valid");
					}

					break;
			}
		}

		public static void ResetTimeEntryTextBoxErrorStates(TaskEntryWindow taskEntryWindowObj, int textBoxState, Dictionary<TextBoxTypes, string> errorMsgList, Dictionary<TextBoxTypes, string> errorTypeMsgList)
		{
			ErrorProvider ktep = taskEntryWindowObj.GetKeyEntryErrorProviderRef();
			ErrorProvider ttep = taskEntryWindowObj.GetTimeSpentErrorProviderRef();
			ErrorProvider dtep = taskEntryWindowObj.GetDescriptionErrorProviderRef();

			TextBox kttbr = taskEntryWindowObj.GetKeyEntryTextBoxRef();
			TextBox tttbr = taskEntryWindowObj.GetTimeSpentTextBoxRef();
			TextBox dttbr = taskEntryWindowObj.GetDescriptionTextBoxRef();

			switch(textBoxState) 
			{
				case 111:
					ktep.ResetError(kttbr, errorMsgList[KeyText]);
					ttep.ResetError(tttbr, errorMsgList[TimeSpentText]);
					dtep.ResetError(dttbr, errorMsgList[DescText]);

					if (IsDebugModeEnabled) {
						Error("Key Error - " + errorMsgList[KeyText] + errorTypeMsgList[KeyText]);
						Error("Time Spent Error - " + errorMsgList[TimeSpentText] + errorTypeMsgList[TimeSpentText]);
						Error("Description Error - " + errorMsgList[DescText] + errorTypeMsgList[DescText]);
					}
					break;
				case 112:
					ktep.ResetError(kttbr, errorMsgList[KeyText]);
					ttep.ResetError(tttbr, errorMsgList[TimeSpentText]);
					dtep.ResetError(dttbr, String.Empty);

					if (IsDebugModeEnabled) {
						Error("Key Error - " + errorMsgList[KeyText] + errorTypeMsgList[KeyText]);
						Error("Time Spent Error - " + errorMsgList[TimeSpentText] + errorTypeMsgList[TimeSpentText]);
						Info("Description Error Canceled - Valid value has been provided");
					}
					break;
				case 122:
					ktep.ResetError(kttbr, errorMsgList[KeyText]);
					ttep.ResetError(tttbr, String.Empty);
					dtep.ResetError(dttbr, String.Empty);

					if (IsDebugModeEnabled) {
						Error("Key Error - " + errorMsgList[KeyText] + errorTypeMsgList[KeyText]);
						Error("Time Spent Error Canceled - Valid value has been provided");
						Error("Description Error Canceled - Valid value has been provided");
					}
					break;
				case 121:
					ktep.ResetError(kttbr, errorMsgList[KeyText]);
					ttep.ResetError(tttbr, String.Empty);
					dtep.ResetError(dttbr, errorMsgList[DescText]);

					if (IsDebugModeEnabled) {
						Error("Key Error - " + errorMsgList[KeyText] + errorTypeMsgList[KeyText]);
						Error("Time Spent Error Canceled - Valid value has been provided");
						Error("Description Error - " + errorMsgList[DescText] + errorTypeMsgList[DescText]);
					}
					break;
				case 211:
					ktep.ResetError(kttbr, String.Empty);
					ttep.ResetError(tttbr, errorMsgList[TimeSpentText]);
					dtep.ResetError(dttbr, errorMsgList[DescText]);

					if (IsDebugModeEnabled) {
						Error("Key Error Canceled - Valid value has been provided");
						Error("Time Spent Error - " + errorMsgList[TimeSpentText] + errorTypeMsgList[TimeSpentText]);
						Error("Description Error - " + errorMsgList[DescText] + errorTypeMsgList[DescText]);
					}
					break;
				case 221:
					ktep.ResetError(kttbr, String.Empty);
					ttep.ResetError(tttbr, String.Empty);
					dtep.ResetError(dttbr, errorMsgList[DescText]);

					if (IsDebugModeEnabled) {
						Error("Key Error Canceled - Valid value has been provided");
						Error("Time Spent Error Canceled - Valid value has been provided");
						Error("Description Error - " + errorMsgList[DescText] + errorTypeMsgList[DescText]);
					}
					break;
				case 212:
					ktep.ResetError(kttbr, String.Empty);
					ttep.ResetError(tttbr, errorMsgList[TimeSpentText]);
					dtep.ResetError(dttbr, String.Empty);

					if (IsDebugModeEnabled) {
						Error("Key Error Canceled - Valid value has been provided");
						Error("Time Spent Error - " + errorMsgList[TimeSpentText] + errorTypeMsgList[TimeSpentText]);
						Error("Description Error Canceled - Valid value has been provided");
					}
					break;
				default:
					ktep.ResetError(kttbr, String.Empty);
					ttep.ResetError(tttbr, String.Empty);
					dtep.ResetError(dttbr, String.Empty);

					if (IsDebugModeEnabled) {
						Error("Key Error Canceled - Valid value has been provided");
						Error("Time Spent Error Canceled - Valid value has been provided");
						Error("Description Error Canceled - Valid value has been provided");
					}
					break;
			}
		}

		public static (int, Dictionary<TextBoxTypes, string>, Dictionary<TextBoxTypes, string>) GetTaskEntryTextBoxValidationStatus(Dictionary<TextBoxTypes, string> textEntryList)
		{
			if(IsDebugModeEnabled)
				Debug("Getting statuses of all 3 text boxes");

			(bool keyEntryInvalid, string keyEntryErrorMsg, string keyEntryErrorTypeMsg) = ValidateKeyTextEntry(textEntryList[KeyText]);
			(bool timeSpentEntryInvalid, string timeSpentEntryErrorMsg, string timeSpentEntryErrorTypeMsg) = ValidateTimeSpentTextEntry(textEntryList[TimeSpentText]);
			(bool descriptionEntryInvalid, string descriptionEntryErrorMsg, string descriptionEntryTypeMsg) = ValidateDescriptionTextEntry(textEntryList[DescText]);

			Dictionary<TextBoxTypes, string> errorMsgList = new Dictionary<TextBoxTypes, string>();
			Dictionary<TextBoxTypes, string> errorMsgTypeList = new Dictionary<TextBoxTypes, string>();

			int returnValue = 0;
			if(keyEntryInvalid) {
				returnValue += 100;
				errorMsgList.Add(KeyText, keyEntryErrorMsg);
				errorMsgTypeList.Add(KeyText, keyEntryErrorTypeMsg);
			}
			else {
				returnValue += 200;
				errorMsgList.Add(KeyText, String.Empty);
				errorMsgTypeList.Add(KeyText, String.Empty);
			}

			if (timeSpentEntryInvalid) {
				returnValue += 10;
				errorMsgList.Add(TimeSpentText, timeSpentEntryErrorMsg);
				errorMsgTypeList.Add(TimeSpentText, timeSpentEntryErrorTypeMsg);
			}
			else {
				returnValue += 20;
				errorMsgList.Add(TimeSpentText, String.Empty);
				errorMsgTypeList.Add(TimeSpentText, String.Empty);
			}

			if(descriptionEntryInvalid) {
				returnValue += 1;
				errorMsgList.Add(DescText, descriptionEntryErrorMsg);
				errorMsgTypeList.Add(DescText, descriptionEntryTypeMsg);
			}
			else {
				returnValue += 2;
				errorMsgList.Add(DescText, descriptionEntryErrorMsg);
				errorMsgTypeList.Add(DescText, descriptionEntryTypeMsg);
			}

			return (returnValue, errorMsgList, errorMsgTypeList);
		}

		public static (bool, string, string) ValidateKeyTextEntry(string entryText)
		{
			bool isErrored = false;
			string errorMsg = String.Empty;
			string errorTypeMsg = String.Empty;

			if(String.IsNullOrWhiteSpace(entryText)) {
				isErrored = true;
				if (IsDebugModeEnabled) {
					errorMsg = "The key text box is null or empty";
					errorTypeMsg = "-- Null or Empty Text Box Error";
				}
			}
			else if(entryText.Length > 20) {
				isErrored = true;
				if (IsDebugModeEnabled) {
					errorMsg = "Key can't have more than 20 characters";
					errorTypeMsg = "-- Key Length Error";
				}
			}

			return (isErrored, errorMsg, errorTypeMsg);
		}

		public static (bool, string, string) ValidateTimeSpentTextEntry(string entryText)
		{
			bool isErrored = false;
			string errorMsg = String.Empty;
			string errorTypeMsg = String.Empty;

			if(String.IsNullOrWhiteSpace(entryText)) {
				isErrored = true;
				if (IsDebugModeEnabled) {
					errorMsg = "Time spent text box is null or empty";
					errorTypeMsg = "-- Null or Empty Text Box Error";
				}
			}
			else {
				bool isInteger = Int32.TryParse(entryText, out _);
				bool isFloat = float.TryParse(entryText, out _);
				if(!isInteger && !isFloat) {
					if(entryText.ContainsAny('h', 'H', 'm', 'M')) {
						string[] entries = entryText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
						int numOfEntries = entries.Length;
						if (entryText.ContainsAny('h', 'H') && entryText.ContainsAny('m', 'M')) {
							const int hoursEntry = 0;
							const int minutesEntry = 1;

							int indexOfH = entryText.ToLower().IndexOf('h');
							int indexOfM = entryText.ToLower().IndexOf('m');

							if(indexOfM > indexOfH) {
								isErrored = true;
								if (IsDebugModeEnabled) {
									errorMsg = "When entering time spent, hours must come before minutes";
									errorTypeMsg = "-- Hours/Minutes Formatting Error";
								}
							}
							else if(numOfEntries > 2) {
								isErrored = true;
								if (IsDebugModeEnabled) {
									errorMsg = "Please only enter hours and minutes into time spent";
									errorTypeMsg = "-- Hours/Minutes Formatting Error";
								}
							}
							else if(numOfEntries < 2) {
								isErrored = true;
								if (IsDebugModeEnabled) {
									errorMsg = "When entering in hours and minutes, please put a space between them";
									errorTypeMsg = "-- Hours/Minutes Formatting Error";
								}
							}
							else {
								bool hoursNumberValid = Int32.TryParse(entries[hoursEntry].AsSpan(0, 2), out int hours);
								bool minutesNumberValid = Int32.TryParse(entries[minutesEntry].AsSpan(0, 2), out int minutes);

								if((entries[hoursEntry].Length > 3 && entries[minutesEntry].Length > 3) ||
								   (entries[hoursEntry].Length > 3 || entries[minutesEntry].Length > 3)) 
								{
									isErrored = true;

									if(true.EqualsAll(hoursNumberValid, minutesNumberValid)) {
										if (IsDebugModeEnabled) {
											errorMsg = "When denoting hours and/or minutes, use h/H and/or m/M.\nWhen entering values for hours/minutes do not use values greater than 99 or 60 respectively";
											errorTypeMsg = "-- Hours/Minutes Value/Formatting";
										}
									}
									else if(false.EqualsAll(hoursNumberValid, minutesNumberValid)) {
										if (IsDebugModeEnabled) {
											errorMsg = "Values for hours and minutes could not be successfully converted to numbers.\n";
											errorTypeMsg = "-- Hours/Minutes Value/Formatting";
										}
									}
									else {
										if (IsDebugModeEnabled) {
											errorMsg = "When entering values for hours/minutes do not use values greater than 99 or 60 respectively";
											errorTypeMsg = "-- Hours/Minutes Value/Formatting";
										}
									}
								}
								else if((entries[hoursEntry].Length <= 3 && entries[minutesEntry].Length <= 3) &&
										(entries[hoursEntry].Length >=2 && entries[minutesEntry].Length >= 2)) 
								{
									if(false.EqualsAll(hoursNumberValid, minutesNumberValid)) {
										isErrored = true;
										if (IsDebugModeEnabled) {
											errorMsg = "Values for hours and minutes could not be successfully converted into integers";
											errorTypeMsg = "-- Hours/Minutes Integer Conversion";
										}
									}
									else if(!hoursNumberValid) {
										isErrored = true;
										if (IsDebugModeEnabled) {
											errorMsg = "Value for hours could not be successfully converted into integer";
											errorTypeMsg = "-- Hours Integer Conversion Error";
										}
									}
									else if(!minutesNumberValid) {
										isErrored = true;
										if (IsDebugModeEnabled) {
											errorMsg = "Value for minutes could not be successfully converted into integer";
											errorTypeMsg = "-- Minutes Integer Coversion Error";
										}
									}
									else {
										if(hours > 99 && minutes > 60) {
											isErrored = true;
											if (IsDebugModeEnabled) {
												errorMsg = "Value for both hours and minutes over allowable values - 99 and 60 respectively";
												errorTypeMsg = "-- Hours/Minutes Values Limits Error";
											}
										}
										else if(hours < 1 && minutes < 5) {
											isErrored = true;
											if (IsDebugModeEnabled) {
												errorMsg = "Value for both hours and minutes is under allowable values - 1 and 5 respectively";
												errorTypeMsg = "-- Hours/Minutes Values Limits Error";
											}
										}
										else if(hours > 99) {
											isErrored = true;
											if (IsDebugModeEnabled) {
												errorMsg = "Value for hours is over allowable value - 99";
												errorTypeMsg = "-- Hours Value Limits Error";
											}
										}
										else if(hours < 1) {
											isErrored = true;
											if (IsDebugModeEnabled) {
												errorMsg = "Value for hours is under allowable value - 1";
												errorTypeMsg = "-- Hours Value Limits Error";
											}
										}
										else if(minutes > 60) {
											isErrored = true;
											if (IsDebugModeEnabled) {
												errorMsg = "Value for minutes is over allowable value - 60";
												errorTypeMsg = "-- Minutes Value Limits Error";
											}
										}
										else if(minutes < 5) {
											isErrored = true;
											if (IsDebugModeEnabled) {
												errorMsg = "Value for minutes is under allowable value - 5";
												errorTypeMsg = "-- Minutes Value Limits Error";
											}
										}
									}
								}
								else {
									isErrored = true;
									if (IsDebugModeEnabled) {
										errorMsg = "Entries for hours and/or minutes is missing the value or the letter (h/H/m/M) format";
										errorTypeMsg = "-- Hours/Minutes Formatting Error";
									}
								}
							}
						}
						else if(entryText.ContainsAny('h', 'H') && !entryText.ContainsAny('m', 'M')) {
							const int hoursEntry = 0;

							if(numOfEntries > 1) {
								isErrored = true;
								if (IsDebugModeEnabled) {
									errorMsg = "Two values are present for the hours value when only one was expected";
									errorTypeMsg = "-- Hours Formatting Error";
								}
							}
							else {
								bool hoursNumberValid = Int32.TryParse(entries[hoursEntry].AsSpan(0, 2), out int hours);
								if(entries[hoursEntry].Length > 3) {
									isErrored = true;
									if (hoursNumberValid) {
										if (IsDebugModeEnabled) {
											errorMsg = "Hour value successfully converted, but entry not in correct format";
											errorTypeMsg = "-- Hours Formatting Error";
										}
									}
									else if (!hoursNumberValid) {
										if (IsDebugModeEnabled) {
											errorMsg = "Hour value not successfully convereted and entry not in correct format";
											errorTypeMsg = "-- Hours Formatting/Conversion Error";
										}
									}
								}
								else if(entries[hoursEntry].Length <= 3 && entries[hoursEntry].Length >= 2) {
									if(!hoursNumberValid) {
										isErrored = true;
										if (IsDebugModeEnabled) {
											errorMsg = "Hour value not successfully convereted into integer";
											errorTypeMsg = "-- Hours Conversion Error";
										}
									}
									else {
										if(hours > 99) {
											isErrored = true;
											if (IsDebugModeEnabled) {
												errorMsg = "Value for hour is over allowable limit - 99";
												errorTypeMsg = "-- Hour Value Limits Error";
											}
										}
										else if(hours < 1) {
											isErrored = true;
											if (IsDebugModeEnabled) {
												errorMsg = "Value for hour is under allowable limit - 1";
												errorTypeMsg = "-- Hour Value Limits Error";
											}
										}
									}
								}
								else {
									isErrored = true;
									if (IsDebugModeEnabled) {
										errorMsg = "Entry for hours is missing the value or the value is to low";
										errorTypeMsg = "-- Hour Formatting Error";
									}
								}
							}
						}
						else if(entryText.ContainsAny('m', 'M') && !entryText.ContainsAny('h', 'H')) {
							const int minutesEntry = 0;

							if (numOfEntries > 1) {
								isErrored = true;
								if (IsDebugModeEnabled) {
									errorMsg = "Two values are present for the minutes value when only one was expected";
									errorTypeMsg = "-- Minutes Formatting Error";
								}
							}
							else {
								bool minutesNumberValid = Int32.TryParse(entries[minutesEntry].AsSpan(0, 2), out int minutes);
								if (entries[minutesEntry].Length > 3) {
									isErrored = true;
									if (minutesNumberValid) {
										if (IsDebugModeEnabled) {
											errorMsg = "Minutes value successfully converted, but entry not in correct format";
											errorTypeMsg = "-- Minutes Formatting Error";
										}
									}
									else if (!minutesNumberValid) {
										if (IsDebugModeEnabled) {
											errorMsg = "Minutes value not successfully convereted and entry not in correct format";
											errorTypeMsg = "-- Minutes Formatting/Conversion Error";
										}
									}
								}
								else if (entries[minutesEntry].Length <= 3 && entries[minutesEntry].Length >= 2) {
									if (!minutesNumberValid) {
										isErrored = true;
										if (IsDebugModeEnabled) {
											errorMsg = "Minutes value not successfully convereted into integer";
											errorTypeMsg = "-- Minutes Conversion Error";
										}
									}
									else {
										if (minutes > 60) {
											isErrored = true;
											if (IsDebugModeEnabled) {
												errorMsg = "Value for minutes is over allowable limit - 60";
												errorTypeMsg = "-- Minute Value Limits Error";
											}
										}
										else if (minutes < 5) {
											isErrored = true;
											if (IsDebugModeEnabled) {
												errorMsg = "Value for minute is under allowable limit - 5";
												errorTypeMsg = "-- Minute Value Limits Error";
											}
										}
									}
								}
								else {
									isErrored = true;
									if (IsDebugModeEnabled) {
										errorMsg = "Entry for minutes is missing the value or the value is to low";
										errorTypeMsg = "-- Minutes Formatting Error";
									}
								}
							}
						}
					}
					else {
						isErrored = true;
						if (IsDebugModeEnabled) {
							errorMsg = "Unknown value entered or value is in an unknown format";
							errorTypeMsg = "-- Unknown Value/Format Error";
						}
					}
				}
				else if(isInteger && !isFloat) {
					int timeSpent = Int32.Parse(entryText);
					if(timeSpent == 0) {
						isErrored = true;
						if (IsDebugModeEnabled) {
							errorMsg = "Value cannot be zero";
							errorTypeMsg = "-- Zero Value Integer Limit Error";
						}
					}
					else if(timeSpent < 0) {
						isErrored = true;
						if (IsDebugModeEnabled) {
							errorMsg = "Value cannot be a negative number";
							errorTypeMsg = "-- Negative Value Integer Limit Error";
						}
					}
					else if(timeSpent > 99) {
						isErrored = true;
						if (IsDebugModeEnabled) {
							errorMsg = "Value cannot be greater than 99";
							errorTypeMsg = "-- High Value Integer Limit Error";
						}
					}
				}
				else if(isFloat && !isInteger) {
					float timeSpent = float.Parse(entryText);
					if(timeSpent == 0.0f) {
						isErrored = true;
						if (IsDebugModeEnabled) {
							errorMsg = "Value cannot be zero";
							errorTypeMsg = "-- Zero Value Float Limit Error";
						}
					}
					else if(timeSpent < 0.0f) {
						isErrored = true;
						if (IsDebugModeEnabled) {
							errorMsg = "Value cannot be a negative number";
							errorTypeMsg = "-- Negative Value Float Limit Error";
						}
					}
					else if(timeSpent > 99.0f) {
						isErrored = true;
						if (IsDebugModeEnabled) {
							errorMsg = "Value cannot be greater than 99";
							errorTypeMsg = "-- High Value Float Limit Error";
						}
					}
				}
			}

			return (isErrored, errorMsg, errorTypeMsg);
		}

		public static (bool, string, string) ValidateDescriptionTextEntry(string entryText)
		{
			bool isErrored = false;
			string errorMsg = String.Empty;
			string errorTypeMsg = String.Empty;

			int textLength = entryText.Length;
			if(String.IsNullOrWhiteSpace(entryText)) {
				isErrored = true;
				if (IsDebugModeEnabled) {
					errorMsg = "Description cannot be black or null";
					errorTypeMsg = "-- Blank or Null Description Entry";
				}
			}
			else if(textLength < 3) {
				isErrored = true;
				if (IsDebugModeEnabled) {
					errorMsg = "Description must be at least 3 characters or longer";
					errorTypeMsg = "-- Description Length Less Than Three";
				}
			}
			else if(textLength > 500) {
				isErrored = true;
				if (IsDebugModeEnabled) {
					errorMsg = "Description cannot be longer than 500 characters";
					errorTypeMsg = "-- Description Length Greater than 500";
				}
			}

			return (isErrored, errorMsg, errorTypeMsg);
		}
	}

	public partial class TaskerMainWindow
	{
		//-- Start Time Text Box Event Functions
		private void StartTimeTextBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (startTimeTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Mouse click event was triggered on the start time text box");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(startTimeTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				startTimeTextBox.Focus();
			}
		}

		private void StartTimeTextBox_MouseHover(object sender, EventArgs e)
		{
			if (startTimeTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Start Time Text Box mouse hover event has been triggered");

				taskerMainWinToolTip.Show("Enter a start time in the following format: HH:MM", startTimeTextBox, 5000);

				isToolTipShown = true;
				toolTipTimer.Enabled = true;
				toolTipTimer.Start();
			}
		}

		private void StartTimeTextBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (startTimeTextBox.Enabled) {
				if (mouseMoveEnabled) {
					if (IsDebugModeEnabled)
						EventLog("Start time text box mouse move event has been triggered");

					if (isToolTipShown) {
						taskerMainWinToolTip.Hide(startTimeTextBox);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void StartTimeTextBox_MouseLeave(object sender, EventArgs e)
		{
			if (startTimeTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Start Time Text Box mouse leave event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(startTimeTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void StartTimeTextBox_Leave(object sender, EventArgs e)
		{
			if (startTimeTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Start time text box leave event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(startTimeTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void StartTimeTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if(startTimeTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Start time text box key down event has been triggered");

				if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
					endTimeTextBox.Focus();

					if (isToolTipShown) {
						taskerMainWinToolTip.Hide(startTimeTextBox);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void StartTimeTextBox_TextChanged(object sender, EventArgs e)
		{
			if(startTimeTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Start time text box text changed event has been triggered");

				taskerSettings.StartTimeValue = startTimeTextBox.Text;
			}
		}


		//-- End Time Text Box Event Functions
		private void EndTimeTextBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (endTimeTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Mouse click event was triggered on the end time text box");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(endTimeTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				endTimeTextBox.Focus();
			}
		}

		private void EndTimeTextBox_MouseHover(object sender, EventArgs e)
		{
			if (endTimeTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("End Time Text Box mouse hover event has been triggered");

				taskerMainWinToolTip.Show("Enter a end time in the following format: HH:MM", endTimeTextBox, 5000);

				isToolTipShown = true;
				toolTipTimer.Enabled = true;
				toolTipTimer.Start();
			}
		}

		private void EndTimeTextBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (endTimeTextBox.Enabled) {
				if (mouseMoveEnabled) {
					if (IsDebugModeEnabled)
						EventLog("End time text box mouse move event has been triggered");

					if (isToolTipShown) {
						taskerMainWinToolTip.Hide(endTimeTextBox);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void EndTimeTextBox_MouseLeave(object sender, EventArgs e)
		{
			if (endTimeTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("End Time Text Box mouse leave event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(endTimeTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void EndTimeTextBox_Leave(object sender, EventArgs e)
		{
			if (endTimeTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("End time text box leave event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(endTimeTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void EndTimeTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if(endTimeTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("End time text box key down event has been triggered");

				if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
					remindIntervTextBox.Focus();

					if(isToolTipShown) {
						taskerMainWinToolTip.Hide(endTimeTextBox);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void EndTimeTextBox_TextChanged(object sender, EventArgs e)
		{
			if(endTimeTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("End time text box text changed event has been triggered");

				taskerSettings.EndTimeValue = endTimeTextBox.Text;
			}
		}


		//-- Reminder Interval Text Box Event Functions
		private void RemindIntervTextBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (remindIntervTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Mouse click event was triggered on the reminder interval text box");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(remindIntervTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}

				remindIntervTextBox.Focus();
			}
		}

		private void RemindIntervTextBox_MouseHover(object sender, EventArgs e)
		{
			if (remindIntervTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Reminder Interval Text Box mouse hover event has been triggered");

				taskerMainWinToolTip.Show("Enter the amount of time you want between task entry prompts", remindIntervTextBox, 5000);

				isToolTipShown = true;
				toolTipTimer.Enabled = true;
				toolTipTimer.Start();
			}
		}

		private void RemindIntervTextBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (remindIntervTextBox.Enabled) {
				if (mouseMoveEnabled) {
					if (IsDebugModeEnabled)
						EventLog("Reminder interval text box mouse move event has been triggered");

					if (isToolTipShown) {
						taskerMainWinToolTip.Hide(remindIntervTextBox);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void RemindIntervTextBox_MouseLeave(object sender, EventArgs e)
		{
			if (remindIntervTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Reminder Interval Text Box mouse leave event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(remindIntervTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}
		
		private void RemindIntervTextBox_Leave(object sender, EventArgs e)
		{
			if (remindIntervTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Reminder Interval leave event has been triggered");

				if (isToolTipShown) {
					taskerMainWinToolTip.Hide(remindIntervTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void RemindIntervTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if(remindIntervTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Reminder interval text box key down event has been triggered");

				if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
					startTrackButton.Focus();

					if(isToolTipShown) {
						taskerMainWinToolTip.Hide(remindIntervTextBox);
						isToolTipShown = false;
						mouseMoveEnabled = false;
						toolTipTimer.Stop();
					}
				}
			}
		}

		private void RemindIntervTextBox_TextChanged(object sender, EventArgs e)
		{
			if(remindIntervTextBox.Enabled) {
				if (IsDebugModeEnabled)
					EventLog("Reminder interval text box text changed event has been triggered");

				taskerSettings.IntervalTimeValue = remindIntervTextBox.Text;
			}
		}
	}

	public partial class TaskEntryWindow
	{
		//-- Key Entry Text Box Event Functions
		private void KeyEntryTextBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Key entry text box mouse click event has been triggered");

			if(isToolTipShown) {
				taskEntryWindowToolTip.Hide(keyEntryTextBox);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			keyEntryTextBox.Focus();
		}

		private void KeyEntryTextBox_MouseHover(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Key entry text box mouse hover event has been triggered");

			taskEntryWindowToolTip.Show("Enter a task key into the text box", keyEntryTextBox, 5000);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void KeyEntryTextBox_MouseMove(object sender, MouseEventArgs e)
		{
			if(mouseMoveEnabled) {
				if (IsDebugModeEnabled)
					EventLog("Key entry text box mouse move event has been triggered");

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(keyEntryTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void KeyEntryTextBox_MouseLeave(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Key entry text box mouse leave event has been triggered");

			taskEntryWindowToolTip.Hide(keyEntryTextBox);
			toolTipTimer.Stop();
			isToolTipShown = false;
			mouseMoveEnabled = false;
		}

		private void KeyEntryTextBox_Leave(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Key entry text box leave event has been triggered");

			if(isToolTipShown) {
				taskEntryWindowToolTip.Hide(keyEntryTextBox);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			if (String.IsNullOrWhiteSpace(keyEntryTextBox.Text))
				isKeyEntryNull = true;
			else
				isKeyEntryNull = false;
		}

		private void KeyEntryTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Key entry text box key down event has been triggered");

			if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
				timeSpentTextBox.Focus();

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(keyEntryTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}



		//-- Time Spent Text Box Event Functions
		private void TimeSpentTextBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Time spent text box mouse click event was triggered");

			if(isToolTipShown) {
				taskEntryWindowToolTip.Hide(timeSpentTextBox);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			timeSpentTextBox.Focus();
		}

		private void TimeSpentTextBox_MouseHover(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Time spent text box mouse hover event was triggered");

			taskEntryWindowToolTip.Show("Enter the time spent on the task into this text box", timeSpentTextBox, 5000);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void TimeSpentTextBox_MouseMove(object sender, MouseEventArgs e)
		{
			if(mouseMoveEnabled) {
				if (IsDebugModeEnabled)
					EventLog("Time spent text box mouse move event was triggered");

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(timeSpentTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void TimeSpentTextBox_MouseLeave(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Time spent text box mouse leave event was triggered");

			taskEntryWindowToolTip.Hide(timeSpentTextBox);
			toolTipTimer.Stop();
			isToolTipShown = false;
			mouseMoveEnabled = false;
		}

		private void TimeSpentTextBox_Leave(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Time spent text box leave event was triggered");

			if(isToolTipShown) {
				taskEntryWindowToolTip.Hide(timeSpentTextBox);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			if (String.IsNullOrWhiteSpace(timeSpentTextBox.Text))
				isTimeSpentEntryNull = true;
			else
				isTimeSpentEntryNull = false;
		}

		private void TimeSpentTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Time spent text box key down event has been triggered");

			if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
				descriptionTextBox.Focus();

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(timeSpentTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}



		//-- Description Text Box Event Functions
		private void DescriptionTextBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Description text box mouse click event has been triggered");

			if(isToolTipShown) {
				taskEntryWindowToolTip.Hide(descriptionTextBox);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			descriptionTextBox.Focus();
		}

		private void DescriptionTextBox_MouseHover(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Description text box mouse hover event has been triggered");

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void DescriptionTextBox_MouseMove(object sender, MouseEventArgs e)
		{
			if(mouseMoveEnabled) {
				if (IsDebugModeEnabled)
					EventLog("Description text box mouse move event has been triggered");

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(descriptionTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}

		private void DescriptionTextBox_MouseLeave(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Description text box mouse leave event has been triggered");

			taskEntryWindowToolTip.Hide(descriptionTextBox);
			toolTipTimer.Stop();
			isToolTipShown = false;
			mouseMoveEnabled = false;
		}

		private void DescriptionTextBox_Leave(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Description text box leave event has been triggered");

			if(isToolTipShown) {
				taskEntryWindowToolTip.Hide(descriptionTextBox);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			if (String.IsNullOrWhiteSpace(descriptionTextBox.Text))
				isDescriptionEntryNull = true;
			else
				isDescriptionEntryNull = false;
		}

		private void DescriptionTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("Description text box key down event has been triggered");

			if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
				okButton.Focus();

				if(isToolTipShown) {
					taskEntryWindowToolTip.Hide(descriptionTextBox);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
		}
	}
}