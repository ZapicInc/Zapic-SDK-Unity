#!/bin/sh
#

bold=$(tput bold)
normal=$(tput sgr0)

echo ""
echo "===================================="
echo "     Copying (iOS) dependencies     "
echo "===================================="
echo "           "$iOS_PLUGIN"            "
echo ""

#Location for the iOS framework
iOS_PLUGIN=./Assets/Plugins/iOS
iOS_FRAMEWORK=$iOS_PLUGIN/Zapic.framework

#Ensure the dir exists
mkdir -p $iOS_PLUGIN

#Url to the release
releaseUrl=https://github.com/ZapicInc/Zapic-SDK-iOS/releases/download/${iOS_SDK}/Zapic.framework.zip

#Download the release to the directory
wget $releaseUrl -P $iOS_PLUGIN

#Unzip the framework
unzip ${iOS_FRAMEWORK}.zip -d $iOS_PLUGIN

#Remove the zip file
rm -r $iOS_FRAMEWORK.zip