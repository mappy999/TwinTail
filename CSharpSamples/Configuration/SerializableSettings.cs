// SerializableSettings.cs

namespace CSharpSamples
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// �V���A���C�Y�\�Ȑݒ�̊�{�N���X
	/// </summary>
	[Serializable]
	public abstract class SerializableSettings : ISerializable
	{
		protected SerializableSettings()
		{
		}

		protected SerializableSettings(SerializationInfo info, StreamingContext context)
		{
			Serializer.Deserialize(this, info);
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Serializer.Serialize(this, info);
		}
	}
}
