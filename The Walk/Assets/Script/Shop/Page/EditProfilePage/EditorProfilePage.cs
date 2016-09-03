using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
public class EditorProfilePage : MonoBehaviour {
	public Button b_information,b_history,b_place,b_addPlace;
	public RectTransform[] pages;
	public int pageIndex = 0;
	float currentX = 0;
	RectTransform currentPage,newPage;
	public RectTransform rect;
	bool canPress = true;

	public Color colorSelect,colorDeselect;
	void OnEnable(){
		MallEvent.OnSaveOrderComplete += MallEvent_OnSaveOrderComplete;
		MallEvent.OnCallEditorProfilePage += MallEvent_OnCallEditorProfilePage;
		AddressDeliveryGrid.OnEditAddress_Delivery += EditAddress_OnEditAddress_Delivery;

	}




	void OnDisable(){
		MallEvent.OnSaveOrderComplete -= MallEvent_OnSaveOrderComplete;
		AddressDeliveryGrid.OnEditAddress_Delivery -= EditAddress_OnEditAddress_Delivery;
		MallEvent.OnCallEditorProfilePage -= MallEvent_OnCallEditorProfilePage;
	}
	void Start(){
		b_information.onClick.AddListener (()=>SetPage (0));
		b_history.onClick.AddListener (OnOpenHistoryPage);
		b_place.onClick.AddListener (()=>SetPage (2));
		b_addPlace.onClick.AddListener (OnAddAddress);
	}
	void OnOpenHistoryPage(){
		ServiceRequest.instance.GetOrderHistory ();
		SetPage ((int)ProfilePageSelect.OrderHistoryPage);
	}


	void MallEvent_OnSaveOrderComplete ()
	{
		SetPage ((int)ProfilePageSelect.OrderHistorySelectPage);
		SetColorButton (1);
	}
	void OnAddAddress(){
		MallEvent.instance.OpenAddressDelivery (true,null);
		SetPage (3);
	}
	void SetPage(int index){
		if (!canPress)
			return;
		if (pageIndex > index) {
			SwapPage (index, -1);
			Previouspage ();
		} else if (pageIndex < index) {
			SwapPage (index, 1);
			NextPage ();
		} else {

		}
		pageIndex = index;
		SetColorButton (index);
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
	void SlideCompelte(){
		currentPage.DOLocalMoveX(10000*(pageIndex+1),0);
		currentPage = newPage;
		newPage = null;
		canPress = true;
	}
	void EditAddress_OnEditAddress_Delivery (PlaceDelivery place)
	{
		MallEvent.instance.OpenAddressDelivery (false,place);
		SetPage (3);
	}
	void MallEvent_OnCallEditorProfilePage (int page)
	{
		SetPage (page);
	}

	void SetColorButton(int index){
		switch (index) {
		case 0:
			OnSelectButton(b_information);
			OnDeselectButton(b_history);
			OnDeselectButton(b_place);
			break;
		case 1:
			OnDeselectButton(b_information);
			OnSelectButton(b_history);
			OnDeselectButton(b_place);
			break;
		case 2:
			OnDeselectButton(b_information);
			OnDeselectButton(b_history);
			OnSelectButton(b_place);
			break;
		case 3:
			OnDeselectButton(b_information);
			OnDeselectButton(b_history);
			OnSelectButton(b_place);
			break;
		}
	}



	void OnSelectButton (Button b){
		b.GetComponent<Image> ().color = colorSelect;
		b.GetComponentInChildren<Text> ().color = colorDeselect;
	}

	void OnDeselectButton (Button b){
		b.GetComponent<Image> ().color = colorDeselect;
		b.GetComponentInChildren<Text> ().color = colorSelect;
	}
}
