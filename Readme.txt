
　２ちゃんねる専用ブラウザ
　ついんてーる ver2.5.1

　配布URL : http://www.geocities.jp/nullpo0/
　更新日: 2013/9/26

《動作環境》
	WindowsXP、Vista, 7
	(64bitでも動作すると思います)
	(Windows 8 は持っていないので動作未確認です)

《使用する前に》
	このソフトを動作させるには .NET Framework 4.0 再頒布パッケージが必要です。
	はじめにインストールしておいてください。

	できれば Windows Update で環境を最新の状態に更新してからご使用ください。


《.NET Frameworkダウンロード》
	Windows Update または以下の URL からダウンロードしてください。

	.NET Framework4.0 (Windows7, Vista, XPの人)
	http://www.microsoft.com/downloads/ja-jp/details.aspx?FamilyID=9cfb2d51-5ff4-4491-b0e5-b386f32c0992

《注意》
　	このソフトはフリーソフトです。
　	使用する場合、自己責任で使用してください。


《初回インストール》
	書庫はLha圧縮されています。
	適当なフォルダに解凍後、twintail.exe を起動させてください。


《バージョンアップ版のインストール》
	解凍して出てきた"すべて"のファイルを前回のバージョンに上書きしてください。


《アンインストール》
	解凍して出てきたフォルダごと削除してください。
	レジストリは使用してません。


《外部板登録の方法》

	外部板を登録する場合は、編集メニューから「外部板を登録」を選択してください。

	例 (したらばJBBS)
	　ホストアドレス：　jbbs.livedoor.jp
	　板のアドレス：　computer/351

	例 (まちBBS)
	　ホストアドレス：　kanto.machi.to
	　板のアドレス：　tokyo

	例 (実況避難所などcgi-binとかがある場合)
	　ホストアドレス： 24h.musume.org/cgi-bin
	　板のアドレス：　live


《トリップ付きコテハンをNGワードに登録する方法》

	たとえば、「てつと ◆/RtqBUGz8o」 を登録する場合は、トリップ部分のみ "◆/RtqBUGz8o" を書き込んでください。
	どうしてもコテハンとトリップを一緒に登録したい場合は、「てつと </b>◆/RtqBUGz8o」という感じに、</b>を付け加えてください。(名前の後の半角スペースも忘れずに)

《発言回数に応じたIDの色分け》v2.2以降

	スキンに <COLORINGID/> を使用すると、発言回数に応じて色づけを行った ID を出力します。
	デフォルトでは 2 回以上は紫、5 回以上は青、10 回以上は 黄緑、15 回以上は オレンジ、20 回以上は ピンク、30 回以上は赤です。
	回数や色はスキンで指定することができます。

	例1（色名で指定）
	5 回以上は 青、10 回以上は 赤 のサンプル。
	<COLORINGID n5=blue n10=red/>

	例2（16進数で指定）
	<COLORINGID n5=#0000FF n10=#FF0000/>

	ただし、<DATE/> キーワードと一緒に使用すると ID が 2重に出力されてしまうので、
	<DATEONLY/> キーワードを使用してください。
	
	Res.html と NewRes.html で使用可能です。


《前回終了時から1日経過したらNGIDを削除》v2.2以降

	NGID がどんどん増えていってしまうのを防ぐために、
	日付が変わるごとに NGID のみを削除できるようにしました。

	まず twintail を一度起動し、何もせずに終了します。
	twintail と同じフォルダにある twin.xml をエディタで開きます。
	すると <NGIDAutoClear>false</NGIDAutoClear> という項目が追加されているので、
	false を true に書き換えて保存してください。

	前回終了時から日付が変わっている場合、twintail 起動後に NGID を自動でクリアします。
	削除するタイミングは起動後に初めてスレッドを開いたときなので、起動しただけでは削除されません。

《不具合》
	バグを発見したら以下の情報と一緒に twintail スレに報告してください。

	OS (例: Windows 2000)
	twintail のバージョン (例: 2.1)
	ファイアーウォール (例: ZoneAlarm)
	再現方法
	エラー内容

	twintailを終了すると、twin.logにエラー内容が保存されます。
	もしあれば、それをコピペしてもらえると原因が分かるかもしれません。

	※報告時の注意
	　エラーが出る場合は、そのエラーの内容を書いてください。
	　板やスレが読めなかったり書き込めない場合は、対象のアドレスも一緒に書いてください。

《更新履歴》
	history.txt に書いてあります。

《バグ修正＆お手伝いして頂いた》

	X Dev ◆.GNMmP6FqY さん
	某ソレ47 ◆ap/yuix/tw さん
	水玉 ◆qHK1vdR8FRIm さん
	アイコンを作ってくれた方

	ありがとうございました。
	とっても助かります。

《開発環境／動作確認》
	Prius 300P
	OS    Windows2000 SP4
	CPU   AMD-450MHz
	メモリ 184MB
	Microsoft Visual C# 2005 Express Edition
	Internet Explorer 6.0

	Prius PN35K5U
	OS    WindowsVista SP2
	CPU   Core2 Duo T5500
	メモリ 1GB
	Microsoft Visual C# 2010 Express Edition
	Internet Explorer 9.0

	OS    Windows 7 SP1
	CPU   Athlon 64 X2 3600+
	メモリ 4GB
	Microsoft Visual C# 2010 Express Edition
	Internet Explorer 9.0


《転載・再配布》
	ご自由にどうぞ。
	作者への連絡は不要です。

《関連リンク》

	twintail wiki
	http://www22.atpages.jp/~moccos/tt/index.php?FrontPage

	twintail wiki
	http://nullpo0.hp.infoseek.co.jp/cgi-bin/wiki/wiki.cgi

	twintail v2 wiki （管理者行方不明）
	http://secilia.s33.xrea.com/twin2wiki/

	２ちゃんねるブラウザ OpenTwin based on twintail 配布サイト
	http://opentwin.sourceforge.jp/

	お手伝いをしていただいた X Dev ◆.GNMmP6FqY氏のサイト
	http://www.geocities.jp/twintailfix/

	ヘルプを作成していただいた677氏のサイト
	http://www.geocities.co.jp/SiliconValley-PaloAlto/3550/

	RebarControl ライブラリ 「RebarDotNet」
	http://www.codeproject.com/cs/menu/rebarcontrol.asp
