namespace Twin
{
	partial class ColorWordRegistDialog
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				SoundStop();
			}

			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxWords = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.checkBoxRegex = new System.Windows.Forms.CheckBox();
			this.checkBoxBold = new System.Windows.Forms.CheckBox();
			this.checkBoxItalic = new System.Windows.Forms.CheckBox();
			this.checkBoxPlaySound = new System.Windows.Forms.CheckBox();
			this.checkBoxShowPopup = new System.Windows.Forms.CheckBox();
			this.textBoxSoundFilePath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonRefSoundFile = new System.Windows.Forms.Button();
			this.textBoxMessage = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.linkLabelForeColor = new System.Windows.Forms.LinkLabel();
			this.linkLabelBackColor = new System.Windows.Forms.LinkLabel();
			this.linkLabel3 = new System.Windows.Forms.LinkLabel();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.labelSample = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(163, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "一行ごとに単語を入力してください";
			// 
			// textBoxWords
			// 
			this.textBoxWords.AcceptsReturn = true;
			this.textBoxWords.Location = new System.Drawing.Point(14, 28);
			this.textBoxWords.Multiline = true;
			this.textBoxWords.Name = "textBoxWords";
			this.textBoxWords.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxWords.Size = new System.Drawing.Size(339, 53);
			this.textBoxWords.TabIndex = 1;
			this.textBoxWords.TextChanged += new System.EventHandler(this.textBoxWords_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 118);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(166, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "以下のオプションを選択してください";
			// 
			// checkBoxRegex
			// 
			this.checkBoxRegex.AutoSize = true;
			this.checkBoxRegex.Location = new System.Drawing.Point(14, 142);
			this.checkBoxRegex.Name = "checkBoxRegex";
			this.checkBoxRegex.Size = new System.Drawing.Size(72, 16);
			this.checkBoxRegex.TabIndex = 5;
			this.checkBoxRegex.Text = "正規表現";
			this.checkBoxRegex.UseVisualStyleBackColor = true;
			// 
			// checkBoxBold
			// 
			this.checkBoxBold.AutoSize = true;
			this.checkBoxBold.Location = new System.Drawing.Point(14, 164);
			this.checkBoxBold.Name = "checkBoxBold";
			this.checkBoxBold.Size = new System.Drawing.Size(48, 16);
			this.checkBoxBold.TabIndex = 6;
			this.checkBoxBold.Text = "太字";
			this.checkBoxBold.UseVisualStyleBackColor = true;
			this.checkBoxBold.CheckedChanged += new System.EventHandler(this.checkBoxBold_CheckedChanged);
			// 
			// checkBoxItalic
			// 
			this.checkBoxItalic.AutoSize = true;
			this.checkBoxItalic.Location = new System.Drawing.Point(14, 186);
			this.checkBoxItalic.Name = "checkBoxItalic";
			this.checkBoxItalic.Size = new System.Drawing.Size(48, 16);
			this.checkBoxItalic.TabIndex = 7;
			this.checkBoxItalic.Text = "斜体";
			this.checkBoxItalic.UseVisualStyleBackColor = true;
			this.checkBoxItalic.CheckedChanged += new System.EventHandler(this.checkBoxItalic_CheckedChanged);
			// 
			// checkBoxPlaySound
			// 
			this.checkBoxPlaySound.AutoSize = true;
			this.checkBoxPlaySound.Location = new System.Drawing.Point(14, 208);
			this.checkBoxPlaySound.Name = "checkBoxPlaySound";
			this.checkBoxPlaySound.Size = new System.Drawing.Size(168, 16);
			this.checkBoxPlaySound.TabIndex = 8;
			this.checkBoxPlaySound.Text = "一致したときにサウンドを鳴らす";
			this.checkBoxPlaySound.UseVisualStyleBackColor = true;
			this.checkBoxPlaySound.CheckedChanged += new System.EventHandler(this.checkBoxPlaySound_CheckedChanged);
			// 
			// checkBoxShowPopup
			// 
			this.checkBoxShowPopup.AutoSize = true;
			this.checkBoxShowPopup.Location = new System.Drawing.Point(14, 290);
			this.checkBoxShowPopup.Name = "checkBoxShowPopup";
			this.checkBoxShowPopup.Size = new System.Drawing.Size(240, 16);
			this.checkBoxShowPopup.TabIndex = 13;
			this.checkBoxShowPopup.Text = "一致したときにポップアップメッセージを表示する";
			this.checkBoxShowPopup.UseVisualStyleBackColor = true;
			this.checkBoxShowPopup.CheckedChanged += new System.EventHandler(this.checkBoxShowPopup_CheckedChanged);
			// 
			// textBoxSoundFilePath
			// 
			this.textBoxSoundFilePath.Enabled = false;
			this.textBoxSoundFilePath.Location = new System.Drawing.Point(33, 255);
			this.textBoxSoundFilePath.Name = "textBoxSoundFilePath";
			this.textBoxSoundFilePath.Size = new System.Drawing.Size(264, 19);
			this.textBoxSoundFilePath.TabIndex = 10;
			this.textBoxSoundFilePath.TextChanged += new System.EventHandler(this.textBoxSoundFilePath_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(31, 236);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(149, 12);
			this.label3.TabIndex = 9;
			this.label3.Text = "サウンドファイルへのファイルパス";
			// 
			// buttonRefSoundFile
			// 
			this.buttonRefSoundFile.Enabled = false;
			this.buttonRefSoundFile.Location = new System.Drawing.Point(303, 253);
			this.buttonRefSoundFile.Name = "buttonRefSoundFile";
			this.buttonRefSoundFile.Size = new System.Drawing.Size(50, 23);
			this.buttonRefSoundFile.TabIndex = 11;
			this.buttonRefSoundFile.Text = "参照...";
			this.buttonRefSoundFile.UseVisualStyleBackColor = true;
			this.buttonRefSoundFile.Click += new System.EventHandler(this.buttonRefSoundFile_Click);
			// 
			// textBoxMessage
			// 
			this.textBoxMessage.AcceptsReturn = true;
			this.textBoxMessage.Enabled = false;
			this.textBoxMessage.Location = new System.Drawing.Point(33, 331);
			this.textBoxMessage.Multiline = true;
			this.textBoxMessage.Name = "textBoxMessage";
			this.textBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxMessage.Size = new System.Drawing.Size(286, 42);
			this.textBoxMessage.TabIndex = 15;
			this.textBoxMessage.TextChanged += new System.EventHandler(this.textBoxMessage_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(31, 314);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(93, 12);
			this.label4.TabIndex = 14;
			this.label4.Text = "表示するメッセージ";
			// 
			// linkLabelForeColor
			// 
			this.linkLabelForeColor.AutoSize = true;
			this.linkLabelForeColor.Location = new System.Drawing.Point(197, 89);
			this.linkLabelForeColor.Name = "linkLabelForeColor";
			this.linkLabelForeColor.Size = new System.Drawing.Size(81, 12);
			this.linkLabelForeColor.TabIndex = 2;
			this.linkLabelForeColor.TabStop = true;
			this.linkLabelForeColor.Text = "文字色の変更...";
			this.linkLabelForeColor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelForeColor_LinkClicked);
			// 
			// linkLabelBackColor
			// 
			this.linkLabelBackColor.AutoSize = true;
			this.linkLabelBackColor.Location = new System.Drawing.Point(278, 89);
			this.linkLabelBackColor.Name = "linkLabelBackColor";
			this.linkLabelBackColor.Size = new System.Drawing.Size(81, 12);
			this.linkLabelBackColor.TabIndex = 3;
			this.linkLabelBackColor.TabStop = true;
			this.linkLabelBackColor.Text = "背景色の変更...";
			this.linkLabelBackColor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBackColor_LinkClicked);
			// 
			// linkLabel3
			// 
			this.linkLabel3.AutoSize = true;
			this.linkLabel3.Enabled = false;
			this.linkLabel3.Location = new System.Drawing.Point(269, 277);
			this.linkLabel3.Name = "linkLabel3";
			this.linkLabel3.Size = new System.Drawing.Size(29, 12);
			this.linkLabel3.TabIndex = 12;
			this.linkLabel3.TabStop = true;
			this.linkLabel3.Text = "再生";
			this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(185, 403);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(83, 23);
			this.buttonCancel.TabIndex = 17;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Enabled = false;
			this.buttonOK.Location = new System.Drawing.Point(92, 403);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(83, 23);
			this.buttonOK.TabIndex = 16;
			this.buttonOK.Text = "登録";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// labelSample
			// 
			this.labelSample.BackColor = System.Drawing.SystemColors.Window;
			this.labelSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelSample.Location = new System.Drawing.Point(14, 84);
			this.labelSample.Name = "labelSample";
			this.labelSample.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.labelSample.Size = new System.Drawing.Size(177, 23);
			this.labelSample.TabIndex = 18;
			this.labelSample.Text = "Sample.zip";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Filter = "Sound Files (*.wav)|*.wav|All Files (*.*)|*.*";
			// 
			// colorDialog1
			// 
			this.colorDialog1.FullOpen = true;
			// 
			// ColorWordRegistDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(362, 438);
			this.Controls.Add(this.labelSample);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.linkLabel3);
			this.Controls.Add(this.linkLabelBackColor);
			this.Controls.Add(this.linkLabelForeColor);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBoxMessage);
			this.Controls.Add(this.buttonRefSoundFile);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBoxSoundFilePath);
			this.Controls.Add(this.checkBoxShowPopup);
			this.Controls.Add(this.checkBoxPlaySound);
			this.Controls.Add(this.checkBoxItalic);
			this.Controls.Add(this.checkBoxBold);
			this.Controls.Add(this.checkBoxRegex);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxWords);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ColorWordRegistDialog";
			this.Text = "強調表示する単語の新規登録";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColorWordRegistDialog_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxWords;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkBoxRegex;
		private System.Windows.Forms.CheckBox checkBoxBold;
		private System.Windows.Forms.CheckBox checkBoxItalic;
		private System.Windows.Forms.CheckBox checkBoxPlaySound;
		private System.Windows.Forms.CheckBox checkBoxShowPopup;
		private System.Windows.Forms.TextBox textBoxSoundFilePath;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonRefSoundFile;
		private System.Windows.Forms.TextBox textBoxMessage;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.LinkLabel linkLabelForeColor;
		private System.Windows.Forms.LinkLabel linkLabelBackColor;
		private System.Windows.Forms.LinkLabel linkLabel3;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label labelSample;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ColorDialog colorDialog1;
	}
}