# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

#Table Of Contents
1. [Getting Started](#Getting-Started)
1. [Installation](#Installation)
1. [Adding New Applications](#Adding-New-Applications)
1. [Adding New Recordsets](#Adding-New-Recordsets)
1. [Authentication](#Authentication)
1. [Authorization](#Authorization)
1. [Authorization](#Authorization)
1. [Creating API Keys](#Creating-API-Keys)
1. [Adding Users](#Adding-Users)
1. [Architecture](#Architecture)
1. [Common Issues](#Common-Issues)
1. [WDS Client](#WDS-Client)

#Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

#Installation
##Application
You can publish the applcation by: 
1. Building and publish within Visual Studio
1. Copy the published files to the folder you want to run the application from
1. Configure IIS to run .NET Core Web API Application
1. Configure the WDS Settings files
1. Configure the WDS Database

##Database
1. Included in this Repo is the WDS database. This database is needed to run the WDS and contains most of the settings needed to run it. There are some exceptions to that. These can be found in the [Configuration](#configuration) section. 

Schema diagrams and further database documentation can be found in the database Diagram folder of the database project. 


##Configuring Settings
The configuration files for the application are located in the {root}/settings folder. Documentation on how to configure those files are located in the {root}/Settings/Documentation folder of this repository. For security reasons these files are not published to the running application. 

### appsettings.json
"AppKeys": {
    "AppName": "WDS",
    "AppClassification": 3,
    "Issuer": "{WEBURL}}",
    "Domain": "{DOMAIN}",
    "AppMode": "Development", // Valid Values:  Development, Staging, Prod, DevelopmentFullAccess
    //  "SwaggerEndpoint": "/WDS/",       //uncomment this for web server
    "SwaggerEndpoint": "/Swagger", //uncomment this for local
    "IsDevelopment": false,
    "IPCacheRefreshMinutes": 20,
    "AppOwner": "Owner name",
    "AppOwnerEmail": "admin of the application that you want to be notified of issues. ",
    "LogLevel": 0 // 0- Informational, 1-Warnings, 2-Errors  Information will capture all the logs "verbose"
  }
  
### Connections.json
Documentation on this file can be found in {root}/Settings/Documentation/Connections.json

** Most connection information can be found in the [wds].[connections] table **
Some connection information is needed on new installations of the WDS and must be supplied in this file. The following three names must exist in this file for the WDS to start up. 
* WDS: How to connect to the WDS Database
* WDSLogging: Where to store the WDS Logs
* WDSCache: Where to store the WDS Cache

Any connections you add to the file will be added to the database. 

### apiKeys.json
Documentation on this file can be found in {root}/Settings/Documentation/apiKeys.json
** Most api keys can be found in the [wds].[api_keys] table **

API Keys are also stored in the database. This is the preferred location because it is more secure. The keys are stored in an encrypted format. 

### Application.json
Documentation on this file can be found in {root}/Settings/Documentation/Application.json
** Most applications can be found in the [wds].[applications] table **

Some additional notes on certain attributes. 
** replaceWith: **
replace with is used to replace the passed in parameter with the sting that exists in this configuration. This string can include other variables names to enhance capability. The varaibles that are allowed are: System Environment Variables, Service Configured DB Variables ([wds].[service_variables]), and those variables that have been passed in as part of the call. See below examples. 

Variables must be surrounded with '{ }'
Assume the following: 
1. Caller passed in build_code = API
2. SYSTEM_HOST environmental variable set to HOSTA
3. variable inside [wds].[serivce_variables] named 'AName' exists with value 'SomeValue'

Output 
"{passedInVariable}-{ENVVariableName}.{AName}" = "A-HOSTA.SomeValue

** defaultValue **
with the exception of 'null', this is a string that is passed into the call when one is not provided. Due to the close relationship to the attribute 'replaceWith', one must take precidence. 'replaceWith' will be processed first, but if the value is evaluated as blank, the default value will be applied. 

** Substitute **
This attribute will allows the WDS to call the next recordset if the first one returned in error or without any roles. You can chain these together to create a fault tolerant and resiliant recordset. 

#Adding New Applications
FUTURE: Create an application that will allow users to create new applications. 

The process for creating a new application is still in early stages and will likely improve dramatically as we move forward. As of 4/4/2023, there are a couple of options. 
1. Create an {application}.json file with the needed values. Place the file in the settings folder and it will be loaded into the WDS on Application Load. 

#Adding New Recordsets

#Authentication 
1. API-Keys
1. SessionID -- Successful user authentication through the WDS. 

#Authorization

#Creating API Keys

#Adding Users

#Architecture

##Auditing

##Security

##Availability

##Caching

##Logging

#Common Issues


#WDS Client
The client is used to communicate with the WDS without understanding all the nuances of setting up the Web Service calls. 

##appSettings.json
There are just two areas that have to be included in this file for the client to work. This file will be specific to each application and seperate from the WDS Client code. 

	"WDSServer": {
		  "Prod": "https://wdsl",
		  "Staging": "https://wds",
		  "Dev": "https://wds",
		  "local": "https://localhost:63872"
		}
	
	"WDSClientSettings": {
      "API-Key": "This is the API Key needed to communicate with the WDS. If this is blank, the user credentials will be passed to the WDS to authorize access.",      
      "Application": "Name of your application (set of recordsets) in the WDS. This will be used for your default calls. You can still call other applications if you specify the application name. ",
      "DataFormatType": "{CSV|Json|XML}",
      "DataDetailType": "{Default|Simple} Simple will just return the data without the WDS metadata."
    }
##Common Issues
1. The SSL connection could not be established, see inner exception => Untrusted Certificate
The solution to this is to open Powershell and run the following command: **dotnet dev-certs https --trust**
This will trust the local certificate and allow you to use the client. 
1. AuthenticationException: The remote certificate is invalid according to the validation procedure: RemoteCertificateNameMismatch
Make sure the WDSServer setting in the appsettings.json file is pointing to a WDS server that matches the certificate you have. 

#WDS Raw Requests
## Multi Request
[
	{
		"id":"Unique Identifier for the call you are making.",
		"Application":"Application Name",
		"call":"Recordset Name",
		"UserParameters": "Name/Value pairs, see below for examples of the parameters that you want to pass in.",
		"returnType": "(int) 0-Not Set (Configured default will apply), 1-Json, 2-CSV, 3-Xml, 4-Email",
		"requestedDataDetailType": "(int) 0-Not Set, 1-Default, 2-Simple (just the data is returned)"
	},
	{
		"id":"7182c26d-2f5c-4ae6-9218-dd711bd12f41",
		"Application":"TestingOnly",
		"call":"VehicleSubTypes",
		"UserParameters": {"":""},
		"returnType":2,
		"requestedDataDetailType":1
	},
	{
		"id":"8d8161b9-dc71-4295-bf54-1c3493057aa6",
		"Application":"TestingOnly",
		"call":"DimDates",
		"UserParameters":
		{
			"Day_Name":"Friday"
		},
		"returnType":0,
		"requestedDataDetailType":1
	},
	{
		"id":"004d09de-b5ac-463e-81eb-fe88c8010851",
		"Application":"Admin",
		"call":"GenerateGuid",
		"UserParameters": {"":""},
		"returnType":0,"requestedDataDetailType":1
	}
]