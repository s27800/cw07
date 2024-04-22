using cwiczenia07.Models.DTOs;

namespace cwiczenia07.Repositories;

public interface IWarehouseRepository
{
    bool DoesProductExist(int id);
    bool DoesWarehouseExist(int id);
    int DoesOrderExist(int id, int amount, string date);
    bool IsOrderCompleted(int id);
    void UpdateDate(int id);
    int CreateProductWarehouse(AddProduct addProduct, int idOrder);
}