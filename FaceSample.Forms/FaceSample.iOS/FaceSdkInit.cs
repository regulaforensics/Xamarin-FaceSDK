using FaceApi.iOS;
using FaceSample.iOS;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(FaceSdkInit))]
namespace FaceSample.iOS
{
    public class FaceSdkInit : IFaceSdkInit
    {
        public FaceSdkInit()
        {
        }

        public void InitFaceSdk()
        {
            RFSFaceSDK.Service.InitializeWithCompletion((bool success, NSError error) =>
            {
                if (success) {
                    System.Console.WriteLine("Init complete");
                    RFSFaceSDK.Service.ServiceURL = "https://test-faceapi.regulaforensics.com";
                } else {
                    System.Console.WriteLine("Init failed:");
                    System.Console.WriteLine(error);
                }
            });
        }
    }
}