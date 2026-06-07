using Demo.Common;

namespace Demo.Service
{
    public class DemoService
    {
        public List<MenuInfo> GetMenuInfo()
        {
            return [
                new(1,"菜单1"),
                new(2,"菜单2"),
                new(3,"菜单3"),
                new(4,"菜单4"),
                new(5,"菜单5"),
                new(6,"菜单6"),
                ];
        }
    }
}
