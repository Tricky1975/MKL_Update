@echo off
rem the actual JCR6 file build
jcr6 add -doj ../MKL_Update/bin/Release/MKL_Update.jcr
rem A fake JCR6 file redirecting to the release jcr, so I don't need two full JCR files to debug (only wastes space after all).
jcr6 add -doj -jif debug.jif ../MKL_Update/bin/Debug/MKL_Update.jcr
echo All done
