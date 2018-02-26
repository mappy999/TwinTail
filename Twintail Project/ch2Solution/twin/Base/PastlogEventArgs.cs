using System;
using System.Collections.Generic;
using System.Text;

namespace Twin
{
	public class PastlogEventArgs : EventArgs
	{
		private ThreadHeader headerInfo;
		/// <summary>
		/// �X���b�h�̃w�b�_�[�����擾���܂��B
		/// </summary>
		public ThreadHeader HeaderInfo
		{
			get
			{
				return headerInfo;
			}
		}
	
		private bool retry = false;
		/// <summary>
		/// �ēx�擾�����݂邩�ǂ����������l���擾�܂��͐ݒ肵�܂��B
		/// </summary>
		public bool Retry
		{
			get
			{
				return retry;
			}
			set
			{
				retry = value;
			}
		}

	

		public PastlogEventArgs(ThreadHeader header)
		{
			this.headerInfo = header;
		}
	}
}
