using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsbMicrophone : MonoBehaviour
{
    private const int WaveSeconds = 1;
    private const int WaveFrequency = 44100;

    private AudioSource MyAudio = null;
    private AudioClip MyClip = null;
    private string MyDeviceName;
    float[] MyWave = new float[WaveSeconds * WaveFrequency];
    private int LastPos;

    // Start is called before the first frame update
    void Start()
    {
        string[] MyDevices = Microphone.devices;
        if (0 < MyDevices.Length)
        {
            MyAudio = GetComponent<AudioSource>();
            MyDeviceName = MyDevices[0];
            MyAudio.clip = Microphone.Start(MyDeviceName, true, WaveSeconds, WaveFrequency);
            while (Microphone.GetPosition(MyDeviceName) < 0) { }
            MyClip = AudioClip.Create("MyClip", MyWave.Length, MyAudio.clip.channels, MyAudio.clip.frequency, false, false);
            MyAudio.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(MyAudio != null)
        {
            int NowPos = Microphone.GetPosition(MyDeviceName);
            if (LastPos != NowPos)
            {
                if (IsRecording())
                {
                    MyAudio.clip.GetData(MyWave, 0);
                    MyClip.SetData(MyWave, 0);
                }
                LastPos = NowPos;
            }
        }
    }

    /// <summary>
    /// ò^âÊíÜÇ©ÅH
    /// </summary>
    /// <returns></returns>
    private bool IsRecording()
    {
        return transform.Find("PanelTest").Find("ToggleRecord").GetComponent<Toggle>().isOn;
    }

    /// <summary>
    /// âπÇçƒê∂ÇÈ
    /// </summary>
    public void PlayAudio()
    {
        if ((MyAudio != null) && (MyClip != null))
        {
            MyAudio.PlayOneShot(MyClip);
        }
    }
}
