using System;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyEmployees.ActionFilters
{
    public class ValidateCompanyExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManger _logger;

        public ValidateCompanyExistsAttribute(IRepositoryManager repository, ILoggerManger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // we check a type of request and only if it is a PUT request we set the trackChanges to true
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            var id = (Guid) context.ActionArguments["id"];

            // we find the entity in the database,
            // we store it in HttpContext because we need that entity in our action methods and
            // we don’t want to query the database two times 
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges);

            if (company == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");

                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("company", company);
                await next();
            }
        }
    }
}