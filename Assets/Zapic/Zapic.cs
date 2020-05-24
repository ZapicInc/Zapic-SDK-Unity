using System;
using System.Collections.Generic;
using ZapicSDK;

#if NET_4_6 || NET_STANDARD_2_0
using System.Threading;
using System.Threading.Tasks;
#endif

/// <summary>Provides static methods to manage and interact with Zapic.</summary>
/// <remarks>Added in 1.0.0.</remarks>
public static class Zapic
{
    /// <summary>The name of the OneSignal push notification tag.</summary>
    public const string NotificationTokenKey = "zapic_player_token";

    /// <summary>The platform-specific interface.</summary>
    private static readonly IZapicInterface _interface;

    /// <summary>Initializes the platform-specific interface.</summary>
    static Zapic()
    {
#if UNITY_EDITOR
        _interface = new ZapicEditorInterface();
#elif UNITY_ANDROID
        _interface = new ZapicAndroidInterface();
#elif UNITY_IOS
        _interface = new ZapiciOSInterface();
#endif
    }

    /// <summary>
    ///     <para>Gets or sets the callback invoked after the player has been logged in.</para>
    ///     <para>The player that has been logged in is passed to the callback.</para>
    /// </summary>
    /// <remarks>Added in 1.0.0.</remarks>
    public static Action<ZapicPlayer> OnLogin
    {
        get
        {
            return _interface.OnLogin;
        }

        set
        {
            _interface.OnLogin = value;
        }
    }

    /// <summary>
    ///     <para>Gets or sets the callback invoked after the player has been logged out.</para>
    ///     <para>The player that has been logged out is passed to the callback.</para>
    /// </summary>
    /// <remarks>Added in 1.0.0.</remarks>
    public static Action<ZapicPlayer> OnLogout
    {
        get
        {
            return _interface.OnLogout;
        }

        set
        {
            _interface.OnLogout = value;
        }
    }

    /// <summary>Gets the list of challenges for the current player.</summary>
    /// <param name="callback">The result callback. This receives either the list of challenges or an error.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="callback"/> is <c>null</c>.</exception>
    /// <remarks>Added in 1.3.0.</remarks>
    public static void GetChallenges(Action<ZapicChallenge[], ZapicException> callback)
    {
        if (callback == null)
        {
            throw new ArgumentNullException("callback");
        }

        _interface.GetChallenges(callback);
    }

#if NET_4_6 || NET_STANDARD_2_0
    /// <summary>Asynchronously gets the list of challenges for the current player.</summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result returns the list of challenges.
    /// </returns>
    /// <remarks>Added in 1.3.0.</remarks>
    public static Task<ZapicChallenge[]> GetChallengesAsync()
    {
        return GetChallengesAsync(CancellationToken.None);
    }
#endif

#if NET_4_6 || NET_STANDARD_2_0
    /// <summary>Asynchronously gets the list of challenges for the current player.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result returns the list of challenges.
    /// </returns>
    /// <remarks>Added in 1.3.0.</remarks>
    public static Task<ZapicChallenge[]> GetChallengesAsync(CancellationToken cancellationToken)
    {
        return _interface.GetChallengesAsync(cancellationToken);
    }
#endif

    /// <summary>Gets the list of competitions for the current player.</summary>
    /// <param name="callback">
    ///     The result callback. This receives either the list of competitions or an error.
    /// </param>
    /// <exception cref="ArgumentNullException">If <paramref name="callback"/> is <c>null</c>.</exception>
    /// <remarks>Added in 1.3.0.</remarks>
    public static void GetCompetitions(Action<ZapicCompetition[], ZapicException> callback)
    {
        if (callback == null)
        {
            throw new ArgumentNullException("callback");
        }

        _interface.GetCompetitions(callback);
    }

#if NET_4_6 || NET_STANDARD_2_0
    /// <summary>Asynchronously gets the list of competitions for the current player.</summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result returns the list of competitions.
    /// </returns>
    /// <remarks>Added in 1.3.0.</remarks>
    public static Task<ZapicCompetition[]> GetCompetitionsAsync()
    {
        return GetCompetitionsAsync(CancellationToken.None);
    }
#endif

#if NET_4_6 || NET_STANDARD_2_0
    /// <summary>Asynchronously gets the list of competitions for the current player.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result returns the list of competitions.
    /// </returns>
    /// <remarks>Added in 1.3.0.</remarks>
    public static Task<ZapicCompetition[]> GetCompetitionsAsync(CancellationToken cancellationToken)
    {
        return _interface.GetCompetitionsAsync(cancellationToken);
    }
#endif

    /// <summary>Gets the current player.</summary>
    /// <param name="callback">The result callback. This receives either the current player or an error.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="callback"/> is <c>null</c>.</exception>
    /// <remarks>Added in 1.3.0.</remarks>
    public static void GetPlayer(Action<ZapicPlayer, ZapicException> callback)
    {
        if (callback == null)
        {
            throw new ArgumentNullException("callback");
        }

        _interface.GetPlayer(callback);
    }

#if NET_4_6 || NET_STANDARD_2_0
    /// <summary>Asynchronously gets the current player.</summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result returns the current player.
    /// </returns>
    /// <remarks>Added in 1.3.0.</remarks>
    public static Task<ZapicPlayer> GetPlayerAsync()
    {
        return GetPlayerAsync(CancellationToken.None);
    }
#endif

#if NET_4_6 || NET_STANDARD_2_0
    /// <summary>Asynchronously gets the current player.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result returns the current player.
    /// </returns>
    /// <remarks>Added in 1.3.0.</remarks>
    public static Task<ZapicPlayer> GetPlayerAsync(CancellationToken cancellationToken)
    {
        return _interface.GetPlayerAsync(cancellationToken);
    }
#endif

    /// <summary>Gets the list of statistics for the current player.</summary>
    /// <param name="callback">Callback with either the statistics or an error</param>
    /// <exception cref="ArgumentNullException">If <paramref name="callback"/> is <c>null</c>.</exception>
    /// <remarks>Added in 1.3.0.</remarks>
    public static void GetStatistics(Action<ZapicStatistic[], ZapicException> callback)
    {
        if (callback == null)
        {
            throw new ArgumentNullException("callback");
        }

        _interface.GetStatistics(callback);
    }

#if NET_4_6 || NET_STANDARD_2_0
    /// <summary>Asynchronously gets the list of statistics for the current player.</summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result returns the list of statistics.
    /// </returns>
    /// <remarks>Added in 1.3.0.</remarks>
    public static Task<ZapicStatistic[]> GetStatisticsAsync()
    {
        return GetStatisticsAsync(CancellationToken.None);
    }
#endif

#if NET_4_6 || NET_STANDARD_2_0
    /// <summary>Asynchronously gets the list of statistics for the current player.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result returns the list of statistics.
    /// </returns>
    /// <remarks>Added in 1.3.0.</remarks>
    public static Task<ZapicStatistic[]> GetStatisticsAsync(CancellationToken cancellationToken)
    {
        return _interface.GetStatisticsAsync(cancellationToken);
    }
#endif

    /// <summary>
    ///     Handles an interaction event. Depending on the event parameters, Zapic may open and show contextual
    ///     information related to the specific interaction.
    /// </summary>
    /// <param name="parameters">The event parameters.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="parameters"/> is <c>null</c>.</exception>
    /// <remarks>Added in 1.2.0.</remarks>
    public static void HandleInteraction(Dictionary<string, object> parameters)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException("parameters");
        }

        _interface.HandleInteraction(parameters);
    }

    /// <summary>Gets the current player.</summary>
    /// <returns>The current player or <c>null</c> if the current player has not logged in.</returns>
    /// <remarks>Added in 1.0.0.</remarks>
    [Obsolete("Replaced with callback-based method Zapic.GetPlayer")]
    public static ZapicPlayer Player()
    {
        return _interface.Player();
    }

    /// <summary>Opens Zapic and shows the default page.</summary>
    /// <remarks>Added in 1.2.0.</remarks>
    public static void ShowDefaultPage()
    {
        _interface.ShowDefaultPage();
    }

    /// <summary>Opens Zapic and shows the specified page.</summary>
    /// <param name="page">The page to show.</param>
    /// <remarks>Added in 1.3.0.</remarks>
    public static void ShowPage(ZapicPage page)
    {
        _interface.ShowPage(page);
    }

    /// <summary>Opens Zapic and shows the specified page.</summary>
    /// <param name="page">The page to show.</param>
    /// <remarks>Added in 1.0.0.</remarks>
    [Obsolete("Replaced enum ZapicPages with enum ZapicPage")]
    public static void ShowPage(ZapicPages page)
    {
        ZapicPage newPage;
        switch (page)
        {
            case ZapicPages.Challenges:
                newPage = ZapicPage.Challenges;
                break;

            case ZapicPages.CreateChallenge:
                newPage = ZapicPage.CreateChallenge;
                break;

            case ZapicPages.Profile:
                newPage = ZapicPage.Profile;
                break;

            case ZapicPages.Stats:
                newPage = ZapicPage.Stats;
                break;

            default:
                ShowDefaultPage();
                return;
        }

        ShowPage(newPage);
    }

    /// <summary>
    ///     <para>Starts Zapic.</para>
    ///     <para>
    ///         This must be called once to start Zapic. This should be called as soon as possible during app startup.
    ///     </para>
    /// </summary>
    /// <remarks>Added in 1.0.0.</remarks>
    public static void Start()
    {
        _interface.Start();
    }

    /// <summary>Handles a gameplay event.</summary>
    /// <param name="parameters">The event parameters.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="parameters"/> is <c>null</c>.</exception>
    /// <remarks>Added in 1.0.0.</remarks>
    public static void SubmitEvent(Dictionary<string, object> parameters)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException("parameters");
        }

        _interface.SubmitEvent(parameters);
    }
}
