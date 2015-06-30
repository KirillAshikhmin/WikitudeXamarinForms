## Setup Guide Xamarin Component

#### Introduction to the Augmented Reality View
From a technical point of view the SDK adds a UI component called **ARchitectView**, similar to a web view. In contrast to a standard web view the AR View can render augmented reality content.

Content developed for the AR View is written in JavaScript and HTML. 

**Note** In this guide we use the terminology `augmented reality experience` and `ARchitect World` synonymously - both of them refer to augmented reality parts of your application. 

<a id="XamarinSampleApp"></a>
### Wikitude Sample App - Getting Started quickly

Wikitude offers a fully-configured example application as Xamarin app. The application demonstrates the usage of the Wikitude Xamarin Component as well as the usage of the `ARchitectView`. Furthermore the application includes several augmented reality examples which are described in more detail in the [Example section](http://www.wikitude.com/external/doc/documentation/latest/xamarin/samples.html) of this documentation.

#### Automatic Setup of Sample App

1. Clone or download our [GitHub](http://github.com/wikitude/wikitude-xamarin) repository.

2. Open the example application that is located in the `source/sample/{Android/iOS}` folder.

3. Select Debug/Release and click `Run`. Please make sure that you have selected the example application and not only the Component project. Otherwise only the Component project will be build and not the example application.

### Using the Wikitude SDK to create your own augmented reality experience

Now that the Wikitude SDK is added you can start using it inside the Xamarin application to load and display your own augmented reality experience. To communicate between the Architect World and the Xamarin application we added a custom `architectsdk://` protocol and a native callback mechanism. Please refer to the example application on how to use them.
 

##### Loading your augmented reality experience

To load an Architect World simply use the `LoadArchitectWorldFromUrl` method provided by the Wikitude SDK / Xamarin Component. It accepts either an URL pointing to the application bundle or on a remote server. 

On iOS, also the `Start` and `Stop` methods need to be called either when the presented view controller is about to be replaced by another one or the application moves into the background/foreground. Please refer to the example application on how to use both methods and when.

##### Adding communication between Xamarin and ARchitect

In order to communicate with the augmented reality experience from within the Xamarin application the Wikitude SDK offers the `CallJavaScript(string javaScript)` method. The JavaScript will be evaluated in the context of the augmented reality experience. If the augmented reality experience is not fully loaded at the time this function is called, it will be evaluated as soon at the experience is loaded. 

The Wikitude SDK defines a custom URL protocol which can be used to communicate from the ARchitect World to the Xamarin application. You can set a C# method that is called each time a `document.location = architectsdk://` load request is done. Please refer to the iOS/Android documentation or API reference for more information about the platform specific implementation details. Also the Wikitude Xamarin example application uses the above mentioned URL protocol to communicate between the Architect World and the Xamarin application.

The Wikitude Component offers more functionality like to inject a custom location or to capture the screen and generate an image that can be shared on Twitter or Facebook. Please also refer to the [platform API documentation and reference](http://www.wikitude.com/external/doc/documentation/latest/xamarin/referencexamarin.html#xamarin-component-reference) for more information about the available APIs.
  

### Further developer resources
* <a href="http://www.wikitude.com/developer/documentation/xamarin" target="_top">Full documentation and additional tutorials</a>
* <a href="http://www.wikitude.com/developer/developer-forum" target="_top">Developer Forum</a>
* <a href="http://www.wikitude.com/download" target="_top">Wikitude SDK Download</a>
* <a href="https://plus.google.com/u/0/103004921345651739447/posts" target="_top">Google+ Page for News</a>
* <a href="http://www.wikitude.com/newsletter" target="_top">Wikitude Newsletter</a>
