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
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
// -----------------------------------------------------------------------
// KEY SAMPLE CODE ENDS HERE
// -----------------------------------------------------------------------

namespace VisionAPI_WPF_Samples
{
    /// <summary>
    /// Interaction logic for DescribePage.xaml.
    /// </summary>
    public partial class DescribePage : ImageScenarioPage
    {
        public DescribePage()
        {
            InitializeComponent();
            this.PreviewImage = _imagePreview;
            this.URLTextBox = _urlTextBox;
            this._language.ItemsSource = RecognizeLanguage.SupportedForDescription;
        }

        /// <summary>
        /// Uploads the image to Cognitive Services and performs description.
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <returns>Awaitable image description.</returns>
        private async Task<ImageDescription> UploadAndDescribeImageAsync(string imageFilePath)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Cognitive Services Vision API Service client.
            //
            using (var client = new ComputerVisionClient(Credentials) { Endpoint = Endpoint })
            {
                Log("ComputerVisionClient is created");

                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    //
                    // Upload and image and request three descriptions.
                    //
                    Log("Calling ComputerVisionClient.DescribeImageInStreamAsync()...");
                    string language = (_language.SelectedItem as RecognizeLanguage).ShortCode;
                    ImageDescription analysisResult = await client.DescribeImageInStreamAsync(imageFileStream, 3, language);
                    return analysisResult;
                }
            }

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        /// <summary>
        /// Sends a URL to Cognitive Services and performs description.
        /// </summary>
        /// <param name="imageUrl">The URL of the image to describe.</param>
        /// <returns>Awaitable image description.</returns>
        private async Task<ImageDescription> DescribeUrlAsync(string imageUrl)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Cognitive Services Vision API Service client.
            //
            using (var client = new ComputerVisionClient(Credentials) { Endpoint = Endpoint })
            {
                Log("ComputerVisionClient is created");

                //
                // Describe the URL and ask for three captions.
                //
                Log("Calling ComputerVisionClient.DescribeImageAsync()...");
                string language = (_language.SelectedItem as RecognizeLanguage).ShortCode;
                ImageDescription analysisResult = await client.DescribeImageAsync(imageUrl, 3, language);
                return analysisResult;
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
            _status.Text = "Describing...";

            //
            // Either upload an image, or supply a URL.
            //
            ImageDescription analysisResult;
            if (upload)
            {
                analysisResult = await UploadAndDescribeImageAsync(imageUri.LocalPath);
            }
            else
            {
                analysisResult = await DescribeUrlAsync(imageUri.AbsoluteUri);
            }
            _status.Text = "Describing Done";

            //
            // Log analysis result in the log window.
            //
            Log("");
            Log("Describe Result:");
            LogDescriptionResults(analysisResult);
        }
    }
}
