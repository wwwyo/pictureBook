using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

public class ImgTracker: MonoBehaviour
{
    [SerializeField] private ARCameraManager _cameraManager;
    [SerializeField] private RawImage _showRawImage;
    private Texture2D _realDisplayTexture;
    public Texture2D CameraTexture => _realDisplayTexture = _realDisplayTexture == null ? null : _realDisplayTexture;
    PracticeOpenCV _OpenCv;

    private void OnEnable()
    {
        _cameraManager.frameReceived += OnARCameraFrameReceived;
        _OpenCv = GetComponent<PracticeOpenCV>();
    }

    private void OnDisable()
    {
        _cameraManager.frameReceived -= OnARCameraFrameReceived;
    }

    unsafe void OnARCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        // get native camera image
        if (!_cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image)) return;

        var format = TextureFormat.RGBA32;
        // _realDisplayTexture ini
        if ( _realDisplayTexture == null || _realDisplayTexture.width != image.width || _realDisplayTexture.height != image.height)
        {
            int width = image.width;
            int height = image.height;
            _realDisplayTexture = new Texture2D(width, height, format, false);
	    }


        XRCpuImage.Transformation imageTransformatation = (Input.deviceOrientation == DeviceOrientation.LandscapeRight) ? XRCpuImage.Transformation.MirrorY : XRCpuImage.Transformation.MirrorX;
        var conversionParams = new XRCpuImage.ConversionParams(image, format, imageTransformatation);
        var rawTextureData = _realDisplayTexture.GetRawTextureData<byte>();
        try
    	{
            image.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
	    }
        finally
        {
            image.Dispose();
	    }
        _realDisplayTexture.Apply();

        if (_showRawImage != null)
        {
            Destroy(_showRawImage.texture);
            _showRawImage.texture = _OpenCv.convertGrayScale(_realDisplayTexture);
        }
        //var conversionParams = new XRCpuImage.ConversionParams
        //{
        //    inputRect = new RectInt(0, 0, image.width, image.height),
        //    outputDimensions = new Vector2Int(image.width / 2, image.height / 2),
        //    outputFormat = TextureFormat.RGBA32,
        //    transformation = XRCpuImage.Transformation.MirrorY
        //};

        //int size = image.GetConvertedDataSize(conversionParams);
        //var buffer = new NativeArray<byte>(size, Allocator.Temp);
        //image.Convert(conversionParams, new IntPtr(buffer.GetUnsafePtr()), buffer.Length);
        //image.Dispose();

        //if (_realDisplayTexture == null)
        //{
        //    var x = conversionParams.outputDimensions.x;
        //    var y = conversionParams.outputDimensions.y;
        //    _realDisplayTexture = new Texture2D(x, y, conversionParams.outputFormat, false);
        //}

        //_realDisplayTexture.LoadRawTextureData(buffer);
        //_realDisplayTexture.Apply();

        //buffer.Dispose();
    }
}
