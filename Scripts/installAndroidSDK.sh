#!/bin/sh

bold=$(tput bold)
normal=$(tput sgr0)

echo ""
echo "===================================="
echo "   Copying (Android) dependencies   "
echo "===================================="
echo ""

#Location for the Android framework
Android_PLUGIN=./Assets/Plugins/Android

#Ensure the dir exists
mkdir -p $Android_PLUGIN

#Url to the release
releaseUrl=https://github.com/ZapicInc/Zapic-SDK-Android/releases/download/${Android_SDK}/Zapic-SDK-Android-${Android_SDK}.zip

#Download the release to the directory
wget $releaseUrl -P $Android_PLUGIN

#Unzip the framework
unzip ${Android_PLUGIN}/Zapic-SDK-Android-${Android_SDK}.zip -d $Android_PLUGIN

#Remove the zip file
rm -r ${Android_PLUGIN}/Zapic-SDK-Android-${Android_SDK}.zip