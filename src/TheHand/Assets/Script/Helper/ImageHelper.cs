using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageHelper
{
    /// <summary>
    /// RGBをグレースケール化
    /// ※加重平均で算出
    /// ※InOutImageの値を書き換える
    /// </summary>
    /// <param name="InOutImage">ピクセルデータ</param>
    public static void ColorToGray(ref Color32[] InOutImage)
    {
        for (int i = 0; i < InOutImage.Length; i++)
        {
            byte gray = System.Convert.ToByte(InOutImage[i].r * 0.2126 + InOutImage[i].g * 0.7152 + InOutImage[i].b * 0.0722);
            InOutImage[i].r = gray;
            InOutImage[i].g = gray;
            InOutImage[i].b = gray;
        }
    }

    /// <summary>
    /// ピクセルデータの差分を算出
    /// ※ピクセルデータはグレースケールとする
    /// ※InOutImageの値を書き換える
    /// </summary>
    /// <param name="InOutImage">ピクセルデータ</param>
    /// <param name="InImage">ピクセルデータ</param>
    public static void CalcDifference(ref Color32[] InOutImage, Color32[] InImage)
    {
        for (int i = 0; i < InOutImage.Length; i++)
        {
            int Diff = System.Math.Abs(InOutImage[i].r - InImage[i].r);
            InOutImage[i].r = System.Convert.ToByte(Diff);
            InOutImage[i].g = InOutImage[i].r;
            InOutImage[i].b = InOutImage[i].r;
        }
    }

    /// <summary>
    /// 輝度の自動調整
    /// ※ピクセルデータはグレースケールとする
    /// ※InOutImageの値を書き換える
    /// </summary>
    /// <param name="InOutImage">ピクセルデータ</param>
    /// <param name="NewBrightness">調整後の輝度</param>
    /// <param name="NewStdDev">調整後の標準偏差</param>
    public static void BrightnessAdjust(ref Color32[] InOutImage, int NewBrightness, int NewStdDev)
    {
        int[] MeanStdDev = GetMeanStdDev(InOutImage);
        int Mean = MeanStdDev[0];
        int StdDev = MeanStdDev[1];
        for (int i = 0; i < InOutImage.Length; i++)
        {
            //誤差で画像がつぶれてしまうため1000倍してから算出
            double Pix = ((InOutImage[i].r * 1000) - Mean) / StdDev;
            Pix = (Pix * NewStdDev) + NewBrightness;
            InOutImage[i].r = System.Convert.ToByte(Pix/1000);
            InOutImage[i].g = InOutImage[i].r;
            InOutImage[i].b = InOutImage[i].r;
        }
    }

    /// <summary>
    /// ピクセルデータをぼかす
    /// ※平均化(3×3)
    /// ※ピクセルデータはグレースケールとする
    /// ※InOutImageの値を書き換える
    /// </summary>
    /// <param name="InOutImage">ピクセルデータ</param>
    /// <param name="Width">画像の幅</param>
    /// <param name="Height">画像の高さ</param>
    public static void GrayToAveraging(ref Color32[] InOutImage, int Width, int Height)
    {
        if (!InOutImage.Length.Equals(Width * Height))
        {
            Debug.Log("WebCamera.GrayToAveraging:Pixel count and size mismatch.");
            return;
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int index = x + (y * Width);
                int total = InOutImage[index].g;
                int count = 1;
                if (0 < y)
                {
                    total += InOutImage[index - Width].g;
                    count++;
                }
                if (y < Height - 1)
                {
                    total += InOutImage[index + Width].g;
                    count++;
                }
                if (0 < x)
                {
                    total += InOutImage[index - 1].g;
                    count++;
                    if (0 < y)
                    {
                        total += InOutImage[index - 1 - Width].g;
                        count++;
                    }
                    if (y < Height - 1)
                    {
                        total += InOutImage[index - 1 + Width].g;
                        count++;
                    }
                }
                if (x < Width - 1)
                {
                    total += InOutImage[index + 1].g;
                    count++;
                    if (0 < y)
                    {
                        total += InOutImage[index + 1 - Width].g;
                        count++;
                    }
                    if (y < Height - 1)
                    {
                        total += InOutImage[index + 1 + Width].g;
                        count++;
                    }
                }
                InOutImage[index].r = System.Convert.ToByte(total / count);
            }
        }
        for (int i = 0; i < InOutImage.Length; i++)
        {
            InOutImage[i].g = InOutImage[i].r;
            InOutImage[i].b = InOutImage[i].r;
        }
    }

    /// <summary>
    /// ピクセルデータのエッジ検出
    /// ※ソーベルフィルタ
    /// ※ぼかしは事前におこなって行っておくこと
    /// ※ピクセルデータはグレースケールとする
    /// ※InOutImageの値を書き換える
    /// </summary>
    /// <param name="InOutImage">ピクセルデータ</param>
    /// <param name="Width">画像の幅</param>
    /// <param name="Height">画像の高さ</param>
    public static void GrayToEdge(ref Color32[] InOutImage, int Width, int Height)
    {
        if (!InOutImage.Length.Equals(Width * Height))
        {
            Debug.Log("WebCamera.GrayToEdge:Pixel count and size mismatch.");
            return;
        }

        List<double[]> FilterH = new List<double[]>{
            new double[]{ 1, 0,-1 },
            new double[]{ 2, 0,-2 },
            new double[]{ 1, 0,-1 },
        };
        List<double[]> FilterV = new List<double[]>{
            new double[]{ 1, 2, 1 },
            new double[]{ 0, 0, 0 },
            new double[]{-1,-2,-1 },
        };

        for (int w = 1; w < Width - 1; w++)
        {
            for (int h = 1; h < Height - 1; h++)
            {
                double SumH = 0;
                double SumV = 0;
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        SumH += InOutImage[(w + x) + (h + y) * Width].g * FilterH[x + 1][y + 1];
                        SumV += InOutImage[(w + x) + (h + y) * Width].g * FilterV[x + 1][y + 1];
                    }
                }
                double Sum = System.Math.Sqrt(SumH * SumH + SumV * SumV);
                //if (Sum < 0) { Sum = 0; }
                InOutImage[w + h * Width].r = System.Convert.ToByte(Sum);
            }
        }
        for (int i = 0; i < InOutImage.Length; i++)
        {
            InOutImage[i].g = InOutImage[i].r;
            InOutImage[i].b = InOutImage[i].r;
        }
    }

    /// <summary>
    /// ピクセルデータの2値化
    /// ※大津の2値化を参考
    /// ※ピクセルデータはグレースケールとする
    /// ※InOutImageの値を書き換える
    /// </summary>
    /// <param name="InOutImage">ピクセルデータ</param>
    public static void GrayToBinarization(ref Color32[] InOutImage)
    {
        //最小値と最大値を見つける
        int[] MinMax = GetMinMax(InOutImage);
        int Min = MinMax[0];
        int Max = MinMax[1];

        //最小値～最大値の範囲で閾値を見つける
        //GetSb2の実装都合で初期のMinSbが0になるためMaxを-1する
        if (Min < Max) { Max--; }
        double MinSb = GetSb2(InOutImage, Min);
        double MaxSb = GetSb2(InOutImage, Max);
        int Th = (Min + Max) / 2;
        while (1 < Max - Min)
        {
            Th = (Min + Max) / 2;
            double Sb = GetSb2(InOutImage, Th);
            if(MinSb < MaxSb)
            {
                Min = Th;
                MinSb = Sb;
            }
            else
            {
                Max = Th;
                MaxSb = Sb;
            }
        }

        //閾値で2値化
        GrayToBinarization(ref InOutImage, Th);
    }

    /// <summary>
    /// ピクセルデータの2値化
    /// ※閾値で2値化
    /// ※ピクセルデータはグレースケールとする
    /// ※InOutImageの値を書き換える
    /// </summary>
    /// <param name="InOutImage">ピクセルデータ</param>
    /// <param name="Th">閾値</param>
    public static void GrayToBinarization(ref Color32[] InOutImage, int Th)
    {
        for (int i = 0; i < InOutImage.Length; i++)
        {
            if (InOutImage[i].r <= Th)
            {
                InOutImage[i].r = 0;
            }
            else
            {
                InOutImage[i].r = 255;
            }
            InOutImage[i].g = InOutImage[i].r;
            InOutImage[i].b = InOutImage[i].r;
        }
    }

    /// <summary>
    /// 平均と標準偏差を見つける
    /// ※ピクセルデータはグレースケールとする
    /// </summary>
    /// <param name="InImage">ピクセルデータ</param>
    /// <returns>平均と標準偏差</returns>
    public static int[] GetMeanStdDev(Color32[] InImage)
    {
        double Sum = 0;
        double SumSq = 0;
        foreach (Color32 Pix in InImage)
        {
            Sum += Pix.r;
            SumSq += Pix.r * Pix.r;
        }
        double Mean = Sum / InImage.Length;
        double StdDev = System.Math.Sqrt((SumSq / InImage.Length) - (Mean * Mean));
        int[] MeanStdDev = new int[2] { System.Convert.ToInt32(Mean), System.Convert.ToInt32(StdDev) };
        return MeanStdDev;
    }

    /// <summary>
    /// 最小値と最大値を見つける
    /// ※ピクセルデータはグレースケールとする
    /// </summary>
    /// <param name="InImage">ピクセルデータ</param>
    /// <returns>最小値と最大値</returns>
    public static int[] GetMinMax(Color32[] InImage)
    {
        int Min = 255;
        int Max = 0;
        foreach (Color32 Pix in InImage)
        {
            if (Pix.r < Min)
            {
                Min = Pix.r;
            }
            if (Max < Pix.r)
            {
                Max = Pix.r;
            }
        }
        int[] MinMax = new int[2] { Min, Max };
        return MinMax;
    }

    /// <summary>
    /// 分散を計算
    /// ※大津の2値化用
    /// ※ピクセルデータはグレースケールとする
    /// </summary>
    /// <param name="InImage">ピクセルデータ</param>
    /// <param name="ThreshHold">閾値</param>
    /// <returns>Sb^2</returns>
    private static double GetSb2(Color32[] InImage, int ThreshHold)
    {
        //R0 = クラス0に含まれる画素数 / 全体の画素数
        //R1 = クラス1に含まれる画素数 / 全体の画素数
        //M0 = クラス0のグレースケールの平均値
        //M1 = クラス1のグレースケールの平均値
        //において
        //Sb^2 = R0 × R1 × (M0−M1)^2
        //が最大になるtを閾値を用いて2値化
        int C0 = 0;
        int C1 = 0;
        int S0 = 0;
        int S1 = 0;
        foreach (Color32 Pix in InImage)
        {
            if (Pix.r <= ThreshHold)
            {
                C0 += 1;
                S0 += Pix.r;
            }
            else
            {
                C1 += 1;
                S1 += Pix.r;
            }
        }
        if (C0.Equals(0) || C1.Equals(0))
        {
            return 0;
        }
        double R0 = System.Convert.ToDouble(C0) / InImage.Length;
        double R1 = System.Convert.ToDouble(C1) / InImage.Length;
        double M0 = S0 / C0;
        double M1 = S1 / C1;
        double Sb = R0 * R1 * System.Math.Pow(M0 - M1, 2);
        return Sb;
    }
}
