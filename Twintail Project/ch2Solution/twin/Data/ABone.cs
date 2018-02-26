// ABone.cs

namespace Twin
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// ���ځ[�����\��
	/// </summary>
	[Serializable]
	[System.ComponentModel.TypeConverter(typeof(TwinExpandableConverter))]
	public class ABone : ISerializable
	{
		public bool Visible;
		public bool Chain;

		/// <summary>
		/// ABone�N���X�̃C���X�^���X��������
		/// </summary>
		public ABone()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.Visible = true;
			this.Chain = false;
		}

		/// <summary>
		/// ABone�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="visible"></param>
		/// <param name="chain"></param>
		public ABone(bool visible, bool chain) : this()
		{
			this.Visible = visible;
			this.Chain = chain;
		}

		public ABone(SerializationInfo info, StreamingContext context)
		{
			try{
			Visible = info.GetBoolean("Visible");
			Chain = info.GetBoolean("Chain");}catch{}
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			try{
			info.AddValue("Visible", Visible);
			info.AddValue("Chain", Chain);}catch{}
		}
	}
}
