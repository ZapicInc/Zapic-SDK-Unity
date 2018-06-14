using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A MonoBehavior that can be attached to any GameObject to initialize Zapic.
/// This provides a template of how to call Zapic from within your game.
/// </summary>
public class ZapicBehavior : MonoBehaviour {
	void Start () {

        //TODO: Uncomment these lines if you want to be notified
        //when the player logs in/out

		// Zapic.OnLogin = ((player) =>
        // {
		// 	//Do stuff here
        // });

        // Zapic.OnLogout = ((player) =>
        // {
        //     //Do stuff here
        // });

        Zapic.Start();
	}

	/// <summary>
	/// Shows the default Zapic page
	/// </summary>
	public void ShowZapicDefaultPage()
    {
        Zapic.ShowDefaultPage();
    }

    ///TODO: Replace XXX with the specific page you want to show
    // public void ShowZapicXXXPage()
    // {
    //     Zapic.ShowPage(ZapicPages.XXX);
    // }

    //Here is an example of how to send an event. This code should be placed in your game
    //and the parameter names should be replaced with the ones you defined in the portal
    //EXAMPLE - Sending an Event:
    // // Submitting an event
    // var eventParams = new Dictionary<string, object>();
    // eventParams.Add("Distance",147);
    // eventParams.Add("Score",22);
    // Zapic.SubmitEvent(eventParams);

}
