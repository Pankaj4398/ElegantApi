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
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
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
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto product)
        {
            //convert productdto to product
            var productData = _mapper.Map<Product>(product);

            //create the product
            var res = await _productRepository.CreateAsync(productData);

            //convert res to the DTO
            var productDTO = _mapper.Map<ProductDto>(res);

            // return product DTO
            return Ok(productDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] ProductDto productDto)
        {
            //convert productDTO to product
            var product = _mapper.Map<Product>(productDto);

            //update the product
            var res = await _productRepository.UpdateAsync(id, product);

            //convert product to productDTO
            var resProductDTO = _mapper.Map<ProductDto>(res);
            return Ok(resProductDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            //get the product
            var res = await _productRepository.DeleteAsync(id);

            //convert product to productDTO
            var resProductDTO = _mapper.Map<ProductDto>(res);

            return Ok(resProductDTO);
        }
    }
}
