using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Mall : MonoBehaviour {
	private static Mall _instance;
	public static Mall GetInstance{
		get{
			if (_instance == null) {
				_instance = new GameObject ("MALL").AddComponent<Mall> ();
				DontDestroyOnLoad (_instance);
			}
			return _instance;
		}
	}
	public List<Province> provinceList;
	public List<Restaurant> shopList;
	public List<Restaurant> historyOrderList;
	public List<Food> foodList;
	public List<Promotion> promotionList;
	public List<CategoryFood> categoryList;
	public Dictionary<int,GameObject> shopBoard = new Dictionary<int, GameObject>();
	//public Dictionary<int,GameObject> foodBoard = new Dictionary<int, GameObject>();
	public List<GameObject> foodBoard = new List<GameObject>();


}

[Serializable]	
public class Restaurant {
	public int shop_id;
	public int open;
	public string name;
	public string address;
	public string phone;
	public string detail;
	public string images;
	public string timestamp;


}

[SerializeField]
public class CategoryFood{
	public int category_id;
	public string name;
	public string timestamp;
}

public class ImageFood{
	public string _images;
}

[Serializable]
public class Food :ImageFood{

	public int menu_id{ get; set;}
	public int price{ get; set;}
	public int shop_id{ get; set;}
	public int category_id{ get; set;}
	public string detail;
	public int open{ get; set;}
	public string name{ get; set;}
	public string images {
		get{
			return _images;
		}
		set{
			char[] chars = new char[] {','};
			string[] parts = value.Split(chars,
			StringSplitOptions.RemoveEmptyEntries);
			_images = parts [0];
		}

	}
	public string timestamp{ get; set;}

}
[SerializeField]
public class Province{
	public int PROVINCE_ID;
	public int PROVINCE_CODE;
	public string PROVINCE_NAME;
	public int GEO_ID;

}
[SerializeField]
public class District{
	public int AMPHUR_ID;
	public int AMPHUR_CODE;
	public string AMPHUR_NAME;
	public int GEO_ID;
	public int PROVINCE_ID;
}

[SerializeField]
public class PostCode{
	public int AMPHUR_ID;
	public int POST_CODE;
}




public enum Page{
	FoodPage = 0,
	ShopPage =1,
	FoodPageWithShopID = 2,
	HowtoPage =3,
	ContactUsPage =4,
	LoginPage = 5,
	RegisterPage =6,
	LostPassword = 7,
	PromotionPage = 8,
	EditProfilePage = 9,
	PlaceDeliveryPage = 10,
	PaymentPage = 11
}
public enum ProfilePageSelect{
	PersonalInformationPage = 0,
	OrderHistoryPage = 1,
	PlaceInformationPage =2,
	AddplacePage = 3,
	OrderHistorySelectPage =4

}

public class PlaceDelivery{
	public int address_id;
	public int member_id;
	public int menu_id;
	public int quantity;
	public int open;
	public string cache_id;
	public string address_name;
	public string fname;
	public string lname;
	public string company;
	public string address;
	public string amphur;
	public string province;
	public int postcode;
	public string email;
	public string phone;
	public string note;
	public string timestamp;
}