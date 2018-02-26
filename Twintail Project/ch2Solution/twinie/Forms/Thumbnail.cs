// Thumbnail.cs

namespace Twin.Forms
{
	using System;
	using System.Runtime.Serialization;
	using System.Drawing;
	using System.ComponentModel;

	/// <summary>
	/// サムネイル情報を表す
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(TwinExpandableConverter))]
	public class Thumbnail : ISerializable
	{
		public int width;
		public int height;
		public bool visible;

		/// <summary>
		/// サムネイルの横幅を取得または設定
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
		/// サムネイルの縦幅を取得または設定
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
		/// サムネイルを表示するかどうかを取得または設定
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
		/// 動作が軽く、メモリを節約できるサムネイルモードかどうかです。
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
		/// Thumbnailクラスのインスタンスを初期化
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Thumbnail(int width, int height)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
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
