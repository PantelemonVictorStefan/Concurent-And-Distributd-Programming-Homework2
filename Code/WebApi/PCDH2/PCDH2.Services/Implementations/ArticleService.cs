using PCDH2.Core.Contracts.Repositories;
using PCDH2.Core.Entities;
using PCDH2.Services.Contracts;
using PCDH2.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCDH2.Services.Implementations
{
    public class ArticleService : BaseService<Article>, IArticleService
    {
        public ArticleService(IGenericRepository<Article> repository) : base(repository)
        {

        }
        public void AddArticle(Article article)
        {
            article.CreatedDate = DateTime.UtcNow;
            repository.Add(article);
        }
    }
}
