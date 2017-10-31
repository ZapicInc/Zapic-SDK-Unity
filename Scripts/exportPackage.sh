#!/bin/sh
#

bold=$(tput bold)
normal=$(tput sgr0)

echo ""
echo "===================================="
echo "      Exporting Unity Package       "
echo "===================================="
echo ""
# /Applications/Unity/Unity.app/Contents/MacOS/Unity \
#         -quit \
#         -nographics \
#         -batchmode \
#         -projectPath $(pwd) \
#         -silent-crashes \
#         -logFile $(pwd)/unity.log \
#         -exportPackage Assets/Zapic Assets/Plugins  Zapic.unitypackage

wget https://github.com/FatihBAKIR/UnityPacker/releases/download/0.0.1/UnityPacker.exe ;
wget https://github.com/FatihBAKIR/UnityPacker/releases/download/0.0.1/ICSharpCode.SharpZipLib.dll ;
chmod +x UnityPacker.exe ;
./UnityPacker.exe Assets Zapic no "." "gitignore,md,exe,dll" ".git" ;
# mv QuarkDefault.unitypackage ~ ;
# rm -rf /tmp/Quark.Default ;
