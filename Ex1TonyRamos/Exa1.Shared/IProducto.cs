﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exa1.Shared
{
    public interface IProducto
    {

        public string Nombre { get; set; }

        public double Precio { get; set; }

        public string ProveedorId { get; set; }
    }
}
