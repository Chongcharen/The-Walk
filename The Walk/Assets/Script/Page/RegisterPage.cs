using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.RegularExpressions;
public class RegisterPage : MonoBehaviour {
	public InputField username_input,password_input;
	public Button b_register,b_login;
	string urlService;
	string tempEmail;
	ProfileForm form;


	void Start(){
		b_register.onClick.AddListener (OnRegister);
		b_login.onClick.AddListener (OnLogin);

	}
	void OnLogin(){
		form = ProfileForm.login;
		urlService = "http://thewalklifestylemall.com/service/login";
		RequestToServer (urlService);
	}
	void OnRegister(){
		if (!Utils.IsValidEmailAddress(username_input.text)) {
			PopupManager.instance.ShowAlertPopup ("กรุณากรอกอีเมล์ให้ถูกต้อง");
			return;
		}
		form = ProfileForm.register;
		urlService = "http://thewalklifestylemall.com/service/register";
		RequestToServer (urlService);
	}
	void RequestToServer(string urlPath){
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
			if (form == ProfileForm.register) {
				RegisterFrom register = JsonHelper.FromSingleJson<RegisterFrom> (www.text);
				if (register.success) {
					PopupManager.instance.ShowAlertPopup (register.description);
					OnLogin ();
				} else {
					PopupManager.instance.ShowAlertPopup (register.description);
				}
			} else {
				ServiceResponse response = JsonHelper.FromSingleJson<ServiceResponse> (www.text);
				if (!response.success) {
					PopupManager.instance.ShowAlertPopup (response.description);
				} else {
					/*LoginForm login = JsonConvert.DeserializeObject<LoginForm> (www.text);
					Profile.GetInstance.user = new User (tempEmail, login);
					Profile.GetInstance.isLogin = true;
					MallEvent.instance.UserLoginComplete ();
					MallEvent.instance.SelectPage ((int)Page.EditProfilePage);*/


					LoginForm login = JsonHelper.FromSingleJson<LoginForm>(www.text);
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
			}

		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    

	}
		

}

