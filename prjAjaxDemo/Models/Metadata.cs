using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjAjaxDemo.Models
{
    public class MemberMetadata
    {
        [DisplayName("姓名")]
        public string Name { get; set; }
        public string Email { get; set; }
        [DisplayName("年齡")]
        public int? Age { get; set; }
        [DisplayName("檔名")]
        public string FileName { get; set; }
        [DisplayName("檔案")]
        public byte[] FileData { get; set; }
    }
    public class AddressMetadata
    {
        [DisplayName("城市")]
        public string City { get; set; }
        [DisplayName("區")]
        public string SiteId { get; set; }
        [DisplayName("路")]
        public string Road { get; set; }
    }

}
