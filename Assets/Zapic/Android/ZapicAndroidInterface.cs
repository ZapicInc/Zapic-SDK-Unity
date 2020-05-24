#if !UNITY_EDITOR && UNITY_ANDROID

using System;
using System.Collections.Generic;
using UnityEngine;

#if NET_4_6 || NET_STANDARD_2_0
using System.Threading;
using System.Threading.Tasks;
#endif

namespace ZapicSDK
{
    /// <summary>An <see cref="IZapicInterface"/> that communicates with the Android SDK.</summary>
    internal sealed class ZapicAndroidInterface : IZapicInterface
    {
        /// <summary>The authentication handler registered with the Android SDK.</summary>
        private readonly ZapicAndroidAuthenticationHandler authenticationHandler;

        /// <summary>A value indicating whether the Android SDK has been initialized.</summary>
        private bool initialized;

        /// <summary>Initializes a new instance of the <see cref="ZapicAndroidInterface"/> class.</summary>
        internal ZapicAndroidInterface()
        {
            authenticationHandler = new ZapicAndroidAuthenticationHandler();
            initialized = false;
        }

        public Action<ZapicPlayer> OnLogin { get; set; }

        public Action<ZapicPlayer> OnLogout { get; set; }

        public void GetChallenges(Action<ZapicChallenge[], ZapicException> callback)
        {
            Get("getChallenges", callback, ZapicAndroidUtilities.ConvertChallenges);
        }

#if NET_4_6 || NET_STANDARD_2_0
        public Task<ZapicChallenge[]> GetChallengesAsync(CancellationToken cancellationToken)
        {
            return GetAsync("getChallenges", ZapicAndroidUtilities.ConvertChallenges, cancellationToken);
        }
#endif

        public void GetCompetitions(Action<ZapicCompetition[], ZapicException> callback)
        {
            Get("getCompetitions", callback, ZapicAndroidUtilities.ConvertCompetitions);
        }

#if NET_4_6 || NET_STANDARD_2_0
        public Task<ZapicCompetition[]> GetCompetitionsAsync(CancellationToken cancellationToken)
        {
            return GetAsync("getCompetitions", ZapicAndroidUtilities.ConvertCompetitions, cancellationToken);
        }
#endif

        public void GetPlayer(Action<ZapicPlayer, ZapicException> callback)
        {
            Get("getPlayer", callback, ZapicAndroidUtilities.ConvertPlayer);
        }

#if NET_4_6 || NET_STANDARD_2_0
        public Task<ZapicPlayer> GetPlayerAsync(CancellationToken cancellationToken)
        {
            return GetAsync("getPlayer", ZapicAndroidUtilities.ConvertPlayer, cancellationToken);
        }
#endif

        public void GetStatistics(Action<ZapicStatistic[], ZapicException> callback)
        {
            Get("getStatistics", callback, ZapicAndroidUtilities.ConvertStatistics);
        }

#if NET_4_6 || NET_STANDARD_2_0
        public Task<ZapicStatistic[]> GetStatisticsAsync(CancellationToken cancellationToken)
        {
            return GetAsync("getStatistics", ZapicAndroidUtilities.ConvertStatistics, cancellationToken);
        }
#endif

        public void HandleInteraction(Dictionary<string, object> parameters)
        {
            var json = MiniJSON.Json.Serialize(parameters);

            using (var parametersObject = new AndroidJavaObject("java/lang/String", json))
            using (var zapicClass = new AndroidJavaClass("com/zapic/sdk/android/Zapic"))
            {
                zapicClass.CallStatic("handleInteraction", parametersObject);
            }
        }

        [Obsolete]
        public ZapicPlayer Player()
        {
            using (var zapicClass = new AndroidJavaClass("com/zapic/sdk/android/Zapic"))
            using (var playerObject = zapicClass.CallStatic<AndroidJavaObject>("getCurrentPlayer"))
            {
                try
                {
                    var player = ZapicAndroidUtilities.ConvertPlayer(playerObject);
                    return player;
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred converting the Java object to ZapicPlayer");
                    Debug.LogException(e);

                    if (e is ZapicException)
                    {
                        throw;
                    }

                    throw new ZapicException(
                        ZapicErrorCode.INVALID_RESPONSE,
                        "An error occurred converting the Java object to ZapicPlayer",
                        e);
                }
            }
        }

        public void ShowDefaultPage()
        {
            using (var unityPlayerClass = new AndroidJavaClass("com/unity3d/player/UnityPlayer"))
            using (var activityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var zapicClass = new AndroidJavaClass("com/zapic/sdk/android/Zapic"))
            {
                zapicClass.CallStatic("showDefaultPage", activityObject);
            }
        }

        public void ShowPage(ZapicPage page)
        {
            using (var unityPlayerClass = new AndroidJavaClass("com/unity3d/player/UnityPlayer"))
            using (var activityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var zapicClass = new AndroidJavaClass("com/zapic/sdk/android/Zapic"))
            using (var pageObject = new AndroidJavaObject("java/lang/String", page.ToString().ToLowerInvariant()))
            {
                zapicClass.CallStatic("showPage", activityObject, pageObject);
            }
        }

        public void Start()
        {
            if (initialized)
            {
                return;
            }

            using (var unityPlayerClass = new AndroidJavaClass("com/unity3d/player/UnityPlayer"))
            using (var activityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                activityObject.Call("runOnUiThread", new AndroidJavaRunnable(StartOnUI));
            }
        }

        public void SubmitEvent(Dictionary<string, object> parameters)
        {
            var json = MiniJSON.Json.Serialize(parameters);

            using (var parametersObject = new AndroidJavaObject("java/lang/String", json))
            using (var zapicClass = new AndroidJavaClass("com/zapic/sdk/android/Zapic"))
            {
                zapicClass.CallStatic("submitEvent", parametersObject);
            }
        }

        private void Get<T>(
            string methodName,
            Action<T, ZapicException> callback,
            Func<AndroidJavaObject, T> convertResult)
            where T : class
        {
            var callbackWrapper = new ZapicAndroidFunctionCallback<T>(callback, convertResult);

            using (var zapicClass = new AndroidJavaClass("com/zapic/sdk/android/Zapic"))
            {
                var future = zapicClass.CallStatic<AndroidJavaObject>(methodName, callbackWrapper);
                if (future != null)
                {
                    future.Dispose();
                }
            }
        }

#if NET_4_6 || NET_STANDARD_2_0
        private Task<T> GetAsync<T>(
            string methodName,
            Func<AndroidJavaObject, T> convertResult,
            CancellationToken cancellationToken)
            where T : class
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            var callbackWrapper = new ZapicAndroidTaskCallback<T>(taskCompletionSource, convertResult);

            using (var zapicClass = new AndroidJavaClass("com/zapic/sdk/android/Zapic"))
            {
                var future = zapicClass.CallStatic<AndroidJavaObject>(methodName, callbackWrapper);
                if (future != null)
                {
                    if (cancellationToken.CanBeCanceled)
                    {
                        var registration = cancellationToken.Register(() =>
                        {
                            if (taskCompletionSource.TrySetCanceled(cancellationToken))
                            {
                                future.Call<bool>("cancel", true);
                            }
                        });

                        return taskCompletionSource.Task.ContinueWith(
                            parentTask =>
                            {
                                registration.Dispose();
                                future.Dispose();
                                return parentTask;
                            },
                            TaskContinuationOptions.ExecuteSynchronously).Unwrap();
                    }
                    else
                    {
                        future.Dispose();
                    }
                }

                return taskCompletionSource.Task;
            }
        }
#endif

        private void StartOnUI()
        {
            if (initialized)
            {
                return;
            }

            // Ensure initialization only runs once.
            initialized = true;

            using (var zapicClass = new AndroidJavaClass("com/zapic/sdk/android/Zapic"))
            {
                zapicClass.CallStatic("setPlayerAuthenticationHandler", authenticationHandler);

                using (var unityPlayerClass = new AndroidJavaClass("com/unity3d/player/UnityPlayer"))
                using (var activityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    zapicClass.CallStatic("attachFragment", activityObject);
                }
            }
        }
    }
}

#endif
