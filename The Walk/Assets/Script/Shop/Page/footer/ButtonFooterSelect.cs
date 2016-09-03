using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonFooterSelect : MonoBehaviour {

	// Use this for initialization
	public Sprite selectSprite , deselectSprite;
	public Image image;
	public int id;

	void OnEnable(){
		MallEvent.OnSelectFooterPageEvent += MallEvent_OnSelectFooterPageEvent;
	}


	void OnDisable(){
		MallEvent.OnSelectFooterPageEvent -= MallEvent_OnSelectFooterPageEvent;
	}

	void MallEvent_OnSelectFooterPageEvent (int select_id)
	{

		if (select_id == id) {
			OnSelectButton ();
		} else {
			OnDeselectButton ();
		}
	}


	void OnSelectButton(){
		image.sprite = selectSprite;
	}

	void OnDeselectButton(){
		image.sprite = deselectSprite;
	}
}
