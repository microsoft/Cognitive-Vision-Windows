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
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
// -----------------------------------------------------------------------
// KEY SAMPLE CODE ENDS HERE
// -----------------------------------------------------------------------

namespace VisionAPI_WPF_Samples
{
    /// <summary>
    /// Interaction logic for TagsPage.xaml
    /// </summary>
    public partial class TagsPage : ImageScenarioPage
    {
        public TagsPage()
        {
            InitializeComponent();
            this.PreviewImage = _imagePreview;
            this.URLTextBox = _urlTextBox;
            this._language.ItemsSource = new RecognizeLanguage[] { RecognizeLanguage.EN, RecognizeLanguage.JA, RecognizeLanguage.PT, RecognizeLanguage.ZH };
        }

        /// <summary>
        /// Uploads the image to Project Oxford and generates tags
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <returns></returns>
        private async Task<TagResult> UploadAndGetTagsForImage(string imageFilePath)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Project Oxford Vision API Service client
            //
            using (ComputerVisionClient VisionServiceClient = new ComputerVisionClient(Credentials) { Endpoint = Endpoint })
            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                Log("VisionServiceClient is created");

                //
                // Upload and image and generate tags
                //
                Log("Calling VisionServiceClient.GetTagsAsync()...");
                string language = (_language.SelectedItem as RecognizeLanguage).ShortCode;
                TagResult analysisResult = await VisionServiceClient.TagImageInStreamAsync(imageFileStream, language);
                return analysisResult;
            }

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        /// <summary>
        /// Sends a url to Project Oxford and generates tags for it
        /// </summary>
        /// <param name="imageUrl">The url of the image to generate tags for</param>
        /// <returns></returns>
        private async Task<TagResult> GenerateTagsForUrl(string imageUrl)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Project Oxford Vision API Service client
            //
            using (var VisionServiceClient = new ComputerVisionClient(Credentials) { Endpoint = Endpoint })
            {
                Log("VisionServiceClient is created");

                //
                // Generate tags for the given url
                //
                Log("Calling VisionServiceClient.GetTagsAsync()...");
                string language = (_language.SelectedItem as RecognizeLanguage).ShortCode;
                TagResult analysisResult = await VisionServiceClient.TagImageAsync(imageUrl, language);
                return analysisResult;
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
            _status.Text = "Generating tags...";

            //
            // Either upload an image, or supply a url
            //
            TagResult tagResult;
            if (upload)
            {
                tagResult = await UploadAndGetTagsForImage(imageUri.LocalPath);
            }
            else
            {
                tagResult = await GenerateTagsForUrl(imageUri.AbsoluteUri);
            }
            _status.Text = "Done";

            //
            // Log analysis result in the log window
            //
            Log("");
            Log("Get Tags Result:");
            LogTagResult(tagResult);
        }
    }
}
