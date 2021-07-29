using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FaceSample
{
    public partial class MainPage : ContentPage
    {
        readonly IFaceSdk FaceSdk;

        private byte[] FirstImageData, SecondImageData;

        private Boolean IsEnableButtons
        {
            set
            {
                MatchButton.IsEnabled = value;
                LivenessButton.IsEnabled = value;
                ClearButton.IsEnabled = value;
            }
        }

        public MainPage()
        {
            InitializeComponent();

            FaceSdk = DependencyService.Get<IFaceSdk>();
            FaceSdk.MatchFacesResultsObtained += MatchFacesEventHandler;
            FaceSdk.LivenessResultsObtained += LivenessEventHandler;
            FaceSdk.FaceCaptureResultsObtained += FaceCaptureEventHander;
        }

        private void LivenessEventHandler(object sender, ILivenessEvent e)
        {
            IsEnableButtons = true;
            LivenessLabel.Text = String.Format("Liveness: {0}", e.LivenessStatus == LivenessStatus.PASSED ? "Passed" : "Unknown");
            if (e.LivenessImage != null)
            {
                FirstImageData = e.LivenessImage;
                FirstImage.Source = ImageSource.FromStream(() => new MemoryStream(FirstImageData));
            }
        }

        private void MatchFacesEventHandler(object sender, IMatchFacesEvent e)
        {
            IsEnableButtons = true;
            if (e.IsSuccess)
            {
                SimilarityLabel.Text = String.Format("Similarity: {0:F2}%", e.Similarity * 100);
            }
            else
            {
                SimilarityLabel.Text = String.Format("Similarity: {0}", e.Error);
            }
        }

        private void FaceCaptureEventHander(object sender, IFaceCaptureImageEvent e)
        {
            if (e.Image == null)
                return;

            FirstImageData = e.Image;
            FirstImage.Source = ImageSource.FromStream(() => new MemoryStream(FirstImageData));
        }

        async void FirstImage_Clicked(System.Object sender, System.EventArgs evt)
        {
            var data = await GetImageAsync();
            if (data == null)
                return;

            FirstImageData = data;
            FirstImage.Source = ImageSource.FromStream(() => new MemoryStream(FirstImageData));
        }

        async void SecondImage_Clicked(System.Object sender, System.EventArgs evt)
        {
            var data = await GetImageAsync();
            if (data == null)
                return;

            SecondImageData = data;
            SecondImage.Source = ImageSource.FromStream(() => new MemoryStream(SecondImageData));
        }

        private async Task<byte[]> GetImageAsync()
        {
            string action = await DisplayActionSheet("Image from:", "Cancel", null, "Gallery", "Camera");

            switch (action)
            {
                case "Gallery":
                    var stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
                    if (stream == null)
                        return null;

                    return Utils.ConvertStreamToByteArray(stream);
                case "Camera":
                    FaceSdk.FaceCaptureImage();
                    break;
            }

            return null;
        }

        void MatchFaces_Clicked(System.Object sender, System.EventArgs evt)
        {
            if (FirstImageData == null || SecondImageData == null)
                return;

            IsEnableButtons = false;
            FaceSdk.MatchFaces(FirstImageData, SecondImageData);
            SimilarityLabel.Text = "Processing...";
        }

        void Liveness_Clicked(System.Object sender, System.EventArgs evt)
        {
            IsEnableButtons = false;
            FaceSdk.StartLiveness();
            LivenessLabel.Text = "Processing...";
        }

        void Clear_Clicked(System.Object sender, System.EventArgs evt)
        {
            FirstImageData = null;
            SecondImageData = null;

            SimilarityLabel.Text = "Similarity: null";
            LivenessLabel.Text = "Liveness: null";

            FirstImage.Source = ImageSource.FromFile("AvatarFirst.png");
            SecondImage.Source = ImageSource.FromFile("AvatarSecond.png");
        }
    }
}
