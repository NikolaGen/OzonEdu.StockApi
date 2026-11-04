using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace OzonEdu.StockApi.Infastructure.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute // чтобы фильтры заработали надо в класс Startup в ConfigureServices в AddControllers добавить запись
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }
        
        public override void OnException(ExceptionContext context) // метод обработки необработанного исключения, ExceptionContext - объект, содержащий информацию по необработанному исключению
        {
            base.OnException(context);
            var resultObject = new // Здесь создаётся анонимный объект (объект без заранее объявленного класса) — он потом превратится в JSON
            {
                ExceptionType = context.Exception.GetType().FullName,
                StackTrace = context.Exception.StackTrace
            };
            
            var jsonResult = new JsonResult(resultObject)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            _logger.LogError(context.Exception, context.Exception.Message);
            context.Result = jsonResult;
        }
    }
}