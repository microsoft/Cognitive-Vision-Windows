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

using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.ProjectOxford.Vision.Contract;
using System.Collections.Generic;
using System;

namespace Microsoft.ProjectOxford.Vision
{
    /// <summary>
    /// Vision service client interfaces.
    /// </summary>
    public interface IVisionServiceClient
    {
        /// <summary>
        /// Analyzes the image.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="visualFeatures">The visual features. If none are specified, 'Categories' will be analyzed.</param>
        /// <param name="languageCode">Optional language code.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The AnalysisResult object.</returns>
        Task<AnalysisResult> AnalyzeImageAsync(string url, string[] visualFeatures = null, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Analyzes the image.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="visualFeatures">The visual features. If none are specified, 'Categories' will be analyzed.</param>
        /// <param name="languageCode">Optional language code.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The AnalysisResult object.</returns>
        Task<AnalysisResult> AnalyzeImageAsync(Stream imageStream, string[] visualFeatures = null, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Analyzes the image.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="visualFeatures">The visual features. If none are specified, VisualFeatures.Categories will be analyzed.</param>
        /// <param name="details">Optional domain-specific models to invoke when appropriate.  To obtain names of models supported, invoke the <see cref="ListModelsAsync">ListModelsAsync</see> method.</param>
        /// <param name="languageCode">Optional language code.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The AnalysisResult object.</returns>
        Task<AnalysisResult> AnalyzeImageAsync(string url, IEnumerable<VisualFeature> visualFeatures = null, IEnumerable<string> details = null, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Analyzes the image.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="visualFeatures">The visual features. If none are specified, VisualFeatures.Categories will be analyzed.</param>
        /// <param name="details">Optional domain-specific models to invoke when appropriate.  To obtain names of models supported, invoke the <see cref="ListModelsAsync">ListModelsAsync</see> method.</param>
        /// <param name="languageCode">Optional language code.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The AnalysisResult object.</returns>
        Task<AnalysisResult> AnalyzeImageAsync(Stream imageStream, IEnumerable<VisualFeature> visualFeatures = null, IEnumerable<string> details = null, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Analyzes the image using a domain-specific image analysis model.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="model">Model representing the domain.</param>
        /// <param name="languageCode">Optional language code.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The AnalysisResult object.</returns>
        Task<AnalysisInDomainResult> AnalyzeImageInDomainAsync(string url, Model model, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Analyzes the image using a domain-specific image analysis model.
        /// </summary>
        /// <param name="stream">The image stream.</param>
        /// <param name="modelName">Name of the model.</param>
        /// <param name="languageCode">Optional language code.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The AnalysisResult object.</returns>
        Task<AnalysisInDomainResult> AnalyzeImageInDomainAsync(Stream imageStream, Model model, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Analyzes the image using a domain-specific image analysis model.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="modelName">Name of the model.</param>
        /// <param name="languageCode">Optional language code.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The AnalysisResult object.</returns>
        Task<AnalysisInDomainResult> AnalyzeImageInDomainAsync(string url, string modelName, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Analyzes the image using a domain-specific image analysis model.
        /// </summary>
        /// <param name="stream">The image stream.</param>
        /// <param name="modelName">Name of the model.</param>
        /// <param name="languageCode">Optional language code.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The AnalysisResult object.</returns>
        Task<AnalysisInDomainResult> AnalyzeImageInDomainAsync(Stream imageStream, string modelName, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Lists domain-specific models currently supported.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>A ModelResult, containing an array of domain-specific models and associated information.</returns>
        Task<ModelResult> ListModelsAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a description of an image.
        /// </summary>
        /// <param name="url">The URL of the image.</param>
        /// <param name="maxCandidates">Maximum candidates to return (default=1).</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>AnalysisResult containing a natural-language description of the image and associated tags.</returns>
        Task<AnalysisResult> DescribeAsync(string url, int maxCandidates = 1, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a description of an image.
        /// </summary>
        /// <param name="url">The URL of the image.</param>
        /// <param name="maxCandidates">Maximum candidates to return (default=1).</param>
        /// <param name="languageCode">Optional language; please refer to online documentation for supported languages.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>AnalysisResult containing a natural-language description of the image and associated tags.</returns>
        Task<AnalysisResult> DescribeAsync(string url, string languageCode, int maxCandidates = 1, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a description of an image.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="maxCandidates">Maximum candidates to return (default=1).</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>AnalysisResult containing a natural-language description of the image and associated tags.</returns>
        Task<AnalysisResult> DescribeAsync(Stream imageStream, int maxCandidates = 1, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a description of an image.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="maxCandidates">Maximum candidates to return (default=1).</param>
        /// <param name="languageCode">Optional language; please refer to online documentation for supported languages.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>AnalysisResult containing a natural-language description of the image and associated tags.</returns>
        Task<AnalysisResult> DescribeAsync(Stream imageStream, string languageCode, int maxCandidates = 1, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="smartCropping">if set to <c>true</c> [smart cropping].</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The byte array.</returns>
        Task<byte[]> GetThumbnailAsync(string url, int width, int height, bool smartCropping = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="smartCropping">if set to <c>true</c> [smart cropping].</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The byte array.</returns>
        Task<byte[]> GetThumbnailAsync(Stream stream, int width, int height, bool smartCropping = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Recognizes the text.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="languageCode">Optional language code.</param>
        /// <param name="detectOrientation">if set to <c>true</c> [detect orientation].</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The OCR object.</returns>
        Task<OcrResults> RecognizeTextAsync(string imageUrl, string languageCode = LanguageCodes.AutoDetect, bool detectOrientation = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Recognizes the text.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="languageCode">Optional language code.</param>
        /// <param name="detectOrientation">if set to <c>true</c> [detect orientation].</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The OCR object.</returns>
        Task<OcrResults> RecognizeTextAsync(Stream imageStream, string languageCode = LanguageCodes.AutoDetect, bool detectOrientation = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Starts a text recognition operation.
        /// </summary>
        /// <param name="imageUrl">Image url</param>
        /// <param name="mode">The recognition mode.</param>
        /// <param name="languageCode">Optional language code; only English is currently supported.</param>
        /// <returns>TextRecognitionOperation created</returns>
        Task<TextRecognitionOperation> CreateTextRecognitionOperationAsync(string imageUrl, TextRecognitionMode mode, string languageCode = LanguageCodes.English, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Starts a text recognition operation.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="mode">The recognition mode.</param>
        /// <param name="languageCode">Optional language code; only English is currently supported.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>TextRecognitionOperation created</returns>
        Task<TextRecognitionOperation> CreateTextRecognitionOperationAsync(Stream imageStream, TextRecognitionMode mode, string languageCode = LanguageCodes.English, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get TextRecognitionOperationResult
        /// </summary>
        /// <param name="operation">TextRecognitionOperation object</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>TextRecognitionOperationResult</returns>
        Task<TextRecognitionOperationResult> GetTextRecognitionOperationResultAsync(TextRecognitionOperation operation, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the tags associated with an image.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="languageCode">Optional language code.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Analysis result with tags.</returns>
        Task<AnalysisResult> GetTagsAsync(string imageUrl, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the tags associated with an image.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="languageCode">Optional language code.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Analysis result with tags.</returns>
        Task<AnalysisResult> GetTagsAsync(Stream imageStream, string languageCode = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
