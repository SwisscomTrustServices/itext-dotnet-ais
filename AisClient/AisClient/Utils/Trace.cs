namespace AIS.Utils
{
    /**
 * Bundles tracking information for an ongoing transaction/request. It is meant to be passed from one layer to another and
 * its information be used whenever logging/auditing/tracing activities are performed.
 */
    public class Trace
    {
        /**
         * The ID of this trace. In most cases this is an UUID.
         */
        public string Id { get; set; }

        public Trace(string id)
        {
            Id = id;
        }

    }
}
