using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleLabel : MonoBehaviour {
	public PlaceDelivery place;
	public Text name_txt;
	void Start(){
		GetComponentInChildren<Toggle> ().onValueChanged.AddListener (SelectPlace);
	}
	public void SetData(PlaceDelivery p){
		place = p;
		name_txt.text = p.company;
	}
	public void SelectPlace(bool isSelect){
		if (isSelect) {
			Profile.GetInstance.address_id = place.address_id;
		}
	}
}
