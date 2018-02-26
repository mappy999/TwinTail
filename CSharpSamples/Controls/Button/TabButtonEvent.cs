// TabButtonEvent.cs

using System;

namespace CSharpSamples
{
	/// <summary>
	/// TabButtonControl.SelectedChanged �C�x���g���������郁�\�b�h�ł��B
	/// </summary>
	public delegate void TabButtonEventHandler(object sender,
		TabButtonEventArgs e);

	/// <summary>
	/// TabButtonEventHandler ���\�b�h�̈�����\���܂��B
	/// </summary>
	public class TabButtonEventArgs : EventArgs
	{
		/// <summary>
		/// �V�����I�����ꂽ TabButton ���擾���܂��B
		/// </summary>
		public TabButton Button {
			get {
				return button;
			}
		}
		private TabButton button;

		/// <summary>
		/// TabButtonEventEventArgs �N���X�̃C���X�^���X���������B
		/// </summary>	
		public TabButtonEventArgs(TabButton button)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.button = button;
		}
	}
}
