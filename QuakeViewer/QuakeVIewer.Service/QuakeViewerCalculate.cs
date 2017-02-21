//
//  Souce	Path	QuakeViewerCalculate.cs
//  create	Date	2017-2-12 16:15:51		
//  created	By	Ares.Zhao
//
using System;
namespace QuakeViewer.Utils
{
    public class QuakeViewerCalculate
    {
        double[,] tgData = new double[,] { { 0.20, 0.25, 0.35, 0.45, 0.65 }, { 0.25, 0.30, 0.40, 0.55, 0.75 }, { 0.30, 0.35, 0.50, 0.65, 0.90 } };

        double[] maxAlphaDataMajor = new double[] { 0.28, 0.50, 0.90, 1.40, 0.72, 1.20 };//6,7,8,9,7.5,8.5 degree//罕遇地震系数
        double[] maxAlphaDataMinor = new double[] { 0.04, 0.08, 0.16, 0.32, 0.12, 0.24 };//6,7,8,9,7.5,8.5 degree//多遇地震系数
        /*
         * input parameters
         */
        int groupNo;//抗震分组
        int siteType;//场地类别
        double intensityDegree; //设防烈度
        int storyNum;//Number of Story
        int struType;//1 RC; 2 steel;3 Mazonry; 4 Earth and Stone; Default 1
        int builtYearGroup;//1 before 1980;2 1980-1990; 3 1990-2000; 4 2000- Default 2000-
        int contructionQuality;//1 good; 2 fair; 3 poor;
        bool isDesigned;//designed or owner-built
        bool structureDataIsFormed;

        /*
         * mechanical parameters
         *
         */
        double Tg;
        double T;
        double Fy;//无量纲，与Sa/g的单位一致
        double Dy;
        double Du;

        int DamageDgreeMinor;//在多遇地震作用下 0 完好，1轻度损伤，2中度损伤，3重度损伤，4 倒塌
        int DamageDgreeMajor;//在罕遇地震作用下 0 完好，1轻度损伤，2中度损伤，3重度损伤，4 倒塌


        public QuakeViewerCalculate()
        {
        }

        public static void Calculate(int groupNo, int siteType, int intensityDegree, int second, int third, int forth, int fifth, int sixth, out int minor, out int major)
        {
            minor = 1;
            major = 2;
        }

        /// <summary>
        /// Inputs the data.
        /// </summary>
        /// <param name="groupNo">抗震分组</param>
        /// <param name="siteType">场地类别</param>
        /// <param name="intensityDegree">设防烈度</param>
        /// <param name="storyNum">楼层</param>
        /// <param name="struType">建筑结构 1 steel; 2 RC;3 Mazonry; 4 Earth and Stone; Default 2</param>
        /// <param name="builtYearGroup">建造年代 //1 before 1980;2 1980-1990; 3 1990-2000; 4 2000- Default 2000-</param>
        /// <param name="contructionQuality">施工质量 1 good; 2 fair; 3 poor;</param>
        /// <param name="isDesigned">是否经过设计 designed or owner-built</param>
        void InputData(int groupNo, int siteType, double intensityDegree,
                       int storyNum,
                       int struType,
                       int builtYearGroup,
                       int contructionQuality,
                       bool isDesigned)
        {

            this.groupNo = groupNo;
            this.siteType = siteType;
            this.intensityDegree = intensityDegree;
            this.storyNum = storyNum;
            this.struType = struType;
            this.builtYearGroup = builtYearGroup;
            this.contructionQuality = contructionQuality;
            this.isDesigned = isDesigned;

            double ductility = 5.0;
            double reduction = 3.5;
            double DesignIntensity = intensityDegree;


        }
        void FormStructure();
        void ResponseMinor();
        void ResponseMajor();


        double GetTg(ref int GroupType, ref int SiteType)
        {
            if (GroupType < 1 || GroupType > 3)
            {
                GroupType = 1;
            }
            if (SiteType < 1 || SiteType > 5)
            {
                SiteType = 2;
            }
            return tgData[GroupType - 1, SiteType - 1];
        }

        double GetMaxAlphaMajor(double intensiteScale)
        {
            double maxAlpha = 0;
            if (intensiteScale == 7.5)
            {
                maxAlpha = maxAlphaDataMajor[4];
            }
            else if (intensiteScale == 8.5)
            {
                maxAlpha = maxAlphaDataMajor[5];
            }
            else if (intensiteScale >= 6.0 && intensiteScale <= 9.0)
            {
                int index = (int)Math.Floor(intensiteScale) - 6;
                maxAlpha = maxAlphaDataMajor[index];
            }
            else
            {
                maxAlpha = maxAlphaDataMajor[1];
            }
            return maxAlpha;
        }

        double GetMaxAlphaMinor(double intensiteScale)
        {
            double maxAlpha = 0;
            if (intensiteScale == 7.5)
            {
                maxAlpha = maxAlphaDataMinor[4];
            }
            else if (intensiteScale == 8.5)
            {
                maxAlpha = maxAlphaDataMinor[5];
            }
            else if (intensiteScale >= 6.0 && intensiteScale <= 9.0)
            {
                int index = (int)Math.Floor(intensiteScale) - 6;
                maxAlpha = maxAlphaDataMinor[index];
            }
            else
            {
                maxAlpha = maxAlphaDataMinor[1];
            }
            return maxAlpha;
        }

        double GetSpectralSeismicFactor(double T, double Tg, double MaxAlpha, double DampingRatio)
        {
            double seismicFactor = 1.0;
            double gama = 0.9;
            double eta1 = 0.02;
            double eta2 = 1.0;

            gama = 0.9 + (0.05 - DampingRatio) / (0.3 + 6.0 * DampingRatio);
            eta1 = 0.02 + (0.05 - DampingRatio) / (4.0 + 32.0 * DampingRatio);
            eta2 = 1.0 + (0.05 - DampingRatio) / (0.08 + 1.6 * DampingRatio);
        }

    }
}
