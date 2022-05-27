using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace KF.ElasticSearch.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEsClientService
    {
        /// <summary>
        /// 
        /// </summary>
        ElasticClient Client { get; }
    }
}
