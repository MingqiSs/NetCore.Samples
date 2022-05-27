﻿using KF.ElasticSearch.ESEntity.Mapping;
using KF.ElasticSearch.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace KF.ElasticSearch.Provider
{
    /// <summary>
    /// 
    /// </summary>
    public class EsSearchService : IEsSearchService, ITransientDependency
    {
        public IElasticClient _elasticClient;

        public EsSearchService(IEsClientProvider esClientProvider)
        {
            _elasticClient = esClientProvider.Client;
        }

        public IEsQueryable<T> Queryable<T>() where T : class
        {
            var mapping = InitMappingInfo<T>();
            return new QueryableProvider<T>(mapping, _elasticClient);
        }

        private static MappingIndex InitMappingInfo<T>()
        {
            return InitMappingInfo(typeof(T));
        }

        private static MappingIndex InitMappingInfo(Type type)
        {
            var mapping = new MappingIndex { Type = type, IndexName = type.Name };
            foreach (var property in type.GetProperties())
                mapping.Columns.Add(new MappingColumn
                {
                    PropertyInfo = property.PropertyType,
                    PropertyName = property.Name,
                    SearchName = FiledHelp.GetValues(property.PropertyType.Name, property.Name)
                });
            return mapping;
        }
    }
}
