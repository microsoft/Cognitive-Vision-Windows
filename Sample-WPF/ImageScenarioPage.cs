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


using static System.FormattableString;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace VisionAPI_WPF_Samples
{
    /// <summary>
    /// Common UI code for each Image based scenario.
    /// </summary>
    public abstract class ImageScenarioPage : Page
    {
        private MainWindow MainWindow => ((MainWindow)Application.Current.MainWindow);
        protected static readonly TimeSpan QueryWaitTimeInSecond = TimeSpan.FromSeconds(3);
        protected static readonly int MaxRetryTimes = 3;

        protected Image PreviewImage { get; set; }

        protected TextBox URLTextBox { get; set; }

        protected ApiKeyServiceClientCredentials Credentials
        {
            get
            {
                return new ApiKeyServiceClientCredentials(MainWindow.ScenarioControl.SubscriptionKey);
            }
        }

        protected string Endpoint
        {
            get
            {
                return MainWindow.ScenarioControl.SubscriptionEndpoint;
            }
        }

        /// <summary>
        /// Perform the work for this scenario.
        /// </summary>
        /// <param name="imageUri">The URI of the image to run against the scenario.</param>
        /// <param name="upload">Upload the image to Cognitive Services if [true]; submit the Uri as a remote URL if [false].</param>
        protected abstract Task DoWorkAsync(Uri imageUri, bool upload);

        private async Task ShowPreviewAndDoWorkAsync(Uri imageUri, bool upload)
        {
            try
            {
                // Clear the log.
                MainWindow.ScenarioControl.ClearLog();

                // Show the image on the GUI.
                BitmapImage bitmapSource = new BitmapImage();
                bitmapSource.BeginInit();
                bitmapSource.CacheOption = BitmapCacheOption.None;
                bitmapSource.UriSource = imageUri;
                bitmapSource.EndInit();
                PreviewImage.Source = bitmapSource;

                // do the actual work for the scenario.
                await DoWorkAsync(imageUri, upload);
            }
            catch (Exception exception)
            {
                // Something wen't wrong :(
                Log(exception.ToString());
            }
        }

        protected async void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openDlg = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Image Files(*.jpg, *.gif, *.bmp, *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png"
            };
            bool? result = openDlg.ShowDialog(Application.Current.MainWindow);

            if (!(bool)result)
            {
                return;
            }

            string imageFilePath = openDlg.FileName;
            Uri fileUri = new Uri(imageFilePath);

            await ShowPreviewAndDoWorkAsync(fileUri, true);
        }

        protected async void SubmitUriButton_Click(object sender, RoutedEventArgs e)
        {
            Uri imageUri = new Uri(URLTextBox.Text);
            await ShowPreviewAndDoWorkAsync(imageUri, false);
        }

        /// <summary>
        /// Logs a message to the parent control.
        /// </summary>
        /// <param name="message">The message to log.</param>
        internal void Log(string message)
        {
           MainWindow.ScenarioControl.Log(message);
        }

        /// <summary>
        /// Show Analysis Result.
        /// </summary>
        /// <param name="result">Analysis result to log.</param>
        protected void LogAnalysisResult(ImageAnalysis result)
        {
            if (result == null)
            {
                Log("null");
                return;
            }

            if (result.Metadata != null)
            {
                Log("Metadata : ");
                Log(Invariant($"   Image Format : {result.Metadata.Format}"));
                Log(Invariant($"   Image Dimensions : {result.Metadata.Width} x {result.Metadata.Height}"));
            }

            if (result.ImageType != null)
            {
                Log("Image Type : ");
                string clipArtType;
                switch (result.ImageType.ClipArtType)
                {
                    case 0:
                        clipArtType = "0 Non-clipart";
                        break;
                    case 1:
                        clipArtType = "1 ambiguous";
                        break;
                    case 2:
                        clipArtType = "2 normal-clipart";
                        break;
                    case 3:
                        clipArtType = "3 good-clipart";
                        break;
                    default:
                        clipArtType = "Unknown";
                        break;
                }
                Log(Invariant($"   Clip Art Type : {clipArtType}"));

                string lineDrawingType;
                switch (result.ImageType.LineDrawingType)
                {
                    case 0:
                        lineDrawingType = "0 Non-LineDrawing";
                        break;
                    case 1:
                        lineDrawingType = "1 LineDrawing";
                        break;
                    default:
                        lineDrawingType = "Unknown";
                        break;
                }
                Log(Invariant($"   Line Drawing Type : {lineDrawingType}"));
            }

            if (result.Adult != null)
            {
                Log("Adult : ");
                Log(Invariant($"   Is Adult Content : {result.Adult.IsAdultContent}"));
                Log(Invariant($"   Adult Score : {result.Adult.AdultScore}"));
                Log(Invariant($"   Is Racy Content : {result.Adult.IsRacyContent}"));
                Log(Invariant($"   Racy Score : {result.Adult.RacyScore}"));
            }

            if (result.Brands != null)
            {
                Log("Brands : ");
                foreach (var brand in result.Brands)
                {
                    Log(Invariant($"   Brand : {brand.Name}; Confidence : {brand.Confidence}; Bounding Box: {brand.Rectangle.ToReadableString()}"));
                }
            }

            if (result.Objects != null)
            {
                Log("Objects : ");
                foreach (var obj in result.Objects)
                {
                    Log(Invariant($"   Object : Bounding Box : {obj.Rectangle.ToReadableString()}"));
                    var detected = new ObjectHierarchy(obj.ObjectProperty, obj.Confidence, obj.Parent);
                    while (detected != null)
                    {
                        Log(Invariant($"      Label : {detected.ObjectProperty}; Confidence : {detected.Confidence}"));
                        detected = detected.Parent;
                    }
                }
            }

            if (result.Categories != null && result.Categories.Count > 0)
            {
                Log("Categories : ");
                foreach (var category in result.Categories)
                {
                    Log(Invariant($"   Name : {category.Name}; Score : {category.Score}"));
                }
            }

            if (result.Faces != null && result.Faces.Count > 0)
            {
                Log("Faces : ");
                foreach (var face in result.Faces)
                {
                    Log(Invariant($"   Age : {face.Age}; Gender : {face.Gender}"));
                }
            }

            if (result.Color != null)
            {
                Log("Color : ");
                Log(Invariant($"   AccentColor : {result.Color.AccentColor}"));
                Log(Invariant($"   Dominant Color Background : {result.Color.DominantColorBackground}"));
                Log(Invariant($"   Dominant Color Foreground : {result.Color.DominantColorForeground}"));

                if (result.Color.DominantColors != null && result.Color.DominantColors.Count > 0)
                {
                    string colors = "Dominant Colors : ";
                    foreach (var color in result.Color.DominantColors)
                    {
                        colors += color + " ";
                    }
                    Log(colors);
                }
            }

            if (result.Description != null)
            {
                Log("Description : ");
                foreach (var caption in result.Description.Captions)
                {
                    Log(Invariant($"   Caption : {caption.Text}; Confidence : {caption.Confidence}"));
                }
                string tags = "   Tags : ";
                foreach (var tag in result.Description.Tags)
                {
                    tags += tag + ", ";
                }
                Log(tags);

            }

            if (result.Tags != null)
            {
                Log("Tags : ");
                foreach (var tag in result.Tags)
                {
                    var hint = string.IsNullOrEmpty(tag.Hint) ? "" : Invariant($"; Hint : {tag.Hint}");
                    Log(Invariant($"   Name : {tag.Name}; Confidence : {tag.Confidence}{hint}"));
                }
            }
        }
        
        /// <summary>
        /// Log the result of an analysis in domain result.
        /// </summary>
        /// <param name="result">Domain analysis result to log.</param>
        protected void LogAnalysisInDomainResult(DomainModelResults result)
        {
            if (result.Metadata != null)
            {
                Log(Invariant($"Image Format : {result.Metadata.Format}"));
                Log(Invariant($"Image Dimensions : {result.Metadata.Width} x {result.Metadata.Height}"));
            }
            
            if (result.Result != null)
            {
                Log(Invariant($"Result : {result.Result.ToString()}"));
            }
        }

        /// <summary>
        /// Show Description Results.
        /// </summary>
        /// <param name="result">Description result to log.</param>
        protected void LogDescriptionResults(ImageDescription result)
        {
            ImageAnalysis analysisResult = new ImageAnalysis
            {
                Metadata = result.Metadata,
                Description = new ImageDescriptionDetails
                {
                    Captions = result.Captions,
                    Tags = result.Tags
                }
            };
            LogAnalysisResult(analysisResult);
        }

        /// <summary>
        /// Show Tagging Result.
        /// </summary>
        /// <param name="result">Tag result to log.</param>
        protected void LogTagResult(TagResult result)
        {
            ImageAnalysis analysisResult = new ImageAnalysis
            {
                Metadata = result.Metadata,
                Tags = result.Tags
            };
            LogAnalysisResult(analysisResult);
        }

        /// <summary>
        /// Log text from the given OCR results object.
        /// </summary>
        /// <param name="results">The OCR results.</param>
        protected void LogOcrResults(OcrResult results)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (results != null && results.Regions != null)
            {
                stringBuilder.Append("Text: ");
                stringBuilder.AppendLine();
                foreach (var item in results.Regions)
                {
                    foreach (var line in item.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            stringBuilder.Append(word.Text);
                            stringBuilder.Append(" ");
                        }

                        stringBuilder.AppendLine();
                    }

                    stringBuilder.AppendLine();
                }
            }

            Log(stringBuilder.ToString());
        }

        /// <summary>
        /// Log text from the given TextOperationResult object.
        /// </summary>
        /// <param name="results">The TextOperationResult.</param>
        protected void LogTextRecognitionResult(TextOperationResult results)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (results != null && results.RecognitionResult != null && results.RecognitionResult.Lines != null && results.RecognitionResult.Lines.Count > 0)
            {
                stringBuilder.Append("Text: ");
                stringBuilder.AppendLine();
                foreach (var line in results.RecognitionResult.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        stringBuilder.Append(word.Text);
                        stringBuilder.Append(" ");
                    }

                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine();
                }
            }

            if (string.IsNullOrWhiteSpace(stringBuilder.ToString()))
            {
                Log("No text is recognized.");
            }
            else
            {
                Log(stringBuilder.ToString());
            }
            
            if (results.Status == TextOperationStatusCodes.Running || results.Status == TextOperationStatusCodes.NotStarted)
            {
                Log(Invariant($"Status is {results.Status} after trying {MaxRetryTimes} times"));
            }
        }
    }
}
