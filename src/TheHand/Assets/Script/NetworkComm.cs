using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkComm : MonoBehaviour
{
    private readonly string MyUrl = "http://127.0.0.1:8000/memo/";
    private string MyCsrf = "";

    private InputField MyInput;
    private Text MyText;
    private Button MyGet;
    private Button MyPost;

    // Start is called before the first frame update
    void Start()
    {
        MyInput = transform.Find("InputField").GetComponent<InputField>();
        MyText = transform.Find("Text").GetComponent<Text>();
        MyGet = transform.Find("ButtonGet").GetComponent<Button>();
        MyPost = transform.Find("ButtonPost").GetComponent<Button>();
    }

    /// <summary>
    /// イベント：Getボタン
    /// </summary>
    public void OnGet()
    {
        if (HttpClientHelper.IsWifiEnable())
        {
            MyInput.enabled = false;
            MyGet.enabled = false;
            MyPost.enabled = false;
            StartCoroutine(HttpClientHelper.Get(MyUrl, (text) => {
                MyText.text = (string.IsNullOrEmpty(text)) ? "Post Error" : text;
                Match match = Regex.Match(MyText.text, ".*csrfmiddlewaretoken.*");
                if (match.Success)
                {
                    string[] value = Regex.Match(match.Value, "value=\".*\"").Value.Split('"');                  
                    MyCsrf = value[1];
                }
                MyInput.enabled = true;
                MyGet.enabled = true;
                MyPost.enabled = true;
            }));
        }
    }

    /// <summary>
    /// イベント：Postボタン
    /// </summary>
    public void OnPost()
    {
        if (HttpClientHelper.IsWifiEnable())
        {
            MyInput.enabled = false;
            MyGet.enabled = false;
            MyPost.enabled = false;
            WWWForm PostData = new WWWForm();
            PostData.AddField("csrfmiddlewaretoken", MyCsrf);
            PostData.AddField("memo", MyInput.text);
            StartCoroutine(HttpClientHelper.Post(MyUrl, PostData, (text) => {
                MyText.text = (string.IsNullOrEmpty(text)) ? "Post Error" : text;
                Match match = Regex.Match(MyText.text, ".*csrfmiddlewaretoken.*");
                if (match.Success)
                {
                    string[] value = Regex.Match(match.Value, "value=\".*\"").Value.Split('"');
                    MyCsrf = value[1];
                }
                MyInput.enabled = true;
                MyGet.enabled = true;
                MyPost.enabled = true;
            }));
        }
    }
}
