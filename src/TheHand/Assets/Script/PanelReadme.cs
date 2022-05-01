using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelReadme : MonoBehaviour
{
    /// <summary>
    /// 表示
    /// </summary>
    public void OnOpen()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 非表示
    /// </summary>
    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
