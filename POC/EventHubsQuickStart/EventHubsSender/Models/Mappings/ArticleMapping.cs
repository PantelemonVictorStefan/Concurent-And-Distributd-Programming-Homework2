using NewsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventHubsSender.Models.Mappings
{
    public static class ArticleMapping
    {
        public static IEnumerable<ArticleModel> Map(this IEnumerable<Article> articles)
        {
            return articles.Select(a => a.Map());
        }

        public static ArticleModel Map(this Article article)
        {
            return new ArticleModel
            {
                Id = Guid.NewGuid(),
                Title = article.Title,
                Description = article.Description,
                Url = article.Url,
                ImageUrl = article.UrlToImage
            };
        }
    }
}
