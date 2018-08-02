//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
//
// Microsoft Cognitive Services (formerly Project Oxford): https://www.microsoft.com/cognitive-services
//
// Microsoft Cognitive Services (formerly Project Oxford) GitHub:
// https://github.com/Microsoft/Cognitive-Vision-Windows
//
// Copyright (c) Microsoft Corporation
// All rights reserved.
//
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;
using System.Threading.Tasks;

// -----------------------------------------------------------------------
// KEY SAMPLE CODE STARTS HERE
// Use the following namesapce for VisionServiceClient
// -----------------------------------------------------------------------
using Microsoft.ProjectOxford.Common;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
// -----------------------------------------------------------------------
// KEY SAMPLE CODE ENDS HERE
// -----------------------------------------------------------------------

namespace VisionAPI_WPF_Samples
{
    /// <summary>
    /// Interaction logic for HandwritingOCRPage.xaml
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
        /// Gets the TextRecognitionMode value from the UI.
        /// </summary>
        /// <returns>TextRecognitionMode enumerated type value.</returns>
        private TextRecognitionMode RecognitionMode => (TextRecognitionMode)Enum.Parse(typeof(TextRecognitionMode), _mode.Text);

        /// <summary>
        /// Uploads the image to Project Oxford and performs Handwriting Recognition
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <returns></returns>
        private async Task<TextOperationResult> UploadAndRecognizeImage(string imageFilePath)
        {
            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                return await RecognizeAsync(
                    async (ComputerVisionClient VisionServiceClient) => await VisionServiceClient.RecognizeTextInStreamAsync(imageFileStream, RecognitionMode),
                    headers => headers.OperationLocation);
            }
        }

        /// <summary>
        /// Sends a url to Project Oxford and performs Handwriting Recognition
        /// </summary>
        /// <param name="imageUrl">The url to perform recognition on</param>
        /// <returns></returns>
        private async Task<TextOperationResult> RecognizeUrl(string imageUrl)
        {
            return await RecognizeAsync(
                async (ComputerVisionClient VisionServiceClient) => await VisionServiceClient.RecognizeTextAsync(imageUrl, RecognitionMode),
                headers => headers.OperationLocation);
        }

        private async Task<TextOperationResult> RecognizeAsync<T>(Func<ComputerVisionClient, Task<T>> GetHeadersAsyncFunc, Func<T, string> GetOperationUrlFunc) where T : new()
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------
            var result = default(TextOperationResult);

            //
            // Create Project Oxford Vision API Service client
            //
            using (var VisionServiceClient = new ComputerVisionClient(Credentials) { Endpoint = Endpoint })
            {
                Log("VisionServiceClient is created");

                try
                {
                    Log("Calling VisionServiceClient.CreateHandwritingRecognitionOperationAsync()...");

                    T recognizeHeaders = await GetHeadersAsyncFunc(VisionServiceClient);
                    string operationUrl = GetOperationUrlFunc(recognizeHeaders);
                    string operationId = operationUrl.Substring(operationUrl.LastIndexOf('/') + 1);

                    for (int attempt = 1; attempt <= MaxRetryTimes; attempt++)
                    {
                        Log("Calling VisionServiceClient.GetHandwritingRecognitionOperationResultAsync()...");
                        result = await VisionServiceClient.GetTextOperationResultAsync(operationId);

                        if (result.Status == TextOperationStatusCodes.Failed || result.Status == TextOperationStatusCodes.Succeeded)
                        {
                            break;
                        }

                        Log(string.Format("Server status: {0}, wait {1} seconds...", result.Status, QueryWaitTimeInSecond));
                        await Task.Delay(QueryWaitTimeInSecond);

                        Log("Calling VisionServiceClient.GetHandwritingRecognitionOperationResultAsync()...");
                        result = await VisionServiceClient.GetTextOperationResultAsync(operationId);
                    }

                }
                catch (ClientException ex)
                {
                    result = new TextOperationResult() { Status = TextOperationStatusCodes.Failed };
                    Log(ex.Error.Message);
                }
                return result;
            }

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
            TextOperationResult result;
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
