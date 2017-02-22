using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ProjectOxford.Vision.Contract
{
    public class HandwritingOCROperationResult
    {
        public HandwritingOCROperationStatus StatusCode { get; set; }
        public HandwritingOCRResult RecognitionResult { get; set; }
    }

    //[Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HandwritingOCROperationStatus
    {
        NotStarted = 0,

        Running = 1,

        Succeeded = 2,

        Failed = 3,
    }
}
