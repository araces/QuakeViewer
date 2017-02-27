//
//  Souce	Path	Program.cs
//  create	Date	2017-2-25 10:42:21		
//  created	By	Ares.Zhao
//
using System;

namespace QuakeViewer.Test
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            QuakeViewerCalculate quakeViewer = new QuakeViewerCalculate();
            /*
            public void InputData(int groupNo, int siteType, double intensityDegree,
                       int storyNum,
                       int struType,
                              bool isDesigned,
                       int contructionQuality,
                              int builtYearGroup
                       )
                       */
            quakeViewer.InputData(1, 3, 7, 1, 4, false, 3, 1);
            quakeViewer.FormStructure();
            quakeViewer.ResponseMinor();
            quakeViewer.ResponseMajor();
            Console.WriteLine("Minor " + quakeViewer.DamageDgreeMinor);
            Console.WriteLine("Minor " + quakeViewer.DamageDgreeMajor);
            Console.ReadLine();
        }
    }
}
