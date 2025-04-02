using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicDiary.Pages.Components.Elems
{
    public class Item
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public Item(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
