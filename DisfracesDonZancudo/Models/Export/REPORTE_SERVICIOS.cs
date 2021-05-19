﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DisfracesDonZancudo.Models.Export
{
    public class REPORTE_SERVICIOS
    {
        public int ID { get; set; }
        public string NOMBRECLIENTE { get; set; }
        public string NOMBREDISFRAZ { get; set; }
        public string TIPOPAGO { get; set; }
        public string TIPODISFRAZ { get; set; }
        public DateTime FECHAARRIENDO { get; set; }
        public int DIASARRIENDO { get; set; }
        public DateTime FECHAFINALIZACION { get; set; }
    }
}