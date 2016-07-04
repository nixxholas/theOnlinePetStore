using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBA_ASSIGNMENT.Models
{
    /**
     * Preset Metric Class
     * By Nicholas
     * 
     * An automatically seeded version for the Metrics class.
     * Users who are creating their customized metrics would
     * need to refer to the standard metric types such as
     * kilograms or even tonnes. However, because the metric 
     * class allows full user customization, this class will
     * be the table storing all the default value types.
     * 
     * **/
    public class PresetMetric
    {
        // Let's provide each Preset Metric it's own Id
        public int PMetricId { get; set; }
        // The type of metric it is (weight, volume) etc...
        public String MetricType { get; set; }
        // The Metric name it is (grams, litres)
        public String MetricSubType { get; set; }
    }
}
