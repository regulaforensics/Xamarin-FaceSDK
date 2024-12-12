using Com.Regula.Facesdk.Callback;
using Com.Regula.Facesdk.Exception;
using Com.Regula.Facesdk;
using Com.Regula.Facesdk.Configuration;

namespace FaceSample.Platforms.Android
{
    public class FaceSdkInit : Java.Lang.Object, IFaceSdkInit, IFaceInitializationCompletion
    {
        public FaceSdkInit()
        {
        }
        public void InitFaceSdk()
        {
            InitializationConfiguration config = null;
            try
            {
                var bytes = default(byte[]);
                using (var streamReader = new StreamReader(Platform.AppContext.Assets.Open("regula.license")))
                {
                    using var memstream = new MemoryStream();
                    streamReader.BaseStream.CopyTo(memstream);
                    bytes = memstream.ToArray();
                }
                config = new InitializationConfiguration.Builder(bytes).Build();
            }
            catch (Exception) { }

            if (config != null)
                FaceSDK.Instance().Initialize(Platform.AppContext, config, this);
            else
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