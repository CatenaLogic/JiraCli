JiraCli
============

![ContinuaInit](design/logo/logo_64.png)

JiraCli allows control of JIRA via the command line interface (CLI). One feature that is very useful is the availibility to create releases via the CLI. This is very useful for build servers and continuous integration (CI) situations. 


## Usage

The usage is simple:

    JiraCli.exe -user [user] -pw [password] -url [url] -action [action]

The actions are implemented and determined dynamically. Use the **-help** parameter to see all available actions.

## Create new version in JIRA

	[basecall] -createversion -project MyProject -version 1.2.0 

## Release new version in JIRA

	[basecall] -releaseversion -project MyProject -version 1.2.0 

#Icon

Flag by Castor & Pollux from The Noun Project