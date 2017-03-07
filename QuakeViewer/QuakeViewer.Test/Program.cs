//
//  Souce	Path	Program.cs
//  create	Date	2017-2-25 10:42:21		
//  created	By	Ares.Zhao
//
using System;
using QuakeViewer.Utils;

namespace QuakeViewer.Test
{
    class MainClass
    {
        public static void Main(string[] args)
        {


            QuakeViewerCalculate quakeViewer = new QuakeViewerCalculate();
            /// <param name="storyNum">楼层</param>
            /// <param name="struType">建筑结构 1 steel; 2 RC;3 Mazonry; 4 Earth and Stone; Default 2</param>

            /// <param name="builtYearGroup">建造年代 //1 before 1980;2 1980-1990; 3 1990-2000; 4 2000- Default 2000-</param>
            ///<param name="contructionQuality">施工质量 1 good; 2 fair; 3 poor;</param>
            /// <param name="isDesigned">是否经过设计 designed or owner-built</param>

            quakeViewer.InputData(1, 3, 7, 200, 4, 1, 3, 0);
            quakeViewer.FormStructure();
            quakeViewer.ResponseMinor();
            quakeViewer.ResponseMajor();
            Console.WriteLine("Minor " + quakeViewer.DamageDgreeMinor);
            Console.WriteLine("Minor " + quakeViewer.DamageDgreeMajor);

            Console.ReadLine();

        }
    }
}
