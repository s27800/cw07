using cwiczenia07.Models.DTOs;
using cwiczenia07.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace cwiczenia07.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseController(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    [HttpPost]
    public IActionResult AddProductToWarehouse(AddProduct addProduct)
    {
        //1
        if (addProduct.Amount <= 0)
            return Problem("Amount must be higher than 0.");

        if (!_warehouseRepository.DoesProductExist(addProduct.IdProduct))
            return NotFound("Product does not exist.");
        
        if (!_warehouseRepository.DoesWarehouseExist(addProduct.IdWarehouse))
            return NotFound("Warehouse does not exist.");

        //2, 3
        int idOrder = _warehouseRepository.DoesOrderExist(addProduct.IdProduct, addProduct.Amount, addProduct.CreatedAt);
        if (idOrder == 0)
            return NotFound("Order with those parameters do not exist.");
        if (_warehouseRepository.IsOrderCompleted(idOrder))
            return Problem("Order with those parameters is already completed.");
        
        //4
        _warehouseRepository.UpdateDate(idOrder);
        
        //5
        return Ok(_warehouseRepository.CreateProductWarehouse(addProduct, idOrder));
    }
}