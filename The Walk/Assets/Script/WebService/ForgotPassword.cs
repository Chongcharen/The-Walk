using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ForgotPassword : MonoBehaviour {
	public InputField password_txt;
	public Button b_submit;

	void Start(){
		b_submit.onClick.AddListener (OnRequestForgetPassword);
	}
	void OnRequestForgetPassword(){
		ServiceRequest.instance.GetNewPassword (password_txt.text);
	}
}
