using System;
using System.Collections.Generic;
using System.Text;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.CustomerSegment
{
    public class DemoCustomerProperty
    {
        public DemoCustomerProperty(string name)
        {
            Name = name;
            Group = "All properties";
            ValueType = "ShortText";
            Values = new List<string>();
        }
        public string Name { get; set; }
        public string Group { get; set; } 
        public string ValueType { get; set; } 
        public bool IsArray { get; set; }
        public bool IsDictonary { get; set; }
        public bool IsMultilingual { get; set; }
        public IList<string> Values { get; set; } 

    }
}
