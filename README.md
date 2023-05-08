# VRChat Packages Fork

This repo is a Maintained Fork of the [VRChat Packages](https://github.com/vrchat/packages) repository for the [VRChat Package Manager](https://docs.vrchat.com/v2021.4.1/docs/package-manager)
The Packages in this fork work in the Unity Package Manager on their own. ~~They will get discontinued eventually as I am going to move all my modifications to a harmony based patch, which you will be able to install via either OpenUPM or as git UPM package.~~

## Deprecation Notice
I am not going to continue maintaining these SDK patches in this repo, as it is technically violating VRChats TOS for modifying the SDK.
If you wish to use the patches I made, look at my new [Harmony based Patches here](https://github.com/TayouVR/VRChat-SDK-UITweaks)! 
You can install these patches through OpenUPM [here](https://openupm.com/packages/org.tayou.vrchat.sdk-ui-tweaks/).
This repository will be privated or deleted within the coming 30 days.

### The VRChat SDK is All Right Reserved (C) VRChat Inc. 
I or anyone else has no right to perform any modifications to the VRChat SDK, even if it is on Github. This repo will be taken down eventually to follow the rules and VRChat ToS. If you wish to colaborate on harmony patches for tweaks and additions (which don't infringe on Copyright Law), feel free to reach out to me via Github Issues, matrix, discord, twitter or whatever other service.

# tweaks-applied Branch
### This branch adds a few QOL tweaks, which are all in pull requests on the main repo too.
In addition this branch disables all kinds of error based upload/build blocking. This could be refined to only disable certain things, but i am too lazy to do so.
In a nutshell the SDK blocks uploading for too many things, e.g. blend shape normals, so I am just disabling the upload blocking alltogether.
