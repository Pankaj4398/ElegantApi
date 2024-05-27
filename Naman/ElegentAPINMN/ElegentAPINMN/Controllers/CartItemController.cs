using ElegentAPINMN.Models.Domain;
using ElegentAPINMN.Repositories.Class;
using ElegentAPINMN.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ElegentAPINMN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepo;

        public CartItemController(ICartItemRepository cartItemRepo)
        {
            this._cartItemRepo = cartItemRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCartItems()
        {
            var res = await _cartItemRepo.GetAllAsync();
            return Ok(res);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetCartItemById([FromRoute] Guid Id)
        {
            var res = await _cartItemRepo.GetByIdAsync(Id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddCartItem([FromBody] CartItem cartItem)
        {
            var res = await _cartItemRepo.CreateAsync(cartItem);
            return Ok(res);
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> UpdateCartItem([FromRoute] Guid Id, [FromBody] CartItem cartItem)
        {
            var res = await _cartItemRepo.UpdateAsync(Id, cartItem);
            return Ok(res);
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> DeleteCartItem([FromRoute] Guid Id)
        {
            var res = await _cartItemRepo.DeleteAsync(Id);
            return Ok(res);
        }
    }
}
