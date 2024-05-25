using AutoMapper;
using ElegentAPINMN.Data;
using ElegentAPINMN.Models.Domain;
using ElegentAPINMN.Models.DTO;
using ElegentAPINMN.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElegentAPINMN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(ApplicationDbContext dbContext, IProductRepository productRepository, IMapper mapper)
        {
            _context = dbContext;
            this._productRepository = productRepository;
            this._mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //get data from DB
            var products = await _productRepository.GetAllAsync();
            
            //map data to DTO
            var productsDto = _mapper.Map<List<ProductDto>>(products);
            
            //return DTO
            return Ok(productsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Get Data
            var product = await _productRepository.GetByIdAsync(id);
            
            //map to DTO
            var productDTO = _mapper.Map<ProductDto>(product);
            
            //return DTO
            return Ok(productDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            var res = await _productRepository.CreateAsync(product);
            return Ok(res);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] Product product)
        {
            var res = await _productRepository.UpdateAsync(id, product);
            return Ok(res);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var res = await _productRepository.DeleteAsync(id);
            return Ok(res);
        }
    }
}
