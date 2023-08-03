using System.Collections;
using App.APIs.DbContexts;
using App.APIs.Models;
using App.APIs.Models.ProductDto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.APIs.Repository;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;
    private IMapper _mapper;
    public ProductRepository( AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        
        List<Product> products = await _dbContext.Products.ToListAsync();
        return _mapper.Map<List<ProductDto>>(products); 
    }

    public async Task<ProductDto> GetProductById(int productId)
    {
        Product products = 
            await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId) ?? throw new InvalidOperationException();
        
        return _mapper.Map<ProductDto>(products);
    }

    public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
    {
        Product product = _mapper.Map<ProductDto, Product>(productDto);
        if (product.Id > 0)
        {
            _dbContext.Products.Update(product);
        }
        else
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<Product, ProductDto>(product); 
    }

    public async Task<bool> DeleteProduct(int productId)
    {
        try
        {
            Product? product = 
                await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product != null)
            {
                _dbContext.Products.Remove(product); 
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;  
        }
        catch (Exception e)
        {
            return false;
        }
    }
}