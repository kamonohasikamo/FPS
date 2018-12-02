# FPS
UnityでFPSもどきを作成する

1. 第一回コミット
  1. 素材配置
  terrainを使わずに床を作成。
  GameObjectのCubeを作成し、空のオブジェクト(マテリアル)をアタッチさせて色を付ける。
  1. プレイヤーの配置
  プレイヤーをCapsuleで作成。
  1. プレイヤーを移動させる処理
  Scriptで設定する。
  StartにGetComponentでオブジェクトを探索。
  UpdateにVector3を用いて、引数にInput.GetAxis("Horizontal")、Input.GetAxis("Vertical")を用いることで滑らかな移動が表現できる。
