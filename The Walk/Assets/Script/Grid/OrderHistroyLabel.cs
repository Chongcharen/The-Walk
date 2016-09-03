using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class OrderHistroyLabel : MonoBehaviour {
	Order order;
	public Button b_view;
	public Text orderNumber_txt,date_txt,status_txt,total_txt;
	public void SetData(int ordernumber,Order o){
		order = o;
		orderNumber_txt.text = ordernumber.ToString ();
		var ci = System.Globalization.CultureInfo.GetCultureInfo("en-us");
		date_txt.text = System.DateTime.Parse(order.timestamp, ci).ToString("MMM dd, yyy");
		status_txt.text = order.order_status;
		total_txt.text = order.total.ToString("N") +"THB "+order.quantity + " items";
	}

	void Start(){
		b_view.onClick.AddListener (OnViewOrder);
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

	void OnViewOrder(){
		//view order.order_id
		//ServiceRequest.instance.RemoveFormCart (cart.menu_id);
		ServiceRequest.instance.GetHistoryView(order.order_id);
		MallEvent.instance.CallEditorProfilePage((int)ProfilePageSelect.OrderHistorySelectPage);
	}
}
