using Microsoft.AspNetCore.Mvc;
using Nest;
using TestElasticSearch.Models;

namespace TestElasticSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class ProductController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IElasticClient elasticClient, ILogger<ProductController> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> Get(string keyWord)
        {
            var result = await _elasticClient.SearchAsync<Product>(
                p => p.Query(q => q.QueryString(qs => qs.Query($"*{keyWord}*")))
                .Size(1000)
            );
            return Ok(result.Documents.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            await _elasticClient.IndexDocumentAsync(product);
            return Ok();
        }
    }
}