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
    public class AreaParamService
    {
        AreaParamContext areaParamContext { get; set; }

        public AreaParamService() : this(new AreaParamContext())
        {
        }

        public AreaParamService(AreaParamContext _areaParamContext)
        {
            areaParamContext = _areaParamContext;
        }

        public List<AreaParam> GetAreaParams()
        {

            var areaParams = areaParamContext.AreaParams.ToList();
            return areaParams;
        }

        public List<AreaParam> GetProvince()
        {

            var provinces = areaParamContext.AreaParams.Where(p => p.ParentId.Equals("0")).ToList();
            return provinces;
        }

        public List<AreaParam> GetAreaParamsByParentId(string parentId)
        {
            var areaParams = areaParamContext.AreaParams.Where(p => p.ParentId == parentId).ToList();
            return areaParams;
        }

        public AreaParam GetAreaParamById(string id)
        {
            var areaParam = areaParamContext.AreaParams.FirstOrDefault(p => p.Id.Equals(id));
            return areaParam;
        }
        public void UpdateParams(AreaParam areaParam)
        {
            //areaParamContext.AreaParams.Attach(areaParam);
            areaParamContext.SaveChanges();
        }

    }
}
