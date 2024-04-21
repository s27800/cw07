using cwiczenia07.Models.DTOs;

namespace cwiczenia07.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    public bool DoesProductExist(int id)
    {

        return true;
    }

    public bool DoesWarehouseExist(int id)
    {

        return true;
    }

    public int DoesOrderExist(int id, int amount, string date)
    {

        return 1;
    }

    public bool IsOrderCompleted(int id)
    {

        return false;
    }

    public void UpdateDate()
    {
        
    }

    public int CreateProductWarehouse(AddProduct addProduct, int idOrder)
    {

        return 1;
    }
}