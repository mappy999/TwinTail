// CSharpToolBarButtonEvent.cs

namespace CSharpSamples
{
	using System;

	/// <summary>
	/// CSharpToolBar.ButtonClick�C�x���g����������f���Q�[�g
	/// </summary>
	public delegate void CSharpToolBarButtonEventHandler(
			object sender, CSharpToolBarButtonEventArgs e);

	/// <summary>
	/// CSharpToolBar.ButtonClick�C�x���g�̃f�[�^���
	/// </summary>
	public class CSharpToolBarButtonEventArgs : EventArgs
	{
		private readonly CSharpToolBarButton button;

		/// <summary>
		/// �N���b�N���ꂽ�{�^�����擾
		/// </summary>
		public CSharpToolBarButton Button {
			get {
				return button;
			}
		}

		/// <summary>
		/// CSharpToolBarButtonEventArgs�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="button">�N���b�N���ꂽ�{�^��</param>
		public CSharpToolBarButtonEventArgs(CSharpToolBarButton button)
		{
			if (button == null) {
				throw new ArgumentNullException("button");
			}
			this.button = button;
		}
	}
}
