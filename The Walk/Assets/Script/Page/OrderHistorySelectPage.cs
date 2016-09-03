using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OrderHistorySelectPage : MonoBehaviour {
	public GameObject prefab,order_result;
	public RectTransform content;
	public Text order_code;
	void OnEnable(){
		MallEvent.OnOrderHistorySelectLoadComplete += MallEvent_OnOrderHistorySelectLoadComplete;
	}



	void OnDisable(){
		MallEvent.OnOrderHistorySelectLoadComplete -= MallEvent_OnOrderHistorySelectLoadComplete;
	}


	void ClearContent(){
		for (int i = 0; i < content.childCount; i++) {
			Destroy(content.GetChild (i).gameObject);
		}
	}


	void MallEvent_OnOrderHistorySelectLoadComplete (List<Cart> cartList,bool isNewOrder,string _order_code)
	{
		order_code.text = _order_code;
		if (isNewOrder) {
			order_result.SetActive (true);
		} else {
			order_result.SetActive (false);
		}
		ClearContent ();
		GameObject go;
		foreach (Cart c in cartList) {
			go = Instantiate (prefab);
			go.transform.SetParent (content);
			go.transform.localScale = Vector3.one;
			go.GetComponent<OrderHistorySelectLabel> ().SetData (c);
		}
	}

}
