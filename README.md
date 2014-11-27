JiraCli
============

![ContinuaInit](design/logo/logo_64.png)

JiraCli allows control of JIRA via the command line interface (CLI). One feature that is very useful is the availibility to create releases via the CLI. This is very useful for build servers and continuous integration (CI) situations. 


## Usage

The usage is simple and always requires this base call:

    JiraCli.exe -user [user] -pw [password] -url [url] -action [action]

The actions are implemented and determined dynamically. Use the **-help** parameter to see all available actions.

## Getting help

When you need help about GitLink, use the following command line:

    JiraCli.exe -help

## Logging to a file ##

When you need to log the information to a file, use the following command line:

    JiraCli.exe -l GitLinkLog.log [other parameters]


## Create new version in JIRA

	[basecall] -createversion -project MyProject -version 1.2.0 

## Release new version in JIRA

	[basecall] -releaseversion -project MyProject -version 1.2.0 

# How to get JiraCli #

There are three general ways to get JiraCli:

## Get it from GitHub ##

The releases will be available as separate executable download on the [releases tab](https://github.com/CatenaLogic/JiraCli/releases) of the project.

## Get it via Chocolatey ##

If you want to install the tool on your (build) computer, the package is available via <a href="https://chocolatey.org/" target="_blank">Chocolatey</a>. To install, use the following command:

    choco install JiraCli

## Get it via NuGet ##

If you want to reference the assembly to use it in code, the recommended way to get it is via <a href="http://www.nuget.org/" target="_blank">NuGet</a>. 

**Note that getting JiraCli via NuGet will add it as a reference to the project**

#Icon

Flag by Castor & Pollux from The Noun Project