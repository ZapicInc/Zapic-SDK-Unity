using System.Collections.Generic;
using UnityEngine;

namespace ZapicSDK
{
    internal sealed class ZapicEditorInterface : IZapicInterface
    {
        /// <summary>
        /// Flag indicating if the zapic editor warning was displayed already
        /// </summary>
        private bool _showedWarning = false;

        /// <summary>
        /// Has Zapic.Start() been called yet?
        /// </summary>
        private bool _started = false;

        public void Start()
        {
            DisplayEditorWarning();

            Debug.LogFormat("Zapic:Start");

            if (_started)
                Debug.LogError("Zapic: Please only call Zapic.Start() once.");

            _started = true;
        }

        public void Show(Views view)
        {
            CheckStarted();
            Debug.LogFormat("Zapic:Show {0}", view);
        }

        public void SubmitEvent(Dictionary<string, object> param)
        {
            CheckStarted();
            var json = MiniJSON.Json.Serialize(param);
            Debug.LogFormat("Zapic: SubmitEvent: {0}", json);
        }

        public string PlayerId()
        {
            CheckStarted();
            Debug.LogFormat("Zapic:GetPlayerId");
            return "0000000-0000-0000-0000-000000000000";
        }

        private void DisplayEditorWarning()
        {
            //If the warning has already been shown, show the error.
            if (_showedWarning)
                return;

            _showedWarning = true;

            Debug.Log("Zapic only works on mobile devices. You will only see log messages when running in the editor.");
        }

        private void CheckStarted()
        {
            //If it was started, don't do anything else
            if (_started)
                return;

            Debug.LogError("Zapic: Please ensure that Zapic.Start() is called before any other Zapic methods.");
        }
    }
}