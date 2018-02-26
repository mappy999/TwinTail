// IEComSearchResult.cs

namespace Twin.Text
{
	using System;
	using System.Drawing;
	using System.Collections;
	using mshtml;
	using Twin.Text;
	using CSharpSamples;
using System.Collections.Generic;

	/// <summary>
	/// IE�R���|�[�l���g���g�p�����P��̌����ƌ��ʂ�\��
	/// </summary>
	public sealed class IEComSearcher : AbstractSearcher
	{
		private readonly HTMLDocument document;
		private IHTMLTxtRange textRange;
		private string bookmark;
		private bool tempRightToLeft;

		private List<__Bookmark> highlights = new List<__Bookmark>();

		private class __Bookmark
		{
			public string Value;
			public object BackColor, ForeColor;
		}

		/// <summary>
		/// IHTMLTxtRange.findText�p�̌����I�v�V�����l���擾
		/// </summary>
		private int ieSearchOptions {
			get {
				SearchOptions flags = 0;

				if ((Options & SearchOptions.MatchCase) != 0)
					flags |= SearchOptions.MatchCase;

				if ((Options & SearchOptions.WholeWordsOnly) != 0)
					flags |= SearchOptions.WholeWordsOnly;

				return (int)flags;
			}
		}

		/// <summary>
		/// �h�L�������g�̌�납�猟�����J�n���邩�ǂ���
		/// </summary>
		private bool RightToLeft {
			get {
				return (Options & SearchOptions.RightToLeft) != 0;
			}
		}

		/// <summary>
		/// document.body.createTextRange�̖߂�l��Ԃ�
		/// </summary>
		private IHTMLTxtRange htmlTextRange {
			get {
				HTMLBody body = (HTMLBody)document.body;
				return body.createTextRange();
			}
		}

		/// <summary>
		/// IEComSearcher�N���X�̃C���X�^���X��������
		/// </summary>
		/// <param name="doc">�����Ώۂ̃h�L�������g</param>
		/// <param name="keyword">�����L�[���[�h</param>
		/// <param name="options">�����I�v�V����</param>
		public IEComSearcher(HTMLDocument doc)
		{
			if (doc == null) {
				throw new ArgumentNullException("doc");
			}
			textRange = null;
			document = doc;
		}

		/// <summary>
		/// ���̒P������������̈ʒu�܂ŃX�N���[��
		/// </summary>
		/// <returns></returns>
		public override bool Search(string text)
		{
			if (textRange == null)
				textRange = htmlTextRange;

			if (text == null || text == String.Empty)
				return false;

			// 0 = none
			// 2 = whole words only
			// 4 = match case
			int flags = ieSearchOptions;
			int count = RightToLeft ? -1 : 0;

			// �����������ύX���ꂽ��͈͂�������
			if (RightToLeft != tempRightToLeft)
			{
				textRange.moveStart("Textedit", 0);
				textRange.moveEnd("Textedit", 1);
				tempRightToLeft = RightToLeft;
			}

			if (textRange.findText(text, count, flags))
			{
				bookmark = textRange.getBookmark();

				textRange.select();
				textRange.scrollIntoView(true);

				// �P�����ʂ̒����ɗ���܂ŃX�N���[������
				HTMLBody body = (HTMLBody)document.body;
				IHTMLElement elem = textRange.parentElement();

				body.scrollTop = elem.offsetTop - body.clientHeight / 2;

				if (RightToLeft) {
					textRange.moveStart("Textedit", 0);
					textRange.moveEnd("Character", -1);
				}
				else {
					textRange.moveStart("Character", 1);
					textRange.moveEnd("Textedit", 1);
				}
				return true;
			}
			else {
				return false;
			}
		}

		/// <summary>
		/// �����P������ׂăn�C���C�g�\��
		/// </summary>
		public override void Highlights(string text)
		{
			if (text == null || text == String.Empty)
				return;

			IHTMLTxtRange range = htmlTextRange;

			while (range.findText(text, 0, ieSearchOptions))
			{
				__Bookmark b = new __Bookmark();
				b.Value = range.getBookmark();
				b.BackColor = range.queryCommandValue("BackColor");
				b.ForeColor = range.queryCommandValue("ForeColor");

				range.execCommand("BackColor", false, 
					ColorTranslator.ToHtml(SystemColors.Highlight));

				range.execCommand("ForeColor", false,
					ColorTranslator.ToHtml(SystemColors.HighlightText));

				range.moveStart("Character", 1);
				range.moveEnd("Textedit", 1);

				highlights.Add(b);
			}
		}

		/// <summary>
		/// ���������Z�b�g��������Ԃɖ߂�
		/// </summary>
		public override void Reset()
		{
			if (highlights.Count > 0)
			{
				IHTMLTxtRange range = this.htmlTextRange;
				foreach (__Bookmark b in highlights)
				{
					if (range.moveToBookmark(b.Value))
					{
						range.execCommand("BackColor", false, b.BackColor);
						range.execCommand("ForeColor", false, b.ForeColor);
					}
				}
				highlights.Clear();
			}
			//document.location.reload(false);
			textRange = null;
			bookmark = null;


		}
	}
}
