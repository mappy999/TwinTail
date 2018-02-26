// DirectoryPicker.cs

namespace CSharpSamples
{
	using System;
	using System.Windows.Forms.Design;
	using System.Windows.Forms;

	// ----------------------------------------------------------------
	// StartLocation �́ADesktop, Favorites, MyComputer, MyDocuments,
	//   MyPictures, NetAndDialUpConnections, NetworkNeighborhood,
	//   Printers, Recent, SendTo, StartMenu, Templates�@���I������B
	//   
	// Styles �́ABrowseForComputer, BrowseForEverything, BrowseForPrinter,
	//   RestrictToDomain, RestrictToFilesystem, RestrictToSubfolders,
	//   ShowTextBox �@���I������B
	// ----------------------------------------------------------------

	/// <summary>
	/// �f�B���N�g���I���_�C�A���O
	/// </summary>
	[Obsolete("System.Windows.Forms.FolderBrowserDialog �N���X���g�p���Ă��������B")]
	public class DirectoryPicker : FolderNameEditor
	{
		private FolderNameEditor.FolderBrowser _folderBrowser;
	
		/// <summary>
		/// �I�����ꂽ�f�B���N�g���p�X���擾
		/// </summary>
		public string DirectoryPath {
			get {
				return _folderBrowser.DirectoryPath;
			}
		}

		/// <summary>
		/// �\�������e�L�X�g���擾�܂��͐ݒ�
		/// </summary>
		public string Text {
			set {
				_folderBrowser.Description = value;
			}
			get {
				return _folderBrowser.Description;
			}
		}

		/// <summary>
		/// DirectoryPicker�N���X�̃C���X�^���X��������
		/// </summary>
		public DirectoryPicker()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			_folderBrowser = new FolderNameEditor.FolderBrowser();
			_folderBrowser.StartLocation = FolderNameEditor.FolderBrowserFolder.Desktop;
			_folderBrowser.Style = FolderNameEditor.FolderBrowserStyles.ShowTextBox;
		}

		~DirectoryPicker()
		{
			_folderBrowser.Dispose();
		}

		/// <summary>
		/// �f�B���N�g���I���_�C�A���O��\��
		/// </summary>
		/// <returns>�����ꂽ�{�^��</returns>
		public DialogResult ShowDialog()
		{
			return _folderBrowser.ShowDialog();
		}
	}
}
