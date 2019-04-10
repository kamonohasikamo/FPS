using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================================
// マップ座標を扱うクラス
//===================================
public class C22_MapAxis {
	public struct Axis_XZ {
		public int x, z;
	}
	private Axis_XZ beforeAxis;				// 移動前の座標
	private Axis_XZ nowAxis;					// 移動後の座標
	private Axis_XZ differenceAxis;		// 移動の差分格納変数

	private GameObject player;				// プレイヤーオブジェクトへの参照用変数
	private C21_MapSize size;					// マップサイズへの参照用変数
	private Vector3 scale;						// 床ブロックのサイズ用

	//---------------------------
	// constructor
	//---------------------------
	public C22_MapAxis(GameObject player, C21_MapSize size, Vector3 scale) {
		this.beforeAxis.x = 0;
		this.beforeAxis.x = 0;
		this.nowAxis.x = 0;
		this.nowAxis.z = 0;
		this.differenceAxis.x = 0;
		this.differenceAxis.z = 0;

		this.player = player;	// 引数で受け渡された変数を参照する様に設定
		this.size = size;			// 引数で受け渡された変数を参照する様に設定
		this.scale = scale;		// Vector3型は値そのものがコピーされる。ブロックの大きさ
	}

	//------------------------------
	// playerの初期座標を設定
	//------------------------------
	public void initialize() {
		nowAxis.x									=	size.getMapHalfSizeX();	// 初期位置(移動後の座標)は、半マップサイズ
		nowAxis.z									=	size.getMapHalfSizeZ();	// 初期位置(移動後の座標)は、半マップサイズ
		player.transform.position	=	new Vector3(nowAxis.x * scale.x, player.transform.position.y, nowAxis.z * scale.z); // playerの位置を移動
	}

	//--------------------------------
	// playerの現在x座標を取得
	//--------------------------------
	private void getNowAxisX() {
		nowAxis.x = Mathf.FloorToInt((player.transform.position.x + scale.x / 2) / scale.x); // (現在位置+ブロック幅/2) / ブロック幅
	}

	//--------------------------------
	// playerの現在z座標を取得
	//--------------------------------
	private void getNowAxisZ() {
		nowAxis.z = Mathf.FloorToInt((player.transform.position.z + scale.z / 2) / scale.z); // (現在位置+ブロック幅/2) / ブロック幅
	}

	//--------------------------------
	// playerの現在xz座標を取得
	//--------------------------------
	public void getNowAxis() {
		nowAxis.x = Mathf.FloorToInt((player.transform.position.x + scale.x / 2) / scale.x); // (現在位置+ブロック幅/2) / ブロック幅
		nowAxis.z = Mathf.FloorToInt((player.transform.position.z + scale.z / 2) / scale.z); // (現在位置+ブロック幅/2) / ブロック幅
	}

	//--------------------------------
	// 座標の差分の更新
	//--------------------------------
	private void setDifferenceAxis() {
		differenceAxis.x = nowAxis.x - beforeAxis.x;
		differenceAxis.z = nowAxis.z - beforeAxis.z;
	}

	//--------------------------------
	// 座標の差分の更新(正面方向)
	//--------------------------------
	private void setDifferenceAxis_Z_Front() {
		differenceAxis.x = 0;
		differenceAxis.z = nowAxis.z - beforeAxis.z;

		if (differenceAxis.z < 0) {
			nowAxis.z = beforeAxis.z;
			differenceAxis.z = 0;
		}
	}

	//--------------------------------
	// player位置座標更新
	//--------------------------------
	public void updateAxis() {
		beforeAxis = nowAxis;
		getNowAxisZ();
		setDifferenceAxis_Z_Front();
	}

	//--------------------------------
	// 差分を返す
	//--------------------------------
	public Axis_XZ getDifferenceAxis() {
		return differenceAxis;
	}

	//--------------------------------
	// マップの橋の位置座標を返す
	// 現在位置±半分サイズ
	//--------------------------------
	public int getAxisMapStartX() {
		return nowAxis.x - size.getMapHalfSizeX();
	}

	public int getAxisMapStartZ() {
		return nowAxis.z - size.getMapHalfSizeZ();
	}

	public int getAxisMapEndX() {
		return nowAxis.x + size.getMapHalfSizeX();
	}

	public int getAxisMapEndZ() {
		return nowAxis.z + size.getMapHalfSizeZ();
	}

	//---------------------------------
	// playerの現在の座標を返す
	//---------------------------------
	public Axis_XZ getNowAxis_XZ() {
		return nowAxis;
	}

	//---------------------------------
	// 床ブロックのサイズを返す
	//---------------------------------
	public Vector3 getBlockSize() {
		return scale;
	}
}
