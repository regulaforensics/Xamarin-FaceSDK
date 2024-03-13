using Com.Regula.Facesdk.Callback;
using Com.Regula.Facesdk.Exception;
using Com.Regula.Facesdk;

namespace FaceSample.Platforms.Android
{
    public class FaceSdkInit : Java.Lang.Object, IFaceSdkInit, IFaceInitializationCompletion
    {
        public FaceSdkInit()
        {
        }

        public void InitFaceSdk()
        {
            FaceSDK.Instance().Initialize(Platform.AppContext, this);
        }

        public void OnInitCompleted(bool success, InitException error)
        {
            if (success)
            {
                Console.WriteLine("Init complete");
            }
            else
            {
                Console.WriteLine("Init failed:");
                Console.WriteLine(error.Message);
            }
        }
    }
}