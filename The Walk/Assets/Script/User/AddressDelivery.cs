using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
public class AddressDelivery : MonoBehaviour {

	public GameObject prefabGrid;
	public RectTransform content;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		MallEvent.OnUserLoginComplete += MallEvent_OnUserLoginComplete;
		MallEvent.OnUserLogoutComplete += MallEvent_OnUserLogoutComplete;
		MallEvent.OnAddressDeliveryUpdate += MallEvent_OnAddressDeliveryUpdate;
	}







	void OnDisable(){
		MallEvent.OnUserLoginComplete -= MallEvent_OnUserLoginComplete;
		MallEvent.OnUserLogoutComplete -= MallEvent_OnUserLogoutComplete;
		MallEvent.OnAddressDeliveryUpdate -= MallEvent_OnAddressDeliveryUpdate;
	}


	void MallEvent_OnUserLoginComplete ()
	{
		//"http://www.thewalklifestylemall.com/service/getMyAddress"
		StartCoroutine(RequestGetMyAddress());
	}
	void MallEvent_OnUserLogoutComplete ()
	{
		ClearPlaceDelivery ();
		MallEvent.instance.AddressDeliveryUpdateComplete ();
	}
	//getMyAddress	{member_id}	ดึงรายชื่อที่อยู่ที่เคยบันทึกไว้
	IEnumerator RequestGetMyAddress(){
		WWWForm form = new WWWForm ();
		form.AddField ("member_id",Profile.GetInstance.user.member_id);
		WWW www = new WWW ("http://www.thewalklifestylemall.com/service/getMyAddress",form);
		yield return StartCoroutine(new WWWRequest(www));
		if (www.isDone) {
			Profile.GetInstance.placeDeliverys = JsonHelper.FromJsonList<PlaceDelivery> (www.text);
			MallEvent.instance.AddressDeliveryUpdateComplete ();
			SetGridPlaceDelivery ();
		}

	}
	void ClearPlaceDelivery(){
		for (int i = 0; i < content.childCount; i++) {
			Destroy(content.GetChild (i).gameObject);
		}
	}
	void SetGridPlaceDelivery(){
		ClearPlaceDelivery ();
		GameObject go;
		foreach (PlaceDelivery p in Profile.GetInstance.placeDeliverys) {
			go = Instantiate (prefabGrid);
			go.transform.SetParent (content);
			go.transform.localScale = Vector3.one;
			go.GetComponent<AddressDeliveryGrid> ().SetData (p);

		}

	}

	void MallEvent_OnAddressDeliveryUpdate ()
	{
		StartCoroutine(RequestGetMyAddress());
	}
}

