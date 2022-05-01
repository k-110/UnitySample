@import "doc.less"

# TheHand

## 概要設計

3Dモデルをスクリプトで動かして遊ぼうと思っ始めていたら、いつの間にかいろいろな機能の動作確認になってしまった・・・

### 機能構造＆データフロー

```plantuml
@startuml
!define  COLOR_DMY   #lightgray
class "3Dモデル\n(hand)" as hand{
}
class "画像\n(RawImage" as RawImage{
}
class "録音＆再生パネル\n(PanelMicrophone)" as PanelMicrophone{
}
class "カメラ\n(MyWebCam)" as MyWebCam{
    ※640 × 480 の RGB
    ※WebCamTextureを使用
}
class "マイク＆スピーカー\n(MyAudio)" as MyAudio{
    ※AudioSourceを使用
}
class "Webアプリ\n(memo)" as memo{
    ※Djangoを使用
}
class "描画機能\n(WebCamera)" as WebCamera{
    ※RawImage
}
class "画像処理機能\n(ImageHelper)" as ImageHelper{
    + グレースケール化()
    + 差分を算出()
    + 輝度の自動調整()
    + ぼかす()
    + エッジ検出()
    + 2値化()
    + 平均と標準偏差を見つける()
    + 最小値と最大値を見つける()
}
class "バーコード機能\n(BarcodeHelper)" as BarcodeHelper{
    + バーコードをデコード()
    + 2次元バーコード作成()
}
class "バーコードライブラリ\n(ZXing.Net)" as ZXing{
}
class "録音＆再生機能\n(UsbMicrophone)" as UsbMicrophone{
}
class "対memo通信機能\n(NetworkComm)" as NetworkComm{
}
class "HTTP通信機能\n(HttpClientHelper)" as HttpClientHelper{
    ※UnityWebRequestを使用
}
class "3Dモデル操作パネル\n(PanelHand)" as PanelHand{
}
class "3Dモデル操作機能\n(HandAnimation)" as HandAnimation{
}
RawImage        <--     WebCamera       : 白黒画像
WebCamera       <--     MyWebCam        : カラー画像
ImageHelper     <->     WebCamera       : 画像
WebCamera       <->     BarcodeHelper   : 画像\n文字列
BarcodeHelper   <-->    ZXing           : 画像\n文字列
PanelMicrophone -->     UsbMicrophone   : 録音\n再生
UsbMicrophone   <-->    MyAudio         : AudioClip
NetworkComm     <-->    HttpClientHelper: 通信データ
HttpClientHelper<-->    memo            : Get\nPost
PanelHand       -->     HandAnimation
HandAnimation   -->     hand
@enduml
```

### 画像処理

- Unity向けのOpenCVが有料のアセットだったので画像処理の動作確認用のコードを実装
  - 本格的に何かを作る段階になったならアセットの購入を検討する

### バーコード

- チェックボックスをONにしたときに動作
  - カメラで移したバーコードのデコード結果を表示
  - 固定の文字列で2次元バーコードを生成して表示

### 3Dモデル操作

- ボーンをスクリプトで稼働
- ボーン毎に初期位置からの角度を指定すると指定した位置へ移動するスクリプトを作成
- 3Dモデルが手抜き過ぎていまいちな動き
  - ボーンの向きがそろってない
  - 関節を曲げるとモデルが潰れる(頂点の数を調整したり、補助ボーンを入れたりするらしい)
  - 一定角度を超えると元に戻らなくなる(ガクガクするのでどこかが干渉していそう)

### 録音＆再生機能

- チェックボックスをONにしている状態で録音
  - 録音時間は1秒(チェックを外す1秒前～の音が録音される)
- ボタンを押すと録音した音を再生

### HTTP通信

- Getでテキストファイルの中身を表示するページを取得
- Postでテキストファイルの中身を書き換え
  - 一回GetしてからでなければPostできない(DjangoではPostにCSRFトークンが必要)

## ライブラリをUnityプロジェクトに追加

- ZXing
  1. 「https://github.com/micjahn/ZXing.Net」からファイルを取得
  2. 「zxing.unity.dll」を「Assets\Plugins」に置く
