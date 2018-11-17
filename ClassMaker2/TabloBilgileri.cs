using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ClassMaker2
{
    public class TabloBilgileri
    {
        [DisplayName("Kolon Adı")]
        public string ColumnName { get; set; }
        [DisplayName("Tipi")]
        public string Type { get; set; }
        [DisplayName("Boş Olabilme Durumu")]
        public bool Nullable { get; set; }

    }
}
