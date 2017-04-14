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

using System.Collections.Generic;

namespace Microsoft.ProjectOxford.Vision.Contract
{
    /// <summary>
    /// The class of polygon
    /// </summary>
    public class Polygon
    {
        /// <summary>
        /// Constructor of the class
        /// </summary>
        public Polygon()
        {
            Points = new List<Point>();
        }

        /// <summary>
        /// Gets and sets the points of polygon
        /// </summary>
        /// <value>
        /// The points of polygon
        /// </value>
        public List<Point> Points { get; set; }

        /// <summary>
        /// Get a polygon object from boundingbox array
        /// </summary>
        /// <param name="boundingBox"> The boundingBox array</param>
        /// <returns>The polygon</returns>
        public static Polygon FromArray(int[] boundingBox)
        {
            Polygon polygon = new Polygon();

            for (int i = 0; i + 1 < boundingBox.Length; i += 2)
            {
                polygon.Points.Add(new Point() { X = boundingBox[i], Y = boundingBox[i + 1] });
            }

            return polygon;
        }
    }
}
