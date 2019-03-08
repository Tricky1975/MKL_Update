# MKL_Update

When you browse around in the various tools I've created, in well most languages I've ever used, you may have seen the MKL_Lic and MKL_Version commands, as well as some pregenerated license blocks. I've always used this tool for that which simply adepts all these in source file which have been changed.

Now the original program was coded in BlitzMax, however as I've the feeling BlitzMax is completely abandoned now (I'm lucky I still have a working version, as you gotta compile it yourself now and that is not always a pleasure I tell ya), I've decided to rewrite this tool with C#.

Why C#? Because it's one of the most coder friendly languages I have now, and since I don't have to deal with hardcore operations that should require me C or C++, it's the easiest way to go. Since this is a console application, it should also work fine on Mac and Linux, but you will need to run it with Mono then, my experiences on that have always been good ;)


# Compiling

The program has been written in Visual Studio, and should compile in any version of VS without much trouble.
The datafile is a JCR6 resource file which should always be named MKL_Update.jcr and be located in the same folder as MKL_Update.exe (Linux users should be aware of the case sensitivity or things may not work well). If you have the JCR6 cli tools installed and your "PATH" environment variable properly set so it can easily be found, then the builddata.bat should work on both Unix and Windows to get this file built.

# W.I.P

This is a work in progress... Don't expect this to work at all as long as this notice is still there!


![](https://upload.wikimedia.org/wikipedia/commons/thumb/8/8b/Copyleft.svg/1024px-Copyleft.svg.png)