//
//  Souce	Path	ResultModel.cs
//  create	Date	2017-2-12 16:43:16		
//  created	By	Ares.Zhao
//
using System;
namespace DisplayModel
{
    public class ResultModel
    {
        public string UserName { get; set; }
        public int MajorLevel { get; set; }
        public string Reason1 { get; set; }
        public string Reason2 { get; set; }
        public string Reason3 { get; set; }

        public string DisplayUserName
        {
            get
            {
                return UserName;
            }
        }
        public string DisplayMajorLevel
        {
            get
            {
                switch (MajorLevel)
                {
                    case 1:
                        return "基本完好";
                    case 2:
                        return "中等破坏";
                    default:
                        return "严重破坏";
                }
            }
        }
        public string DisplayReason1
        {
            get
            {
                switch (MajorLevel)
                {
                    case 1:
                        return "钢结构";
                    case 2:
                        return "钢筋混凝土结构";
                    case 3:
                        return "砖砌结构";
                    default:
                        return "土石结构";
                }
            }
        }
        public string DisplayReason2
        {
            get
            {
                switch (MajorLevel)
                {
                    case 1:
                        return "经过专业设计";
                    default:
                        return "没有经过专业设计";
                }
            }
        }
        public string DisplayReason3
        {
            get
            {
                switch (MajorLevel)
                {
                    case 3:
                        return "房屋的施工质量好";
                    case 2:
                        return "房屋的施工质量一般";
                    default:
                        return "房屋的施工质量差";
                }
            }
        }
    }
}

