using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
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

        private Action<ZapicPlayer> _loginHandler;

        private Action<ZapicPlayer> _logoutHandler;

        private readonly ZapicPlayer _player = new ZapicPlayer
        {
            PlayerId = "0000000-0000-0000-0000-000000000000",
            NotificationToken = "AAAAAAAAABBBBBBBBBCCCCCCCCC",
        };

        public Action<ZapicPlayer> OnLogin
        {
            get
            {
                return _loginHandler;
            }

            set
            {
                Debug.LogFormat("Zapic:OnLogin set");
                _loginHandler = value;
            }
        }

        public Action<ZapicPlayer> OnLogout
        {
            get
            {
                return _logoutHandler;
            }

            set
            {
                Debug.LogFormat("Zapic:OnLogout set");
                _logoutHandler = value;
            }
        }

        public void Start()
        {
            DisplayEditorWarning();

            Debug.LogFormat("Zapic:Start");

            if (_started)
                Debug.LogError("Zapic: Please only call Zapic.Start() once.");

            _started = true;

            var timer = new System.Timers.Timer(1000);

            timer.Elapsed += new ElapsedEventHandler((s, e) =>
            {
                if (_loginHandler != null)
                    _loginHandler(_player);

                timer.Enabled = false;
            });
            timer.Enabled = true;
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

        public ZapicPlayer Player()
        {
            CheckStarted();
            Debug.LogFormat("Zapic:GetPlayer");
            return _player;
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

        public void HandleData(Dictionary<string, object> data)
        {
            Debug.LogFormat("Zapic:HandleData");
        }
    }
}
