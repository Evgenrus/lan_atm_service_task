using Catalog.Database;
using Catalog.Database.Entity;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Service;

public class CatalogService : ICatalogService
{
    private CatalogDbContext _context { get; set; }

    public CatalogService(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<ProductModel>> AddProductsToCart(ICollection<ProductModel> productModels)
    {
        var resProducts = new List<ProductModel>();

        foreach (var product in productModels)
        {
            var res = await _context.Products.FirstOrDefaultAsync(a => a.ProductId == product.Id);
            if (res == null)
                throw new ArgumentException($"No such product ID: {product.Id}");
            
            var stock = await _context.Stocks.FirstOrDefaultAsync(a => a.ProductId == product.Id);
            if (stock.Stock < product.Count)
                throw new ArgumentException($"Product {product.Id} stock is {stock.Stock}");

            resProducts.Add(product);
        }

        return resProducts;
    }

    public async Task<ICollection<ProductModel>> OrderProducts(ICollection<ProductModel> productModels)
    {
        var resProducts = new List<ProductModel>();

        foreach (var product in productModels)
        {
            var res = await _context.Products.FirstOrDefaultAsync(a => a.ProductId == product.Id);
            if (res == null)
                throw new ArgumentException($"No such product ID: {product.Id}");
            
            var stock = await _context.Stocks.FirstOrDefaultAsync(a => a.ProductId == product.Id);
            if (stock.Stock < product.Count)
                throw new ArgumentException($"Product {product.Id} stock is {stock.Stock}");

            stock.Stock -= product.Count;

            resProducts.Add(product);
        }

        return resProducts;
    }

    public async Task<ProductModel> ItemById(int id)
    {
        var prod = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
        if (prod == null)
        {
            throw new ArgumentException("Wrong Id");
        }

        var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductStockId == prod.ProductStockId);
        var brand = await _context.Brands.FirstOrDefaultAsync(x => x.BrandId == prod.BrandId);
        
        return new ProductModel
        {
            Brand = brand.BrandId, 
            Count = stock.Stock, 
            Descr = prod.Description,
            Name = prod.Name,
            Id = prod.ProductId,
            Category = prod.CategoryId
        };
    }

    public async Task<ProductModel> ItemByName(string name)
    {
        var prod = await _context.Products.FirstOrDefaultAsync(x => x.Name == name);
        if (prod == null)
        {
            throw new ArgumentException("Wrong Product Name");
        }

        var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductStockId == prod.ProductStockId);
        var brand = await _context.Brands.FirstOrDefaultAsync(x => x.BrandId == prod.BrandId);
        
        return new ProductModel
        {
            Brand = brand.BrandId, 
            Count = stock.Stock, 
            Descr = prod.Description,
            Name = prod.Name,
            Id = prod.ProductId,
            Category = prod.CategoryId
        };
    }

    public async Task<ICollection<ProductModel>> ItemsByBrandName(string name)
    {
        var res = new List<ProductModel>();
        var brand = await _context.Brands.FirstOrDefaultAsync(x => x.Name == name);
        if (brand == null)
        {
            throw new ArgumentException("Wrong Brand Name");
        }

        if (brand.Products.Count == 0)
            throw new ArgumentException("No matching Items");

        foreach (var product in brand.Products)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == product.ProductId);
            var model = new ProductModel
            {
                Brand = brand.BrandId,
                Count = stock.Stock,
                Descr = product.Description,
                Id = product.ProductId,
                Name = product.Name,
                Category = product.CategoryId
            };
            res.Add(model);
        }

        return res;
    }

    public async Task NewItem(ProductModel model)
    {
        var brand = await _context.Brands.FirstOrDefaultAsync(x => x.BrandId == model.Brand);
        if (brand == null)
            throw new ArgumentException("No such Brand");
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == model.Category);
        if (category == null)
            throw new ArgumentException("No such category");
        var prod = new Product
        {
            Brand = brand,
            BrandId = brand.BrandId,
            Category = category,
            CategoryId = category.CategoryId,
            Description = model.Descr,
            Name = model.Name,
        };
        var stock = new ProductStock
        {
            Stock = model.Count,
            Products = prod
        };
        await _context.Products.AddAsync(prod);
        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();
    }

    public async Task NewBrand(BrandModel model)
    {
        var brand = await _context.Brands.FirstOrDefaultAsync(x => x.Name == model.Name);
        if (brand != null)
            throw new ArgumentException("Brand already exists");
    }
}