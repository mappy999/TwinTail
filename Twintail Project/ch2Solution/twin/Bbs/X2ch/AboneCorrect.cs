using System;
using System.Collections.Generic;
using System.Text;
using Twin.IO;

namespace Twin.Bbs
{
	public static class AboneCorrect
	{
		/// <summary>
		/// ���ځ[�񂳂ꂽ���X�������o���A�����擾�\�ȃo�C�g�I�t�Z�b�g�������āA�V�����I�t�Z�b�g�ɒ�������
		/// </summary>
		/// <param name="oldItems"></param>
		/// <param name="headerInfo"></param>
		/// <returns></returns>
		public static int Test(ResSetCollection oldItems, ThreadHeader headerInfo)
		{
			ThreadHeader h = new X2chThreadHeader(headerInfo as X2chThreadHeader);
			X2chThreadReader reader = new X2chThreadReader();

			try
			{
				if (reader.Open(h))
				{
					ResSetCollection newItems = new ResSetCollection();

					while (reader.Read(newItems) != 0)
						;

					int correctlyOffset = AboneDetecting(oldItems, newItems);

					// �������ꂽ�V�����I�t�Z�b�g
					return correctlyOffset;
				}

				return -1;
			}
			finally
			{
				reader.Close();
			}
		}

		private static int AboneDetecting(ResSetCollection oldItems, ResSetCollection newItems)
		{
			ResSetCollection temp = new ResSetCollection();

			for (int i = 0; i < oldItems.Count; i++)
			{
				ResSet old = oldItems[i];

				ResSet _new = newItems[i];
				temp.Add(_new);


				if (IsAboned(_new, old))
				{
					old.IsServerAboned = true;
					oldItems[i] = old;
				}
			}

			X2chThreadFormatter formatter = new X2chThreadFormatter();

			string datStr = formatter.Format(temp);

			byte[] rawBytes = Encoding.GetEncoding("shift_jis").GetBytes(datStr);

			int newByteOffset = rawBytes.Length;

			// ���ꂪ�������o�C�g�I�t�Z�b�g�ɂȂ饥��͂��B
			return newByteOffset;
		}

		private static bool IsAboned(ResSet resNew, ResSet resOld)
		{
			string[] aboneStr = { "���ځ[��", "���Ӂ`��" };

			foreach (string s in aboneStr)
			{
				if (resNew.DateString.Trim() == s && resOld.DateString != s)
				{
					return true;
				}
			}
			return false;
		}
	}
}
