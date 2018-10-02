using System.Collections.Generic;
using UnityEngine;

public class ZapicExample : MonoBehaviour
{
    private void Start()
    {
        // Register to receive notifications when the current player has logged in. Use this notification to load any
        // state or data to be shown to the player.
        Zapic.OnLogin = (player) =>
        {
            Debug.LogFormat(
                "Received player logged in event! Player ID: {0}, Player Name: {1}, Player Icon URL: {2}",
                player.Id,
                player.Name,
                player.IconUrl);

            Zapic.GetChallenges((challenges, error) =>
            {
                if (error != null)
                {
                    Debug.LogErrorFormat(
                        "An error occurred getting the list of challenges for the current player: {0}, {1}",
                        error.Code,
                        error.Message);
                }
                else
                {
                    Debug.LogFormat("Received {0} challenges!", challenges.Length);
                }
            });

            Zapic.GetCompetitions((competitions, error) =>
            {
                if (error != null)
                {
                    Debug.LogErrorFormat(
                        "An error occurred getting the list of competitions for the current player: {0}, {1}",
                        error.Code,
                        error.Message);
                }
                else
                {
                    Debug.LogFormat("Received {0} competitions!", competitions.Length);
                }
            });

            Zapic.GetStatistics((stats, error) =>
            {
                if (error != null)
                {
                    Debug.LogErrorFormat(
                        "An error occurred getting the list of statistics for the current player: {0}, {1}",
                        error.Code,
                        error.Message);
                }
                else
                {
                    Debug.LogFormat("Received {0} statistics!", stats.Length);
                }
            });
        };

        // Register to receive notifications when the current player has logged out. Use this notification to reset any
        // state or data that shouldn't be shown to a new player.
        Zapic.OnLogout = (player) =>
        {
            Debug.LogFormat("Received player logged out event! Player ID: {0}", player.Id);
        };

        // Start Zapic.
        Zapic.Start();
    }

    private void Update()
    {
        // Simulate a Zapic gameplay event on left click.
        if (Input.GetMouseButtonDown(0))
        {
            SendEvent();
        }

        // Simulate a Zapic interaction event on right click.
        else if (Input.GetMouseButtonDown(1))
        {
            HandleInteraction();
        }

        // Simulate getting the current Zapic player on space bar.
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            GetPlayer();
        }
    }

    private void GetPlayer()
    {
        // Gets the current player. The result may be null if the current player has not logged in!
        Zapic.GetPlayer((result, error) =>
        {
            if (error != null)
            {
                Debug.LogErrorFormat(
                    "An error occurred getting the current player: {0}, {1}",
                    error.Code,
                    error.Message);
            }
            else
            {
                Debug.LogFormat(
                    "Received player! Player ID: {0}, Player Name: {1}, Player Icon URL: {2}",
                    result.Id,
                    result.Name,
                    result.IconUrl);
            }
        });
    }

    private void HandleInteraction()
    {
        // Interaction events are triggered by additional integrations (app links, push notifications, banner
        // notifications, etc.). Interaction events usually display Zapic in response to a player action (opening an
        // invitation, acknowledging a notification, etc.).

        // The following code simulates an interaction event and exists for demonstration purposes only. Interaction
        // events should never be created manually! (The semantics of the following code can change without notice!)
        Zapic.HandleInteraction(new Dictionary<string, object>()
        {
            { "zapic", "/challenge/00000000-0000-0000-0000-000000000000" }
        });
    }

    private void SendEvent()
    {
        // Parameters have string keys and boolean, number, or string values.
        var parameters = new Dictionary<string, object>();
        parameters.Add("SCORE", 22);
        parameters.Add("LEVEL", 2);
        parameters.Add("CHARACTER", "Santa");
        parameters.Add("CRASHED", false);
        Zapic.SubmitEvent(parameters);
    }
}
