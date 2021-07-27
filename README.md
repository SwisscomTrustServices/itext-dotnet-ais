# iText7 and .NET based AIS Client

A .NET Standard client library and a CLI wrapper for using the [Swisscom All-in Signing Service (AIS)](https://www.swisscom.ch/en/business/enterprise/offer/security/all-in-signing-service.html)
to sign and/or timestamp PDF documents. The library(AIS project) can be used as a project dependency. You can also use the CLI wrapper as a command-line tool for batch operations.
It relies on the [iText](https://itextpdf.com/en) library for PDF processing.

## Nuget Package

The standalone client library is available as a [nuget package](https://www.nuget.org/packages/TrustServices.AIS.Net.Client/) to refrence in your projects. 

## .Net AIS Client Video Demo

[![Watcht the video](https://i.imgur.com/DsSRUjW.png)](https://youtu.be/iXJJWOIvBXc)

or see it on SharePoint:

* https://swisscom-my.sharepoint.com/:v:/p/paul_muntean/EQG2JYWKJvFHqiwefyQGiQgBBITIJt2PHkxH9zs-MdbcJw?e=pjqaoE


## Getting started

To start using the Swisscom AIS service and this client library, do the following:
1. Acquire an [iText license](https://itextpdf.com/en/how-buy)
2. [Get authentication details to use with the AIS client](docs/get-authentication-details.md).
3. [Build or download the AIS client binary package](docs/build-or-download.md)
4. [Configure the AIS client for your use case](docs/configure-the-AIS-client.md)
5. Use the AIS client, either [programmatically](docs/use-the-AIS-client-programmatically.md) or from the [command line](docs/use-the-AIS-client-via-CLI.md)

Other topics of interest might be:
* [On PAdES Long Term Validation support](docs/pades-long-term-validation.md)

## Quick examples

The rest of this page provides some quick examples for using the AIS client. Please see the links
above for detailed instructions on how to get authentication data, download and configure
the AIS client. The following snippets assume that you are already set up.

### Command line usage
Get a help listing by calling the client without any parameters:
```shell
.\CLI.exe
```
or
```shell
..\CLI.exe -help
```
Apply an On Demand signature with Step Up on a local PDF file:
```shell
.\CLI.exe -type ondemand-stepup -input local-sample-doc.pdf -output test-sign.pdf -config config.json
```
You can also add the following parameter for extra help:

- _-v_: verbose log output (sets the loggers to debug)

More than one file can be signed/timestamped at once:
```shell
.\CLI.exe -type ondemand-stepup -input doc1.pdf -input doc2.pdf -input doc3.pdf -config config.json
```

You don't have to specify the output file:
```shell
.\CLI.exe -type ondemand-stepup -input doc1.pdf -config config.json
```
The output file name is composed of the input file name plus a configurable _suffix_ (by default it is "-signed-#time", where _#time_
is replaced at runtime with the current date and time). You can customize this suffix:
```shell
.\CLI.exe -type ondemand-stepup -config config.json -input doc1.pdf -suffix -output-#time 
```

### Notes for command line usage

* The command starts with CLI.exe ….. instead of the various shell scripts used for Java.
* There is no -init command as the sample config folder is supplied along the executable(“config.json”) and the .NET does not have other config files like java(e.g., logback).
* The verbosity is in 2 layers. Either use “-v” for verbose or without it for less info.
* The -config argument should always be used. You can supply any name for the config file you like, but the .NET doesn’t assume a default config file and location like Java.


### Programmatic usage
Once you add the AIS client library as a dependency to your project, you can configure it in the following way(the same is demostrated in Tests\TestFullyProgramaticConfiguration):
```C#
// load/deserialize or build the configuration properties
  ConfigurationProperties properties = new ConfigurationProperties
            {
                ClientPollRounds = "10",
                ClientPollIntervalInSeconds = "10",
                ITextLicenseFilePath = "your-license-file", // if not supplied it will run in unlicensed mode
                ServerRestSignUrl = "https://ais.swisscom.com/AIS-Server/rs/v1.0/sign",
                ServerRestPendingUrl = "https://ais.swisscom.com/AIS-Server/rs/v1.0/pending",
                ClientAuthKeyFile = "d:/Work/Swisscom/my-ais.key",
                ClientAuthKeyPassword = "your-password",
                ClientCertFile = "d:/Work/Swisscom/my-ais.crt",
                SkipServerCertificateValidation = true, // set this to false if the server certificate is trusted
                ClientHttpMaxConnectionsPerServer = "10",
                ClientHttpRequestTimeoutInSeconds = "10"
            };
			
// initialize a configuration for the rest client and initialize a rest client
RestClientConfiguration restClientConfiguration = new RestClientConfiguration(properties);
IRestClient restClient = new RestClient(restClientConfiguration);

// initialize the AIS client config
AisClientConfiguration aisClientConfiguration = new AisClientConfiguration(properties);

//build an AIS client and a UserData instance for with details about this signature
 try
            {
                IAisClient aisClient = new AisClient(restClient, aisClientConfiguration);
                UserData userData = new UserData
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    ClaimedIdentityName = "ais-90days-trial",
                    ClaimedIdentityKey = "OnDemand-Advanced",
                    DistinguishedName = "cn=Test Name, givenname=Test, surname=Test, c=US, serialnumber=0b5e3f1eb4b1a84b31ea3ff45fcab1049c95a00c",
                    StepUpLanguage = "en",
                    StepUpMessage = "Please confirm the signing of the document",
                    StepUpMsisdn = "40740634123",
                    SignatureReason = "For testing purposes",
                    SignatureLocation = "Topeka, Kansas",
                    SignatureContactInfo = "test@test.com",
                    SignatureStandard = new SignatureStandard("PAdES-baseline"),
                    RevocationInformation = new RevocationInformation("PAdES-baseline"),
                    ConsentUrlCallback = new ConsentUrlCallback()
                };
//subscribe to the OnConsentUrlReceived event
				userData.ConsentUrlCallback.OnConsentUrlReceived += LogAtConsole;

// populate a list of PdfHandle objects with details about the document to be signed. 
     			List<PdfHandle> documents = new List<PdfHandle>
                	{
                    	new PdfHandle
                    	{
                        	InputFileName = "input.pdf",
                        	OutputFileName = "output-programatic.pdf",
                       	 	DigestAlgorithm = DigestAlgorithm.SHA256
                    	}
                	};
//do the signature
      SignatureResult signatureResult = aisClient.SignWithOnDemandCertificateAndStepUp(documents, userData);
        if (signatureResult == SignatureResult.SUCCESS) {
            // yay!
        }
    }
```

## References

- [Swisscom All-In Signing Service homepage](https://www.swisscom.ch/en/business/enterprise/offer/security/all-in-signing-service.html)
- [Swisscom All-In Signing Service reference documentation (PDF)](http://documents.swisscom.com/product/1000255-Digital_Signing_Service/Documents/Reference_Guide/Reference_Guide-All-in-Signing-Service-en.pdf)
- [Swisscom Trust Services documentation](https://trustservices.swisscom.com/en/downloads/)
- [iText library](https://itextpdf.com/en)
