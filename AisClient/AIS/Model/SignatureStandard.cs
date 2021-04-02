﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AIS.Model
{
    public class SignatureStandard
    {
        /**
     * The default signature standard that is used. This is normally CAdES but the actual decision is left to the AIS service.
     */
        public static readonly string Default = "";

        /**
         * CAdES compliant signature.
         */
        public static readonly string Cades = "CAdES";

        /**
         * Formerly named PAdES: Adds to the CMS a revocation info archival attribute as described in the PDF reference.
         */
        public static readonly string Pdf = "PDF";

        /**
         * Alias for PDF for backward compatibility. Since 1st of December 2020, the PADES signature standard has been replaced
         * with the PDF option, to better transmit the idea that the revocation information archival attribute is added to the
         * CMS signature that is returned to the client, as per the PDF reference. This signature standard (PADES) is now
         * deprecated and should not be used. Use instead the PDF one, which has the same behaviour.
         *
         * @deprecated Please use the {@link #PDF} element.
         */
        public static readonly string Pades = "PAdES";

        /**
         * PAdES compliant signature, which returns the revocation information as optional output.
         * In order to get an LTV-enabled PDF signature, the client must process the optional output and fill the PDF's DSS (this AIS client
         * library already does this for your). This is in contrast with the PDF option (see above) that embeds the revocation information
         * as an archival attribute inside the CMS content, which might trip some strict checkers (e.g. ETSI Signature Conformance Checker).
         */
        public static readonly string Pades_baselines = "PAdES-baseline";

        /**
         * Plain signature, which returns revocation information as optional output.
         */
        public static readonly string Plain = "PLAIN";

        public static readonly List<string> SignatureStandards = new List<string>
        {
            Default,
            Cades,
            Pdf,
            Pades,
            Pades_baselines,
            Plain
        };

        public readonly string Standard;

        public SignatureStandard(string standard)
        {
            Standard = standard;
        }
    }
}
