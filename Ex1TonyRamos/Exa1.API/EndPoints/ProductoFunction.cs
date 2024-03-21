using Exa1.API.Contratos.Repositorio;
using Exa1.API.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Exa1.API.EndPoints
{
    public class ProductoFunction
    {
        private readonly ILogger<ProductoFunction> _logger;
        private readonly IProductoRepositorio repos;

        public ProductoFunction(ILogger<ProductoFunction> logger, IProductoRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarProducto")]
        [OpenApiOperation("Insertarspec", "InsertarProducto", Description = "Sirve para Insertar una Producto")]
        [OpenApiRequestBody("application/json", typeof(Producto), Description = "Producto modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Producto), Description = "Mostrara el Producto Creada")]
        public async Task<HttpResponseData> InsertarProducto([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe ingresar un Producto con todos sus datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.Now;
                bool sw = await repos.Create(registro);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;

                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ListarProducto")]
        [OpenApiOperation("Listarspec", "ListarProducto", Description = "Sirve para listar todas las Producto")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Producto>),
           Description = "Mostrara una lista de Productos")]
        public async Task<HttpResponseData> ListarProducto([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var lista = repos.GetAll();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(lista.Result);
                return respuesta;

            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ObtenerProductoById")]
        [OpenApiOperation("Obtenerspec", "ObtenerProductoById", Description = "Sirve para obtener una Producto")]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Producto), Description = "Mostrara una Producto")]
        public async Task<HttpResponseData> ObtenerProductoById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerProductoById/{rowkey}")] HttpRequestData req, string rowkey)
        {
            HttpResponseData respuesta;
            try
            {
                var producto = repos.Get(rowkey);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(producto.Result);
                return respuesta;
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ModificarProducto")]
        [OpenApiOperation("Modificarspec", "ModificarProducto", Description = "Sirve para Modificar un Producto")]
        [OpenApiRequestBody("application/json", typeof(Producto), Description = "Producto modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Producto),
           Description = "Mostrara el Producto modificado")]
        public async Task<HttpResponseData> ModificarProducto([HttpTrigger(AuthorizationLevel.Function, "put", Route = "ModificarProducto")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Producto>() ?? throw new Exception("Debe ingresar un Producto con todos sus datos");
                bool sw = await repos.Update(registro);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;

                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("EliminarProducto")]
        [OpenApiOperation("Eliminarspec", "EliminarProducto", Description = "Sirve para Eliminar una Producto")]
        [OpenApiParameter(name: "partitionkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        public async Task<HttpResponseData> EliminarProducto([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "EliminarProducto/{partitionkey}/{rowkey}")] HttpRequestData req, string partitionkey, string rowkey)
        {
            HttpResponseData respuesta;
            try
            {
                bool sw = await repos.Delete(partitionkey, rowkey);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;

                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }
    }
}

