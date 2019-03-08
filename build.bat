@echo off
rem This is only a quick building script for my own usage
rem buf if you have use for it yourself, feel free to use it.
rem Also note, this is based on how Visual Studio has been installed
rem on my system... If it has been installed the same way on yours
rem is something I can never know, right? :-P

"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild" -p:Configuration=Release
