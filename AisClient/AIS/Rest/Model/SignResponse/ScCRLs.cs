using System.Collections.Generic;
using Newtonsoft.Json;

namespace AIS.Rest.Model.SignResponse
{
    public class ScCRLs
    {
        [JsonProperty("sc.CRL")] public List<string> ScCRL { get; set; }
    }
}

    
