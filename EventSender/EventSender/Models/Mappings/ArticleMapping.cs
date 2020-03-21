using NewsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSender.Models.Mappings
{
    public static class ArticleMapping
    {
        public static IEnumerable<ArticleModel> Map(this IEnumerable<Article> articles, string keyword)
        {
            return articles.Select(a => a.Map(keyword));
        }

        public static ArticleModel Map(this Article article, string keyword)
        {
            return new ArticleModel
            {
                Id = Guid.NewGuid(),
                Title = article.Title,
                Description = article.Description,
                Url = article.Url,
                ImageUrl = article.UrlToImage,
                Keyword = keyword
            };
        }
    }
}
