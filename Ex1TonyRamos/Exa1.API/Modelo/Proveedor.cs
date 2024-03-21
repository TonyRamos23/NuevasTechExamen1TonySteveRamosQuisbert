using Azure;
using Azure.Data.Tables;
using Exa1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exa1.API.Modelo
{
    public class Proveedor : IProveedor, ITableEntity
    {
        public string Nombre {  get; set; }
        public string Direccion {  get; set; }
        public string PartitionKey {  get; set; }
        public string RowKey {  get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag {  get; set; }
    }
}
