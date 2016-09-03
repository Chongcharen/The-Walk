using UnityEngine;
using System.Collections;

public class StaticColor : MonoBehaviour {
	public static StaticColor instance;
	public Color selectColor,deselectColor;
	void Awake(){
		instance = this;
	}

}
