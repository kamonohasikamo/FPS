# FPS
UnityでFPSもどきを作成する

  - 第一回コミット
    - 素材配置  
      terrainを使わずに床を作成。  
      GameObjectのCubeを作成し、空のオブジェクト(マテリアル)をアタッチさせて色を付ける。
    - プレイヤーの配置  
        プレイヤーをCapsuleで作成。
    - プレイヤーを移動させる処理  
        Scriptで設定する。  
        StartにGetComponentでオブジェクトを探索。  
        UpdateにVector3を用いて、引数にInput.GetAxis("Horizontal")、Input.GetAxis("Vertical")を用いることで滑らかな移動が表現できる。  
        
       
  - 第二回コミット
    - プレイヤーに重力を持たせる(着地)  
      GRAVITYという定数を用意。y軸方向に掛け算。  
    - プレイヤーのジャンプ(スペースキーで)  
      charaController.isGroundがtrueのとき(地面に足がついているとき)にのみジャンプ。  
      予め跳躍力を設定しておき、その力でy軸方向に力を加えてあげる。
