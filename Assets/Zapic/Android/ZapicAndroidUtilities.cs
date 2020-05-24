#if !UNITY_EDITOR && UNITY_ANDROID

using System;
using UnityEngine;

namespace ZapicSDK
{
    /// <summary>Provides utility methods to convert the Java class instances to C# class instances.</summary>
    internal static class ZapicAndroidUtilities
    {
        /// <summary>
        ///     Converts a <c>ZapicChallenge</c> Java class instance to a <see cref="ZapicChallenge"/> instance.
        /// </summary>
        /// <param name="challengeObject">The <c>ZapicChallenge</c> Java class instance.</param>
        /// <returns>The converted <see cref="ZapicChallenge"/> instance or <c>null</c>.</returns>
        /// <exception cref="ZapicException">If an error occurs converting the Java class instance.</exception>
        internal static ZapicChallenge ConvertChallenge(AndroidJavaObject challengeObject)
        {
            try
            {
                if (challengeObject == null || challengeObject.GetRawObject() == IntPtr.Zero)
                {
                    return null;
                }

                // active
                var active = challengeObject.Get<bool>("active");

                // description
                var description = challengeObject.Get<string>("description");

                // end.getTime()
                DateTime end;
                using (var endObject = challengeObject.Get<AndroidJavaObject>("end"))
                {
                    if (endObject == null || endObject.GetRawObject() == IntPtr.Zero)
                    {
                        throw new ZapicException(
                            ZapicErrorCode.INVALID_RESPONSE,
                            "The end date and time must not be null");
                    }

#if NET_4_6 || NET_STANDARD_2_0
                    end = DateTimeOffset.FromUnixTimeMilliseconds(endObject.Call<long>("getTime")).UtcDateTime;
#else
                    end = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) +
                        TimeSpan.FromMilliseconds(endObject.Call<long>("getTime"));
#endif
                }

                // formattedScore
                var formattedScore = challengeObject.Get<string>("formattedScore");

                // id
                var id = challengeObject.Get<string>("id");

                // metadata
                var metadata = challengeObject.Get<string>("metadata");

                // rank.longValue()
                long? rank;
                using (var rankObject = challengeObject.Get<AndroidJavaObject>("rank"))
                {
                    rank = rankObject == null || rankObject.GetRawObject() == IntPtr.Zero
                        ? (long?)null
                        : rankObject.Call<long>("longValue");
                }

                // score.doubleValue()
                double? score;
                using (var scoreObject = challengeObject.Get<AndroidJavaObject>("score"))
                {
                    score = scoreObject == null || scoreObject.GetRawObject() == IntPtr.Zero
                        ? (double?)null
                        : scoreObject.Call<double>("doubleValue");
                }

                // start.getTime()
                DateTime start;
                using (var startObject = challengeObject.Get<AndroidJavaObject>("start"))
                {
                    if (startObject == null || startObject.GetRawObject() == IntPtr.Zero)
                    {
                        throw new ZapicException(
                            ZapicErrorCode.INVALID_RESPONSE,
                            "The start date and time must not be null");
                    }

#if NET_4_6 || NET_STANDARD_2_0
                    start = DateTimeOffset.FromUnixTimeMilliseconds(startObject.Call<long>("getTime")).UtcDateTime;
#else
                    start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) +
                            TimeSpan.FromMilliseconds(startObject.Call<long>("getTime"));
#endif
                }

                // status
                var status = (ZapicChallengeStatus)challengeObject.Get<int>("status");

                // title
                var title = challengeObject.Get<string>("title");

                // totalUsers
                var totalUsers = challengeObject.Get<long>("totalUsers");

                return new ZapicChallenge(
                    id,
                    title,
                    description,
                    metadata,
                    active,
                    start,
                    end,
                    totalUsers,
                    status,
                    score,
                    formattedScore,
                    rank);
            }
            catch (Exception e)
            {
                throw new ZapicException(
                    ZapicErrorCode.INVALID_RESPONSE,
                    "An error occurred converting the challenge",
                    e);
            }
        }

        /// <summary>
        ///     Converts an array of <c>ZapicChallenge</c> Java class instances to an array of
        ///     <see cref="ZapicChallenge"/> instances.
        /// </summary>
        /// <param name="arrayObject">The array of <c>ZapicChallenge</c> Java class instances.</param>
        /// <returns>The converted array of <see cref="ZapicChallenge"/> instances or <c>null</c>.</returns>
        /// <exception cref="ZapicException">If an error occurs converting the Java class instances.</exception>
        internal static ZapicChallenge[] ConvertChallenges(AndroidJavaObject arrayObject)
        {
            if (arrayObject == null)
            {
                return null;
            }

            AndroidJavaObject[] itemObjects;
            try
            {
                var arrayPointer = arrayObject.GetRawObject();
                itemObjects = arrayPointer == IntPtr.Zero
                    ? null
                    : AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(arrayPointer);
                if (itemObjects == null)
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new ZapicException(
                    ZapicErrorCode.INVALID_RESPONSE,
                    "An error occurred converting the array of challenges",
                    e);
            }

            var challenges = new ZapicChallenge[itemObjects.Length];
            for (var i = 0; i < itemObjects.Length; i++)
            {
                try
                {
                    var itemObject = itemObjects[i];
                    challenges[i] = ConvertChallenge(itemObject);
                    itemObject.Dispose();
                }
                catch
                {
                    for (var j = i; j < itemObjects.Length; j++)
                    {
                        itemObjects[j].Dispose();
                    }

                    throw;
                }
            }

            return challenges;
        }

        /// <summary>
        ///     Converts a <c>ZapicCompetition</c> Java class instance to a <see cref="ZapicCompetition"/> instance.
        /// </summary>
        /// <param name="competitionObject">The <c>ZapicCompetition</c> Java class instance.</param>
        /// <returns>The converted <see cref="ZapicCompetition"/> instance or <c>null</c>.</returns>
        /// <exception cref="ZapicException">If an error occurs converting the Java class instance.</exception>
        internal static ZapicCompetition ConvertCompetition(AndroidJavaObject competitionObject)
        {
            try
            {
                if (competitionObject == null || competitionObject.GetRawObject() == IntPtr.Zero)
                {
                    return null;
                }

                // active
                var active = competitionObject.Get<bool>("active");

                // description
                var description = competitionObject.Get<string>("description");

                // end.getTime()
                DateTime end;
                using (var endObject = competitionObject.Get<AndroidJavaObject>("end"))
                {
                    if (endObject == null || endObject.GetRawObject() == IntPtr.Zero)
                    {
                        throw new ZapicException(
                            ZapicErrorCode.INVALID_RESPONSE,
                            "The end date and time must not be null");
                    }

#if NET_4_6 || NET_STANDARD_2_0
                    end = DateTimeOffset.FromUnixTimeMilliseconds(endObject.Call<long>("getTime")).UtcDateTime;
#else
                    end = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) +
                        TimeSpan.FromMilliseconds(endObject.Call<long>("getTime"));
#endif
                }

                // formattedScore
                var formattedScore = competitionObject.Get<string>("formattedScore");

                // id
                var id = competitionObject.Get<string>("id");

                // leaderboardRank.longValue()
                long? leaderboardRank;
                using (var leaderboardRankObject = competitionObject.Get<AndroidJavaObject>("leaderboardRank"))
                {
                    leaderboardRank = leaderboardRankObject == null || leaderboardRankObject.GetRawObject() == IntPtr.Zero
                        ? (long?)null
                        : leaderboardRankObject.Call<long>("longValue");
                }

                // leagueRank.longValue()
                long? leagueRank;
                using (var leagueRankObject = competitionObject.Get<AndroidJavaObject>("leagueRank"))
                {
                    leagueRank = leagueRankObject == null || leagueRankObject.GetRawObject() == IntPtr.Zero
                        ? (long?)null
                        : leagueRankObject.Call<long>("longValue");
                }

                // metadata
                var metadata = competitionObject.Get<string>("metadata");

                // score.doubleValue()
                double? score;
                using (var scoreObject = competitionObject.Get<AndroidJavaObject>("score"))
                {
                    score = scoreObject == null || scoreObject.GetRawObject() == IntPtr.Zero
                        ? (double?)null
                        : scoreObject.Call<double>("doubleValue");
                }

                // start.getTime()
                DateTime start;
                using (var startObject = competitionObject.Get<AndroidJavaObject>("start"))
                {
                    if (startObject == null || startObject.GetRawObject() == IntPtr.Zero)
                    {
                        throw new ZapicException(
                            ZapicErrorCode.INVALID_RESPONSE,
                            "The start date and time must not be null");
                    }

#if NET_4_6 || NET_STANDARD_2_0
                    start = DateTimeOffset.FromUnixTimeMilliseconds(startObject.Call<long>("getTime")).UtcDateTime;
#else
                    start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) +
                            TimeSpan.FromMilliseconds(startObject.Call<long>("getTime"));
#endif
                }

                // status
                var status = (ZapicCompetitionStatus)competitionObject.Get<int>("status");

                // title
                var title = competitionObject.Get<string>("title");

                // totalUsers
                var totalUsers = competitionObject.Get<long>("totalUsers");

                return new ZapicCompetition(
                    id,
                    title,
                    description,
                    metadata,
                    active,
                    start,
                    end,
                    totalUsers,
                    status,
                    score,
                    formattedScore,
                    leaderboardRank,
                    leagueRank);
            }
            catch (Exception e)
            {
                throw new ZapicException(
                    ZapicErrorCode.INVALID_RESPONSE,
                    "An error occurred converting the competition",
                    e);
            }
        }

        /// <summary>
        ///     Converts an array of <c>ZapicCompetition</c> Java class instances to an array of
        ///     <see cref="ZapicCompetition"/> instances.
        /// </summary>
        /// <param name="arrayObject">The array of <c>ZapicCompetition</c> Java class instances.</param>
        /// <returns>The converted array of <see cref="ZapicCompetition"/> instances or <c>null</c>.</returns>
        /// <exception cref="ZapicException">If an error occurs converting the Java class instances.</exception>
        internal static ZapicCompetition[] ConvertCompetitions(AndroidJavaObject arrayObject)
        {
            if (arrayObject == null)
            {
                return null;
            }

            AndroidJavaObject[] itemObjects;
            try
            {
                var arrayPointer = arrayObject.GetRawObject();
                itemObjects = arrayPointer == IntPtr.Zero
                    ? null
                    : AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(arrayPointer);
                if (itemObjects == null)
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new ZapicException(
                    ZapicErrorCode.INVALID_RESPONSE,
                    "An error occurred converting the array of competitions",
                    e);
            }

            var competitions = new ZapicCompetition[itemObjects.Length];
            for (var i = 0; i < itemObjects.Length; i++)
            {
                try
                {
                    var itemObject = itemObjects[i];
                    competitions[i] = ConvertCompetition(itemObject);
                    itemObject.Dispose();
                }
                catch
                {
                    for (var j = i; j < itemObjects.Length; j++)
                    {
                        itemObjects[j].Dispose();
                    }

                    throw;
                }
            }

            return competitions;
        }

        /// <summary>
        ///     Converts a <c>ZapicException</c> Java class instance to a <see cref="ZapicException"/> instance.
        /// </summary>
        /// <param name="errorObject">The <c>ZapicException</c> Java class instance.</param>
        /// <returns>The converted <see cref="ZapicException"/> instance or <c>null</c>.</returns>
        /// <exception cref="ZapicException">If an error occurs converting the Java class instance.</exception>
        internal static ZapicException ConvertError(AndroidJavaObject errorObject)
        {
            try
            {
                if (errorObject == null || errorObject.GetRawObject() == IntPtr.Zero)
                {
                    return null;
                }

                // code
                var code = errorObject.Get<int>("code");

                // getMessage()
                var message = errorObject.Call<string>("getMessage");

                return new ZapicException(code, message);
            }
            catch (Exception e)
            {
                throw new ZapicException(
                    ZapicErrorCode.INVALID_RESPONSE,
                    "An error occurred converting the exception",
                    e);
            }
        }

        /// <summary>
        ///     Converts a <c>ZapicPlayer</c> Java class instance to a <see cref="ZapicPlayer"/> instance.
        /// </summary>
        /// <param name="playerObject">The <c>ZapicPlayer</c> Java class instance.</param>
        /// <returns>The converted <see cref="ZapicPlayer"/> instance or <c>null</c>.</returns>
        /// <exception cref="ZapicException">If an error occurs converting the Java class instance.</exception>
        internal static ZapicPlayer ConvertPlayer(AndroidJavaObject playerObject)
        {
            try
            {
                if (playerObject == null || playerObject.GetRawObject() == IntPtr.Zero)
                {
                    return null;
                }

                // iconUrl.toString()
                Uri iconUrl;
                using (var iconUrlObject = playerObject.Get<AndroidJavaObject>("iconUrl"))
                {
                    if (iconUrlObject == null || iconUrlObject.GetRawObject() == IntPtr.Zero)
                    {
                        throw new ZapicException(ZapicErrorCode.INVALID_RESPONSE, "The icon URL must not be null");
                    }

                    iconUrl = new Uri(iconUrlObject.Call<string>("toString"));
                }

                // id
                var id = playerObject.Get<string>("id");

                // name
                var name = playerObject.Get<string>("name");

                // notificationToken
                var notificationToken = playerObject.Get<string>("notificationToken");

                return new ZapicPlayer(id, name, iconUrl, notificationToken);
            }
            catch (Exception e)
            {
                throw new ZapicException(ZapicErrorCode.INVALID_RESPONSE, "An error occurred converting the player", e);
            }
        }

        /// <summary>
        ///     Converts a <c>ZapicStatistic</c> Java class instance to a <see cref="ZapicStatistic"/> instance.
        /// </summary>
        /// <param name="statisticObject">The <c>ZapicStatistic</c> Java class instance.</param>
        /// <returns>The converted <see cref="ZapicStatistic"/> instance or <c>null</c>.</returns>
        /// <exception cref="ZapicException">If an error occurs converting the Java class instance.</exception>
        internal static ZapicStatistic ConvertStatistic(AndroidJavaObject statisticObject)
        {
            try
            {
                if (statisticObject == null || statisticObject.GetRawObject() == IntPtr.Zero)
                {
                    return null;
                }

                // formattedScore
                var formattedScore = statisticObject.Get<string>("formattedScore");

                // id
                var id = statisticObject.Get<string>("id");

                // metadata
                var metadata = statisticObject.Get<string>("metadata");

                // percentile.intValue()
                int? percentile;
                using (var percentileObject = statisticObject.Get<AndroidJavaObject>("percentile"))
                {
                    percentile = percentileObject == null || percentileObject.GetRawObject() == IntPtr.Zero
                        ? (int?)null
                        : percentileObject.Call<int>("intValue");
                }

                // score.doubleValue()
                double? score;
                using (var scoreObject = statisticObject.Get<AndroidJavaObject>("score"))
                {
                    score = scoreObject == null || scoreObject.GetRawObject() == IntPtr.Zero
                        ? (double?)null
                        : scoreObject.Call<double>("doubleValue");
                }

                // title
                var title = statisticObject.Get<string>("title");

                return new ZapicStatistic(id, title, metadata, score, formattedScore, percentile);
            }
            catch (Exception e)
            {
                throw new ZapicException(
                    ZapicErrorCode.INVALID_RESPONSE,
                    "An error occurred converting the statistic",
                    e);
            }
        }

        /// <summary>
        ///     Converts an array of <c>ZapicStatistic</c> Java class instances to an array of
        ///     <see cref="ZapicStatistic"/> instances.
        /// </summary>
        /// <param name="arrayObject">The array of <c>ZapicStatistic</c> Java class instances.</param>
        /// <returns>The converted array of <see cref="ZapicStatistic"/> instances or <c>null</c>.</returns>
        /// <exception cref="ZapicException">If an error occurs converting the Java class instances.</exception>
        internal static ZapicStatistic[] ConvertStatistics(AndroidJavaObject arrayObject)
        {
            if (arrayObject == null)
            {
                return null;
            }

            AndroidJavaObject[] itemObjects;
            try
            {
                var arrayPointer = arrayObject.GetRawObject();
                itemObjects = arrayPointer == IntPtr.Zero
                    ? null
                    : AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(arrayPointer);
                if (itemObjects == null)
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new ZapicException(
                    ZapicErrorCode.INVALID_RESPONSE,
                    "An error occurred converting the array of statistics",
                    e);
            }

            var statistics = new ZapicStatistic[itemObjects.Length];
            for (var i = 0; i < itemObjects.Length; i++)
            {
                try
                {
                    var itemObject = itemObjects[i];
                    statistics[i] = ConvertStatistic(itemObject);
                    itemObject.Dispose();
                }
                catch
                {
                    for (var j = i; j < itemObjects.Length; j++)
                    {
                        itemObjects[j].Dispose();
                    }

                    throw;
                }
            }

            return statistics;
        }
    }
}

#endif
