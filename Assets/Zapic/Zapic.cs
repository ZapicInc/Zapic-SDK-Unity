using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZapicSDK;

public static class Zapic {

	static ZapicBehavior zapicBehavior;

	static Zapic(){

		// Set up a MonoBehaviour to run Zapi, and hide it
        GameObject zapicGo = new GameObject();
        zapicBehavior = (ZapicBehavior)zapicGo.AddComponent<ZapicBehavior>();
        zapicGo.name = "Zapic";
        // zapicGo.hideFlags = HideFlags.HideInHierarchy;
	}

	public static void Init(){
		//Check network connectivity
		//Start login with gamecenter/play services
	}

	public static void ShowMenu(){
		zapicBehavior.ShowMenu();
		// Debug.Log("Show Zapic Menu");
	}

	public static void ShowMenu(Views view){
		zapicBehavior.ShowMenu(view);
		// Debug.Log("Show Menu " + view);
	}
}
