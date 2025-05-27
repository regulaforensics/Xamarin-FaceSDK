using FaceSample.Platforms.iOS;
using Foundation;

namespace FaceSample;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
        protected override MauiApp CreateMauiApp()
        {
                // WARNING: necessary!
                // These 2 lines prevent MAUI from shrinking assemblies
                new FaceCoreSDK.iOS.FaceSDK();
                new RegulaCommon.iOS.RGLCCamera();

                DependencyService.Register<IFaceSdkInit, FaceSdkInit>();
                DependencyService.Register<IFaceSdk, FaceSdk>();
                DependencyService.Register<IPhotoPickerService, PhotoPickerService>();

                return MauiProgram.CreateMauiApp();
        }
}