using FirstMVC.Models;
using FirstMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace FirstMVC.Controllers
{
    [Controller] //Контроллер
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ITest _test;
       

        public HomeController(ILogger<HomeController> logger, ITest test) //Передача зависимостей в контроллер через конструктор
        {
            _logger = logger;
            _test = test;
            var timeService = HttpContext.RequestServices.GetService<ITest>();//Передача зависимостей в контроллер через свойства
        }
        /// <summary>
        /// Действия контроллера
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Метод, который не являются действиями
        /// </summary>
        private void NonAct()
        { 
        }
        [HttpGet]
        [ActionName("Welcome")] //Изменение имени действия в запросе, передача параметров через строку запросов
        public string TestMsg(string name)
        {
            return $"Hello, {name}!";
        }

        [HttpGet]
        [ActionName("Form")] // передача параметров через форму
        public async Task TestMsg4()
        {
            string content = @"<form method='post'>
                <label>Name:</label><br />
                <input name='name' /><br />
                <label>Age:</label><br />
                <input type='number' name='age' /><br />
                <input type='submit' value='Send' />
            </form>";
            Response.ContentType = "text/html;charset=utf-8";
            await Response.WriteAsync(content);
        }
        [HttpPost]        
        public string Form(string name, int age)
        {
            return $"{name},{age}";
        }
        [HttpGet, ActionName("GetFile")] //Передача файла
        public IActionResult FileResultat(string path)
        {
           var fileName = Path.GetFileName(path);
           return PhysicalFile(path, "application/octet-stream", fileName);           
           
        }

        [ActionName("Response")] //Параметры ответа Response
        public async Task TestMsg2()
        {
            Response.ContentType = "text/html;charset=utf-8";
            await Response.WriteAsync("<h2>Response</h2>");
        }

        [ActionName("Request")] //Параметры запроса Request
        public async Task TestMsg3()
        {
            Response.ContentType = "text/html;charset=utf-8";
            System.Text.StringBuilder tableBuilder = new("<h2>Request headers</h2><table>");
            foreach (var header in Request.Headers)
            {
                tableBuilder.Append($"<tr><td>{header.Key}</td><td>{header.Value}</td></tr>");
            }
            tableBuilder.Append("</table>");
            await Response.WriteAsync(tableBuilder.ToString());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [NonAction]
        public void Dependency([FromServices]ITest test) // передача зависимости через метод
        {

        }
    }
}
