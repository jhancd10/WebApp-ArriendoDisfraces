//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class servicio
    {
        public int id { get; set; }
        public string observacion { get; set; }
        public System.DateTime fecha_arriendo { get; set; }
        public int dias_arriendo { get; set; }
        public int cliente_id { get; set; }
        public int disfraz_id { get; set; }
        public int tipo_pago_id { get; set; }
        public int cantidad { get; set; }
        public bool estado { get; set; }
    
        public virtual cliente cliente { get; set; }
        public virtual disfraz disfraz { get; set; }
        public virtual tipo_pago tipo_pago { get; set; }
    }
}