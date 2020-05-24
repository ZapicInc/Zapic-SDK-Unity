#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;

#if NET_4_6 || NET_STANDARD_2_0
using System.Threading;
using System.Threading.Tasks;
#endif

namespace ZapicSDK
{
    /// <summary>
    ///     An <see cref="IZapicInterface"/> that mocks the Zapic SDK. This is used when playing the app in the Unity
    ///     editor.
    /// </summary>
    internal sealed class ZapicEditorInterface : IZapicInterface
    {
        /// <summary>The mock delay in milliseconds when calling a query method.</summary>
        private const int QUERY_DELAY = 200;

        /// <summary>The mock delay in milliseconds when calling the <see cref="Start"/> method.</summary>
        private const int START_DELAY = 1000;

        /// <summary>The list of mocked challenges.</summary>
        private readonly ZapicChallenge[] challenges;

        /// <summary>The list of mocked competitions.</summary>
        private readonly ZapicCompetition[] competitions;

        /// <summary>The mock player.</summary>
        private readonly ZapicPlayer player;

        /// <summary>The list of mocked statistics.</summary>
        private readonly ZapicStatistic[] statistics;

        /// <summary>The login handler.</summary>
        private Action<ZapicPlayer> loginHandler;

        /// <summary>The logout handler.</summary>
        private Action<ZapicPlayer> logoutHandler;

        /// <summary>A value indicating whether <see cref="Start"/> been called.</summary>
        private bool started;

        /// <summary>Initializes a new instance of the <see cref="ZapicEditorInterface"/> class.</summary>
        internal ZapicEditorInterface()
        {
            var currentTime = DateTime.UtcNow;

            challenges = new[]
            {
                new ZapicChallenge(
                    Guid.NewGuid().ToString("D"),
                    "Challenge 1",
                    "Win!",
                    null,
                    true,
                    currentTime.AddHours(-10),
                    currentTime.AddHours(5),
                    10,
                    ZapicChallengeStatus.Accepted,
                    1234,
                    "1,234",
                    2),
                new ZapicChallenge(
                    Guid.NewGuid().ToString("D"),
                    "Challenge 2",
                    "Win!",
                    null,
                    false,
                    currentTime.AddHours(-30),
                    currentTime.AddHours(-15),
                    10,
                    ZapicChallengeStatus.Invited,
                    null,
                    null,
                    null),
            };

            competitions = new[]
            {
                new ZapicCompetition(
                    Guid.NewGuid().ToString("D"),
                    "Competition 1",
                    "Win!",
                    "{\"level\":2}",
                    true,
                    currentTime.AddHours(-12),
                    currentTime.AddHours(12),
                    250,
                    ZapicCompetitionStatus.Accepted,
                    467,
                    "467",
                    null,
                    3),
            };

            player = new ZapicPlayer(
                Guid.NewGuid().ToString("D"),
                "John Doe",
                new Uri("https://randomuser.me/api/portraits/men/3.jpg"),
                "AAAAAAAAAABBBBBBBBBBCCCCCCCCCC");

            statistics = new[]
            {
                new ZapicStatistic(Guid.NewGuid().ToString("D"), "Stat 1", null, 1234, "1,234", 90),
                new ZapicStatistic(Guid.NewGuid().ToString("D"), "Stat 2", null, 567.8, "567.80", 15),
            };
        }

        /// <remarks/>
        public Action<ZapicPlayer> OnLogin
        {
            get
            {
                return loginHandler;
            }

            set
            {
                Debug.Log("Zapic: OnLogin was set");
                loginHandler = value;
            }
        }

        /// <remarks/>
        public Action<ZapicPlayer> OnLogout
        {
            get
            {
                return logoutHandler;
            }

            set
            {
                Debug.Log("Zapic: OnLogout was set");
                logoutHandler = value;
            }
        }

        /// <remarks/>
#if NET_4_6 || NET_STANDARD_2_0
        [Obsolete]
#endif
        public void GetChallenges(Action<ZapicChallenge[], ZapicException> callback)
        {
            EnsureStarted();
#if NET_4_6 || NET_STANDARD_2_0
            Debug.LogWarning("Zapic: GetChallenges was called");
#else
            Debug.Log("Zapic: GetChallenges was called");
#endif

            var timer = new System.Timers.Timer(QUERY_DELAY);
            timer.Elapsed += (sender, evt) =>
            {
                timer.Stop();

                try
                {
                    callback(challenges, null);
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred calling the GetChallenges callback");
                    Debug.LogException(e);
                    throw;
                }

                timer.Dispose();
            };
            timer.Start();
        }

#if NET_4_6 || NET_STANDARD_2_0
        /// <remarks/>
        public Task<ZapicChallenge[]> GetChallengesAsync(CancellationToken cancellationToken)
        {
            EnsureStarted();
            Debug.Log("Zapic: GetChallengesAsync was called");

            return Task.Delay(QUERY_DELAY, cancellationToken).ContinueWith(
                parentTask => challenges,
                TaskContinuationOptions.ExecuteSynchronously);
        }
#endif

        /// <remarks/>
#if NET_4_6 || NET_STANDARD_2_0
        [Obsolete]
#endif
        public void GetCompetitions(Action<ZapicCompetition[], ZapicException> callback)
        {
            EnsureStarted();
#if NET_4_6 || NET_STANDARD_2_0
            Debug.LogWarning("Zapic: GetCompetitions was called");
#else
            Debug.Log("Zapic: GetCompetitions was called");
#endif

            var timer = new System.Timers.Timer(QUERY_DELAY);
            timer.Elapsed += (sender, evt) =>
            {
                timer.Stop();

                try
                {
                    callback(competitions, null);
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred calling the GetCompetitions callback");
                    Debug.LogException(e);
                    throw;
                }

                timer.Dispose();
            };
            timer.Start();
        }

#if NET_4_6 || NET_STANDARD_2_0
        /// <remarks/>
        public Task<ZapicCompetition[]> GetCompetitionsAsync(CancellationToken cancellationToken)
        {
            EnsureStarted();
            Debug.Log("Zapic: GetCompetitionsAsync was called");

            return Task.Delay(QUERY_DELAY, cancellationToken).ContinueWith(
                parentTask => competitions,
                TaskContinuationOptions.ExecuteSynchronously);
        }
#endif

        /// <remarks/>
#if NET_4_6 || NET_STANDARD_2_0
        [Obsolete]
#endif
        public void GetPlayer(Action<ZapicPlayer, ZapicException> callback)
        {
            EnsureStarted();
#if NET_4_6 || NET_STANDARD_2_0
            Debug.LogWarning("Zapic: GetPlayer was called");
#else
            Debug.Log("Zapic: GetPlayer was called");
#endif

            var timer = new System.Timers.Timer(QUERY_DELAY);
            timer.Elapsed += (sender, evt) =>
            {
                timer.Stop();

                try
                {
                    callback(player, null);
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred calling the GetPlayer callback");
                    Debug.LogException(e);
                    throw;
                }

                timer.Dispose();
            };
            timer.Start();
        }

#if NET_4_6 || NET_STANDARD_2_0
        /// <remarks/>
        public Task<ZapicPlayer> GetPlayerAsync(CancellationToken cancellationToken)
        {
            EnsureStarted();
            Debug.Log("Zapic: GetPlayerAsync was called");

            return Task.Delay(QUERY_DELAY, cancellationToken).ContinueWith(
                parentTask => player,
                TaskContinuationOptions.ExecuteSynchronously);
        }
#endif

        /// <remarks/>
#if NET_4_6 || NET_STANDARD_2_0
        [Obsolete]
#endif
        public void GetStatistics(Action<ZapicStatistic[], ZapicException> callback)
        {
            EnsureStarted();
#if NET_4_6 || NET_STANDARD_2_0
            Debug.LogWarning("Zapic: GetStatistics was called");
#else
            Debug.Log("Zapic: GetStatistics was called");
#endif

            var timer = new System.Timers.Timer(QUERY_DELAY);
            timer.Elapsed += (sender, evt) =>
            {
                timer.Stop();

                try
                {
                    callback(statistics, null);
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred calling the GetStatistics callback");
                    Debug.LogException(e);
                    throw;
                }

                timer.Dispose();
            };
            timer.Start();
        }

#if NET_4_6 || NET_STANDARD_2_0
        /// <remarks/>
        public Task<ZapicStatistic[]> GetStatisticsAsync(CancellationToken cancellationToken)
        {
            EnsureStarted();
            Debug.Log("Zapic: GetStatisticsAsync was called");

            return Task.Delay(QUERY_DELAY, cancellationToken).ContinueWith(
                parentTask => statistics,
                TaskContinuationOptions.ExecuteSynchronously);
        }
#endif

        /// <remarks/>
        public void HandleInteraction(Dictionary<string, object> parameters)
        {
            EnsureStarted();
            var json = MiniJSON.Json.Serialize(parameters);
            Debug.LogFormat("Zapic: HandleInteraction was called, {0}", json);
        }

        /// <remarks/>
        [Obsolete]
        public ZapicPlayer Player()
        {
            EnsureStarted();
            Debug.LogWarning("Zapic: Player was called");

            return player;
        }

        /// <remarks/>
        public void ShowDefaultPage()
        {
            EnsureStarted();
            Debug.Log("Zapic: ShowDefaultPage was called");
        }

        /// <remarks/>
        public void ShowPage(ZapicPage page)
        {
            EnsureStarted();
            Debug.LogFormat("Zapic: ShowPage was called, {0}", page.ToString());
        }

        /// <remarks/>
        public void Start()
        {
            if (started)
            {
                Debug.LogWarning("Zapic: You should only call Zapic.Start once");
                return;
            }
            else
            {
                started = true;
                Debug.Log("Zapic: You will only see log messages and mock data when running in the Unity editor");
            }

            var timer = new System.Timers.Timer(START_DELAY);
            timer.Elapsed += (sender, evt) =>
            {
                timer.Stop();

                try
                {
                    var callback = loginHandler;
                    if (callback != null)
                    {
                        loginHandler(player);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred calling the OnLogin callback");
                    Debug.LogException(e);
                    throw;
                }

                timer.Dispose();
            };
            timer.Start();
        }

        /// <remarks/>
        public void SubmitEvent(Dictionary<string, object> parameters)
        {
            EnsureStarted();
            var json = MiniJSON.Json.Serialize(parameters);
            Debug.LogFormat("Zapic: SubmitEvent was called, {0}", json);
        }

        /// <remarks/>
        private void EnsureStarted()
        {
            if (!started)
            {
                Debug.LogError("Zapic: You must call Zapic.Start once before calling other methods");
            }
        }
    }
}

#endif
