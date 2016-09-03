using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Header : MonoBehaviour {
	public Button b_profile;
	// Use this for initialization
	void Start () {
		//b_profile.onClick.AddListener (OpenProfile);
	}

	void OpenProfile(){
		if (!Profile.GetInstance.isLogin) {
			MallEvent.instance.SelectPage ((int)Page.RegisterPage);
		}
	}
}
