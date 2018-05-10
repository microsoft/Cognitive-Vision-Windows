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
    /// Interaction logic for TextRecognitionPage.xaml
    /// </summary>
    public partial class TextRecognitionPage : ImageScenarioPage
    {
        public TextRecognitionPage()
        {
            InitializeComponent();
            this.PreviewImage = _imagePreview;
            this.URLTextBox = _urlTextBox;
        }

        /// <summary>
        /// Uploads the image to Project Oxford and performs Text Recognition
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <param name="mode">The recognition mode.</param>
        /// <returns></returns>
        private async Task<TextRecognitionOperationResult> UploadAndRecognizeImage(string imageFilePath, TextRecognitionMode mode)
        {
            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                return await RecognizeAsync(async (VisionServiceClient VisionServiceClient) => await VisionServiceClient.CreateTextRecognitionOperationAsync(imageFileStream, mode));
            }
        }

        /// <summary>
        /// Sends a url to Project Oxford and performs Text Recognition
        /// </summary>
        /// <param name="imageUrl">The url to perform recognition on</param>
        /// <param name="mode">The recognition mode.</param>
        /// <returns></returns>
        private async Task<TextRecognitionOperationResult> RecognizeUrl(string imageUrl, TextRecognitionMode mode)
        {
            return await RecognizeAsync(async (VisionServiceClient VisionServiceClient) => await VisionServiceClient.CreateTextRecognitionOperationAsync(imageUrl, mode));
        }

        private async Task<TextRecognitionOperationResult> RecognizeAsync(Func<VisionServiceClient, Task<TextRecognitionOperation>> Func)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Project Oxford Vision API Service client
            //
            VisionServiceClient VisionServiceClient = new VisionServiceClient(SubscriptionKey, "https://westus.api.cognitive.microsoft.com/vision/v2.0");
            Log("VisionServiceClient is created");

            TextRecognitionOperationResult result;
            try
            {
                Log("Calling VisionServiceClient.CreateTextRecognitionOperationAsync()...");
                TextRecognitionOperation operation = await Func(VisionServiceClient);

                Log("Calling VisionServiceClient.GetTextRecognitionOperationResultAsync()...");
                result = await VisionServiceClient.GetTextRecognitionOperationResultAsync(operation);

                int i = 0;
                while ((result.Status == TextRecognitionOperationStatus.Running || result.Status == TextRecognitionOperationStatus.NotStarted) && i++ < MaxRetryTimes)
                {
                    Log(string.Format("Server status: {0}, wait {1} seconds...", result.Status, QueryWaitTimeInSecond));
                    await Task.Delay(QueryWaitTimeInSecond);

                    Log("Calling VisionServiceClient.GetTextRecognitionOperationResultAsync()...");
                    result = await VisionServiceClient.GetTextRecognitionOperationResultAsync(operation);
                }

            }
            catch (ClientException ex)
            {
                result = new TextRecognitionOperationResult() { Status = TextRecognitionOperationStatus.Failed };
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
            TextRecognitionMode mode = printedRadioButton.IsChecked ?? true ? TextRecognitionMode.Printed : TextRecognitionMode.Handwritten;

            string logInfo = string.Format("Performing text recognition using {0} Mode...", mode);
            _status.Text = logInfo;
            Log(logInfo);
            //
            // Either upload an image, or supply a url
            //
            TextRecognitionOperationResult result;
            if (upload)
            {
                result = await UploadAndRecognizeImage(imageUri.LocalPath, mode);
            }
            else
            {
                result = await RecognizeUrl(imageUri.AbsoluteUri, mode);
            }

            logInfo = "Text recognition finished!";
            _status.Text = logInfo;

            //
            // Log analysis result in the log window
            //
            LogTextRecognitionResult(result);
            Log(logInfo);
        }
    }
}
