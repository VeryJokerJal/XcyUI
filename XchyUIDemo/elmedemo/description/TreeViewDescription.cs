using XchyUI.Components;
using XchyUI.expansions;
using XchyUI.models;
using XchyUI.views;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.widgets.XWidget;

namespace XchyUI.Demo.elmedemo.description
{
    public class TreeItem()
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool IsExpand { get; set; }
        public bool HasChild { get; set; }
    }
    public class TreeViewDescription
    {
        private static List<TreeItem> items = new List<TreeItem>()
        {
             new(){ Name = "父节点",Id = 0,IsExpand = true, ParentId = -1},
             new(){ Name = "父节点2", Level = 1, Id = 1, ParentId =  0},
             new(){ Name = "父节点3", Level = 1, Id = 2, ParentId =  0 },
             new(){ Name = "父节点4", Level = 1, Id = 3, ParentId =  0 },
             new(){ Name = "父节点5", Level = 1, Id = 4, ParentId =  0 },
             new(){ Name = "子节点q", Level = 2, Id = 5, ParentId =  1 },
             new(){ Name = "子节点a", Level = 2, Id =6, ParentId =  2 },
             new(){ Name = "子节点b", Level = 2, Id = 7, ParentId =  3 },
             new(){ Name = "子节点c", Level = 2, Id = 8, ParentId = 3 },
             new(){ Name = "子节点d", Level = 2, Id = 9, ParentId =  3 },
        };

        private static XState<List<TreeItem>> itemsState = new XState<List<TreeItem>>();

        private static void ToggleFileItem(TreeItem item)
        {
            var list = new List<TreeItem>(itemsState.Value);
            item.IsExpand = !item.IsExpand;
            var index = list.IndexOf(item);
            if (item.IsExpand)
            {
                var childList = new List<TreeItem>();
                AddChild(childList, item);
                list.InsertRange(index + 1, childList);
            }
            else
            {
                RemoveChild(list, item);
            }
            itemsState.Value = list;
        }
       
        public static void AddChild(List<TreeItem> list, TreeItem item)
        {
            var childs = items.Where(n => n.ParentId == item.Id);
            item.HasChild = childs.Count() > 0;
            foreach (var item1 in childs)
            {
                list.Add(item1);
                item1.HasChild = items.Count(n => n.ParentId == item1.Id) >0;
                if (item1.IsExpand)
                {
                    AddChild(list, item1);
                }
            }
        }

        public static void RemoveChild(List<TreeItem> list, TreeItem item)
        {
            var childs = items.Where(n => n.ParentId == item.Id);
            foreach (var item1 in childs)
            {
                list.Remove(item1);
                if (item1.IsExpand)
                {
                    RemoveChild(list, item1);
                }
            }
        }

        private static void Init()
        {
            // 具体算法可以根据自己的实际情况来
            var list = new List<TreeItem>();
            var root = items.Where(n => n.Id == 0);
            foreach (var item in root)
            {
                list.Add(item);
                var childList = new List<TreeItem>();
                if (item.IsExpand)
                {
                    AddChild(childList, item);
                }
                list.AddRange(childList);
            }
            itemsState.Value = list;
        }
        public static List<DescriptionInfo> GetInfos()
        {
            Init();
            return new List<DescriptionInfo>()
            {
                new DescriptionInfo()
                {
                    Title = "TreeView 树组件",
                    Tag = "TreeView",
                    Desription = "TreeView 树形组件示例",
                    ContentFunction = ()=>
                    {
                        // datas 数据源自己维护,treeview/treegrid 其实就是每一行第一个格子通过空白占位来呈现树形的层次感
                        LazyColumn(itemsState, datas =>
                        {
                            LazyItem(datas,true, item =>
                            {
                                Row(() =>
                                {
                                    // 层次感空白占位
                                    Spacer().Width(item.Level * 15);
                                    // 箭头图标
                                    var arrowIcon = item.IsExpand ? SvgRes.ArrowDown : SvgRes.ArrowRight;
                                    Icon(arrowIcon)
                                    .Size(24)
                                    .Hand()
                                    .InVisible(item.HasChild)
                                    .Click(() =>
                                    {
                                        ToggleFileItem(item);
                                    }, false);
                                    // 节点名称
                                    Text(item.Name);
                                })                                
                                .Width(FILL).Space(10).Padding(10);
                            })
                            .OnViewSetting<TreeItem>((builder,item)=>
                            {
                                builder.Background(xTheme.Colors.Background);
                            });
                        }).Size(500,300).DefaultBorder().Background(xTheme.Colors.Background).Shadow();
                    },
                    Code = @""
                },
            };
        }
    }
}
