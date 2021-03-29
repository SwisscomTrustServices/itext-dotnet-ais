﻿using Newtonsoft.Json;

namespace AIS.Rest.Model.SignRequest
{
    public class DocumentHash
    {
        [JsonProperty("@ID")]
        public string Id { get; set; }

        [JsonProperty("dsig.DigestMethod")]
        public DsigDigestMethod DsigDigestMethod { get; set; }

        [JsonProperty("dsig.DigestValue")]
        public string DsigDigestValue { get; set; }
    }
}
