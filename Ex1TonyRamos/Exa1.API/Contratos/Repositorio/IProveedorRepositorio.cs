using Exa1.API.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exa1.API.Contratos.Repositorio
{
    public interface IProveedorRepositorio
    {
        public Task<bool> Create(Proveedor proveedor);
        public Task<bool> Update(Proveedor proveedor);
        public Task<bool> Delete(string partition, string rowkey);
        public Task<List<Proveedor>> GetAll();
        public Task<Proveedor> Get(string rowkey);
    }
}
