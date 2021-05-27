# Use the AIS client programmatically
The client library can be used as a normal project dependency, allowing your project to access
the document signing and timestamping features provided by the AIS service. Until a nuget package is released(in the works) you will have to install the dependencies yourself. 
Please install the following nuget packages: 
Common.Logging >= 3.4.1 
itext7 >= 7.1.4 
itext7.licensekey >= 3.1.4 
Newtonsoft.Json >= 13.0.1 
Portable.BouncyCastle >= 1.8.10  



## Using the library
This section describes the usage of the library in code. See the sample files 
in the [root source folder](../AisClient/Tests) for complete examples of how to use the library in code.

First create a configuration properties object by deserializing it from a config file or create it from code. This object can later be used in constructors of other more 
specific configuration objects(RestClientConfiguration, AisClientConfiguration, UserData)
```C#
ConfigurationProperties properties = new ConfigurationProperties
{
  "ITextLicenseFilePath": "d:/home/user/license-file.txt",
	"ServerRestSignUrl": "https://ais.swisscom.com/AIS-Server/rs/v1.0/sign",
	"ServerRestPendingUrl": "https://ais.swisscom.com/AIS-Server/rs/v1.0/pending",
	"SkipServerCertificateValidation": true,
	"ClientAuthKeyFile": "d:/home/user/ais-client.key",
	"ClientAuthKeyPassword": "secret",
	"ClientCertFile": "d:/home/user/ais-client.crt",
  "ClientHttpMaxConnectionsPerRoute": "10",
  "ClientHttpResponseTimeoutInSeconds": "20",
	"ClientPollIntervalInSeconds": "10",
	"ClientPollRounds": "10",
	"SignatureStandard": "PAdES-Baseline",
	"SignatureRevocationInformation": "PAdES",
	"SignatureAddTimestamp": "true",
	"SignatureClaimedIdentityName": "ais-90days-trial",
	"SignatureClaimedIdentityKey": "keyEntity",
	"SignatureDistinguishedName": "cn=TEST User, givenname=Max, surname=Maximus, c=US, serialnumber=abcdefabcdefabcdefabcdefabcdef",
	"SignatureStepUpLanguage": "en",
	"SignatureStepUpMsisdn": "40799999999",
	"SignatureStepUpMessage": "Please confirm the signing of the document",
	"SignatureStepUpSerialNumber": "",
	"SignatureName": "TEST Signer",
	"SignatureReason": "Testing signature",
	"SignatureLocation": "Testing location",
	"SignatureContactInfo": "tester.test@test.com",
};
```

Second create the following configuration objects, one for the REST client 
([RestClientConfiguration](../AisClient/AIS/Rest/RestClientConfiguration.cs)) and one for the AIS client 
([AISClientConfiguration](../AisClient/AIS/AisClientConfiguration.cs)). This needs to be done once per 
application lifetime, as the AIS client, once it is created and properly configured, can be reused over and over for each incoming request. It is 
implemented in a thread-safe way and makes use of proper HTTP connection pooling in order to correctly reuse resources.

Configure the REST client:
```C#
RestClientConfiguration restClientConfiguration = new RestClientConfiguration(properties);
IRestClient restClient = new RestClient(restClientConfiguration);
```

Then create the AIS client configuration:
```C#
AisClientConfiguration aisClientConfiguration = new AisClientConfiguration(properties);
```

Finally, create the AIS client with these objects:
```C#
 IAisClient aisClient = new AisClient(restClient, aisClientConfiguration);
```

Once the client is up and running, you can request it to sign and/or timestamp documents. For this, a 
[UserData](../AisClient/AIS/Model/UserData.cs) object is needed, to specify all the details required for the signature
or timestamp.

```C#
UserData userData = new UserData(properties);
userData.ConsentUrlCallback.OnConsentUrlReceived += (sender, e) => Console.WriteLine("Consent URL: " + e.Url);;
```

The last line is quite interesting. If you go with the _On Demand signature with Step Up_, there is a Consent URL that is generated and that
needs to be passed to the mobile user, so that he or she can access it, authenticate there and confirm the signature. The _UserData.ConsentUrlCallback_ class
allows you to subscribe to OnConsentUrlReceived event that is raised as soon as the URL is generated and received by the client. In the example above
the URL is just printed at the console, but in your case you might want to display it to the user by other means (web, mobile UI, etc).
Keep in mind that this even is raised EACH TIME the consent URL is received. For a signature request that goes into pending/polling mode,
this will happen each time the response comes back from the server. 

Third, you need one object (or more) that identifies the document to sign and/or timestamp. More than one document can be signed/timestamped at
a time.

```C#
 List<PdfHandle> documents = new List<PdfHandle>
                {
                    new PdfHandle
                    {
                        InputFileName = properties.LocalTestInputFile,
                        OutputFileName = properties.LocalTestOutputFile,
                        DigestAlgorithm = DigestAlgorithm.SHA256
                    }
                };
```

Finally, use all these objects to create the signature:

```java
SignatureResult signatureResult = signingService.Sign(documents, signatureMode, userData);
if (signatureResult == SignatureResult.Success) {
    // yay!
}
```

The [returned result](../AisClient/AIS/Model/SignatureResult.cs) is a coder-friendly way of finding how the signature went. 
As long as the signature terminates as caused by the mobile user (success, user cancel, user timeout) then the AIS client gracefully returns a result. 
If some other error is encountered, the client throws an [AisClientException](../AisClient/AIS/Common/AisClientException.cs).
