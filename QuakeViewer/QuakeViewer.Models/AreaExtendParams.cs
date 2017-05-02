using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuakeViewer.Models
{
    public class AreaExtendParams
    {
        [Key]
        public string Id { get; set; }
        public string ProvinceId { get; set; }
        public string Province { get; set; }
        public string CityId { get; set; }
        public string City { get; set; }
        public string RegionId { get; set; }
        public string Region { get; set; }
        public string StreetId { get; set; }
        public string Street { get; set; }
        public int? GroupNo { get; set; }
        public int? SiteType { get; set; }
        public int? IntensityDegree { get; set; }

        public override string ToString()
        {
            return !this.Region.Equals("城区") ? $"{this.Province}{this.City}{this.Region}{this.Street}" : $"{this.Province}{this.City}{this.Street}";
        }
    }
}
