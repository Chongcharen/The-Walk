using UnityEngine;
using System.Collections;
public class InternetProcess : MonoBehaviour {

	// Use this for initialization
	public static InternetProcess instance;
	void Awake(){
		instance = this;
	}
	void Start () {
		//StartCoroutine ("checkInternetConnection");
		//Initialize();
	}

/*	IEnumerator checkInternetConnection(){
		Debug.Log ("connect " + IsConnected ());
		if(!IsConnected()){
			PopupManager.instance.ShowAlertPopup ("ไม่สามารถเชื่อมต่อ internet ได้",new InternetFailConnection());
		}
		yield return new WaitForSeconds (5);
		StartCoroutine ("checkInternetConnection");
	}*/




	IEnumerator checkInternetConnection(){

		StopCoroutine ("WaitForWWW");
		WWW www = new WWW("https://google.com");

		StartCoroutine ("WaitForWWW", www);
		yield return StartCoroutine(new WWWRequest(www));
		if (www.error != null) {
			
			PopupManager.instance.ShowAlertPopup ("ไม่สามารถเชื่อมต่อ internet ได้",new InternetFailConnection());
		} else {
			//Messenger.Broadcast (SingletonPopupEvent.OPEN_SPOPUP_MUSTACCEPT , "ท่านไม่ได้เชื่อมต่ออินเตอร์เน็ต กรุณาเข้าเกมใหม่อีกครั้ง");
		
		}
		StartCoroutine ("checkInternetConnection");
	} 
		
	IEnumerator WaitForWWW(WWW www)
	{
		yield return new WaitForSeconds (10);
		if (!www.isDone) {
			/*Messenger.Broadcast(PopupEvent.OPEN_GAME_LOADER,false);
			Messenger.Broadcast(SingletonPopupEvent.OPEN_INTERNET_ERROR);*/
			PopupManager.instance.ShowAlertPopup ("ไม่สามารถเชื่อมต่อ internet ได้", new InternetFailConnection ());
		} else {
			//ServiceRequest.instance.LoadDataFormServer ();
		}

	}
}
