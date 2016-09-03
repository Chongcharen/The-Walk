using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MallEvent : MonoBehaviour {

	public delegate void PromotionMenuLoadComplete();
	public static event PromotionMenuLoadComplete OnPromotionLoadComplete;

		public delegate void FoodMenuLoadComplete();
		public static event FoodMenuLoadComplete OnFoodLoadComplete;

		public delegate void ShopMenuLoadComplete();
		public static event ShopMenuLoadComplete OnShopLoadComplete;

		public delegate void CategoryMenuLoadComplete();
		public static event CategoryMenuLoadComplete OnCategoryLoadComplete;

		public delegate void ProvinceMenuLoadComplete();
		public static event ProvinceMenuLoadComplete OnProvinceLoadComplete;

		public delegate void DistrictMenuLoadComplete(List<District> districts);
		public static event DistrictMenuLoadComplete OnDistrictLoadComplete;

		public delegate void PostCodeMenuLoadComplete(PostCode postCode);
		public static event PostCodeMenuLoadComplete OnPostCodeLoadComplete;

		public delegate void GameOverEvent (bool isGameOver);
		public static event GameOverEvent OnGameOver;

		public delegate void DataUpdateEvent();
		public static event DataUpdateEvent OnDataUpdate;

		public delegate void SceneEvent(string mode);	
		public static event SceneEvent OnSceneDispatch;
		

		public delegate void SelectShopEvent(int shop_id);
		public static event SelectShopEvent OnSelectShop;

		public delegate void SelectCategoryEvent(int category_id);
		public static event SelectCategoryEvent OnSelectCategory;

		public delegate void SelectPageEvent(int page_id);
		public static event SelectPageEvent OnSelectPage;

		public delegate void ProfileSliderOpenEvent(bool isOpen);
		public static event ProfileSliderOpenEvent OnProfileSliderOpen;
		
		public delegate void OpenAddressDeliveryEvent(bool addAddress,PlaceDelivery place);
		public static event OpenAddressDeliveryEvent OnOpenAddressDelivery;

		public delegate void AddressDeliveryUpdateEvent();
		public static event AddressDeliveryUpdateEvent OnAddressDeliveryUpdate;

		public delegate void AddressDeliveryUpdateCompleteEvent();
		public static event AddressDeliveryUpdateCompleteEvent OnAddressDeliveryUpdateComplete;

			//userEvent

		public delegate void UserLoginCompleteEvent();
		public static event UserLoginCompleteEvent OnUserLoginComplete;

		public delegate void UserLogoutCompleteEvent();
		public static event UserLogoutCompleteEvent OnUserLogoutComplete;

		//Payment
	public delegate void GetMyCartLoadCompleteEvent();
	public static event GetMyCartLoadCompleteEvent OnMyCartLoadComplete;

	public delegate void FootHitLoadCompleteEvent(List<Food> orderList);
	public static event FootHitLoadCompleteEvent OnFoodHitLoadComplete;

	//Order
	public delegate void OrderHistoryLoadCompleteEvent(List<Order> orderList);
	public static event OrderHistoryLoadCompleteEvent OnOrderHistoryLoadComplete;

	public delegate void OrderHistorySelectLoadCompleteEvent(List<Cart> cartList,bool isNewOrder,string order_code);
	public static event OrderHistorySelectLoadCompleteEvent OnOrderHistorySelectLoadComplete;

	public delegate void SaveOrderCompleteEvent();
	public static event SaveOrderCompleteEvent OnSaveOrderComplete;


	//EditorProfilePage
	public delegate void CallEditorProfilePageEvent(int page);
	public static event CallEditorProfilePageEvent OnCallEditorProfilePage;

	//Search

	public delegate void SearchFoodEvent(string name);
	public static event SearchFoodEvent OnSearchFood;

	//footer
	public delegate void SelectFooterPageEvent(int id);
	public static event SelectFooterPageEvent OnSelectFooterPageEvent;


	public delegate void SelectPromotionEvent(int id);
	public static event SelectPromotionEvent OnSelectPromotion;

	//Back on android

	public delegate void BackDeviceCallEvent();
	public static event BackDeviceCallEvent OnBackDeviceCall;



		private static MallEvent _instance;
		public static MallEvent instance
		{
				get {
						if(_instance == null){
							_instance = new GameObject("EventManager").AddComponent<MallEvent>();
						}
						DontDestroyOnLoad(_instance);
						return _instance;
				}
		}
		

		public void PromotionLoadComplete(){
			OnPromotionLoadComplete ();
		}
		public void FoodLoadComplete(){
			OnFoodLoadComplete ();
		}
		public void ShopLoadComplete(){
			OnShopLoadComplete ();
		}
		public void CategoryLoadComplete(){
		    OnCategoryLoadComplete ();
		}
		public void ProvinceLoadComplete(){
			OnProvinceLoadComplete ();	
		}
		public void DistrictLoadComplete(List<District> districts){
			OnDistrictLoadComplete (districts);	
		}
		public void PostCodeLoadComplete(PostCode postCode){
			OnPostCodeLoadComplete (postCode);	
		}

	public void OpenAddressDelivery(bool addAddress,PlaceDelivery place){
		OnOpenAddressDelivery (addAddress,place);
	}
	public void AddressDeliveryUpdate(){
		OnAddressDeliveryUpdate ();
	}


	public void AddressDeliveryUpdateComplete(){
		OnAddressDeliveryUpdateComplete();
	}

	//User login complete
		public void UserLoginComplete(){
			OnUserLoginComplete ();
		}
	public void UserLogoutComplete(){
		OnUserLogoutComplete ();
	}
	public void FoodHitLoadComplete(List<Food> foodHitList){
		OnFoodHitLoadComplete (foodHitList);
	}



	//cart
	public void MyCartLoadComplete(){
		OnMyCartLoadComplete ();
	}
	public void OrderHistoryLoadComplete(List<Order> orderList){
		OnOrderHistoryLoadComplete (orderList);
	}
	public void OrderHistorySelectLoadComplete(List<Cart> cartList,bool isNewOrder,string order_code = ""){
		OnOrderHistorySelectLoadComplete (cartList,isNewOrder,order_code);
	}
	public void SaveOrderComplete(){
		OnSaveOrderComplete ();
	}





	public void SelectPromotion(int id){
		OnSelectPromotion (id);
	}

	public void SelectFooterPage(int id){
		OnSelectFooterPageEvent (id);
	}

	//search

	public void SearchFood(string name){
		OnSearchFood (name);
	}

	//Back device callEvent
	public void BackDeviceCall(){
		OnBackDeviceCall ();
	}


	//EditorProfilePage
	public void CallEditorProfilePage(int page){
		OnCallEditorProfilePage (page);
	}

		public void SelectPage(int page_id){
			OnSelectPage (page_id);
		}
		public void SelectShop(int shop_id){
			OnSelectShop (shop_id);
		}
		public void SelectCategory(int category_id){
			OnSelectCategory (category_id);
		}
		public void ProfileSliderOpen(bool isOpen){
			OnProfileSliderOpen (isOpen);
		}
		public void DataUpdate(){
				Debug.Log ("Broadcast DataUpdate");
				OnDataUpdate ();
		}
		public void GameOver(){
				OnGameOver (true);
		}
		public void Check(){
			//	Debug.Log ("initialize OK");
		}
		public void SceneDispatch(string mode){
			OnSceneDispatch (mode);
		}
}
	
