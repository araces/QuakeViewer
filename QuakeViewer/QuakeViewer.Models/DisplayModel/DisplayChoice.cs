using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuakeViewer.Models
{
   public class DisplayChoice
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstChoice { get; set; }
        public int? SecondChoice { get; set; }
        public int? ThirdChoice { get; set; }
        public int? ForthChoice { get; set; }
        public int? FifthChoice { get; set; }
        public int? Sixth { get; set; }
        public int? FromType { get; set; }
        public int? MinorResult { get; set; }
        public int? MajorResult { get; set; }
        public DateTime? CreateDate { get; set; }

        public static DisplayChoice GetDisplayChoiceFromNormakChoice(Choice choice)
        {
            DisplayChoice  thisModel=new DisplayChoice();
            thisModel.UserId = choice.UserId;
            thisModel.UserName = choice.UserName;
            thisModel.FirstChoice = choice.FirstChoice;
            thisModel.SecondChoice = choice.SecondChoice;
            thisModel.ThirdChoice = choice.ThirdChoice;
            thisModel.ForthChoice = choice.ForthChoice;
            thisModel.FifthChoice = choice.FifthChoice;
            thisModel.Sixth = choice.Sixth;
            thisModel.FromType = choice.FromType;
            thisModel.MinorResult = choice.MinorResult;
            thisModel.MajorResult = choice.MajorResult;
            thisModel.CreateDate = choice.CreateDate;

            return thisModel;
        }

        public string DisplayCreateDate {
            get { return this.CreateDate.Value.ToString("yyyy年M月d日"); }
        }
        public string DisplayUserName {
            get { return this.UserName; }
        }
        public string DisplayFirstChoice {
            get { return this.FirstChoice; }
        }
        public string DisplaySecondChoice {
            get { return $"{this.SecondChoice}层"; }
        }
        public string DisplayThirdChoice {
            get
            {
                switch (this.ThirdChoice)
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
        public string DisplayForthChoice {
            get
            {
                switch (this.ForthChoice)
                {
                    case 1:
                        return "是";
                    default:
                        return "否";
                }
            }
        }
        public string DisplayFifthChoice
        {
            get
            {
                switch (this.FifthChoice)
                {
                    case 3:
                        return "好";
                    case 2:
                        return "一般";
                    default:
                        return "差";
                }
            }
        }
        public string DisplaySixth {
            get
            {
                switch (this.Sixth)
                {
                    case 1:
                        return "1980年前";
                    case 2:
                        return "1980-1990年";
                    case 3:
                        return "1990-2000年";
                    default:
                        return "2000年后";
                }
            }
        }
        public string DisplayFromType {
            get
            {
                switch (this.FromType)
                {
                    case (int)EnumUserType.Mobile:
                        return "手机";
                    default:
                        return "网站";
                   
                }
            }
        }
        public string DisplayMinorResult {
            get
            {
                switch (this.MinorResult)
                {
                    case 0:
                        return "基本完好";
                    case 1:
                        return "基本完好";
                    case 2:
                        return "中等破坏";
                    default:
                        return "严重破坏";
                }
            }
        }
    }
}
