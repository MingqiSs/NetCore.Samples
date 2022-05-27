using System;
using System.Collections.Generic;
using System.Text;

namespace KF.ElasticSearch.ESEntity.Mapping
{
    public class MappingIndex
    {
        public Type Type { get; set; }

        public string IndexName { get; set; }

        public List<MappingColumn> Columns { get; set; } = new List<MappingColumn>(0);
    }
}
