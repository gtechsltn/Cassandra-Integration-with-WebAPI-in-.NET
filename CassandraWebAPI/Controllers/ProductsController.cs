using CassandraWebAPI.Helpers;
using CassandraWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ISession = Cassandra.ISession;

namespace CassandraWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ISession _session;

        public ProductsController(CassandraHelper cassandraHelper)
        {
            _session = cassandraHelper.GetSession();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            product.Id = Guid.NewGuid();
            var query = $"INSERT INTO products (id, name, price) VALUES ({product.Id}, '{product.Name}', {product.Price})";
            _session.Execute(query);
            return Ok("Product created successfully.");
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(Guid id)
        {
            var query = $"SELECT * FROM products WHERE id = {id}";
            var row = _session.Execute(query).FirstOrDefault();
            if (row == null) return NotFound();

            var product = new Product
            {
                Id = id,
                Name = row.GetValue<string>("name"),
                Price = row.GetValue<decimal>("price")
            };
            return Ok(product);
        }
    }
}
