using AutoMapper;
using ElegentAPINMN.Models.Domain;
using ElegentAPINMN.Models.DTO;
using ElegentAPINMN.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElegentAPINMN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public DiscountController(IDiscountRepository discountRepository, IMapper mapper)
        {
            this._discountRepository = discountRepository;
            this._mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDiscount()
        {
            //get data from the discount
            var discountList = await _discountRepository.GetAllAsync();

            //convert to DiscountDto
            var discountListDto = _mapper.Map<List<DiscountDto>>(discountList);

            //return DiscountDto list
            return Ok(discountListDto);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetDiscountByID([FromRoute] Guid Id)
        {
            //get the discount data
            var res = await _discountRepository.GetByIdAsync(Id);

            //convert discount data to DiscountDTO
            var resDTO = _mapper.Map<DiscountDto>(res);

            //return discountDTO
            return Ok(resDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddDiscount(DiscountDto disc)
        {
            //convert discountDTO to discount 
            var discount = _mapper.Map<Discount>(disc);

            //add the discount to the database
            var res = await _discountRepository.CreateAsync(discount);

            //convert the result to the discountDTO
            var resDTO = _mapper.Map<DiscountDto>(res);
            return Ok(resDTO);
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> DeleteDiscount([FromRoute] Guid Id)
        {
            var res = await _discountRepository.DeleteAsync(Id);

            //convert discount to discountDTO
            var resDTO = _mapper.Map<DiscountDto>(res);

            //return discountDTO
            return Ok(resDTO);
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> UpdateDiscount([FromRoute] Guid Id,  DiscountDto disc)
        {
            //convert discountDTO to discount
            var discount = _mapper.Map<Discount>(disc);

            //update the discount
            var updatedItem = await _discountRepository.UpdateAsync(Id, discount);

            //convert updated discount to the discountDTO
            var updatedDiscountDTO = _mapper.Map<DiscountDto>(updatedItem);

            //return discountDTO
            return Ok(updatedItem);
        }
    }
}
