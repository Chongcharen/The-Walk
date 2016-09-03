using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class PlaceDeliveryPage : MonoBehaviour {
	public Button b_payment;
	public ToggleGroup group;
	public GameObject togglePrefab;
	public Transform content;
	void Start () {
		b_payment.onClick.AddListener (ToPaymentPage);
	}
	void OnEnable(){
		MallEvent.OnAddressDeliveryUpdateComplete += MallEvent_OnAddressDeliveryUpdate;
		MallEvent.OnUserLogoutComplete += MallEvent_OnUserLogoutComplete;
	}



	void OnDisable(){
		MallEvent.OnAddressDeliveryUpdateComplete -= MallEvent_OnAddressDeliveryUpdate;
		MallEvent.OnUserLogoutComplete -= MallEvent_OnUserLogoutComplete;
	}
	void ToPaymentPage(){
		ServiceRequest.instance.GetMyCart ();
		MallEvent.instance.SelectPage ((int)Page.PaymentPage);
	}

	void MallEvent_OnAddressDeliveryUpdate(){
		RectTransform content = group.gameObject.GetComponent<RectTransform> ();
		for (int i = 0; i < content.childCount; i++) {
			Destroy(content.GetChild (i).gameObject);
		}

		GameObject go;
		bool ison = true;
		foreach (PlaceDelivery p in Profile.GetInstance.placeDeliverys) {
			go = Instantiate (togglePrefab);
			go.transform.SetParent (content);
			go.transform.localScale = Vector3.one;
			go.GetComponentInChildren<Toggle> ().group = group;

			go.GetComponent<ToggleLabel> ().SetData (p);
			if (ison) {
				go.GetComponentInChildren<Toggle> ().isOn = true;
				go.GetComponent<ToggleLabel> ().SelectPlace (true);
				ison = false;
			}
		}


	}
	void MallEvent_OnUserLogoutComplete ()
	{
		//RectTransform content = group.gameObject.GetComponent<RectTransform> ();
		for (int i = 0; i < content.childCount; i++) {
			Destroy(content.GetChild (i).gameObject);
		}
	}
}
