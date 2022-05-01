using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class BarcodeHelper
{
    /// <summary>
    /// �o�[�R�[�h���f�R�[�h
    /// </summary>
    /// <param name="TexBc">�摜</param>
    /// <returns>�f�R�[�h����</returns>
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
    /// 2�����o�[�R�[�h�쐬
    /// </summary>
    /// <param name="TextData">�o�[�R�[�h�ɂ��镶����</param>
    /// <param name="ImageWidth">�摜�̕�</param>
    /// <param name="ImageHeight">�摜�̍���</param>
    /// <returns>�����o�[�R�[�h</returns>
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
