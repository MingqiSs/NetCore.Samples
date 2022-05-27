using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace KF.ElasticSearch.Interfaces
{
    public interface IEsClientProvider
    {
        ElasticClient Client { get; }
    }
}
