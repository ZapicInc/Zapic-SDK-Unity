using System;
using System.Collections.Generic;

namespace ZapicSDK
{
    internal interface IZapicInterface
    {
        /// <summary>
        /// Gets or sets the callback invoked after the player has been logged in
        /// </summary>
        /// <remarks>
        /// The player that has been logged in is passed to the callback.
        /// </remarks>
        Action<ZapicPlayer> OnLogin { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked after the player has been logged out
        /// </summary>
        /// <remarks>
        /// The player that has been logged out is passed to the callback.
        /// </remarks>
        Action<ZapicPlayer> OnLogout { get; set; }

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
