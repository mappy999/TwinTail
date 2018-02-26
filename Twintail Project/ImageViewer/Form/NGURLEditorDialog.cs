// NGUrlEditorDialog.cs

namespace ImageViewerDll
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;

	/// <summary>
	/// NGUrlEditorDialog の概要の説明です。
	/// </summary>
	public class NGUrlEditorDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBoxPatterns;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// NGURLのパターン配列を取得または設定
		/// </summary>
		public string[] Patterns {
			set {
				textBoxPatterns.Lines = value;
			}
			get {
				//return textBoxPatterns.Lines;

				ArrayList arrayList = new ArrayList();

				// 空行は除く
				foreach (string word in textBoxPatterns.Lines)
					if (word != String.Empty) arrayList.Add(word);

				return (string[])arrayList.ToArray(typeof(string));
			}
		}

		/// <summary>
		/// NGUrlEditorDialogクラスのインスタンスを初期化
		/// </summary>
		public NGUrlEditorDialog()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.textBoxPatterns = new System.Windows.Forms.TextBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBoxPatterns
			// 
			this.textBoxPatterns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxPatterns.Location = new System.Drawing.Point(6, 5);
			this.textBoxPatterns.Multiline = true;
			this.textBoxPatterns.Name = "textBoxPatterns";
			this.textBoxPatterns.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxPatterns.Size = new System.Drawing.Size(403, 240);
			this.textBoxPatterns.TabIndex = 0;
			this.textBoxPatterns.WordWrap = false;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonCancel.AutoSize = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(213, 250);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(106, 25);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "キャンセル";
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonOK.AutoSize = true;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOK.Location = new System.Drawing.Point(95, 250);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(107, 25);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			// 
			// NGUrlEditorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(414, 276);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.textBoxPatterns);
			this.Name = "NGUrlEditorDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "NGURLを編集";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
	}
}
