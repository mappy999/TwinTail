// TwinListView.cs

namespace Twin.Forms
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;

	using ListViewSubItem =
		System.Windows.Forms.ListViewItem.ListViewSubItem;

	public class TwinListView : ListView
	{
		public TwinListView()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			DoubleBuffered = true;
			ShowItemToolTips = true;
		}
	}
}
