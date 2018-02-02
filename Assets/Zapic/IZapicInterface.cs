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
        string PlayerId();

        /// <summary>
        /// Submit a new in-game event to zapic.
        /// </summary>
        /// <param name="param">Collection of parameter names and associate values (numeric, string, bool)</param>
        void SubmitEvent(Dictionary<string, object> param);
    }
}