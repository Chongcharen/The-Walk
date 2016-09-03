using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class AlertPopup : MonoBehaviour {

	public Text description_txt;
	public Button b_ok;
	IPopupOkReference popupRef;
	void Start(){
		b_ok.onClick.AddListener (OnOK);
	}
	void OnEnable(){
		PopupManager.OnShowAlertPopup += PopupManager_OnShowAlertPopup;
	}
		
	void OnDisable(){
		PopupManager.OnShowAlertPopup -= PopupManager_OnShowAlertPopup;
	}

	void PopupManager_OnShowAlertPopup (string msg,IPopupOkReference popupOk = null)
	{
		description_txt.text = msg;
		popupRef = popupOk;
	}
	void OnOK(){
		this.gameObject.SetActive (false);
		if (popupRef != null) {
			popupRef.OnOk ();
			popupRef = null; 
		}
	}
}

public interface IPopupOkReference{
	void OnOk();
}
public class InternetFailConnection : IPopupOkReference{
	public void OnOk(){
		SceneManager.LoadScene("Main");
	}

}
