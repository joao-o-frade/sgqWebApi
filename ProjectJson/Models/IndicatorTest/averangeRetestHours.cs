﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectWebApi.Models
{
    public class AverangeRetestHours
    {
        public string month { get; set; }
        public string year { get; set; }
        public string testManuf { get; set; }
        public string system { get; set; }
        public string subprojectDelivery { get; set; }

        public int qtyDefect { get; set; }
        public double qtyRetestHours { get; set; }
    }
}