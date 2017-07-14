using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZapicButton : MonoBehaviour {

	void Awake () {

		var button = this.GetComponent<Button>();
		
		if(button == null){
			Debug.LogWarning("ZapicButton must have a Button component");
			return;
		}
		
		button.onClick.AddListener(ShowZapicMenu); 
	}

	private void ShowZapicMenu(){
		// Debug.Log("Show Zapic Menu");
		Zapic.ShowMenu();
	}
}
