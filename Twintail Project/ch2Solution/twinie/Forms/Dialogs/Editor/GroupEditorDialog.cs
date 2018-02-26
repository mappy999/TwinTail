using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Collections;

namespace Twin.Forms
{
	public partial class GroupEditorDialog : Form
	{
		private List<ThreadGroup> groupList;
		private ThreadGroup selectedGroup = null;

		public List<ThreadHeader> SelectedThreadItems
		{
			get
			{
				List<ThreadHeader> selectedItems = new List<ThreadHeader>();

				foreach (ThreadListItem item in listView1.SelectedItems)
				{
					selectedItems.Add(item.ThreadHeader);
				}

				return selectedItems;
			}
		}
	
		public GroupEditorDialog(ref List<ThreadGroup> groupList)
		{
			InitializeComponent();

			this.groupList = groupList;

			UpdateTreeView();
		}

		private void UpdateTreeView()
		{
			treeView1.Nodes.Clear();

			foreach (ThreadGroup g in groupList)
				treeView1.Nodes.Add(new GroupNode(g));
		}

		private void GroupEditorDialog_FormClosing(object sender, FormClosingEventArgs e)
		{

		}


		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			GroupNode n = (GroupNode)e.Node;
			selectedGroup = n.ThreadGroup;

			List<ThreadListItem> list = new List<ThreadListItem>();

			foreach (ThreadHeader h in n.ThreadGroup.ThreadList.Items)
				list.Add(new ThreadListItem(h));

			listView1.Items.Clear();
			listView1.Items.AddRange(list.ToArray());
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void menuItemThread_DropDownOpening(object sender, EventArgs e)
		{

		}

		private void menuItemGroup_DropDownOpening(object sender, EventArgs e)
		{

		}

		private void treeView1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeNode n = treeView1.GetNodeAt(
					treeView1.PointToClient(MousePosition));

				if (n != null)
					treeView1.SelectedNode = n;
			}
		}

		#region inner class
		internal class GroupNode : TreeNode
		{
			private ThreadGroup threadGroup;

			public ThreadGroup ThreadGroup
			{
				get
				{
					return threadGroup;
				}
				set
				{
					threadGroup = value;
				}
			}

			public GroupNode(ThreadGroup group)
				: base(group.Name)
			{
				this.threadGroup = group;
			}
		}

		internal class ThreadListItem : ListViewItem
		{
			private ThreadHeader header;

			public ThreadHeader ThreadHeader
			{
				get
				{
					return header;
				}
				set
				{
					header = value;
				}
			}
	
			public ThreadListItem(ThreadHeader h)
				: base (h.Subject)
			{
				this.header = h;

				base.ImageIndex = ImageIndexValues.ItemNormal;

				base.SubItems.Add(h.BoardInfo.Name);
				base.SubItems.Add(h.ResCount.ToString());
				base.SubItems.Add((h.GotByteCount / 1024).ToString() + "KB");
				base.SubItems.Add(StringUtility.StringOf(h.LastModified, true));

				if (h.LastWritten != DateTime.MinValue)
					base.SubItems.Add(StringUtility.StringOf(h.LastWritten, true));
			}
		}
		#endregion

		struct ImageIndexValues
		{
			public const int FolderClosed = 0;
			public const int FolderOpened = 1;
			public const int ItemNormal = 2;
			public const int ItemSelected = 3;
		}

		private void toolStripButtonNewGroup_Click(object sender, EventArgs e)
		{
			menuItemGroupNew_Click(null, null);
		}

		private void toolStripButtonGroupRename_Click(object sender, EventArgs e)
		{
			menuItemGroupRename_Click(null, null);
		}

		private void toolStripButtonGroupUp_Click(object sender, EventArgs e)
		{
			menuItemGroupUp_Click(null, null);
		}

		private void toolStripButtonGroupDown_Click(object sender, EventArgs e)
		{
			menuItemGroupDown_Click(null, null);
		}

		private void toolStripButtonGroupDel_Click(object sender, EventArgs e)
		{
			menuItemGroupDel_Click(null, null);
		}

		private void toolStripButtonItemUp_Click(object sender, EventArgs e)
		{
			menuItemThreadUp_Click(null, null);
		}

		private void toolStripButtonItemDown_Click(object sender, EventArgs e)
		{
			menuItemThreadDown_Click(null, null);
		}

		private void toolStripButtonItemDel_Click(object sender, EventArgs e)
		{
			menuItemThreadDel_Click(null, null);
		}

		private void menuItemFileClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void menuItemFileImport_Click(object sender, EventArgs e)
		{

		}

		private void menuItemExport_Click(object sender, EventArgs e)
		{

		}

		private void menuItemGroupNew_Click(object sender, EventArgs e)
		{
			FileNameEditorDialog dlg = new FileNameEditorDialog();

			dlg.Message = "作成するグループ名を入力してください";

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				ThreadGroup newGroup =
					new ThreadGroup(Twinie.Cache, Path.Combine(Settings.GroupFolderPath, dlg.FileName + ".grp"));

				newGroup.Save();
				groupList.Add(newGroup);

				TreeNode newNode = new GroupNode(newGroup);
				treeView1.Nodes.Add(newNode);
				treeView1.SelectedNode = newNode;
			}
		}

		private void menuItemGroupRename_Click(object sender, EventArgs e)
		{
			GroupNode node = (GroupNode)treeView1.SelectedNode;
			if (node == null)
				return;
			ThreadGroup group = node.ThreadGroup;

			FileNameEditorDialog dlg = new FileNameEditorDialog();

			dlg.Message = "変更後のグループ名を入力してください";
			dlg.FileName = Path.GetFileNameWithoutExtension(group.FileName);

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				group.Name =
					node.Text = dlg.FileName;
			}
		}

		private void menuItemGroupUp_Click(object sender, EventArgs e)
		{
			GroupNode n = (GroupNode)treeView1.SelectedNode;
			int index = n.Index;

			if (index == 0)
			{
				SystemSounds.Beep.Play();
			}
			else
			{
				treeView1.Nodes.RemoveAt(index);
				treeView1.Nodes.Insert(index - 1, n);

				groupList.RemoveAt(index);
				groupList.Insert(index - 1, n.ThreadGroup);
			}

			treeView1.SelectedNode = n;
		}

		private void menuItemGroupDown_Click(object sender, EventArgs e)
		{
			GroupNode n = (GroupNode)treeView1.SelectedNode;
			int index = n.Index;

			if (index + 1 < treeView1.Nodes.Count)
			{
				treeView1.Nodes.RemoveAt(index);
				treeView1.Nodes.Insert(index + 1, n);

				groupList.RemoveAt(index);
				groupList.Insert(index + 1, n.ThreadGroup);
			}
			else
			{
				SystemSounds.Beep.Play();
			}

			treeView1.SelectedNode = n;
		}

		private void menuItemGroupSortAc_Click(object sender, EventArgs e)
		{
			groupList.Sort(delegate(ThreadGroup x, ThreadGroup y)
			{
				return String.CompareOrdinal(x.Name, y.Name);
			});
			UpdateTreeView();
		}

		private void menuItemGroupSortDc_Click(object sender, EventArgs e)
		{
			groupList.Sort(delegate(ThreadGroup x, ThreadGroup y)
			{
				return String.CompareOrdinal(y.Name, x.Name);
			});
			UpdateTreeView();
		}

		private void menuItemGroupDel_Click(object sender, EventArgs e)
		{
			GroupNode node = (GroupNode)treeView1.SelectedNode;
			if (node == null)
				return;

			if (MessageBox.Show("選択されている項目 \"" + node.ThreadGroup.Name + "\" を削除してもよろしいですか？", "グループを削除",
				MessageBoxButtons.OKCancel)
				== DialogResult.OK)
			{
				groupList.Remove(node.ThreadGroup);
				node.Remove();

				File.Delete(node.ThreadGroup.FileName);
			}
		}

		private void menuItemGroupClear_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("すべてのグループを削除します。よろしいですか？", "全消去の確認",
				MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
			{
				treeView1.Nodes.Clear();

				foreach (ThreadGroup gp in groupList)
					File.Delete(gp.FileName);

				groupList.Clear();
			}
		}

		private void menuItemThreadCopyName_Click(object sender, EventArgs e)
		{
			ClipboardUtility.Copy(SelectedThreadItems, CopyInfo.Name);
		}

		private void menuItemThreadCopyURLName_Click(object sender, EventArgs e)
		{
			ClipboardUtility.Copy(SelectedThreadItems, CopyInfo.Name | CopyInfo.Url);
		}

		private void menuItemThreadCopyURL_Click(object sender, EventArgs e)
		{
			ClipboardUtility.Copy(SelectedThreadItems, CopyInfo.Url);
		}

		private void menuItemThreadUp_Click(object sender, EventArgs e)
		{
			ListItemUpDown(false);
		}

		private void menuItemThreadDown_Click(object sender, EventArgs e)
		{
			ListItemUpDown(true);
		}

		private void menuItemThreadDel_Click(object sender, EventArgs e)
		{
			int count = listView1.SelectedIndices.Count;

			if (MessageBox.Show("選択されている" + count + "個の項目を削除してもよろしいですか？", "選択項目を削除",
				MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				RemoveSelectedListItems();
				selectedGroup.Save();
			}
		}

		private void RemoveSelectedListItems()
		{
			while (listView1.SelectedIndices.Count > 0)
			{
				ThreadListItem item = (ThreadListItem)listView1.SelectedItems[0];

				selectedGroup.ThreadList.Items.Remove(item.ThreadHeader);
				listView1.Items.Remove(item);
			}
		}

		private void ListItemUpDown(bool down)
		{
			int count = listView1.SelectedIndices.Count;

			if (count == 0)
				return;

			int index = listView1.SelectedIndices[0];

			if (!down && index == 0 ||
				down && (index + count >= listView1.Items.Count))
			{
				return;
			}

			ArrayList items = new ArrayList(listView1.SelectedItems);

			RemoveSelectedListItems();

			index += down ? 1 : -1;

			for (int i = 0; i < items.Count; i++)
			{
				ThreadListItem item = (ThreadListItem)items[i];

				listView1.Items.Insert(index + i, item);
				selectedGroup.ThreadList.Items.Insert(index + i, item.ThreadHeader);
			}

			selectedGroup.Save();
		}
	}
}