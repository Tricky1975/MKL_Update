@echo off
rem the actual JCR6 file build
jcr6 add -doj ../MKL_Update/bin/Release/MKL_Update.jcr
rem A fake JCR6 file redirecting to the release jcr, so I don't need two full JCR files to debug (only wastes space after all).
jcr6 add -doj -jif debug.jif ../MKL_Update/bin/Debug/MKL_Update.jcr
echo All done



rem Please note in JCR6 it's conventional to only use / and never \ 
rem even when working in Windows or other systems coming from the DOS family
rem
rem Now JCR6 is able to translate it all by itself anyway, but where would 
rem JCR6 go, if I, it's own creator, didn't set the right example :P

