#if !UNITY_EDITOR && UNITY_ANDROID && (NET_4_6 || NET_STANDARD_2_0)

using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ZapicSDK
{
    /// <summary>A <see cref="Task"/>-based implementation of the <c>ZapicCallback</c> Java interface.</summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    internal sealed class ZapicAndroidTaskCallback<T> : AndroidJavaProxy
        where T : class
    {
        /// <summary>The callback invoked to convert the result of the asynchronous operation.</summary>
        private readonly Func<AndroidJavaObject, T> convertResult;

        /// <summary>The task completion source to receive the result of the asynchronous operation.</summary>
        private readonly TaskCompletionSource<T> task;

        /// <summary>Initializes a new instance of the <see cref="ZapicAndroidTaskCallback{T}"/> class.</summary>
        /// <param name="task">
        ///     The task completion source to receive the result of the asynchronous operation.
        /// </param>
        /// <param name="convertResult">
        ///     The callback invoked to convert the result of the asynchronous operation.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="task"/> or <paramref name="convertResult"/> are <c>null</c>.
        /// </exception>
        internal ZapicAndroidTaskCallback(TaskCompletionSource<T> task, Func<AndroidJavaObject, T> convertResult)
            : base("com/zapic/sdk/android/ZapicCallback")
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            if (convertResult == null)
            {
                throw new ArgumentNullException("convertResult");
            }

            this.convertResult = convertResult;
            this.task = task;
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

                    task.TrySetException(error);
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

                if (error != null)
                {
                    task.TrySetException(error);
                }
                else
                {
                    task.TrySetResult(result);
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
