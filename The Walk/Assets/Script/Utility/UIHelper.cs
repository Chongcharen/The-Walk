using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public delegate void DelegateFunction();
public delegate void DelegateFunctionParam(object param);
public class UIHelper : MonoBehaviour {

	public static void AddTriggerButton(Button btn,DelegateFunction f)
	{
		EventTrigger trigger = btn.gameObject.AddComponent<EventTrigger> ();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener ((BaseEventData) => {
			f ();
		});
		trigger.triggers.Add (entry);
	} 
	public static void AddTriggerButton(Button btn,DelegateFunctionParam f,object param = null)
	{
		EventTrigger trigger = btn.gameObject.AddComponent<EventTrigger> ();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener ((BaseEventData) => {
			f (param);
		});
		trigger.triggers.Add (entry);
	} 
	public static void AddTriggerGameObject(GameObject go,DelegateFunction f)
	{
		EventTrigger trigger = go.AddComponent<EventTrigger> ();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener ((BaseEventData) => {
			f ();
		});
		trigger.triggers.Add (entry);
	} 

	public static string SetCurrencyToK(float amount){
		if (amount > 9999) {
			amount = amount / 1000;
			return amount.ToString ("#,###") + "K";
		} else {
			return amount.ToString ();
		}
		return ((int)amount).ToString ();
	}
	public static string SetCurrencyWithoutK(float amount){
		return amount.ToString ("#,###");
	}
}
