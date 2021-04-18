﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class PropertyMapping<TSource,TDestination>: IPropertyMapping
    {
        public Dictionary<string,PropertyMappingValue> MappingDictionnary { get; private set; }

        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionnary = null)
        {
            MappingDictionnary = mappingDictionnary??throw new ArgumentNullException(nameof(mappingDictionnary));
        }
    }
}
