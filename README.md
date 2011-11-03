ミストルティン
==============

Gitを使ったVisual Studio上での開発をサポートするVisual Studioの拡張機能です。
既存のGitフロントエンドとは異なり、使用者は基本的にGitを意識することがありません。
Gitを開発のバックエンドに据えた、開発の新しい形、それがミストルティンです。

ミストルティンは、Visual Studio 2010に対応しています。

前提
----

前提知識として、以下のことを知っていると理解が早くなるでしょう。

* Gitの基本的な概念
* トピックブランチを使った開発
* Git-Hooksを使った開発

考え方
------

### フィックスアップ

バックエンドにGitを採用しているため、コミットを重ねることで歴史を作っていくのは変わりません。
しかし、ミストルティンではファイルの保存のたびに(git-nowによって)コミットを行うため、
今までのコミットの粒度よりもはるかに細かい粒度のコミットが大量にできることになります。
しかし、これらのコミットは一時的なもので、最終的にはまとめあげることになります。
このタイミングが従来のコミットと同程度の粒度となりますが、
ミストルティンではこれをコミットと呼ばず、**フィックスアップ**と呼んでいます。

通常、開発者がファイル保存のたびに行われるコミットを意識する必要はありません。
フィックスアップすら、裏でコミットがまとめられて一つのコミットになるということを意識する必要はありません。
何かしらひと段落した段階でフィックスアップするだけでいいのです。

### コミットベースロールバック

*未実装の機能*です。

これが実装されると、現在の作業のロールバックに必要なポイントだけを表示したウィンドウからどこまで戻すかを決定できます。
また、セーブポイントにGitを使っているため、エディタの元に戻す機能よりも強力です。
例えば、ロールバックしてからいくらか作業を行った後でも、ロールバックする前の状態に戻すことが可能です。

### masterize

トピックブランチ上で開発を行って、それをmasterブランチにマージする場合、

1. トピックブランチをmasterブランチにリベース
1. masterブランチをチェックアウト
1. masterブランチを先ほどまでのトピックブランチに移動
1. トピックブランチを削除

という一連のコマンドを実行する必要があります。
この一連の流れ、つまりトピックブランチをmasterブランチにマージすることを、
ミストルティンでは**msterize**と呼んでいます。
ミストルティンを使うことによって、Visual Studio上からボタン一つでmasterizeを行えます。
この実現のために、git-masterというスクリプトを使用しています。

その他の機能
------------

### チケットリスト

Git-HooksによるチケットIDの自動追加は便利ですが、チケット番号の確認が面倒です。
ミストルティンの**チケットリスト**を使用することによって、一覧から作業するチケットを選べるようになります。

チケットリストのソースとしては、

* ローカルファイル
* Redmine
* GithubのIssue

に対応しています。

### テストの自動実行

*未実装の機能*です。

ビルド後にテストを自動実行する機能です。
NUnitベースのテスティングフレームワークに対応予定です。

### private build

*未実装の機能*です。

privateリポジトリにpushを行い、**private build**を実行します。
単に実行するだけではなく、ビルド結果を取得して通知も行います。
