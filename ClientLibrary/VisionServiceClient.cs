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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Vision.Contract;

namespace Microsoft.ProjectOxford.Vision
{
    /// <summary>
    /// The vision service client.
    /// </summary>
    public class VisionServiceClient : ServiceClient, IVisionServiceClient
    {
        /// <summary>
        /// The header key for authorization
        /// </summary>
        private const string API_AUTH_HEADER_KEY = "Ocp-Apim-Subscription-Key";

        /// <summary>
        /// Initializes a new instance of the <see cref="VisionServiceClient"/> class.
        /// </summary>
        /// <param name="subscriptionKey">The subscription key.</param>
        /// <param name="apiRoot">Root URI for the service endpoint.</param>
        public VisionServiceClient(string subscriptionKey, string apiRoot)
            : base()
        {
            ApiRoot = apiRoot?.TrimEnd('/');
            AuthKey = API_AUTH_HEADER_KEY;
            AuthValue = subscriptionKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisionServiceClient"/> class.
        /// </summary>
        /// <param name="httpClient">HTTP client.  The caller is responsible for disposing this object.</param>
        /// <param name="subscriptionKey">The subscription key.</param>
        /// <param name="apiRoot">Root URI for the service endpoint.</param>
        public VisionServiceClient(HttpClient httpClient, string subscriptionKey, string apiRoot)
            : base(httpClient)
        {
            ApiRoot = apiRoot?.TrimEnd('/');
            AuthKey = API_AUTH_HEADER_KEY;
            AuthValue = subscriptionKey;
        }

        /// <summary>
        /// Analyzes the image.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="visualFeatures">The visual features.</param>
        /// <param name="languageCode">The language code. If omitted, 'en' will be used.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>The AnalysisResult object.</returns>
        [Obsolete("Please use the overloaded method which takes IEnumerable<VisualFeature>")]
        public async Task<AnalysisResult> AnalyzeImageAsync(string url, string[] visualFeatures = null, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var visualFeatureEnums = visualFeatures?.Select(feature => (VisualFeature)Enum.Parse(typeof(VisualFeature), feature, true));

            return await AnalyzeImageAsync(url, visualFeatureEnums, null, languageCode, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Analyzes the image.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="visualFeatures">The visual features.</param>
        /// <param name="languageCode">The language code. If omitted, 'en' will be used.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>The AnalysisResult object.</returns>
        [Obsolete("Please use the overloaded method which takes IEnumerable<VisualFeature>")]
        public Task<AnalysisResult> AnalyzeImageAsync(Stream imageStream, string[] visualFeatures = null, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var visualFeatureEnums = visualFeatures?.Select(feature => (VisualFeature)Enum.Parse(typeof(VisualFeature), feature, true));

            return AnalyzeImageCommonAsync(imageStream, visualFeatureEnums, null, languageCode, cancellationToken);
        }

        /// <summary>
        /// Analyzes the image.
        /// </summary>
        /// <param name="imageUrl">The URL.</param>
        /// <param name="visualFeatures">The visual features.</param>
        /// <param name="languageCode">The language code. If omitted, 'en' will be used.</param>
        /// <param name="details">Optional domain-specific models to invoke when appropriate.  To obtain names of models supported, invoke the <see cref="ListModelsAsync">ListModelsAsync</see> method.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>The AnalysisResult object.</returns>
        public Task<AnalysisResult> AnalyzeImageAsync(string imageUrl, IEnumerable<VisualFeature> visualFeatures = null, IEnumerable<string> details = null, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestBody = new UrlRequest() { url = imageUrl };

            return AnalyzeImageCommonAsync(requestBody, visualFeatures, details, languageCode, cancellationToken);
        }

        /// <summary>
        /// Analyzes the image.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="visualFeatures">The visual features.</param>
        /// <param name="details">Optional domain-specific models to invoke when appropriate.  To obtain names of models supported, invoke the <see cref="ListModelsAsync">ListModelsAsync</see> method.</param>
        /// <param name="languageCode">The language code. If omitted, 'en' will be used.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>The AnalysisResult object.</returns>
        public Task<AnalysisResult> AnalyzeImageAsync(Stream imageStream, IEnumerable<VisualFeature> visualFeatures = null, IEnumerable<string> details = null, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return AnalyzeImageCommonAsync(imageStream, visualFeatures, details, languageCode, cancellationToken);
        }

        /// <summary>
        /// Analyzes the image.
        /// </summary>
        /// <param name="requestBody">Body </param>
        /// <param name="visualFeatures">The visual features.</param>
        /// <param name="details">Optional domain-specific models to invoke when appropriate.  To obtain names of models supported, invoke the <see cref="ListModelsAsync">ListModelsAsync</see> method.</param>
        /// <param name="languageCode">The language code. If omitted, 'en' will be used.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>The AnalysisResult object.</returns>
        private async Task<AnalysisResult> AnalyzeImageCommonAsync<T>(T requestBody, IEnumerable<VisualFeature> visualFeatures, IEnumerable<string> details, string languageCode, CancellationToken cancellationToken)
        {
            var requestUrl = new StringBuilder("/analyze?");
            requestUrl.Append(string.Join("&", new List<string>
            {
                VisualFeaturesToString(visualFeatures),
                DetailsToQueryParam(details),
                LanguageCodeToQueryParam(languageCode)
            }
            .Where(s => !string.IsNullOrEmpty(s))));

            return await PostAsync<T, AnalysisResult>(requestUrl.ToString(), requestBody, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Analyzes the image using a domain-specific model.
        /// </summary>
        /// <param name="url">The image URL.</param>
        /// <param name="model">Domain-specific model.</param>
        /// <param name="languageCode">The language code. If omitted, 'en' will be used.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <remarks>The list of currently available models can be listed via the <see cref="ListModelsAsync"/> method.</remarks>
        /// <returns>The AnalysisInDomainResult object.</returns>
        public async Task<AnalysisInDomainResult> AnalyzeImageInDomainAsync(string url, Model model, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await AnalyzeImageInDomainAsync(url, model.Name, languageCode, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Analyzes the image using a domain-specific model.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="model">Domain-specific model.</param>
        /// <param name="languageCode">The language code. If omitted, 'en' will be used.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <remarks>The list of currently available models can be listed via the <see cref="ListModelsAsync"/> method.</remarks>
        /// <returns>The AnalysisInDomainResult object.</returns>
        public Task<AnalysisInDomainResult> AnalyzeImageInDomainAsync(Stream imageStream, Model model, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return AnalyzeImageInDomainAsync(imageStream, model.Name, languageCode, cancellationToken);
        }

        /// <summary>
        /// Analyzes the image using a domain-specific model.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="modelName">Name of the domain-specific model.</param>
        /// <param name="languageCode">The language code. If omitted, 'en' will be used.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <remarks>The list of currently available models can be listed via the <see cref="ListModelsAsync"/> method.</remarks>
        /// <returns>The AnalysisInDomainResult object.</returns>
        public Task<AnalysisInDomainResult> AnalyzeImageInDomainAsync(string imageUrl, string modelName, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestBody = new UrlRequest() { url = imageUrl };

            return AnalyzeImageInDomainCommonAsync(requestBody, modelName, languageCode, cancellationToken);
        }

        /// <summary>
        /// Analyzes the image using a domain-specific model.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="modelName">Name of the domain-specific model.</param>
        /// <param name="languageCode">The language code. If omitted, 'en' will be used.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <remarks>The list of currently available models can be listed via the <see cref="ListModelsAsync"/> method.</remarks>
        /// <returns>The AnalysisInDomainResult object.</returns>
        public Task<AnalysisInDomainResult> AnalyzeImageInDomainAsync(Stream imageStream, string modelName, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return AnalyzeImageInDomainCommonAsync(imageStream, modelName, languageCode, cancellationToken);
        }

        private async Task<AnalysisInDomainResult> AnalyzeImageInDomainCommonAsync<T>(T requestBody, string modelName, string languageCode, CancellationToken cancellationToken)
        {
            var requestUrl = new StringBuilder("/models/").Append(modelName).Append("/analyze?");
            requestUrl.Append(string.Join("&", new List<string>
            {
                LanguageCodeToQueryParam(languageCode)
            }
            .Where(s => !string.IsNullOrEmpty(s))));

            return await PostAsync<T, AnalysisInDomainResult>(requestUrl.ToString(), requestBody, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Lists the domain-specific image-analysis models.
        /// </summary>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>A ModelResult, containing an array of domain-specific models and associated information.</returns>
        public async Task<ModelResult> ListModelsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<ModelResult>("/models", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the description of an image.
        /// </summary>
        /// <param name="url">The image URL.</param>
        /// <param name="maxCandidates">Maximum number of candidates to return.  Defaults to 1.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>An AnalysisResult object with an image description.</returns>
        public Task<AnalysisResult> DescribeAsync(string url, int maxCandidates = 1, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DescribeAsync(url, string.Empty, maxCandidates, cancellationToken);
        }

        /// <summary>
        /// Gets the description of an image.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="languageCode">Optional language; please refer to online documentation for supported languages.</param>
        /// <param name="maxCandidates">Maximum number of candidates to return.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>An AnalysisResult object with an image description.</returns>
        public Task<AnalysisResult> DescribeAsync(string imageUrl, string languageCode, int maxCandidates = 1, CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestBody = new UrlRequest() { url = imageUrl };

            return DescribeCommonAsync(requestBody, languageCode, maxCandidates, cancellationToken);
        }

        /// <summary>
        /// Gets the description of an image.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="maxCandidates">Maximum number of candidates to return.  Defaults to 1.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>An AnalysisResult object with an image description.</returns>
        public Task<AnalysisResult> DescribeAsync(Stream imageStream, int maxCandidates = 1, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DescribeAsync(imageStream, string.Empty, maxCandidates, cancellationToken);
        }

        /// <summary>
        /// Gets the description of an image.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="languageCode">language; please refer to online documentation for supported languages.</param>
        /// <param name="maxCandidates">Maximum number of candidates to return.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>An AnalysisResult object with an image description.</returns>
        public Task<AnalysisResult> DescribeAsync(Stream imageStream, string languageCode, int maxCandidates = 1, CancellationToken cancellationToken = default(CancellationToken))
        {
            return DescribeCommonAsync(imageStream, languageCode, maxCandidates, cancellationToken);
        }

        private async Task<AnalysisResult> DescribeCommonAsync<T>(T requestBody, string languageCode, int maxCandidates, CancellationToken cancellationToken)
        {
            var requestUrl = new StringBuilder("/describe?");
            requestUrl.Append(string.Join("&", new List<string>
            {
                string.Format(CultureInfo.InvariantCulture, "maxCandidates={0}", maxCandidates),
                LanguageCodeToQueryParam(languageCode)
            }
            .Where(s => !string.IsNullOrEmpty(s))));

            return await PostAsync<T, AnalysisResult>(requestUrl.ToString(), requestBody, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="imageUrl">The URL.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="smartCropping">if set to <c>true</c> [smart cropping].</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>Image bytes</returns>
        public Task<byte[]> GetThumbnailAsync(string imageUrl, int width, int height, bool smartCropping = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestBody = new UrlRequest() { url = imageUrl };

            return GetThumbnailCommonAsync(requestBody, width, height, smartCropping, cancellationToken);
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="smartCropping">if set to <c>true</c> [smart cropping].</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>Image bytes</returns>
        public Task<byte[]> GetThumbnailAsync(Stream stream, int width, int height, bool smartCropping = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetThumbnailCommonAsync(stream, width, height, smartCropping, cancellationToken);
        }

        private async Task<byte[]> GetThumbnailCommonAsync<T>(T requestBody, int width, int height, bool smartCropping = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            string requestUrl = new StringBuilder("/generateThumbnail?width=")
                .Append(width.ToString(CultureInfo.InvariantCulture))
                .Append("&height=")
                .Append(height.ToString(CultureInfo.InvariantCulture))
                .Append("&smartCropping=")
                .Append(smartCropping)
                .ToString();

            return await PostAsync<T, byte[]>(requestUrl, requestBody, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Recognizes the text asynchronous.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="detectOrientation">if set to <c>true</c> [detect orientation].</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>The OCR object.</returns>
        public Task<OcrResults> RecognizeTextAsync(string imageUrl, string languageCode = LanguageCodes.AutoDetect, bool detectOrientation = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestBody = new UrlRequest() { url = imageUrl };

            return RecognizeTextCommonAsync(requestBody, languageCode, detectOrientation, cancellationToken);
        }

        /// <summary>
        /// Recognizes the text asynchronous.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="detectOrientation">if set to <c>true</c> [detect orientation].</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>The OCR object.</returns>
        public Task<OcrResults> RecognizeTextAsync(Stream imageStream, string languageCode = LanguageCodes.AutoDetect, bool detectOrientation = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            return RecognizeTextCommonAsync(imageStream, languageCode, detectOrientation, cancellationToken);
        }

        private async Task<OcrResults> RecognizeTextCommonAsync<T>(T requestBody, string languageCode, bool detectOrientation, CancellationToken cancellationToken = default(CancellationToken))
        {
            string requestUrl = new StringBuilder("/ocr?detectOrientation=")
                .Append(detectOrientation)
                .Append("&")
                .Append(LanguageCodeToQueryParam(languageCode))
                .ToString();

            return await PostAsync<T, OcrResults>(requestUrl, requestBody, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Recognizes the text asynchronously.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="mode">The recognition mode.</param>
        /// <param name="languageCode">Optional language code; only English is currently supported.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>TextRecognitionOperation created</returns>
        public Task<TextRecognitionOperation> CreateTextRecognitionOperationAsync(string imageUrl, TextRecognitionMode mode, string languageCode = LanguageCodes.English, CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestBody = new UrlRequest() { url = imageUrl };

            return CreateTextRecognitionOperationCommonAsync(requestBody, mode, cancellationToken);
        }

        /// <summary>
        /// Recognizes the text asynchronously.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="mode">The recognition mode.</param>
        /// <param name="languageCode">Optional language code; only English is currently supported.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>TextRecognitionOperation created</returns>
        public Task<TextRecognitionOperation> CreateTextRecognitionOperationAsync(Stream imageStream, TextRecognitionMode mode, string languageCode = LanguageCodes.English, CancellationToken cancellationToken = default(CancellationToken))
        {
            return CreateTextRecognitionOperationCommonAsync(imageStream, mode, cancellationToken);
        }

        private async Task<TextRecognitionOperation> CreateTextRecognitionOperationCommonAsync<T>(T requestBody, TextRecognitionMode mode, CancellationToken cancellationToken = default(CancellationToken))
        {
            string requestUrl = new StringBuilder("/recognizeText?mode=")
                .Append(mode.ToString())
                .ToString();

            return await PostAsync<T, TextRecognitionOperation>(requestUrl, requestBody, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the TextRecognitionOperationResult
        /// </summary>
        /// <param name="operation">TextRecognitionOperationResult object</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>TextRecognitionOperationResult</returns>
        public async Task<TextRecognitionOperationResult> GetTextRecognitionOperationResultAsync(TextRecognitionOperation operation, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<TextRecognitionOperationResult>(operation.Url, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the tags associated with an image.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="languageCode">The language code. If omitted, 'en' will be used.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>Analysis result with tags.</returns>
        public Task<AnalysisResult> GetTagsAsync(Stream imageStream, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetTagsCommonAsync(imageStream, languageCode, cancellationToken);
        }

        /// <summary>
        /// Gets the tags associated with an image.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="languageCode">The language code. If omitted, 'en' will be used.</param>
        /// <param name="cancellationToken">Optional request cancellation token.</param>
        /// <returns>Analysis result with tags.</returns>
        public Task<AnalysisResult> GetTagsAsync(string imageUrl, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestBody = new UrlRequest() { url = imageUrl };

            return GetTagsCommonAsync(requestBody, languageCode, cancellationToken);
        }

        private async Task<AnalysisResult> GetTagsCommonAsync<T>(T requestBody, string languageCode, CancellationToken cancellationToken)
        {
            var requestUrl = new StringBuilder("/tag?").Append(LanguageCodeToQueryParam(languageCode)).ToString();

            return await PostAsync<T, AnalysisResult>(requestUrl, requestBody, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Joins the visual features array to a string.
        /// </summary>
        /// <param name="features">String array of feature names.</param>
        /// <returns>The visual features query parameter string.</returns>
        private string VisualFeaturesToQueryParam(string[] features)
        {
            return (features == null || features.Length == 0)
                ? string.Empty
                : "visualFeatures=" + string.Join(",", features);
        }

        /// <summary>
        /// Variable-length VisualFeatures arguments to a string.
        /// </summary>
        /// <param name="features">Variable-length VisualFeatures.</param>
        /// <returns>The visual features query parameter string.</returns>
        private string VisualFeaturesToString(IEnumerable<VisualFeature> features)
        {
            return VisualFeaturesToQueryParam(features?.Select(feature => feature.ToString()).ToArray());
        }

        /// <summary>
        /// Strings the array to string.
        /// </summary>
        /// <param name="features">String array of feature names.</param>
        /// <returns>The visual features query parameter string.</returns>
        private string DetailsToQueryParam(IEnumerable<string> details)
        {
            return (details == null || details.Count() == 0)
                ? string.Empty
                : "details=" + string.Join(",", details);
        }

        /// <summary>
        /// Convert to optional languageCode argument to a query parameter
        /// </summary>
        /// <param name="languageCode">Optional language code.</param>
        /// <returns>The language query parameter string.</returns>
        private string LanguageCodeToQueryParam(string languageCode)
        {
            return string.IsNullOrEmpty(languageCode) ? string.Empty : "language=" + languageCode;
        }
    }
}
