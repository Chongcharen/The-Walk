using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopBoard : MonoBehaviour {

	public Text shopName_txt,detail_txt,price_txt;
	public RawImage image;
	public Button b_select;
	Restaurant restaurant;

	public void Start(){
		b_select.onClick.AddListener (OnSelectShop);
	}
	public void SetData(Restaurant res){
		restaurant = res;

		shopName_txt.text = res.name;
		detail_txt.text = restaurant.name + " " + restaurant.address + " " + restaurant.detail;
		StartCoroutine ("LoadImage",res.images);
		Mall.GetInstance.shopBoard.Add (res.shop_id, this.gameObject);
	}
	IEnumerator LoadImage(string urlPath){
		WWW www = new WWW (urlPath);
		yield return www;
		image.texture = www.texture;
	}
	void OnSelectShop(){
		MallEvent.instance.SelectShop (restaurant.shop_id);
	}
}
