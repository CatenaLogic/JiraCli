JiraCli
============

![ContinuaInit](design/logo/logo_64.png)

JiraCli allows control of JIRA via the command line interface (CLI). One feature that is very useful is the availibility to create releases via the CLI. This is very useful for build servers and continuous integration (CI) situations. 


## Usage

The usage is simple:

    JiraCli.exe -user [user] -pw [password] -url [url] [otheroptions]

## Create new release in JIRA

	JiraCli.exe -user [user] -pw [password] -url [url] -createrelease -project MyProject -version 1.2.0 


#Icon

Flag by Castor & Pollux from The Noun Project