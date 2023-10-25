using FaceApi.iOS;
using Foundation;

namespace FaceSample.Platforms.iOS
{
    public class FaceSdkInit : RFSURLRequestInterceptingDelegate, IFaceSdkInit
    {
        public FaceSdkInit()
        {
        }

        public void InitFaceSdk()
        {
            RFSFaceSDK.Service.InitializeWithCompletion((bool success, NSError error) =>
            {
                if (success)
                {
                    Console.WriteLine("Init complete");
                }
                else
                {
                    Console.WriteLine("Init failed:");
                    Console.WriteLine(error);
                }
            });
        }
    }
}