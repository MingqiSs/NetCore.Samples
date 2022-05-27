using System;
using System.Collections.Generic;
using System.Text;

namespace KF.ElasticSearch.ESEntity.Mapping
{
    public class MappingColumn
    {
        public Type PropertyInfo { get; set; }

        public string PropertyName { get; set; }

        public string SearchName { get; set; }
    }
}
