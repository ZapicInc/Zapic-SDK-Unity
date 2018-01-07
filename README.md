# Zapic-SDK-Unity

[![Build Status](https://travis-ci.org/ZapicInc/Zapic-SDK-Unity.svg?branch=master)](https://travis-ci.org/ZapicInc/Zapic-SDK-Unity)

This open-source library allows you to integrate Zapic into your Unity app.

Learn more about about the provided samples, documentation, integrating the SDK into your app, accessing source code, and more at https://www.zapic.com

GIVE FEEDBACK
-------------
Please report bugs or issues right here in the [GitHub Issues](https://github.com/ZapicInc/Zapic-SDK-Unity/issues)

CONTRIBUTING
-------------
We are always accepting contributions to the Zapic SDK for Unity, simply submit a pull request.

How to create a release
-------------

### Update iOS SDK version

Edit the .travis.yml file to include the desired version of the [iOS SDK](https://github.com/ZapicInc/Zapic-SDK-iOS). Make sure this matches exactly.

### Create new .unitypackage

Simply create a new [release](https://github.com/ZapicInc/Zapic-SDK-Unity/releases). Ensure the version follows the required format of "vX.X.X" for example "v1.2.3". CI should automatically place all artifacts within the GitHub release upon build completion.

