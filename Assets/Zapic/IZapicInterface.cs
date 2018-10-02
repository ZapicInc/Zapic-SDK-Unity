using System;
using System.Collections.Generic;

#if NET_4_6 || NET_STANDARD_2_0
using System.Threading;
using System.Threading.Tasks;
#endif

namespace ZapicSDK
{
    internal interface IZapicInterface
    {
        Action<ZapicPlayer> OnLogin { get; set; }

        Action<ZapicPlayer> OnLogout { get; set; }

        void GetChallenges(Action<ZapicChallenge[], ZapicException> callback);

#if NET_4_6 || NET_STANDARD_2_0
        Task<ZapicChallenge[]> GetChallengesAsync(CancellationToken cancellationToken);
#endif

        void GetCompetitions(Action<ZapicCompetition[], ZapicException> callback);

#if NET_4_6 || NET_STANDARD_2_0
        Task<ZapicCompetition[]> GetCompetitionsAsync(CancellationToken cancellationToken);
#endif

        void GetPlayer(Action<ZapicPlayer, ZapicException> callback);

#if NET_4_6 || NET_STANDARD_2_0
        Task<ZapicPlayer> GetPlayerAsync(CancellationToken cancellationToken);
#endif

        void GetStatistics(Action<ZapicStatistic[], ZapicException> callback);

#if NET_4_6 || NET_STANDARD_2_0
        Task<ZapicStatistic[]> GetStatisticsAsync(CancellationToken cancellationToken);
#endif

        void HandleInteraction(Dictionary<string, object> parameters);

        [Obsolete]
        ZapicPlayer Player();

        void ShowDefaultPage();

        void ShowPage(ZapicPage page);

        void Start();

        void SubmitEvent(Dictionary<string, object> parameters);
    }
}
