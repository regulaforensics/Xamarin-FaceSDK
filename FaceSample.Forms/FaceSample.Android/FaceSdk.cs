using System;
using System.Collections.Generic;
using System.IO;
using Android.Graphics;
using Com.Regula.Facesdk;
using Com.Regula.Facesdk.Callback;
using Com.Regula.Facesdk.Model.Results;
using Com.Regula.Facesdk.Request;
using FaceSample.Droid;
using Xamarin.Forms;

[assembly: Dependency(
          typeof(FaceSdk))]
namespace FaceSample.Droid
{
    public class MatchFacesEvent : EventArgs, IMatchFacesEvent
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public double Similarity { get; set; }
    }

    public class LivenessEvent : EventArgs, ILivenessEvent
    {
        public LivenessStatus LivenessStatus { get; set; }
        public byte[] LivenessImage { get; set; }
    }

    public class FaceCaptureImageEvent : EventArgs, IFaceCaptureImageEvent
    {
        public byte[] Image { get; set; }
    }

    public class FaceSdk: Java.Lang.Object, IFaceSdk, ILivenessCallback, IMatchFaceCallback, IFaceCaptureCallback
    {
        private int FirstImageIndex = 1;
        private int SecondImageIndex = 2;
        public FaceSdk()
        {
        }

        public event EventHandler<IMatchFacesEvent> MatchFacesResultsObtained;
        public event EventHandler<ILivenessEvent> LivenessResultsObtained;
        public event EventHandler<IFaceCaptureImageEvent> FaceCaptureResultsObtained;

        public void MatchFaces(byte[] firstStream, byte[] secondStream)
        {
            Bitmap firstBitmap = BitmapFactory.DecodeByteArray(firstStream, 0, firstStream.Length);
            Bitmap secondBitmap = BitmapFactory.DecodeByteArray(secondStream, 0, secondStream.Length);
            IList<Com.Regula.Facesdk.Model.Image> listImages = new List<Com.Regula.Facesdk.Model.Image>{
                new Com.Regula.Facesdk.Model.Image(FirstImageIndex, firstBitmap),
                new Com.Regula.Facesdk.Model.Image(SecondImageIndex, secondBitmap)
            };
            var matchFacesRequest = new MatchFacesRequest(listImages);
            FaceSDK.Instance().MatchFaces(matchFacesRequest, this);
        }

        public void OnFaceMatched(MatchFacesResponse matchFacesResponse)
        {
            MatchFacesEvent matchFacesEvent = new MatchFacesEvent();
            if (matchFacesResponse.Exception != null)
            {
                matchFacesEvent.IsSuccess = false;
                matchFacesEvent.Error = matchFacesResponse.Exception.Message;
            } else
            {
                if (matchFacesResponse.MatchedFaces.Count > 0)
                {
                    double similarity = matchFacesResponse.MatchedFaces[0].Similarity;
                    matchFacesEvent.Similarity = similarity;
                    matchFacesEvent.IsSuccess = true;
                } else
                {
                    matchFacesEvent.IsSuccess = false;
                    matchFacesEvent.Error = "Faces are not equals";
                }
                        
            }

            MatchFacesResultsObtained(this, matchFacesEvent);
        }

        public void FaceCaptureImage()
        {
            FaceSDK.Instance().PresentFaceCaptureActivity(Forms.Context, this);
        }

        public void StartLiveness()
        {
            FaceSDK.Instance().StartLiveness(Forms.Context, this);
        }

        public void OnLivenessCompete(LivenessResponse response)
        {
            LivenessEvent livenessEvent = new LivenessEvent
            {
                LivenessStatus = response.Liveness == Com.Regula.Facesdk.Enums.LivenessStatus.Passed ? LivenessStatus.PASSED : LivenessStatus.UNKNOWN
            };
            if (response.Exception == null)
                Console.WriteLine("No exception");

            if (response.Bitmap != null)
            {
                livenessEvent.LivenessImage = ConvertBitmap(response.Bitmap);
            }

            LivenessResultsObtained(this, livenessEvent);
        }

        public void OnFaceCaptured(FaceCaptureResponse captureResponse)
        {
            if (captureResponse.Image == null || captureResponse.Image.Bitmap == null)
                return;

            FaceCaptureImageEvent faceCaptureImageEvent = new FaceCaptureImageEvent();
            faceCaptureImageEvent.Image = ConvertBitmap(captureResponse.Image.Bitmap);
            FaceCaptureResultsObtained(this, faceCaptureImageEvent);
        }

        private byte[] ConvertBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            byte[] bitmapData = null;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }
            return bitmapData;
        }
    }
}
