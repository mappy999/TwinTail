// BoardInfoCollection.cs
// #2.0

namespace Twin
{
	using System;
	using System.Text;
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using System.ComponentModel;

	/// <summary>
	/// BoardInfo �N���X���R���N�V�����Ǘ����܂��B
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(BoardInfoCollectionConverter))]
	public class BoardInfoCollection : List<BoardInfo>, ISerializable
	{
		/// <summary>
		/// BoardInfoCollection�N���X�̃C���X�^���X��������
		/// </summary>
		public BoardInfoCollection() 
		{
		}

		public BoardInfoCollection(SerializationInfo info, StreamingContext context)
		{
			BoardInfoCollection items =
				(BoardInfoCollection)info.GetValue("BoardInfoCollection", typeof(BoardInfoCollection));

			AddRange(items);
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("BoardInfoCollection", this);
		}

		/// <summary>
		/// ���� boardName �ƈ�v�����̃R���N�V�������C���f�b�N�X���擾���܂��B
		/// </summary>
		/// <param name="boardName">���������</param>
		/// <returns>���݂���΂��̃C���f�b�N�X��Ԃ��A������Ȃ���� -1 ��Ԃ��܂��B</returns>
		public int IndexOfName(string boardName)
		{
			return FindIndex(new Predicate<BoardInfo>(delegate (BoardInfo bi)
			{
				return bi.Name.Equals(boardName);
			}));
		}

		/// <summary>
		/// �w�肵�� url �Ɉ�v����̃R���N�V�������C���f�b�N�X���擾���܂��B
		/// </summary>
		/// <param name="url">��������URL</param>
		/// <returns>���݂���΂��̃C���f�b�N�X��Ԃ��A������Ȃ���� -1 ��Ԃ��܂��B</returns>
		public int IndexOfUrl(string url)
		{
			return FindIndex(new Predicate<BoardInfo>(delegate(BoardInfo bi)
			{
				return bi.Url.Equals(url);
			}));
		}

		/// <summary>
		/// �i�[����Ă������A������������ɕϊ����܂��B
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(16 * Count);
			for (int i = 0; i < Count; i++)
			{
				sb.Append(this[i].Name);
				if (i+1 < Count) sb.Append("<>");
			}
			return sb.ToString();
		}
	}
}
