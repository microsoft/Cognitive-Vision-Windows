using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ProjectOxford.Vision.Contract
{
    public class HandwritingOCRResult
    {
        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        /// <value>
        /// The regions.
        /// </value>
        public HandwritingOCRLine[] Lines { get; set; }
    }
}
