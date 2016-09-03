using UnityEngine;
using System.Collections;

public class ShopMenu : MonoBehaviour {
	public GameObject shopBox;
	// Use this for initialization
	void OnEnable(){
		MallEvent.OnShopLoadComplete += MallEvent_OnShopLoadComplete;
	}
	void OnDisable(){
		MallEvent.OnShopLoadComplete -= MallEvent_OnShopLoadComplete;
	}
	void MallEvent_OnShopLoadComplete ()
	{
		GameObject go;
		for (int i = 0; i < Mall.GetInstance.shopList.Count; i++) {
			go = Instantiate (shopBox);
			go.transform.SetParent (this.transform);
			go.transform.localScale = Vector3.one;
			go.GetComponent<ShopBoard> ().SetData ((Restaurant)Mall.GetInstance.shopList[i]);

		}
	}

}
