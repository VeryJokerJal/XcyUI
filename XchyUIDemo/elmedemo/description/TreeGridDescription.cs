using System.Net.NetworkInformation;
using XchyUI.Components;
using XchyUI.expansions;
using XchyUI.models;
using XchyUI.views;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.widgets.XWidget;

namespace XchyUI.Demo.elmedemo.description
{
    public class TreeGridItem()
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public DateTime Date { get; set; }
        public int Level { get; set; }
        public bool IsExpand { get; set; }
        public bool HasChild { get; set; }
    }
    public class TreeGridDescription
    {
        private static List<TreeGridItem> items = new List<TreeGridItem>()
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

        private static XState<List<TreeGridItem>> itemsState = new XState<List<TreeGridItem>>();

        private static void ToggleFileItem(TreeGridItem item)
        {
            var list = new List<TreeGridItem>(itemsState.Value);
            item.IsExpand = !item.IsExpand;
            var index = list.IndexOf(item);
            if (item.IsExpand)
            {
                var childList = new List<TreeGridItem>();
                AddChild(childList, item);
                list.InsertRange(index + 1, childList);
            }
            else
            {
                RemoveChild(list, item);
            }
            itemsState.Value = list;
        }
       
        public static void AddChild(List<TreeGridItem> list, TreeGridItem item)
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

        public static void RemoveChild(List<TreeGridItem> list, TreeGridItem item)
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
            var list = new List<TreeGridItem>();
            var root = items.Where(n => n.Id == 0);
            foreach (var item in root)
            {
                list.Add(item);
                var childList = new List<TreeGridItem>();
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
                    Title = "TreeGrid 树组件",
                    Tag = "TreeGrid",
                    Desription = "TreeGrid 树表格",
                    ContentFunction = ()=>
                    {
                        
                        LazyColumn(itemsState, datas =>
                        {
                            LazyItem(datas,true, item =>
                            {
                                Row(() =>
                                {
                                    NodeCell(item);
                                    TextCell(item.Sex == 1?"男":"女");
                                    TextCell("测试类容");
                                    TextCell(item.Date.ToShortDateString(),120);
                                });
                            })
                            .OnViewSetting<TreeGridItem>((builder,item)=>
                            {
                                builder.Background(xTheme.Colors.Background).BottomBorder();
                            });
                        }).Size(FILL,300).DefaultBorder().Background(xTheme.Colors.Background).Shadow();
                    },
                    Code = @""
                },
            };
        }

        private static void NodeCell(TreeGridItem item,int width = 200)
        {
            Row(() =>
            {
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
            }).Space(10).Size(width,60).RightBorder();
            // 层次感空白占位
        }

        private static void TextCell(string text,int width = 100)
        {
            Text(text).Size(width, 60).TextAlignment(XAlignment.Center).RightBorder();
            // 层次感空白占位
        }
    }
}
