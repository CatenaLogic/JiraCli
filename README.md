JiraCli
============

[![Join the chat at https://gitter.im/CatenaLogic/JiraCli](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/CatenaLogic/JiraCli?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

![License](https://img.shields.io/github/license/catenalogic/jiracli.svg)
![NuGet downloads](https://img.shields.io/nuget/dt/jiracli.svg)
![Version](https://img.shields.io/nuget/v/jiracli.svg)
![Pre-release version](https://img.shields.io/nuget/vpre/jiracli.svg)
![Chocolatey count](https://img.shields.io/chocolatey/dt/jiracli.svg)
![Chocolatey version](https://img.shields.io/chocolatey/v/jiracli.svg)

![JiraCli](design/logo/logo_64.png)

JiraCli allows control of JIRA via the command line interface (CLI). One feature that is very useful is the availability to create releases via the CLI. This is very useful for build servers and continuous integration (CI) situations. 


## Usage

The usage is simple and always requires this base call:

    JiraCli.exe -user [user] -pw [password] -url [url] -action [action]

The actions are implemented and determined dynamically. Use the **-help** parameter to see all available actions.

## Getting help

When you need help about JiraCli, use the following command line:

    JiraCli.exe -help

## Logging to a file ##

When you need to log the information to a file, use the following command line:

    JiraCli.exe -l JiraCliLog.log [other parameters]

# How to get JiraCli #

There are three general ways to get JiraCli:

## Get it from GitHub ##

The releases will be available as separate executable download on the [releases tab](https://github.com/CatenaLogic/JiraCli/releases) of the project.

## Get it via Chocolatey ##

If you want to install the tool on your (build) computer, the package is available via <a href="https://chocolatey.org/" target="_blank">Chocolatey</a>. To install, use the following command:

    choco install jiracli

## Get it via NuGet ##

If you want to reference the assembly to use it in code, the recommended way to get it is via <a href="http://www.nuget.org/" target="_blank">NuGet</a>. 

**Note that getting JiraCli via NuGet will add it as a reference to the project**

# Icon

Flag by Castor & Pollux from The Noun Project
