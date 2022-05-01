using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClientHelper
{
    private static readonly string ClassName = "HttpClientHelper";

    /// <summary>
    /// Wi-Fi���g�p�ł����Ԃ��H
    /// ���L�����A�ł̐ڑ��͎g�p�s�����Ƃ���
    /// </summary>
    /// <returns>���茋��</returns>
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
    /// <param name="Complete">�I�����ɌĂ΂�鏈��</param>
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
    /// <param name="PostData">���M����f�[�^</param>
    /// <param name="Complete">�I�����ɌĂ΂�鏈��</param>
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
    /// ���N�G�X�g���ʂ̊m�F
    /// </summary>
    /// <param name="Request">����</param>
    /// <param name="Complete">�I�����ɌĂ΂�鏈��</param>
    private static void ResultCheck(UnityWebRequest Request, System.Action<string> Complete)
    {
        switch (Request.result)
        {
            case UnityWebRequest.Result.InProgress:
                //�ʐM��
                break;
            case UnityWebRequest.Result.Success:
                //�ʐM����
                Debug.Log(string.Format("{0}.{1}:Success\tResult:Success", ClassName, Request.method));
                Complete(Request.downloadHandler.text);
                break;
            default:
                //�ʐM���s
                Debug.Log(string.Format("{0}.{1}:Errot\tResult:{2}", ClassName, Request.method, Request.result.ToString()));
                Complete(null);
                break;
        }
    }
}
