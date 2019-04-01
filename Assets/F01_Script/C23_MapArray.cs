using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//====================================
// マップ配列を扱うクラス
//====================================

public class C23_MapArray {
	protected string name;						// 配列の名前(オブジェクト名)
	protected GameObject folder;			// 作成しタップオブジェクトを入れるフォルダ
	protected GameObject[,] array;		// マップオブジェクト格納用配列
	protected C21_MapSize size;				// マップサイズへの参照用変数
	protected C22_MapAxis axis;				// マップ座標への参照用変数
	protected C22_MapAxis.Axis_XZ arrayAxis_Start;	//map始点の配列座標格納用
	protected C22_MapAxis.Axis_XZ arrayAxis_End;		//map終点の配列座標格納用

	protected int SIGN;								// 符号

	//---------------------------------------
	// constructor
	//---------------------------------------
	public C23_MapArray (string name, C21_MapSize size, C22_MapAxis axis) {
		this.name		= name;
		this.folder	= new GameObject(this.name + "_Folder");
		this.array	= new GameObject[size.getMapSizeX(), size.getMapSizeZ()];
		this.size		= size;
		this.axis		= axis;

		this.SIGN		= 1;
	}

	//---------------------------------------
	// 位置座標Xを配列座標に変換して返す
	//---------------------------------------
	protected int getArrayNumX(int value) {
		int ret = value % size.getMapSizeX();
		if (ret < 0) {
			ret += size.getMapSizeX();
		}
		return ret;
	}

	//---------------------------------------
	// 位置座標Zを配列座標に変換して返す
	//---------------------------------------
	protected int getArrayNumZ(int value) {
		int ret = value % size.getMapSizeZ();
		if (ret < 0) {
			ret += size.getMapSizeZ();
		}
		return ret;
	}

	//---------------------------------------
	// 指定された位置座標にオブジェクトを作成
	// 配列に格納
	//---------------------------------------
	protected void cleateObject(GameObject prefab, C22_MapAxis.Axis_XZ arg) {
		cleateObject(prefab, arg.x, arg.z);
	}

	protected void cleateObject(GameObject prefab, int x, int z) {
		int array_z = getArrayNumZ(z); // 配列座標xを取得
		int array_x = getArrayNumX(x); // 配列座標zを取得

		if (array[array_x, array_z] != null) {
			MonoBehaviour.Destroy(array[array_x, array_z].gameObject); // 配列内にオブジェクトが格納されていたら、削除する
			array[array_x, array_z] = null; // 削除したら中身は空っぽにする
		}

		Vector3 scale 					= prefab.transform.localScale; // 受け渡されたオブジェクトのサイズを格納
		Vector3 pos							= new Vector3(scale.x * x, SIGN * scale.y / 2, scale.z * z);
		GameObject obj		 			= GameObject.Instantiate (prefab, pos, Quaternion.identity) as GameObject;
		obj.name								= name + "[" + array_x + "," + array_z + "]";		// 作成したオブジェクトの名前変更
		obj.transform.parent		= folder.transform;						// 作成したオブジェクトの親を、フォルダーにする
		array[array_x, array_z]	= obj;
	}

	//---------------------------------------
	// 指定された位置座標のオブジェクトを削除
	// 配列に格納されている値の削除
	//---------------------------------------
	protected void deleteObject(C22_MapAxis.Axis_XZ arg) {
		deleteObject(arg.x, arg.z);
	}

	protected void deleteObject(int x, int z) {
		int array_x = getArrayNumX(x);
		int array_z = getArrayNumZ(z);
		if (array[array_x, array_z] != null) { //配列内にオブジェクトが格納されていたら削除
			MonoBehaviour.Destroy(array[array_x, array_z].gameObject);
			array[array_x, array_z] = null;
		}
	}

	//---------------------------------------
	// マップ始点・終点の配列座標の取得
	//---------------------------------------
	protected void renewal_arrayAxis() {
		arrayAxis_Start.x	= getArrayNumX(axis.getAxisMapStartX());	// マップ始点のX位置座標を、配列座標Xとして取得
		arrayAxis_Start.z	= getArrayNumZ(axis.getAxisMapStartZ());	// マップ始点のZ位置座標を、配列座標Zとして取得
		arrayAxis_End.x		= getArrayNumX(axis.getAxisMapEndX());	// マップ終点のX位置座標を、配列座標Xとして取得
		arrayAxis_End.z		= getArrayNumZ(axis.getAxisMapEndZ());	// マップ終点のZ位置座標を、配列座標Zとして取得
	}
}

//=======================================================
// マップ配列の床を扱うクラス
//=======================================================

public class MapArrayBlock : C23_MapArray {
	private GameObject[] block;		// 床ブロックの参照用

	//---------------------------------------
	// constructor
	//---------------------------------------
	public MapArrayBlock (GameObject[] obj, string name, C21_MapSize size, C22_MapAxis axis) : base(name, size, axis){
		this.block = obj;		// 引数で受け渡された変数を参照する様に設定。
		this.SIGN = -1;			// 床側なので、符合をマイナスに。
	}

	//---------------------------------------
	//指定座標の、床タイプ番号を返す (座標x+zをプレファブ数で割った余値を返す)
	//---------------------------------------
	private int getInt_BlockType(C22_MapAxis.Axis_XZ arg) {
		return getInt_BlockType(arg.x, arg.z);
	}
	private int getInt_BlockType(int x, int z){
		if (block.Length != 0) {
			int ret = (x + z) % block.Length;
			if(ret < 0){
				ret += block.Length;
			}
			return ret;
		} else {
			return 0;		// 配列の中身が無い場合は、０を返す
		}
	}

	//---------------------------------------
	// スタート時のマップ(床)作成
	//---------------------------------------
	public void startMap_Create(){
		renewal_arrayAxis();
		if(block.Length != 0){
			int n;	// 床タイプ番号
			for (int z = 0; z < size.getMapSizeZ(); z++) {
				for (int x = 0; x < size.getMapSizeX(); x++) {
					n = getInt_BlockType(x, z);				// 床タイプ番号を取得
					cleateObject(block [n], x, z);			// オブジェクト作成
				}
			}
		}else{
			return;		// 配列の中身が無い場合は、何もせず抜ける
		}
	}

	//---------------------------------------
	// マップ(床)の更新
	//---------------------------------------
	public void renewal(){
		renewal_Z();	// 行方向のマップ(床)更新
	}

	//---------------------------------------
	// 列方向のマップ(床)更新
	//---------------------------------------
	private void renewal_X(){
		if(axis.getDifferenceAxis().x != 0){		// 位置座標の差分xが０で無いなら
			C22_MapAxis.Axis_XZ posAxis;			// 位置座標の始点
			posAxis.x = (axis.getDifferenceAxis().x > 0) ? axis.getAxisMapEndX() : axis.getAxisMapStartX(); // 始点Xはマップ端　（現在位置±半マップサイズ）
			posAxis.z = axis.getAxisMapStartZ();	// 始点Zはマップ端　（現在位置－半マップサイズ）

			if(block.Length != 0){
				int n;	// 床タイプ番号
				for (int z = 0; z< size.getMapSizeZ(); z++) {
					n = getInt_BlockType(posAxis.x, z + posAxis.z);			// 床タイプ番号を取得
					cleateObject(block[n], posAxis.x, z + posAxis.z);		// オブジェクト作成
				}
			}else{
				return;		// 配列の中身が無い場合は、何もせず抜ける
			}
		}
	}

	//---------------------------------------
	// 行方向のマップ(床)更新
	//---------------------------------------
	private void renewal_Z(){
		if(axis.getDifferenceAxis().z != 0){		// 位置座標の差分Zが０で無いなら
			C22_MapAxis.Axis_XZ posAxis;			// 位置座標の始点
			posAxis.x = axis.getAxisMapStartX();	// 始点Xはマップ端　（現在位置－半マップサイズ）
			posAxis.z = (axis.getDifferenceAxis().z > 0) ? axis.getAxisMapEndZ() : axis.getAxisMapStartZ(); // 始点Zはマップ端　（現在位置±半マップサイズ）

			if(block.Length != 0){
				int n;	// 床タイプ番号
				for (int x = 0; x < size.getMapSizeX(); x++) {
					n = getInt_BlockType(x + posAxis.x, posAxis.z);			// 床タイプ番号を取得
					cleateObject(block[n], x + posAxis.x, posAxis.z);		// オブジェクト作成
				}
			}else{
				return;		// 配列の中身が無い場合は、何もせず抜ける
			}
		}
	}
}

//==========================================
// マップ配列(地上)を扱うクラス
//==========================================
public class MapArrayFloor : C23_MapArray{
	private GameObject[] wall;		// 壁オブジェクトの参照用

	private GameObject[] obstacle;		// 障害物オブジェクト格納用配列
	private bool[,] isMoveArea;				// プレイヤーが移動できるエリア
	private bool isMapAppropriate;		// プレイヤーがそのマップで移動可能かどうかの判定

	//---------------------------------------
	// constructor
	//---------------------------------------
	public MapArrayFloor (string name, C21_MapSize size, C22_MapAxis axis) : base(name, size, axis) { //配列の確保とfalseに初期化
		isMapAppropriate = false;
		isMoveArea = new bool[size.getMapSizeX(), size.getMapSizeZ()];
		for (int z = 0; z < size.getMapSizeZ(); z++) {
			for (int x = 0; x < size.getMapSizeX(); x++) {
				isMoveArea[x, z] = false;
			}
		}
	}

	//---------------------------------------
	// 障害物オブジェクトのセット
	//---------------------------------------
	public void setObstacle(GameObject[] obj) {
		obstacle = obj;
	}

	//---------------------------------------
	// 壁オブジェクトのセット
	//---------------------------------------
	public void setWall(GameObject[] obj){
		wall = obj;
	}

	//---------------------------------------
	// スタート時のマップ(地上)作成
	//---------------------------------------
	public void startMap_Create(){
		renewal_arrayAxis();
		for (int z = 0; z< size.getMapSizeZ(); z++) {
			cleateObject(wall[0], axis.getAxisMapStartX(), z);	// 0列目に壁オブジェクト作成
			cleateObject(wall[0], axis.getAxisMapEndX(), z);		// (マップサイズ－１)列目に壁オブジェクト作成
		}
		isAxis_SetIsMoveArea(axis.getNowAxis_XZ());							// 該当座標を基準に調べ、プレイヤーが移動出来るエリアをtrueに変えていく
		isMapInPlayerMoving();																// Map上端の一列を確認し、プレイヤーが移動できるかどうか判定
	}

	//---------------------------------------
	// マップ(地上)の更新
	//---------------------------------------
	public void renewal(){
		renewal_wallZ();			// 行方向のマップ(壁)の更新
		randomSetObstacleZ();	// 行方向のマップ(障害物)の更新

		if (axis.getDifferenceAxis().z > 0) { // 位置座標の差分Zがプラスなら
			renewal_arrayAxis();								// 配列座標を更新
			renewal_IsMoveArea();								// マップ上端一列のプレイヤー移動可能判定を更新
			isMapInPlayerMoving();							// 移動可能かどうかの判定

			while(!isMapAppropriate) {					// プレイヤー移動可能となるまで
				correctMap();											// Map修正
				isMapInPlayerMoving();						// 移動可能かどうかの判定
			}
		}
	}

	//---------------------------------------
	// 行方向のマップ(壁)更新
	//---------------------------------------
	private void renewal_wallZ(){
		if(axis.getDifferenceAxis().z != 0){		// 位置座標の差分Zが０で無いなら
			if(wall.Length != 0){
				int z = (axis.getDifferenceAxis().z > 0) ? axis.getAxisMapEndZ() : axis.getAxisMapStartZ(); // Zはマップ端　（現在位置±半マップサイズ）
				cleateObject(wall[0], axis.getAxisMapStartX(), z);		// オブジェクト作成
				cleateObject(wall[0], axis.getAxisMapEndX(), z);		// オブジェクト作成
			} else {
				return;		// 配列の中身が無い場合は、何もせず抜ける
			}
		}
	}

	//---------------------------------------
	// 行方向のマップ(障害物)更新
	//---------------------------------------
	private void randomSetObstacleZ() {
		if (axis.getDifferenceAxis().z > 0) { 	// 位置座標の差分Zがプラスなら
			C22_MapAxis.Axis_XZ posAxis;					// 位置座標の始点
			posAxis.x = axis.getAxisMapStartX();	// 始点Xはマップ始端（現在位置－半マップサイズ）
			posAxis.z = axis.getAxisMapEndZ();		// Zはマップ終端　（現在位置＋半マップサイズ）

			if (obstacle.Length != 0) {
				for (int x = 1; x < size.getMapSizeX() - 1; x++) {
					if (Random.Range(0, 100) < 30) {	// ランダム値0～99で30以下なら、壁を生成
						int randomize = Random.Range(0, obstacle.Length);
						cleateObject(obstacle[randomize], x + posAxis.x, posAxis.z); // 壁
					} else {
						deleteObject(x + posAxis.x, posAxis.z); // objectの削除
					}
				}
			}
		} else {
			return;	// 配列の中身がない場合は何もせずに抜ける
		}
	}

	//------------------------------------------------------------------------
	// playerの移動可能エリアの判定
	// 渡された位置座標を配列座標に変換し、プレイヤーが移動できるエリアをtrueに
	//------------------------------------------------------------------------
	private void isAxis_SetIsMoveArea(C22_MapAxis.Axis_XZ arg) {
		isAxis_SetIsMoveArea(arg.x, arg.z);
	}

	private void isAxis_SetIsMoveArea(int x, int z) {
		isAxisArray_SetIsMoveArea(getArrayNumX(x), getArrayNumZ(z));
	}

	//------------------------------------------------------------------------
	// 受け渡された配列座標からプレイヤーが移動できるエリアをtrueに
	//------------------------------------------------------------------------
	private void isAxisArray_SetIsMoveArea(int array_x, int array_z) {
		if (array[array_x, array_z] == null && !isMoveArea[array_x, array_z]) { //その配列座標が空(オブジェクトが入っていない)　かつ　falseなら
			isMoveArea[array_x, array_z] = true;	// trueにする
			isDir_IsMoveArea(array_x, array_z);		// その配列座標の周囲を調べ、オブジェクトがなければtrueに変える
		}
	}

	//------------------------------------------------------------------------
	// 受け渡された配列座標の上下左右を調べ
	// objectがなければtrueに変える
	//------------------------------------------------------------------------
	private void isDir_IsMoveArea(int array_x, int array_z) {
		if (array_x != arrayAxis_Start.x) { // Map左端でないとき
			isAxis_SetIsMoveArea(getArrayNumX(array_x - 1), array_z); // x-1座標を、プレイヤーが移動出来るエリアかどうか調べる
		}
		if (array_x != arrayAxis_Start.x) { // Map左端でないとき
			isAxis_SetIsMoveArea(array_x, getArrayNumZ(array_z - 1)); // z-1座標を、プレイヤーが移動出来るエリアかどうか調べる
		}
		if (array_x != arrayAxis_Start.x) { // Map左端でないとき
			isAxis_SetIsMoveArea(getArrayNumX(array_x + 1), array_z); // x+1座標を、プレイヤーが移動出来るエリアかどうか調べる
		}
		if (array_x != arrayAxis_Start.x) { // Map左端でないとき
			isAxis_SetIsMoveArea(array_x, getArrayNumZ(array_z + 1)); // z+1座標を、プレイヤーが移動出来るエリアかどうか調べる
		}
	}

	//------------------------------------------------------------------------
	// Mapの上端一列を確認し、プレイヤーが移動できるかどうかを判定
	// 例:5×5のマスを考える。
	// 上方向にプレイヤーが移動するので、上一列に移動可能である◎が存在すればOK
	// 良い例
	//　■◎◎◎◎◎■		←この列を見ればOK
	//　■◎◎◎◎◎■
	//　■◎◎◎■◎■
	//　■◎■ ■ ■◎■
	//　■◎◎◎◎◎■
	// 今回の実装では、一つでも◎があれば移動できると判定できる
	//------------------------------------------------------------------------
	private void isMapInPlayerMoving() {
		for (int x = 1; x < size.getMapSizeX() - 1; x++) {
			if (getIsMoveArea(axis.getAxisMapStartX() + x, axis.getAxisMapEndZ())) { // 指定座標(マップ上端一列)を調査し、プレイヤーが移動出来るエリアを見つけたら
				isMapAppropriate = true;
				return;
			}
		}
		isMapAppropriate = false; // そのマップは移動不可
	}

	//-------------------------------------------------------------------------
	// 指定された位置座標のisMoveAreaの値を返す
	//-------------------------------------------------------------------------
	private bool getIsMoveArea(int x, int z) {
		return isMoveArea[getArrayNumX(x), getArrayNumZ(z)];
	}

	//-------------------------------------------------------------------------
	// 指定された位置座標のisMoveAreaをfalseにする
	//-------------------------------------------------------------------------
	private void setIsMoveArea_False(int x, int z) {
		isMoveArea[getArrayNumX(x), getArrayNumZ(z)] = false;
	}

	//-------------------------------------------------------------------------
	// マップ上端一列のisMoveAreaを更新
	//-------------------------------------------------------------------------
	private void renewal_IsMoveArea() {
		for (int x = 1; x < size.getMapSizeX() - 1; x++) {
			setIsMoveArea_False(axis.getAxisMapStartX() + x, axis.getAxisMapEndZ());
		}
		for (int x = 1; x < size.getMapSizeX() - 1; x++) {
			isRenewalMapAxisIsMoveArea(axis.getAxisMapStartX() + x, axis.getAxisMapEndZ()); // 指定座標とその真下座標を調べ、プレイヤーが通れるなら、プレイヤーが移動出来るエリアをtrueに変えていく
		}
	}

	//------------------------------------------------------------------------
	// 受け渡された位置座標と、その下を調べる。
	// プレイヤーが通れるなら、プレイヤーが移動できるエリアをTrueに
	//------------------------------------------------------------------------
	private void isRenewalMapAxisIsMoveArea(int x, int z) {
		int array_x = getArrayNumX(x);		// 配列座標Xを取得
		int array_z = getArrayNumZ(z);		// 配列座標Zを取得

		if(array[array_x , array_z] != null || isMoveArea[array_x, array_z]){
			return;
		}	// 該当座標が空でない、もしくはtrueなら、処理を抜ける

		if(isMoveArea[array_x, getArrayNumZ(array_z - 1)]) {		// 真下の座標がtrueなら
			isAxis_SetIsMoveArea(array_x, array_z);	//
		}
	}

	//-----------------------------------------------------------------------
	// map修正
	//-----------------------------------------------------------------------
	private void correctMap() {
		int array_x = getArrayNumX(axis.getAxisMapStartX() + Random.Range(1, size.getMapSizeX() - 1));
		int array_z = arrayAxis_End.z;

		if (array[array_x, array_z] != null) {
			if (isMoveArea[array_x, getArrayNumZ(array_z - 1)]) { // 指定座標Z-1がプレイヤー通行可能エリアだった場合
				deleteObject(array_x, array_z);											// その座標のオブジェクトを破壊
				isAxisArray_SetIsMoveArea(array_x, arrayAxis_End.z);// プレイヤーが移動出来るエリアをtrueに変えていく
			}
		}
	}

}
