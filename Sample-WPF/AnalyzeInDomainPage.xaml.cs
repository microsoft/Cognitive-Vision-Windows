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
using System.Windows;

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
    /// Interaction logic for AnalyzeInDomainPage.xaml.
    /// </summary>
    public partial class AnalyzeInDomainPage : ImageScenarioPage
    {
        public AnalyzeInDomainPage()
        {
            InitializeComponent();
            this.PreviewImage = _imagePreview;
            this.URLTextBox = _urlTextBox;
            this._language.ItemsSource = RecognizeLanguage.SupportedForAnalysis;
        }

        /// <summary>
        /// Get a list of available domain models.
        /// </summary>
        /// <returns>Awaitable list of domains available for analysis.</returns>
        private async Task<ListModelsResult> GetAvailableDomainModelsAsync()
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
                // Analyze the URL against the given domain.
                //
                Log("Calling ComputerVisionClient.ListModelsAsync()...");
                ListModelsResult modelResult = await client.ListModelsAsync();
                return modelResult;
            }

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        /// <summary>
        /// Uploads the image to Cognitive Services and performs analysis against a given domain.
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <param name="domainModel">The domain model for analyzing an image.</param>
        /// <returns>Awaitable domain-specific analysis result.</returns>
        private async Task<DomainModelResults> UploadAndAnalyzeInDomainImageAsync(string imageFilePath, ModelDescription domainModel)
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
                    // Analyze the image for the given domain.
                    //
                    Log("Calling ComputerVisionClient.AnalyzeImageByDomainInStreamAsync()...");
                    string language = ((RecognizeLanguage)_language.SelectedItem).ShortCode;
                    DomainModelResults analysisResult = await client.AnalyzeImageByDomainInStreamAsync(domainModel.Name, imageFileStream, language);
                    return analysisResult;
                }
            }

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        /// <summary>
        /// Sends a URL to Cognitive Services and performs analysis against a given domain.
        /// </summary>
        /// <param name="imageUrl">The URL of the image to analyze.</param>
        /// <param name="domainModel">The domain model to analyze against.</param>
        /// <returns>Awaitable domain-specific analysis result.</returns>
        private async Task<DomainModelResults> AnalyzeInDomainUrlAsync(string imageUrl, ModelDescription domainModel)
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
                // Analyze the URL against the given domain.
                //
                Log("Calling ComputerVisionClient.AnalyzeImageByDomainAsync()...");
                string language = (_language.SelectedItem as RecognizeLanguage).ShortCode;
                DomainModelResults analysisResult = await client.AnalyzeImageByDomainAsync(domainModel.Name, imageUrl, language);
                return analysisResult;
            }
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        protected override async Task DoWorkAsync(Uri imageUri, bool upload)
        {
            _status.Text = "Analyzing...";

            ModelDescription domainModel = _domainModelComboBox.SelectedItem as ModelDescription;

            //
            // Either upload an image, or supply a URL.
            //
            DomainModelResults analysisInDomainResult;
            if (upload)
            {
                analysisInDomainResult = await UploadAndAnalyzeInDomainImageAsync(imageUri.LocalPath, domainModel);
            }
            else
            {
                analysisInDomainResult = await AnalyzeInDomainUrlAsync(imageUri.AbsoluteUri, domainModel);
            }
            _status.Text = "Analyzing Done";

            //
            // Log analysis result in the log window.
            //
            Log("");
            Log("Analysis In Domain Result:");
            LogAnalysisInDomainResult(analysisInDomainResult);
        }

        private async void LoadModelsButton_Click(object sender, RoutedEventArgs e)
        {
            _status.Text = "Loading models...";

            //
            // Get the available models.
            //
            var modelResult = await GetAvailableDomainModelsAsync();
            _domainModelComboBox.ItemsSource = modelResult.ModelsProperty;

            _status.Text = "Loaded models";

            _stepTwoPanel.Visibility = Visibility.Visible;

            Log("Models loaded");

        }
    }
}
