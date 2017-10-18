#!/bin/sh
#

bold=$(tput bold)
normal=$(tput sgr0)

echo ""
echo "===================================="
echo "      Exporting Unity Package       "
echo "===================================="
echo ""
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
        -quit \
        -nographics \
        -batchmode \
        -projectPath $(pwd) \
        -silent-crashes \
        -logFile $(pwd)/unity.log \
        -exportPackage Assets/Zapic Assets/Plugins  Zapic.unitypackage
