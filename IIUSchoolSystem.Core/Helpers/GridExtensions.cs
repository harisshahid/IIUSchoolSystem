using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace IIUSchoolSystem.Core.Helpers
{
    public static class GridExtensions
    {
        /// <summary>
        /// Apply the Commands Returned by the RadGrid to the Query like the Filtering, Sorting and Paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current"></param>
        /// <param name="command"></param>
        /// <param name="TotalRecords"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyGridCommandsWithPaging<T>(this IQueryable<T> current, GridCommand command, out int TotalRecords)
        {
            current = current.ApplyGridCommand(command);
            TotalRecords = current.Count();
            return current.PagedForCommand(command);
        }

        /// <summary>
        /// Apply the Commands Returned by the RadGrid to the Query like the Filtering and Sorting 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyGridCommand<T>(this IQueryable<T> current, GridCommand command)
        {
            var queryable = current.AsQueryable() as IQueryable;
            if (command.FilterDescriptors.Any())
            {
                queryable = queryable.Where(command.FilterDescriptors.AsEnumerable()).AsQueryable();
            }

            IList<SortDescriptor> temporarySortDescriptors = new List<SortDescriptor>();

            if (!command.SortDescriptors.Any() && queryable.Provider.IsEntityFrameworkProvider())
            {
                // The Entity Framework provider demands OrderBy before calling Skip.
                SortDescriptor sortDescriptor = new SortDescriptor
                {
                    Member = queryable.ElementType.FirstSortableProperty()
                };
                command.SortDescriptors.Add(sortDescriptor);
                temporarySortDescriptors.Add(sortDescriptor);
            }

            if (command.GroupDescriptors.Any())
            {
                command.GroupDescriptors.Reverse().Each(groupDescriptor =>
                {
                    SortDescriptor sortDescriptor = new SortDescriptor
                    {
                        Member = groupDescriptor.Member,
                        SortDirection =
                            groupDescriptor.SortDirection
                    };

                    command.SortDescriptors.Insert(0, sortDescriptor);
                    temporarySortDescriptors.Add(sortDescriptor);
                });
            }

            if (command.SortDescriptors.Any())
            {
                try
                {
                    queryable = queryable.Sort(command.SortDescriptors);
                }
                catch (Exception)
                { }

            }

            return queryable as IQueryable<T>;
        }

        /// <summary>
        /// Apply the Paging Commands Returned by the RadGrid to the Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static IQueryable<T> PagedForCommand<T>(this IQueryable<T> current, GridCommand command)
        {
            int pageSize = command.PageSize;
            if (pageSize == 0)
                pageSize = 10;

            int pageIndex = command.Page;
            if (pageIndex < 1)
                pageIndex = 1;

            return current.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public static bool IsEntityFrameworkProvider(this IQueryProvider provider)
        {
            return provider.GetType().FullName == "System.Data.Objects.ELinq.ObjectQueryProvider";
        }

        /// <summary>
        /// Dynamically find and return the FIrst Property of a Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string FirstSortableProperty(this Type type)
        {
            PropertyInfo firstSortableProperty = type.GetProperties().Where(property => property.PropertyType.IsPredefinedType()).FirstOrDefault();

            if (firstSortableProperty == null)
            {
                throw new NotSupportedException("Cannot find property to sort by.");
            }

            return firstSortableProperty.Name;
        }

        internal static bool IsPredefinedType(this Type type)
        {
            return PredefinedTypes.Any(t => t == type);
        }

        public static readonly Type[] PredefinedTypes = {
            typeof(Object),
            typeof(Boolean),
            typeof(Char),
            typeof(String),
            typeof(SByte),
            typeof(Byte),
            typeof(Int16),
            typeof(UInt16),
            typeof(Int32),
            typeof(UInt32),
            typeof(Int64),
            typeof(UInt64),
            typeof(Single),
            typeof(Double),
            typeof(Decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Math),
            typeof(Convert)
        };
    }
}
