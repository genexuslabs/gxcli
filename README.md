# GeneXus CLI
[![](https://github.com/genexuslabs/gxcli/workflows/Build/badge.svg?branch=master)](https://github.com/genexuslabs/gxcli/actions?query=workflow%3ABuild+branch%3Amaster)
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

This is for power users. Users that know how to move around in a command line interface, and actually prefer to do some heavy dutty tasks from there. 

GeneXus has plenty of support for MSBuild tasks, so you can pretty much do everything with MSBuild scripts. But sometimes you don't have the time (or will) to set up the scripts, remember the MSBuild syntax, dig into the documentation for parameters and so on.

FYI: as you will see, this tool in built on top of the existing MSBuild tasks ecosystem.

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
Download the [latest release](https://github.com/sebagomez/gxcli/releases/latest) and unzip the gxcli.zip file into any folder you wish.  
Make sure you install the tool first by running `gx install <GeneXus Path>`. And that's it!


## GeneXus CLI Platform
Want to create your own modules? Maybe you want to call your own extensions.  
In Visual Studio create a new `Class Library (.NET Framework 4.7.1)` project and add a reference to the `gxcli.common` project of this repo (it'll also be a NuGet package soon).
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

Here's an example of the actual `build` verb.
```cs
public class BuildProvider : IGXCliVerbProvider
{
  public string Name => "build";

  public string Description => "Build All for the working environment";

  public string Target => "Build";

  public List<VerbParameter> Parameters => new List<VerbParameter>(KBBasedVerbProvider.KBParameters)
  {
    new VerbParameter { Name = "ForceRebuild", Description = "Force rebuild the objects" },
    new VerbParameter { Name = "DoNotExecuteReorg", Description = "Do not execute reorg, just create it" },
    new VerbParameter { Name = "FailIfReorg", Description = "Generate, but do not make a build if a reorg is needed" },
    new VerbParameter { Name = "CompileMains", Description = "Compile all main objects, if false only compile the Developer Menu." },
    new VerbParameter { Name = "DetailedNavigation", Description = "Show detailed navigation" },
  };

  public List<Example> Examples => new List<Example>
  {
    new Example{ Command = "gx build kbpath=C:\\Models\\MyKB forceRebuild", Description = "Rebulid All on your Knowledge Base" }, 
    new Example{ Command = "gx build kbpath=C:\\Models\\MyKB failIfReorg", Description = "Bulid All on your Knowledge Base, but fail if a database reorganization is found" }
  };
}
```
so when you run `gx build help` this is what you'll see  
```
Usage: gx build [parameters]|help [global options]

    build                     : Build All for the working environment

Parameters
    KBPath                    : Path to the Knowledge Base directory. [Required]
    Version                   : Name of the Knowledge Base version to use.
    Environment               : Name of the Environment to use.
    ForceRebuild              : Force rebuild the objects
    DoNotExecuteReorg         : Do not execute reorg, just create it
    FailIfReorg               : Generate, but do not make a build if a reorg is needed
    CompileMains              : Compile all main objects, if false only compile the Developer Menu.
    DetailedNavigation        : Show detailed navigation

Examples
    Rebulid All on your Knowledge Base
        gx build kbpath=C:\Models\MyKB forceRebuild

    Bulid All on your Knowledge Base, but fail if a database reorganization is found
        gx build kbpath=C:\Models\MyKB failIfReorg
```

4 - Compile your module and that's it. In order to try it copy it into the `<GeneXus>\gxclimodules` folder with its .msbuild/.targets file. Run `gx install` so the *GeneXus CLI* can pick it up and have it available for future use.
