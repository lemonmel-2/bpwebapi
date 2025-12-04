using Microsoft.AspNetCore.Mvc;

namespace webapi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        // private readonly ProductContext _context;

        public GameController()
        {
            // _context = context;

            // if (_context.Products.Count() == 0)
            // {
            //     // Create a new Product if collection is empty,
            //     // which means you can't delete all Products.
            //     _context.Products.Add(new Product { Name = "Sample Product", Price = 9.99M });
            //     _context.SaveChanges();
            // }
        }

        [HttpPost]
        public ActionResult<string> Auth()
        {
            return "hello";
        }

        [HttpPost]
        public ActionResult<string> RecordScore()
        {
            return "hello";
        }

        [HttpGet]
        public ActionResult<string> ShowItem()
        {
            return "hello";
        }

        [HttpGet]
        public ActionResult<string> GenerateItem()
        {
            return "hello";
        }

        [HttpGet]
        public ActionResult<string> ShowLeaderboard()
        {
            return "hello";
        }

        // [HttpGet("{id}", Name = "GetProduct")]
        // public ActionResult<Product> GetById(int id)
        // {
        //     var product = _context.Products.Find(id);

        //     if (product == null)
        //     {
        //         return NotFound();
        //     }

        //     return product;
        // }
    }
}