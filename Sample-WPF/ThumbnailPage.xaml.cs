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
using System.Windows.Media.Imaging;

// -----------------------------------------------------------------------
// KEY SAMPLE CODE STARTS HERE
// Use the following namespace for ComputerVisionClient.
// -----------------------------------------------------------------------
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
// -----------------------------------------------------------------------
// KEY SAMPLE CODE ENDS HERE
// -----------------------------------------------------------------------

namespace VisionAPI_WPF_Samples
{
    /// <summary>
    /// Interaction logic for ThumbnailPage.xaml.
    /// </summary>
    public partial class ThumbnailPage : ImageScenarioPage
    {
        public ThumbnailPage()
        {
            InitializeComponent();
            this.PreviewImage = _imagePreview;
            this.URLTextBox = _urlTextBox;
        }

        /// <summary>
        /// Uploads the image to Cognitive Services and generates a thumbnail.
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <param name="width">Width of the thumbnail. It must be between 1 and 1024. Recommended minimum of 50.</param>
        /// <param name="height">Height of the thumbnail. It must be between 1 and 1024. Recommended minimum of 50.</param>
        /// <param name="smartCropping">Boolean flag for enabling smart cropping.</param>
        /// <returns>Awaitable stream containing the image thumbnail.</returns>
        private async Task<Stream> UploadAndThumbnailImageAsync(string imageFilePath, int width, int height, bool smartCropping)
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
                    // Upload an image and generate a thumbnail.
                    //
                    Log("Calling ComputerVisionClient.GenerateThumbnailInStreamAsync()...");
                    return await client.GenerateThumbnailInStreamAsync(width, height, imageFileStream, smartCropping);
                }
            }

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }

        /// <summary>
        /// Sends a URL to Cognitive Services and generates a thumbnail.
        /// </summary>
        /// <param name="imageUrl">The URL of the image for which to generate a thumbnail.</param>
        /// <param name="width">Width of the thumbnail. It must be between 1 and 1024. Recommended minimum of 50.</param>
        /// <param name="height">Height of the thumbnail. It must be between 1 and 1024. Recommended minimum of 50.</param>
        /// <param name="smartCropping">Boolean flag for enabling smart cropping.</param>
        /// <returns>Awaitable stream containing the image thumbnail.</returns>
        private async Task<Stream> ThumbnailUrlAsync(string imageUrl, int width, int height, bool smartCropping)
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
                // Generate a thumbnail for the given URL.
                //
                Log("Calling ComputerVisionClient.GenerateThumbnailAsync()...");
               return await client.GenerateThumbnailAsync(width, height, imageUrl, smartCropping);
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
            _status.Text = "Generating Thumbnail...";

            //
            // Get the parameters.
            //
            int width = int.Parse(_widthTextBox.Text);
            int height = int.Parse(_heightTextBox.Text);
            bool useSmartCropping = _smartCroppingCheckbox.IsChecked.Value;

            //
            // Either upload an image, or supply a URL.
            //
            Stream thumbnailStream;
            if (upload)
            {
                thumbnailStream = await UploadAndThumbnailImageAsync(imageUri.LocalPath, width, height, useSmartCropping);
            }
            else
            {
                thumbnailStream = await ThumbnailUrlAsync(imageUri.AbsoluteUri, width, height, useSmartCropping);
            }
            _status.Text = "Thumbnail Generated";

            //
            // Show the generated thumbnail in the GUI.
            //
            BitmapImage thumbSource = new BitmapImage();
            thumbSource.BeginInit();
            thumbSource.CacheOption = BitmapCacheOption.None;
            thumbSource.StreamSource = thumbnailStream;
            thumbSource.EndInit();

            _thumbPreview.Source = thumbSource;

            Log("Generated Thumbnail");
        }
    }
}
