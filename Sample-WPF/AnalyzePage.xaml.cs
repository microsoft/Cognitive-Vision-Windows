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
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;

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
    /// Interaction logic for AnalyzePage.xaml.
    /// </summary>
    public partial class AnalyzePage : ImageScenarioPage
    {
        public AnalyzePage()
        {
            InitializeComponent();
            this.PreviewImage = _imagePreview;
            this.URLTextBox = _urlTextBox;
            this._language.ItemsSource = RecognizeLanguage.SupportedForAnalysis;
        }

        /// <summary>
        /// Uploads the image to Cognitive Services and performs analysis.
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <returns>Awaitable image analysis result.</returns>
        private async Task<ImageAnalysis> UploadAndAnalyzeImageAsync(string imageFilePath)
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
                    // Analyze the image for all visual features.
                    //
                    Log("Calling ComputerVisionClient.AnalyzeImageInStreamAsync()...");
                    VisualFeatureTypes[] visualFeatures = GetSelectedVisualFeatures();
                    string language = (_language.SelectedItem as RecognizeLanguage).ShortCode;
                    ImageAnalysis analysisResult = await client.AnalyzeImageInStreamAsync(imageFileStream, visualFeatures, null, language);
                    return analysisResult;
                }
            }

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        /// <summary>
        /// Sends a URL to Cognitive Services and performs analysis.
        /// </summary>
        /// <param name="imageUrl">The URL of the image to analyze.</param>
        /// <returns>Awaitable image analysis result.</returns>
        private async Task<ImageAnalysis> AnalyzeUrlAsync(string imageUrl)
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
                // Analyze the URL for all visual features.
                //
                Log("Calling ComputerVisionClient.AnalyzeImageAsync()...");
                VisualFeatureTypes[] visualFeatures = GetSelectedVisualFeatures();
                string language = (_language.SelectedItem as RecognizeLanguage).ShortCode;
                ImageAnalysis analysisResult = await client.AnalyzeImageAsync(imageUrl, visualFeatures, null, language);
                return analysisResult;
            }

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        private VisualFeatureTypes[] GetSelectedVisualFeatures()
        {
            var visualFeatures = new List<VisualFeatureTypes>();
            foreach (var child in _visualFeatures.Children)
            {
                var control = child as CheckBox;
                if (control?.IsChecked == true)
                {
                    visualFeatures.Add((VisualFeatureTypes)Enum.Parse(typeof(VisualFeatureTypes), control?.Content?.ToString()));
                }
            }
            return visualFeatures.Count > 0 ? visualFeatures.ToArray() : null;
        }

        /// <summary>
        /// Perform the work for this scenario.
        /// </summary>
        /// <param name="imageUri">The URI of the image to run against the scenario.</param>
        /// <param name="upload">Upload the image to Cognitive Services if [true]; submit the Uri as a remote URL if [false].</param>
        protected override async Task DoWorkAsync(Uri imageUri, bool upload)
        {
            _status.Text = "Analyzing...";

            //
            // Either upload an image, or supply a URL.
            //
            ImageAnalysis analysisResult;
            if (upload)
            {
                analysisResult = await UploadAndAnalyzeImageAsync(imageUri.LocalPath);
            }
            else
            {
                analysisResult = await AnalyzeUrlAsync(imageUri.AbsoluteUri);
            }
            _status.Text = "Analyzing Done";

            //
            // Log analysis result in the log window.
            //
            Log("");
            Log("Analysis Result:");
            LogAnalysisResult(analysisResult);
        }
    }
}
