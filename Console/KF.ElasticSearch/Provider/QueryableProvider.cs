﻿using KF.ElasticSearch.ESEntity;
using KF.ElasticSearch.ESEntity.Mapping;
using KF.ElasticSearch.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KF.ElasticSearch.Provider
{
    public class QueryableProvider<T> : ITransientDependency, IEsQueryable<T> where T : class
    {
        public readonly IElasticClient _client;

        private readonly MappingIndex _mappingIndex;

        private readonly ISearchRequest _request;

        private long _totalNumber;

        public QueryableProvider(MappingIndex mappingIndex, IElasticClient client)
        {
            _mappingIndex = mappingIndex;
            _request = new SearchRequest(_mappingIndex.IndexName) { Size = 10000 };
            _client = client;
        }

        public QueryContainer QueryContainer { get; set; }

        public IEsQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            _Where(expression);
            return this;
        }

        public virtual List<T> ToList()
        {
            return _ToList<T>();
        }

        public virtual async Task<List<T>> ToListAsync()
        {
            return await _ToListAsync<T>();
        }

        public virtual List<T> ToPageList(int pageIndex, int pageSize)
        {
            _request.From = ((pageIndex < 1 ? 1 : pageIndex) - 1) * pageSize;
            _request.Size = pageSize;
            return _ToList<T>();
        }

        public virtual List<T> ToPageList(int pageIndex, int pageSize, ref long totalNumber)
        {
            var list = ToPageList(pageIndex, pageSize);
            totalNumber = _totalNumber;
            return list;
        }

        public virtual IEsQueryable<T> OrderBy(Expression<Func<T, object>> expression, ESOrderByType type = ESOrderByType.Asc)
        {
            _OrderBy(expression, type);
            return this;
        }

        public IEsQueryable<T> GroupBy(Expression<Func<T, object>> expression)
        {
            _GroupBy(expression);
            return this;
        }

        private static void _GroupBy(Expression expression)
        {
            // var propertyName = ReflectionExtensionHelper.GetProperty(expression as LambdaExpression).Name;
            // propertyName = _mappingIndex.Columns.FirstOrDefault(x => x.PropertyName == propertyName)?.SearchName ?? propertyName;
            // _request.Aggregations = new AggregationDictionary
            // {
            //     TermQuery = new TermsAggregation(propertyName)
            //     {
            //         Field = propertyName,
            //         Size = 1000
            //     }
            // };
        }

        private void _OrderBy(Expression expression, ESOrderByType type = ESOrderByType.Asc)
        {
            var propertyName = ReflectionExtensionHelper.GetProperty(expression as LambdaExpression).Name;
            propertyName = _mappingIndex.Columns.FirstOrDefault(x => x.PropertyName == propertyName)?.SearchName ?? propertyName;
            _request.Sort = new ISort[]
            {
                new FieldSort
                {
                    Field = propertyName,
                    Order = type == ESOrderByType.Asc ? SortOrder.Ascending : SortOrder.Descending
                }
            };
        }

        private List<TResult> _ToList<TResult>() where TResult : class
        {
            _request.Query = QueryContainer;

            var response = _client.Search<TResult>(_request);

            if (!response.IsValid)
                throw new Exception($"查询失败:{response.OriginalException.Message}");

            _totalNumber = response.Total;
            return response.Documents.ToList();
        }

        private async Task<List<TResult>> _ToListAsync<TResult>() where TResult : class
        {
            _request.Query = QueryContainer;

            var response = await _client.SearchAsync<TResult>(_request);

            if (!response.IsValid)
                throw new Exception($"查询失败:{response.OriginalException.Message}");
            _totalNumber = response.Total;
            return response.Documents.ToList();
        }

        private void _Where(Expression expression)
        {
            QueryContainer = ExpressionsGetQuery.GetQuery(expression, _mappingIndex);
        }
    }
}
