using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
public class AddAddress : MonoBehaviour {

	// Use this for initialization
	public Dropdown dd_district,dd_province;
	public Text provinceText,districtText;
	public InputField company_txt,name_txt,lastname_txt,email_txt,phone_txt,address_txt,postCode_txt;
	public Button b_back,b_addNewAddress,b_updateAddress;

	//
	List<District> districtList;
	Province province_select;
	District district_select;
	PlaceDelivery placeDelivery;
	ServiceResponse response;

	void Start () {
		dd_province.onValueChanged.AddListener (OnProvinceChange);
		dd_district.onValueChanged.AddListener (OnDistrictChange);
		b_back.onClick.AddListener (OnBack);
		b_addNewAddress.onClick.AddListener (OnAddNewAddress);
		b_updateAddress.onClick.AddListener (OnUpdateAddress);
	}

	void OnEnable(){
		MallEvent.OnProvinceLoadComplete += MallEvent_OnProvinceLoadComplete;
		MallEvent.OnDistrictLoadComplete += MallEvent_OnDistrictLoadComplete;
		MallEvent.OnPostCodeLoadComplete += MallEvent_OnPostCodeLoadComplete;
		MallEvent.OnOpenAddressDelivery += MallEvent_OnOpenAddressDelivery;
	}




	void OnDisable(){
		MallEvent.OnProvinceLoadComplete -= MallEvent_OnProvinceLoadComplete;
		MallEvent.OnDistrictLoadComplete -= MallEvent_OnDistrictLoadComplete;
		MallEvent.OnPostCodeLoadComplete -= MallEvent_OnPostCodeLoadComplete;
		MallEvent.OnOpenAddressDelivery -= MallEvent_OnOpenAddressDelivery;
	}

	void MallEvent_OnProvinceLoadComplete ()
	{
		List<string> provincesName = new List<string>(); 
		foreach(Province p in Mall.GetInstance.provinceList){
			provincesName.Add (p.PROVINCE_NAME);
		}


		dd_province.AddOptions (provincesName);

		dd_province.value = 0;
		OnProvinceChange (dd_province.value);

		//province_select = Mall.GetInstance.provinceList.Find (p => p.PROVINCE_NAME == dd_province.captionText.text);
		//StartCoroutine(ServiceRequest.instance.LoadDistrict (province_select.PROVINCE_ID));
	}
	void MallEvent_OnDistrictLoadComplete (List<District> districts)
	{
		
		if (dd_district.options.Count > 0)
			dd_district.options.Clear ();
		districtList = districts;
		List<string> districtNames = new List<string>(); 
		foreach(District d in districtList){
			districtNames.Add (d.AMPHUR_NAME);
		}
		dd_district.AddOptions (districtNames);

		dd_district.value = 0;
		OnDistrictChange (dd_district.value);


	}
	void MallEvent_OnPostCodeLoadComplete (PostCode postCode)
	{

		postCode_txt.text = postCode.POST_CODE.ToString ();

	}

	void OnProvinceChange(int id){
		province_select = Mall.GetInstance.provinceList.Find (p => p.PROVINCE_NAME == provinceText.text);
		StartCoroutine(ServiceRequest.instance.LoadDistrict (province_select.PROVINCE_ID));
	}
	void OnDistrictChange(int id){
		district_select = districtList.Find (d => d.AMPHUR_NAME == districtText.text);
		StartCoroutine (ServiceRequest.instance.LoadPost_code (district_select.AMPHUR_ID));
	}
	void MallEvent_OnOpenAddressDelivery (bool addAddress,PlaceDelivery place)
	{
		if (addAddress) {
			b_addNewAddress.gameObject.SetActive (true);
			b_updateAddress.gameObject.SetActive (false);
			ClearAddress ();
		} else {
			b_addNewAddress.gameObject.SetActive (false);
			b_updateAddress.gameObject.SetActive (true);
			SetUpAddress (place);
		}
	}
	void SetUpAddress(PlaceDelivery p){
		placeDelivery = p;
		company_txt.text = p.company;
		name_txt.text = p.fname;
		lastname_txt.text = p.lname;
		email_txt.text = p.email;
		phone_txt.text = p.phone;
		address_txt.text = p.address;
		dd_province.captionText.text = p.province;
		province_select = Mall.GetInstance.provinceList.Find (pv => pv.PROVINCE_NAME == provinceText.text);
		StartCoroutine(ServiceRequest.instance.LoadDistrict (province_select.PROVINCE_ID));
		districtText.text = p.amphur;
		postCode_txt.text = p.postcode.ToString();
	}
	void ClearAddress (){
		company_txt.text = "";
		name_txt.text = "";
		lastname_txt.text = "";
		email_txt.text = "";
		phone_txt.text = "";
		address_txt.text = "";
		//districtText.text = "";
		//postCode_txt.text = "";
	}
	void OnBack(){
		MallEvent.instance.CallEditorProfilePage ((int)ProfilePageSelect.PlaceInformationPage);

	}
	//{member_id,cache_id,address_name,fname,lname,company,address,amphur,province,postcode,email,phone}	
	void OnAddNewAddress(){
		
		if (CheckInput ()) {
			StartCoroutine (AddNewAddress ());
		}
	}
	void OnUpdateAddress(){
		if (CheckInput ()) {
			StartCoroutine (UpdateAddress ());
		}
	}

	IEnumerator AddNewAddress(){

		WWWForm form = new WWWForm ();
		form.AddField ("member_id",Profile.GetInstance.user.member_id);
		form.AddField ("cache_id","");
		form.AddField ("address_name",address_txt.text); //ชื่อสถานที่จัดส่ง
		form.AddField ("fname",name_txt.text);
		form.AddField ("lname",lastname_txt.text);
		form.AddField ("company",company_txt.text);
		form.AddField ("address",address_txt.text);
		form.AddField ("amphur",district_select.AMPHUR_NAME.ToString());
		form.AddField ("province",province_select.PROVINCE_NAME);
		form.AddField ("postcode",postCode_txt.text);
		form.AddField ("email",email_txt.text);
		form.AddField ("phone",phone_txt.text);
		WWW www = new WWW ("http://www.thewalklifestylemall.com/service/setMyAddress", form);
		yield return www;
		if (www.isDone) {
			response = JsonHelper.FromSingleJson<ServiceResponse> (www.text);
			PopupManager.instance.ShowAlertPopup (response.description);
			if (response.success) {
				MallEvent.instance.AddressDeliveryUpdate ();
				if (Profile.GetInstance.doing == UserDoing.PaymentButNoAddress) {
					MallEvent.instance.SelectPage ((int)Page.PlaceDeliveryPage);
				} else {
					OnBack ();
				}
			}

		}

	}

	//updateMyAddress	{member_id,cache_id,address_id,address_name,fname,lname,company,address,amphur,province,postcode,email,phone}	อัพเดทที่อยู่เก่า ส่ง address_id มาด้วย	amphur และ province ส่งเป็นรหัสอำเภอและจังหวัดมา
	IEnumerator UpdateAddress(){
		WWWForm form = new WWWForm ();
		form.AddField ("member_id",Profile.GetInstance.user.member_id);
		form.AddField ("cache_id","");
		form.AddField ("address_id",placeDelivery.address_id);
		form.AddField ("address_name",address_txt.text); //ชื่อสถานที่จัดส่ง
		form.AddField ("fname",name_txt.text);
		form.AddField ("lname",lastname_txt.text);
		form.AddField ("company",company_txt.text);
		form.AddField ("address",address_txt.text);
		form.AddField ("amphur",dd_district.captionText.text);
		form.AddField ("province",dd_province.captionText.text);
		form.AddField ("postcode",postCode_txt.text);
		form.AddField ("email",email_txt.text);
		form.AddField ("phone",phone_txt.text);
		WWW www = new WWW ("http://www.thewalklifestylemall.com/service/updateMyAddress", form);
		yield return www;
		if (www.isDone) {
			response = JsonConvert.DeserializeObject<ServiceResponse> (www.text);
			PopupManager.instance.ShowAlertPopup (response.description);
			if (response.success) {
				MallEvent.instance.AddressDeliveryUpdate ();
			}
		}

	}


	bool CheckInput(){
		if(string.IsNullOrEmpty(dd_province.captionText.text) || string.IsNullOrEmpty(dd_district.captionText.text) || string.IsNullOrEmpty(address_txt.text) || string.IsNullOrEmpty(name_txt.text) || string.IsNullOrEmpty(lastname_txt.text) || string.IsNullOrEmpty(company_txt.text)||
			string.IsNullOrEmpty(postCode_txt.text) || string.IsNullOrEmpty(email_txt.text) || string.IsNullOrEmpty(phone_txt.text)){
			PopupManager.instance.ShowAlertPopup ("กรุณากรอกข้อมูลให้ครบ");
			return false;
		}




		if (!Utils.IsValidEmailAddress(email_txt.text)) {
			PopupManager.instance.ShowAlertPopup ("กรุณากรอกอีเมล์ให้ถูกต้อง");
			return false;
		}
		if (string.IsNullOrEmpty (phone_txt.text)) {
			PopupManager.instance.ShowAlertPopup ("กรุณากรอกหมายเลขโทรศัพท์");
			return false;
		}

		return true;
	}
}
