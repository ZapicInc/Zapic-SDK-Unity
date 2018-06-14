using System.Collections.Generic;
using UnityEngine;

public class ZapicExample : MonoBehaviour
{
    void Start()
    {
        Zapic.OnLogin = ((player) =>
        {
            Debug.LogFormat("Player logged in. Id:{0}, Notification:{1}", player.PlayerId, player.NotificationToken);
        });

        Zapic.OnLogout = ((player) =>
        {
            Debug.LogFormat("Player logged out. Id:{0}, Notification:{1}", player.PlayerId, player.NotificationToken);
        });

        Zapic.Start();
    }

    private void Update()
    {
        //Simulate a Zapic event on left click
        if (Input.GetMouseButtonDown(0))
        {
            SendEvent();
        }
        //Simulate HandleInteraction on right click
        else if(Input.GetMouseButtonDown(1))
        {
            HandleInteraction();
        }
        //Simulate getting current player on space bar
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            GetPlayer();
        }
    }

    private void SendEvent()
    {
        var eventParams = new Dictionary<string, object>();

        //Add parameter information to the event
        eventParams.Add("Score",22);
        eventParams.Add("PARAM2","abc");
        eventParams.Add("PARAM3",true);
        eventParams.Add("PARAM4","\"blab");
        eventParams.Add("PAR\"AM5", "\"blab" );

        //Sumbit the event to the Zapic server
        Zapic.SubmitEvent(eventParams);
    }

    private void GetPlayer()
    {
        //Gets the current player
        var player = Zapic.Player();

        if (player == null)
        {
            Debug.Log("Player is null");
        }
        else
        {
            Debug.LogFormat("Current Player, Id:{0}, NotificationToken:{1}", player.PlayerId, player.NotificationToken);
        }
    }

    private void HandleInteraction()
    {
        var data = new Dictionary<string, object>()
            { { "zapic", "/challenge/00000000-0000-0000-0000-000000000000" }
            };

        Zapic.HandleInteraction(data);
    }
}
