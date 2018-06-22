using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Models
{
    public class UrlShortedModel
    {

        [DataType(DataType.Url)]
        [Required]
        public Uri Url { get; set; }

        public string ShortedHash { get; set; }
    }
}
