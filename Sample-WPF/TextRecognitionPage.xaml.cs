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
// Use the following namespace for ComputerVisionClient.
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
    /// Interaction logic for TextRecognitionPage.xaml.
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
        /// Uploads the image to Cognitive Services and performs Text Recognition.
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <returns>Awaitable OCR result.</returns>
        private async Task<TextOperationResult> UploadAndRecognizeImageAsync(string imageFilePath)
        {
            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                return await RecognizeAsync(
                    async (ComputerVisionClient client) => await client.RecognizeTextInStreamAsync(imageFileStream, RecognitionMode),
                    headers => headers.OperationLocation);
            }
        }

        /// <summary>
        /// Sends a URL to Cognitive Services and performs Text Recognition.
        /// </summary>
        /// <param name="imageUrl">The image URL on which to perform recognition</param>
        /// <returns>Awaitable OCR result.</returns>
        private async Task<TextOperationResult> RecognizeUrlAsync(string imageUrl)
        {
            return await RecognizeAsync(
                async (ComputerVisionClient client) => await client.RecognizeTextAsync(imageUrl, RecognitionMode),
                headers => headers.OperationLocation);
        }

        private async Task<TextOperationResult> RecognizeAsync<T>(Func<ComputerVisionClient, Task<T>> GetHeadersAsyncFunc, Func<T, string> GetOperationUrlFunc) where T : new()
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------
            var result = default(TextOperationResult);

            //
            // Create Cognitive Services Vision API Service client.
            //
            using (var client = new ComputerVisionClient(Credentials) { Endpoint = Endpoint })
            {
                Log("ComputerVisionClient is created");

                try
                {
                    Log("Calling ComputerVisionClient.RecognizeTextAsync()...");

                    T recognizeHeaders = await GetHeadersAsyncFunc(client);
                    string operationUrl = GetOperationUrlFunc(recognizeHeaders);
                    string operationId = operationUrl.Substring(operationUrl.LastIndexOf('/') + 1);

                    Log("Calling ComputerVisionClient.GetTextOperationResultAsync()...");
                    result = await client.GetTextOperationResultAsync(operationId);

                    for (int attempt = 1; attempt <= MaxRetryTimes; attempt++)
                    {
                        if (result.Status == TextOperationStatusCodes.Failed || result.Status == TextOperationStatusCodes.Succeeded)
                        {
                            break;
                        }

                        Log(string.Format("Server status: {0}, wait {1} seconds...", result.Status, QueryWaitTimeInSecond));
                        await Task.Delay(QueryWaitTimeInSecond);

                        Log("Calling ComputerVisionClient.GetTextOperationResultAsync()...");
                        result = await client.GetTextOperationResultAsync(operationId);
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
        /// Perform the work for this scenario.
        /// </summary>
        /// <param name="imageUri">The URI of the image to run against the scenario.</param>
        /// <param name="upload">Upload the image to Cognitive Services if [true]; submit the Uri as a remote URL if [false].</param>
        protected override async Task DoWorkAsync(Uri imageUri, bool upload)
        {
            _status.Text = "Performing text recognition...";

            //
            // Either upload an image, or supply a URL.
            //
            TextOperationResult result;
            if (upload)
            {
                result = await UploadAndRecognizeImageAsync(imageUri.LocalPath);
            }
            else
            {
                result = await RecognizeUrlAsync(imageUri.AbsoluteUri);
            }
            _status.Text = "Text recognition finished!";

            //
            // Log analysis result in the log window.
            //
            LogTextRecognitionResult(result);
            Log("Text recognition finished!");
        }
    }
}
