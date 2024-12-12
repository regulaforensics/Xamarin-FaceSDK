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
            RFSInitializationConfiguration config = null;
            try
            {
                var licenseData = NSData.FromFile(NSBundle.MainBundle.PathForResource("regula.license", null));
                config = RFSInitializationConfiguration.ConfigurationWithBuilder((RFSInitializationConfigurationBuilder builder) =>
                {
                    builder.LicenseData = licenseData;
                });
            }
            catch (Exception) { }
            
            RFSFaceSDK.Service.InitializeWithConfiguration(config, (bool success, NSError error) =>
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