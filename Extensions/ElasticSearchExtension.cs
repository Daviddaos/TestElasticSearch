using Nest;
using TestElasticSearch.Models;

namespace TestElasticSearch.Extensions
{
    public static class ElasticSearchExtension
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ElasticConfiguration:Uri"];
            var defaultIndex = configuration["ElasticConfiguration:Index"];
            var settings = new ConnectionSettings(new Uri(url)).PrettyJson().DefaultIndex(defaultIndex);
            AddDefaultIndex(settings);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultIndex(ConnectionSettings connectionSettings)
        {
            connectionSettings.DefaultMappingFor<Product>(p => p
                .Ignore(i => i.Price)
                .Ignore(i => i.Id)
                .Ignore(i => i.Quantity));
        }

        private static void CreateIndex(ElasticClient elasticClient, string indexName)
        {
            elasticClient.Indices.Create(indexName, i => i.Map<Product>(p => p.AutoMap()));
        }
    }
}