//
//  Souce	Path	AreaParamService.cs
//  create	Date	2017-2-12 15:7:2		
//  created	By	Ares.Zhao
//
using System;
using System.Collections.Generic;
using System.Linq;
using QuakeViewer.Utils;
using QuakeViewer.DAL;
using QuakeViewer.Models;

namespace QuakeViewer.Service
{
    public class AreaExtendParamsService
    {
        AreaExtendParamsContext areaExtendParamsContext { get; set; }

        public AreaExtendParamsService() : this(new AreaExtendParamsContext())
        {
        }

        public AreaExtendParamsService(AreaExtendParamsContext _areaExtendParamsContext)
        {
            areaExtendParamsContext = _areaExtendParamsContext;
        }

        public List<AreaExtendParams> GetAreaParams()
        {

            var areaParams = areaExtendParamsContext.AreaExtendParamses.ToList();
            return areaParams;
        }

        public List<KeyValuePair<string,string>> GetProvince()
        {
            List<KeyValuePair<string, string>> provinceList =new List<KeyValuePair<string, string>>();

            var provinces = from r in areaExtendParamsContext.AreaExtendParamses
                group r by new {nameKey = r.Province, idKey = r.ProvinceId}
                into g
                select new {Key = g.Key.idKey, Value = g.Key.nameKey};


            foreach (var q in provinces)
            {
                KeyValuePair<string,string> p =new KeyValuePair<string, string>(q.Key,q.Value);
                provinceList.Add(p);
            }

            return provinceList;
        }

        public List<KeyValuePair<string, string>> GetCityByProvinceId(string provinceId)
        {
            List<KeyValuePair<string, string>> cityList = new List<KeyValuePair<string, string>>();

            var cities = from r in areaExtendParamsContext.AreaExtendParamses
                            where r.ProvinceId == provinceId
                            group r by new { nameKey = r.City, idKey = r.CityId }
                into g
               
                            select new { Key = g.Key.idKey, Value = g.Key.nameKey };


            foreach (var q in cities)
            {
                KeyValuePair<string, string> p = new KeyValuePair<string, string>(q.Key, q.Value);
                cityList.Add(p);
            }

            return cityList;
        }

        public List<KeyValuePair<string, string>> GetRegionByCityId(string cityId)
        {
            List<KeyValuePair<string, string>> regionList = new List<KeyValuePair<string, string>>();

            var regions = from r in areaExtendParamsContext.AreaExtendParamses
                            where r.CityId == cityId
                            group r by new { nameKey = r.Region, idKey = r.RegionId }
                into g

                            select new { Key = g.Key.idKey, Value = g.Key.nameKey };


            foreach (var q in regions)
            {
                KeyValuePair<string, string> p = new KeyValuePair<string, string>(q.Key, q.Value);
                regionList.Add(p);
            }

            return regionList;
        }


        public List<KeyValuePair<string, string>> GetStreetByRegionId(string regionId)
        {
            List<KeyValuePair<string, string>> streetList = new List<KeyValuePair<string, string>>();

            var regions = from r in areaExtendParamsContext.AreaExtendParamses
                          where r.RegionId == regionId
                          group r by new { nameKey = r.Street, idKey = r.StreetId }
                into g

                          select new { Key = g.Key.idKey, Value = g.Key.nameKey };


            foreach (var q in regions)
            {
                KeyValuePair<string, string> p = new KeyValuePair<string, string>(q.Key, q.Value);
                streetList.Add(p);
            }

            return streetList;
        }

        public Dictionary<string, string> GetAreaDict()
        {
            Dictionary<string,string> areaDict =new Dictionary<string, string>();
            List<AreaExtendParams> areaExtendParamses = areaExtendParamsContext.AreaExtendParamses.ToList();
            foreach (var q in areaExtendParamses)
            {
                    areaDict.Add(q.StreetId, q.ToString());
            }

            return areaDict;
          
        }



        public AreaExtendParams GetStreetByStreatId(string streetId)
        {
            return areaExtendParamsContext.AreaExtendParamses.FirstOrDefault(p => p.StreetId == streetId);
        }
    }
}
