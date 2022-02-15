using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Entities.Models;

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
    }
}