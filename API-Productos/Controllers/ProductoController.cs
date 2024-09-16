 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using API_Productos.models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace API_Productos.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly string cadenaSql;

        public ProductoController (IConfiguration configuration)
        {
            cadenaSql = configuration.GetConnectionString("CadenaSQl");
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                { 
                    conexion.Open();
                    var cmd = new SqlCommand("SP_ListaProductos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                NombreProducto = rd["NombreProducto"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"]),
                            });
                        }
                    }

                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }catch(Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }

        }


        [HttpPost]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            List<Producto> lista = new List<Producto>();
            Producto producto = new Producto();

            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_ListaProductos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                NombreProducto = rd["NombreProducto"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"]),
                            });
                        }
                    }

                }
                producto = lista.Where(item => item.IdProducto == idProducto).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = producto });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = producto });
            }

        }



        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
          

            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_GuardarProducto", conexion);
                    cmd.Parameters.AddWithValue("CodigoBarra", objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("NombreProducto", objeto.NombreProducto);
                    cmd.Parameters.AddWithValue("Marca", objeto.Marca);
                    cmd.Parameters.AddWithValue("Categoria", objeto.Categoria);
                    cmd.Parameters.AddWithValue("Precio", objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok"});
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }

        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {


            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_EditarProducto", conexion);
                    cmd.Parameters.AddWithValue("IdProducto", objeto.IdProducto == 0 ? DBNull.Value : objeto.IdProducto);
                    cmd.Parameters.AddWithValue("CodigoBarra", objeto.CodigoBarra is null ? DBNull.Value : objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("NombreProducto", objeto.NombreProducto is null ? DBNull.Value : objeto.NombreProducto);
                    cmd.Parameters.AddWithValue("Marca", objeto.Marca is null ? DBNull.Value : objeto.Marca);
                    cmd.Parameters.AddWithValue("Categoria", objeto.Categoria is null ? DBNull.Value : objeto.Categoria);
                    cmd.Parameters.AddWithValue("Precio", objeto.Precio == 0 ? DBNull.Value : objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }

        }

        [HttpDelete]
        [Route("Eliminar/{IdProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {


            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("SP_EliminarProducto", conexion);
                    cmd.Parameters.AddWithValue("IdProducto", idProducto);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Producto eliminado " });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }

        }
    }
}
