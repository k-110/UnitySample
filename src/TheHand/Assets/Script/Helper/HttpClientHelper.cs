using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClientHelper
{
    private static readonly string ClassName = "HttpClientHelper";

    /// <summary>
    /// Wi-Fiが使用できる状態か？
    /// ※キャリアでの接続は使用不可扱いとする
    /// </summary>
    /// <returns>判定結果</returns>
    public static bool IsWifiEnable()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                return true;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
            default:
                break;
        }
        return false;
    }

    /// <summary>
    /// Get
    /// </summary>
    /// <param name="Url">URL</param>
    /// <param name="Complete">終了時に呼ばれる処理</param>
    /// <returns></returns>
    public static IEnumerator Get(string Url, System.Action<string> Complete)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(Url))
        {
            Debug.Log(string.Format("{0}.Get:{1}", ClassName, Url));
            yield return request.SendWebRequest();
            ResultCheck(request, Complete);
        }
    }

    /// <summary>
    /// Post
    /// </summary>
    /// <param name="Url">URL</param>
    /// <param name="PostData">送信するデータ</param>
    /// <param name="Complete">終了時に呼ばれる処理</param>
    /// <returns></returns>
    public static IEnumerator Post(string Url, WWWForm PostData, System.Action<string> Complete)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(Url, PostData))
        {
            //request.SetRequestHeader("Content-Type", "application/json");
            Debug.Log(string.Format("{0}.Post:{1}", ClassName, Url));
            yield return request.SendWebRequest();
            ResultCheck(request, Complete);
        }
    }

    /// <summary>
    /// リクエスト結果の確認
    /// </summary>
    /// <param name="Request">結果</param>
    /// <param name="Complete">終了時に呼ばれる処理</param>
    private static void ResultCheck(UnityWebRequest Request, System.Action<string> Complete)
    {
        switch (Request.result)
        {
            case UnityWebRequest.Result.InProgress:
                //通信中
                break;
            case UnityWebRequest.Result.Success:
                //通信成功
                Debug.Log(string.Format("{0}.{1}:Success\tResult:Success", ClassName, Request.method));
                Complete(Request.downloadHandler.text);
                break;
            default:
                //通信失敗
                Debug.Log(string.Format("{0}.{1}:Errot\tResult:{2}", ClassName, Request.method, Request.result.ToString()));
                Complete(null);
                break;
        }
    }
}
