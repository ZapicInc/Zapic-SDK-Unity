using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleStartup : MonoBehaviour
{
    void Start()
    {
        Zapic.Start();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var p = new Dictionary<string, object>()
            {
                {"SCORE",22},
                {"PARAM2","abc"},
                {"PARAM3",true},
                {"PARAM4","\"blab"},
                 {"PAR\"AM5","\"blab"}
            };

            Zapic.SubmitEvent(p);
        }
    }
}
