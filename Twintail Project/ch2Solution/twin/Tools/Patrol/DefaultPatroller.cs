// DefaultPatroller.cs

namespace Twin.Tools
{
	using System;
	using System.IO;
	using System.Text;
	using System.Net;
	using System.Collections;
	using Twin.IO;
	using Twin.Bbs;
	using CSharpSamples;

	/// <summary>
	/// �f�t�H���g�̏���N���X
	/// </summary>
	public class DefaultPatroller : PatrolBase
	{
		/// <summary>
		/// DefaultPatroller�N���X�̃C���X�^���X��������
		/// </summary>
		public DefaultPatroller(Cache cacheInfo) : base(cacheInfo)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// ����J�n
		/// </summary>
		public override void Patrol()
		{
			try {
				ThreadReaderRelay reader = null;
				BbsType  bbs = BbsType.None;

				foreach (ThreadHeader header in Items)
				{
					if (header.Pastlog || header.IsLimitOverThread)
						continue;

					ResSetCollection temp = new ResSetCollection();
					BoardInfo board = header.BoardInfo;

					// ���[�_�[���쐬
					if (bbs != board.Bbs)
					{
						reader = null;
						reader = new ThreadReaderRelay(Cache, TypeCreator.CreateThreadReader(board.Bbs));
						bbs = board.Bbs;
					}

					// �擾�O�̐V����ۑ�
					int newResCount = header.NewResCount;

					try {
						OnStatusTextChanged(header.Subject + " �̏���...");

						ClientBase.Connect.WaitOne();

						PatrolEventArgs e = new PatrolEventArgs(header);
						OnPatroling(e);

						if (!e.Cancel)
						{
							// �V���݂̂�ǂݎ��
							reader.ReadCache = false;

							if (reader.Open(header))
								while (reader.Read(temp) != 0);
						}
					}
					catch {}
					finally {
						if (reader != null)
							reader.Close();
						ClientBase.Connect.Set();
					}

					if (header.NewResCount > 0)
					{
						// �O��̐V���𑫂��āA�C���f�b�N�X��ۑ�
						header.NewResCount += newResCount;
						OnUpdated(new PatrolEventArgs(header));
					}
				}
			}
			finally {
				OnStatusTextChanged("������������܂���");
			}
		}
	}
}
