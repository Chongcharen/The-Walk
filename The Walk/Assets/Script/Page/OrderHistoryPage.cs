using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OrderHistoryPage : MonoBehaviour {
	public GameObject orderPrefab;
	public RectTransform content;

	void OnEnable(){
		MallEvent.OnOrderHistoryLoadComplete += MallEvent_OnOrderHistoryLoadComplete;

	}



	void OnDisable(){
		MallEvent.OnOrderHistoryLoadComplete -= MallEvent_OnOrderHistoryLoadComplete;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	void MallEvent_OnOrderHistoryLoadComplete (List<Order> orderList)
	{
		ClearContent ();

		GameObject go;
		int index = 1;
		foreach (Order order in orderList) {
			Debug.Log (order.total);
			go = Instantiate (orderPrefab);
			go.transform.SetParent (content);
			go.transform.localScale = Vector3.one;
			go.GetComponent<OrderHistroyLabel> ().SetData (index,order);
			index++;
		}


	}

	void ClearContent(){
		for (int i = 0; i < content.childCount; i++) {
			Destroy(content.GetChild (i).gameObject);
		}
	}
}
