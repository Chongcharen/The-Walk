using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
public class FoodPage : MonoBehaviour {
	public RectTransform mainHeader,menubar,headerbar,canvas,searchbar;
	public Text header_txt;
	public Button b_search;

	public InputField search_input;
	int headerBar_index = 1;
	List<Food> foods;
	FoodBoard boardSelect;

	void Start(){
		headerbar.anchoredPosition = new Vector2(canvas.rect.width,headerbar.anchoredPosition.y);
		searchbar.anchoredPosition = new Vector2(-canvas.rect.width,headerbar.anchoredPosition.y);
		b_search.onClick.AddListener (OpenSearch);
		//search_input.onValueChanged.AddListener (OnSearch);
		search_input.onEndEdit.AddListener (str => CloseSearch ());
	}
	void OnEnable(){
		MallEvent.OnSelectShop += MallEvent_OnSelectShop;
		MallEvent.OnSelectCategory += MallEvent_OnSelectCategory;
		MallEvent.OnFoodHitLoadComplete += MallEvent_OnFoodHitLoadComplete;
	}



	void OnDisable(){
		MallEvent.OnSelectShop -= MallEvent_OnSelectShop;
		MallEvent.OnSelectCategory -= MallEvent_OnSelectCategory;
	}
	void MallEvent_OnSelectShop (int shop_id)
	{
		if (shop_id == -1) {
			mainHeader.DOLocalMoveX (0, 0.05f);
			for (int i = 0; i < Mall.GetInstance.foodBoard.Count; i++) {
					Mall.GetInstance.foodBoard [i].SetActive (true);
			}
		} else {
			for (int i = 0; i < Mall.GetInstance.foodBoard.Count; i++) {
				if (Mall.GetInstance.foodBoard [i].GetComponent<FoodBoard> ().restaurant.shop_id == shop_id) {
					Mall.GetInstance.foodBoard [i].SetActive (true);
				} else {
					Mall.GetInstance.foodBoard [i].SetActive (false);
				}
			}
			header_txt.text = Mall.GetInstance.shopList.Find (res => res.shop_id == shop_id).name;
			mainHeader.DOLocalMoveX (headerBar_index * -menubar.rect.width, 0.05f);
		}
	}
	void MallEvent_OnSelectCategory (int _category_id)
	{
		
		for (int i = 0; i < Mall.GetInstance.foodBoard.Count; i++) {
			if (_category_id == -2) {
				Mall.GetInstance.foodBoard [i].SetActive (true);
			} else {
				if (Mall.GetInstance.foodBoard [i].GetComponent<FoodBoard> ().category_id == _category_id) {
					Mall.GetInstance.foodBoard [i].SetActive (true);
				} else {
					Mall.GetInstance.foodBoard [i].SetActive (false);
				}
			}
		}

	}
	void MallEvent_OnFoodHitLoadComplete (List<Food> orderList)
	{
			for (int i = 0; i < Mall.GetInstance.foodBoard.Count; i++) {
			if (orderList[i].menu_id == Mall.GetInstance.foodBoard [i].GetComponent<FoodBoard> ().food.menu_id) {
					Mall.GetInstance.foodBoard [i].SetActive (true);
				} else {
					Mall.GetInstance.foodBoard [i].SetActive (false);
				}
			}
	}



	void OpenSearch(){
		search_input.onValueChanged.AddListener (OnSearch);
			mainHeader.DOLocalMoveX (headerBar_index * menubar.rect.width, 0.05f);
	}
	void CloseSearch(){
		search_input.onValueChanged.RemoveListener (OnSearch);
		mainHeader.DOLocalMoveX (0 * -menubar.rect.width, 0.05f);
	}
	void OnSearch(string search){

		if (search.Length <= 0) {
			for (int i = 0; i < Mall.GetInstance.foodBoard.Count; i++) {
				Mall.GetInstance.foodBoard [i].SetActive (true);
			}
		} else {
			foods = Mall.GetInstance.foodList.FindAll (f => f.name.Contains (search));
			for (int i = 0; i < Mall.GetInstance.foodBoard.Count; i++) {
				boardSelect = Mall.GetInstance.foodBoard [i].GetComponent<FoodBoard> ();
				if (boardSelect.food.name.Contains (search) || boardSelect.detail_txt.text.Contains(search)) {
					Mall.GetInstance.foodBoard [i].SetActive (true);
				} else {
					Mall.GetInstance.foodBoard [i].SetActive (false);
				}
			}
		}
	}

}
