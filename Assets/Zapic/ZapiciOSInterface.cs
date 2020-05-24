#if !UNITY_EDITOR && UNITY_IOS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using ZapicSDK.MiniJSON;

#if NET_4_6 || NET_STANDARD_2_0
using System.Threading;
using System.Threading.Tasks;
#endif

namespace ZapicSDK
{
    #region Objective-C Structs

    [StructLayout(LayoutKind.Sequential)]
    internal struct ZPCUChallenge
    {
        public string identifier;
        public string title;
        public bool active;
        public string description;
        public string metadata;
        public string start;
        public string end;
        public long totalUsers;
        public int status;
        public string formattedScore;
        public bool hasScore;
        public bool hasRank;
        public double score;
        public int rank;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ZPCUCompetition
    {
        public string identifier;
        public string title;
        public string description;
        public string metadata;
        public bool active;
        public string start;
        public string end;
        public long totalUsers;
        public int status;
        public string formattedScore;
        public bool hasScore;
        public double score;
        public bool hasLeaderboardRank;
        public int leaderboardRank;
        public bool hasLeagueRank;
        public int leagueRank;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ZPCUError
    {
        public int code;
        public string message;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ZPCUPlayer
    {
        public string id;
        public string name;
        public string url;
        public string notificationToken;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ZPCUStatistic
    {
        public string identifier;
        public string title;
        public string metadata;
        public string formattedScore;
        public bool hasScore;
        public double score;
        public bool hasPercentile;
        public int percentile;
    }

    /// <summary>Provides utility methods to convert the Objective-C structs to C# class instances.</summary>
    internal static class ZPCUUtilities
    {
        /// <summary>Converts a <c>ZPCUChallenge</c> struct to a <see cref="ZapicChallenge"/> instance.</summary>
        /// <param name="s">The <c>ZPCUChallenge</c> struct.</param>
        /// <returns>The converted <see cref="ZapicChallenge"/>.</returns>
        /// <exception cref="ZapicException">If an error occurs converting the struct.</exception>
        internal static ZapicChallenge ConvertChallenge(ZPCUChallenge s)
        {
            var challenge = new ZapicChallenge(
                s.identifier,
                s.title,
                s.description,
                s.metadata,
                s.active,
                ParseDateTime(s.start),
                ParseDateTime(s.end),
                s.totalUsers,
                (ZapicChallengeStatus)s.status,
                ParseNullable(s.hasScore, s.score),
                s.formattedScore,
                ParseNullable(s.hasRank, s.rank)
            );
            return challenge;
        }

        /// <summary>Converts a <c>ZPCUCompetition</c> struct to a <see cref="ZapicCompetition"/> instance.</summary>
        /// <param name="s">The <c>ZPCUCompetition</c> struct.</param>
        /// <returns>The converted <see cref="ZapicCompetition"/>.</returns>
        /// <exception cref="ZapicException">If an error occurs converting the struct.</exception>
        internal static ZapicCompetition ConvertCompetition(ZPCUCompetition s)
        {
            var competition = new ZapicCompetition(
                s.identifier,
                s.title,
                s.description,
                s.metadata,
                s.active,
                ParseDateTime(s.start),
                ParseDateTime(s.end),
                s.totalUsers,
                (ZapicCompetitionStatus)s.status,
                ParseNullable(s.hasScore, s.score),
                s.formattedScore,
                ParseNullable(s.hasLeaderboardRank, s.leaderboardRank),
                ParseNullable(s.hasLeagueRank, s.leagueRank)
            );
            return competition;
        }

        /// <summary>Converts a <c>ZPCUError</c> struct to a <see cref="ZapicException"/> instance.</summary>
        /// <param name="s">The <c>ZPCUError</c> struct.</param>
        /// <returns>The converted <see cref="ZapicException"/>.</returns>
        internal static ZapicException ConvertError(ZPCUError s)
        {
            return new ZapicException(s.code, s.message);
        }

        /// <summary>Converts a <c>ZPCUPlayer</c> struct to a <see cref="ZapicPlayer"/> instance.</summary>
        /// <param name="s">The <c>ZPCUPlayer</c> struct.</param>
        /// <returns>The converted <see cref="ZapicPlayer"/>.</returns>
        /// <exception cref="ZapicException">If an error occurs converting the struct.</exception>
        internal static ZapicPlayer ConvertPlayer(ZPCUPlayer s)
        {
            if (string.IsNullOrEmpty(s.url))
            {
                throw new ZapicException(ZapicErrorCode.INVALID_RESPONSE, "The icon URL must not be null");
            }

            var player = new ZapicPlayer(s.id, s.name, new Uri(s.url), s.notificationToken);
            return player;
        }

        /// <summary>Converts a <c>ZPCUStatistic</c> struct to a <see cref="ZapicStatistic"/> instance.</summary>
        /// <param name="s">The <c>ZPCUStatistic</c> struct.</param>
        /// <returns>The converted <see cref="ZapicStatistic"/>.</returns>
        internal static ZapicStatistic ConvertStatistic(ZPCUStatistic s)
        {
            var statistic = new ZapicStatistic(
                s.identifier,
                s.title,
                s.metadata,
                ParseNullable(s.hasScore, s.score),
                s.formattedScore,
                ParseNullable(s.hasPercentile, s.percentile)
            );
            return statistic;
        }

        private static DateTime ParseDateTime(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ZapicException(ZapicErrorCode.INVALID_RESPONSE, "The date and time must not be null");
            }

            return DateTime.Parse(s);
        }

        private static T? ParseNullable<T>(bool hasValue, T value)
            where T : struct
        {
            return hasValue ? value : (T?)null;
        }
    }

    #endregion Objective-C Structs

    /// <summary>An <see cref="IZapicInterface"/> that communicates with the iOS SDK.</summary>
    internal sealed class ZapiciOSInterface : IZapicInterface
    {
        #region Objective-C Interop

        private delegate void internal_GetPlayer(string id, ZPCUPlayer player, ZPCUError error);

        private delegate void internal_GetResponse(string id, IntPtr statsPtr, int size, ZPCUError error);

        private delegate void internal_PlayerLogin(ZPCUPlayer player);

        private delegate void internal_PlayerLogout(ZPCUPlayer player);

        [MonoPInvokeCallback(typeof(internal_GetResponse))]
        private static void _get_challenges(string id, IntPtr result, int resultLength, ZPCUError error)
        {
            HandleArrayResponse<ZPCUChallenge, ZapicChallenge>(
                id,
                result,
                resultLength,
                ZPCUUtilities.ConvertChallenge,
                error);
        }

        [MonoPInvokeCallback(typeof(internal_GetResponse))]
        private static void _get_competitions(string id, IntPtr result, int resultLength, ZPCUError error)
        {
            HandleArrayResponse<ZPCUCompetition, ZapicCompetition>(
                id,
                result,
                resultLength,
                ZPCUUtilities.ConvertCompetition,
                error);
        }

        [MonoPInvokeCallback(typeof(internal_GetResponse))]
        private static void _get_player(string id, ZPCUPlayer result, ZPCUError error)
        {
            HandleObjectResponse(id, result, ZPCUUtilities.ConvertPlayer, error);
        }

        [MonoPInvokeCallback(typeof(internal_GetResponse))]
        private static void _get_statistics(string id, IntPtr result, int resultLength, ZPCUError error)
        {
            HandleArrayResponse<ZPCUStatistic, ZapicStatistic>(
                id,
                result,
                resultLength,
                ZPCUUtilities.ConvertStatistic,
                error);
        }

        [MonoPInvokeCallback(typeof(internal_PlayerLogin))]
        private static void _player_login(ZPCUPlayer p)
        {
            var handler = loginHandler;
            if (handler == null)
            {
                return;
            }

            handler(ZPCUUtilities.ConvertPlayer(p));
        }

        [MonoPInvokeCallback(typeof(internal_PlayerLogout))]
        private static void _player_logout(ZPCUPlayer p)
        {
            var handler = logoutHandler;
            if (handler == null)
            {
                return;
            }

            handler(ZPCUUtilities.ConvertPlayer(p));
        }

        private static void HandleArrayResponse<TNative, TUnity>(
            string id,
            IntPtr result,
            int resultLength,
            Func<TNative, TUnity> convertResult,
            ZPCUError error)
            where TUnity : class
        {
            ZapicException exception;
            TUnity[] items;
            if (error.code == 0)
            {
                var array = MarshalData<TNative>(result, resultLength);
                items = array.Select(convertResult).ToArray();
                exception = null;
            }
            else
            {
                exception = ZPCUUtilities.ConvertError(error);
                items = null;
            }

            HandleResponse(id, items, exception);
        }

        private static void HandleObjectResponse<TNative, TUnity>(
            string id,
            TNative result,
            Func<TNative, TUnity> convertResult,
            ZPCUError error)
            where TUnity : class
        {
            ZapicException exception;
            TUnity item;
            if (error.code == 0)
            {
                item = convertResult(result);
                exception = null;
            }
            else
            {
                exception = ZPCUUtilities.ConvertError(error);
                item = null;
            }

            HandleResponse(id, item, exception);
        }

        private static void HandleResponse<T>(string id, T result, ZapicException exception)
            where T : class
        {
            object queryCallback;
            if (!queryCallbacks.TryGetValue(id, out queryCallback))
            {
                Debug.LogWarningFormat("Zapic: Failed to find callback for request {0}", id);
                return;
            }

            var callback = queryCallback as Action<T, ZapicException>;
            if (callback == null)
            {
                Debug.LogWarningFormat("Zapic: Failed to find callback for request {0}", id);
                return;
            }

            queryCallbacks.Remove(id);
            callback(result, exception);
        }

        private static T[] MarshalData<T>(IntPtr result, int resultLength)
        {
            Debug.LogFormat("Zapic: Marshalling an array of {0} {1}", resultLength, typeof(T).Name);
            if (resultLength == 0)
            {
                return new T[0];
            }

            var sSize = Marshal.SizeOf(typeof(T));
            var array = new T[resultLength];
            for (int i = 0; i < resultLength; ++i)
            {
                var item = new IntPtr(result.ToInt64() + sSize * i);
                var s = (T)Marshal.PtrToStructure(item, typeof(T));
                array[i] = s;
            }

            return array;
        }

        [DllImport("__Internal")]
        private static extern void zpc_getChallenges(string id, internal_GetResponse responseCallback);

        [DllImport("__Internal")]
        private static extern void zpc_getCompetitions(string id, internal_GetResponse responseCallback);

        [DllImport("__Internal")]
        private static extern void zpc_getPlayer(string id, internal_GetPlayer responseCallback);

        [DllImport("__Internal")]
        private static extern void zpc_getStatistics(string id, internal_GetResponse responseCallback);

        [DllImport("__Internal")]
        private static extern void zpc_handleInteraction(string dataJson);

        [DllImport("__Internal")]
        private static extern void zpc_setLoginHandler(internal_PlayerLogin loginCallback);

        [DllImport("__Internal")]
        private static extern void zpc_setLogoutHandler(internal_PlayerLogout logoutCallback);

        [DllImport("__Internal")]
        private static extern void zpc_show(string pageName);

        [DllImport("__Internal")]
        private static extern void zpc_showDefault();

        [DllImport("__Internal")]
        private static extern void zpc_start();

        [DllImport("__Internal")]
        private static extern void zpc_submitEventWithParams(string eventJson);

        #endregion Objective-C Interop

        private static readonly Dictionary<string, object> queryCallbacks = new Dictionary<string, object>();

        private static Action<ZapicPlayer> loginHandler;

        private static Action<ZapicPlayer> logoutHandler;

        internal ZapiciOSInterface()
        {
            zpc_setLoginHandler(_player_login);
            zpc_setLogoutHandler(_player_logout);
        }

        public Action<ZapicPlayer> OnLogin
        {
            get
            {
                return loginHandler;
            }

            set
            {
                loginHandler = value;
            }
        }

        public Action<ZapicPlayer> OnLogout
        {
            get
            {
                return logoutHandler;
            }

            set
            {
                logoutHandler = value;
            }
        }

        public void GetChallenges(Action<ZapicChallenge[], ZapicException> callback)
        {
            var id = Guid.NewGuid().ToString();
            queryCallbacks.Add(id, callback);
            zpc_getChallenges(id, _get_challenges);
        }

#if NET_4_6 || NET_STANDARD_2_0
        public Task<ZapicChallenge[]> GetChallengesAsync(CancellationToken cancellationToken)
        {
            var task = new TaskCompletionSource<ZapicChallenge[]>();

            var id = Guid.NewGuid().ToString();
            queryCallbacks.Add(id, new Action<ZapicChallenge[], ZapicException>((result, error) =>
            {
                if (error != null)
                {
                    task.TrySetException(error);
                }
                else
                {
                    task.TrySetResult(result);
                }
            }));
            zpc_getChallenges(id, _get_challenges);

            return task.Task;
        }
#endif

        public void GetCompetitions(Action<ZapicCompetition[], ZapicException> callback)
        {
            var id = Guid.NewGuid().ToString();
            queryCallbacks.Add(id, callback);
            zpc_getCompetitions(id, _get_competitions);
        }

#if NET_4_6 || NET_STANDARD_2_0
        public Task<ZapicCompetition[]> GetCompetitionsAsync(CancellationToken cancellationToken)
        {
            var task = new TaskCompletionSource<ZapicCompetition[]>();

            var id = Guid.NewGuid().ToString();
            queryCallbacks.Add(id, new Action<ZapicCompetition[], ZapicException>((result, error) =>
            {
                if (error != null)
                {
                    task.TrySetException(error);
                }
                else
                {
                    task.TrySetResult(result);
                }
            }));
            zpc_getCompetitions(id, _get_competitions);

            return task.Task;
        }
#endif

        public void GetPlayer(Action<ZapicPlayer, ZapicException> callback)
        {
            var id = Guid.NewGuid().ToString();
            queryCallbacks.Add(id, callback);
            zpc_getPlayer(id, _get_player);
        }

#if NET_4_6 || NET_STANDARD_2_0
        public Task<ZapicPlayer> GetPlayerAsync(CancellationToken cancellationToken)
        {
            var task = new TaskCompletionSource<ZapicPlayer>();

            var id = Guid.NewGuid().ToString();
            queryCallbacks.Add(id, new Action<ZapicPlayer, ZapicException>((result, error) =>
            {
                if (error != null)
                {
                    task.TrySetException(error);
                }
                else
                {
                    task.TrySetResult(result);
                }
            }));
            zpc_getPlayer(id, _get_player);

            return task.Task;
        }
#endif

        public void GetStatistics(Action<ZapicStatistic[], ZapicException> callback)
        {
            var id = Guid.NewGuid().ToString();
            queryCallbacks.Add(id, callback);
            zpc_getStatistics(id, _get_statistics);
        }

#if NET_4_6 || NET_STANDARD_2_0
        public Task<ZapicStatistic[]> GetStatisticsAsync(CancellationToken cancellationToken)
        {
            var task = new TaskCompletionSource<ZapicStatistic[]>();

            var id = Guid.NewGuid().ToString();
            queryCallbacks.Add(id, new Action<ZapicStatistic[], ZapicException>((result, error) =>
            {
                if (error != null)
                {
                    task.TrySetException(error);
                }
                else
                {
                    task.TrySetResult(result);
                }
            }));
            zpc_getStatistics(id, _get_statistics);

            return task.Task;
        }
#endif

        public void HandleInteraction(Dictionary<string, object> parameters)
        {
            var json = Json.Serialize(parameters);
            zpc_handleInteraction(json);
        }

        [Obsolete]
        public ZapicPlayer Player()
        {
            // TODO: Implement method.
            throw new NotImplementedException();
        }

        public void ShowDefaultPage()
        {
            zpc_showDefault();
        }

        public void ShowPage(ZapicPage page)
        {
            zpc_show(page.ToString().ToLower());
        }

        public void Start()
        {
            zpc_start();
        }

        public void SubmitEvent(Dictionary<string, object> parameters)
        {
            var json = Json.Serialize(parameters);
            zpc_submitEventWithParams(json);
        }
    }
}

#endif
