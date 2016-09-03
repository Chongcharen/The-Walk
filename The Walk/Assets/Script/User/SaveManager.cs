using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
public class SaveManager : MonoBehaviour {
	public static SaveManager instance;
	void Awake(){
		instance = this;
	}



	string GetEmail(){
		return PlayerPrefs.GetString("email");
	}
	public string GetPassword(){
		return PlayerPrefs.GetString("password");
	}
	string GetRawDataUser (){
		return PlayerPrefs.GetString ("rawData");
	}

	public void SetEmail(string email){
		PlayerPrefs.SetString("email",email);
	}
	public void SetPassword(string password){
		PlayerPrefs.SetString("password",password);
	}

	public void SetRawDataUser(string rawData){
		PlayerPrefs.SetString("rawData",rawData);
	}
	public void GetUserFromData(){
		GetUser ();
	}
	public void ClearData(){
		PlayerPrefs.DeleteKey ("rawData");
		PlayerPrefs.DeleteKey ("email");
		PlayerPrefs.DeleteAll ();

	}




	void GetUser(){
		if (string.IsNullOrEmpty (GetEmail()) || string.IsNullOrEmpty (GetRawDataUser()))
			return;
		
		LoginForm login = JsonConvert.DeserializeObject<LoginForm> (GetRawDataUser());
		Profile.GetInstance.user = new User (GetEmail(), login);
		Profile.GetInstance.isLogin = true;
		MallEvent.instance.UserLoginComplete ();
		ServiceRequest.instance.GetMyCart ();

	}
}
