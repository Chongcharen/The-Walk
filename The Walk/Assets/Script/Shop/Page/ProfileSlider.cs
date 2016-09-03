using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
public class ProfileSlider : MonoBehaviour {

	public RectTransform rect;
	public Image blackLayout;
	public Button b_promotion,b_payment,b_contact,b_profile,b_logout;
	public Text name_txt,cart_txt,price_txt;
	float price = 0;
	void Start(){
		b_promotion.onClick.AddListener (OpenPromotion);
		b_payment.onClick.AddListener (OpenPlaceDelivery);
		b_contact.onClick.AddListener (OpenContact);
		b_profile.onClick.AddListener (OpenProfileEditor);
		b_logout.onClick.AddListener (OnLogout);

		GetComponent<Button> ().onClick.AddListener (OnCloseProfileSlider);
	}
	void OnEnable(){
		MallEvent.OnProfileSliderOpen += MallEvent_OnProfileSliderOpen;
		MallEvent.OnUserLoginComplete += MallEvent_OnUserLoginComplete;
		MallEvent.OnMyCartLoadComplete += MallEvent_OnMyCartLoadComplete;
	}

	void OnDisable(){
		MallEvent.OnProfileSliderOpen -= MallEvent_OnProfileSliderOpen;
		MallEvent.OnUserLoginComplete -= MallEvent_OnUserLoginComplete;
		MallEvent.OnMyCartLoadComplete -= MallEvent_OnMyCartLoadComplete;
	}
	void OnLogout(){
		Profile.GetInstance.Logout ();
		MallEvent_OnProfileSliderOpen (false);
	}
	void MallEvent_OnProfileSliderOpen (bool isOpen)
	{
		GetComponent<CanvasGroup> ().blocksRaycasts = isOpen;
		blackLayout.enabled = isOpen;
		if (isOpen) {
			rect.DOLocalMoveX (0, 0.05f);
			ServiceRequest.instance.GetMyCart ();
		} else {
			rect.DOLocalMoveX (640, 0.05f);
		}
	}

	void OnCloseProfileSlider(){
		MallEvent_OnProfileSliderOpen (false);
	}
	void OpenPromotion(){
		MallEvent_OnProfileSliderOpen (false);
		MallEvent.instance.SelectPage ((int)Page.PromotionPage);
	}
	void OpenContact(){
		MallEvent_OnProfileSliderOpen (false);
		MallEvent.instance.SelectPage ((int)Page.ContactUsPage);
	}
	void OpenPlaceDelivery(){
		MallEvent_OnProfileSliderOpen (false);
		if (Profile.GetInstance.placeDeliverys.Count <= 0) {
			Profile.GetInstance.doing = UserDoing.PaymentButNoAddress;
			MallEvent.instance.SelectPage ((int)Page.EditProfilePage);
			MallEvent.instance.CallEditorProfilePage ((int)ProfilePageSelect.AddplacePage);
		} else {
			MallEvent.instance.SelectPage ((int)Page.PlaceDeliveryPage);
		}
	}
	void OpenProfileEditor(){
		MallEvent_OnProfileSliderOpen (false);
		MallEvent.instance.SelectPage ((int)Page.EditProfilePage);
		MallEvent.instance.CallEditorProfilePage ((int)ProfilePageSelect.PersonalInformationPage);
	}

	void MallEvent_OnUserLoginComplete ()
	{
		if (string.IsNullOrEmpty (Profile.GetInstance.user.fname) && string.IsNullOrEmpty (Profile.GetInstance.user.lname)) {
			name_txt.text = Profile.GetInstance.user.email;
		} else {
			name_txt.text = Profile.GetInstance.user.fname + " " + Profile.GetInstance.user.lname;
		}
	}

	void MallEvent_OnMyCartLoadComplete ()
	{
		price = 0;
		cart_txt.text = "[ "+Profile.GetInstance.carts.Count+" ]";
		foreach (Cart c in Profile.GetInstance.carts) {
			price += c.price * c.quantity;
		}
		price_txt.text = UIHelper.SetCurrencyWithoutK(price)+ " THB";

	}
}
