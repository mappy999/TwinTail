// Be2chPost.cs

using System;
using System.Text;

namespace Twin.Bbs
{
	/// <summary>
	/// Be2chPost �̊T�v�̐����ł��B
	/// </summary>
	public class Be2chPost : X2chPost
	{
		//public override bool CanPostThread{get {return false;}}
		//public override bool CanPostRes{get {return false;}}

		public override bool SendBeID {
			set {}
			get { return true; }
		}

		/// <summary>
		/// Be2chPost �N���X�̃C���X�^���X��������
		/// </summary>
		public Be2chPost() : base(Encoding.GetEncoding("euc-jp"))
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}
	}
}
