using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Common
{
    public struct MenuInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MenuInfo(int id,string name)
        {
            Id = id;
            Name = name;
        }
    }
}
