using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PromotionLabel : MonoBehaviour {

	Promotion promo;
	public GameObject promoCode;
	public Button b_click_promo;
	public Text header_txt, detail_txt,promo_code_txt;
	public InputField pro_code_txt;
	public RawImage rawImage;
	public Image imgBackground, imgIcon;
	public int id = 0;
	void Start(){
		
		b_click_promo.onClick.AddListener (OnClickButton);
	}
	public void SetData(int _id ,Promotion p){
		id = _id;
		promo_code_txt.text = p.promo_code;
		pro_code_txt.text = p.promo_code;
		header_txt.text = p.promo_header;
		detail_txt.text = p.promo_detail;
		StartCoroutine (LoadImage (p.promo_images));

	}
	void OnClickButton(){
		MallEvent.instance.SelectPromotion (id);
	}

	void OnEnable(){
		MallEvent.OnSelectPromotion += MallEvent_OnSelectPromotion;
		MallEvent.OnSelectPage += MallEvent_OnSelectPage;
	}


		
	void OnDisable(){
		MallEvent.OnSelectPromotion -= MallEvent_OnSelectPromotion;
		MallEvent.OnSelectPage -= MallEvent_OnSelectPage;
	}
	void MallEvent_OnSelectPromotion (int promo_id)
	{
		if (id == promo_id) {
			imgBackground.color = StaticColor.instance.selectColor;
			imgIcon.color = StaticColor.instance.deselectColor;
			promoCode.SetActive (true);
			detail_txt.gameObject.SetActive (false);
			GUIUtility.systemCopyBuffer = pro_code_txt.text;
			//ClipboardHelper.clipBoard = promo_code_txt.text;
			//GUIUtility.systemCopyBuffer = promo_code_txt.text;

			UniClipboard.value = promo_code_txt.text;

		} else {
			imgBackground.color = StaticColor.instance.deselectColor;
			imgIcon.color = StaticColor.instance.selectColor;
			promoCode.SetActive (false);
			detail_txt.gameObject.SetActive (true);
		}
	}

	void MallEvent_OnSelectPage (int page_id)
	{
		if (page_id != (int)Page.PromotionPage) {

			MallEvent_OnSelectPromotion (-1);
		}
	}

	IEnumerator LoadImage(string urlPath){
		WWW www = new WWW (urlPath);
		yield return StartCoroutine(new WWWRequest(www));
		rawImage.texture = www.texture;
	}
}
