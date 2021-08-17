using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Nagivation Properties
        public List<Book> Books { get; set; }
    }
}
