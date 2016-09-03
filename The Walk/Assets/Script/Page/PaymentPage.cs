using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PaymentPage : MonoBehaviour {

	public GameObject orderGridprefab;
	public RectTransform content;
	public Text orderPrice_txt;
	public Button b_saveOrder;
	public Text cash_code_txt, promo_code_txt;
	public float orderPrice;

	void Start(){
		b_saveOrder.onClick.AddListener (OnSaveOrder);
	}
	void OnEnable(){
		MallEvent.OnMyCartLoadComplete += MallEvent_OnMyCartLoadComplete;
		MallEvent.OnUserLogoutComplete += MallEvent_OnUserLogoutComplete;
	}


		
	void OnDisable(){
		MallEvent.OnMyCartLoadComplete -= MallEvent_OnMyCartLoadComplete;
		MallEvent.OnUserLogoutComplete -= MallEvent_OnUserLogoutComplete;
	}

	void MallEvent_OnMyCartLoadComplete ()
	{
		ClearOrderLabel ();
		GameObject go;
		int index = 1;
		orderPrice = 0;
		foreach(Cart c in Profile.GetInstance.carts){
			go = Instantiate(orderGridprefab);
			go.transform.SetParent(content);
			go.transform.localScale = Vector3.one;
			go.GetComponent<OrderLabel> ().SetData (index,c);
			orderPrice += (float)(c.price * c.quantity);
			index++;
		}
		orderPrice_txt.text = orderPrice.ToString("N") + " THB";
	
	}
	void OnSaveOrder(){
		string cashcode = cash_code_txt.text;

		/*if (Profile.GetInstance.carts.Count <= 0) {
			PopupManager.instance.ShowAlertPopup("ยังไม่มีรายการอาหารที่ถูกสั่ง");
			return;
		}
		if (string.IsNullOrEmpty (cashcode) || cashcode.Length < 10) {
			PopupManager.instance.ShowAlertPopup("โปรดใส่รหัสบัตรให้ครบ 10 หลัก");
			return;
		}*/

		ServiceRequest.instance.SaveOrder (cashcode,promo_code_txt.text);
	}
	void ClearOrderLabel(){
		for (int i = 0; i < content.childCount; i++) {
			Destroy(content.GetChild (i).gameObject);
		}
	}
	void MallEvent_OnUserLogoutComplete ()
	{
		ClearOrderLabel ();
	}
}
