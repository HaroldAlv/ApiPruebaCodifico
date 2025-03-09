using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

[Route("api/StoreSample")]
[ApiController]
public class DatabaseController : ControllerBase
{
    private readonly PruebaFunctions _dbFunctions;

    // Inyección de dependencia de PruebaFunctions
    public DatabaseController(PruebaFunctions dbFunctions)
    {
        _dbFunctions = dbFunctions;
    }

    /// <summary>
    /// Listar clientes con fecha de ultima orden y fecha de posible orden.
    /// </summary>
    [HttpGet("vw_NextPredictedOrder")]
    public IActionResult vw_NextPredictedOrder()
    {
        string query = "SELECT * FROM vw_NextPredictedOrder"; 
        DataTable data = _dbFunctions.ExecuteQuery(query);
        var jsonData = _dbFunctions.ConvertDataTableToJson(data);
        return Ok(jsonData);
    }

    /// <summary>
    /// Listar ordenes por cliente.
    /// </summary>
    [HttpGet("GetClientOrders")]
    public IActionResult GetClientOrders()
    {
        string query = "SELECT * FROM GetClientOrders"; 
        DataTable data = _dbFunctions.ExecuteQuery(query);
        var jsonData = _dbFunctions.ConvertDataTableToJson(data);
        return Ok(jsonData);
    }

    /// <summary>
    /// Listar ordenes por cliente.
    /// </summary>
    [HttpGet("GetClientOrders/{companyname}")]
    public IActionResult GetClientOrdersById(string companyname)
    {
        string query = "SELECT * FROM GetClientOrders WHERE custid IN (SELECT custid FROM Sales.Customers WHERE companyname = '" + companyname + "')";
        DataTable data = _dbFunctions.ExecuteQuery(query);
        var jsonData = _dbFunctions.ConvertDataTableToJson(data);
        return Ok(jsonData);
    }

    /// <summary>
    /// Listar todos los empleados.
    /// </summary>
    [HttpGet("GetEmployees")]
    public IActionResult GetEmployees()
    {
        string query = "SELECT * FROM GetEmployees";
        DataTable data = _dbFunctions.ExecuteQuery(query);
        var jsonData = _dbFunctions.ConvertDataTableToJson(data);
        return Ok(jsonData);
    }

    /// <summary>
    /// Listar todos los transportistas.
    /// </summary>
    [HttpGet("GetShippers")]
    public IActionResult GetShippers()
    {
        string query = "SELECT * FROM GetShippers"; 
        DataTable data = _dbFunctions.ExecuteQuery(query);
        var jsonData = _dbFunctions.ConvertDataTableToJson(data);
        return Ok(jsonData);
    }

    /// <summary>
    /// Listar todos los productos.
    /// </summary>
    [HttpGet("GetProducts")]
    public IActionResult GetProducts()
    {
        string query = "SELECT * FROM GetProducts"; 
        DataTable data = _dbFunctions.ExecuteQuery(query);
        var jsonData = _dbFunctions.ConvertDataTableToJson(data);
        return Ok(jsonData);
    }

    /// <summary>
    /// Crear una orden nueva con un producto.
    /// </summary>
    [HttpPost("AddNewOrder")]
    public IActionResult AddNewOrder([FromBody] AddNewOrder order)
    {
        if (order == null)
        {
            return BadRequest("El objeto 'order' no puede ser nulo.");
        }

        int rowsAffected = _dbFunctions.AddNewOrder("AddNewOrder", order);
        return Ok(new { message = "Procedimiento ejecutado", affectedRows = rowsAffected });
    }
}
