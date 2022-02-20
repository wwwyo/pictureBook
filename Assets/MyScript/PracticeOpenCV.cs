using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp.Demo;
using OpenCvSharp;
using OpenCvSharp.Aruco;

public class PracticeOpenCV : MonoBehaviour
{
    public Texture2D texture;

    void Start()
    {
        // 画像読み込み
        //Mat mat = OpenCvSharp.Unity.TextureToMat(this.texture);

        //// 画像書き出し
        //Texture2D outTexture = new Texture2D(mat.Width, mat.Height, TextureFormat.ARGB32, false);
        //OpenCvSharp.Unity.MatToTexture(mat, outTexture);

        //// 表示
        //GetComponent<RawImage>().texture = outTexture;
    }

    public Texture2D convertGrayScale(Texture2D texture)
    {
        Mat srcMat = OpenCvSharp.Unity.TextureToMat(texture);
        Mat grayMat = new Mat();
        Cv2.CvtColor(srcMat, grayMat, ColorConversionCodes.RGBA2GRAY);
        return OpenCvSharp.Unity.MatToTexture(grayMat);
    }
}