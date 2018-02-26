// CheckOnlyPatroller.cs

namespace Twin.Tools
{
	using System;
	using System.Net;
	using System.Collections;
	using System.Collections.Generic;
	using Twin.IO;

	/// <summary>
	/// subject.txt�̔�r�ɂ��X�V�`�F�b�N�݂̂��s���N���X
	/// </summary>
	public class CheckOnlyPatroller : PatrolBase
	{
		/// <summary>
		/// CheckOnlyPatroller�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="cache"></param>
		public CheckOnlyPatroller(Cache cache) : base(cache)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		/// <summary>
		/// ������J�n (�X�V�`�F�b�N�̂�)
		/// </summary>
		public override void Patrol()
		{
			// �X�V�Ώۂ̔����ׂăR���N�V�����ɋl�߂�
			Hashtable boardList = new Hashtable();

			foreach (ThreadHeader item1 in Items)
			{
				BoardInfo board = item1.BoardInfo;
				OnPatroling(new PatrolEventArgs(item1));

				// �X���b�h�ꗗ����M
				if (!boardList.Contains(board.Url))
				{
					OnStatusTextChanged(board.Url + "subject.txt ���擾��...");

					List<ThreadHeader> headers = new List<ThreadHeader>();
					ThreadListReader listReader = TypeCreator.CreateThreadListReader(board.Bbs);
					
					listReader.ServerChange += 
						new EventHandler<ServerChangeEventArgs>(delegate (object sender, ServerChangeEventArgs e)
					{
						item1.BoardInfo.Server = board.Server = e.NewBoard.Server;
					});

					try {
						if (listReader.Open(board))
							while (listReader.Read(headers) != 0);
					}
					catch {}
					finally {
						if (listReader != null)
							listReader.Close();
					}

					boardList[board.Url] = headers;
				}

				// ���X���������Ă���΍X�V����Ă���A���݂��Ȃ����dat����
				List<ThreadHeader> targetList = (List<ThreadHeader>)boardList[board.Url];
				int index = targetList.IndexOf(item1);

				if (index == -1 && targetList.Count > 0)
				{
					item1.Pastlog = true;
					OnUpdated(new PatrolEventArgs(item1));
				}
				else if (index >= 0 && item1.GotResCount < targetList[index].ResCount)
				{
					item1.ResCount = targetList[index].ResCount;
					OnUpdated(new PatrolEventArgs(item1));
				}
			}
		}
	}
}
