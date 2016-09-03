using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Text;

public class LoginPage : MonoBehaviour {
	public InputField username_input,password_input;
	public Button b_register,b_login,b_lostpassword;
	string urlService,tempEmail;
	ProfileForm form;
	ServiceResponse response;

	void Start(){
		b_register.onClick.AddListener (OnRegister);
		b_login.onClick.AddListener (OnLogin);
		b_lostpassword.onClick.AddListener (OnLostPasword);
	}
	void OnLogin(){
		form = ProfileForm.login;
		urlService = "http://thewalklifestylemall.com/service/login";
		CallServiceRequest (urlService);
	}
	void OnRegister(){
		MallEvent.instance.SelectPage ((int)Page.RegisterPage);
	}
	void OnLostPasword(){
		MallEvent.instance.SelectPage ((int)Page.LostPassword);
	}
	void CallServiceRequest(string urlPath){

		WWWForm wwwForm = new WWWForm ();
		tempEmail = username_input.text;
		wwwForm.AddField ("email", username_input.text);
		wwwForm.AddField ("password", password_input.text);
		WWW www = new WWW (urlPath,wwwForm);
		StartCoroutine (WaitForRequest (www));
	}
	IEnumerator WaitForRequest(WWW www){
		yield return StartCoroutine(new WWWRequest(www));
		if (www.error == null)
		{
			Debug.Log (www.text);
			response = JsonHelper.FromSingleJson<ServiceResponse> (www.text);//JsonConvert.DeserializeObject<ServiceResponse> (www.text);
			if (!response.success) {
				PopupManager.instance.ShowAlertPopup (response.description);
				yield break;
			}
			if (form == ProfileForm.register) {
				RegisterFrom register =  JsonHelper.FromSingleJson<RegisterFrom> (www.text);
			} else {
				
				LoginForm login = JsonHelper.FromSingleJson<LoginForm> (www.text);
				Profile.GetInstance.user = new User (tempEmail,login);
				Profile.GetInstance.isLogin = true;
				MallEvent.instance.UserLoginComplete ();
				MallEvent.instance.SelectPage ((int)Page.EditProfilePage);
				MallEvent.instance.CallEditorProfilePage ((int)ProfilePageSelect.PersonalInformationPage);
				ServiceRequest.instance.GetMyCart ();

				SaveManager.instance.SetEmail (tempEmail);
				SaveManager.instance.SetRawDataUser (www.text);
				SaveManager.instance.SetPassword (password_input.text);

			}

		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    

	}


}
[System.Serializable]
public class RegisterFrom{
	public bool success;
	public string description;

}

[System.Serializable]
public class LoginForm{
	public bool success;
	public int member_id;
	public string fname,lname,avatar,description;

}
public enum ProfileForm{
	login,register
}
