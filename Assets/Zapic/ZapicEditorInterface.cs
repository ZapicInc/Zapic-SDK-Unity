using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;
using ZapicSDK.MiniJSON;

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

        private readonly ZapicPlayer _player = new ZapicPlayer("0000000-0000-0000-0000-000000000000", "Test User", new Uri("https://randomuser.me/api/portraits/men/3.jpg"), "AAAAAAAAABBBBBBBBBCCCCCCCCC");

        private readonly ZapicStatistic[] _stats;

        private readonly ZapicChallenge[] _challenges;

        private readonly ZapicCompetition[] _competitions;

        public Action<ZapicPlayer> OnLogin
        {
            get
            {
                return _loginHandler;
            }

            set
            {
                Debug.LogFormat("Zapic:OnLogin was set successfully");
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
                Debug.LogFormat("Zapic:OnLogout was set successfully");
                _logoutHandler = value;
            }
        }

        public ZapicEditorInterface()
        {
            _stats = new []
            {
                new ZapicStatistic(Guid.NewGuid().ToString(), "Stat1", "1,234", 1234, .90, 8),
                new ZapicStatistic(Guid.NewGuid().ToString(), "Stat2", "567.8", 567.8, .1, null),
            };

            _challenges = new []
            {
                new ZapicChallenge(Guid.NewGuid().ToString(), "Challenge 1", true, "Win!", DateTime.UtcNow.AddHours(-10), DateTime.UtcNow.AddHours(5), "1,234", 1234, "level1", ZapicChallengeStatus.Accepted, 4, 15),
                new ZapicChallenge(Guid.NewGuid().ToString(), "Challenge 2", true, "Win!", DateTime.UtcNow.AddHours(-15), DateTime.UtcNow.AddHours(15), null, null, "level1", ZapicChallengeStatus.Invited, null, null)

            };

            _competitions = new []
            {
                new ZapicCompetition(Guid.NewGuid().ToString(), "Competition 1", "Beat everyone else", "Zone2", true, DateTime.UtcNow.AddHours(-10), DateTime.UtcNow.AddHours(5), 10234, ZapicCompetitionStatus.Accepted, "467", 467, null, 3),
            };
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
                ((System.Timers.Timer) s).Stop(); //s is the Timer

                if (_loginHandler != null)
                    _loginHandler(_player);
            });
            timer.Enabled = true;

        }

        public void ShowDefaultPage()
        {
            CheckStarted();
            Debug.LogFormat("Zapic:Show Default page");
        }

        public void ShowPage(ZapicPages page)
        {
            CheckStarted();
            Debug.LogFormat("Zapic:Show {0}", page);
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

        public void HandleInteraction(Dictionary<string, object> data)
        {
            Debug.LogFormat("Zapic:HandleInteraction");
        }

        public void GetCompetitions(Action<ZapicCompetition[], ZapicError> callback)
        {
            callback(_competitions, null);
        }

        public void GetStatistics(Action<ZapicStatistic[], ZapicError> callback)
        {
            callback(_stats, null);
        }

        public void GetChallenges(Action<ZapicChallenge[], ZapicError> callback)
        {
            callback(_challenges, null);
        }

        public void GetPlayer(Action<ZapicPlayer, ZapicError> callback)
        {
            callback(_player, null);
        }
    }
}