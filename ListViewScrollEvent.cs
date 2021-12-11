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
using System.Windows.Forms;
using System.Runtime.InteropServices;

using static Tasker.TaskerVariables;
using static Tasker.LogCalls;

namespace Tasker
{
	public class ListViewWithScrollEvent : ListView
	{
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

		[DllImport("user32.dll")]
		private static extern int SendMessage(int hWnd, uint Msg, long wParam, long lParam);

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int GetScrollPos(IntPtr hWnd, int nBar);

		[StructLayout(LayoutKind.Sequential)]
		private struct SCROLLINFO
		{
			public uint cbSize;
			public uint fMask;
			public int nMin;
			public int nMax;
			public uint nPage;
			public int nPos;
			public int nTrackPos;
		}

		public event ScrollEventHandler? OnScroll;
		
		//private const int WM_PAINT = 0x000F;
		private const int WM_HSCROLL = 0x0114;
		private const int WM_VSCROLL = 0x0115;
		private const int WM_MOUSEWHEEL = 0x020A;
		//private const int WM_MBUTTONDOWN = 0x0207;
		//private const int WM_MBUTTONUP = 0x0208;
		private const int WM_KEYDOWN = 0x0100;
		//private const int WM_LBUTTONUP = 0x0202;

		private const int SB_HORZ = 0;
		private const int SB_VERT = 1;

		//private const int SIF_TRACKPOS = 0x10;
		//private const int SIF_RANGE = 0x01;
		//private const int SIF_POS = 0x04;
		//private const int SIF_PAGE = 0x02;
		//private const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;

		private const uint LVM_SCROLL = 0x1014;
		private const int LVM_FIRST = 0x1000;
		//private const int LVM_SETGROUPINFO = (LVM_FIRST + 147);

		public enum ScrollBarCommands : ushort
		{
			SB_LINEUP = 0,
			SB_LINELEFT = 1,
			SB_LINEDOWN = 2,
			SB_LINERIGHT = 3,
			SB_PAGEUP = 4,
			SB_PAGEDOWN = 5,
			SB_PAGERIGHT = 6,
			SB_THUMBPOSITION = 7,
			SB_THUMBTRACK = 8,
			SB_TOP = 9,
			SB_LEFT = 10,
			SB_BOTTOM = 11,
			SB_RIGHT = 12,
			SB_ENDSCROLL = 13
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			switch(m.Msg) 
			{
				case WM_VSCROLL:
					OnScroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.EndScroll, GetScrollPos(this.Handle, SB_VERT)));
					break;

				case WM_HSCROLL:
					OnScroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.EndScroll, GetScrollPos(this.Handle, SB_HORZ)));
					break;

				case WM_MOUSEWHEEL:
					OnScroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.EndScroll, GetScrollPos(this.Handle, SB_VERT)));
					break;

				case WM_KEYDOWN:
					switch((Keys)m.WParam.ToInt32()) 
					{
						case Keys.Down:
							OnScroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.SmallDecrement, GetScrollPos(this.Handle, SB_VERT)));
							break;
						case Keys.Up:
							OnScroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.SmallIncrement, GetScrollPos(this.Handle, SB_VERT)));
							break;
						case Keys.Left:
							OnScroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.SmallIncrement, GetScrollPos(this.Handle, SB_HORZ)));
							break;
						case Keys.Right:
							OnScroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.SmallIncrement, GetScrollPos(this.Handle, SB_HORZ)));
							break;
						case Keys.PageDown:
							OnScroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.LargeDecrement, GetScrollPos(this.Handle, SB_VERT)));
							break;
						case Keys.PageUp:
							OnScroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.LargeIncrement, GetScrollPos(this.Handle, SB_VERT)));
							break;
						case Keys.Home:
							OnScroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.First, GetScrollPos(this.Handle, SB_VERT)));
							break;
						case Keys.End:
							OnScroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.Last, GetScrollPos(this.Handle, SB_VERT)));
							break;
					}
					break;
			}
		}

		public int VertScrollPos
		{
			get
			{
				return GetScrollPos(this.Handle, SB_VERT);
			}

			set
			{
				int prevPos = GetScrollPos(this.Handle, SB_VERT);
				int	scrollVal = -(prevPos - value);

				_ = SendMessage(this.Handle, LVM_SCROLL, (IntPtr)0, (IntPtr)scrollVal);
			}
		}

		public int HortScrollPos
		{
			get
			{
				return GetScrollPos(this.Handle, SB_HORZ);
			}

			set
			{
				int prevPos = GetScrollPos(this.Handle, SB_HORZ);
				int scrollVal = -(prevPos - value);

				_ = SendMessage(this.Handle, LVM_SCROLL, (IntPtr)0, (IntPtr)scrollVal);
			}
		}

		private ColumnHeader? keyColumnHeader = null;
		private ColumnHeader? timeColumnHeader = null;
		private ColumnHeader? timeLoggedColumnHeader = null;
		private ColumnHeader? descriptionColumnHeader = null;

		public ColumnHeader? KeyColumnHeader
		{
			get
			{
				return keyColumnHeader;
			}
		}

		public ColumnHeader? TimeColumnHeader
		{
			get
			{
				return timeColumnHeader;
			}
		}

		public ColumnHeader? TimeLoggedColumnHeader
		{
			get
			{
				return timeLoggedColumnHeader;
			}
		}

		public ColumnHeader? DescriptionColumnHeader
		{
			get
			{
				return descriptionColumnHeader;
			}
		}

		public void SetupListViewColumnHeaders()
		{
			keyColumnHeader = this.Columns[this.Columns.IndexOfKey("keyColumn")];
			timeColumnHeader = this.Columns[this.Columns.IndexOfKey("timeColumn")];
			timeLoggedColumnHeader = this.Columns[this.Columns.IndexOfKey("timeLoggedColumn")];
			descriptionColumnHeader = this.Columns[this.Columns.IndexOfKey("descriptionColumn")];
		}
	}
}