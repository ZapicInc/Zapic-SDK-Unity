#if !UNITY_EDITOR && UNITY_ANDROID

using System;
using UnityEngine;

namespace ZapicSDK
{
    /// <summary>An implementation of the <c>ZapicPlayerAuthenticationHandler</c> Java interface.</summary>
    internal sealed class ZapicAndroidAuthenticationHandler : AndroidJavaProxy
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ZapicAndroidAuthenticationHandler"/> class.
        /// </summary>
        internal ZapicAndroidAuthenticationHandler()
            : base("com/zapic/sdk/android/ZapicPlayerAuthenticationHandler")
        {
        }

        /// <summary>
        ///     <para>Gets or sets the callback invoked after the player has been logged in.</para>
        ///     <para>The player that has been logged in is passed to the callback.</para>
        /// </summary>
        public Action<ZapicPlayer> OnLogin { get; set; }

        /// <summary>
        ///     <para>Gets or sets the callback invoked after the player has been logged out.</para>
        ///     <para>The player that has been logged out is passed to the callback.</para>
        /// </summary>
        public Action<ZapicPlayer> OnLogout { get; set; }

        /// <summary>Invoked after the player has logged in.</summary>
        /// <param name="playerObject">The current player.</param>
        public void onLogin(AndroidJavaObject playerObject)
        {
            try
            {
                var handler = OnLogin;
                if (handler == null)
                {
                    return;
                }

                ZapicPlayer player;
                try
                {
                    player = ZapicAndroidUtilities.ConvertPlayer(playerObject);
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred converting the Java object to ZapicPlayer");
                    Debug.LogException(e);
                    return;
                }
                finally
                {
                    if (playerObject != null)
                    {
                        playerObject.Dispose();
                        playerObject = null;
                    }
                }

                try
                {
                    handler(player);
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred invoking the application callback");
                    Debug.LogException(e);
                }
            }
            finally
            {
                if (playerObject != null)
                {
                    playerObject.Dispose();
                }
            }
        }

        /// <summary>Invoked after the player has logged out.</summary>
        /// <param name="playerObject">The previous player.</param>
        public void onLogout(AndroidJavaObject playerObject)
        {
            try
            {
                var handler = OnLogout;
                if (handler == null)
                {
                    return;
                }

                ZapicPlayer player;
                try
                {
                    player = ZapicAndroidUtilities.ConvertPlayer(playerObject);
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred converting the Java object to ZapicPlayer");
                    Debug.LogException(e);
                    return;
                }
                finally
                {
                    if (playerObject != null)
                    {
                        playerObject.Dispose();
                        playerObject = null;
                    }
                }

                try
                {
                    handler(player);
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred invoking the application callback");
                    Debug.LogException(e);
                }
            }
            finally
            {
                if (playerObject != null)
                {
                    playerObject.Dispose();
                }
            }
        }
    }
}

#endif
