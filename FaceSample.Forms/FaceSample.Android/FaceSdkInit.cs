using Com.Regula.Facesdk.Callback;
using Com.Regula.Facesdk.Exception;
using Com.Regula.Facesdk;
using FaceSample.Droid;

[assembly: Xamarin.Forms.Dependency(
          typeof(FaceSdkInit))]
namespace FaceSample.Droid
{
    public class FaceSdkInit : Java.Lang.Object, IFaceSdkInit, IInitCallback
    {
        public FaceSdkInit()
        {
        }

        public void InitFaceSdk()
        {
            FaceSDK.Instance().Init(Android.App.Application.Context, this);
        }

        public void OnInitCompleted(bool success, InitException error)
        {
            if (success)
                System.Console.WriteLine("Init complete");
            else
            {
                System.Console.WriteLine("Init failed:");
                System.Console.WriteLine(error.Message);
            }
        }
    }
}
