using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ZapicSDK
{
    internal sealed class ZapiciOSInterface : IZapicInterface
    {
        #region DLLImports

        [DllImport("__Internal")]
        private static extern void z_start();

        [DllImport("__Internal")]
        private static extern void z_show(string viewName);

        [DllImport("__Internal")]
        private static extern void z_submitEventWithParams(string eventJson);

        [DllImport("__Internal")]
        private static extern string z_playerId();

        #endregion

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
            z_show(view.ToString().ToLower());
        }

        /// <summary>
        /// Gets the current players unique id.
        /// </summary>
        /// <returns>The unique id.</returns>
        public string PlayerId()
        {
            return z_playerId();
        }

        /// <summary>
        /// Submit a new in-game event to zapic.
        /// </summary>
        /// <param name="param">Collection of parameter names and associate values (numeric, string, bool)</param>
        public void SubmitEvent(Dictionary<string, object> param)
        {
            var json = MiniJSON.Json.Serialize(param);

            z_submitEventWithParams(json);
        }
    }
}