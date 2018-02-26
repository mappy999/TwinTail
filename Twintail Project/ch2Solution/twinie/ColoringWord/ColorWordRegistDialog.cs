using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace Twin
{
	public partial class ColorWordRegistDialog : Form
	{
		private Color fore, back;
		private ColorWordInfo newWordInfo = null;
		private SoundPlayer soundPlayer = null;
		private bool enableUpdateSample = true;

		public ColorWordInfo[] ResultColorWordInfoArray
		{
			get
			{
				List<ColorWordInfo> list = new List<ColorWordInfo>();

				if (this.DialogResult != DialogResult.OK)
					return list.ToArray();

				foreach (string text in textBoxWords.Lines)
				{
					ColorWordInfo info = new ColorWordInfo(newWordInfo);
					string t = text.Trim();
					if (t.Length > 0)
					{
						info.Text = t;
						list.Add(info);
					}
				}
				return list.ToArray();
			}
		}


		public ColorWordRegistDialog(ColorWordInfo baseInfo)
		{
			InitializeComponent();

			if (baseInfo != null)
			{
				fore = baseInfo.ForeColor;
				back = baseInfo.BackColor;
			}
			else
			{
				fore = SystemColors.WindowText;
				back = SystemColors.Window;
			}

			enableUpdateSample = false;
			textBoxWords.Text = baseInfo.Text;

			if (baseInfo.IsBold)
				checkBoxBold.Checked = true;

			if (baseInfo.IsItalic)
				checkBoxItalic.Checked = true;

			if (baseInfo.IsRegex)
				checkBoxRegex.Checked = true;

			if (baseInfo.IsPlaySound)
			{
				checkBoxPlaySound.Checked = true;
				textBoxSoundFilePath.Text = baseInfo.SoundFilePath;
			}
			if (baseInfo.IsPopup)
			{
				checkBoxShowPopup.Checked = true;
				textBoxMessage.Text = baseInfo.PopupText;
			}
			enableUpdateSample = true;
			UpdateSample();
		}

		public ColorWordRegistDialog()
			: this(new ColorWordInfo())
		{
		}

		private void textBoxWords_TextChanged(object sender, EventArgs e)
		{
			buttonOK.Enabled = textBoxWords.TextLength > 0;
			UpdateSample();
		}

		private void linkLabelForeColor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			colorDialog1.Color = this.fore;
			if (colorDialog1.ShowDialog(this) == DialogResult.OK)
			{
				this.fore = colorDialog1.Color;
				UpdateSample();
			}
		}

		private void linkLabelBackColor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			colorDialog1.Color = this.back;
			if (colorDialog1.ShowDialog(this) == DialogResult.OK)
			{
				this.back = colorDialog1.Color;
				UpdateSample();
			}
		}

		private void checkBoxPlaySound_CheckedChanged(object sender, EventArgs e)
		{
			buttonRefSoundFile.Enabled = textBoxSoundFilePath.Enabled = checkBoxPlaySound.Checked;
		}

		private void checkBoxShowPopup_CheckedChanged(object sender, EventArgs e)
		{
			textBoxMessage.Enabled = checkBoxShowPopup.Checked;
		}

		private void checkBoxBold_CheckedChanged(object sender, EventArgs e)
		{
			UpdateSample();
		}

		private void checkBoxItalic_CheckedChanged(object sender, EventArgs e)
		{
			UpdateSample();
		}

		private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			SoundStop();
			soundPlayer = new SoundPlayer(textBoxSoundFilePath.Text);
			soundPlayer.Play();
		}

		private void buttonRefSoundFile_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
				textBoxSoundFilePath.Text = openFileDialog1.FileName;
		}

		private void textBoxSoundFilePath_TextChanged(object sender, EventArgs e)
		{
			SoundStop();
			linkLabel3.Enabled = File.Exists(textBoxSoundFilePath.Text);
		}

		private void UpdateSample()
		{
			if (enableUpdateSample == false)
				return;

			labelSample.ForeColor = this.fore;
			labelSample.BackColor = this.back;

			FontStyle fs = FontStyle.Regular;
			if (checkBoxBold.Checked)
				fs |= FontStyle.Bold;
			if (checkBoxItalic.Checked)
				fs |= FontStyle.Italic;

			labelSample.Font = new Font("‚l‚r ‚oƒSƒVƒbƒN", 9, fs);
			labelSample.Text = (textBoxWords.Lines.Length > 0) ? textBoxWords.Lines[0] : "ƒTƒ“ƒvƒ‹";
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			newWordInfo = new ColorWordInfo();

			newWordInfo.ForeColor = fore;
			newWordInfo.BackColor = back;
			newWordInfo.IsBold = checkBoxBold.Checked;
			newWordInfo.IsItalic = checkBoxItalic.Checked;
			newWordInfo.IsRegex = checkBoxRegex.Checked;
			newWordInfo.IsPopup = checkBoxShowPopup.Checked;
			newWordInfo.PopupText = textBoxMessage.Text;
			newWordInfo.IsPlaySound = checkBoxPlaySound.Checked;

			if (File.Exists(textBoxSoundFilePath.Text))
			{
				newWordInfo.SoundFilePath = textBoxSoundFilePath.Text;
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
		}

		private void SoundStop()
		{
			if (soundPlayer != null)
			{
				soundPlayer.Stop();
				soundPlayer.Dispose();
				soundPlayer = null;
			}
		}

		private void ColorWordRegistDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			SoundStop();
		}

		private void textBoxMessage_TextChanged(object sender, EventArgs e)
		{
		}
	}
}