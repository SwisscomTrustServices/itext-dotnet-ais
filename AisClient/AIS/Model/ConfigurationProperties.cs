namespace AIS.Model
{
    public class ConfigurationProperties
    {
        public string ITextLicenseFilePath { get; set; }

        public string ServerRestSignUrl { get; set; }
        public string ServerRestPendingUrl { get; set; }
        public bool SkipServerCertificateValidation { get; set; }

        public string ClientAuthKeyFile { get; set; }
        public string ClientAuthKeyPassword { get; set; }
        public string ClientCertFile { get; set; }
        public string ClientHttpMaxConnectionsPerServer { get; set; }
        public string ClientHttpRequestTimeoutInSeconds { get; set; }
        public string ClientPollIntervalInSeconds { get; set; }
        public string ClientPollRounds { get; set; }

        public string SignatureStandard { get; set; }
        public string SignatureRevocationInformation { get; set; }
        public string SignatureAddTimestamp { get; set; }
        public string SignatureClaimedIdentityName { get; set; }
        public string SignatureClaimedIdentityKey { get; set; }
        public string SignatureDistinguishedName { get; set; }
        public string SignatureStepUpLanguage { get; set; }
        public string SignatureStepUpMsisdn { get; set; }
        public string SignatureStepUpMessage { get; set; }
        public string SignatureStepUpSerialNumber { get; set; }
        public string SignatureName { get; set; }
        public string SignatureReason { get; set; }
        public string SignatureLocation { get; set; }
        public string SignatureContactInfo { get; set; }

        public string LocalTestInputFile { get; set; }
        public string LocalTestOutputFile { get; set; }
    }
}
