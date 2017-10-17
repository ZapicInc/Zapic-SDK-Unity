#! /bin/sh

# Find the latest linux build links here
# https://forum.unity.com/threads/unity-on-linux-release-notes-and-known-issues.350256/#post-2556301 

LINUX_URL= http://beta.unity3d.com/download/061bcf22327f/unity-editor_amd64-2017.1.0xf3Linux.deb

echo 'Downloading Unity pkg:'
curl --retry 5 -o Unity.deb $LINUX_URL
if [ $? -ne 0 ]; then { echo "Download failed"; exit $?; } fi

# Run installer
echo 'Installing Unity.deb'
sudo dpkg -i ./Unity.deb
