using System;
using System.Collections.Generic;
using System.Text;

namespace PCDH2.Services.Models
{
    public class GenericEventArg : EventArgs
    {
        public object Data { get; set; }
    }
}
