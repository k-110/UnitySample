using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamera : MonoBehaviour
{
    private WebCamTexture MyWebCam = null;
    private Texture2D MyTexture = null;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] MyDevices = WebCamTexture.devices;
        if (0 < MyDevices.Length)
        {
            MyWebCam = new WebCamTexture(MyDevices[0].name, 640, 480, 30);
            MyWebCam.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MyTexture == null)
        {
            //WebCamTexture‚Ìî•ñ‚ª³‚µ‚­Žæ“¾‚Å‚«‚é‚æ‚¤‚É‚È‚é‚Ü‚Å‚ÉŽžŠÔ‚ª‚©‚©‚é‚Á‚Û‚¢
            if ((MyWebCam.width > 16) && (MyWebCam.height > 16))
            {
                MyTexture = new Texture2D(MyWebCam.width, MyWebCam.height, TextureFormat.RGBA32, false);
                GetComponent<RawImage>().texture = MyTexture;
            }
        }
        else if (transform.Find("ToggleBarcode").GetComponent<Toggle>().isOn)
        {
            string BcText = BarcodeHelper.Read(MyWebCam);
            if (BcText != null)
            {
                transform.Find("Text").GetComponent<Text>().text = BcText;
            }
            Color32[] MyImage = MyWebCam.GetPixels32();
            Color32[] MyQr = BarcodeHelper.Write2D("abcdefghijklmnopqrstuvwxyz", 256, 256);
            for (int w = 0; w < 256; w++)
            {
                for (int h = 0; h < 256; h++)
                {
                    MyImage[w + (h * MyTexture.width)] = MyQr[w + (h * 256)];
                }

            }
            MyTexture.SetPixels32(MyImage);
            MyTexture.Apply();
        }
        else
        {
            Color32[] MyImage = MyWebCam.GetPixels32();
            ImageHelper.ColorToGray(ref MyImage);
            ImageHelper.GrayToAveraging(ref MyImage, MyTexture.width, MyTexture.height);
            ImageHelper.GrayToEdge(ref MyImage, MyTexture.width, MyTexture.height);
            MyTexture.SetPixels32(MyImage);
            MyTexture.Apply();
        }
    }
}
