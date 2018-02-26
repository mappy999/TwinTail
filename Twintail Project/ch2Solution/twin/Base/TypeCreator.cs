// TypeCreator.cs

namespace Twin
{
	using System;
	using System.Collections;
	using Twin.IO;

	/// <summary>
	/// �o�^����Ă���^�����ɏ���������N���X
	/// </summary>
	public sealed class TypeCreator
	{
		private static Hashtable typeTable = new Hashtable();

		private class BbsClassTypes
		{
			public Type ThreadHeader;
			public Type ThreadReader;
			public Type ThreadListReader;
			public Type PostBase;
		}

		/// <summary>
		/// bbs�ɑΉ�����e�N���X��o�^
		/// </summary>
		/// <param name="bbs">�o�^����f����</param>
		/// <param name="headerType">bbs�̂ɑΉ����Ă���w�b�_�N���X���w��</param>
		/// <param name="readerType">bbs�̃X���b�h�ǂݍ��݂ɑΉ����Ă��郊�[�_�[���w��</param>
		/// <param name="listReaderType">bbs�̃X���b�h�ꗗ�ǂݍ��݂ɑΉ����Ă��郊�[�_�[���w��</param>
		/// <param name="postType">bbs�̓��e�ɑΉ����Ă���N���X���w��</param>
		public static void Regist(BbsType bbs, 
			Type headerType, Type readerType, Type listReaderType, Type postType)
		{
			BbsClassTypes obj = new BbsClassTypes();
			obj.PostBase = postType;
			obj.ThreadHeader = headerType;
			obj.ThreadReader = readerType;
			obj.ThreadListReader = listReaderType;

			typeTable[bbs] = obj;
		}

		/// <summary></summary>
		/// <param name="bbs"></param>
		/// <returns></returns>
		private static BbsClassTypes CreateInternal(BbsType bbs)
		{
			if (typeTable.Contains(bbs))
			{
				return  (BbsClassTypes)typeTable[bbs];
			}
			throw new NotSupportedException(bbs.ToString());
		}

		/// <summary>
		/// bbs�ɑΉ������w�b�_�N���X���쐬
		/// </summary>
		/// <param name="bbs"></param>
		/// <returns></returns>
		public static ThreadHeader CreateThreadHeader(BbsType bbs)
		{
			BbsClassTypes obj = CreateInternal(bbs);
			return (ThreadHeader)Activator.CreateInstance(obj.ThreadHeader);
		}

		/// <summary>
		/// bbs�ɑΉ������X���b�h���[�_�[���쐬
		/// </summary>
		/// <param name="bbs"></param>
		/// <returns></returns>
		public static ThreadReader CreateThreadReader(BbsType bbs)
		{
			BbsClassTypes obj = CreateInternal(bbs);
			return (ThreadReader)Activator.CreateInstance(obj.ThreadReader);
		}

		/// <summary>
		/// bbs�ɑΉ������X���b�h�ꗗ���[�_�[���쐬
		/// </summary>
		/// <param name="bbs"></param>
		/// <returns></returns>
		public static ThreadListReader CreateThreadListReader(BbsType bbs)
		{
			BbsClassTypes obj = CreateInternal(bbs);
			return (ThreadListReader)Activator.CreateInstance(obj.ThreadListReader);
		}

		/// <summary>
		/// bbs�ɑΉ��������e�N���X���쐬
		/// </summary>
		/// <param name="bbs"></param>
		/// <returns></returns>
		public static PostBase CreatePost(BbsType bbs)
		{
			BbsClassTypes obj = CreateInternal(bbs);
			return (PostBase)Activator.CreateInstance(obj.PostBase);
		}
	}
}
