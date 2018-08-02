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

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace VisionAPI_WPF_Samples
{
    public sealed class RecognizeLanguage
    {
        public static readonly RecognizeLanguage UNK = new RecognizeLanguage("unk", OcrLanguages.Unk, "AutoDetect");
        public static readonly RecognizeLanguage ZH = new RecognizeLanguage("zh", OcrLanguages.ZhHans, "Chinese");
        public static readonly RecognizeLanguage AR = new RecognizeLanguage("ar", OcrLanguages.Ar, "Arabic");
        public static readonly RecognizeLanguage ZH_CN = new RecognizeLanguage("zh-Hans", OcrLanguages.ZhHans, "Chinese (Simplified)");
        public static readonly RecognizeLanguage ZH_TW = new RecognizeLanguage("zh-Hant", OcrLanguages.ZhHant, "Chinese (Traditional)");
        public static readonly RecognizeLanguage CS = new RecognizeLanguage("cs", OcrLanguages.Cs, "Czech");
        public static readonly RecognizeLanguage DA = new RecognizeLanguage("da", OcrLanguages.Da, "Danish");
        public static readonly RecognizeLanguage NL = new RecognizeLanguage("nl", OcrLanguages.Nl, "Dutch");
        public static readonly RecognizeLanguage EN = new RecognizeLanguage("en", OcrLanguages.En, "English");
        public static readonly RecognizeLanguage FI = new RecognizeLanguage("fi", OcrLanguages.Fi, "Finnish");
        public static readonly RecognizeLanguage FR = new RecognizeLanguage("fr", OcrLanguages.Fr, "French");
        public static readonly RecognizeLanguage DE = new RecognizeLanguage("de", OcrLanguages.De, "German");
        public static readonly RecognizeLanguage EL = new RecognizeLanguage("el", OcrLanguages.El, "Greek");
        public static readonly RecognizeLanguage HU = new RecognizeLanguage("hu", OcrLanguages.Hu, "Hungarian");
        public static readonly RecognizeLanguage IT = new RecognizeLanguage("it", OcrLanguages.It, "Italian");
        public static readonly RecognizeLanguage JA = new RecognizeLanguage("ja", OcrLanguages.Ja, "Japanese");
        public static readonly RecognizeLanguage KO = new RecognizeLanguage("ko", OcrLanguages.Ko, "Korean");
        public static readonly RecognizeLanguage NB = new RecognizeLanguage("nb", OcrLanguages.Nb, "Norwegian");
        public static readonly RecognizeLanguage PL = new RecognizeLanguage("pl", OcrLanguages.Pl, "Polish");
        public static readonly RecognizeLanguage PT = new RecognizeLanguage("pt", OcrLanguages.Pt, "Portuguese");
        public static readonly RecognizeLanguage RO = new RecognizeLanguage("ro", OcrLanguages.Ro, "Romanian");
        public static readonly RecognizeLanguage RU = new RecognizeLanguage("ru", OcrLanguages.Ru, "Russian");
        public static readonly RecognizeLanguage SR_CYRL = new RecognizeLanguage("sr-Cyrl", OcrLanguages.SrCyrl, "Serbian (Cyrillic)");
        public static readonly RecognizeLanguage SR_LATN = new RecognizeLanguage("sr-Latn", OcrLanguages.SrLatn, "Serbian (Latin)");
        public static readonly RecognizeLanguage SK = new RecognizeLanguage("sk", OcrLanguages.Sk, "Slovak");
        public static readonly RecognizeLanguage ES = new RecognizeLanguage("es", OcrLanguages.Es, "Spanish");
        public static readonly RecognizeLanguage SV = new RecognizeLanguage("sv", OcrLanguages.Sv, "Swedish");
        public static readonly RecognizeLanguage TR = new RecognizeLanguage("tr", OcrLanguages.Tr, "Turkish");

        public string ShortCode { get; }

        public OcrLanguages OcrEnum { get; }

        public string LongName { get; }

        internal RecognizeLanguage(string shortCode, OcrLanguages ocrEnum, string longName)
        {
            ShortCode = shortCode;
            OcrEnum = ocrEnum;
            LongName = longName;
        }
    }
}
