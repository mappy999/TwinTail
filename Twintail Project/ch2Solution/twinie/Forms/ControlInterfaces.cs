// ITwinControl.cs

namespace Twin.Forms
{
	using System;
	using Twin.Bbs;
	using System.Drawing;

	/// <summary>
	/// Tab�R���g���[���𑀍삷��C���^�[�t�F�[�X
	/// </summary>
	public interface ITwinTabController<THeader, TControl>
		where TControl : ClientBase//Ex<THeader>
	{
		/// <summary>
		/// �V�����R���g���[�����쐬���r���[�A���擾
		/// </summary>
		TControl Create(THeader header, bool newWindow);

		/// <summary>
		/// �w�肵���X���b�h�������R���g���[��������
		/// </summary>
		TControl FindControl(THeader header);

		/// <summary>
		/// �\������Ă��邷�ׂẴR���g���[����z��Ŏ擾
		/// </summary>
		TControl[] GetControls();

		/// <summary>
		/// �E�C���h�E�̏����i�[���Ă���N���X������
		/// </summary>
		TwinWindow<THeader, TControl> FindWindow(THeader header);

		/// <summary>
		/// �E�C���h�E�̏����i�[���Ă��邷�ׂẴN���X��z��Ŏ擾
		/// </summary>
		TwinWindow<THeader, TControl>[] GetWindows();

		/// <summary>
		/// �w�肵���E�C���h�E�����
		/// </summary>
		/// <param name="window"></param>
		void Destroy(TControl window);

		/// <summary>
		/// ���ׂẴE�C���h�E�����
		/// </summary>
		void Clear();

		/// <summary>
		/// ���̃E�C���h�E��I��
		/// </summary>
		/// <param name="next"></param>
		void Select(bool next);

		/// <summary>
		/// �w�肵���R���g���[���ɕ������ݒ�
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="text"></param>
		void SetText(TControl ctrl, string text);

		/// <summary>
		/// �I������Ă���E�C���h�E���擾
		/// </summary>
		TControl Control
		{
			get;
		}

		/// <summary>
		/// �I������Ă���E�C���h�E�̃X���b�h�����擾
		/// </summary>
		THeader HeaderInfo
		{
			get;
		}

		/// <summary>
		/// �I������Ă���E�C���h�E�̃I�u�W�F�N�g���擾
		/// </summary>
		TwinWindow<THeader, TControl> Window
		{
			get;
		}

		/// <summary>
		/// �X���b�h���I������Ă��邩�ǂ����𔻒f
		/// </summary>
		bool IsSelected
		{
			get;
		}

		/// <summary>
		/// �\������Ă���E�C���h�E�̐����擾
		/// </summary>
		int WindowCount
		{
			get;
		}

		/// <summary>
		/// �\������Ă���E�C���h�E�̃C���f�b�N�X���擾���܂��B
		/// </summary>
		int Index
		{
			get;
		}
	}

	/// <summary>
	/// �ꗗ�e�[�u���r���[�̃C���^�[�t�F�[�X
	/// </summary>
	public interface ITwinTableControl
	{
		/// <summary>
		/// ���Ɏw�肵����������܂ޔ�����
		/// </summary>
		BoardInfo[] Find(string text);

		/// <summary>
		/// �I������Ă�����擾�܂��͐ݒ�
		/// </summary>
		BoardInfo Selected
		{
			set;
			get;
		}

		/// <summary>
		/// �I������Ă��邩�ǂ����𔻒f
		/// </summary>
		bool IsSelected
		{
			get;
		}
	}

	/// <summary>
	/// �E�C���h�E��\��
	/// </summary>
	public class TwinWindow<THeader, TControl>
		where TControl : ClientBase//Ex<THeader>
	{
		private TControl ctrl;
		private bool visibled;
		private object tag;

		public TControl Control
		{
			get
			{
				return ctrl;
			}
		}
	
		private TabColorSet colorSet = new TabColorSet();
		/// <summary>
		/// �E�C���h�E�̔z�F�����擾�܂��͐ݒ肵�܂��B
		/// ���̃v���p�e�B��get/set��p�ŁA�����v���p�e�B��Reset���\�b�h���Ăяo���Ă͂����Ȃ��B
		/// </summary>
		public TabColorSet ColorSet
		{
			get
			{
				return colorSet;
			}
			set
			{
				colorSet = value;
			}
		}

		/// <summary>
		/// �����̃��X���Q�Ƃ��ꂽ���ɂ� true �ɂȂ�A�^�u�̃A�C�R����ύX����B��x�ł��E�C���h�E���A�N�e�B�u�ɂȂ�����A�^�u���N���b�N������ false�B
		/// </summary>
		public bool Referenced { get; set; }

		/// <summary>
		/// �����ŉ摜URL���J�����ǂ����B�{���͂��̃N���X�ɒǉ�����ׂ��ł͂Ȃ����낤���ǁc
		/// </summary>
		public bool AutoImageOpen { get; set; }

		public bool Visibled
		{
			set
			{
				if (visibled != value)
					visibled = value;
			}
			get
			{
				return visibled;
			}
		}

		public object Tag
		{
			set
			{
				tag = value;
			}
			get
			{
				return tag;
			}
		}

		public TwinWindow(TControl viewer)
		{
			this.ctrl = viewer;
			this.visibled = false;
			this.tag = null;
		}
	}

	
}