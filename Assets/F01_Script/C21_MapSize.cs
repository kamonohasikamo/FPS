using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//====================================
// MapSizeCollection
//====================================
public class C21_MapSize {
	private const int DEFAULT_SIZE = 11;		//default value
	private int map_size_x;									//mapsize x point
	private int map_size_z;									//mapsize z point
	private int map_half_size_x;						//Half of map size x point
	private int map_half_size_z;						//Half of map size z point

	//-------------------------
	// constructor
	//-------------------------
	public C21_MapSize(int x, int z) {
		if (x <= 0) { // Assign a default value to x if it is less than 0
			x = DEFAULT_SIZE;
		}
		if (z <= 0) { // Assign a default value to z if it is less than 0
			z = DEFAULT_SIZE;
		}
		this.map_size_x = (x % 2 == 1) ? x : x + 1; //x value Oddization
		this.map_size_z = (z % 2 == 1) ? z : z + 1; //z value Oddization
		this.map_half_size_x = Mathf.FloorToInt(this.map_size_x / 2); //halfsizemap
		this.map_half_size_z = Mathf.FloorToInt(this.map_size_z / 2); //halfsizemap
	}

	//--------------------------
	// getter
	//--------------------------
	public int getMapSizeX() {
		return map_size_x;
	}
	public int getMapSizeZ() {
		return map_size_z;
	}
	public int getMapHalfSizeX() {
		return map_half_size_x;
	}
	public int getMapHalfSizeZ() {
		return map_half_size_z;
	}
}
