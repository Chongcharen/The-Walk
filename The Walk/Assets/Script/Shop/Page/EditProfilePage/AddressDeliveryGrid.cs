using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AddressDeliveryGrid : MonoBehaviour {
	public delegate void EditAddress_DeliveryEvent(PlaceDelivery place);
	public static event EditAddress_DeliveryEvent OnEditAddress_Delivery;
	public Button b_editAddress;
	public PlaceDelivery place;
	public Text company_txt;
	// Use this for initialization
	void Start () {
		b_editAddress.onClick.AddListener (OnEditAddress);
	}
	void OnEditAddress(){
		OnEditAddress_Delivery (place);
	}
	void OnEnable(){
		
	}
	void OnDisable(){
		
	}
		
	public void SetData(PlaceDelivery _place){
		place = _place;
		company_txt.text = place.company;
	}

	void MallEvent_OnAddressDeliveryUpdate ()
	{
		Destroy (this.gameObject);
	}
}
