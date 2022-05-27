using KF.ElasticSearch.ESEntity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KF.ElasticSearch.Interfaces
{
    public interface IEsQueryable<T>
    {
        IEsQueryable<T> Where(Expression<Func<T, bool>> expression);

        List<T> ToList();

        Task<List<T>> ToListAsync();
        List<T> ToPageList(int pageIndex, int pageSize);
        List<T> ToPageList(int pageIndex, int pageSize, ref long totalNumber);
        IEsQueryable<T> OrderBy(Expression<Func<T, object>> expression, ESOrderByType type = ESOrderByType.Asc);

        IEsQueryable<T> GroupBy(Expression<Func<T, object>> expression);
    }
}
