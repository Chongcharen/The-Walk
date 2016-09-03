using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContactPage : MonoBehaviour {

    public Button b_map;
	void Start(){
		b_map.onClick.AddListener (OpenMap);
	}
	void OpenMap(){
		Application.OpenURL("https://www.google.co.th/maps/place/The+Walk+Lifestyle+Mall/@13.4377658,101.103957,17z/data=!4m5!3m4!1s0x0:0x7eb6c6cea2f33720!8m2!3d13.4377655!4d101.1061511?hl=th");
	}
}
