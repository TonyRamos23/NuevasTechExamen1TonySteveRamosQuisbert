using Exa1.API.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exa1.API.Contratos.Repositorio
{
    public interface IProductoRepositorio
    {
        public Task<bool> Create(Producto producto);
        public Task<bool> Update(Producto producto);
        public Task<bool> Delete(string partition, string rowkey);
        public Task<List<Producto>> GetAll();
        public Task<Producto> Get(string rowkey);
    }
}
