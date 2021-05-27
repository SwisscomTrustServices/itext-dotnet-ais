# Configure the AIS client
To use the AIS client, you first have to [obtain it (or build it)](build-or-download.md), then you have to configure it. The way you configure
the client depends a lot on how you plan to use the client and integrate it in your project/setup.

## Configuration file
The AIS client can be configured from a Json configuration file. Here is an example of such a file(a sample version without the comments is available in the release - "config.json"):

```properties
{
// The iText license file path. If missing the library will not attempt to load the license
	"ITextLicenseFilePath": "d:/home/user/license-file.txt",
  
// The AIS server REST URL for sending the Signature requests
	"ServerRestSignUrl": "https://ais.swisscom.com/AIS-Server/rs/v1.0/sign",
// The AIS server REST URL for sending the Signature status poll requests (Pending requests)
	"ServerRestPendingUrl": "https://ais.swisscom.com/AIS-Server/rs/v1.0/pending",
// Option to skip server certificate validation if the Swisscom certificate is not trusted on your machine. 
	"SkipServerCertificateValidation": true,
 
// The client's private key file (corresponding to the public key attached to the client's certificate)
	"ClientAuthKeyFile": "d:/home/user/ais-client.key",
// The password of the client's private key.
	"ClientAuthKeyPassword": "secret",
// The client's certificate file
	"ClientCertFile": "d:/home/user/ais-client.crt",
// The maximum number of connections PER ROUTE that the HTTP client used by the AIS client can use
	"ClientHttpMaxConnectionsPerRoute": "10",
// The HTTP response timeout in SECONDS (the maximum time allowed for the HTTP client to wait for the response to be received
// for any one request until the request is dropped and the client gives up).
	"ClientHttpResponseTimeoutInSeconds": "20",
// The interval IN SECONDS for the client to poll for signature status (for each parallel request).
	"ClientPollIntervalInSeconds": "10",
// The total number of rounds (including the first Pending request) that the client runs for each parallel request. After this
// number of rounds of calling the Pending endpoint for an ongoing request, the client gives up and signals a timeout for that
// respective request.
	"ClientPollRounds": "10",
  
//  The standard to use for creating the signature.
// Choose from: DEFAULT, CAdES, PDF, PAdES, PAdES-Baseline, PLAIN.
// Leave it empty and the client will use sensible defaults.
	"SignatureStandard": "PAdES-Baseline",
// The type and method of revocation information to receive from the server.
// Choose from: DEFAULT, CAdES, PDF, PAdES, PAdES-Baseline, BOTH, PLAIN.
// Leave it empty and the client will use sensible defaults.
	"SignatureRevocationInformation": "PAdES",
// Whether to add a timestamp to the signature or not. Default is true.
// Leave it empty and the client will use sensible defaults.
	"SignatureAddTimestamp": "true",
  
// The AIS Claimed Identity name. The right Claimed Identity (and key, see below) must be used for the right signature type. 
	"SignatureClaimedIdentityName": "ais-90days-trial",
// The AIS Claimed Identity key. The key together with the name (see above) is used for starting the correct signature type.
	"SignatureClaimedIdentityKey": "keyEntity",
// The client's Subject DN to which the certificate is bound.
	"SignatureDistinguishedName": "cn=TEST User, givenname=Max, surname=Maximus, c=US, serialnumber=abcdefabcdefabcdefabcdefabcdef",
// The language (one of "en", "fr", "de", "it") to be used during the Step Up interaction with the mobile user.
	"SignatureStepUpLanguage": "en",
// The MSISDN (in international format) of the mobile user to interact with during the Step Up phase.
	"SignatureStepUpMsisdn": "40799999999",
// The message to present to the mobile user during the Step Up phase.
	"SignatureStepUpMessage": "Please confirm the signing of the document",
// The mobile user's Serial Number to validate during the Step Up phase. If this number is different than the one registered on the server
// side for the mobile user, the request will fail.
	"SignatureStepUpSerialNumber": "",
  
// The name to embed in the signature to be created.
	"SignatureName": "TEST Signer",
// The reason for this signature to be created.
	"SignatureReason": "Testing signature",
// The location where the signature is created.
	"SignatureLocation": "Testing location",
// The contact info to embed in the signature to be created.
	"SignatureContactInfo": "tester.test@test.com"
  
// name of the input file to be used if running the unit tests
	"LocalTestInputFile": "in.pdf",
// name of the output file to be used if running the unit tests
	"LocalTestOutputFile": "out.pdf"
}
```

Once you create this file and configure its properties accordingly, it can either be picked up by the AIS client when you use it via its 
CLI interface, or you can use it to populate the objects that configure the client.

*CLI usage:*
```cmd
.\CLI.exe -type ondemand-stepup -config config.json -input local-sample-doc.pdf -output test-sign.pdf
```

##Programmatic usage of config file
```C#
      string aisConfig = File.ReadAllText("config.json");
      ConfigurationProperties configurationProperties = JsonConvert.DeserializeObject<ConfigurationProperties>(aisConfig);
      AisClientConfiguration aisClientConfiguration = new AisClientConfiguration(configurationProperties);
      RestClientConfiguration restClientConfiguration = new RestClientConfiguration(configurationProperties);
      IRestClient restClient = new RestClient(restClientConfiguration);
      try
            {
                AisClient aisClient = new AisClient(restClient, aisClientConfiguration);
                SigningService signingService = new SigningService(aisClient);
                UserData userData = new UserData(configurationProperties);
                userData.ConsentUrlCallback.OnConsentUrlReceived += LogAtConsole;
                List<PdfHandle> documents = new List<PdfHandle>
                {
                    new PdfHandle
                    {
                        InputFileName = properties.LocalTestInputFile,
                        OutputFileName = properties.LocalTestOutputFile
                    }
                };
                SignatureResult signatureResult = signingService.Sign(documents, signatureMode, userData);
                Console.WriteLine($"Finished signing the document(s) with the status: {signatureResult}");
            }
```


## Programmatic configuration
In order not to repeat content, please see the 
[TestFullyProgrammaticConfiguration](../AisClient/Tests/TestFullyProgramaticConfiguration.cs) sample class for how 
this can be implemented.

