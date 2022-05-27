using System;
using System.Collections.Generic;
using System.Text;

namespace KF.ElasticSearch.Interfaces
{
    public interface IEsSearchService
    {
        IEsQueryable<T> Queryable<T>() where T : class;
    }
}
