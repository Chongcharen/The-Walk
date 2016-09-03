using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;
using LitJson;
//using Pathfinding.Serialization.JsonFx;
//using LitJson;
public class ServiceRequest : MonoBehaviour {

	// Use this for initialization
	public static ServiceRequest instance;
	private ServiceResponse response;
	[Serializable]
	public class MyClass
	{
		public int level;
		public float timeElapsed;
		public string playerName;
	}


	void Awake(){
		instance = this;
	}
	void Start(){
		
		LitJson.JsonMapper.RegisterImporter<double, float>(input => Convert.ToSingle(input));
		LitJson.JsonMapper.RegisterImporter<int, long>(input => Convert.ToInt64(input));
		LitJson.JsonMapper.RegisterImporter<string, decimal>(input => Convert.ToDecimal(input));
		LitJson.JsonMapper.RegisterImporter<string, int>(input => Convert.ToInt32(input));
		LitJson.JsonMapper.RegisterImporter<string, float>(input => Convert.ToSingle(input));


		LoadDataFormServer ();
		//GetOrderHistory ();
		//StartCoroutine (LoadDistrict ());


	}
	public void LoadDataFormServer(){

		StartCoroutine (LoadShop ());
		StartCoroutine (LoadFood ());
		StartCoroutine (LoadCategory ());
		StartCoroutine (LoadProvince ());
		LoadPromotion ();
	}
	public void LoadPromotion(){
		StartCoroutine (Request_LoadPromotion ());
	}
	IEnumerator Request_LoadPromotion(){
		WWW www = new WWW ("http://thewalklifestylemall.com/service/getAllPromotion");
		yield return StartCoroutine(new WWWRequest(www));

		if (www.isDone) {

			if (Mall.GetInstance.promotionList == null) {
				List<Promotion> promos = JsonHelper.FromJsonList<Promotion>(www.text);
			
				Mall.GetInstance.promotionList = promos;
				promos = null;
				MallEvent.instance.PromotionLoadComplete ();
			}


		}
	}
	IEnumerator LoadShop(){
		WWW www = new WWW ("http://thewalklifestylemall.com/service/getallshop");
		yield return StartCoroutine(new WWWRequest(www));
		if (www.isDone) {
			if (Mall.GetInstance.shopList == null) {
				List<Restaurant> shop = JsonHelper.FromJsonList<Restaurant>(www.text);
				Mall.GetInstance.shopList = shop;
				shop = null;
			
				MallEvent.instance.ShopLoadComplete ();
		
			}

		}
		yield return new WaitForSeconds(5);
	}

	IEnumerator LoadFood(){

		WWW www = new WWW ("http://thewalklifestylemall.com/service/getallmenu");
		yield return StartCoroutine(new WWWRequest(www));

		if (www.isDone) {
			if (Mall.GetInstance.foodList == null) {
				List<Food> foods = JsonHelper.FromJsonList<Food>(www.text);
				Mall.GetInstance.foodList = foods;
				foods = null;
				yield return new WaitForSeconds(1);
				MallEvent.instance.FoodLoadComplete ();
			}

		}
	}

	IEnumerator LoadCategory(){
		WWW www = new WWW ("http://thewalklifestylemall.com/service/getAllCategory");
		yield return StartCoroutine(new WWWRequest(www));

		if (www.isDone) {
			if (Mall.GetInstance.categoryList == null) {
			//	List<CategoryFood> category =  JsonHelper.FromJsonList<CategoryFood>(www.text);
				List<CategoryFood> category = JsonHelper.FromJsonList<CategoryFood>(www.text);
				Mall.GetInstance.categoryList = category;
				category = null;
				MallEvent.instance.CategoryLoadComplete ();
			}

		}

	}

	IEnumerator LoadProvince(){
		WWW www = new WWW ("http://www.thewalklifestylemall.com/service/getAllProvince");
		yield return StartCoroutine(new WWWRequest(www));

		if (www.isDone) {
			if (Mall.GetInstance.provinceList == null) {
				List<Province> provinces = JsonHelper.FromJsonList<Province>(www.text);
				Mall.GetInstance.provinceList = provinces;
				provinces = null;
				MallEvent.instance.ProvinceLoadComplete ();
			}

		}
	}

	public IEnumerator LoadDistrict(int province_id){
		WWWForm form = new WWWForm ();
		form.AddField ("PROVINCE_ID", province_id);
		WWW www = new WWW ("http://www.thewalklifestylemall.com/service/getAmphurByPROVINCE_ID",form);
		yield return StartCoroutine(new WWWRequest(www));

		if (www.isDone) {
			List<District> districts = JsonHelper.FromJsonList<District>(www.text);
			MallEvent.instance.DistrictLoadComplete (districts);
		}

	}


	public IEnumerator LoadPost_code(int district_id){

		WWWForm form = new WWWForm ();
		form.AddField ("AMPHUR_ID", district_id);
		WWW www = new WWW ("http://www.thewalklifestylemall.com/service/getPostcodeByAMPHUR_ID",form);
		yield return StartCoroutine(new WWWRequest(www));

		if (www.isDone) {
			List<PostCode> postCodes = JsonHelper.FromJsonList<PostCode>(www.text);
			MallEvent.instance.PostCodeLoadComplete (postCodes [0]);
		}

	}




	/*addtocart	{menu_id,member_id,cache_id}	สำหรับเพิ่มเมนูลงตระกร้า	ถ้าไม่ได้ล๊อกอินไม่ต้องส่ง member_id มา 
	ให้ส่ง cache_id มาแทน โดยที่ cache_id ให้แรนดอมตัวเลขกับตัวอักษรมา 8 ตัว ไว้อ้างอิงกรณีไม่ล๊อกอิน*/
	public void AddToCart(int menu_id,string foodName){
		if (Profile.GetInstance.isLogin) {
			StartCoroutine (Request_AddToCart (menu_id,GetMemberID (),foodName));
		} else {
			MallEvent.instance.SelectPage ((int)Page.LoginPage);
		}
	}
	public void RemoveFormCart (int menu_id){
		StartCoroutine (Request_RemoveFormCart (menu_id,GetMemberID ()));
	}
	public void GetMyCart(){
		StartCoroutine ("Request_GetMyCart",Profile.GetInstance.user.member_id);
	}

	public void GetHitMenu(){
		StartCoroutine ("Request_GetHitMenu");
	}

	public void GetOrderHistory(){
		StartCoroutine (Request_GetOrderHistory());
	}
	public void GetHistoryView(int order_id){
		StartCoroutine (Request_GetHistoryView(order_id));
	}
	public void SaveOrder(string cash_code,string promo_code){
		StartCoroutine (Request_SaveOrder (cash_code,promo_code));
	}
	public void GetNewPassword(string email){
		StartCoroutine (Request_GetNewPassword (email));
	}
	//user forgetpassword
	IEnumerator Request_GetNewPassword(string email){

		WWWForm form = new WWWForm ();
		if (string.IsNullOrEmpty(email)) {

			yield return null;
		}
		form.AddField ("email", email);
		WWW www = new WWW ("http://thewalklifestylemall.com/service/forgotPassword",form);

		yield return StartCoroutine(new WWWRequest(www));

		if (www.isDone) {
			ServiceResponse response = JsonHelper.FromSingleJson<ServiceResponse>(www.text);
			PopupManager.instance.ShowAlertPopup (response.description);
		}
	}
	//cart
	IEnumerator Request_AddToCart(int menu_id,string member_id,string foodName){
		WWWForm form = new WWWForm ();
		if (menu_id < 0 || string.IsNullOrEmpty(member_id)) {
			yield return null;
		}
		form.AddField ("menu_id", menu_id);
		form.AddField("member_id",member_id);
		form.AddField("cache_id","");
		WWW www = new WWW ("http://thewalklifestylemall.com/service/addtocart",form);

		yield return StartCoroutine(new WWWRequest(www));

		if (www.isDone) {
			response = JsonHelper.FromSingleJson<ServiceResponse>(www.text);
			PopupManager.instance.ShowAlertPopup ("เพิ่ม "+foodName+" แล้ว");
		}

	}
	IEnumerator Request_RemoveFormCart(int menu_id,string member_id){
		WWWForm form = new WWWForm ();
		if (menu_id <0 || string.IsNullOrEmpty(member_id)) {

			yield return null;
		}
		form.AddField ("menu_id", menu_id);
		form.AddField("member_id",member_id);
		form.AddField("cache_id","");
		WWW www = new WWW ("http://thewalklifestylemall.com/service/removeformcart",form);

		yield return StartCoroutine(new WWWRequest(www));
		if (www.isDone) {
			print(www.text);
			StartCoroutine(Request_GetMyCart (Profile.GetInstance.user.member_id));
		}

	}
	IEnumerator Request_GetMyCart(int member_id){
		
		WWWForm form = new WWWForm ();
		form.AddField ("member_id", member_id);
		WWW www = new WWW("http://thewalklifestylemall.com/service/getMyCart",form);
		yield return StartCoroutine(new WWWRequest(www));
		if (www.isDone) {
			Profile.GetInstance.carts = JsonHelper.FromJsonList<Cart>(www.text);
			MallEvent.instance.MyCartLoadComplete();
		}
	}
	//Order
	//	saveorder	{member_id,cache_id,cart_id,address_id,cash_code,promo_code,email,name}	สำหรับบันทึกออเดอร์หลังจากกดสั่งซื้อแล้ว
	IEnumerator Request_SaveOrder(string cash_code,string promo_code){
		
		WWWForm form = new WWWForm ();
		form.AddField ("member_id", Profile.GetInstance.user.member_id);
		form.AddField ("cache_id", "");
		form.AddField ("address_id",Profile.GetInstance.address_id);
		form.AddField ("cash_code",cash_code);
		form.AddField ("promo_code",promo_code);
		form.AddField ("email",Profile.GetInstance.user.email);
		form.AddField ("name",Profile.GetInstance.user.fname);
		WWW www = new WWW ("http://thewalklifestylemall.com/service/saveorder", form);
		yield return StartCoroutine(new WWWRequest(www));
		if (www.isDone) {
			OrderSelect cartsOrder =JsonHelper.FromSingleJson<OrderSelect> (www.text);
			MallEvent.instance.SaveOrderComplete ();
			MallEvent.instance.OrderHistorySelectLoadComplete (cartsOrder.cart,true,cartsOrder.order_code);
		}
	}

	IEnumerator Request_GetOrderHistory(){
		
		WWWForm form = new WWWForm ();
		form.AddField ("member_id", Profile.GetInstance.user.member_id);
		WWW www = new WWW ("http://thewalklifestylemall.com/service/getOrderHistory",form);
		yield return StartCoroutine(new WWWRequest(www));
		if (www.isDone) {
			List<Order> orderList = JsonHelper.FromJsonList<Order>(www.text);
			MallEvent.instance.OrderHistoryLoadComplete (orderList);
		}
	}

	IEnumerator Request_GetHistoryView(int order_id){
		WWWForm form = new WWWForm ();
		form.AddField ("order_id", order_id);
		WWW www = new WWW ("http://thewalklifestylemall.com/service/historyview",form);
		yield return StartCoroutine(new WWWRequest(www));
		if (www.isDone) {
			
			List<Cart> orderHistoryList = JsonHelper.FromJsonList<Cart>(www.text);
			MallEvent.instance.OrderHistorySelectLoadComplete (orderHistoryList,false);
			//MallEvent.instance.OrderHistoryLoadComplete (orderList);
		}
	}

	IEnumerator Request_GetHitMenu(){
		Debug.Log ("Reauest Githitmenu");
		//WWWForm form = new WWWForm ();
		//form.AddField ("order_id", order_id);
		WWW www = new WWW ("http://thewalklifestylemall.com/service/getHitMenu");
		yield return StartCoroutine(new WWWRequest(www));
		if (www.isDone) {
			List<Food> foodHitList = JsonHelper.FromJsonList<Food>(www.text);
			MallEvent.instance.FoodHitLoadComplete (foodHitList);
		}
	}




	T GetObject<T>(Dictionary<string,object> dict)
	{
		Type type = typeof(T);
		var obj = Activator.CreateInstance(type);

		foreach (var kv in dict)
		{
			type.GetProperty(kv.Key).SetValue(obj,kv.Value,null);
		}
		return (T)obj;
	}
	string GetMemberID(){
		if (Profile.GetInstance.isLogin) {
			return Profile.GetInstance.user.member_id.ToString();
		} else {
			return Profile.GetInstance.cache_id;
		}

	}

}

class TestClass
{
	public string Name;
	public int Id;
}

public class Person {
	public string Name;
	public int Age;
}
[System.Serializable]
public class ServiceResponse{
	public bool success;
	public string description;

}

[Serializable]
public class Cart{
	public int cart_id;
	public int order_id;
	public int member_id;
	public string cache_id;
	public int menu_id;
	public int quantity;
	public int open;
	public int shop_id;
	public int category_id;
	public float price;
	public string name;

	public string images;
	public string detail;
	public string timestamp;
}

[Serializable]
public class Order{
	public int order_id;
	public int member_id;
	public string address_id;
	public string cache_id;
	public int cart_id;
	public int price;
	public int total;
	public string order_code;
	public string promo_code;
	public string cash_code;
	public string order_status;
	public int open;
	public int menu_id;
	public int quantity;
	public string timestamp;
}

[Serializable]
public class Promotion{
	public int promo_id;
	public string promo_header;
	public string promo_code;
	public string promo_images;
	public string promo_expire;
	public string promo_detail;
	public int open;
	public string timestamp;

}

[Serializable]
public class OrderSelect{
	public bool success;
	public string description;
	public string order_code;
	public List<Cart> cart;

}

	

