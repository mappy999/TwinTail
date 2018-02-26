using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Twin.Text;
using Twin.Tools;
using CSharpSamples;
using Twin.Bbs;
using System.ComponentModel;

namespace Twin.Forms
{
	public partial class Twin2IeBrowser
	{
		#region ファイルメニュー

		private void menuItemFile_Popup(object sender, System.EventArgs e)
		{
			menuItemFileOnline.Checked = IsOnline;
		}

		// 板一覧をオンラインで更新
		private void menuItemFileBoardUpdate_Click(object sender, System.EventArgs e)
		{
			BoardUpdateDialog dlg = new BoardUpdateDialog(_2chTable);
			dlg.Updated += new BoardUpdateEventHandler(OnBoardUpdate);
			
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				_2chTable.SaveTable(Settings.BoardTablePath);

				allTable.Clear();
				allTable.Add(userTable);
				allTable.Add(_2chTable);

				tableView.Table = allTable;
			}
		}

		// お気に入り以外のログを削除
		private void menuItemFileRemoveLogs_Click(object sender, System.EventArgs e)
		{
			if (threadTabController.WindowCount == 0)
			{
				BookmarkRoot all = new BookmarkRoot();
				all.Children.AddRange(bookmarkRoot.Children);
				all.Children.AddRange(warehouseRoot.Children);

				CacheClearDialog dlg = new CacheClearDialog(cache, allTable, all);
				dlg.ShowDialog(this);
			}
			else {
				MessageBox.Show("その前にまずスレッドをすべて閉じてください");
			}
		}

		// Webキャッシュを削除
		private void menuItemFileClearWebCache_Click(object sender, System.EventArgs e)
		{
		}

		// 最近閉じた履歴を作成
		private void menuItemFileCloseHistory_Popup(object sender, System.EventArgs e)
		{
			menuItemFileCloseHistory.DropDownItems.Clear();
			int count = 1;

			foreach (ThreadHeader header in closedThreadHistory.Items)
			{
				// アクセスキーは数字とアルファベットを使用する
				string key = StringUtility.GetAccessKeyString(count++);

				ToolStripMenuItem menu = new ToolStripMenuItem();
				menu.Text = String.Format("(&{0}) {1}", key, header.Subject);
				menu.Click += new EventHandler(ClosedHistory_Click);
				menu.Tag = header;

				menuItemFileCloseHistory.DropDownItems.Add(menu);
			}

			menuItemFileCloseHistory.DropDownItems.Add(new ToolStripSeparator());
			menuItemFileCloseHistory.DropDownItems.Add(menuItemFileHistoryClear);
			menuItemFileCloseHistory.DropDownItems.Add(menuItemFileHistoryUpdateCheck);
		}
		
		// 最近閉じたスレッドの履歴がクリックされた
		private void ClosedHistory_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			ThreadOpen(menu.Tag as ThreadHeader, true);
		}

		// 最近閉じた履歴をクリア
		private void menuItemFileHistoryClear_Click(object sender, System.EventArgs e)
		{
			closedThreadHistory.Items.Clear();
		}

		// 最近閉じたスレを更新チェック
		private void menuItemFileHistoryUpdateCheck_Click(object sender, System.EventArgs e)
		{
			BookmarkPatrol(closedThreadHistory.Items, true);
		}

		#region File IO
		private void menuItemOpenDat_Click(object sender, System.EventArgs e)
		{
			OpenDat();
		}

		private void menuItemFileOpenMonalog_Click(object sender, System.EventArgs e)
		{
			OpenMonalog();
		}

		private void menuItemFileSaveDat_Click(object sender, System.EventArgs e)
		{
			SaveDat();
		}

		private void menuItemFileSaveHtml_Click(object sender, System.EventArgs e)
		{
			SaveHtml();
		}

		private void menuItemFileSaveMonalog_Click(object sender, System.EventArgs e)
		{
			SaveMonalog();
		}
		#endregion

		private void menuItemFilePrint_Click(object sender, System.EventArgs e)
		{
		
		}

		private void menuItemFileOnline_Click(object sender, System.EventArgs e)
		{
			IsOnline = !IsOnline;
		}

		private void menuItemFileExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void miSaveSettings_Click(object sender, EventArgs e)
		{
			SaveWindowsUrl();
			SaveSettingsAll();
		}



		private void menuItemFileClearNameHistory_Click(object sender, EventArgs e)
		{
			Twinie.Settings.Post.NameHistory.Keys.Clear();
			Twinie.Settings.Post.MailHistory.Keys.Clear();
		}

		private void menuItemFileClearSearchHistory_Click(object sender, EventArgs e)
		{
			Twinie.Settings.Search.SearchHistory.Keys.Clear();
		}

		private void menuItemClearAllHistory_Click(object sender, EventArgs e)
		{
			menuItemFileClearNameHistory_Click(sender, EventArgs.Empty);
			menuItemFileClearSearchHistory_Click(sender, EventArgs.Empty);
		}


		#endregion

		#region 編集メニュー
		private void menuItemEdit_Popup(object sender, System.EventArgs e)
		{
		
		}

		private void menuItemEditRegistBoard_Click(object sender, System.EventArgs e)
		{
			BoardTableEditorDialog dlg = new BoardTableEditorDialog(userTable);
			if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				userTable.SaveTable(Settings.UserTablePath);

			allTable.Clear();
			allTable.Add(userTable);
			allTable.Add(_2chTable);

			tableView.Table = allTable;
		}

		private void menuItemEditItaBtan_Click(object sender, System.EventArgs e)
		{
			ItaBotanEditorDialog dlg = new ItaBotanEditorDialog(allTable, cSharpToolBar);
			
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				if (settings.View.TableItaBotan)
					SetTableItaBotan(allTable);
			}
		}

		private void menuItemEditNGWords_Click(object sender, System.EventArgs e)
		{
			ShowNGWordsEditor();
		}

		private void menuItemSaveScrap_Click(object sender, System.EventArgs e)
		{
			if (threadTabController.IsSelected)
			{
				string text = threadTabController.Control.SelectedText;
				if (text != String.Empty) Scrap.Save(text);
				else ShowScrapEditor();
			}
		}

		private void menuItemSaveAa_Click(object sender, System.EventArgs e)
		{
			if (threadTabController.IsSelected)
			{
				string text = threadTabController.Control.SelectedText;
				SimpleAAEditorDialog dlg = new SimpleAAEditorDialog(Settings.AaFolderPath, text);
				dlg.Show();
			}
		}

		private void menuItemEditShortcut_Click(object sender, System.EventArgs e)
		{
			MenuShortcutEditor editor = new MenuShortcutEditor(mainMenu);
			if (editor.ShowDialog(this) == DialogResult.OK)
			{
				MenuSerializer2.Serialize(Settings.MenuShortcutPath, this);
			}
		}
		#endregion

		#region 検索メニュー
		private void menuItemSearch_Popup(object sender, System.EventArgs e)
		{
			menuItemEditSearchList.Enabled = listTabController.IsSelected;
		}
	
		private void menuItemEditSearchCache_Click(object sender, System.EventArgs e)
		{
			CacheSearch();
		}

		private void menuItemEditSearchList_Click(object sender, System.EventArgs e)
		{
			ListSearch();
		}
	
		private void menuItemEditFindBoard_Click(object sender, System.EventArgs e)
		{
			BoardSearch();
		}


		private void menuItemEditSearchSubjectBotanAdd_Click(object sender, EventArgs e)
		{
			RegistSearchBotan("");
		}
		#endregion

		#region 表示メニュー
		private void menuItemView_Popup(object sender, System.EventArgs e)
		{
			ViewSettings view = settings.View;
			RebarSettings rebar = settings.Rebar;
			menuItemViewAddressBar.Checked = rebar.AddressBar.Visible;
			menuItemViewToolBar.Checked = rebar.ToolBar.Visible;
			menuItemViewThreadToolBar.Checked = view.ThreadToolBar;
			menuItemViewListBar.Checked = rebar.ListToolBar.Visible;
			menuItemViewIButton.Checked = rebar.ItaButton.Visible;
			menuItemViewStatusBar.Checked = view.StatusBar;
			menuItemViewTools.Checked = rebar.ToolsBar.Visible;
			menuItemViewTableItaBotan.Checked = view.TableItaBotan;
			menuItemViewTableDockRight.Checked = view.TableDockRight;
			menuItemViewDockWriteBar.Checked = view.DockWriteBar;
			menuItemViewLiveMode.Checked = settings.Livemode;

			menuItemView_FixedRebarBands.Checked = settings.View.FixedRebarControl;

			menuItemFocusList.Enabled = listTabController.IsSelected;
			menuItemFocusThread.Enabled = threadTabController.IsSelected;
			
			menuItemViewHideTable.Checked = settings.View.HideTable;
			menuItemViewFillList.Checked = settings.View.FillList;
			menuItemViewFillThread.Checked = settings.View.FillThread;

			#region
			foreach (ToolStripItem a in menuItemLayout.DropDownItems)
			{
				ToolStripMenuItem menu = a as ToolStripMenuItem;
				if (menu != null) menu.Checked = false;
			}

			if (display.Layout == DisplayLayout.Default) menuItemLayoutStd.Checked = true;
			else if (display.Layout == DisplayLayout.Tate3) menuItemLayoutTate3.Checked = true;
			else if (display.Layout == DisplayLayout.Yoko3) menuItemLayoutYoko3.Checked = true;
			else if (display.Layout == DisplayLayout.TateYoko2) menuItemLayoutTateYoko2.Checked = true;
			else if (display.Layout == DisplayLayout.Extend1) menuItemLayoutExtend01.Checked = true;
			#endregion
		}

		private void menuItemViewToolBar_Click(object sender, System.EventArgs e)
		{
			settings.Rebar.ToolBar.Visible =
				bandWrapperMain.Visible = !settings.Rebar.ToolBar.Visible;	
		}

		private void menuItemViewThreadToolBar_Click(object sender, System.EventArgs e)
		{
			settings.View.ThreadToolBar = 
				threadToolPanel.Visible = !settings.View.ThreadToolBar;
		}

		private void menuItemViewListBar_Click(object sender, System.EventArgs e)
		{
			settings.Rebar.ListToolBar.Visible = 
				bandWrapperList.Visible = !settings.Rebar.ListToolBar.Visible;
		}

		private void menuItemViewAddressBar_Click(object sender, System.EventArgs e)
		{
			settings.Rebar.AddressBar.Visible = 
				bandWrapperAddress.Visible = !settings.Rebar.AddressBar.Visible;
		}

		private void menuItemViewIButton_Click(object sender, System.EventArgs e)
		{
			settings.Rebar.ItaButton.Visible =
				bandWrapperIButton.Visible = !settings.Rebar.ItaButton.Visible;		
		}

		private void menuItemViewTools_Click(object sender, System.EventArgs e)
		{
			settings.Rebar.ToolsBar.Visible = 
				bandWrapperTools.Visible = !settings.Rebar.ToolsBar.Visible;		
		}

		private void menuItemViewStatusBar_Click(object sender, System.EventArgs e)
		{
			settings.View.StatusBar =
				statusBar.Visible = !settings.View.StatusBar;		
		}
		
		private void menuItemViewTableItaBotan_Click(object sender, System.EventArgs e)
		{
			SetTableItaBotan(allTable);
		}

		private void menuItemLayoutStd_Click(object sender, System.EventArgs e)
		{
			display.SetLayout(DisplayLayout.Default);
			UpdateToolBar();
		}

		private void menuItemLayoutTate3_Click(object sender, System.EventArgs e)
		{
			display.SetLayout(DisplayLayout.Tate3);
			UpdateToolBar();
		}

		private void menuItemLayoutYoko3_Click(object sender, System.EventArgs e)
		{
			display.SetLayout(DisplayLayout.Yoko3);
			UpdateToolBar();
		}

		private void menuItemLayoutTateYoko2_Click(object sender, System.EventArgs e)
		{
			display.SetLayout(DisplayLayout.TateYoko2);
			UpdateToolBar();
		}

		private void menuItemLayoutExtend01_Click(object sender, System.EventArgs e)
		{
			display.SetLayout(DisplayLayout.Extend1);
			UpdateToolBar();
		}

		private void menuItemViewTableDockRight_Click(object sender, System.EventArgs e)
		{
			settings.View.TableDockRight = !settings.View.TableDockRight;
			display.SetLayout(display.Layout);
		}

		private void menuItemViewLiveMode_Click(object sender, System.EventArgs e)
		{
			Livemode();
		}

		private void menuItemViewUpChecker_Click(object sender, System.EventArgs e)
		{
			threadUpdateChecker.Show();
		}

		private void menuItemViewDockWriteBar_Click(object sender, System.EventArgs e)
		{
			dockWriteBar.Visible = settings.View.DockWriteBar =
				!settings.View.DockWriteBar;
		}

		private void menuItemFocusTable_Click(object sender, System.EventArgs e)
		{
			tabControlTable.SelectedTab = tabPageBoards;
			tableView.Focus();
		}

		private void menuItemFocusList_Click(object sender, System.EventArgs e)
		{
			listTabController.Control._Select();
		}

		private void menuItemFocusThread_Click(object sender, System.EventArgs e)
		{
			threadTabController.Control._Select();
		}

		private void menuItemFocusBookmark_Click(object sender, System.EventArgs e)
		{
			tabControlTable.SelectedTab = tabPageBookmarks;
			bookmarkView.Focus();
		}

		private void menuItemFocusWare_Click(object sender, System.EventArgs e)
		{
			tabControlTable.SelectedTab = tabPageWareHouse;
			warehouseView.Focus();
		}

		private void menuItemViewHideTable_Click(object sender, System.EventArgs e)
		{
			ViewHideTable(!settings.View.HideTable);		
		}

		private void menuItemViewFillList_Click(object sender, System.EventArgs e)
		{
			ViewFillList(!settings.View.FillList);
		}

		private void menuItemViewFillThread_Click(object sender, System.EventArgs e)
		{
			ViewFillThread(!settings.View.FillThread);
		}

		private void menuItemFontSize_Popup(object sender, System.EventArgs e)
		{
			foreach (ToolStripItem a in menuItemFontSize.DropDownItems)
			{
				ToolStripMenuItem menu = a as ToolStripMenuItem;
				if (menu != null)
					menu.Checked = false;
			}
			
			switch (settings.Thread.FontSize) {
			case FontSize.XLarge: menuItemFontSizeXLarge.Checked = true; break;
			case FontSize.Large: menuItemFontSizeLarge.Checked = true; break;
			case FontSize.Medium: menuItemFontSizeMedium.Checked = true; break;
			case FontSize.Small: menuItemFontSizeSmall.Checked = true; break;
			case FontSize.XSmall: menuItemFontSizeXSmall.Checked = true; break;
			}
		}

		private void menuItemFontSizeChange_Click(object sender, System.EventArgs e)
		{
			if (menuItemFontSizeLarge.Equals(sender))
				settings.Thread.FontSize = FontSize.Large;
			
			else if (menuItemFontSizeMedium.Equals(sender))
				settings.Thread.FontSize = FontSize.Medium;
			
			else if (menuItemFontSizeSmall.Equals(sender))
				settings.Thread.FontSize = FontSize.Small;
			
			else if (menuItemFontSizeXSmall.Equals(sender))
				settings.Thread.FontSize = FontSize.XSmall;

			else if (menuItemFontSizeXLarge.Equals(sender))
				settings.Thread.FontSize = FontSize.XLarge;

			UpdateFontSize();
		}


		private void menuItemView_FixedRebarBands_Click(object sender, EventArgs e)
		{
			FixedRebarBandSize = !FixedRebarBandSize;
		}
		#endregion

		#region お気に入りメニュー
		private void bookmarkMenu_Selected(object sender, ThreadHeaderEventArgs e)
		{
			foreach (ThreadHeader h in e.Items)
				ThreadOpen(h, true);
		}

		private void bookmarkMenu_UpdateCheck(object sender, BookmarkEventArgs e)
		{
			BookmarkPatrol(e.Selected, true, true);
		}

		// 更新チェックが可能かどうかを判断する時に呼ばれる
		private bool bookmarkMenu_IsUpdateCheckEnabled()
		{
			return (patroller == null) ? true : false;
		}
		#endregion

		#region 板メニュー
		private void menuItemList_Popup(object sender, System.EventArgs e)
		{
			foreach (ToolStripItem menu in menuItemList.DropDownItems)
				menu.Enabled = listTabController.IsSelected;

			menuItemListAllThreads.Enabled =
				menuItemListWrittenThreads.Enabled = true;

			menuItemListHistoryOpen.Enabled =
				listTabController.IsSelected && kakikomi.IsExists(listTabController.HeaderInfo);
		}

		private void menuItemListReload_Click(object sender, System.EventArgs e)
		{
			ListReload();
		}

		private void menuItemShowSettingTxt_Click(object sender, EventArgs e)
		{
			if (listTabController.IsSelected)
				tableView.ShowSettingTxt(listTabController.HeaderInfo);
		}

		private void menuItemListStop_Click(object sender, System.EventArgs e)
		{
			ListStop();		
		}
		
		private void menuItemListHistoryOpen_Click(object sender, System.EventArgs e)
		{
			if (listTabController.IsSelected)
				HistoryOpen(listTabController.HeaderInfo);
		}
		
		private void menuItemListDraftOpen_Click(object sender, System.EventArgs e)
		{
			if (listTabController.IsSelected)
				DraftOpen(listTabController.HeaderInfo);
		}

		private void menuItemListHistoryClear_Click(object sender, System.EventArgs e)
		{
			if (listTabController.IsSelected)
				HistoryClear(listTabController.HeaderInfo);		
		}

		private void menuItemListShowLocalRule_Click(object sender, EventArgs e)
		{
			if (listTabController.IsSelected)
				tableView.ShowLocalRule(listTabController.HeaderInfo);
		}

		private void menuItemListShowPicture_Click(object sender, EventArgs e)
		{
			if (listTabController.IsSelected)
				tableView.ShowPicture(listTabController.HeaderInfo);
		}

		private void menuItemListCacheOpen_Click(object sender, System.EventArgs e)
		{
			if (listTabController.IsSelected)
				CacheOpen(listTabController.HeaderInfo);		
		}

		private void menuItemListCacheClear_Click(object sender, System.EventArgs e)
		{
			if (listTabController.IsSelected)
				CacheClear(listTabController.HeaderInfo);		
		}

		private void menuItemListAllThreads_Click(object sender, System.EventArgs e)
		{
			ListAllThreads();
		}

		private void menuItemListWrittenThreads_Click(object sender, System.EventArgs e)
		{
			ListWrittenThreads();
		}

		private void menuItemListClose_Click(object sender, System.EventArgs e)
		{
			ListClose();
		}

		private void menuItemListIndexing_Click(object sender, System.EventArgs e)
		{
			ListIndexing();		
		}

		#endregion

		#region スレッドメニュー
		private void menuItemThread_Popup(object sender, System.EventArgs e)
		{
			foreach (ToolStripItem menu in menuItemThread.DropDownItems)
				menu.Enabled = threadTabController.IsSelected;

			menuItemNewThread.Enabled =
				listTabController.IsSelected;

			menuItemThreadAutoFocus.Checked = settings.ThreadAutoFocus;

			if (threadTabController.IsSelected)
			{
				ThreadControl thread = threadTabController.Control;
				menuItemThreadAutoScroll.Checked = thread.AutoScroll;
				menuItemThreadAutoReload.Checked = thread.AutoReload;
				menuItemThreadBookmark.Checked = IsBookmarked(thread.HeaderInfo);
				menuItemThreadSetUpChecker.Checked = threadUpdateChecker.IsContains(thread.HeaderInfo);
				menuItemThreadNextThreadCheck.Enabled = nextThreadChecker == null;
			}
		}

		private void menuItemThreadReload_Click(object sender, System.EventArgs e)
		{
			ThreadReload();
		}

		private void menuItemThreadReloadAll_Click(object sender, System.EventArgs e)
		{
			ThreadReloadAll();
		}

		private void menuItemThreadStop_Click(object sender, System.EventArgs e)
		{
			ThreadStop();
		}

		private void menuItemNewThread_Click(object sender, System.EventArgs e)
		{
			if (listTabController.IsSelected)
				PostThread(listTabController.HeaderInfo);
		}

		private void menuItemThreadPostRes_Click(object sender, System.EventArgs e)
		{
			ThreadPostRes();
		}

		private void menuItemThreadFind_Click(object sender, System.EventArgs e)
		{
			ThreadFind();
		}

		private void menuItemThreadLinkExtract_Click(object sender, System.EventArgs e)
		{
		
		}
	
		private void menuItemThreadSetUpChecker_Click(object sender, System.EventArgs e)
		{
			threadUpdateChecker.AddOrRemove(
				threadTabController.Control.HeaderInfo);
		}

		private void menuItemThreadResExtract_Click(object sender, System.EventArgs e)
		{
			ThreadExtract();
		}

		private void menuItemThreadReget_Click(object sender, System.EventArgs e)
		{
			ThreadReget();
		}

		private void menuItemThreadSirusiManager_Click(object sender, System.EventArgs e)
		{
			if (threadTabController.IsSelected)
			{
				SirusiDialog dlg = new SirusiDialog(threadTabController);
				dlg.ShowDialog(this);
			}
		}

		private void menuItemThreadAutoScroll_Click(object sender, System.EventArgs e)
		{
			ThreadSetAutoScroll(
				!threadTabController.Control.AutoScroll);
		}

		private void menuItemThreadAutoReload_Click(object sender, System.EventArgs e)
		{
			ThreadSetAutoReload(
				!threadTabController.Control.AutoReload);
		}

		private void menuItemThreadBookmark_Click(object sender, System.EventArgs e)
		{
			BookmarkSet(threadTabController.HeaderInfo);
		}

		private void menuItemScrollToTop_Click(object sender, System.EventArgs e)
		{
			threadTabController.Control.ScrollTo(ScrollPosition.Top);
		}

		private void menuItemScrollToBottom_Click(object sender, System.EventArgs e)
		{
			threadTabController.Control.ScrollTo(ScrollPosition.Bottom);		
		}

		private void menuItemThreadDeleteClose_Click(object sender, System.EventArgs e)
		{
			ThreadClose(true);
		}

		private void menuItemThreadClose_Click(object sender, System.EventArgs e)
		{
			ThreadClose(false);
		}

		private void menuItemThreadRedraw_Popup(object sender, System.EventArgs e)
		{
		
		}

		private void menuItemThreadAutoFocus_Click(object sender, System.EventArgs e)
		{
			settings.ThreadAutoFocus = !settings.ThreadAutoFocus;
		}

		#endregion

		#region ツールメニュー
		private void menuItemTools_Popup(object sender, System.EventArgs e)
		{
			while (menuItemToolsSub.DropDownItems.Count > 2)
				menuItemToolsSub.DropDownItems.RemoveAt(2);

			foreach (ToolItem item in tools)
			{
				ToolStripMenuItem menu = new ToolStripMenuItem(item.Name);
				menu.Click += new EventHandler(OnToolClick);
				menu.Tag = item;

				menuItemToolsSub.DropDownItems.Add(menu);
			}
		}

		private void OnToolClick(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			RunTool(menu.Tag as ToolItem, comboBoxTools.Text);
		}

		private void menuItemToolsInetOption_Click(object sender, System.EventArgs e)
		{
			Process.Start("inetcpl.cpl");
		}

		private void menuItemToolsOption_Click(object sender, System.EventArgs e)
		{
			ShowOption();
		}

		private void menuItemToolsScrapEditor_Click(object sender, System.EventArgs e)
		{
			ShowScrapEditor();
		}

		private void menuItemToolsImageViewer_Click(object sender, System.EventArgs e)
		{
			ImageViewerOpen(String.Empty);
		}

		private void menuItemToolsRegist_Click(object sender, System.EventArgs e)
		{
			ToolsEditorDialog dlg = new ToolsEditorDialog(tools);
			if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				SaveTools();

			UpdateToolsComboBox();
		}

		private void menuItemToolsSaveWindowUrls_Click(object sender, System.EventArgs e)
		{
			if (settings.OpenStartupUrls)
			{
				DialogResult r = MessageBox.Show("「起動時に終了前の状態を復元」オプションがOnに設定されています。\r\n" +
					"この機能と競合してしまうためOffにする必要があります。今すぐOffにしますか？", "確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (r == DialogResult.Yes)
					settings.OpenStartupUrls = false;
			}

			SaveWindowsUrl();
		}

		private void menuItemToolsOpenStartupUrls_Click(object sender, System.EventArgs e)
		{
			OpenStartup();
		}
		#endregion

		#region ウインドウメニュー
		private void menuItemWindow_Popup(object sender, System.EventArgs e)
		{
			menuItemWindowThreadCloseAll.Enabled = 
				menuItemWindowThreadCloseNotActive.Enabled = 
				menuItemWindowSelectNext.Enabled =
				menuItemWindowSelectPrev.Enabled = threadTabController.IsSelected;

			menuItemWindowListCloseAll.Enabled =
				menuItemWindowListCloseNotActive.Enabled = 
				menuItemWindowListPrev.Enabled =
				menuItemWindowListNext.Enabled = listTabController.IsSelected;
		}

		private void menuItemWindowThreadCloseAll_Click(object sender, System.EventArgs e)
		{
			ThreadCloseAll();
		}

		private void menuItemWindowThreadCloseNotActive_Click(object sender, System.EventArgs e)
		{
			ThreadCloseNotActive();
		}

		private void menuItemWindowListCloseAll_Click(object sender, System.EventArgs e)
		{
			ListCloseAll();
		}

		private void menuItemWindowListCloseNotActive_Click(object sender, System.EventArgs e)
		{
			ListCloseNotActive();
		}

		private void menuItemWindowSelectNext_Click(object sender, System.EventArgs e)
		{
			threadTabController.Select(true);
		}

		private void menuItemWindowSelectPrev_Click(object sender, System.EventArgs e)
		{
			threadTabController.Select(false);
		}

		private void menuItemWindowListPrev_Click(object sender, System.EventArgs e)
		{
			listTabController.Select(false);
		}

		private void menuItemWindowListNext_Click(object sender, System.EventArgs e)
		{
			listTabController.Select(true);
		}
		#endregion

		#region ヘルプメニュー
		private void menuItemHelp_Popup(object sender, System.EventArgs e)
		{
		
		}

		private void menuItemHelpOpen_Click(object sender, System.EventArgs e)
		{
			if (File.Exists(Settings.HelpFilePath))
				CommonUtility.OpenWebBrowser(Settings.HelpFilePath);

			else {
				MessageBox.Show("ヘルプファイルが存在しません", "twin2help.chm");
			}
		}

		private void menuItemHelpAbout_Click(object sender, System.EventArgs e)
		{
			// なぜかここで ImageViewURLReplace.dat を再読込
			IEComThreadBrowser.ImageViewUrlReplace.Refresh();

			AboutDialog about = new AboutDialog();
			about.ShowDialog(this);
		}

		private void menuItemHelpExit_Click(object sender, System.EventArgs e)
		{
//			if (MessageBox.Show("強制終了しますか？ (たぶん設定は保存されません)", "確認", 
//				MessageBoxButtons.YesNo) == DialogResult.Yes)
//			{
//				Application.Exit();
//			}
		}

		private void menuItemHelpOpenWeb_Click(object sender, System.EventArgs e)
		{
			CommonUtility.OpenWebBrowser(Settings.WebSiteUrl);
		}

		private void menuItemHelpOpenLoadFactor_Click(object sender, System.EventArgs e)
		{
			CommonUtility.OpenWebBrowser(Settings.LoadFactorUrl);
		}

		private void menuItemOpenServerWatch_Click(object sender, System.EventArgs e)
		{
			CommonUtility.OpenWebBrowser(Settings.ServerWatcherUrl);
		}


		private void menuItemHelpOpenServerWatch2_Click(object sender, EventArgs e)
		{
			CommonUtility.OpenWebBrowser(Settings.ServerWatcher2Url);
		}

		
		private void menuItemHelpTest_Click(object sender, System.EventArgs e)
		{

		}
		#endregion

		#region コンテキストメニュー

		#region スクロール
		private void contextMenuScroll_Popup(object sender, System.EventArgs e)
		{
			foreach (ToolStripItem menu in contextMenuScroll.Items)
				menu.Enabled = threadTabController.IsSelected;

			if (threadTabController.IsSelected)
			{
				menuItemScrollSetAutoScroll.Checked =
					threadTabController.Control.AutoScroll;

				menuItemScrollSetNewScroll.Checked =
					threadTabController.Control.ScrollToNewRes;
			}
		}

		private void menuItemScrollSetNewScroll_Click(object sender, System.EventArgs e)
		{
			threadTabController.Control.ScrollToNewRes =
				!threadTabController.Control.ScrollToNewRes;
		}

		private void menuItemScrollBack_Click(object sender, System.EventArgs e)
		{
			threadTabController.Control.ScrollTo(
				ScrollPosition.Prev);
		}
		#endregion

		#region 表示変更
		private void contextMenuViewChange_Popup(object sender, System.EventArgs e)
		{
			ThreadHeader h = threadTabController.HeaderInfo;
			int viewResCount = threadTabController.Control.ViewResCount;

			menuItemViewShiori.Enabled = h.Shiori > 0;
			menuItemRemoveShiori.Enabled = h.Shiori > 0;
			menuItemViewNewResOnly.Enabled = h.NewResCount > 0;
			menuItemSetLimitFirstXXX.Text = "    " + viewResCount;
			menuItemSetLimitLastXXX.Text = "    " + viewResCount;
			menuItemViewChangePrev.Text = String.Format("前の{0}レス", viewResCount);
			menuItemViewChangeNext.Text = String.Format("次の{0}レス", viewResCount);
		}

		private void menuItemSetLimitFirst_Click(object sender, System.EventArgs e)
		{
			if (!threadTabController.Control.IsReading)
			{
				ToolStripMenuItem menu = (ToolStripMenuItem)sender;
				threadTabController.Control.Range(1, Int32.Parse(menu.Text));
			}
			else MessageBox.Show("読み込み中にこの操作は出来ません");
		}

		private void menuItemSetLimitLast_Click(object sender, System.EventArgs e)
		{
			if (!threadTabController.Control.IsReading)
			{
				ToolStripMenuItem menu = (ToolStripMenuItem)sender;
				threadTabController.Control.ViewResCount = Int32.Parse(menu.Text);
			}
			else MessageBox.Show("読み込み中にこの操作は出来ません");
		}

		private void menuItemViewChangePrev_Click(object sender, System.EventArgs e)
		{
			if (!threadTabController.Control.IsReading)
			{
				ThreadControl thread = threadTabController.Control;
				thread.Range(RangeMovement.Back);
			}
			else MessageBox.Show("読み込み中にこの操作は出来ません");
		}

		private void menuItemViewChangeNext_Click(object sender, System.EventArgs e)
		{
			if (!threadTabController.Control.IsReading)
			{
				ThreadControl thread = threadTabController.Control;
				thread.Range(RangeMovement.Forward);
			}
			else MessageBox.Show("読み込み中にこの操作は出来ません");
		}

		private void menuItemViewAll_Click(object sender, System.EventArgs e)
		{
			if (!threadTabController.Control.IsReading)
			{
				ThreadControl thread = threadTabController.Control;
				thread.Range(1, -1);
			}
			else MessageBox.Show("読み込み中にこの操作は出来ません");		
		}

		private void menuItemViewNewResOnly_Click(object sender, System.EventArgs e)
		{
			if (!threadTabController.Control.IsReading)
			{
				ThreadHeader h = threadTabController.HeaderInfo;
				threadTabController.Control.Range(h.GotResCount - h.NewResCount + 1, -1);
			}
			else MessageBox.Show("読み込み中にこの操作は出来ません");
		}

		private void menuItemViewShiori_Click(object sender, System.EventArgs e)
		{
			if (!threadTabController.Control.IsReading)
			{
				threadTabController.Control.OpenBookmark();
			}
			else MessageBox.Show("読み込み中にこの操作は出来ません");
		}

		private void menuItemViewSirusi_Click(object sender, System.EventArgs e)
		{
			if (!threadTabController.Control.IsReading)
			{
				threadTabController.Control.OpenSirusi();
			}
			else MessageBox.Show("読み込み中にこの操作は出来ません");
		}

		private void menuItemRemoveShiori_Click(object sender, EventArgs e)
		{
			if (threadTabController.IsSelected)
			{
				threadTabController.Control.Bookmark(0);
			}
		}
		#endregion

		#region 更新
		private void contextMenuRead_Popup(object sender, System.EventArgs e)
		{
			foreach (ToolStripItem menu in contextMenuRead.Items)
				menu.Enabled = threadTabController.IsSelected;
		}

		private void menuItemReadReloadAll_Click(object sender, System.EventArgs e)
		{
			ThreadReloadAll();
		}
		#endregion

		#region レス番号
		private void contextMenuRes_Popup(object sender, System.EventArgs e)
		{
			foreach (ToolStripItem menu in contextMenuRes.Items)
				menu.Enabled = !threadTabController.Control.IsReading;

			menuItemResKokokara.Visible = (numberClickEventArgsSt == null) ? true : false;
			menuItemResKokomade.Visible = (numberClickEventArgsSt != null) ? true : false;
			
			menuItemResKokomade.Text = (numberClickEventArgsSt != null) ?
				String.Format("{0}からココまで(&E)", numberClickEventArgsSt.ResSet.Index) : "ココから連続して(&E)";

			// しおりの状態
			int shiori = threadTabController.HeaderInfo.Shiori;
			if (shiori == numberClickEventArgs.ResSet.Index)
			{
				menuItemResBookmark.Text = "ココまで読んだのを解除(&B)";
			}
			else
			{
				menuItemResBookmark.Text = "ココまで読んだの(&B)";
			}
		}

		private void menuItemResWrite_Click(object sender, System.EventArgs e)
		{
			ThreadPostRes(
				String.Format("{0}{1}\r\n",
				settings.ResRefAnchor,
				numberClickEventArgs.ResSet.Index)
			);
		}

		private void menuItemRefResWrite_Click(object sender, System.EventArgs e)
		{
			// 引用符を付加
			string data = HtmlTextUtility.HtmlToText(numberClickEventArgs.ResSet.Body);
			data = Regex.Replace(data, "^(.*)", "> ${0}", RegexOptions.Multiline);

			StringBuilder sb = new StringBuilder();
			sb.Append(settings.ResRefAnchor);
			sb.Append(numberClickEventArgs.ResSet.Index);
			sb.Append("\r\n");
			sb.Append(data);
			sb.Append("\r\n");

			ThreadPostRes(sb.ToString());
		}

		private void menuItemResCopyID_Click(object sender, EventArgs e)
		{
			Clipboard.SetDataObject(
				numberClickEventArgs.ResSet.ID, true);
		}


		private void menuItemResCopy_Click(object sender, System.EventArgs e)
		{
			Clipboard.SetDataObject(
				numberClickEventArgs.ResSet.ToString(PlainTextSkin.Default), true);
		}

		private void menuItemResRefCopy_Click(object sender, System.EventArgs e)
		{
			// 引用符を付加
			string data = numberClickEventArgs.ResSet.ToString(PlainTextSkin.Default);
			data = Regex.Replace(data, "^(.*)", "> ${0}", RegexOptions.Multiline);

			Clipboard.SetDataObject(data, true);
		}

		private void menuItemResCopyUrl_Click(object sender, System.EventArgs e)
		{
			Clipboard.SetDataObject(
				numberClickEventArgs.Header.Url + 
				numberClickEventArgs.ResSet.Index, true);		
		}

		private void menuItemResCopyNameUrl_Click(object sender, System.EventArgs e)
		{
			Clipboard.SetDataObject(numberClickEventArgs.Header.Subject + Environment.NewLine +
				numberClickEventArgs.Header.Url + numberClickEventArgs.ResSet.Index, true);		
		}

		private void menuItemResIDPopup_Click(object sender, System.EventArgs e)
		{
			AbstractExtractor extractor = threadTabController.Control.BeginExtract();
			extractor.NewWindow = settings.Search.ResExtract.Popup;
			extractor.InnerExtract(numberClickEventArgs.ResSet.ID, ResSetElement.ID);
		}

		private void menuItemResBackReference_Click(object sender, System.EventArgs e)
		{
			threadTabController.Control.PopupBackReferences(
				numberClickEventArgs.ResSet.Index);
		}

		private void menuItemResBookmark_Click(object sender, System.EventArgs e)
		{
			int shiori = numberClickEventArgs.ResSet.Index;
			threadTabController.Control.Bookmark(shiori);
		}

		private void menuItemResSirusi_Click(object sender, System.EventArgs e)
		{
			int sirusi = numberClickEventArgs.ResSet.Index;
			threadTabController.Control.Sirusi(sirusi, true);
		}

		private void menuItemResAddNG_Click(object sender, System.EventArgs e)
		{
			NGWords nGWords = Twinie.NGWords.Get(numberClickEventArgs.Header.BoardInfo, true);
			nGWords.ID.Add(numberClickEventArgs.ResSet.ID);
			IEComThreadBrowser b = (IEComThreadBrowser)threadTabController.Control;
			b.Redraw();
		}

		private void menuItemResOpenLinks_Click(object sender, System.EventArgs e)
		{
			LinkCollection links = numberClickEventArgs.ResSet.Links;
			OpenLinks(links);
		}

		private void menuItemResABone_Click(object sender, System.EventArgs e)
		{
			ResABoneInternal(true);
		}

		private void menuItemResHideABone_Click(object sender, System.EventArgs e)
		{
			ResABoneInternal(false);
		}
		
		#region ここから機能
		private void menuItemResKokokara_Click(object sender, System.EventArgs e)
		{
			numberClickEventArgsSt = numberClickEventArgs;
		}

		private void menuItemResKokoMadeWrite_Click(object sender, System.EventArgs e)
		{
			ThreadPostRes(
				String.Format("{0}{1}-{2}\r\n",
				settings.ResRefAnchor,
				numberClickEventArgsSt.ResSet.Index,
				numberClickEventArgs.ResSet.Index)
			);
			numberClickEventArgsSt = null;
		}

		private void menuItemKokoMadeCopy_Click(object sender, System.EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			ReadOnlyResSetCollection resSets = threadTabController.Control.ResSets;

			int st, ed;
			GetClickedRange(out st, out ed);
			
			for (int i = st; i <= ed; i++) {
				sb.Append(resSets[i-1].ToString(PlainTextSkin.Default));
				sb.Append("\r\n\r\n");
			}

			Clipboard.SetDataObject(sb.ToString(), true);
			numberClickEventArgsSt = null;
		}

		private void menuItemResKokoMadeRefCopy_Click(object sender, System.EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			ReadOnlyResSetCollection resSets = threadTabController.Control.ResSets;

			int st, ed;
			GetClickedRange(out st, out ed);
			
			for (int i = st; i <= ed; i++)
			{
				string data = resSets[i-1].ToString(PlainTextSkin.Default);
				sb.Append(Regex.Replace(data, "^(.*)", "> ${0}", RegexOptions.Multiline));
				sb.Append("\r\n\r\n");
			}

			Clipboard.SetDataObject(sb.ToString(), true);
			numberClickEventArgsSt = null;
		}

		private void menuItemResKokoMadeCopyUrl_Click(object sender, System.EventArgs e)
		{
			Clipboard.SetDataObject(
				numberClickEventArgs.Header.Url +
				numberClickEventArgsSt.ResSet.Index + "-" + numberClickEventArgs.ResSet.Index, true);		
			numberClickEventArgsSt = null;
		}

		private void menuItemResKokoMadeCopyNameUrl_Click(object sender, System.EventArgs e)
		{
			Clipboard.SetDataObject(
				numberClickEventArgs.Header.Subject + "\r\n" +
				numberClickEventArgs.Header.Url +
				numberClickEventArgsSt.ResSet.Index + "-" + numberClickEventArgs.ResSet.Index, true);		
			numberClickEventArgsSt = null;
		}

		private void menuItemResKokoMadeAddNGID_Click(object sender, System.EventArgs e)
		{
			ReadOnlyResSetCollection resSets = threadTabController.Control.ResSets;
			StringBuilder sb = new StringBuilder();

			int st, ed;
			GetClickedRange(out st, out ed);
			
			// 開かれているスレッドのNGワード設定を取得
			NGWords nGWords = 
				Twinie.NGWords.Get(numberClickEventArgsSt.Header.BoardInfo, true);

			for (int i = st; i <= ed; i++)
				nGWords.ID.Add(resSets[i-1].ID);

			numberClickEventArgsSt = null;
			((IEComThreadBrowser)threadTabController.Control).Redraw();
		}


		private void menuItemResKokoMadeLink_Click(object sender, EventArgs e)
		{
			ReadOnlyResSetCollection resSets = threadTabController.Control.ResSets;

			int st, ed;
			GetClickedRange(out st, out ed);

			for (int i = st; i <= ed; i++)
			{
				ResSet res = resSets[i-1];
				OpenLinks(res.Links);
			}
			numberClickEventArgsSt = null;
		}



		private void menuItemResKokoMadeABone_Click(object sender, System.EventArgs e)
		{
			ResABoneInternal(true);
			numberClickEventArgsSt = null;
		}

		private void menuItemResKokoMadeHideABone_Click(object sender, System.EventArgs e)
		{
			ResABoneInternal(false);
			numberClickEventArgsSt = null;
		}

		private void menuItemResKokoMadeCancel_Click(object sender, System.EventArgs e)
		{
			numberClickEventArgsSt = null;
		}

		/// <summary>
		/// クリックされたレス番号を取得
		/// </summary>
		/// <param name="st"></param>
		/// <param name="ed"></param>
		private void GetClickedRange(out int st, out int ed)
		{
			if (numberClickEventArgsSt != null)
			{
				int a1 = numberClickEventArgsSt.ResSet.Index;
				int a2 = numberClickEventArgs.ResSet.Index;

				st = Math.Min(a1, a2);
				ed = Math.Max(a1, a2);
			}
			else {
				st = ed = numberClickEventArgs.ResSet.Index;
			}
		}
		#endregion

		#region ResABoneInternal
		private void ResABoneInternal(bool visible)
		{
			try {
				if (threadTabController.Control.IsReading)
				{
					MessageBox.Show("読み込み中にこの操作は出来ません");
					return;
				}

				int st, ed;
				GetClickedRange(out st, out ed);

				int[] indices = new int[ed - st + 1];
				for (int i = st; i <= ed; i++) indices[i - st] = i;

				cache.ResABone(numberClickEventArgs.Header, indices, visible);
				ThreadOpen(numberClickEventArgs.Header, false);
			}
			catch (Exception ex) {
				TwinDll.ShowOutput(ex);
			}
		}
		#endregion

		#endregion

		#region 板一覧
		private void contextMenuTable_Popup(object sender, System.ComponentModel.CancelEventArgs e)
		{
			TreeNode node = tableView.GetNodeAt(
				tableView.PointToClient(MousePosition));

			bool enabled = node != null && node.Tag is BoardInfo;

			foreach (ToolStripItem menu in contextMenuTable.Items)
				menu.Enabled = enabled;

			menuItemTableSetItaBotan.Enabled = node != null;

			if (node != null)
			{
				menuItemTableSetItaBotan.Checked = ItaBotanFind(node.Tag) != null;
			}
		}


		private void menuItemTableShowSettingTxt_Click(object sender, EventArgs e)
		{
			tableView.ShowSettingTxt(tableView.SelectedItem);
		}

		private void menuItemTableNewOpen_Click(object sender, EventArgs e)
		{
			if (TableInterface.IsSelected)
				ListOpen(TableInterface.Selected, true);
		}

		private void menuItemTableSetItaBotan_Click(object sender, System.EventArgs e)
		{
			if (tableView.SelectedNode != null)
			{
				object obj = tableView.SelectedNode.Tag;
				ItaBotanSet(obj);
			}
		}

		private void menuItemTableCopyURL_Click(object sender, EventArgs e)
		{
			if (TableInterface.IsSelected)
			{
				ClipboardUtility.Copy(
					TableInterface.Selected, CopyInfo.Url);
			}
		}

		private void menuItemTableCopyURLName_Click(object sender, EventArgs e)
		{
			if (TableInterface.IsSelected)
			{
				ClipboardUtility.Copy(
					TableInterface.Selected, CopyInfo.Name | CopyInfo.Url);
			}
		}

		private void menuItemTableOpenWebBrowser_Click(object sender, EventArgs e)
		{
			if (TableInterface.IsSelected)
				CommonUtility.OpenWebBrowser(TableInterface.Selected.Url);
		}

		private void menuItemTableShowLocalRule_Click(object sender, EventArgs e)
		{
			tableView.ShowLocalRule(
				tableView.SelectedItem);
		}

		private void menuItemTableShowPicture_Click(object sender, EventArgs e)
		{
			tableView.ShowPicture(
				tableView.SelectedItem);
		}

		private void menuItemTableDeleteLog_Click(object sender, EventArgs e)
		{
			// tableView.DeleteLog();
			CacheClear(tableView.SelectedItem);
		}
		#endregion

		#region スレッド一覧
		private void contextMenuListView_Popup(object sender, System.EventArgs e)
		{
			bool enable = listTabController.Control.SelectedItems.Count > 0;
			foreach (ToolStripItem menu in contextMenuListView.Items)
				menu.Enabled = enable;

			if (enable)
			{
				ThreadHeader header = listTabController.Control.SelectedItems[0];
				// dat落ちログであればチェックを付ける
				if (ThreadIndexer.Read(cache, header) != null)
					menuItemListPastlog.Checked = header.Pastlog;

				menuItemListUpdateCheck.Checked = threadUpdateChecker.IsContains(header);
				menuItemListSetBookmark.Checked = IsBookmarked(header);
			}
			else {
				menuItemListUpdateCheck.Checked = false;
				menuItemListPastlog.Checked = false;
				menuItemListSetBookmark.Checked = false;
			}
		}


		private void menuItemListSearchNext_Click(object sender, EventArgs e)
		{
			BeginNextThreadCheck(listTabController.Control.SelectedItems[0]);
		}

		private void menuItemListOpenNewTab_Click(object sender, System.EventArgs e)
		{
			ReadOnlyCollection<ThreadHeader> items =
				listTabController.Control.SelectedItems;

			foreach (ThreadHeader header in items)
				ThreadBeforeOpen(header, true);
		}

		private void menuItemListHeadPopup_Click(object sender, System.EventArgs e)
		{
			try {
				if (listTabController.Control.SelectedItems.Count > 0)
				{
					PopupTest.Popup1(cache, listTabController.Control.SelectedItems);
				}
			}
			catch (Exception ex) {
				TwinDll.ShowOutput(ex);
			}
		}


		private void menuItemListTabRefresh_Click(object sender, EventArgs e)
		{
			ListReload();
		}

		private void menuItemListNewResPopup_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (listTabController.Control.SelectedItems.Count > 0)
				{
					PopupTest.PopupNewRes(cache, listTabController.Control.SelectedItems);
				}
			}
			catch (Exception ex)
			{
				TwinDll.ShowOutput(ex);
			}
		}

		private void menuItemListCopyURL_Click(object sender, System.EventArgs e)
		{
			ReadOnlyCollection<ThreadHeader> items = listTabController.Control.SelectedItems;
			ClipboardUtility.Copy(items, CopyInfo.Url);
		}

		private void menuItemListCopyURLName_Click(object sender, System.EventArgs e)
		{
			ReadOnlyCollection<ThreadHeader> items = listTabController.Control.SelectedItems;
			ClipboardUtility.Copy(items, CopyInfo.Url | CopyInfo.Name);
		}

		private void menuItemListCopyName_Click(object sender, System.EventArgs e)
		{
			ReadOnlyCollection<ThreadHeader> items = listTabController.Control.SelectedItems;
			ClipboardUtility.Copy(items, CopyInfo.Name);		
		}

		private void menuItemListSetBookmark_Click(object sender, System.EventArgs e)
		{
			foreach (ThreadHeader header in listTabController.Control.SelectedItems)
				BookmarkSet(header);
		}

		private void menuItemListSetWarehouse_Click(object sender, System.EventArgs e)
		{
			List<ThreadHeader> removed = new List<ThreadHeader>();

			foreach (ThreadHeader header in listTabController.Control.SelectedItems)
			{
				if (bookmarkRoot.Contains(header))
					bookmarkView.RemoveBookmark(header);

				warehouseView.AddBookmarkRoot(new BookmarkThread(header));
				removed.Add(header);
			}

			listTabController.Control.RemoveItems(removed);
		}

		private void menuItemListOpenWebBrowser_Click(object sender, System.EventArgs e)
		{
			foreach (ThreadHeader header in listTabController.Control.SelectedItems)
				CommonUtility.OpenWebBrowser(header.Url);
		}

		private void menuItemListABone_Click(object sender, System.EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			ReadOnlyCollection<ThreadHeader> items = listTabController.Control.SelectedItems;
			
			foreach (ThreadHeader header in items)
			{
				NGWords nGWords = Twinie.NGWords.Get(header.BoardInfo, true);
				nGWords.Subject.Add(header.Subject);
			}
		}

		private void menuItemListPastlog_Click(object sender, System.EventArgs e)
		{
			ReadOnlyCollection<ThreadHeader> items = listTabController.Control.SelectedItems;
			foreach (ThreadHeader header in items)
			{
				if (ThreadIndexer.Read(cache, header) != null)
				{
					// dat落ちしていればフラグを外す、dat落ちしていなければフラグを設定
					header.Pastlog = !header.Pastlog;

					if (header.BoardInfo.Bbs == BbsType.X2chKako)
						header.BoardInfo.Bbs = BbsType.X2ch;

					ThreadIndexer.Write(cache, header);
					listTabController.Control.UpdateItem(header);
				}
			}
		}

		private void menuItemListDeleteLog_Click(object sender, System.EventArgs e)
		{
			foreach (ThreadHeader header in listTabController.Control.SelectedItems)
				ThreadDelete(header);
		}

		private void menuItemListUpdateCheck_Click(object sender, System.EventArgs e)
		{
			foreach (ThreadHeader header in listTabController.Control.SelectedItems)
				threadUpdateChecker.AddOrRemove(header);
		}
		#endregion

		#region お気に入り
		private void contextMenuBookmarkFolder_Popup(object sender, System.EventArgs e)
		{
			foreach (ToolStripItem menu in contextMenuBookmarkFolder.Items)
				menu.Enabled = bookmarkView.SelectedFolder != null;

			// 巡回中はメニュー項目を無効にする
			menuItemBookmarkPatrol.Enabled =
				menuItemBookmarkUpdateCheck.Enabled = (patroller == null) ? true : false;

			menuItemBookmarkSetItabotan.Checked =
				ItaBotanFind(bookmarkView.SelectedEntry) != null;
		}

		private void menuItemBookmarkOpen_Click(object sender, System.EventArgs e)
		{
			BookmarkOpen(bookmarkView.SelectedFolder, false);
		}

		private void menuItemBookmarkOpenIncludeSubChildren_Click(object sender, System.EventArgs e)
		{
			BookmarkOpen(bookmarkView.SelectedFolder, true);
		}

		private void menuItemBookmarkToWareHouse_Click(object sender, System.EventArgs e)
		{
			warehouseView.AddBookmarkRoot(bookmarkView.SelectedEntry.Clone());
			bookmarkView.RemoveSelected(false);
		}

		// 更新チェック
		private void menuItemBookmarkUpdateCheck_Click(object sender, System.EventArgs e)
		{
			BookmarkPatrol(true, true);
		}

		// 巡回
		private void menuItemBookmarkPatrol_Click(object sender, System.EventArgs e)
		{
			BookmarkPatrol(false, true);		
		}

		private void menuItemBookmarkNewFolder_Click(object sender, System.EventArgs e)
		{
			bookmarkView.NewFolder();
		}

		private void menuItemBookmarkRename_Click(object sender, System.EventArgs e)
		{
			bookmarkView.Rename();
		}

		private void menuItemBookmarkSort_Click(object sender, System.EventArgs e)
		{
			bookmarkView.SortChildren();
		}

		private void menuItemBookmarkRemove_Click(object sender, System.EventArgs e)
		{
			bookmarkView.RemoveSelected(false);
			UpdateToolBar();
		}

		private void menuItemBookmarkSetItabotan_Click(object sender, System.EventArgs e)
		{
			BookmarkEntry entry = bookmarkView.SelectedEntry;
			ItaBotanSet(entry);
		}

		// スレッド選択時のコンテキストメニュー
		private void contextMenuBookmarkItem_Popup(object sender, System.EventArgs e)
		{
			foreach (ToolStripItem menu in contextMenuBookmarkItem.Items)
				menu.Enabled = bookmarkView.SelectedThread != null;
		}

		private void menuItemBookmarkNewOpen_Click(object sender, System.EventArgs e)
		{
			ThreadOpen(bookmarkView.SelectedThread.HeaderInfo, true);
		}
		#endregion

		#region 過去ログ倉庫
		private void contextMenuWareHouse_Popup(object sender, System.EventArgs e)
		{
			foreach (ToolStripItem menu in contextMenuWareHouseFolder.Items)
				menu.Enabled = warehouseView.SelectedFolder != null;

			menuItemBookmarkSetItabotan.Checked =
				ItaBotanFind(warehouseView.SelectedEntry) != null;
		}

		private void menuItemWareHouseOpen_Click(object sender, System.EventArgs e)
		{
			BookmarkOpen(warehouseView.SelectedFolder, false);
		}

		private void menuItemWareHouseOpenIncludeSubChildren_Click(object sender, System.EventArgs e)
		{
			BookmarkOpen(warehouseView.SelectedFolder, true);
		}

		private void menuItemWareHouseNewFolder_Click(object sender, System.EventArgs e)
		{
			warehouseView.NewFolder();
		}

		private void menuItemWareHouseSort_Click(object sender, System.EventArgs e)
		{
			warehouseView.SortChildren();
		}

		private void menuItemWareHouseFolderRename_Click(object sender, System.EventArgs e)
		{
			warehouseView.Rename();
		}

		private void menuItemWareHouseRemove_Click(object sender, System.EventArgs e)
		{
			warehouseView.RemoveSelected(false);
			UpdateToolBar();
		}

		private void menuItemWareHouseSetItabotan_Click(object sender, System.EventArgs e)
		{
			BookmarkEntry entry = warehouseView.SelectedEntry;
			ItaBotanSet(entry);
		}

		// スレッド選択時のコンテキストメニュー
		private void contextMenuWareHouseItem_Popup(object sender, System.EventArgs e)
		{
			foreach (ToolStripItem menu in contextMenuBookmarkItem.Items)
				menu.Enabled = warehouseView.SelectedThread != null;
		}

		private void menuItemWareHouseNewOpen_Click(object sender, System.EventArgs e)
		{
			ThreadOpen(warehouseView.SelectedThread.HeaderInfo, true);
		}
		#endregion

		#region 板ボタン
		private CSharpToolBarButton tempButton;

		private void contextMenuItaBotan_Popup(object sender, System.EventArgs e)
		{
			Point pt = cSharpToolBar.PointToClient(MousePosition);
			tempButton = cSharpToolBar.ButtonFromPoint(pt.X, pt.Y);
			
			if (tempButton == null)
				return;

			menuItemItaBotanRemove.Enabled = !tempButton.Tag.Equals(allTable);
		}

		private void menuItemItaBotanRemove_Click(object sender, System.EventArgs e)
		{
			if (tempButton != null)
				ItaBotanRemove(tempButton);
		}
		#endregion

		#region スレッド一覧タブ
		private void contextMenuListTab_Popup(object sender, System.EventArgs e)
		{
			foreach (ToolStripItem menu in contextMenuListTab.Items)
				menu.Enabled = listTabController.IsSelected;

			menuItemListTabUpdateCheck.Enabled = 
				IsUpdateCheckable(listTabController.HeaderInfo) && patroller == null;
		}

		private void menuItemListTabClose_Click(object sender, System.EventArgs e)
		{
			ListClose();
		}

		private void menuItemListTabClose2_Click(object sender, System.EventArgs e)
		{
			ListCloseNotActive();
		}

		private void menuItemListTabCloseAll_Click(object sender, System.EventArgs e)
		{
			ListCloseAll();
		}

		private void menuItemListTabOpenUpThreads_Click(object sender, System.EventArgs e)
		{
			ListOpenUpThreads();
		}

		private void menuItemListTabOpenWebBrowsre_Click(object sender, System.EventArgs e)
		{
			CommonUtility.OpenWebBrowser(listTabController.HeaderInfo.Url);
		}

		private void menuItemListTabCacheOpen_Click(object sender, System.EventArgs e)
		{
			CacheOpen(listTabController.HeaderInfo);
		}

		private void menuItemListTabCacheClear_Click(object sender, System.EventArgs e)
		{
			CacheClear(listTabController.HeaderInfo);
		}


		// お気に入り、検索結果、履歴
		private void menuItemListTabUpdateCheck_Click(object sender, System.EventArgs e)
		{
			ReadOnlyCollection<ThreadHeader> items = listTabController.Control.Items;

			List<ThreadHeader> list = new List<ThreadHeader>(items);
			BookmarkPatrol(list, true);
		}

		private void menuItemListTabWithout1000Res_Click(object sender, EventArgs e)
		{
			menuItemListTab_RemoveItems(new Predicate<ThreadHeader>(delegate (ThreadHeader h)
			{
				return h.IsLimitOverThread;
			}));
		}

		private void menuItemListTabWithoutPastlog_Click(object sender, EventArgs e)
		{
			menuItemListTab_RemoveItems(new Predicate<ThreadHeader>(delegate(ThreadHeader h)
			{
				return h.Pastlog;
			}));
		}

		private void menuItemListTabWithoutKakolog_Click(object sender, EventArgs e)
		{
			menuItemListTab_RemoveItems(new Predicate<ThreadHeader>(delegate(ThreadHeader h)
			{
				return warehouseRoot.Contains(h);
			}));
		}

		private void menuItemListTab_RemoveItems(Predicate<ThreadHeader> match)
		{
			List<ThreadHeader> buff = new List<ThreadHeader>(listTabController.Control.Items);

			List<ThreadHeader> removeItems = buff.FindAll(match);

			listTabController.Control.RemoveItems(removeItems);
		}


		private void menuItemListTabCopyURL_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(listTabController.Control.HeaderInfo.Url);
		}

		private void menuItemListTabCopyNameURL_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(
				listTabController.Control.HeaderInfo.Name + Environment.NewLine + listTabController.Control.HeaderInfo.Url);
		}

		#endregion

		#region スレッドタブ
		private void contextMenuThreadTab_Popup(object sender, System.EventArgs e)
		{
			foreach (ToolStripItem menu in contextMenuThreadTab.Items)
				menu.Enabled = threadTabController.IsSelected;

			if (threadTabController.IsSelected)
			{
				DraftBox box = new DraftBox(cache);
				menuItemThreadTabOpenDraft.Enabled = box.Load(threadTabController.HeaderInfo) != null;
				menuItemThreadTabNextThreadCheck.Enabled = nextThreadChecker == null;
			}
		}

		private void menuItemThreadTabOpenDraft_Click(object sender, System.EventArgs e)
		{
			DraftBox box = new DraftBox(cache);
			Draft draft = box.Load(threadTabController.HeaderInfo);

			if (draft != null)
				DraftEdit(draft);
		}

		private void menuItemThreadTabClose_Click(object sender, System.EventArgs e)
		{
			ThreadClose(false);
		}

		private void menuItemThreadTabClose2_Click(object sender, System.EventArgs e)
		{
			ThreadCloseNotActive();
		}

		private void menuItemThreadTabCloseLeft_Click(object sender, EventArgs e)
		{
			ThreadCloseLeft();
		}

		private void menuItemThreadTabCloseRight_Click(object sender, EventArgs e)
		{
			ThreadCloseRight();
		}

		private void menuItemThreadTabCloseAll_Click(object sender, System.EventArgs e)
		{
			ThreadCloseAll();
		}

		private void menuItemThreadTabCopyURL_Click(object sender, System.EventArgs e)
		{
			ClipboardUtility.Copy(
				threadTabController.HeaderInfo, CopyInfo.Url);
		}

		private void menuItemThreadTabCopyURLAndName_Click(object sender, System.EventArgs e)
		{
			ClipboardUtility.Copy(
				threadTabController.HeaderInfo, CopyInfo.Url | CopyInfo.Name);	
		}

		private void menuItemThreadTabCopyName_Click(object sender, System.EventArgs e)
		{
			ClipboardUtility.Copy(
				threadTabController.HeaderInfo, CopyInfo.Name);		
		}

		private void menuItemThreadTabOpenWebBrowser_Click(object sender, System.EventArgs e)
		{
			CommonUtility.OpenWebBrowser(
				threadTabController.HeaderInfo.Url);
		}

		private void menuItemThreadTabNewThread_Click(object sender, System.EventArgs e)
		{
			ThreadHeader h = threadTabController.HeaderInfo;
			Twin.PostThread thread = StringUtility.GetThreadTemplate(h);
			PostThread(h.BoardInfo, thread);
		}

		private void menuItemThreadTabDelClose_Click(object sender, System.EventArgs e)
		{
			ThreadClose(true);
		}

		private void menuItemThreadNextThreadCheck_Click(object sender, System.EventArgs e)
		{
			BeginNextThreadCheck(threadTabController.HeaderInfo);		
		}

		private void menuItemThreadTabReload_Click(object sender, EventArgs e)
		{
			ThreadReload();
		}

		private void menuItemThreadTabReget_Click(object sender, EventArgs e)
		{
			ThreadReget();
		}


		private void toolStripMenuItemSaveImages_Click(object sender, EventArgs e)
		{
			PreFolderBrowserDialog dlg = new PreFolderBrowserDialog();
			dlg.SetDefaultPathChecked = settings.RefFolderBrowserDialog_SetDefaultPathChecked;
			dlg.SelectedPath = settings.LastSelectedDirectoryPath;

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				if (dlg.SetDefaultPathChecked)
					settings.LastSelectedDirectoryPath = dlg.SelectedPath;

				List<string> list = new List<string>();
				int bookmark = threadTabController.Control.HeaderInfo.Shiori;

				foreach (ResSet r in threadTabController.Control.ResSets)
				{
					// しおりが設定されている場合、しおり以降のレスのみ処理するため
					if (bookmark > 0 && r.Index <= bookmark)
						continue;

					list.AddRange(r.Links["jpg|jpeg|gif|png|bmp|jpg.html"]);
				}

				Download d = new Download(dlg.SelectedPath, list.ToArray());
				d.Show();
			}
		}


		private void menuItemThreadTabAllOpenImageViewer_Click(object sender, EventArgs e)
		{
			var list = new List<ResSet>();
			int bookmark = threadTabController.Control.HeaderInfo.Shiori;
			foreach (ResSet r in threadTabController.Control.ResSets)
			{
				// しおりが設定されている場合、しおり以降のレスのみ処理するため
				if (bookmark > 0 && r.Index <= bookmark) continue;
				list.Add(r);
			}
			ImageViewer_OpenImageAsync(threadTabController.HeaderInfo, list.ToArray(), false);
		}


		#endregion



		#region グループ

		void miGroup_DropDownOpening(object sender, EventArgs e)
		{
			while (miGroup.DropDownItems.Count > 3)
				miGroup.DropDownItems.RemoveAt(3);

			foreach (ThreadGroup group in threadGroupList)
			{
				ToolStripMenuItem item = new ToolStripMenuItem();
				item.Text = group.Name;
				item.Tag = group;
				item.DropDownOpening += new EventHandler(item_DropDownOpening);

				item.DropDownItems.Add("dummy");

				miGroup.DropDownItems.Add(item);
			}
		}

		void upcheck_Click(object sender, EventArgs e)
		{
			if (!bookmarkMenu_IsUpdateCheckEnabled())
				return;

			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			ThreadGroup g = (ThreadGroup)menu.Tag;

			BookmarkPatrol(g.ThreadList.Items, true);
		}

		void openmenu_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			ThreadGroup g = (ThreadGroup)menu.Tag;

			foreach (ThreadHeader h in g.ThreadList.Items)
				ThreadOpen(h, true);
		}

		void item_DropDownOpening(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			ThreadGroup g = (ThreadGroup)menu.Tag;

			menu.DropDownItems.Clear();

			ToolStripMenuItem openmenu = new ToolStripMenuItem();
			openmenu.Text = "すべて開く(&O)";
			openmenu.Click += new EventHandler(openmenu_Click);
			openmenu.Tag = g;

			ToolStripMenuItem upcheck = new ToolStripMenuItem();
			upcheck.Text = "更新チェック(&U)";
			upcheck.Click += new EventHandler(upcheck_Click);
			upcheck.Enabled = bookmarkMenu_IsUpdateCheckEnabled();
			upcheck.Tag = g;

			menu.DropDownItems.Add(openmenu);
			menu.DropDownItems.Add(upcheck);
			menu.DropDownItems.Add(new ToolStripSeparator());

			foreach (ThreadHeader h in g.ThreadList.Items)
			{
				ToolStripMenuItem item = new ToolStripMenuItem();
				item.Text = h.Subject;
				item.Tag = h;
				item.Click += new EventHandler(item_Click);

				menu.DropDownItems.Add(item);
			}
		}

		void item_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			ThreadOpen((ThreadHeader)menu.Tag, true);
		}

		private void miGroupEdit_Click(object sender, EventArgs e)
		{
			GroupEditorDialog dlg = new GroupEditorDialog(ref threadGroupList);
			dlg.ShowDialog(this);
			SaveThreadGroupList();
		}

		private void miGroupAdd_Click(object sender, EventArgs e)
		{
			List<ThreadHeader> list = new List<ThreadHeader>();

			foreach (ThreadControl c in threadTabController.GetControls())
				list.Add(c.HeaderInfo);

			GroupAddDialog dlg = new GroupAddDialog(threadGroupList, list);
			
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				// 同名のグループが存在したらそれに追加する
				foreach (ThreadGroup gp in threadGroupList)
				{
					if (String.Compare(gp.Name, dlg.FileName, true) == 0)
					{
						gp.ThreadList.Items.AddRange(dlg.CheckedItems);
						gp.Save();
						return;
					}
				}

				ThreadGroup newGroup = new ThreadGroup(cache, Path.Combine(Settings.GroupFolderPath, dlg.FileName + ".grp"));
				newGroup.ThreadList.Items.AddRange(dlg.CheckedItems);
				newGroup.Save();

				threadGroupList.Add(newGroup);
				SaveThreadGroupList();
			}

		}
		#endregion



		private void menuItemThreadTabColoring_Click(object sender, EventArgs e)
		{
			TabColorChangeDialog dlg = new TabColorChangeDialog(
				threadTabController.Window.ColorSet);

			string subj = threadTabController.HeaderInfo.Subject;
			dlg.TabText = subj;

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				tabColorTable.Set(subj, dlg.ColorSet);
				tabColorTable.Save();

				threadTabController.Window.ColorSet = dlg.ColorSet;
				threadTabCtrl.Refresh();
			}
		}

		private void menuItemListTabColoring_Click(object sender, EventArgs e)
		{
			TabColorChangeDialog dlg = new TabColorChangeDialog(
				listTabController.Window.ColorSet);

			string boardName = listTabController.HeaderInfo.Name;
			dlg.TabText = boardName;

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				tabColorTable.Set(boardName, dlg.ColorSet);
				listTabController.Window.ColorSet = dlg.ColorSet;
				listTabCtrl.Refresh();

				if (MessageBox.Show("現在開いている " + boardName + "板 のスレッドタブも同じ色に設定しますか？", "確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					tabColorTable.Set(boardName + "*", dlg.ColorSet);

					foreach (TwinWindow<ThreadHeader, ThreadControl> window in threadTabController.GetWindows())
					{
						ThreadHeader h = window.Control.HeaderInfo;

						if (h.BoardInfo.Name == boardName)
						{
							window.ColorSet = dlg.ColorSet;
						}
					}
					threadTabCtrl.Refresh();
				}
				tabColorTable.Save();
			}
		}


		#endregion

		private void menuItemFileLogIndexing_Click(object sender, EventArgs e)
		{
			DatIndexerDialog dlg = new DatIndexerDialog(this.cache);
			dlg.ShowDialog();
		}

		private void menuItemPatrolHiddenPastlog_Click(object sender, EventArgs e)
		{
			settings.Patrol_HiddenPastlog = !settings.Patrol_HiddenPastlog;
		}

		private void menuItemThreadResetPastlogFlags_Click(object sender, EventArgs e)
		{
			ThreadResetPastlogFlags();
		}

		private void menuItemViewPatrol_DropDownOpening(object sender, EventArgs e)
		{
			menuItemPatrolHiddenPastlog.Checked = settings.Patrol_HiddenPastlog;
		}

		private void miMouseGestureSetting_Click(object sender, EventArgs e)
		{
			ShowMouseGestureSetting();
		}


		private void menuItemNotifyIconExit_Click(object sender, EventArgs e)
		{
			Close();
		}


		private void menuItemHelpOpenErrorLog_Click(object sender, EventArgs e)
		{
			Process.Start(Settings.ErrorLogPath);
		}



		private void menuItemToolOyster_DropDownOpening(object sender, EventArgs e)
		{
			menuItemToolOysterEnable.Enabled =
				X2chAuthenticator.IsValidUsernamePassword(settings.Authentication.Username, settings.Authentication.Password);

			menuItemToolOysterEnable.Checked = settings.Authentication.AuthenticationOn;
			menuItemToolOysterDisable.Checked = !settings.Authentication.AuthenticationOn;
		}

		private void menuItemToolOysterEnable_Click(object sender, EventArgs e)
		{
			OysterLogon();
		}

		private void menuItemToolOysterDisable_Click(object sender, EventArgs e)
		{
			OysterLogout();
		}


		private void menuItemAutoImageOpen_Click(object sender, EventArgs e)
		{
			var win = threadTabController.Window;
			win.AutoImageOpen = !win.AutoImageOpen;
		}

		private void contextMenuStripAutoReload_Opening(object sender, CancelEventArgs e)
		{
			if (threadTabController.IsSelected)
			{
				var win = threadTabController.Window;
				menuItemAutoImageOpen.Enabled = true;
				menuItemAutoImageOpen.Checked = win.AutoImageOpen;
			}
			else
			{
				menuItemAutoImageOpen.Enabled = false;
				menuItemAutoImageOpen.Checked = false;
			}
		}

	}
}
