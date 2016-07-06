# Microsoft Computer Vision API: Windows Client Library & Sample
This repo contains the Windows client library & sample for the Microsoft Computer Vision API, an offering within [Microsoft Cognitive Services](https://www.microsoft.com/cognitive-services), formerly known as Project Oxford.
* [Learn about the Computer Vision API](https://www.microsoft.com/cognitive-services/en-us/computer-vision-api)
* [Read the documentation](https://www.microsoft.com/cognitive-services/en-us/computer-vision-api/documentation)
* [Find more SDKs & Samples](https://www.microsoft.com/cognitive-services/en-us/SDK-Sample?api=computer%20vision)


## The Client Library
The client library is a thin C\# client wrapper for the Computer Vision API.

The easiest way to use this client library is to get microsoft.projectoxford.vision package from [nuget](<http://nuget.org>). Please go to [Vision API Package in nuget](https://www.nuget.org/packages/Microsoft.ProjectOxford.Vision/) for more details.

## The Sample
This sample is a Windows WPF application to demonstrate the use of the Computer Vision API.

### Build the sample
 1. Starting in the folder where you clone the repository (this folder)

 2. In a git command line tool, type `git submodule init` (or do this through a UI)

 3. Pull in the shared Windows code by calling `git submodule update`

 4. Start Microsoft Visual Studio 2015 and select `File > Open > Project/Solution`.

 5. Starting in the folder where you clone the repository, go to `Vision > Windows > Sample-WPF` Folder.

 6. Double-click the Visual Studio 2015 Solution (.sln) file VisionAPI-WPF-Samples.

 7. Press Ctrl+Shift+B, or select `Build > Build Solution`.

### Run the sample
After the build is complete, press F5 to run the sample.

First, you must obtain a Vision API subscription key by [following the instructions on our website](<https://www.microsoft.com/cognitive-services/en-us/sign-up>).

Locate the text edit box saying "Paste your subscription key here to start" on
the top right corner. Paste your subscription key. You can choose to persist
your subscription key in your machine by clicking "Save Key" button. When you
want to delete the subscription key from the machine, click "Delete Key" to
remove it from your machine.

Microsoft will receive the images you upload and may use them to improve the Computer Vision
API and related services. By submitting an image, you confirm you have consent
from everyone in it.

<img src="SampleScreenshots/SampleRunning1.png" width="80%"/>


## Contributing
We welcome contributions. Feel free to file issues and pull requests on the repo and we'll address them as we can. Learn more about how you can help on our [Contribution Rules & Guidelines](</CONTRIBUTING.md>). 

You can reach out to us anytime with questions and suggestions using our communities below:
 - **Support questions:** [StackOverflow](<https://stackoverflow.com/questions/tagged/microsoft-cognitive>)
 - **Feedback & feature requests:** [Cognitive Services UserVoice Forum](<https://cognitive.uservoice.com>)

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.


## License
All Microsoft Cognitive Services SDKs and samples are licensed with the MIT License. For more details, see
[LICENSE](</LICENSE.md>).

Sample images are licensed separately, please refer to [LICENSE-IMAGE](</LICENSE-IMAGE.md>).


## Developer Code of Conduct
Developers using Cognitive Services, including this client library & sample, are expected to follow the “Developer Code of Conduct for Microsoft Cognitive Services”, found at [http://go.microsoft.com/fwlink/?LinkId=698895](http://go.microsoft.com/fwlink/?LinkId=698895).
