using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using ZapicSDK.MiniJSON;

namespace ZapicSDK
{
    internal struct ZPCUPlayer
    {
        public string id;
        public string name;
        public string url;
        public string notificationToken;
    }

    internal struct ZPCUCompetition
    {
        public string identifier;
        public string title;
        public string description;
        public string metadata;
        public bool active;
        public string start;
        public string end;
        public bool hasTotalUsers;
        public int totalUsers;
        public int status;
        public string formattedScore;
        public bool hasScore;
        public double score;
        public bool hasLeaderboardRank;
        public int leaderboardRank;
        public bool hasLeagueRank;
        public int leagueRank;
    }

    internal struct ZPCUChallenge
    {
        public string identifier;
        public string title;
        public bool active;
        public string description;
        public string metadata;
        public string start;
        public string end;
        public bool hasTotalUsers;
        public int totalUsers;
        public int status;
        public string formattedScore;
        public bool hasScore;
        public double score;
        public bool hasRank;
        public int rank;
    }

    internal struct ZPCUStatistic
    {
        public string identifier;
        public string title;
        public string formattedScore;
        public bool hasScore;
        public double score;
        public bool hasRank;
        public int rank;
        public bool hasPercentile;
        public float percentile;
    }

    internal struct ZPCUError
    {
        public int code;
        public string message;
    }

    internal sealed class ZapiciOSInterface : IZapicInterface
    {
        private delegate void internal_PlayerLogin(ZPCUPlayer player);

        private delegate void internal_PlayerLogout(ZPCUPlayer player);

        private delegate void internal_GetResponse(string id, IntPtr statsPtr, int size, ZPCUError error);

        private delegate void internal_GetPlayer(string id, ZPCUPlayer player, ZPCUError error);

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
        private static extern void zpc_setLogoutHandler(internal_PlayerLogout logoutCallback);

        [DllImport("__Internal")]
        private static extern void zpc_getStatistics(string id, internal_GetResponse responseCallback);

        [DllImport("__Internal")]
        private static extern void zpc_getPlayer(string id, internal_GetPlayer responseCallback);

        [DllImport("__Internal")]
        private static extern void zpc_getChallenges(string id, internal_GetResponse responseCallback);

        [DllImport("__Internal")]
        private static extern void zpc_getCompetitions(string id, internal_GetResponse responseCallback);

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

        [MonoPInvokeCallback(typeof(internal_GetResponse))]
        private static void _get_statistics(string id, IntPtr ptr, int size, ZPCUError error)
        {
            HandleArrayResponse<ZPCUStatistic, ZapicStatistic>(id, ptr, size, error, ZapicExtensions.ToStatistic);
        }

        [MonoPInvokeCallback(typeof(internal_GetResponse))]
        private static void _get_player(string id, ZPCUPlayer p, ZPCUError error)
        {
            HandleResponse<ZapicPlayer>(id, error, () => p.ToPlayer());
        }

        [MonoPInvokeCallback(typeof(internal_GetResponse))]
        private static void _get_challenges(string id, IntPtr ptr, int size, ZPCUError error)
        {
            HandleArrayResponse<ZPCUChallenge, ZapicChallenge>(id, ptr, size, error, ZapicExtensions.ToChallenge);
        }

        [MonoPInvokeCallback(typeof(internal_GetResponse))]
        private static void _get_competitions(string id, IntPtr ptr, int size, ZPCUError error)
        {
            HandleArrayResponse<ZPCUCompetition, ZapicCompetition>(id, ptr, size, error, ZapicExtensions.ToCompetition);
        }

        private static void HandleResponse<T>(string id, ZPCUError error, Func<T> successCallback) where T : class
        {
            var callback = _queryCallbacks[id] as Action<T, ZapicError>;

            if (callback == null)
            {
                Debug.LogWarning("Unable to find callback");
                return;
            }

            _queryCallbacks.Remove(id);

            //If there is an error
            if (error.code != 0)
            {
                var e = error.ToError();

                callback(null, e);
            }
            else
            {
                Debug.Log("Received a successful response");
                var result = successCallback();
                Debug.LogFormat("Received result {0}", result);

                callback(result, null);
            }
        }

        private static void HandleArrayResponse<TCPP, TUnity>(string id, IntPtr ptr, int size, ZPCUError error, Func<TCPP, TUnity> transform)
        {
            var cppData = MarshalData<TCPP>(ptr, size);
            var unityData = cppData.Select(x => transform(x)).ToArray();
            HandleResponse<TUnity[]>(id, error, () =>
            {
                return unityData;
            });
        }

        private static T[] MarshalData<T>(IntPtr ptr, int size)
        {
            Debug.LogFormat("Marshalling an array of {0} {1}", size, typeof(T).Name);

            if (size == 0)
            {
                Debug.LogFormat("No items, skipping marshalling step");
                return new T[0];
            }

            int structSize = Marshal.SizeOf(typeof(T));
            Console.WriteLine(structSize);

            var array = new T[size];

            for (int i = 0; i < size; ++i)
            {
                Debug.LogFormat("Attempting to marshall item {0} {1}", i, typeof(T).Name);
                IntPtr data = new IntPtr(ptr.ToInt64() + structSize * i);
                T ms = (T) Marshal.PtrToStructure(data, typeof(T));
                array[i] = ms;
            }

            Debug.LogFormat("Marshalled an array of {0} {1}", array.Length, typeof(T).Name);

            return array;
        }

        #endregion DLLImports

        private static Action<ZapicPlayer> _loginHandler;

        private static Action<ZapicPlayer> _logoutHandler;

        private static Dictionary<string, object> _queryCallbacks = new Dictionary<string, object>();

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
        public void ShowPage(ZapicPages page)
        {
            zpc_show(page.ToString().ToLower());
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

        public void GetCompetitions(Action<ZapicCompetition[], ZapicError> callback)
        {
            var id = Guid.NewGuid().ToString();
            _queryCallbacks.Add(id, callback);
            zpc_getCompetitions(id, _get_competitions);
        }

        public void GetStatistics(Action<ZapicStatistic[], ZapicError> callback)
        {
            var id = Guid.NewGuid().ToString();
            _queryCallbacks.Add(id, callback);
            zpc_getStatistics(id, _get_statistics);
        }

        public void GetChallenges(Action<ZapicChallenge[], ZapicError> callback)
        {
            var id = Guid.NewGuid().ToString();
            _queryCallbacks.Add(id, callback);
            zpc_getChallenges(id, _get_challenges);
        }

        public void GetPlayer(Action<ZapicPlayer, ZapicError> callback)
        {
            var id = Guid.NewGuid().ToString();
            _queryCallbacks.Add(id, callback);
            zpc_getPlayer(id, _get_player);
        }
    }

    internal static class ZapicExtensions
    {
        internal static ZapicPlayer ToPlayer(this ZPCUPlayer p)
        {
            Uri url = null;

            if (!string.IsNullOrEmpty(p.url))
            {
                url = new Uri(p.url);
            }

            return new ZapicPlayer(p.id, p.name, url, p.notificationToken);
        }

        internal static ZapicError ToError(this ZPCUError e)
        {
            return new ZapicError(e.code, e.message);
        }

        internal static ZapicStatistic ToStatistic(this ZPCUStatistic s)
        {
            var stat = new ZapicStatistic(
                s.identifier,
                s.title,
                s.formattedScore,
                Null(s.hasScore, s.score),
                Null(s.hasPercentile, s.percentile),
                Null(s.hasRank, s.rank)
            );

            return stat;
        }

        internal static ZapicChallenge ToChallenge(this ZPCUChallenge s)
        {
            return new ZapicChallenge(s.identifier,
                s.title,
                s.active,
                s.description,
                ParseDateTime(s.start),
                ParseDateTime(s.end),
                s.formattedScore,
                Null(s.hasScore, s.score),
                s.metadata,
                (ZapicChallengeStatus) s.status,
                Null(s.hasRank, s.rank),
                Null(s.hasTotalUsers, s.totalUsers));
        }

        internal static ZapicCompetition ToCompetition(this ZPCUCompetition s)
        {
            return new ZapicCompetition(s.identifier,
                s.title,
                s.description,
                s.metadata,
                s.active,
                ParseDateTime(s.start),
                ParseDateTime(s.end),
                Null(s.hasTotalUsers, s.totalUsers),
                (ZapicCompetitionStatus) s.status,
                s.formattedScore,
                Null(s.hasScore, s.score),
                Null(s.hasLeaderboardRank, s.leaderboardRank),
                Null(s.hasLeagueRank, s.leagueRank));
        }

        private static T? Null<T>(bool hasValue, T val) where T : struct
        {
            if (hasValue)
                return val;
            else
                return null;
        }

        private static DateTime? ParseDateTime(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            return DateTime.Parse(str);
        }
    }
}