using System;
using System.Collections.Generic;
using System.Text;

namespace JARdineria
{
   public class NuevaPlanta
    {
        public string Nombre { get; set; }
        public DateTime FechaPlantado { get; set; } = DateTime.Now.Date;
        public string Imagen { get; set; }
        public string Nota { get; set; }
    }
}
