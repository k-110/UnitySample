using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class BarcodeHelper
{
    /// <summary>
    /// バーコードをデコード
    /// </summary>
    /// <param name="TexBc">画像</param>
    /// <returns>デコード結果</returns>
    public static string Read(WebCamTexture TexBc)
    {
        BarcodeReader BcReader = new BarcodeReader() {
            Options = new ZXing.Common.DecodingOptions()
            {
                TryHarder = true,
                PossibleFormats = new List<BarcodeFormat>() { BarcodeFormat.QR_CODE }
            }
        };
        Color32[] MyImage = TexBc.GetPixels32();
        ImageHelper.ColorToGray(ref MyImage);
        Result result = BcReader.Decode(MyImage, TexBc.width, TexBc.height);
        if (result != null)
        {
            return result.Text;
        }
        return null;
    }

    /// <summary>
    /// 2次元バーコード作成
    /// </summary>
    /// <param name="TextData">バーコードにする文字列</param>
    /// <param name="ImageWidth">画像の幅</param>
    /// <param name="ImageHeight">画像の高さ</param>
    /// <returns>次元バーコード</returns>
    public static Color32[] Write2D(string TextData, int ImageWidth, int ImageHeight)
    {
        BarcodeWriter BcWriter = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Width = ImageWidth,
                Height = ImageHeight
            }
        };
        return BcWriter.Write(TextData);
    }
}
