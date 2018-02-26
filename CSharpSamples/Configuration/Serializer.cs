// Serializer.cs

namespace CSharpSamples
{
	using System;
	using System.Reflection;
	using System.Runtime.Serialization;

	/// <summary>
	/// Serializer
	/// </summary>
	public class Serializer
	{
		/// <summary>
		/// obj�̃t�B�[���h���V���A���C�Y
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="info"></param>
		/// <param name="flags"></param>
		public static void Serialize(object obj, SerializationInfo info, BindingFlags flags)
		{
			FieldInfo[] fields = obj.GetType().GetFields(flags);
			foreach (FieldInfo field in fields)
			{
				object val = field.GetValue(obj);
				info.AddValue(field.Name, val);
			}
		}

		/// <summary>
		/// obj�̃t�B�[���h���V���A���C�Y
		/// �f�t�H���g�ł̓p�u���b�N���C���X�^���X�ȃ����o�̂݌����B
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="info"></param>
		public static void Serialize(object obj, SerializationInfo info)
		{
			Serialize(obj, info, BindingFlags.Public | BindingFlags.Instance);
		}

		/// <summary>
		/// info���g�p���ċt�V���A���C�Y��obj�ɒl��ݒ�
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="info"></param>
		/// <param name="flags"></param>
		public static void Deserialize(object obj, SerializationInfo info, BindingFlags flags)
		{
			FieldInfo[] fields = obj.GetType().GetFields(flags);
			foreach (FieldInfo field in fields)
			{
				try {
					object data = info.GetValue(field.Name, field.FieldType);
					field.SetValue(obj, data);
				}
				catch (SerializationException) {}
			}
		}

		/// <summary>
		/// info���g�p���ċt�V���A���C�Y��obj�ɒl��ݒ�B
		/// �f�t�H���g�ł̓p�u���b�N���C���X�^���X�ȃ����o�̂݌����B
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="info"></param>
		public static void Deserialize(object obj, SerializationInfo info)
		{
			Deserialize(obj, info, BindingFlags.Public | BindingFlags.Instance);
		}
	}
}
