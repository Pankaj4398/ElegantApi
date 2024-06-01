using AutoMapper;
using ElegentAPINMN.Models.Domain;
using ElegentAPINMN.Models.DTO;
using ElegentAPINMN.Repositories.Class;
using ElegentAPINMN.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using System.Reflection.Metadata.Ecma335;

namespace ElegentAPINMN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepo;
        private readonly IMapper _mapper;

        public CartItemController(ICartItemRepository cartItemRepo, IMapper mapper)
        {
            this._cartItemRepo = cartItemRepo;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCartItems()
        {
            //get the data from cartItem
            var res = await _cartItemRepo.GetAllAsync();

            //convert data to cartItemDTO
            var resDTO = _mapper.Map<List<CartItemDto>>(res);

            //return cartDTO
            return Ok(resDTO);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetCartItemById([FromRoute] Guid Id)
        {
            //get the cart item data
            var res = await _cartItemRepo.GetByIdAsync(Id);

            //convert cart item data to cartItemDTO
            var resDTO = _mapper.Map<CartItemDto>(res);

            //return cartItemDTO
            return Ok(resDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddCartItem([FromBody] CartItemDto cartItem)
        {
            //convert cartDto to cart
            var res = _mapper.Map<CartItem>(cartItem);

            //add cart data to table
            var resData = await _cartItemRepo.CreateAsync(res);

            //convert cart data to cartDTO
            var resDTO = _mapper.Map<CartItem>(resData);

            //return resDTO
            return Ok(resDTO);
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> UpdateCartItem([FromRoute] Guid Id, [FromBody] CartItemDto cartItemDto)
        {
            //convert cartItemDto to cartItem
            var cartItem = _mapper.Map<CartItem>(cartItemDto);

            //update the table
            var res = await _cartItemRepo.UpdateAsync(Id, cartItem);

            //convert cartItem to cartItemDTO
            var resDto = _mapper.Map<CartItemDto>(res);
            
            return Ok(resDto);
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> DeleteCartItem([FromRoute] Guid Id)
        {
            //delete the item from the table
            var res = await _cartItemRepo.DeleteAsync(Id);

            //convert the deleted item to cartItemDTO
            var resDTO = _mapper.Map<CartItemDto>(res);

            //return resDTO
            return Ok(resDTO);
        }
    }
}
