using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using MiniJSON;

namespace ZapicSDK
{
    internal sealed class ZapiciOSInterface : IZapicInterface
    {
        private delegate void internal_PlayerLogin(string playerJson);

        private delegate void internal_PlayerLogout(string playerJson);

        #region DLLImports

        [DllImport("__Internal")]
        private static extern void z_start();

        [DllImport("__Internal")]
        private static extern void z_showDefaultPage();

        [DllImport("__Internal")]
        private static extern void z_showPage(string pageName);

        [DllImport("__Internal")]
        private static extern void z_submitEventWithParams(string eventJson);

        [DllImport("__Internal")]
        private static extern void z_handleInteraction(string dataJson);

        [DllImport("__Internal")]
        private static extern void z_setLoginHandler(internal_PlayerLogin loginCallback);

        [DllImport("__Internal")]
        private static extern void z_setLogoutHandler(internal_PlayerLogin logoutCallback);

        [DllImport("__Internal")]
        private static extern string z_player();

        [MonoPInvokeCallback(typeof(internal_PlayerLogin))]
        private static void _player_login(string playerJson)
        {
            if (_loginHandler == null)
                return;

            var player = DeserializePlayer(playerJson);
            _loginHandler(player);
        }

        [MonoPInvokeCallback(typeof(internal_PlayerLogout))]
        private static void _player_logout(string playerJson)
        {
            if (_logoutHandler == null)
                return;

            var player = DeserializePlayer(playerJson);
            _logoutHandler(player);
        }

        #endregion DLLImports

        private static Action<ZapicPlayer> _loginHandler;

        private static Action<ZapicPlayer> _logoutHandler;

        public ZapiciOSInterface()
        {
            z_setLoginHandler(_player_login);
            z_setLogoutHandler(_player_logout);
        }

        public Action<ZapicPlayer> OnLogin
        {
            get
            {
                return _loginHandler;
            }

            set
            {
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
                _logoutHandler = value;
            }
        }

        /// <summary>
        /// Starts zapic. This should be called
        /// as soon as possible during app startup.
        /// </summary>
        /// <param name="version">App version id.</param>
        public void Start()
        {
            z_start();
        }

        /// <summary>
        /// Shows the default Zapic page
        /// </summary>
        public void ShowDefaultPage()
        {
            z_showDefaultPage();
        }

        /// <summary>
        /// Shows the given Zapic page
        /// </summary>
        /// <param name="page">Page to show.</param>
        public void ShowPage(ZapicPages page)
        {
            z_showPage(page.ToString().ToLower());
        }


        /// <summary>
        /// Gets the current player
        /// </summary>
        /// <returns>The player.</returns>
        public ZapicPlayer Player()
        {
            var playerJson = z_player();
            var player = DeserializePlayer(playerJson);
            return player;
        }

        public void HandleInteraction(Dictionary<string, object> data)
        {
            var json = Json.Serialize(data);

            z_handleInteraction(json);
        }

        /// <summary>
        /// Submit a new in-game event to zapic.
        /// </summary>
        /// <param name="param">Collection of parameter names and associate values (numeric, string, bool)</param>
        public void SubmitEvent(Dictionary<string, object> param)
        {
            var json = Json.Serialize(param);

            z_submitEventWithParams(json);
        }

        private static ZapicPlayer DeserializePlayer(string playerJson)
        {
            UnityEngine.Debug.Log("Deserializing " + playerJson);
            if (playerJson == null)
            {
                UnityEngine.Debug.Log("Null player deserialized");
                return null;
            }

            var dict = Json.Deserialize(playerJson) as Dictionary<string, object>;

            var player = new ZapicPlayer
            {
                PlayerId = (string)dict["playerId"],
                NotificationToken = (string)dict["notificationToken"],
            };

            return player;
        }
    }
}
