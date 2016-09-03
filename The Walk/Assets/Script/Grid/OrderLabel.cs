using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OrderLabel : MonoBehaviour {
	Cart cart;
	public Button b_removeCart;
	public Text orderNumber_txt,foodName_txt,shop_txt,quantity_txt,price_txt;
	public void SetData(int ordernumber,Cart c){
		cart = c;
		orderNumber_txt.text = ordernumber.ToString ();
		foodName_txt.text = c.name;
		shop_txt.text = Mall.GetInstance.shopList.Find( s => s.shop_id == c.shop_id).name;
		quantity_txt.text = c.quantity.ToString();
		price_txt.text = c.price.ToString("N")+" THB";
	}

	void Start(){
		b_removeCart.onClick.AddListener (OnRemoveCart);
	}
	void OnEnable(){
		MallEvent.OnMyCartLoadComplete += MallEvent_OnMyCartLoadComplete;
	}


	void OnDisable(){
		MallEvent.OnMyCartLoadComplete -= MallEvent_OnMyCartLoadComplete;
	}

	void MallEvent_OnMyCartLoadComplete ()
	{
		Destroy (this.gameObject);
	}

	void OnRemoveCart(){
		ServiceRequest.instance.RemoveFormCart (cart.menu_id);
	}
}
