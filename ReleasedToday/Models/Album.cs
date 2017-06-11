using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReleasedToday.Models
{
    public class Album
    {
        public string Id { get; set; }
        public string Artist { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public string ImageMega { get; set; }
        public string ImageLarge { get; set; }
        public string ImageExtraLarge { get; set; }
    }
}
