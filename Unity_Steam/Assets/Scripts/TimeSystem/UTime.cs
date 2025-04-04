using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UTime : MonoBehaviour
{
    private const string URL_TIME_SEVER = "https://script.google.com/macros/s/AKfycbyal_mx91_jytjMzr_ykoP3NfZXBVMNRNXCX7qmt0QpTj6mAHg/exec";
    
    /// <summary>
    /// Get time from server asynchronously with callback (bool success, string error, DateTime time)
    /// </summary>
    public void GetUtcTimeAsync(Action<bool, string, DateTime> callback)
    {
        StartCoroutine(this.coDownload(request =>
        {
    #if UNITY_2020_1_OR_NEWER
            if(request.result == UnityWebRequest.Result.Success)
    #else
            if(!request.isNetworkError && !request.isHttpError)
    #endif
            {
                try
                {
                    string strTime = request.downloadHandler.text;
                    DateTime time = DateTime.Parse(strTime).ToUniversalTime();
                    callback?.Invoke(true, null, time);
                }
                catch(Exception e)
                {
                    callback?.Invoke(false, e.Message, DateTime.MinValue);
                }
            }
            else
            {
                callback?.Invoke(false, request.error, DateTime.MinValue);
            }
        }));
    }

    /// <summary>
    /// Check network connection asynchronously with callback (bool success)
    /// </summary>
    public void HasConnection(Action<bool> callback)
    {
        StartCoroutine(this.coDownload(request =>
        {
    #if UNITY_2020_1_OR_NEWER
            bool isSuccess = request.result == UnityWebRequest.Result.Success;
    #else
            bool isSuccess = !request.isNetworkError && !request.isHttpError;
    #endif
            callback?.Invoke(isSuccess);
        }));
    }

    /// <summary>
    /// General purpose web request
    /// </summary>
    private IEnumerator coDownload(Action<UnityWebRequest> callback)
    {
        using(UnityWebRequest request = UnityWebRequest.Get(URL_TIME_SEVER))
        {
            yield return request.SendWebRequest();
            callback?.Invoke(request);
        }
    }
}