using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleStartup : MonoBehaviour
{
    void Start()
    {
        Zapic.Start();

        Zapic.OnLoginHandler((player) =>
        {
            Debug.LogFormat("Player logged in. Id:{0}, Notification:{1}", player.PlayerId, player.NotificationToken);
        });

        Zapic.OnLogoutHandler((player) =>
        {
            Debug.LogFormat("Player logged out. Id:{0}, Notification:{1}", player.PlayerId, player.NotificationToken);
        });
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SendEvent();

            GetPlayer();

            HandleData();
        }
    }

    void SendEvent()
    {
        var p = new Dictionary<string, object>()
            { { "SCORE", 22 }, { "PARAM2", "abc" }, { "PARAM3", true }, { "PARAM4", "\"blab" }, { "PAR\"AM5", "\"blab" }
            };

        Zapic.SubmitEvent(p);
    }

    void GetPlayer()
    {
        var player = Zapic.Player();

        if (player == null)
        {
            Debug.Log("Player is null");
        }
        else
        {
            Debug.LogFormat("Player, Id:{0}, NotifToken:{1}", player.PlayerId, player.NotificationToken);
        }
    }

    void HandleData()
    {
        var data = new Dictionary<string, object>()
            { { "zapic", "/challenge/00000000-0000-0000-0000-000000000000" }
            };

        Zapic.HandleData(data);
    }
}