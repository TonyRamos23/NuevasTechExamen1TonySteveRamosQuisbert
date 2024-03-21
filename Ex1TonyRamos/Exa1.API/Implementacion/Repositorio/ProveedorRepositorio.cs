using Azure.Data.Tables;
using Exa1.API.Contratos.Repositorio;
using Exa1.API.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exa1.API.Implementacion.Repositorio
{
    public class ProveedorRepositorio : IProveedorRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;
        public ProveedorRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Proveedor";
        }

        public async Task<bool> Create(Proveedor proveedor)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpsertEntityAsync(proveedor);
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

        public async Task<Proveedor> Get(string rowkey)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var proveedores = await tablaCliente.GetEntityAsync<Proveedor>("Proveedor", rowkey);
            return proveedores.Value;
        }

        public async Task<List<Proveedor>> GetAll()
        {
            List<Proveedor> lista = new List<Proveedor>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Proveedor'";

            await foreach (Proveedor proveedor in tablaCliente.QueryAsync<Proveedor>(filter: filtro))
            {
                lista.Add(proveedor);
            }

            return lista;
        }

        public async Task<bool> Update(Proveedor proveedor)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpdateEntityAsync(proveedor, proveedor.ETag);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}


