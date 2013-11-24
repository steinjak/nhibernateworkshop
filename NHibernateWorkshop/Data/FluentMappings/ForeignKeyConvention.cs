using System;
using FluentNHibernate;

namespace NHibernateWorkshop.Data.FluentMappings
{
    public class ForeignKeyConvention : FluentNHibernate.Conventions.ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            return (property != null ? property.Name : type.Name) + "Id";
        }
    }
}