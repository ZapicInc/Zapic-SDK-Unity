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
        /// Shows the default Zapic page
        /// </summary>
        void ShowDefaultPage();

        /// <summary>
        /// Shows the given Zapic page
        /// </summary>
        /// <param name="page">Page to show.</param>
        void ShowPage(ZapicPages page);

        /// <summary>
        /// Handle Zapic data. Usually from an integration like push notifications.
        /// </summary>
        /// <param name="data">The data.</param>
        void HandleInteraction(Dictionary<string, object> data);

        /// <summary>
        /// Submit a new in-game event to zapic.
        /// </summary>
        /// <param name="param">Collection of parameter names and associate values (numeric, string, bool)</param>
        void SubmitEvent(Dictionary<string, object> param);

        /// <summary>
        /// Gets the current competitions
        /// </summary>
        /// <param name="callback">Callback with either the competitions or an error</param>
        void GetCompetitions(Action<ZapicCompetition[], ZapicError> callback);

        /// <summary>
        /// Gets the current statistics
        /// </summary>
        /// <param name="callback">Callback with either the statistics or an error</param>
        void GetStatistics(Action<ZapicStatistic[], ZapicError> callback);

        /// <summary>
        /// Gets the current challenges
        /// </summary>
        /// <param name="callback">Callback with either the challenges or an error</param>
        void GetChallenges(Action<ZapicChallenge[], ZapicError> callback);

        /// <summary>
        /// Gets the current player
        /// </summary>
        /// <param name="callback">Callback with either the challenges or an error</param>
        void GetPlayer(Action<ZapicPlayer, ZapicError> callback);
    }
}