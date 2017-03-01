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
            quakeViewer.InputData(1, 2, 7, 1, 1, true, 1, 1);
            quakeViewer.FormStructure();
            quakeViewer.ResponseMinor();
            quakeViewer.ResponseMajor();
            Console.WriteLine("Minor " + quakeViewer.DamageDgreeMinor);
            Console.WriteLine("Minor " + quakeViewer.DamageDgreeMajor);

            Console.ReadLine();

        }
    }
}
