using System;
using System.Collections.Generic;

namespace ZapicSDK
{
    internal interface IZapicInterface
    {
        /// <summary>
        /// Starts zapic. This should be called
        /// as soon as possible during app startup.
        /// </summary>
        void Start();

        /// <summary>
        /// Shows the given zapic window
        /// </summary>
        /// <param name="view">View to show.</param>
        void Show(Views view);

        /// <summary>
        /// Gets the current players unique id.
        /// </summary>
        /// <returns>The unique id.</returns>
        ZapicPlayer Player();

        /// <summary>
        /// Callback when the player logins in
        /// </summary>
        /// <param name="loginHandler">Callback handler</param>
        void OnLoginHandler(Action<ZapicPlayer> loginHandler);

        /// <summary>
        /// Callback when the player logins out
        /// </summary>
        /// <param name="logoutHandler">Callback handler</param>
        void OnLogoutHandler(Action<ZapicPlayer> logoutHandler);

        /// <summary>
        /// Handle Zapic data. Usually from an integration like push notifications.
        /// </summary>
        /// <param name="data">The data.</param>
        void HandleData(Dictionary<string, object> data);

        /// <summary>
        /// Submit a new in-game event to zapic.
        /// </summary>
        /// <param name="param">Collection of parameter names and associate values (numeric, string, bool)</param>
        void SubmitEvent(Dictionary<string, object> param);
    }
}