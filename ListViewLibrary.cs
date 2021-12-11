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
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using ClosedXML.Excel;

using static Tasker.TaskerVariables;
using static Tasker.LogCalls;

namespace Tasker
{
	//This is not a general use class and is specifically written to work with a ListView object
	//Once this class has been instantiated, the number of columns is assumed to not change while application is running
	public class ListViewOperations
	{
		private class ListViewCellTracking
		{
			readonly Dictionary<int, int[]> trackingTable;
			List<(int, int)> cellList;
			int rowIncrement;

			//Set when class is instantiated and doesn't change for the remainder of the active session
			readonly int numOfColumns;

			bool multipleCellsSelected;

			public ListViewCellTracking(int columns)
			{
				trackingTable = new();
				cellList = new();
				rowIncrement = 0;
				numOfColumns = columns;

				multipleCellsSelected = false;
				if(IsDebugModeEnabled)
					Debug("List view cell tracking has been setup");
			}

			public bool MultipleCellsSelected
			{
				get
				{
					return multipleCellsSelected;
				}
			}

			//When a new row of cells is added, all values are initially set to false
			public void AddNewRowCells()
			{
				if(IsDebugModeEnabled)
					Debug("New row has been added to list view. Creating row of cells in cell tracker for tracking");

				int[] rowArr = new int[numOfColumns];
				for (int c = 0; c < numOfColumns; c++) {
					rowArr[c] = false.ToInt();
				}

				trackingTable.Add(rowIncrement++, rowArr);

				for (int x = 0; x < numOfColumns; x++) {
					cellList.Add((rowIncrement, x));
				}
			}

			public void AddMultipleRowsCell(int totalRows)
			{
				for (int r = 0; r < totalRows; r++) {
					int[] newRow = new int[numOfColumns];
					for (int c = 0; c < numOfColumns; c++) {
						newRow[c] = false.ToInt();
					}

					trackingTable.Add(rowIncrement++, newRow);
				}

				for (int y = 0; y < totalRows; y++) {
					for (int x = 0; x < numOfColumns; x++) {
						cellList.Add((y, x));
					}
				}
			}

			public void SetCellSelected(int yCoord, int xCoord = 0)
			{
				trackingTable[yCoord].SetValue(true.ToInt(), xCoord);

				multipleCellsSelected = false;
			}

			public void SetMultipleCellsSelected(ref List<(int, int)> selectedCells)
			{
				foreach ((int y, int x) in selectedCells) {
					trackingTable[y].SetValue(true.ToInt(), x);
				}

				multipleCellsSelected = true;
			}

			public void SetCellUnselected(int yCoord, int xCoord = 0)
			{
				trackingTable[yCoord].SetValue(false.ToInt(), xCoord);
			}

			public void SetMultipleCellsUnselected(ref List<(int, int)> unselectedCells)
			{
				foreach ((int y, int x) in unselectedCells) {
					trackingTable[y].SetValue(false.ToInt(), x);
				}
			}

			public List<(int, int)> FindSelectedCells()
			{
				List<(int, int)> selectedCellList = new();

				foreach ((int yC, int[] xCArr) in trackingTable) {
					for (int i = 0; i < xCArr.Length; i++) {
						if (xCArr[i].ToBool() == true)
							selectedCellList.Add((yC, i));
					}
				}

				return selectedCellList;
			}

			public List<(int, int)> FindAllUnselectedCells()
			{
				List<(int, int)> unselectedCellList = new();

				foreach ((int yC, int[] xCArr) in trackingTable) {
					for (int i = 0; i < xCArr.Length; i++) {
						if (xCArr[i].ToBool() == false)
							unselectedCellList.Add((yC, i));
					}
				}

				return unselectedCellList;
			}

			public bool IsCellSelected(int yCoord, int xCoord = 0)
			{
				return ((int)trackingTable[yCoord].GetValue(xCoord)!).ToBool();
			}

			public void SetAllCellsSelected()
			{
				for (int yCoord = 0; yCoord < trackingTable.Count; yCoord++) {
					for (int xCoord = 0; xCoord < trackingTable[yCoord].Length; xCoord++) {
						trackingTable[yCoord].SetValue(true.ToInt(), xCoord);
					}
				}
			}

			public void SetAllCellsUnselected()
			{
				for (int yCoord = 0; yCoord < trackingTable.Count; yCoord++) {
					for (int xCoord = 0; xCoord < trackingTable[yCoord].Length; xCoord++) {
						trackingTable[yCoord].SetValue(false.ToInt(), xCoord);
					}
				}
			}

			public ref List<(int, int)> GetCellList()
			{
				return ref cellList;
			}
		}

		//Column ID and rectangle of column
		private readonly List<(int, Rectangle)> listViewColumnList;
		//Row ID and rectangle of row
		private readonly List<(int, Rectangle)> listViewRowList;
		//Used to track which cells in the list view have been selected.
		//Will be used in conjunction with copy/paste functionality
		private readonly ListViewCellTracking cellTracker;
		//Permanent session reference to listView
		private readonly ListViewWithScrollEvent lv;

		//Used to store previously selected sub-item/sub-tems within the listView
		int prevSelectedColumn;
		int prevSelectedRow;
		bool multiSelect;
		readonly List<(int, int)> prevSelectedCRs;

		bool isLstTrckInit;

		int totalColumns;
		int totalRows;

		bool isMouseOverListView;
		bool isRightClickActive;

		public ListViewOperations(ref ListViewWithScrollEvent listViewRef)
		{
			if(IsDebugModeEnabled)
				Debug("Starting mouse location tracker instance");

			listViewColumnList = new List<(int, Rectangle)>();
			listViewRowList = new List<(int, Rectangle)>();
			
			if(IsDebugModeEnabled)
				Debug("Setup column and row lists for list view tracking");

			lv = listViewRef;

			isLstTrckInit = false;
			totalColumns = 0;
			totalRows = 0;

			isMouseOverListView = false;
			isRightClickActive = false;

			if(IsDebugModeEnabled)
				Debug("Setting initial tracker values");

			cellTracker = new(lv.Columns.Count);

			if(IsDebugModeEnabled)
				Debug("Zeroing out previous selected cell trackers");

			prevSelectedColumn = 0;
			prevSelectedRow = 0;
			multiSelect = false;
			prevSelectedCRs = new();
			prevSelectedCRs.Add((-1, -1));
		}

		public int IsMouseOverLV
		{
			get
			{
				if (isLstTrckInit && isMouseOverListView)
					return true.ToInt();
				else if (isLstTrckInit && !isMouseOverListView)
					return false.ToInt();
				else
					throw new Exception("");
			}
		}

		public bool SetMouseOverLV
		{
			set
			{
				if (isLstTrckInit)
					isMouseOverListView = value;
			}
		}

		public int IsRightClickActive
		{
			get
			{
				if (isRightClickActive && isLstTrckInit)
					return true.ToInt();
				else if (!isRightClickActive && isLstTrckInit)
					return false.ToInt();
				else
					throw new Exception("");
			}
		}

		public bool SetRightClickActive
		{
			set
			{
				if (isLstTrckInit)
					isRightClickActive = value;
			}
		}

		public void IntitializeMouseTracking()
		{
			//Rectangle object to be used to generate new rectangle objects in the loop
			Rectangle colRect;

			if(IsDebugModeEnabled)
				Debug("Generating virtual columns for mouse tracking");

			//Get rectangles for columns
			//Total accumulated width of all processed columns in a list view
			int totalColWidth = 0;
			//Total number of columns in the list view
			int numOfCol = lv.Columns.Count;
			for (int i = 0; i < numOfCol; i++) {
				//Is this the first column
				if (i == 0)
					//Set locaton of rectangle within the form, then set the width and height of rectangle
					colRect = new(0, 0, lv.Columns[i].Width, lv.Height);
				else
					//For every other column in the list view
					colRect = new(totalColWidth, 0, lv.Columns[i].Width, lv.Height);

				//Add new rectangle to column list
				listViewColumnList.Add((++totalColumns, colRect));
				if (IsDebugModeEnabled) {
					Debug("--------------------------------");
					Debug("Column ID: " + totalColumns.ToString());
					Debug("Column X Loc: " + colRect.X.ToString());
					Debug("Column Y Loc: " + colRect.Y.ToString());
					Debug("Column Width: " + colRect.Width.ToString());
					Debug("Column Height: " + colRect.Height.ToString());
					Debug("--------------------------------");
				}

				//Update total width - Add curent column width to total
				//The total width will serve as the starting X position of the next rectangle
				totalColWidth += lv.Columns[i].Width;
				if (IsDebugModeEnabled) {
					Debug("Total Columns Processed: " + totalColumns.ToString());
					Debug("Total Columns Width: " + totalColWidth.ToString());
					Debug("--------------------------------");
				}
			}

			//Rectangle object to be used to generate new rectangle objects in the loop
			Rectangle rowRect;

			if(IsDebugModeEnabled)
				Debug("Generating virtual rows for mouse tracking");

			//Get rectangles for rows
			//Total number of rows in the list view
			int numOfRows = lv.Items.Count;
			//Check to make sure there are rows initially to calculate rectangles for rows
			if (numOfRows != 0) {
				for (int i = 0; i < numOfRows; i++) {
					//Setup new rectangle for new row
					rowRect = new(0, lv.Items[i].Bounds.Y, lv.Items[i].Bounds.Width, lv.Items[i].Bounds.Height);

					//Add new rectangle to row list
					listViewRowList.Add((++totalRows, rowRect));
					if (IsDebugModeEnabled) {
						Debug("--------------------------------");
						Debug("Row ID: " + totalRows.ToString());
						Debug("Row X Loc: " + rowRect.X.ToString());
						Debug("Row Y Loc: " + rowRect.Y.ToString());
						Debug("Row Width: " + rowRect.Width.ToString());
						Debug("Row Height: " + rowRect.Height.ToString());
						Debug("--------------------------------");

						//Update total height - Add current row height to total
						//The total height will service as the starting Y position of the next rectangle
						Debug("Total Rows Processed: " + totalRows.ToString());
						Debug("Total Row Width: " + rowRect.Width.ToString());
						Debug("--------------------------------");
					}
				}
			}

			if (totalRows != 0)
				cellTracker.AddMultipleRowsCell(numOfRows);

			isLstTrckInit = true;
		}

		//This should be run after the new row is added into list view
		public void AddNewRow()
		{
			if (isLstTrckInit) {
				//Increment number of rows as we're adding a new row
				totalRows++;
				//Get the index of the listView item that corresponds to the new row
				int index = totalRows - 1;
				//Create new rectangle to store row bounds
				Rectangle newRowRect = new(0, lv.Items[index].Bounds.Y, lv.Items[index].Bounds.Width, lv.Items[index].Bounds.Height);

				//Add new row rectangle to list
				listViewRowList.Add((totalRows, newRowRect));

				//Tell the cell tracker that a new row has been added
				cellTracker.AddNewRowCells();

				if (IsDebugModeEnabled) {
					Debug("--------------------------------");
					Debug("New virtual row generated:");
					Debug("Column ID: " + totalRows.ToString());
					Debug("Row X Loc: " + newRowRect.X.ToString());
					Debug("Row Y Loc: " + newRowRect.Y.ToString());
					Debug("Row Width: " + newRowRect.Width.ToString());
					Debug("Row Height: " + newRowRect.Height.ToString());
					Debug("--------------------------------");
				}
			}
		}

		//columnIndex: The index used with the listViewColumnList to find the column
		//resizedColumnID: The actual column id that's stored with each column entry in the list
		//newWidth: The new width
		public void UpdateAfterColumnWidthResize(int columnIndex, int resizedColumnID, int newWidth)
		{
			if (isLstTrckInit) {
				if (newWidth != listViewColumnList[columnIndex].Item2.Width) {
					if (IsDebugModeEnabled) {
						Debug("-----------------------------------------------------------");
						Debug("Recalculating virtual column size after column resize event");
						Debug("-----------------------------------------------------------");
					}
					int totalColWidth = 0;
					for (int i = 0; i < listViewColumnList.Count; i++) {
						(int columnID, Rectangle column) = listViewColumnList[i];
						if (columnID < resizedColumnID)
							totalColWidth += column.Width;
						else if (columnID == resizedColumnID) {
							Rectangle newColumn = listViewColumnList[columnIndex].Item2;

							if (IsDebugModeEnabled) {
								Debug("Resized Column ID: " + resizedColumnID.ToString());
								Debug("Old Column Width: " + newColumn.Width.ToString());
							}
							newColumn.Width = newWidth;

							if (IsDebugModeEnabled)
								Debug("New Column Width: " + newColumn.Width.ToString());

							listViewColumnList[columnIndex++] = (columnID, newColumn);

							totalColWidth += newWidth;

							if (IsDebugModeEnabled)
								Debug("Recalculating remaining columns if any exist");
						}
						else if (columnID > resizedColumnID) {
							Rectangle adjustedColumn = listViewColumnList[columnIndex].Item2;
							if (IsDebugModeEnabled)
								Debug("Readjusting X Location for Column ID: " + columnID.ToString());

							adjustedColumn.X = totalColWidth;

							if (IsDebugModeEnabled)
								Debug("New X Location: " + adjustedColumn.X.ToString());

							listViewColumnList[columnIndex++] = (columnID, adjustedColumn);

							totalColWidth += adjustedColumn.Width;
						}
					}

					if (IsDebugModeEnabled) {
						Debug("-----------------------------------------------------------");
						Debug("Ending column width recalculation...");
						Debug("-----------------------------------------------------------");

						Debug("-----------------------------------------------------------");
						Debug("Recalculating total row length using total column width");
						Debug("-----------------------------------------------------------");
					}

					for (int i = 0; i < listViewRowList.Count; i++) {
						(int rowID, Rectangle oldRow) = listViewRowList[i];

						//Display row id and old value for debugging purposes
						if (IsDebugModeEnabled) {
							Debug("Resize Row ID: " + rowID.ToString());
							Debug("Old Total Row Length: " + oldRow.ToString());
						}

						//Create new rectangle and display new values for debugging purposes
						Rectangle newRow = oldRow;
						newRow.Width = totalColWidth;
						if (IsDebugModeEnabled)
							Debug("New Total Row Length: " + newRow.ToString());

						listViewRowList[i] = (rowID, newRow);
					}

					if (IsDebugModeEnabled) {
						Debug("-----------------------------------------------------------");
						Debug("Ending row length recalculation...");
						Debug("-----------------------------------------------------------");
					}
				}
				else {
					if(IsDebugModeEnabled)
						Debug("Column width resize event was triggered, but no change in column width occurred");
				}
			}
		}

		public void UpdateAfterColumnHeightResize(int columnHeight)
		{
			if (isLstTrckInit) {
				if (columnHeight != listViewColumnList[0].Item2.Height) {
					if (IsDebugModeEnabled) {
						Debug("-----------------------------------------------------------");
						Debug("Recalculating column height after window resize");
						Debug("-----------------------------------------------------------");
					}
					for (int i = 0; i < listViewColumnList.Count; i++) {
						Rectangle newColumn = listViewColumnList[i].Item2;

						if (IsDebugModeEnabled) {
							Debug("Current Column ID: " + listViewColumnList[i].Item1.ToString());
							Debug("Old Column Height: " + newColumn.Height.ToString());
						}
						newColumn.Height = columnHeight;
						if (IsDebugModeEnabled)
							Debug("New Column Height: " + newColumn.Height.ToString());

						listViewColumnList[i] = (listViewColumnList[i].Item1, newColumn);
					}
				}
				else {
					if(IsDebugModeEnabled)
						Debug("Column height resize event was triggered, but no change in column height occurred");
				}
			}
		}

		//Get the X,Y coordinates of the cell that was clicked on in the list view
		//When sending back a response, the format is: rowID, columnID
		public (int, int) GetMouseLocationOnClick(Point p)
		{
			int columnID = -1;
			int rowID = -1;

			if (IsDebugModeEnabled) {
				if (!IsLVMouseTrackDebugEnabled) {
					Debug("Mouse Location On Click: ");
					Debug("Mouse X: " + p.X.ToString());
					Debug("Mouse Y: " + p.Y.ToString());
				}
			}

			if (isLstTrckInit) {
				for (int i = 0; i < totalRows; i++) {
					if (listViewRowList[i].Item2.Contains(p)) {
						rowID = listViewRowList[i].Item1;
						if (IsDebugModeEnabled) {
							if (!IsLVMouseTrackDebugEnabled) {
								Debug("--Row match has been found--");
								Debug("Row ID: " + rowID.ToString());
							}
						}
						break;
					}
				}

				for (int i = 0; i < totalColumns; i++) {
					if (listViewColumnList[i].Item2.Contains(p)) {
						columnID = listViewColumnList[i].Item1;
						if (IsDebugModeEnabled) {
							if (!IsLVMouseTrackDebugEnabled) {
								Debug("--Column match has been found--");
								Debug("Column ID: " + columnID.ToString());
							}
						}
						break;
					}
				}

				if (IsDebugModeEnabled) {
					if (!IsLVMouseTrackDebugEnabled) {
						Debug("---------------------------------");
						Debug("--Raw Mouse X,Y Output On Click--");
						Debug("X Coord: " + p.X.ToString());
						Debug("Y Coord: " + p.Y.ToString());
						Debug("---------------------------------");
					}
				}
			}
			else {
				if (IsDebugModeEnabled) {
					if (!IsLVMouseTrackDebugEnabled)
						Error("Mouse tracking has not been initialized yet");
				}
			}

			return (rowID, columnID);
		}

		public void ChangeListViewSelection(ref List<(int, int)> cellSelectionList)
		{
			if (cellSelectionList.Count == 1) {
				if(IsDebugModeEnabled)
					Debug("Only one cell was selected");

				SelectSingleCell(cellSelectionList[0].Item1, cellSelectionList[0].Item2);
			}
			else {
				if(IsDebugModeEnabled)
					Debug("More than one cell was selected");

				SelectMultipleCells(ref cellSelectionList);
			}
		}

		public void ChangeListViewSelection(int row, int column)
		{
			if(IsDebugModeEnabled)
				Debug("Only one cell was selected");

			SelectSingleCell(row, column);
		}

		private void SelectSingleCell(int row, int column)
		{
			const int offset = -1;
			int yCoord = row + offset;
			int xCoord = column + offset;

			//If prevSelectedColumn/Row are both not zero, then a cell has been clicked on previously
			bool alreadySelected = false;
			if (multiSelect) {
				if(IsDebugModeEnabled)
					Debug("Multiple cells were selected previously. Previous cells will now be unselected before continuing");

				if (prevSelectedCRs.Contains((row, column))) {
					prevSelectedCRs.Remove((row, column));
					alreadySelected = true;
				}

				SetMultipleCellsUnselected(prevSelectedCRs);

				multiSelect = false;
				prevSelectedCRs.Clear();
				prevSelectedCRs.Add((-1, -1));
			}
			else {
				if (prevSelectedRow != 0 && prevSelectedColumn != 0) {
					if (prevSelectedRow == row && prevSelectedColumn == column) {
						//If prevSelectedColumn/Row are the same as previously selected, then just toggle the color
						if(IsDebugModeEnabled)
							Debug("Previously selected cell is the same as currently selected cell");
						if (cellTracker.IsCellSelected(yCoord, xCoord)) {
							SetCellUnselected(yCoord, xCoord);

							if(IsDebugModeEnabled)
								Debug("Cell - " + row.ToString() + ", " + column.ToString() + " was selected, is now unselected");
						}
						else {
							SetCellSelected(yCoord, xCoord);
							
							if(IsDebugModeEnabled)
								Debug("Cell - " + row.ToString() + ", " + column.ToString() + " was unselected, is now selected");
						}
					}
					else if (prevSelectedRow != row || prevSelectedColumn != column) {
						if (IsDebugModeEnabled) {
							Debug("Previously selected cell is not the same as the currently selected cell");
							Debug("Previously selected cell: " + prevSelectedRow.ToString() + ", " + prevSelectedColumn.ToString());
						}
						int prevYCoord = prevSelectedRow - 1;
						int prevXCoord = prevSelectedColumn - 1;

						SetCellUnselected(prevYCoord, prevXCoord);
						
						if(IsDebugModeEnabled)
							Debug("Previously selected cell - " + prevSelectedRow.ToString() + ", " + prevSelectedColumn.ToString() + " is now unselected");
					}
				}
			}

			if (!alreadySelected) {
				SetCellSelected(yCoord, xCoord);
				if(IsDebugModeEnabled)
					Debug("Currently selected cell: " + row.ToString() + ", " + column.ToString() + " is now selected");
			}

			prevSelectedRow = row;
			prevSelectedColumn = column;
		}

		public void SelectMultipleCells(ref List<(int, int)> selectedCells)
		{
			const int offset = -1;
			List<(int, int)> cellUnselectList = new List<(int, int)>();
			List<(int, int)> cellSelectList = new List<(int, int)>();

			if(IsDebugModeEnabled)
				Debug("Check if multiple cells have been previously selected");

			if (!multiSelect) {
				if (IsDebugModeEnabled) {
					Debug("MultiSelect set to false. Only one cell selected previous to this");
					Debug("Previously selected single cell: " + prevSelectedRow.ToString() + ", " + prevSelectedColumn.ToString());
				}

				if (selectedCells.Contains((prevSelectedRow, prevSelectedColumn))) {
					if (IsDebugModeEnabled) {
						FullDebug("Previously selected single cell was found in current selection");

						Debug("Removing cell: " + prevSelectedRow.ToString() + ", " + prevSelectedColumn.ToString() + " from list as it has already been selected");
					}
					
					selectedCells.Remove((prevSelectedRow, prevSelectedColumn));
				}
				else {
					if(IsDebugModeEnabled)
						Debug("Adding cell: " + prevSelectedRow.ToString() + ", " + prevSelectedColumn.ToString() + " to deselection list");

					cellUnselectList.Add((prevSelectedRow + offset, prevSelectedColumn + offset));
				}

				prevSelectedRow = 0;
				prevSelectedColumn = 0;
			}
			else {
				foreach ((int prevRowSelection, int prevColumnSelection) in prevSelectedCRs) {
					if (IsDebugModeEnabled) {
						Debug("MultiSelect set to true. More than one cell selected previous to this");
						Debug("Previously selected cell: " + prevRowSelection.ToString() + ", " + prevColumnSelection.ToString());
					}

					if (selectedCells.Contains((prevRowSelection, prevColumnSelection))) {
						if (IsDebugModeEnabled) {
							FullDebug("Previously selected single cell was found in current selection");

							Debug("Removing cell: " + prevRowSelection.ToString() + ", " + prevColumnSelection.ToString() + " from list as it has already been selected");
						}

						selectedCells.Remove((prevRowSelection, prevColumnSelection));
					}
					else {
						if(IsDebugModeEnabled)
							Debug("Adding cell: " + prevRowSelection.ToString() + ", " + prevColumnSelection.ToString() + " to deselection list");

						cellUnselectList.Add((prevRowSelection + offset, prevColumnSelection + offset));
					}
				}
			}

			foreach ((int row, int column) in selectedCells) {
				cellSelectList.Add((row + offset, column + offset));
			}

			SetMultipleCellsUnselected(cellUnselectList);
			SetMultipleCellsSelected(cellSelectList);
		}


		public (List<(int, int)>, int) GetSelectedCells()
		{
			int numOfSelectedCells;
			List<(int, int)> selectedCells;
			if (!cellTracker.MultipleCellsSelected) {
				numOfSelectedCells = 1;
				selectedCells = cellTracker.FindSelectedCells();
			}
			else {
				selectedCells = cellTracker.FindSelectedCells();
				numOfSelectedCells = selectedCells.Count;
			}

			return (selectedCells, numOfSelectedCells);
		}

		public string GetCellValue(int yCoord, int xCoord = 0)
		{
			string returnValue;
			if (xCoord == 0)
				returnValue = lv.Items[yCoord].Text;
			else
				returnValue = lv.Items[yCoord].SubItems[xCoord].Text;

			return returnValue;
		}

		public void UnselectAllCells()
		{
			SetMultipleCellsUnselected(cellTracker.FindSelectedCells());

			prevSelectedColumn = 0;
			prevSelectedRow = 0;
		}

		public void SelectAllCells()
		{
			SetMultipleCellsSelected(cellTracker.GetCellList());
		}

		public bool IsCellSelected(int row, int column)
		{
			return cellTracker.IsCellSelected(row - 1, column - 1);
		}

		public bool IsCellUnselected(int row, int column)
		{
			return !cellTracker.IsCellSelected(row - 1, column - 1);
		}

		public void SetCellSelected(int yCoord, int xCoord = 0)
		{
			lv.BeginUpdate();
			{
				lv.Items[yCoord].UseItemStyleForSubItems = false;

				if (xCoord == 0) {
					lv.Items[yCoord].BackColor = Color.MediumBlue;
					lv.Items[yCoord].ForeColor = Color.White;
				}
				else {
					lv.Items[yCoord].SubItems[xCoord].BackColor = Color.FromArgb(0, 120, 215);
					lv.Items[yCoord].SubItems[xCoord].ForeColor = Color.White;
				}
			}
			lv.EndUpdate();

			cellTracker.SetCellSelected(yCoord, xCoord);
		}

		private void SetCellUnselected(int yCoord, int xCoord = 0)
		{
			lv.BeginUpdate();
			{
				lv.Items[yCoord].UseItemStyleForSubItems = false;

				if (xCoord == 0) {
					lv.Items[yCoord].BackColor = Color.White;
					lv.Items[yCoord].ForeColor = Color.Black;
				}
				else {
					lv.Items[yCoord].SubItems[xCoord].BackColor = Color.White;
					lv.Items[yCoord].SubItems[xCoord].ForeColor = Color.Black;
				}
			}
			lv.EndUpdate();

			cellTracker.SetCellUnselected(yCoord, xCoord);
		}

		private void SetMultipleCellsSelected(List<(int, int)> selectedCells)
		{
			lv.BeginUpdate();
			{
				foreach ((int yCoord, int xCoord) in selectedCells) {
					lv.Items[yCoord].UseItemStyleForSubItems = false;
					if (xCoord == 0) {
						lv.Items[yCoord].BackColor = Color.MediumBlue;
						lv.Items[yCoord].ForeColor = Color.White;
					}
					else {
						lv.Items[yCoord].SubItems[xCoord].BackColor = Color.FromArgb(1, 120, 215);
						lv.Items[yCoord].SubItems[xCoord].ForeColor = Color.White;
					}
				}
			}
			lv.EndUpdate();

			cellTracker.SetMultipleCellsSelected(ref selectedCells);
		}

		private void SetMultipleCellsUnselected(List<(int, int)> unselectedCells)
		{
			lv.BeginUpdate();
			{
				foreach ((int yCoord, int xCoord) in unselectedCells) {
					lv.Items[yCoord].UseItemStyleForSubItems = false;
					if (xCoord == 0) {
						lv.Items[yCoord].BackColor = Color.White;
						lv.Items[yCoord].ForeColor = Color.Black;
					}
					else {
						lv.Items[yCoord].SubItems[xCoord].BackColor = Color.White;
						lv.Items[yCoord].SubItems[xCoord].BackColor = Color.Black;
					}
				}
			}
			lv.EndUpdate();

			cellTracker.SetMultipleCellsUnselected(ref unselectedCells);
		}

		public (int, int) GetCurrentRowSize(int row)
		{
			int length = -1;
			int height = -1;

			int rowIndex = row - 1;
			if (row != -1) {
				length = listViewRowList[rowIndex].Item2.Width;
				height = listViewRowList[rowIndex].Item2.Height;
			}

			return (length, height);
		}

		public (int, int) GetCurrentColumnSize(int column)
		{
			int width = -1;
			int height = -1;

			int columnIndex = column - 1;
			if (column != -1) {
				width = listViewColumnList[columnIndex].Item2.Width;
				height = listViewColumnList[columnIndex].Item2.Height;
			}

			return (width, height);
		}

		public void AutoExportListViewToExcel(FileStream exportStream)
		{
			XLWorkbook exportHandle = new XLWorkbook();
			IXLWorksheet wrkshtExportHandle = exportHandle.Worksheets.Add();

			if (totalRows != 0) {
				for (int row = 1; row < totalRows + 2; row++) {
					for (int column = 1; column < totalColumns + 1; column++) {
						if (row == 1)
							wrkshtExportHandle.Cell(row, column).Value = lv.Columns[column - 1].Text;

						if (column == 1 && row != 1)
							wrkshtExportHandle.Cell(row, column).Value = lv.Items[row - 2].Text;

						if (column != 1 && row != 1)
							wrkshtExportHandle.Cell(row, column).Value = lv.Items[row - 2].SubItems[column - 1].Text;
					}
				}

				wrkshtExportHandle.Workbook.SaveAs((Stream)exportStream);
			}
		}

		public void ManualExportListViewToExcel()
		{
			XLWorkbook exportHandle = new XLWorkbook();
			IXLWorksheet wrkshtExportHandle = exportHandle.Worksheets.Add();

			if (totalRows != 0) {
				for (int row = 1; row < totalRows + 2; row++) {
					for (int column = 1; column < totalColumns + 1; column++) {
						if (row == 1)
							wrkshtExportHandle.Cell(row, column).Value = lv.Columns[column - 1].Text;

						if (column == 1 && row != 1)
							wrkshtExportHandle.Cell(row, column).Value = lv.Items[row - 2].Text;

						if (column != 1 && row != 1)
							wrkshtExportHandle.Cell(row, column).Value = lv.Items[row - 2].SubItems[column - 1].Text;
					}
				}

				FileStream exportOutput;
				SaveFileDialog saveDialog = new SaveFileDialog
				{
					Filter = "All files (*.*)|*.*",
					FilterIndex = 2,
					RestoreDirectory = true
				};

				if (saveDialog.ShowDialog() == DialogResult.OK) {
					exportOutput = (FileStream)saveDialog.OpenFile();
					if (exportOutput != null) {
						wrkshtExportHandle.Workbook.SaveAs((Stream)exportOutput);
						exportOutput.Close();
						exportOutput.Dispose();
					}
				}
			}
			else {
				MessageBox.Show("There are no entries in the task list to export", "Tasker Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}

	public partial class TaskerMainWindow
	{
		private void ListView1_MouseUp(object sender, MouseEventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("List view mouse up event has been triggered");

			(int yRow, int xColumn) = lvOps.GetMouseLocationOnClick(e.Location);

			if (yRow == -1 || xColumn == -1)
				copySelectCell.Enabled = false;
			else
				copySelectCell.Enabled = true;
		}

		private void ListView1_MouseDown(object sender, MouseEventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("List view mouse down event has been triggered");

			if(isToolTipShown) {
				taskerMainWinToolTip.Hide(listView1);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			(int yRow, int xColumn) = lvOps.GetMouseLocationOnClick(e.Location);

			if (yRow != -1 && xColumn != -1) {
				if (IsDebugModeEnabled)
					Warn("Current Cell Selected Was: " + "R - " + yRow.ToString() + ", " + "C - " + xColumn.ToString());

				lvOps.ChangeListViewSelection(yRow, xColumn);

				if (e.Button == MouseButtons.Left)
					lvOps.SetRightClickActive = false;
				else if (e.Button == MouseButtons.Right) {
					if (!lvOps.IsCellSelected(yRow, xColumn))
						lvOps.ChangeListViewSelection(yRow, xColumn);

					lvOps.SetRightClickActive = true;
				}
			}
			else {
				if (IsDebugModeEnabled) {
					Warn("No row was selected");
					Debug("Unselecting any previously selected cells");
				}
				lvOps.UnselectAllCells();
			}
		}

		private void ListView1_MouseHover(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("List View mouse hover event has been triggered");

			taskerMainWinToolTip.Show("This displays previously entered time logs", listView1, toolTipTimerShowInterval);

			isToolTipShown = true;
			toolTipTimer.Enabled = true;
			toolTipTimer.Start();
		}

		private void ListView1_MouseMove(object sender, MouseEventArgs e)
		{
			if (!mouseMoveDebugMsgTriggered) {
				if (IsDebugModeEnabled)
					EventLog("List view mouse move event has been triggered");

				mouseMoveDebugMsgTriggered = true;
			}
			
			if(mouseMoveEnabled) {
				if(isToolTipShown) {
					taskerMainWinToolTip.Hide(listView1);
					isToolTipShown = false;
					mouseMoveEnabled = false;
					toolTipTimer.Stop();
				}
			}
			
			if (IsLVMouseTrackDebugEnabled) {
				//This only applies when we are debugging the listView
				if (lvOps.IsMouseOverLV.ToBool()) {
					(int yRow, int xColumn) = lvOps.GetMouseLocationOnClick(e.Location);

					(int rLength, int rHeight) = lvOps.GetCurrentRowSize(yRow);

					(int cWidth, int cHeight) = lvOps.GetCurrentColumnSize(xColumn);

					if (lvDbgWin != null) {
						lvDbgWin.XCoord = e.Location.X;
						lvDbgWin.YCoord = e.Location.Y;
						lvDbgWin.CurrentRow = yRow;
						lvDbgWin.CurRowLength = rLength;
						lvDbgWin.CurRowHeight = rHeight;
						lvDbgWin.CurrentColumn = xColumn;
						lvDbgWin.CurColumnWidth = cWidth;
						lvDbgWin.CurColumnHeight = cHeight;
					}
				}
			}
		}

		private void ListView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("List view column width changed event has been triggered");

			if (lvOps != null)
				lvOps.UpdateAfterColumnWidthResize(e.ColumnIndex, e.ColumnIndex + 1, listView1.Columns[e.ColumnIndex].Width);

			if (listView1.KeyColumnHeader != null && listView1.TimeColumnHeader != null && listView1.TimeLoggedColumnHeader != null && listView1.DescriptionColumnHeader != null) {
				taskerSettings.KeyColumnWidth = listView1.KeyColumnHeader.Width;
				taskerSettings.TimeColumnWidth = listView1.TimeColumnHeader.Width;
				taskerSettings.TimeLoggedColumnWidth = listView1.TimeLoggedColumnHeader.Width;
				taskerSettings.DescriptionColumnWidth = listView1.DescriptionColumnHeader.Width;
			}
		}

		private void ListView1_OnScroll(object sender, ScrollEventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("List view on scroll event has been triggered");
			
			if (lvOps != null)
				if (IsDebugModeEnabled)
					Debug("The list view has been scrolled");
		}

		private void ListView1_MouseEnter(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("List view mouse enter event has been triggered");

			lvOps.SetMouseOverLV = true;

			mouseMoveDebugMsgTriggered = false;
		}

		private void ListView1_MouseLeave(object sender, EventArgs e)
		{
			if (IsDebugModeEnabled)
				EventLog("List view mouse leave event has been triggered");

			if (isToolTipShown) {
				taskerMainWinToolTip.Hide(listView1);
				isToolTipShown = false;
				mouseMoveEnabled = false;
				toolTipTimer.Stop();
			}

			lvOps.SetMouseOverLV = false;

			if (lvDbgWin != null) {
				lvDbgWin.XCoord = 0;
				lvDbgWin.YCoord = 0;
				lvDbgWin.CurrentRow = 0;
				lvDbgWin.CurRowLength = 0;
				lvDbgWin.CurRowHeight = 0;
				lvDbgWin.CurrentColumn = 0;
				lvDbgWin.CurColumnWidth = 0;
				lvDbgWin.CurColumnHeight = 0;
			}
		}
	}
}