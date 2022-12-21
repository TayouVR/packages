# VRChat Packages Fork

This repo is a Maintained Fork of the [VRChat Packages](https://github.com/vrchat/packages) repository for the [VRChat Package Manager](https://docs.vrchat.com/v2021.4.1/docs/package-manager)
The Packages in this fork work i nthe Unity Package Manager o ntheir own. I miht add Auto Updating in Unity eventually too.

# tweaks-applied Branch
### This branch adds a few QOL tweaks, which are all in pull requests on the main repo too.
In addition this branch disables all kinds of error based upload/build blocking. This could be refined to only disabled certain things, but i am too lazy to do so.
In a nutshell the SDK blocks uploading for too many things, e.g. blend shape normals, so I am just disabling the upload blocking alltogether.
