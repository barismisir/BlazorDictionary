using BlazorDictionary.Common.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlazorDictionary.Api.WebApi.Infrastructure.ActionFilters
{
    public class ValidateModelStateFilter:IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var messages = context.ModelState.Values.SelectMany(s => s.Errors)
                                                        .Select(s => !string.IsNullOrEmpty(s.ErrorMessage)
                                                        ? s.ErrorMessage : s.Exception?.Message)
                                                        .Distinct().ToList();

                var result = new ValidationResponseModel(messages);
                context.Result = new BadRequestObjectResult(result);

                return;
            }

            await next();
        }
    }
}
