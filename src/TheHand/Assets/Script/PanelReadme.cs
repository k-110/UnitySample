using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelReadme : MonoBehaviour
{
    /// <summary>
    /// �\��
    /// </summary>
    public void OnOpen()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// ��\��
    /// </summary>
    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
