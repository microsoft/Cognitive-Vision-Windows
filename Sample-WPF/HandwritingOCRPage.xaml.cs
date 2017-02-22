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
    public partial class HandwritingOCRPage : ImageScenarioPage
    {
        public HandwritingOCRPage()
        {
            InitializeComponent();
            this.PreviewImage = _imagePreview;
            this.URLTextBox = _urlTextBox;
            this.languageComboBox.ItemsSource = GetSupportedLanguages();
        }

        /// <summary>
        /// Uploads the image to Project Oxford and performs OCR
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <param name="language">The language code to recognize for</param>
        /// <returns></returns>
        private async Task<HandwritingOCROperationResult> UploadAndRecognizeImage(string imageFilePath, string language)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Project Oxford Vision API Service client
            //
            VisionServiceClient VisionServiceClient = new VisionServiceClient(SubscriptionKey);
            Log("VisionServiceClient is created");

            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                //
                // Upload an image and perform OCR
                //
                Log("Calling VisionServiceClient.RecognizeTextAsync()...");

                HandwritingOCROperationResult result;
                try
                {
                    HandwritingOCROperation operationLocation = await VisionServiceClient.RecognizeHandwritingTextAsync(imageFileStream, language);

                    do
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5));
                        result = await VisionServiceClient.CheckRecognizeHandWritingTextStatus(operationLocation.OperationId);
                    }
                    while (result.StatusCode == HandwritingOCROperationStatus.Running || result.StatusCode == HandwritingOCROperationStatus.NotStarted);
                }
                catch (ClientException ex)
                {
                    result = new HandwritingOCROperationResult() { StatusCode = HandwritingOCROperationStatus.Failed };
                    Log(ex.Error.Message);
                }
                return result;
            }

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        /// <summary>
        /// Sends a url to Project Oxford and performs OCR
        /// </summary>
        /// <param name="imageUrl">The url to perform recognition on</param>
        /// <param name="language">The language code to recognize for</param>
        /// <returns></returns>
        private async Task<HandwritingOCROperationResult> RecognizeUrl(string imageUrl, string language)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Project Oxford Vision API Service client
            //
            VisionServiceClient VisionServiceClient = new VisionServiceClient(SubscriptionKey);
            Log("VisionServiceClient is created");

            //
            // Perform OCR on the given url
            //
            Log("Calling VisionServiceClient.RecognizeTextAsync()...");
            HandwritingOCROperationResult result;
            try
            {
            HandwritingOCROperation operationLocation = await VisionServiceClient.RecognizeHandwritingTextAsync(imageUrl, language);

            do
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                result = await VisionServiceClient.CheckRecognizeHandWritingTextStatus(operationLocation.OperationId);
            }
            while (result.StatusCode == HandwritingOCROperationStatus.Running || result.StatusCode == HandwritingOCROperationStatus.NotStarted);
            }
            catch (ClientException ex)
            {
                result = new HandwritingOCROperationResult() { StatusCode = HandwritingOCROperationStatus.Failed };
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
            _status.Text = "Performing OCR...";

            string languageCode = (languageComboBox.SelectedItem as RecognizeLanguage).ShortCode;

            //
            // Either upload an image, or supply a url
            //
            HandwritingOCROperationResult ocrResult;
            if (upload)
            {
                ocrResult = await UploadAndRecognizeImage(imageUri.LocalPath, languageCode);
            }
            else
            {
                ocrResult = await RecognizeUrl(imageUri.AbsoluteUri, languageCode);
            }
            _status.Text = "OCR Done";

            //
            // Log analysis result in the log window
            //
            Log("");
            Log("OCR Result:");
            LogOneOCRResult(ocrResult);
        }

        private List<RecognizeLanguage> GetSupportedLanguages()
        {
            return new List<RecognizeLanguage>()
            {
                new RecognizeLanguage(){ ShortCode = "en",      LongName = "English"  },
            };
        }
    }
}
