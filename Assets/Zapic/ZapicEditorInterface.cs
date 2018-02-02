using System.Collections.Generic;
using UnityEngine;

namespace ZapicSDK
{
    internal sealed class ZapicEditorInterface : IZapicInterface
    {
        public void Start()
        {
            Debug.LogFormat("Zapic:Start");
        }

        public void Show(Views view)
        {
            Debug.LogFormat("Zapic:Show {0}", view);
        }

        public void SubmitEvent(Dictionary<string, object> param)
        {
            var json = MiniJSON.Json.Serialize(param);
            Debug.LogFormat("Zapic: SubmitEvent: {0}", json);
        }

        public string PlayerId()
        {
            Debug.LogFormat("Zapic:GetPlayerId");
            return "0000000-0000-0000-0000-000000000000";
        }
    }
}