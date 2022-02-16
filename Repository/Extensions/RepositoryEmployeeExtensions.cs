using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using Entities.Models;
using Repository.Extensions.Utility;

namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge,
            uint maxAge)
        {
            return employees.Where(emp => (emp.Age >= minAge && emp.Age <= maxAge));
        }

        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return employees;
            }

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return employees.Where(emp => emp.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
        {
            // If it is null or empty, we just return the same collection ordered by name.
            if (string.IsNullOrWhiteSpace(orderByQueryString)) return employees.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

            // // we are splitting our query string to get the individual fields:
            // var orderParams = orderByQueryString.Trim().Split(',');
            // var propertyInfos = typeof(Employee).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            // var orderQueryBuilder = new StringBuilder();
            //
            // foreach (var param in orderParams)
            // {
            //     if (string.IsNullOrWhiteSpace(param)) continue;
            //
            //     var propertyFromQueryName = param.Split(" ")[0];
            //     // we can actually run through all the parameters and check for their existence
            //     var objectProperty = propertyInfos.FirstOrDefault(pi =>
            //         pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            //
            //     // If we don’t find such a property, we skip the step in the foreach loop
            //     // and go to the next parameter in the list
            //     if (objectProperty == null) continue;
            //
            //     // If we do find the property, we return it and additionally
            //     // check if our parameter contains “desc” at the end of the string. 
            //     var direction = param.EndsWith(" desc") ? "descending" : "ascending";
            //     orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            // }
            //
            // var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            // Console.WriteLine("orderQuery:" + orderQuery);
            
            if (string.IsNullOrWhiteSpace(orderQuery)) return employees.OrderBy(e => e.Name);
            
            // we can order our query
            return employees.OrderBy(orderQuery);
        }
    }
}