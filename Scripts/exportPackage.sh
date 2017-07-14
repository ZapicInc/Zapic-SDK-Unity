#!/bin/sh
#

bold=$(tput bold)
normal=$(tput sgr0)

#echo ""
#echo "===================================="
#echo "  Started building Zapic artifacts  "
#echo "===================================="
#echo ""
#
#echo "${bold}Carthage Config${normal}"
#echo "$(cat ../Cartfile)"
#
#echo ""
#echo "${bold}Android Config${normal}"
#
#
#echo ""
#echo "===================================="
#echo "Building Carthage (iOS) dependencies"
#echo "===================================="
#echo ""
#(cd ../; carthage bootstrap --no-use-binaries --platform iOS --cache-builds)



echo ""
echo "===================================="
echo "     Copying (iOS) dependencies     "
echo "===================================="
echo ""

CARTHAGE_BUILD=../Carthage/Build/iOS
iOS_PLUGIN=../Assets/Plugins/iOS

mkdir -p $iOS_PLUGIN

#Copy all .framework files into the ios plugin directory
ditto $CARTHAGE_BUILD $iOS_PLUGIN/

#Remove unused files
find $iOS_PLUGIN -maxdepth 1 -not -type d -delete
find $iOS_PLUGIN -name "*.dSYM"
find $iOS_PLUGIN -maxdepth 1 -name "*.dSYM" -exec rm -r "{}" \;
find $iOS_PLUGIN -maxdepth 1 -name "*Test*" -exec rm -r "{}" \;


echo ""
echo "===================================="
echo "      Exporting Unity Package       "
echo "===================================="
echo ""
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
        -quit \
        -nographics \
        -batchmode \
        -silent-crashes \
        -logFile $(pwd)/unity.log \
        -projectPath $(pwd)/../ \
        -exportPackage Assets/Zapic Assets/Plugins  ZapicTest.unitypackage

#zip -r archive_name.zip folder_to_compress


#SCRIPT_DIR= pwd
#echo $SCRIPT_DIR
##BUILD_DIR= ../build
#PROJ_DIR= $(cd ../; pwd)
##echo $BUILD_DIR
#echo $PROJ_DIR
#
#ls ../
