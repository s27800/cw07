using cwiczenia07.Models;
using cwiczenia07.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace cwiczenia07.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public bool DoesProductExist(int id)
    {
        //Otworzenie polaczenia
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        //Definicja query
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Product WHERE IdProduct = @id";
        command.Parameters.AddWithValue("id", id);

        //Wykonanie commandow
        var reader = command.ExecuteReader();

        if (reader.Read())
            return true;
        
        return false;
    }

    public bool DoesWarehouseExist(int id)
    {
        //Otworzenie polaczenia
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        //Definicja query
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Warehouse WHERE IdWarehouse = @id";
        command.Parameters.AddWithValue("id", id);

        //Wykonanie commandow
        var reader = command.ExecuteReader();

        if (reader.Read())
            return true;
        
        return false;
    }

    public int DoesOrderExist(int id, int amount, string date)
    {
        //Otworzenie polaczenia
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        //Definicja query
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Order WHERE IdProduct = @id AND Amount = @amount AND CreatedAt = @date";
        command.Parameters.AddWithValue("id", id);
        command.Parameters.AddWithValue("amount", amount);
        command.Parameters.AddWithValue("date", date);

        //Wykonanie commandow
        var reader = command.ExecuteReader();
        var order = new Order();

        int idOrderOriginal = reader.GetOrdinal("IdOrder");
        int idProductOriginal = reader.GetOrdinal("IdProduct");
        int amountOriginal = reader.GetOrdinal("Amount");
        int createdAtOriginal = reader.GetOrdinal("CreatedAt");
        int fullfilledAtOriginal = reader.GetOrdinal("FullfilledAt");
        
        if (reader.Read())
        {
            order = new Order()
                    {
                        IdOrder = reader.GetInt32(idOrderOriginal),
                        IdProduct = reader.GetInt32(idProductOriginal),
                        Amount = reader.GetInt32(amountOriginal),
                        CreatedAt = reader.GetString(createdAtOriginal),
                        FullfilledAt = reader.GetString(fullfilledAtOriginal)
                    };
            
            return order.IdOrder;
        }

        return 0;
    }

    public bool IsOrderCompleted(int id)
    {
        //Otworzenie polaczenia
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        //Definicja query
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Product_Warehouse WHERE IdOrder = @id";
        command.Parameters.AddWithValue("id", id);

        //Wykonanie commandow
        var reader = command.ExecuteReader();

        if (reader.Read())
            return true;

        return false;
    }

    public void UpdateDate(int id)
    {
        //Otworzenie polaczenia
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        //Definicja query
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "UPDATE Order SET FullfilledAt = @date WHERE IdOrder = @id";
        command.Parameters.AddWithValue("date", DateTime.Today);
        command.Parameters.AddWithValue("id", id);

        command.ExecuteNonQuery();
    }

    public int CreateProductWarehouse(AddProduct addProduct, int idOrder)
    {
        //Otworzenie polaczenia
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        //Definicja query
        using SqlCommand command1 = new SqlCommand();
        command1.Connection = connection;
        command1.CommandText = "INSERT INTO Product_Warehouse VALUES (@idWar, @idProd, @idOrd, @amount, @price, @date)";
        command1.Parameters.AddWithValue("idWar", addProduct.IdWarehouse);
        command1.Parameters.AddWithValue("idProd", addProduct.IdProduct);
        command1.Parameters.AddWithValue("idOrd", idOrder);
        command1.Parameters.AddWithValue("amount", addProduct.Amount);
        command1.Parameters.AddWithValue("price", GetPrice(addProduct.IdProduct)*addProduct.Amount);
        command1.Parameters.AddWithValue("date", addProduct.CreatedAt);
        
        using SqlCommand command2 = new SqlCommand();
        command2.Connection = connection;
        command2.CommandText = "SELECT IdProductWarehouse FROM Product_Warehouse WHERE IdWarehouse = @idWar AND IdProduct = @idProd AND IdOrder = @idOrd";
        command2.Parameters.AddWithValue("idWar", addProduct.IdWarehouse);
        command2.Parameters.AddWithValue("idProd", addProduct.IdProduct);
        command2.Parameters.AddWithValue("idOrd", idOrder);
        
        //Wykonanie commandow
        command1.ExecuteNonQuery();

        var reader = command2.ExecuteReader();
        var idOriginal = reader.GetOrdinal("IdProductWarehouse");

        if (reader.Read())
            return reader.GetInt32(idOriginal);
        
        return 0;
    }

    public int GetPrice(int id)
    {
        //Otworzenie polaczenia
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        //Definicja query
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT IdProduct FROM Product WHERE IdProduct = @id";
        command.Parameters.AddWithValue("id", id);

        //Wykonanie commandow
        var reader = command.ExecuteReader();
        int idProductOriginal = reader.GetOrdinal("IdProduct");

        if (reader.Read())
        {
            return reader.GetInt32(idProductOriginal);
        }

        return 0;
    }
}