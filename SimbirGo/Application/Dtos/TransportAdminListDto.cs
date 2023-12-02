using Domain.Enumerations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class TransportAdminListDto
    {
        [Range(0, int.MaxValue), DefaultValue(0)]
        public int Start { get; set; }
        [Range(1, int.MaxValue), DefaultValue(10)]
        public int Count { get; set; }
        [DefaultValue(TransportTypeListEnum.All)]
        public TransportTypeListEnum TransportType { get; set; }
    }
}
