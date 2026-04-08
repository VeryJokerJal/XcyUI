using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static XchyUI.models.XFunctions;

namespace XchyUI.Demo.elmedemo
{
    public class DescriptionInfo
    {
        public static readonly int Demo = 0;
        public static readonly int Api = 1;
        public required string Title { get; set; }
        public required string Desription { get; set; }
        public int Type { get; set; }
        public string Tag { get; set; }
        public required XFunction ContentFunction { get; set; }
        public string? Code { get; set; }
        public bool isShowCode { get; set; }
    }
}
