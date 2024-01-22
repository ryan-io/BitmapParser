<a name="readme-top"></a>
<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->


<!-- PROJECT LOGO -->

<p align="center">
<img width="300" height="125" src="https://i.imgur.com/w5hcUtR.png">
</p>

<div align="center">

<h3 align="center">Bitmap Parser</h3>

  <p align="center">
    A fast bitmap parser. Uses GDI+ through System.Drawing.
    <br />
    <br />
    <a href="https://github.com//ryan-io/CommandPipeline/issues">Report Bug</a>
    ·
    <a href="https://github.com//ryan-io/CommandPipeline/issues">Request Feature</a>
  </p>
</div>

---
<!-- TABLE OF CONTENTS -->

<details align="center">
  <summary>Table of Contents</summary>
  <ol>
  <li>
      <a href="#overview">Overview</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started & Usage</a>
      <ul>
        <li><a href="#prerequisites-and-dependencies">Prerequisites and Dependencies</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments-and-credit">Acknowledgments</a></li>
  </ol>
</details>

---

<!-- ABOUT THE PROJECT -->

# Overview

This project came to fruition in my current role and is used in two internal tools for annotating photos. The goal was to make a fast parser. This is done using access to GDI+ through System.Drawing.Common. 

##### Features

<ol>
<li>
Get all images in a directory or directories asynchronously. 
</li>
<li>
Batch process or modify each photo individually based on criteria you define.
</li>
<li>
Save the modified photos.
</li>
</ol>

<p align="right">(<a href="#readme-top">back to top</a>)</p>

# Built With
- JetBrains Rider
- Tested with WFP & ScottPlot

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- GETTING STARTED -->
# Getting Started & Usage

```csharp
using RIO.BCL.Parsing;  
  
// for creating a parser, define an array of image locations  
var paths = new[] { @"C:\img1.png", @"C:\img2.jpg" };  
  
// for creating an image grabber that gets all images in a folder  
const string folder = @"C:\your-folder\";  
  
// create an instance of BitmapParser  
var parser = new BitmapParser(ref paths);  
  
// create an instance of ImageGrabber  
var grabber = await ImageGrabber.CreateAsync(ImageType.ALL, folder);  
  
// modify a single image via parser  
parser.ModifyRgbUnsafeRef(0, ModifyFunctor);  
  
// modify many images via parser (images w/ indices 0, 3 and 4)  
parser.ModifyRgbUnsafeRef(ModifyFunctor, 0, 3, 4);  
  
//modify all images via parser  
parser.ModifyAllRgbUnsafeRef(ModifyFunctor);  
  
// getting images via parser  
var images = parser.GetAllBitmapsRef();  
ref var imagesRef = ref parser.GetAllBitmapsRef();  
  
// modify a single image via image grabber  
grabber.Parser.ModifyRgbUnsafeRef(0, ModifyFunctor);  
  
// modify many images via grabber (images w/ indices 0, 3 and 4)  
grabber.Parser.ModifyRgbUnsafeRef(ModifyFunctor, 0, 3, 4);  
  
//modify all images via grabber  
grabber.Parser.ModifyAllRgbUnsafeRef(ModifyFunctor);  
  
// getting images via image grabber  
grabber.GetAllBitmapsRef();  
grabber.Parser.GetAllBitmapsRef();  
var imagesGrabber = grabber.GetAllBitmapsRef();  
ref var imagesGrabberRef = ref grabber.Parser.GetAllBitmapsRef();  
  
// a modify 'functor' is a delegate that takes four reference parameters: index, red, green and blue  
// the core of the functor method should modify an image per logic appropriate for your application  
void ModifyFunctor(ref int pxlIndex, ref int red, ref int green, ref int blue)  
{  
    // modify the images RGB values however you see fit    
if (pxlIndex % 2 == 0)  
    {  
        red -= 25;  
        green = 10;  
        blue += 20;  
    }  
  
    red -= 25;  
    green += 10;  
    blue += 20;  
}
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>


# Prerequisites and Dependencies

- .NET 6
* C# 10
* Microsoft.Extensions.Logging.7.0.0

##### Please feel free to contact me with any issues or concerns in regards to the dependencies defined above. We can work around the majority of them if needed.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

# Installation

- Clone or fork this repository. Once done, add a reference to this library in your project
- Download the latest dll and create a reference to it in your project
- Install via NPM

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP -->
# Roadmap

There is currently no future features planned.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTRIBUTING -->
# Contributing

Contributions are absolutely welcome. This is an open source project. 

1. Fork the repository
2. Create a feature branch
```Shell
git checkout -b feature/your-feature-branch
```
3. Commit changes on your feature branch
```Shell
git commit -m 'Summary feature'
```
4. Push your changes to your branch
```Shell
git push origin feature/your-feature-branch
```
5. Open a pull request to merge/incorporate your feature

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->
# License

Distributed under the MIT License.

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- CONTACT -->
# Contact

<p align="center">
<b><u>RyanIO</u></b> 
<br/><br/> 
<a href = "mailto:ryan.io@programmer.net?subject=[RIO]%20Procedural%20Generator%20Project" >[Email]</a>
<br/>
[LinkedIn]
<br/>
<a href="https://github.com/ryan-io">[GitHub]</a></p>

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->
# Acknowledgments and Credit

* [Stephen Cleary's Blog](https://blog.stephencleary.com/)
	* In particular, his blog on [Async Events in OOP](https://blog.stephencleary.com/2013/02/async-oop-5-events.html) provided me with inspiration for this.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo_name.svg?style=for-the-badge
[contributors-url]: https://github.com/github_username/repo_name/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/github_username/repo_name.svg?style=for-the-badge
[forks-url]: https://github.com/github_username/repo_name/network/members
[stars-shield]: https://img.shields.io/github/stars/github_username/repo_name.svg?style=for-the-badge
[stars-url]: https://github.com/github_username/repo_name/stargazers
[issues-shield]: https://img.shields.io/github/issues/github_username/repo_name.svg?style=for-the-badge
[issues-url]: https://github.com/github_username/repo_name/issues
[license-shield]: https://img.shields.io/github/license/github_username/repo_name.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo_name/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/linkedin_username
[product-screenshot]: images/screenshot.png
[Next.js]: https://img.shields.io/badge/next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white
[Next-url]: https://nextjs.org/
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Vue.js]: https://img.shields.io/badge/Vue.js-35495E?style=for-the-badge&logo=vuedotjs&logoColor=4FC08D
[Vue-url]: https://vuejs.org/
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[Svelte.dev]: https://img.shields.io/badge/Svelte-4A4A55?style=for-the-badge&logo=svelte&logoColor=FF3E00
[Svelte-url]: https://svelte.dev/
[Laravel.com]: https://img.shields.io/badge/Laravel-FF2D20?style=for-the-badge&logo=laravel&logoColor=white
[Laravel-url]: https://laravel.com
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[JQuery.com]: https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[JQuery-url]: https://jquery.com
