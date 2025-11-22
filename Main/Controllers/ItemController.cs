using Main.Base;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ItemController : ApplicationBaseController<ItemController>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public ItemController(IControllerDependencies<ItemController> dependencies) : base(dependencies)
        {
        }

        [HttpGet]
        public IActionResult GetItem()
        {
            var result = _businessHandlers.Item.GetOne();
            return Ok(result);
        }
    }
}