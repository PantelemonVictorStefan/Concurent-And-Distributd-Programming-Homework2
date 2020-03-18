using System;
using System.Collections.Generic;
using System.Text;

namespace PCDH2.Core.Entities
{
    public class Article : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }
}
