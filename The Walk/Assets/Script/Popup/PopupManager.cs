using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupManager : MonoBehaviour {
	public static PopupManager instance;

	public delegate void ShowAlertPopupEvent(string msg,IPopupOkReference popupRef);
	public static event ShowAlertPopupEvent OnShowAlertPopup;



	public GameObject alertPopup;




	void Awake(){
		instance = this;
	}




	public void ShowAlertPopup(string msg,IPopupOkReference popupRef = null){
		alertPopup.SetActive (true);
		OnShowAlertPopup (msg,popupRef);
	}
}
