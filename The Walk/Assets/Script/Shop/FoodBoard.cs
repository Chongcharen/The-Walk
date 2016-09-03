using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoodBoard : MonoBehaviour {
	public Button b_cart,b_share;
	public Text foodName_txt,detail_txt,price_txt;
	public RawImage image;
	public Restaurant restaurant;
	public Food food;
	public int category_id;
	string linkImg;

	void Start(){
		b_cart.onClick.AddListener (AddtoCart);
		b_share.onClick.AddListener (OnShareFB);
	}



	/*addtocart	{menu_id,member_id,cache_id}	สำหรับเพิ่มเมนูลงตระกร้า	ถ้าไม่ได้ล๊อกอินไม่ต้องส่ง member_id มา 
	ให้ส่ง cache_id มาแทน โดยที่ cache_id ให้แรนดอมตัวเลขกับตัวอักษรมา 8 ตัว ไว้อ้างอิงกรณีไม่ล๊อกอิน*/
	void AddtoCart(){
		ServiceRequest.instance.AddToCart (food.menu_id,food.name);
	}


	public void SetData(Food _food){
		food = _food;

		restaurant = Mall.GetInstance.shopList.Find (res => res.shop_id == food.shop_id);

		if (restaurant == null) {
			Debug.Log ("ไม่พบรา้นค้า");
		}
		category_id = food.category_id;
		//Mall.GetInstance.foodBoard.Add (food.menu_id, this.gameObject);
		Mall.GetInstance.foodBoard.Add(this.gameObject);
		foodName_txt.text = food.name;
		detail_txt.text = restaurant.name + " " + restaurant.address + " " + restaurant.detail;
		price_txt.text = (food.price.ToString () + " บาท");
		StartCoroutine ("LoadImage",food.images);
	}
	public void OnShareFB(){
		
		GeneralSharing.instance.OnShareTextWithLinkImage (linkImg,new Vector2(image.texture.width,image.texture.height));
		//GeneralSharing.instance.OnShareTextWithImage();
	}
	IEnumerator LoadImage(string urlPath){
		linkImg = urlPath;
		WWW www = new WWW (urlPath);
		yield return StartCoroutine(new WWWRequest(www));
		image.texture = www.texture;
	}


}
