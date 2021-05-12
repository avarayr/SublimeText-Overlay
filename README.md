## Sublime Overlay

# :no_entry: THIS PROJECT IS DEPRECATED AND NO LONGER MAINTANED

![img](https://img.shields.io/badge/version-stable-lightgrey.svg)
![img](https://img.shields.io/github/issues/mikadev001/SublimeText-Overlay.svg)


![img](http://i.imgur.com/tmJlAUj.png)

**Warning: Tested only on Windows 10 with Sublime Text 3.x. Please open an issue if it's working incorrectly.**

Ever wanted to make Sublime Text borderless like OS X?
### :fire: [Download Latest Version](https://github.com/mikadev001/SublimeText-Overlay/releases/latest)
***SublimeOverlay is currently under MIT License. See LICENSE for more information***
# Features:

* Adjustable offset of inner window.
* Togglable title text
* Adjustable primary color for overlay

*Note:*  These settings are accessible through options window (button in top right corner)
<h4>For best experience, hide the menu from View->Hide Menu. It will still be available by pressing Alt key</h4>
# Requirements:
* [.NET Framework 4.5+](https://www.microsoft.com/en-us/download/details.aspx?id=42642)
* Windows 7+

# Usage:

Open an instance of Sublime Text and run the executable of SublimeOverlay.  


 Currently supports only 1 instance of editor. 

## Auto run editor on application startup
* Create a shortcut and pass the following arguments
  * ```--startsublime``` - most likely it will automatically find your sublime path and run it.
  * ```--sublime-path``` - Use this in case of application not detecting your Sublime installation path

_Always use quotes for_ ```--sublime-path```

Example arguments: ```--startsublime --sublime-path="C:\MySublime\sublime_text.exe"```

------
# FAQ: 

 * Q: My editor borders are visible 
 * A: **Adjust the top/left offsets in settings form.**
 

 * Q: Sublime Text **is not starting** after closing the SublimeOverlay instance.
 * A: *Most likely Sublime Text is in your processes*, use your task manager to kill the instance and try again, **or open RUN/CMD and type `taskkill /f /im sublime_text.exe`  (warning: this will kill all sublime instances open)**
 
Credits for icon goes to [Salvatore Gentile](https://dribbble.com/shots/2273297-Sublime-Text-Icon)
## My Sublime Configuration: 
* Theme - Material (Dark)
* Color scheme - Dark-Dracula (SL)
* Font face: BitStream Vera Sans Mono
