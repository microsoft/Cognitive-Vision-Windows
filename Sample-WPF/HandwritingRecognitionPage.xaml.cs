using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.IO;

namespace VisionAPI_WPF_Samples
{
    /// <summary>
    /// Interaction logic for HandwritingOCRPage.xaml
    /// </summary>
    public partial class HandwritingRecognitionPage : ImageScenarioPage
    {
        public HandwritingRecognitionPage()
        {
            InitializeComponent();
            this.PreviewImage = _imagePreview;
            this.URLTextBox = _urlTextBox;
        }

        /// <summary>
        /// Uploads the image to Project Oxford and performs Handwriting Recognition
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <returns></returns>
        private async Task<HandwritingRecognitionOperationResult> UploadAndRecognizeImage(string imageFilePath)
        {
            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                return await RecognizeAsync(async (VisionServiceClient VisionServiceClient) => await VisionServiceClient.CreateHandwritingRecognitionOperationAsync(imageFileStream));
            }
        }

        /// <summary>
        /// Sends a url to Project Oxford and performs Handwriting Recognition
        /// </summary>
        /// <param name="imageUrl">The url to perform recognition on</param>
        /// <returns></returns>
        private async Task<HandwritingRecognitionOperationResult> RecognizeUrl(string imageUrl)
        {
            return await RecognizeAsync(async (VisionServiceClient VisionServiceClient) => await VisionServiceClient.CreateHandwritingRecognitionOperationAsync(imageUrl));
        }

        private async Task<HandwritingRecognitionOperationResult> RecognizeAsync(Func<VisionServiceClient, Task<HandwritingRecognitionOperation>> Func)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Project Oxford Vision API Service client
            //
            VisionServiceClient VisionServiceClient = new VisionServiceClient(SubscriptionKey, SubscriptionEndpoint);
            Log("VisionServiceClient is created");

            HandwritingRecognitionOperationResult result;
            try
            {
                Log("Calling VisionServiceClient.CreateHandwritingRecognitionOperationAsync()...");
                HandwritingRecognitionOperation operation = await Func(VisionServiceClient);

                Log("Calling VisionServiceClient.GetHandwritingRecognitionOperationResultAsync()...");
                result = await VisionServiceClient.GetHandwritingRecognitionOperationResultAsync(operation);

                int i = 0;
                while ((result.Status == HandwritingRecognitionOperationStatus.Running || result.Status == HandwritingRecognitionOperationStatus.NotStarted) && i++ < MaxRetryTimes)
                {
                    Log(string.Format("Server status: {0}, wait {1} seconds...", result.Status, QueryWaitTimeInSecond));
                    await Task.Delay(QueryWaitTimeInSecond);

                    Log("Calling VisionServiceClient.GetHandwritingRecognitionOperationResultAsync()...");
                    result = await VisionServiceClient.GetHandwritingRecognitionOperationResultAsync(operation);
                }

            }
            catch (ClientException ex)
            {
                result = new HandwritingRecognitionOperationResult() { Status = HandwritingRecognitionOperationStatus.Failed };
                Log(ex.Error.Message);
            }

            return result;

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        /// <summary>
        /// Perform the work for this scenario
        /// </summary>
        /// <param name="imageUri">The URI of the image to run against the scenario</param>
        /// <param name="upload">Upload the image to Project Oxford if [true]; submit the Uri as a remote url if [false];</param>
        /// <returns></returns>
        protected override async Task DoWork(Uri imageUri, bool upload)
        {
            _status.Text = "Performing Handwriting recognition...";

            //
            // Either upload an image, or supply a url
            //
            HandwritingRecognitionOperationResult result;
            if (upload)
            {
                result = await UploadAndRecognizeImage(imageUri.LocalPath);
            }
            else
            {
                result = await RecognizeUrl(imageUri.AbsoluteUri);
            }
            _status.Text = "Handwriting recognition finished!";

            //
            // Log analysis result in the log window
            //
            LogHandwritingRecognitionResult(result);
            Log("Handwriting recognition finished!");
        }
    }
}
