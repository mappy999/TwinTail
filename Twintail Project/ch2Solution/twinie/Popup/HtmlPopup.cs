// HtmlPopup.cs

namespace Twin
{
	using System;
	using System.Drawing;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using mshtml;
	using Twin;
	using Twin.Forms;

	/// <summary>
	/// HtmlPopup �̊T�v�̐����ł��B
	/// </summary>
	public class HtmlPopup : IPopupBase
	{
		private IEComThreadBrowser webBrowser;
		private ThreadSkinBase skin;
		// NTwin23
//		private Point adjust = new Point(10, 10);
		private Point adjust = new Point( 8 , 8 );
		private Point adjustImg = new Point( 8 , 0 );
		// NTwin23

		private PopupPosition position;
		private Size maximum;
		private Font font;
		private string foreColorHtml;
		private string backColorHtml;
		private string fontHtml;
		private int width, height;
		private Size imageSize;

		private int childCount;
		internal bool inPopup = false;

		public int Count
		{
			get
			{
				return childCount;
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v������W���擾
		/// </summary>
		protected Point PopupLocation
		{
			get
			{
				HTMLBody body = webBrowser.GetHtmlBody();
				Point pt = webBrowser.PointToClient(Control.MousePosition);
				pt.Y += body.scrollTop;
				pt.X += body.scrollLeft;

				switch (position)
				{
					case PopupPosition.TopLeft:
						return new Point(pt.X - Width + adjust.X, pt.Y - Height + adjust.Y);

					case PopupPosition.TopRight:
						return new Point(pt.X - adjust.X, pt.Y - Height + adjust.Y);

					case PopupPosition.BottomLeft:
						return new Point(pt.X - Width + adjust.X, pt.Y - adjust.Y);

					case PopupPosition.BottomRight:
						return new Point(pt.X - adjust.X, pt.Y - adjust.Y);
				}
				throw new ApplicationException("PopupPosition���s���ł�");
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v����ʒu���擾�܂��͐ݒ�
		/// </summary>
		public PopupPosition Position
		{
			set
			{
				if (position != value)
					position = value;
			}
			get
			{
				return position;
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v�̍ő�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public Size Maximum
		{
			set
			{
				maximum = value;
			}
			get
			{
				return maximum;
			}
		}

		/// <summary>
		/// ��ɕ\�����邩�ǂ����������l���擾
		/// </summary>
		public bool ShowAlways
		{
			set
			{
				throw new NotSupportedException("ShowAlways�v���p�e�B�̓T�|�[�g���Ă��܂���");
			}
			get
			{
				return false;
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v���镶���̃t�H���g���擾�܂��͐ݒ�
		/// </summary>
		public Font Font
		{
			set
			{
				font = value;
				fontHtml = String.Format("font-family:{0}; font-size:{1}pt;",
						value.Name, value.Size);
			}
			get
			{
				return font;
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v���镶���F���擾�܂��͐ݒ�
		/// </summary>
		public Color ForeColor
		{
			set
			{
				foreColorHtml = ColorTranslator.ToHtml(value);
			}
			get
			{
				return ColorTranslator.FromHtml(foreColorHtml);
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v�̔w�i�F���擾�܂��͐ݒ�
		/// </summary>
		public Color BackColor
		{
			set
			{
				backColorHtml = ColorTranslator.ToHtml(value);
			}
			get
			{
				return ColorTranslator.FromHtml(backColorHtml);
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v�̕����擾
		/// </summary>
		public int Width
		{
			get
			{
				return width;
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v�̍������擾
		/// </summary>
		public int Height
		{
			get
			{
				return height;
			}
		}

		/// <summary>
		/// �摜�|�b�v�A�b�v�̉摜�T�C�Y���擾�܂��͐ݒ�
		/// </summary>
		public Size ImageSize
		{
			set
			{
				imageSize = new Size(
					Math.Max(0, value.Width),
					Math.Max(0, value.Height));
			}
			get
			{
				return imageSize;
			}
		}

		/// <summary>
		/// HtmlPopup�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="doc"></param>
		public HtmlPopup(IEComThreadBrowser wb, ThreadSkinBase skin)
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
			this.webBrowser = wb;
			this.skin = skin;
			this.Font = new Font("�l�r �o�S�V�b�N", 9);

			position = PopupPosition.TopRight;
			maximum = new Size(500, 350);
			imageSize = new Size(200, 0);
			foreColorHtml = "#000000";
			backColorHtml = "#ffffff";
			width = height = 0;
		}

		/// <summary>
		/// �w�肵��html��������|�b�v�A�b�v
		/// </summary>
		/// <param name="htmlText"></param>
//		public void Show(string htmlText)				// NTwin23
		private void Show( string htmlText , bool img )	// NTwin23
		{
			HTMLDocument doc = webBrowser.GetDomDocument();
			HTMLBody body = (HTMLBody)doc.body;
			IHTMLElement root = doc.getElementById("popupBase");

			// �|�b�v�A�b�v�̃��[�g���쐬����Ă��Ȃ����
			if (root == null)
			{
				// �|�b�v�A�b�v�̊�{�ƂȂ�v�f��}��
				body.insertAdjacentHTML("afterBegin", "<dl><div id=\"popupBase\" style=\"" + fontHtml + "\"></div></dl>");
				root = doc.getElementById("popupBase");
			}

			// �|�b�v�A�b�v�����쐬
			root.insertAdjacentHTML("beforeEnd",
				String.Format("<div id=\"p{0}\" style=\"border: solid gray 1px; padding: 3px; background: window; overflow: auto; position: absolute;\"></div>", childCount));

			// �\�����e��ݒ�
			IHTMLElement div = doc.getElementById("p" + childCount);
			div.innerHTML = htmlText;


			// �|�b�v�A�b�v�̍ő啝�����߂�
			int maxWidth = body.clientWidth;
			int maxHeight = body.clientHeight / 2 + body.clientHeight / 3; // ��ʂ�3/2

			// �v�f���ő�c�����傫���Ȃ�Ȃ��悤�ɒ���
			width = Math.Min(div.offsetWidth + 25, maxWidth);
			height = Math.Min(div.offsetHeight, maxHeight);

			// �|�b�v�A�b�v�ʒu�𒲐� (���X�̉E��ɔz�u)
			Point point = PopupLocation;
			// NTwin23
			if ( img )
			{
				point = new Point( point.X + adjustImg.X , point.Y + adjustImg.Y );
			}
			// NTwin23

			// point���N���C�A���g���W�ɕϊ�
			Point clientPos = new Point(
				point.X - body.scrollLeft,
				point.Y - body.scrollTop);

			// ��ƍ��̂͂ݏo���`�F�b�N
			if (clientPos.X < 0)
				point.X = body.scrollLeft;
			if (clientPos.Y < 0)
				point.Y = body.scrollTop;

			// �����N���C�A���g���𒴂��Ă����璲��
			if (clientPos.X + width > body.clientWidth)
			{
				int sub_w = (body.clientWidth - width);
				point.X = (body.scrollLeft + Math.Max(0, sub_w));
			}

			// �������N���C�A���g���𒴂��Ă����璲��
			if (clientPos.Y + height > body.clientHeight)
			{
				int sub_h = (body.clientHeight - height);
				point.Y = (body.scrollTop + Math.Max(0, sub_h));
			}

			// �T�C�Y��ݒ�
			div.style.pixelLeft = point.X;
			div.style.pixelTop = point.Y;
			div.style.pixelWidth = width;
			div.style.pixelHeight = height;

			// �F��ݒ�
			div.style.backgroundColor = backColorHtml;
			div.style.color = foreColorHtml;
			childCount++;
			/*
			HtmlDocument doc = webBrowser.GetDocument();
			HtmlElement body = doc.Body;
			HtmlElement root = doc.GetElementById("popupBase");

			// �|�b�v�A�b�v�̃��[�g���쐬����Ă��Ȃ����
			if (root == null)
			{
				root = doc.CreateElement("DL");

				HtmlElement dummy = doc.CreateElement("DIV");
				dummy.Id = "popupBase";
				dummy.Style = fontHtml;

				root.InsertAdjacentElement(HtmlElementInsertionOrientation.BeforeEnd, dummy);

				// �|�b�v�A�b�v�̊�{�ƂȂ�v�f��}��
				body.InsertAdjacentElement(HtmlElementInsertionOrientation.BeforeEnd, root);
			}

			HtmlElement popupParentHtml = doc.CreateElement("DIV");
			popupParentHtml.Style = "border: solid gray 1px; padding: 3px; background: window; overflow: auto; position: absolute;";
			popupParentHtml.Id = "p" + childCount.ToString();

			// �|�b�v�A�b�v�����쐬
			root.InsertAdjacentElement(HtmlElementInsertionOrientation.BeforeEnd,
				popupParentHtml);

			// �\�����e��ݒ�
			HtmlElement div = doc.GetElementById("p" + childCount);
			div.InnerHtml = htmlText;

			// �|�b�v�A�b�v�̍ő啝�����߂�
			int maxWidth = body.ClientRectangle.Width;
			int maxHeight = body.ClientRectangle.Height / 2 + body.ClientRectangle.Height / 3;

			// �v�f���ő�c�����傫���Ȃ�Ȃ��悤�ɒ���
			width = Math.Min(div.OffsetRectangle.Width + 15, maxWidth);
			height = Math.Min(div.OffsetRectangle.Height, maxHeight);

			// �|�b�v�A�b�v�ʒu�𒲐� (���X�̉E��ɔz�u)
			Point point = PopupLocation;

			// point���N���C�A���g���W�ɕϊ�
			Point clientPos = new Point(
				point.X - body.ScrollLeft,
				point.Y - body.ScrollTop);

			// ��ƍ��̂͂ݏo���`�F�b�N
			if (clientPos.X < 0)
				point.X = body.ScrollLeft;
			if (clientPos.Y < 0)
				point.Y = body.ScrollTop;

			// �����N���C�A���g���𒴂��Ă����璲��
			if (clientPos.X + width > body.ClientRectangle.Width)
			{
				int sub_w = (body.ClientRectangle.Width - width);
				point.X = (body.ScrollLeft + Math.Max(0, sub_w));
			}

			// �������N���C�A���g���𒴂��Ă����璲��
			if (clientPos.Y + height > body.ClientRectangle.Height)
			{
				int sub_h = (body.ClientRectangle.Height - height);
				point.Y = (body.ScrollTop + Math.Max(0, sub_h));
			}

			// �T�C�Y��ݒ�
			div.Style +=
				String.Format("pixelLeft: {0}; pixelTop: {1}; pixelWidth: {2}; pixelHeight: {3}; backgroundColor: {4}; color: {5};",
				point.X, point.Y, width, height, backColorHtml, foreColorHtml);

			childCount++;*/
		}

		// NTwin23
		public void Show( string htmlText )
		{
			Show( htmlText , false );
		}
		// NTwin23

		/// <summary>
		/// �w�肵�����X���X�L��������|�b�v�A�b�v�\��
		/// </summary>
		/// <param name="items"></param>
		public void Show(ResSetCollection items)
		{
			string html = skin.Convert(items);

			if (webBrowser.Thumbnail.Visible)
			{
				html = webBrowser.CreateThumbnailHtml(html, items);
			}

			Show( html , false );	// NTwin23


			webBrowser.FlushCreateThumbnailStack();

			if (Twinie.Settings.Thread.IsColoringBackReference)
			{
				webBrowser.ColoringBackReference(items);
			}
		}

		/// <summary>
		/// �w�肵�����X���X�L��������|�b�v�A�b�v�\��
		/// </summary>
		/// <param name="res"></param>
		public void Show(ResSet res)
		{
			ResSetCollection items = new ResSetCollection();
			items.Add(res);

			Show(items);
		}

		/// <summary>
		/// �w�肵��uri���C���[�W�^�O���g�p���ă|�b�v�A�b�v�\��
		/// </summary>
		/// <param name="uri"></param>
		public void ShowImage(string uri)
		{
			string html = "<img src=\"" + uri + "\"";

			if (imageSize.Width > 0)
				html += " width=\"" + imageSize.Width + "\"";

			if (imageSize.Height > 0)
				html += " height=\"" + imageSize.Height + "\"";

			html += ">";

			Show( html , true );	// NTwin23
		}

		/// <summary>
		/// �|�b�v�A�b�v������
		/// </summary>
		public void Hide()
		{
			if (childCount > 0)
			{
				HTMLDocument doc = webBrowser.GetDomDocument();
				IHTMLWindow2 window = (IHTMLWindow2)doc.parentWindow;
				IHTMLElement element = (window.@event != null) ? window.@event.srcElement : doc.body;
				bool onPopup = false;

				while (element.tagName != "BODY")
				{
					if (element.id != null &&
						Regex.IsMatch(element.id, "p\\d+"))
					{
						onPopup = true;
						break;
					}
					element = element.parentElement;
				}

				if (onPopup)
				{
					for (; ; )
					{
						IHTMLDOMNode node = (IHTMLDOMNode)element.parentElement;
						IHTMLElement temp = (IHTMLElement)node.lastChild;

						if (element.id != temp.id)
						{
							((IHTMLDOMNode)temp).removeNode(true);
							webBrowser.lastPopupRef = String.Empty;
							webBrowser.lastPopupRefImg = String.Empty;
							childCount--;
						}
						else
							break;
					}
				}
				else if (inPopup)
				{
					// ���ׂẴ|�b�v�A�b�v���폜
					IHTMLDOMNode node = (IHTMLDOMNode)doc.getElementById("popupBase");
					if (node != null)
						node.removeNode(true);

					webBrowser.lastPopupRef = String.Empty;
					webBrowser.lastPopupRefImg = String.Empty;	// NTwin23
					webBrowser.clickedPopup = false;
					childCount = 0;
				}

				inPopup = onPopup;
			}
		}

		/// <summary>
		/// �|�b�v�A�b�v�\���^��\���̏��
		/// </summary>
		public bool Visible
		{
			get
			{
				return (childCount > 0);
			}
		}
	}
}
