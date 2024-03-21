using Exa1.API.Contratos.Repositorio;
using Exa1.API.Implementacion.Repositorio;
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
    public class ProveedorFunction
    {
        private readonly ILogger<ProveedorFunction> _logger;
        private readonly IProveedorRepositorio repos;


        public ProveedorFunction(ILogger<ProveedorFunction> logger, IProveedorRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }



        [Function("InsertarProveedor")]
        [OpenApiOperation("Insertarspec", "InsertarProveedor", Description = "Sirve para Insertar un Proveedor")]
        [OpenApiRequestBody("application/json", typeof(Proveedor), Description = "Proveedor modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Proveedor), Description = "Mostrara el Proveedor creado")]
        public async Task<HttpResponseData> InsertarProveedor([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresae un proveedor con todos sus datos");
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



        [Function("ListarProveedor")]
        [OpenApiOperation("Listarspec", "ListarProveedor", Description = "Sirve para listar todas los Proveedores")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Proveedor>),
            Description = "Mostrara una lista de proveedores")]
        public async Task<HttpResponseData> ListarProveedor([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
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



        [Function("ObtenerProveedorById")]
        [OpenApiOperation("Obtenerspec", "ObtenerProveedorById", Description = "Sirve para obtener un Proveedor")]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Proveedor), Description = "Mostrara un proveedor")]
        public async Task<HttpResponseData> ObtenerProveedorById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerProveedorById/{rowkey}")] HttpRequestData req, string rowkey)
        {
            HttpResponseData respuesta;
            try
            {
                var proveedor = repos.Get(rowkey);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(proveedor.Result);
                return respuesta;
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ModificarProveedor")]
        [OpenApiOperation("Modificarspec", "ModificarProveedor", Description = "Sirve para Modificar un Proveedor")]
        [OpenApiRequestBody("application/json", typeof(Proveedor), Description = "Institucion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Proveedor),
            Description = "Mostrara la proveedor modificado")]
        public async Task<HttpResponseData> ModificarProveedor([HttpTrigger(AuthorizationLevel.Function, "put", Route = "ModificarProveedor")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresae un proveedor con todos sus datos");
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
        [Function("EliminarProveedor")]
        [OpenApiOperation("Eliminarspec", "EliminarProveedor", Description = "Sirve para Eliminar un proveedor")]
        [OpenApiParameter(name: "partitionkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        public async Task<HttpResponseData> EliminarProveedor([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "EliminarProveedor/{partitionkey}/{rowkey}")] HttpRequestData req, string partitionkey, string rowkey)
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



