using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class EditorHeaderSelection : MonoBehaviour {

	public Button[] buttons;
	// Use this for initialization
	public Color colorSelect,colorDeselect;
	void Start () {
		
		foreach (Button b in buttons) {

			EventTrigger trigger = b.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;

			entry.callback.AddListener( (eventData) => { OnPointerDown(eventData); } );
			trigger.triggers.Add (entry);	
		}
	}

	public void OnPointerDown(BaseEventData b){
		foreach(Button button in buttons){
			if (b.selectedObject.name == button.name) {
				OnSelectButton (button);
			} else {
				OnDeselectButton  (button);
			}
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
