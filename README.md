# PAUSE-Archive
Restored project file for V&amp;A comission : In the Pause Between the Ringing.


MAC Specific audio import fix : 

    In Unity Hub install Unity 2018.1.9f2
    In Unity Hub install Unity 2019.3.0f3 (or any version that can import Audio on macOS 10.15
    In Unity Hub, for both versions, click on the dot, dot, dot menu and choose Reveal in Finder
    In the Finder, for both version, right click on Unity.app and choose Show package contents
    In the Finder, for both versions, navigate to Contents > Tools > FSBTool
    In the Finder, for Unity 2019.3.0f3, Command+Click on FSBTool and libvorbis.dylib
    From the File menu, choose Copy
    Select the FSBTool folder in Unity 2018.1.9f2
    From the File menu, choose Paste
    In the Paste dialog, choose Apply to All
    In the Paste dialog, click Replace
    In the Finder, for Unity 2019.3.0f3, right click on libmp3lame.dylib
    From the context menu, choose Show original
    In the Finder, click the original libmp3lame.dylib
    From the File menu, choose Copy
    Select the FSBTool folder in Unity 2018.1.9.f2
    From the File menu, choose Paste
    In the Paste dialog, choose Apply to All
    In the Paste dialog, click Replace

