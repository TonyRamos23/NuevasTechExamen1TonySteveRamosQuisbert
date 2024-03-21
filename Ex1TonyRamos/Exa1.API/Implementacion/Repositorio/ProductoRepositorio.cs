using Azure.Data.Tables;
using Exa1.API.Contratos.Repositorio;
using Exa1.API.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exa1.API.Implementacion.Repositorio
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;
        public ProductoRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Producto";
        }

        public async Task<bool> Create(Producto producto)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpsertEntityAsync(producto);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> Delete(string partitionkey, string rowkey)
        {
            try
            {
                var tableCliente = new TableClient(cadenaConexion, tablaNombre);
                await tableCliente.DeleteEntityAsync(partitionkey, rowkey);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<Producto> Get(string rowkey)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var pro = await tablaCliente.GetEntityAsync<Producto>("Producto", rowkey);
            return pro.Value;
        }

        public async Task<List<Producto>> GetAll()
        {
            List<Producto> lista = new List<Producto>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Producto'";

            await foreach (Producto producto in tablaCliente.QueryAsync<Producto>(filter: filtro))
            {
                lista.Add(producto);
            }

            return lista;
        }

        public async Task<bool> Update(Producto producto)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpdateEntityAsync(producto, producto.ETag);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}

