// HtmlPopup.cs

namespace PopupTest
{
	using System;
	using System.Drawing;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using AxSHDocVw;
	using SHDocVw;
	using mshtml;
	using Twin;
	using Twin.Forms;

	/// <summary>
	/// HtmlPopup の概要の説明です。
	/// </summary>
	public class HtmlPopup : IPopupBase
	{
		private IEComThreadBrowser webBrowser;
		private ThreadSkinBase skin;
		private Point adjust = new Point(10, 10);

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

		/// <summary>
		/// ポップアップする座標を取得
		/// </summary>
		protected Point PopupLocation {
			get {
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
				throw new ApplicationException("PopupPositionが不正です");
			}
		}

		/// <summary>
		/// ポップアップする位置を取得または設定
		/// </summary>
		public PopupPosition Position {
			set {
				if (position != value)
					position = value;
			}
			get { return position; }
		}

		/// <summary>
		/// ポップアップの最大サイズを取得または設定
		/// </summary>
		public Size Maximum {
			set {
				maximum = value;
			}
			get {
				return maximum;
			}
		}

		/// <summary>
		/// 常に表示するかどうかを示す値を取得
		/// </summary>
		public bool ShowAlways {
			set {
				throw new NotSupportedException("ShowAlwaysプロパティはサポートしていません");
			}
			get {
				return false;
			}
		}

		/// <summary>
		/// ポップアップする文字のフォントを取得または設定
		/// </summary>
		public Font Font {
			set {
				font = value;
				fontHtml = String.Format("font-family:{0}; font-size:{1}pt;",
						value.Name, value.Size);
			}
			get {
				return font;
			}
		}

		/// <summary>
		/// ポップアップする文字色を取得または設定
		/// </summary>
		public Color ForeColor {
			set {
				foreColorHtml = ColorTranslator.ToHtml(value);
			}
			get {
				return ColorTranslator.FromHtml(foreColorHtml);
			}
		}

		/// <summary>
		/// ポップアップの背景色を取得または設定
		/// </summary>
		public Color BackColor {
			set {
				backColorHtml = ColorTranslator.ToHtml(value);
			}
			get {
				return ColorTranslator.FromHtml(backColorHtml);
			}
		}

		/// <summary>
		/// ポップアップの幅を取得
		/// </summary>
		public int Width {
			get {
				return width;
			}
		}

		/// <summary>
		/// ポップアップの高さを取得
		/// </summary>
		public int Height {
			get {
				return height;
			}
		}

		/// <summary>
		/// 画像ポップアップの画像サイズを取得または設定
		/// </summary>
		public Size ImageSize {
			set {
				imageSize = new Size(
					Math.Max(0, value.Width),
					Math.Max(0, value.Height));
			}
			get { return imageSize; }
		}

		/// <summary>
		/// HtmlPopupクラスのインスタンスを初期化
		/// </summary>
		/// <param name="doc"></param>
		public HtmlPopup(IEComThreadBrowser wb, ThreadSkinBase skin)
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
			this.webBrowser = wb;
			this.skin = skin;
			this.Font = new Font("ＭＳ Ｐゴシック", 9);

			position = PopupPosition.TopRight;
			maximum = new Size(450, 300);
			imageSize = new Size(200, 0);
			foreColorHtml = "#000000";
			backColorHtml = "#ffffff";
			width = height = 0;
		}

		/// <summary>
		/// 指定したhtml文字列をポップアップ
		/// </summary>
		/// <param name="htmlText"></param>
		public void Show(string htmlText)
		{
			HTMLDocument doc = webBrowser.GetDocument();
			HTMLBody body = (HTMLBody)doc.body;
			IHTMLElement root = doc.getElementById("popupBase");

			// ポップアップのルートが作成されていなければ
			if (root == null)
			{
				// ポップアップの基本となる要素を挿入
				body.insertAdjacentHTML("afterBegin", "<dl><div id=\"popupBase\" style=\"" + fontHtml + "\"></div></dl>");
				root = doc.getElementById("popupBase");
			}

			// ポップアップ元を作成
			root.insertAdjacentHTML("beforeEnd",
				String.Format("<div id=\"p{0}\" style=\"border: solid gray 1px; padding: 3px; background: window; overflow: auto; position: absolute;\"></div>", childCount));

			// 表示内容を設定
			IHTMLElement div = doc.getElementById("p" + childCount);
			div.innerHTML = htmlText;

			// ポップアップの最大幅を求める
			int maxWidth = body.clientWidth;
			int maxHeight = body.clientHeight / 2 + body.clientHeight / 3; // 画面の3/2

			// 要素が最大縦幅より大きくならないように調整
			width = Math.Min(div.offsetWidth, maxWidth);
			height = Math.Min(div.offsetHeight, maxHeight);

			// ポップアップ位置を調整 (レスの右上に配置)
			Point point = PopupLocation;
			
			// pointをクライアント座標に変換
			Point clientPos = new Point(
				point.X - body.scrollLeft,
				point.Y - body.scrollTop);

			// 上と左のはみ出しチェック
			if (clientPos.X < 0) point.X = body.scrollLeft;
			if (clientPos.Y < 0) point.Y = body.scrollTop;

			// 幅がクライアント幅を超えていたら調整
			if (clientPos.X + width > body.clientWidth)
			{
				int sub_w = (body.clientWidth - width);
				point.X = (body.scrollLeft + Math.Max(0, sub_w));
			}

			// 高さがクライアント幅を超えていたら調整
			if (clientPos.Y + height > body.clientHeight)
			{
				int sub_h = (body.clientHeight - height);
				point.Y = (body.scrollTop + Math.Max(0, sub_h));
			}

			// サイズを設定
			div.style.pixelLeft = point.X;
			div.style.pixelTop = point.Y;
			div.style.pixelWidth = width;
			div.style.pixelHeight = height;

			// 色を設定
			div.style.backgroundColor = backColorHtml;
			div.style.color = foreColorHtml;
			childCount++;
		}

		/// <summary>
		/// 指定したレスをスキン処理後ポップアップ表示
		/// </summary>
		/// <param name="items"></param>
		public void Show(ResSetCollection items)
		{
			Show(skin.Convert(items));
		}

		/// <summary>
		/// 指定したレスをスキン処理後ポップアップ表示
		/// </summary>
		/// <param name="res"></param>
		public void Show(ResSet res)
		{
			ResSetCollection items = new ResSetCollection();
			items.Add(res);

			Show(res);
		}

		/// <summary>
		/// 指定したuriをイメージタグを使用してポップアップ表示
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

			Show(html);
		}

		/// <summary>
		/// ポップアップを消す
		/// </summary>
		public void Hide()
		{
			if (childCount > 0)
			{
				HTMLDocument doc = webBrowser.GetDocument();
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
					for (;;)
					{
						IHTMLDOMNode node = (IHTMLDOMNode)element.parentElement;
						IHTMLElement temp = (IHTMLElement)node.lastChild;

						if (element.id != temp.id)
						{
							((IHTMLDOMNode)temp).removeNode(true);
							webBrowser.lastPopupRef = String.Empty;
							childCount--;
						}
						else break;
					}
				}
				else if (inPopup)
				{
					// すべてのポップアップを削除
					IHTMLDOMNode node = (IHTMLDOMNode)doc.getElementById("popupBase");
					if (node != null)
						node.removeNode(true);

					webBrowser.lastPopupRef = String.Empty;
					childCount = 0;
				}

				inPopup = onPopup;
			}
		}
	}
}
