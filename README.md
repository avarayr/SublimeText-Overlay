### Sublime Overlay [ BETA ]
![img](http://i.imgur.com/tmJlAUj.png)

**Warning: Tested only on Windows 10 with Sublime Text 3.x. Please open an issue if it's working incorrectly.**

Ever wanted to make Sublime Text borderless like OS X?
# Features:

* Adjustable offset of inner window.
* Togglable title text
* Adjutable primary color for overlay

*Note:*  These settings are accesable through options window (button in top right corner)
# Requirements:
* [.NET Framework 4.5+](https://www.microsoft.com/en-us/download/details.aspx?id=42642)
* Windows 7+

# Usage:

Open an instance of Sublime Text and run the executable of SublimeOverlay.  
Currently supports only 1 instance of editor.
------
# FAQ: 

 * Q: My editor borders are visible 
 * A: **Adjust the top/left offsets in settings form.**
 

 * Q: Sublime Text **is not starting** after closing the SublimeOverlay instance.
 * A: *Most likely Sublime Text is in your processes*, use your task manager to kill the instance and try again, **or open RUN/CMD and type `taskkill /f /im sublime_text.exe`  (warning: this will kill all sublime instances open)**