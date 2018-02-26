// Thumbnail.cs

namespace Twin.Forms
{
	using System;
	using System.Runtime.Serialization;
	using System.Drawing;
	using System.ComponentModel;

	/// <summary>
	/// �T���l�C������\��
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class Thumbnail : ISerializable
	{
		public int width;
		public int height;
		public bool visible;

		/// <summary>
		/// �T���l�C���̉������擾�܂��͐ݒ�
		/// </summary>
		public int Width
		{
			set
			{
				width = value;
			}
			get
			{
				return width;
			}
		}

		/// <summary>
		/// �T���l�C���̏c�����擾�܂��͐ݒ�
		/// </summary>
		public int Height
		{
			set
			{
				height = value;
			}
			get
			{
				return height;
			}
		}

		/// <summary>
		/// �T���l�C����\�����邩�ǂ������擾�܂��͐ݒ�
		/// </summary>
		public bool Visible
		{
			set
			{
				visible = value;
			}
			get
			{
				return visible;
			}
		}

		public bool lightMode;
		/// <summary>
		/// ���삪�y���A��������ߖ�ł���T���l�C�����[�h���ǂ����ł��B
		/// </summary>
		public bool IsLightMode
		{
			get
			{
				return lightMode;
			}
			set
			{
				lightMode = value;
			}
		}


		public Size Size
		{
			get
			{
				return new Size(width, height);
			}
		}

		public Thumbnail()
		{
		}

		/// <summary>
		/// Thumbnail�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Thumbnail(int width, int height)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.width = width;
			this.height = height;
			this.lightMode = true;
			this.visible = false;

		}

		public Thumbnail(SerializationInfo info, StreamingContext context)
		{
			width = info.GetInt32("Width");
			height = info.GetInt32("Height");
			visible = info.GetBoolean("Visible");

			try
			{
				lightMode = info.GetBoolean("IsLightMode");
			}
			catch (SerializationException)
			{
				lightMode = true;
			}
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Width", width);
			info.AddValue("Height", height);
			info.AddValue("Visible", visible);
			info.AddValue("IsLightMode", lightMode);
		}
	}
}
