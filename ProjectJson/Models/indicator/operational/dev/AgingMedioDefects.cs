﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectWebApi.Models
{
    public class AgingMedioDefects
    {
        public string severity { get; set; }
        public string date { get; set; }
        public string devManuf { get; set; }
        public string system { get; set; }
        public string subDel { get; set; }
        public string subproject { get; set; }
        public string delivery { get; set; }
        public int qtyDefects { get; set; }
        public double qtyHours { get; set; }
        public double Media { get; set; }

    }
}