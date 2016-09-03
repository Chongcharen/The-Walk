using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
public class ContentManager : MonoBehaviour {
	public RectTransform[] pages;
	public Button b_food,b_shop,b_profile,b_howto,b_contact,b_back;
	public int pageIndex = 0;
	float currentX = 0;
	RectTransform currentPage,newPage;
	private RectTransform rect;
	bool canPress = true;
	Page oldPage = Page.FoodPage;
	Page activatePage = Page.FoodPage;



	void Start(){
		rect = GetComponent<RectTransform> ();
		b_food.onClick.AddListener (OpenFoodPage);
		b_shop.onClick.AddListener (OpenShopPage);
		b_profile.onClick.AddListener (OpenProfilePage);
		b_howto.onClick.AddListener (OpenHowtoPage);
		b_contact.onClick.AddListener (OpenContactPage);
		b_back.onClick.AddListener (OnBack);
		//b_continuetoPayment.onClick.AddListener()
	}
	void OnEnable(){
		MallEvent.OnSelectShop += MallEvent_OnSelectShop;
		MallEvent.OnSelectPage += MallEvent_OnSelectPage;
		MallEvent.OnSelectCategory += MallEvent_OnSelectCategory;
		MallEvent.OnSaveOrderComplete += MallEvent_OnSaveOrderComplete;
		MallEvent.OnUserLogoutComplete += MallEvent_OnUserLogoutComplete;
	}



	void OnDisable(){
		MallEvent.OnSelectShop -= MallEvent_OnSelectShop;
		MallEvent.OnSelectPage -= MallEvent_OnSelectPage;
		MallEvent.OnSelectCategory -= MallEvent_OnSelectCategory;
		MallEvent.OnSaveOrderComplete -= MallEvent_OnSaveOrderComplete;
		MallEvent.OnUserLogoutComplete += MallEvent_OnUserLogoutComplete;

	}



	void OnBack(){
		
		if (activatePage == Page.RegisterPage || activatePage == Page.LostPassword) {
			MallEvent.instance.SelectPage ((int)Page.LoginPage);
			oldPage = 0;
			return;
		}
		SetPage ((int)oldPage);
		if (oldPage == 0) {
			MallEvent.instance.SelectCategory (-2);
		}
	}
	void OpenFoodPage(){
		//pageIndex = 0;
		MallEvent.instance.SelectShop(-1);
		SetPage (0);
		MallEvent.instance.SelectFooterPage (0);
		MallEvent.instance.SelectCategory(-2);
		oldPage = Page.FoodPage;
	}
	void OpenShopPage(){
		//pageIndex = 1;
		//ArragePage ();
		SetPage(1);
		MallEvent.instance.SelectFooterPage (1);
		oldPage = Page.ShopPage;
	}
	void OpenProfilePage(){

		//MallEvent.instance.ProfileSliderOpen (true);
		if (!Profile.GetInstance.isLogin) {
			SetPage ((int)Page.LoginPage);
		} else {
			MallEvent.instance.ProfileSliderOpen (true);
		}
	}
	void OpenHowtoPage(){
		SetPage (3);
		MallEvent.instance.SelectFooterPage (2);
		oldPage = Page.HowtoPage;
	}
	void OpenContactPage(){
		SetPage (4);
		MallEvent.instance.SelectFooterPage (3);
		oldPage = Page.ContactUsPage;
	}
	void ArragePage(){
		GetComponent<RectTransform> ().DOLocalMoveX (pageIndex * -640, 0.05f);
		//MallEvent.instance.SelectShop();
	}
	void MallEvent_OnSelectCategory (int category_id)
	{
		if (category_id > -1) {
			b_back.gameObject.SetActive (true);
		} else {
			b_back.gameObject.SetActive (false);
		}
		Debug.Log ("category " + category_id);
		if (category_id == -1) {
			ServiceRequest.instance.GetHitMenu ();
		}
	}
	void SetPage(int index){
		if (index > 4) {
			b_back.gameObject.SetActive (true);
		} else {
			b_back.gameObject.SetActive (false);
		}
		if (!canPress)
			return;
		if (pageIndex == 2 && index == 0) {
			pageIndex = 0;
			MallEvent.instance.SelectShop (-1);
			return;
		}
		/*if (index == (int)Page.EditProfilePage) {
			MallEvent.instance.CallEditorProfilePage (0);
		}*/
		if (pageIndex > index) {
			SwapPage (index, -1);
			Previouspage ();
		} else if (pageIndex < index) {
			SwapPage (index, 1);
			NextPage ();
		} else {
			
		}


		pageIndex = index;

		activatePage = (Page)System.Enum.ToObject(typeof(Page) , pageIndex);

	}
	void NextPage(){
		canPress = false;
		rect.DOLocalMoveX(rect.anchoredPosition.x-640, 0.1f).OnComplete(SlideCompelte);
	}
	void Previouspage(){
		canPress = false;
		rect.DOLocalMoveX(rect.anchoredPosition.x+640, 0.1f).OnComplete(SlideCompelte);
	}
	void SwapPage(int indexPage ,int next){
		newPage = pages [indexPage];
		pages[indexPage].DOLocalMoveX (currentX + (640*next), 0);
		currentX = currentX + (640 * next);
	}
	void MallEvent_OnSelectPage (int page_id)
	{
		SetPage (page_id);
	}
	void MallEvent_OnSelectShop (int shop_id)
	{
		if (shop_id < 0)
			return;
		SetPage (2);
		oldPage = Page.ShopPage;
		b_back.gameObject.SetActive (true);
	}
	void SlideCompelte(){
		currentPage.DOLocalMoveX(1000000*(pageIndex +1),0);

		currentPage = newPage;
		newPage = null;
		canPress = true;
	}


	///Event
	/// 
	void MallEvent_OnSaveOrderComplete ()
	{
		SetPage ((int)Page.EditProfilePage);
		MallEvent.instance.CallEditorProfilePage ((int)ProfilePageSelect.OrderHistorySelectPage);
	}

	void MallEvent_OnUserLogoutComplete ()
	{
		SetPage (0);
	}
		

	void Update(){
		if (Input.GetKey (KeyCode.Escape)) {
			OnBack ();
		}

	}
}
