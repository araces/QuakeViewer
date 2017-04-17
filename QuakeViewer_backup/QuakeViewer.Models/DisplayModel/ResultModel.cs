//
//  Souce	Path	ResultModel.cs
//  create	Date	2017-2-12 16:43:16		
//  created	By	Ares.Zhao
//
using System;
namespace QuakeViewer.Models
{
    [Serializable]
    public class ResultModel
    {
        public string UserName { get; set; }
        public int MajorLevel { get; set; }
        public int MinorLevel { get; set; }
        public int Reason1 { get; set; }
        public int Reason2 { get; set; }
        public int Reason3 { get; set; }

        public string DisplayUserName
        {
            get
            {
                return UserName;
            }
        }
        public string DisplayMinorLevel
        {
            get
            {
                switch (MinorLevel)
                {
                    case 0:
                        return "完好";
                    case 1:
                        return "轻微破坏";
                    case 2:
                        return "中等破坏";
                    case 3:
                        return "严重破坏";
                    default:
                        return "倒塌（包括局部倒塌）";
                }
            }
        }
        public string DisplayMajorLevel
        {
            get
            {
                switch (MajorLevel)
                {
                    case 0:
                        return "完好";
                    case 1:
                        return "轻微破坏";
                    case 2:
                        return "中等破坏";
                    case 3:
                        return "严重破坏";
                    default:
                        return "倒塌（包括局部倒塌）";
                }
            }
        }
        public string DisplayReason1
        {
            get
            {
                switch (Reason1)
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
                switch (Reason2)
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
                switch (Reason3)
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

