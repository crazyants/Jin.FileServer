
###### 使用Jin.FileServer为你的.net core网站创建文件服务器
JinFileServer，可以让你在只保存一张原图的情况下，
无侵入的给现有.net core项目添加动态生成图片功能，可以生成任意大小哦。

## 1. 使用方法
#### 1.1 安装
    nuget 搜索 Jin.FileServer安装。
#### 1.2 配置
    app.UseJinFileServer(env);
就是这么简单，一行代码搞定。
#### 1.3 体验一把
    保证项目wwwroot文件夹中有一张图片。
    假设图片地址是：
    https://localhost:44370/upload/201806/1.png
    那么，使用浏览器访问：
    https://localhost:44370/upload/201806/1.png_200x200.png
注：这样就会在原目录下面先生成名为1.png_200x200.png，大小为200x200的png文件，
然后输出到客户端！
    
再次访问这个地址，这个图片由于已存在，将直接输出到客户端浏览器。
## 2.图片生成参数
一个受JinFileServer托管的地址由：原图地址+图片生成参数构成。
#### 2.1 图片生成参数格式:
    _宽度 x 高度 扩展名 
如：_200x200.png  
完整地址如下：     
    https://localhost:44370/upload/201806/1.png_200x200.png     
## 3. 配置说明
#### 3.1 指定图片扩展名
    app.UseJinFileServer(env, opt => { 
        opt.ImageFormats= new List<string> { 
            ".bmp", ".gif", ".jpg", ".jpeg", ".png" 
        };
    });
注：也是默认值。
以上配置的扩展名，在访问的时候，会自动被JinFileServer托管，当访问的地址带有以上后缀时，且带有图片生成参数，那么JinFileServer会自动生成指定要求的图片。
#### 3.2 指定可生成的图片尺寸
    app.UseJinFileServer(env, opt => { 
         opt.ImageSizes = new List<Size>
         {
              new Size(80, 80),
              new Size(100, 100),
              new Size(200, 200),
              new Size(480, 480),
              new Size(500, 500),
              new Size(600, 600),
              new Size(800, 800),
         };
    });
注：也是默认值。    
没有配置的尺寸，在访问的时候，JinFileServer不会进行任何处理。     
但是，如果没有配置任何尺寸，JinFileServer将允许生成任何大小的图片，慎用。
## 4. 记录
1. 根据webp后缀，自动缩放文件大小 ags.jpg_500x500.jpg 加_是为了把真实路径区分开。
2. 支持防盗链
3. 支持一般文件，支持图片
4. 支持设定某些尺寸可以设置为自动调整
## 5. 展望
    下个版本，可支持防盗链。