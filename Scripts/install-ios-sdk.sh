#!/bin/sh
#

bold=$(tput bold)
normal=$(tput sgr0)

echo ""
echo "===================================="
echo "      Copying iOS source files      "
echo "===================================="
echo "           "$iOS_PLUGIN"            "
echo ""

#Repo to clone
reporUrl=https://github.com/ZapicInc/Zapic-SDK-iOS.git
#TODO:switch to the proper version/tag
iOS_REPO=./zapic-ios
iOS_PLUGIN=../Assets/Plugins/iOS/Zapic

#Ensure the repo directory is clear
rm -rf $iOS_REPO

#Ensure the plugin directory is clear
rm -rf $iOS_PLUGIN

#Clone the repo
git clone $reporUrl $iOS_REPO

#Ensure the dir exists
mkdir -p $iOS_PLUGIN

#Copy the source code into the plugin folder
cp -r $iOS_REPO/Zapic/. $iOS_PLUGIN/

#Remove the repo    
rm -rf $iOS_REPO
