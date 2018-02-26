// LinkCollection.cs

namespace Twin
{
	using System;
	using System.Collections.Specialized;
	using System.Collections;
	using System.IO;

	/// <summary>
	/// �����N��������R���N�V�����Ǘ�
	/// </summary>
	public class LinkCollection : StringCollection
	{
		/// <summary>
		/// �w�肵���g���q�̃����N���擾 (OR���Z�q�ŕ����w��\)
		/// ��: LinkCollection[".jpg|.gif"]
		/// </summary>
		public string[] this[string extension]
		{
			get
			{
				ArrayList matches = new ArrayList();
				string[] array = extension.Split('|');

				if (array.Length > 0 && Count > 0)
				{
					// �����g���q���������N������
					foreach (string link in this)
					{
						foreach (string ext in array)
						{
							if (link.ToLower().EndsWith(ext.ToLower()))
								matches.Add(link);
						}
					}
				}
				return (string[])matches.ToArray(typeof(string));
			}
		}

		/// <summary>
		/// LinkCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public LinkCollection()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
