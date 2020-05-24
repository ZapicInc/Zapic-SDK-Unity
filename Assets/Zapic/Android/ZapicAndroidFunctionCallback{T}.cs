#if !UNITY_EDITOR && UNITY_ANDROID

using System;
using UnityEngine;

namespace ZapicSDK
{
    /// <summary>A delegate-based implementation of the <c>ZapicCallback</c> Java interface.</summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    internal sealed class ZapicAndroidFunctionCallback<T> : AndroidJavaProxy
        where T : class
    {
        /// <summary>The callback invoked with the result of the asynchronous operation.</summary>
        private readonly Action<T, ZapicException> callback;

        /// <summary>The callback invoked to convert the result of the asynchronous operation.</summary>
        private readonly Func<AndroidJavaObject, T> convertResult;

        /// <summary>Initializes a new instance of the <see cref="ZapicAndroidFunctionCallback{T}"/> class.</summary>
        /// <param name="callback">The callback invoked with the result of the asynchronous operation.</param>
        /// <param name="convertResult">
        ///     The callback invoked to convert the result of the asynchronous operation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="callback"/> or <paramref name="convertResult"/> are <c>null</c>.
        /// </exception>
        internal ZapicAndroidFunctionCallback(
            Action<T, ZapicException> callback,
            Func<AndroidJavaObject, T> convertResult)
            : base("com/zapic/sdk/android/ZapicCallback")
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            if (convertResult == null)
            {
                throw new ArgumentNullException("convertResult");
            }

            this.callback = callback;
            this.convertResult = convertResult;
        }

        /// <summary>Called when the asynchronous operation is complete.</summary>
        /// <param name="resultObject">The result of the asynchronous operation.</param>
        /// <param name="errorObject">
        ///     The error thrown by the asynchronous operation. This will be {@code null} if the asynchronous operation
        ///     completed normally.
        /// </param>
        public void onComplete(AndroidJavaObject resultObject, AndroidJavaObject errorObject)
        {
            try
            {
                ZapicException error;
                try
                {
                    error = ZapicAndroidUtilities.ConvertError(errorObject);
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred converting the Java object to ZapicException");
                    Debug.LogException(e);

                    error = e as ZapicException;
                    if (error == null)
                    {
                        error = new ZapicException(
                            ZapicErrorCode.INVALID_RESPONSE,
                            "An error occurred converting the Java object to ZapicException",
                            e);
                    }
                }
                finally
                {
                    if (errorObject != null)
                    {
                        errorObject.Dispose();
                        errorObject = null;
                    }
                }

                if (error != null)
                {
                    if (resultObject != null)
                    {
                        resultObject.Dispose();
                        resultObject = null;
                    }

                    try
                    {
                        callback(null, error);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Zapic: An error occurred invoking the application callback");
                        Debug.LogException(e);
                    }

                    return;
                }

                T result;
                try
                {
                    result = convertResult(resultObject);
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred converting the Java object to " + typeof(T).Name);
                    Debug.LogException(e);

                    error = e as ZapicException;
                    if (error == null)
                    {
                        error = new ZapicException(
                            ZapicErrorCode.INVALID_RESPONSE,
                            "An error occurred converting the Java object to " + typeof(T).Name,
                            e);
                    }

                    result = null;
                }
                finally
                {
                    if (resultObject != null)
                    {
                        resultObject.Dispose();
                        resultObject = null;
                    }
                }

                try
                {
                    if (error != null)
                    {
                        callback(null, error);
                    }
                    else
                    {
                        callback(result, null);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Zapic: An error occurred invoking the application callback");
                    Debug.LogException(e);
                }
            }
            finally
            {
                if (errorObject != null)
                {
                    errorObject.Dispose();
                }

                if (resultObject != null)
                {
                    resultObject.Dispose();
                }

                javaInterface.Dispose();
            }
        }
    }
}

#endif
