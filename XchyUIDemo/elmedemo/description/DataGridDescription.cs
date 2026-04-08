using System;
using XchyUI.Components;
using XchyUI.widgets;
using XchyUI.widgets.extensions;
using static XchyUI.widgets.XWidget;
using static XchyUI.Components.Compoments;
using XchyUI.views;
using XchyUI.models;

namespace XchyUI.Demo.elmedemo.description
{
    public class DataGridDescription
    {
        public class TestData
        {
            public required string Date { get; set; }
            public required string Name { get; set; }
            public required string Address { get; set; }
        }
        public static List<DescriptionInfo> GetInfos()
        {
            var datas = new List<TestData>()
            {
                new(){Date = DateTime.Now.ToString("yyyy-MM-dd"),Name = "Tom",Address = "No. 189, Grove St, Los Angeles"},
                new(){Date = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),Name = "Tom",Address = "No. 189, Grove St, Los Angeles"},
                new(){Date = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd"),Name = "Tom",Address = "No. 189, Grove St, Los Angeles"},
                new(){Date = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd"),Name = "Tom",Address = "No. 189, Grove St, Los Angeles"},
            };
            var dataState = StateValueOf(datas);
            return new List<DescriptionInfo>()
            {
                new DescriptionInfo()
                {
                    Title = "DataGrid 数据表格",
                    Tag = "DataGrid",
                    Desription = "DataGrid 一个简单大数据表格示例",
                    ContentFunction = ()=>
                    {
                        DataGrid(
                            dataState: dataState,
                            titles: ["Date", "Name", "Address"],
                            widths: [200, 200, 300],
                            valueFunction: data =>
                            {
                                return [data.Date, data.Name, data.Address];
                            },
                            itemClick: item =>
                            {
                                ShowToast("点击了"+item.Date);
                            }
                        ).Size(FILL,300);
                    },
                    Code = @"
DataGrid(
    dataState: dataState,
    titles: [""Date"", ""Name"", ""Address""],
    widths: [200, 200, 300],
    valueFunction: data =>
    {
        return [data.Date, data.Name, data.Address];
    },
    itemClick: item =>
    {
        CommonViews.ShowToast(""点击了""+item.Date);
    }
).Size(FILL,300);"
                },
            };
        }
    }
}
