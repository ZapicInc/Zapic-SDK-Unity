using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using MiniJSON;

namespace ZapicSDK
{
    internal struct ZPCUPlayer
    {
        public string id;

        public string notificationToken;
    }

    internal sealed class ZapiciOSInterface : IZapicInterface
    {
        private delegate void internal_PlayerLogin(ZPCUPlayer player);

        private delegate void internal_PlayerLogout(ZPCUPlayer player);

        #region DLLImports

        [DllImport("__Internal")]
        private static extern void zpc_start();

        [DllImport("__Internal")]
        private static extern void zpc_showDefault();

        [DllImport("__Internal")]
        private static extern void zpc_show(string pageName);

        [DllImport("__Internal")]
        private static extern void zpc_submitEventWithParams(string eventJson);

        [DllImport("__Internal")]
        private static extern void zpc_handleInteraction(string dataJson);

        [DllImport("__Internal")]
        private static extern void zpc_setLoginHandler(internal_PlayerLogin loginCallback);

        [DllImport("__Internal")]
        private static extern void zpc_setLogoutHandler(internal_PlayerLogin logoutCallback);

        [DllImport("__Internal")]
        private static extern ZPCUPlayer zpc_player();

        [MonoPInvokeCallback(typeof(internal_PlayerLogin))]
        private static void _player_login(ZPCUPlayer p)
        {
            if (_loginHandler == null)
                return;

            _loginHandler(p.ToPlayer());
        }

        [MonoPInvokeCallback(typeof(internal_PlayerLogout))]
        private static void _player_logout(ZPCUPlayer p)
        {
            if (_logoutHandler == null)
                return;
            _logoutHandler(p.ToPlayer());
        }

        #endregion DLLImports

        private static Action<ZapicPlayer> _loginHandler;

        private static Action<ZapicPlayer> _logoutHandler;

        public ZapiciOSInterface()
        {
            zpc_setLoginHandler(_player_login);
            zpc_setLogoutHandler(_player_logout);
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
            zpc_start();
        }

        /// <summary>
        /// Shows the default Zapic page
        /// </summary>
        public void ShowDefaultPage()
        {
            zpc_showDefault();
        }

        /// <summary>
        /// Shows the given Zapic page
        /// </summary>
        /// <param name="page">Page to show.</param>
        public void ShowPage(string page)
        {
            zpc_show(page.ToLower());
        }

        /// <summary>
        /// Gets the current player
        /// </summary>
        /// <returns>The player.</returns>
        public ZapicPlayer Player()
        {
            var p = zpc_player();
            return p.ToPlayer();
        }

        public void HandleInteraction(Dictionary<string, object> data)
        {
            var json = Json.Serialize(data);

            zpc_handleInteraction(json);
        }

        /// <summary>
        /// Submit a new in-game event to zapic.
        /// </summary>
        /// <param name="param">Collection of parameter names and associate values (numeric, string, bool)</param>
        public void SubmitEvent(Dictionary<string, object> param)
        {
            var json = Json.Serialize(param);

            zpc_submitEventWithParams(json);
        }
    }

    internal static class ZapicExtensions
    {
        internal static ZapicPlayer ToPlayer(this ZPCUPlayer p)
        {
            return new ZapicPlayer
            {
                PlayerId = p.id,
                    NotificationToken = p.notificationToken
            };
        }
    }
}