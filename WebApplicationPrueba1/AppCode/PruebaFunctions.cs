using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

public class PruebaFunctions
{
    private readonly string _connectionString;

    public PruebaFunctions(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    /// Ejecuta una consulta SELECT a una vista y devuelve un DataTable.
    /// </summary>
    public DataTable ExecuteQuery(string sqlQuery)
    {
        try 
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable resultTable = new DataTable();
                    adapter.Fill(resultTable);
                    return resultTable;
                }
            }
        }
        catch (Exception ex) 
        { 
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.InnerException?.Message);
            throw;
        }

    }

    /// <summary>
    /// Añade una orden.
    /// </summary>
    public int AddNewOrder(string procedureName, AddNewOrder order)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Configurar los parámetros del procedimiento almacenado
                    command.Parameters.Add(new SqlParameter("@EmpID", order.EmpID));
                    command.Parameters.Add(new SqlParameter("@CustID", order.CustID));
                    command.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperID));
                    command.Parameters.Add(new SqlParameter("@ShipName", order.ShipName));
                    command.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                    command.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                    command.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate));
                    command.Parameters.Add(new SqlParameter("@RequiredDate", order.RequiredDate));
                    command.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate));
                    command.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                    command.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));
                    command.Parameters.Add(new SqlParameter("@ProductID", order.ProductID));
                    command.Parameters.Add(new SqlParameter("@UnitPrice", order.UnitPrice));
                    command.Parameters.Add(new SqlParameter("@Qty", order.Qty));
                    command.Parameters.Add(new SqlParameter("@Discount", order.Discount));

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.InnerException?.Message);
            throw;
        }
    }

    /// <summary>
    /// Convierte un DataTable en una serialización JSON.
    /// </summary>
    public List<Dictionary<string, object>> ConvertDataTableToJson(DataTable table)
    {
        var result = new List<Dictionary<string, object>>();
        try
        {
            foreach (DataRow row in table.Rows)
            {
                var rowData = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    rowData[col.ColumnName] = row[col];
                }
                result.Add(rowData);
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.InnerException?.Message);
            throw;
        }
    }
}
