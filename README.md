## Jin.FileServer
Justify your images on you file server dynamicly.
#### Demo
![avatar](https://raw.githubusercontent.com/jinchou/Jin.FileServer/master/%E6%BC%94%E7%A4%BA.gif)
## 1. How to use
#### 1.1 Installation
    Install-Package Jin.FileServer
    Gihub address: https://github.com/jinchou/Jin.FileServer
#### 1.2 Configuration
    app.UseJinFileServer(env);
It's that simple, one line of code is fixed.
#### 1.3 Experience a hand
    Make sure there is an image in the project wwwroot folder.
    Suppose the image address is:
        Https://localhost:44370/upload/201806/1.png
    Then, use a browser to access:
        Https://localhost:44370/upload/201806/1.png_200x200.png
Note: This will be in the original directory under the name of 1.png_200x200.png, the size of the 200x200 png file,
Then output to the client!

Visit this address again, this image will be output directly to the client browser as it already exists.
## 2. Image generation parameters
An address hosted by JinFileServer consists of: original image address + image generation parameters.
#### 2.1 Image generation parameter format:
    _width x height converted format
Such as: _200x200.png
The full address is as follows:
    Https://localhost:44370/upload/201806/1.png_200x200.png
#### 2.2 Advanced Skills
    _200.png means to generate a picture with a width and height of 200, and ensure that the background color is automatically filled without distortion.
    _200x.png means to generate a picture with a width of 200 and a height that is automatically scaled.
    _x200.png indicates that the height is 200 and the width is automatically scaled.
    _200x480.png means to generate a picture with a width of 200 and a height of 480, and ensure that the background color is automatically filled without distortion.
## 3. Configuration instructions
#### 3.1 Specify image extensions
    pp.UseJinFileServer(env, opt => {
       opt.ImageFormats= new List<string> {
           ".bmp", ".gif", ".jpg", ".jpeg", ".png"
       };
    });
Note: It is also the default value.
The extension of the above configuration will be automatically managed by JinFileServer when accessing. When the accessed address has the above suffix and has image generation parameters, JinFileServer will automatically generate the specified image.
#### 3.2 Specify the image size that can be generated
    app.UseJinFileServer(env, opt => {
         opt.ImageSizes = new List<Size>
         {
             //80, 100, 200, 480, 500, 600, 800
             New Size(80,80),
             New Size(100,100),
             New Size(200,200),
             New Size(480,480),
             New Size(500,500),
             New Size(600,600),
             New Size(800,800),
         };
    });
Note: It is also the default value.
There is no configured size, JinFileServer will not do any processing when accessing.
## Upgrade history
#### v2.1
    Internal algorithm adjustment, doc optimization.
