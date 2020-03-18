using PCDH2.Core.Entities;
using PCDH2.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCDH2.Services.Contracts
{
    public interface IArticleService
    {
        public void AddArticle(Article article);
    }
}
