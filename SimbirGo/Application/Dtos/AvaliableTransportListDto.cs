using Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class AvaliableTransportListDto
    {
        [BindProperty(Name = "lat")]
        [Range(-90D, 90D)]
        public double Latitude { get; set; }
        [BindProperty(Name = "long")]
        [Range(-180D, 180D)]
        public double Longitude { get; set; }
        [Range(0D, 500D)]
        public double Radius { get; set; }
        [BindProperty(Name = "type"), DefaultValue(TransportTypeListEnum.All)]
        public TransportTypeListEnum TransportType { get; set; }
    }
}
