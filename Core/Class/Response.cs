using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public class Response
    {
        public bool Status { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }

        public Response()
        {
            this.Status = false;
            this.Message = "Operación Fallida.";
        }
    }
}