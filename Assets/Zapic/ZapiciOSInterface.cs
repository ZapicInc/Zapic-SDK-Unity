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
        private static extern void z_show(string viewName);

        [DllImport("__Internal")]
        private static extern void z_submitEventWithParams(string eventJson);

        [DllImport("__Internal")]
        private static extern void z_handleData(string dataJson);

        [DllImport("__Internal")]
        private static extern void z_setLoginHandler(internal_PlayerLogin loginCallback);

        [DllImport("__Internal")]
        private static extern void z_setLogoutHandler(internal_PlayerLogin logoutCallback);

        [DllImport("__Internal")]
        private static extern string z_player();

        [MonoPInvokeCallback(typeof(internal_PlayerLogin))]
        private static void _player_login(string playerJson)
        {
            _player = Deserialize(playerJson);

            if (_loginHandler == null)
                return;

            _loginHandler(_player);
        }

        [MonoPInvokeCallback(typeof(internal_PlayerLogout))]
        private static void _player_logout(string playerJson)
        {
            _player = Deserialize(playerJson);

            if (_logoutHandler == null)
                return;

            _logoutHandler(_player);
        }

        #endregion DLLImports

        private static Action<ZapicPlayer> _loginHandler;

        private static Action<ZapicPlayer> _logoutHandler;

        private static ZapicPlayer _player;

        public ZapiciOSInterface()
        {
            z_setLoginHandler(_player_login);
            z_setLogoutHandler(_player_logout);
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
        /// Shows the given zapic window
        /// </summary>
        /// <param name="view">View to show.</param>
        public void Show(Views view)
        {
            var viewName = "";

            switch (view)
            {
                case Views.Main:
                    viewName = "default";
                    break;

                default:
                    viewName = view.ToString().ToLower();
                    break;
            }

            z_show(viewName);
        }

        /// <summary>
        /// Gets the current player
        /// </summary>
        /// <returns>The player.</returns>
        public ZapicPlayer Player()
        {
            if (_player == null)
                _player = Deserialize(z_player());

            return _player;
        }

        public void HandleData(Dictionary<string, object> data)
        {
            var json = Json.Serialize(data);

            z_handleData(json);
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

        private static ZapicPlayer Deserialize(string playerJson)
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

        public void OnLoginHandler(Action<ZapicPlayer> loginHandler)
        {
            _loginHandler = loginHandler;

            if (_player != null)
                loginHandler(_player);
        }

        public void OnLogoutHandler(Action<ZapicPlayer> logoutHandler)
        {
            _logoutHandler = logoutHandler;
        }
    }
}
