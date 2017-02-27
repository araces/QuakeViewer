//
//  Souce   Path    QuakeViewerCalculate.cs
//  create  Date    2017-2-12 16:15:51      
//  created By  Ares.Zhao
//
using System;
namespace QuakeViewer.Test
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

        public int DamageDgreeMinor;//在多遇地震作用下 0 完好，1轻度损伤，2中度损伤，3重度损伤，4 倒塌
        public int DamageDgreeMajor;//在罕遇地震作用下 0 完好，1轻度损伤，2中度损伤，3重度损伤，4 倒塌


        public QuakeViewerCalculate()
        {
        }



        /// <summary>
        /// Inputs the data.
        /// </summary>
        /// <param name="groupNo">抗震分组</param>
        /// <param name="siteType">场地类别</param>
        /// <param name="intensityDegree">设防烈度</param>
        /// <param name="storyNum">楼层</param>
        /// <param name="struType">建筑结构 1 steel; 2 RC;3 Mazonry; 4 Earth and Stone; Default 2</param>
        /// <param name="isDesigned">是否经过设计 designed or owner-built</param>
        /// <param name="contructionQuality">施工质量 1 good; 2 fair; 3 poor;</param>
        /// <param name="builtYearGroup">建造年代 //1 before 1980;2 1980-1990; 3 1990-2000; 4 2000- Default 2000-</param>

        public void InputData(int groupNo, int siteType, double intensityDegree,
                       int storyNum,
                       int struType,
                              bool isDesigned,
                       int contructionQuality,
                              int builtYearGroup
                       )
        {

            this.groupNo = groupNo;
            this.siteType = siteType;
            this.intensityDegree = intensityDegree;
            this.storyNum = storyNum;
            this.struType = struType;
            this.builtYearGroup = builtYearGroup;
            this.contructionQuality = contructionQuality;
            this.isDesigned = isDesigned;
            Console.WriteLine("GroupNo" + groupNo + "SiteType" + siteType + "IntensityDegree" + intensityDegree);
            Console.WriteLine("StoryNum" + storyNum + "StruType" + struType + "BuiltYearGroup" + builtYearGroup);
            Console.WriteLine("ContructionQuality" + contructionQuality + "isDesigned" + isDesigned + "StructureDataIsFormed" + structureDataIsFormed);


        }
        public void FormStructure()
        {
            double ductility = 5.0;
            double reduction = 0.35;

            double DesignIntensity = intensityDegree;
            Tg = GetTg(ref groupNo, ref siteType);
            if (isDesigned == false && intensityDegree > 7) DesignIntensity = 7;
            double MaxAlpha;
            MaxAlpha = GetMaxAlphaMajor(DesignIntensity);
            T = GetT(storyNum, struType);

            Console.WriteLine("GroupNo" + groupNo + "SiteType" + siteType + "IntensityDegree" + intensityDegree);
            Console.WriteLine("StoryNum" + storyNum + "StruType" + struType + "BuiltYearGroup" + builtYearGroup);
            Console.WriteLine("ContructionQuality" + contructionQuality + "isDesigned" + isDesigned + "StructureDataIsFormed" + structureDataIsFormed);

            if (builtYearGroup == 1) reduction = 0.85 * reduction;
            else if (builtYearGroup == 2) reduction = 0.9 * reduction;
            else if (builtYearGroup == 3) reduction = 0.95 * reduction;
            else reduction = 1.0 * reduction;

            if (contructionQuality == 1) reduction = 1.1 * reduction;
            else if (builtYearGroup == 2) reduction = 1.0 * reduction;
            else if (builtYearGroup == 3) reduction = 0.9 * reduction;
            else
            {
            }

            Fy = reduction * GetSpectralSeismicFactor(T, Tg, MaxAlpha, 0.05); //Unit: Acel,g,相当于无量纲
            if (struType == 3) Fy = 0.9 * reduction * GetSpectralSeismicFactor(T, Tg, MaxAlpha, 0.05);
            else if (struType == 4) Fy = 0.6 * reduction * GetSpectralSeismicFactor(T, Tg, MaxAlpha, 0.05);
            else if (struType == 2) Fy = 1.1 * reduction * GetSpectralSeismicFactor(T, Tg, MaxAlpha, 0.05);

            Dy = Fy * T * T * 9.8 / (4.0 * 3.14 * 3.14);//unit: m
                                                        //define ductility

            if (struType == 1) Du = ductility * Dy;
            else if (struType == 2) Du = 1.3 * ductility * Dy;
            else if (struType == 3) Du = 0.9 * ductility * Dy;
            else if (struType == 4) Du = 0.5 * ductility * Dy;
            else
                Du = 0.5 * ductility * Dy;

            if (isDesigned == false)
            {
                Fy = 0.6 * reduction * GetSpectralSeismicFactor(T, Tg, MaxAlpha, 0.05);
                Dy = Fy * T * T * 9.8 / (4.0 * 3.14 * 3.14);
                Du = 0.5 * ductility * Dy;
            }

            structureDataIsFormed = true;

            Console.WriteLine("GroupNo" + groupNo + "SiteType" + siteType + "IntensityDegree" + intensityDegree);
            Console.WriteLine("StoryNum" + storyNum + "StruType" + struType + "BuiltYearGroup" + builtYearGroup);
            Console.WriteLine("ContructionQuality" + contructionQuality + "isDesigned" + isDesigned + "StructureDataIsFormed" + structureDataIsFormed);

        }
        public void ResponseMinor()
        {
            if (structureDataIsFormed == false)
            {
                this.FormStructure();
            }
            double MaxAlpha;
            double DampingRatio = 0.05;
            double ADampingRatio1 = 0;
            double ADampingRatio2 = 0;
            double T1 = 0;
            double d1 = 0;

            MaxAlpha = GetMaxAlphaMinor(intensityDegree);

            //判断是否弹性
            if (Fy >= MaxAlpha)
            {
                DamageDgreeMinor = 0; //no damage
                if (0.6 * Fy < GetSpectralSeismicFactor(T, Tg, MaxAlpha, DampingRatio))
                    DamageDgreeMinor = 1;
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    DampingRatio = ADampingRatio1 + 0.05;
                    T1 = GetTbaseAlpha(Fy, Tg, MaxAlpha, DampingRatio);
                    d1 = T1 * T1 * Fy * 9.8 / (4.0 * 3.14 * 3.14);
                    ADampingRatio2 = GetAdditionalDampingRatio(Dy, d1);
                    if (Math.Abs(ADampingRatio1 - ADampingRatio2) < 0.05 * ADampingRatio1)
                        break;
                    else
                        ADampingRatio1 = 0.5 * (ADampingRatio1 + ADampingRatio2);
                }

                if (d1 > (Dy + Du) * 0.5 && d1 < Du)
                {
                    DamageDgreeMinor = 3;
                }
                else if (d1 > Du)
                {
                    DamageDgreeMinor = 4;
                }
                else DamageDgreeMinor = 2;

            }
        }
        public void ResponseMajor()
        {
            if (structureDataIsFormed == false)
            {
                this.FormStructure();
            }
            double MaxAlpha;
            double DampingRatio = 0.05;
            double ADampingRatio1 = 0;
            double ADampingRatio2 = 0;
            double T1 = 0;
            double d1 = 0;
            MaxAlpha = GetMaxAlphaMajor(intensityDegree);

            //判断是否弹性
            if (Fy >= MaxAlpha)
            {
                DamageDgreeMajor = 0; //no damage
                if (0.6 * Fy < GetSpectralSeismicFactor(T, Tg, MaxAlpha, DampingRatio))
                    DamageDgreeMajor = 1;
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    DampingRatio = ADampingRatio1 + 0.05;
                    T1 = GetTbaseAlpha(Fy, Tg, MaxAlpha, DampingRatio);
                    d1 = T1 * T1 * Fy * 9.8 / (4.0 * 3.14 * 3.14);
                    ADampingRatio2 = GetAdditionalDampingRatio(Dy, d1);
                    if (Math.Abs(ADampingRatio1 - ADampingRatio2) < 0.05 * ADampingRatio1)
                        break;
                    else
                        ADampingRatio1 = 0.5 * (ADampingRatio1 + ADampingRatio2);
                }

                if (d1 > (Dy + Du) * 0.5 && d1 < Du) DamageDgreeMajor = 3;
                else if (d1 > Du) DamageDgreeMajor = 4;
                else DamageDgreeMajor = 2;

            }
        }


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
            Console.WriteLine("GetTg" + tgData[GroupType - 1, SiteType - 1]);
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
            Console.WriteLine("GetMaxAlphaMajor" + maxAlpha);
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
            Console.WriteLine("GetMaxAlphaMinor" + maxAlpha);
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

            if (T >= 0.0 && T < 0.1)
            {
                seismicFactor = (0.45 + (eta2 - 0.45) * T / 0.1) * MaxAlpha;
            }
            else if (T >= 0.1 && T < Tg) seismicFactor = eta2 * MaxAlpha;
            else if (T >= Tg && T < 5.0 * Tg) seismicFactor = Math.Pow(Tg / T, gama) * eta2 * MaxAlpha;
            else if (T >= 5.0 * Tg && T <= 6.0) seismicFactor = (Math.Pow(0.2, gama) * eta2 - eta1 * (T - 5.0 * Tg)) * MaxAlpha;// the design is limited to 6.0s, extrapolation is used
            else seismicFactor = eta2 * MaxAlpha;

            Console.WriteLine("GetSpectralSeismicFactor" + seismicFactor);
            return seismicFactor;
        }

        double GetT(int storyNum, int struType)
        {
            double T;
            if (struType == 1)
                T = 0.1 * storyNum;
            else if (struType == 2)
                T = 0.15 * storyNum;
            else if (struType == 3)
                T = 0.1 * storyNum;
            else if (struType == 4)
                T = 0.1 * storyNum;
            else
                T = 0.1 * storyNum;

            Console.WriteLine("GetT" + T);
            return T;

        }

        double GetTbaseAlpha(double Alpha, double Tg, double MaxAlpha, double DampingRatio)
        {
            double T = 0.1;
            double SeismicFactor = 1.0;
            double gama = 0.9;
            double eta1 = 0.02;
            double eta2 = 1.0;

            gama = 0.9 + (0.05 - DampingRatio) / (0.3 + 6.0 * DampingRatio);
            eta1 = 0.02 + (0.05 - DampingRatio) / (4.0 + 32.0 * DampingRatio);
            eta2 = 1.0 + (0.05 - DampingRatio) / (0.08 + 1.6 * DampingRatio);

            if (Alpha < MaxAlpha && Alpha >= Math.Pow(0.2, gama) * eta2 * MaxAlpha)
                T = Tg / Math.Pow((Alpha / eta2 / MaxAlpha), 1.0 / gama);
            else if (Alpha < Math.Pow(0.2, gama) * eta2 * MaxAlpha)
                T = 1.0 / eta1 * (eta2 * Math.Pow(0.2, gama) - Alpha / MaxAlpha) + 5.0 * Tg;
            else T = Tg;
            Console.WriteLine("GetTbaseAlpha" + T);
            return T;
        }

        double GetAdditionalDampingRatio(double dy, double d)
        {
            double result = (d / dy - 1) / 3.14 / (d / dy);
            Console.WriteLine("GetAdditionalDampingRatio" + result);
            return (d / dy - 1) / 3.14 / (d / dy);
        }



    }
}
