# GeneXus CLI
![Build](https://github.com/sebagomez/gxcli/workflows/Build/badge.svg?branch=master&event=push)
```
   ____               ___  __
  / ___| ___ _ __   ___\ \/ /   _ ___
 | |  _ / _ \ '_ \ / _ \\  / | | / __|
 | |_| |  __/ | | |  __//  \ |_| \__ \
  \____|\___|_| |_|\___/_/\_\__,_|___/

Welcome to the cool new GeneXus CLI!
```

We're excited to introduce *GeneXus CLI*, our next-generation command line tool for GeneXus.

## Why a command line tool?

We believe there are times where you want to perform some tasks in batch. Like firing a `Rebuild All` or a big `Update` from GXserver.

We do have support for MSBuild tasks, so you can pretty much do everything with MSBuild scripts. But sometimes you don't have the time (or will) to set up the scripts, remember the MSBuild syntax, dig into the documentation for parameters and so on.

## Introducing GeneXus CLI

*GeneXus CLI* is a small command line tool that allows you to run every batch task you can think of via the command line, without the need of firing up the whole IDE.

It is also based on MSBuild, so whatever this tool does, it can be done via MSBuild scripts. As a matter of fact, every module has an MSBuild script to support the exposed commands.

## Usage

```
gx [command] [parameters]|help [global options]
```

Like any modern cli tool, *GeneXus CLI* contains commands (or verbs) that allows you to easily learn about the tool.

### Commands
Commands (or verbs) are all the tasks the tool can perform. You can learn more about the needed parameters for those commands by typing the `help` subcommand.
```
gx build help
```

### Parameters
Parameters are space-separated and the `=` (equals) sign is used to assign a value. Boolean values are not required to be assigned a `true` value. If the parameter is present with no value, `true` is assumed.
```
gx build kbpath=C:\Models\MyKB forceRebuild
```

### Global Options
Global options are not specific to any command, it's a way of sending general properties to the engine, like its output verbosity.
```
gx build kbpath=C:\Models\MyKB failIfReorg verbosity=minimal
```

## Installation
Copy the files to your installation of GeneXu and that's it. Coming soon there will be a way of installing via Nuget.

## GeneXus CLI Platform
What to create your own modules? Maybe you want to call your own extensions.  
In Visual Studio create a new `Class Library (.NET Framework 4.7.1)` project and add a reference to the `gxcli common` project of this repo (it'll also be a NuGet package soon).
These are the needed steps to create a class that exposes commands (verbs).  

1 - In you project's AssemblyInfo.cs file, add the `GXCliVerbProvider` attribute as follows:

```c#
using gxcli.common;
[assembly: GXCliVerbProvider]
```

2 - Add a .msbuild file to you project that will hold the targets GeneXus CLI will eventually call. This file must be named after your assembly and must have the extension .msbuild or .targets.

3 - Create your Verb provider class, that must implement the `IGXCliVerbProvider` interface. Keep in mind this is what every property is for:

- Name : The name of the command, it will be what the user type to fire your command.
- Description: A brief description that helps the user understand what the commands does.
- Target : The target this command will call in your script created in 2.
- Parameters: List of parameters your commands need or accept. Make sure you use the `Required` property accordingly.
- Examples: You can also add a few examples that will be shown to the users when they want to know more about your command.

4 - Compile your module and that's it. In order to try it copy it into the `<GeneXus>\gxclimodules` folder with its .msbuild/.targets file. Run `gx install` so the GeneXus CLI can pick it up and have it available for future use.
