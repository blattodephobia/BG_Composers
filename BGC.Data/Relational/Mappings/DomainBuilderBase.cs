﻿using BGC.Data.Relational.ManyToMany;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal abstract class DomainBuilderBase<TDto, TEntity>
        where TDto : RelationdalDtoBase
    {
        protected abstract TEntity BuildInternal(TDto dto);

        public TEntity Build(TDto dto)
        {
            Shield.ArgumentNotNull(dto).ThrowOnError();

            return BuildInternal(dto);
        }
    }
}