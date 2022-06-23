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

    public async Task<ICollection<Item>> AddProductsToCart(ICollection<Item> productModels)
    {
        var resProducts = new List<Item>();

        foreach (var product in productModels)
        {
            var res = await _context.Products.FirstOrDefaultAsync(a => a.Article == product.Article);
            if (res == null)
                throw new ArgumentException($"No such product Article: {product.Article}");
            
            var stock = await _context.Stocks.FirstOrDefaultAsync(a => a.ProductId == res.ProductId);
            if (stock.Stock < product.Count)
                throw new ArgumentException($"Product {product.Article} stock is {stock.Stock}");

            resProducts.Add(product);
        }

        return resProducts;
    }

    public async Task<ICollection<Item>> OrderProducts(ICollection<Item> productModels)
    {
        var resProducts = new List<Item>();

        foreach (var product in productModels)
        {
            var res = await _context.Products.FirstOrDefaultAsync(a => a.Article == product.Article);
            if (res == null)
                throw new ArgumentException($"No such product: Article {product.Article}");
            
            var stock = await _context.Stocks.FirstOrDefaultAsync(a => a.ProductId == res.ProductId);
            if (stock.Stock < product.Count)
                throw new ArgumentException($"Product {product.Name} stock is {stock.Stock}");

            stock.Stock -= product.Count;

            resProducts.Add(product);
        }

        return resProducts;
    }

    public async Task<Item> CheckItem(Item item)
    {
        var itemcatalog = _context.Products
            .Include(x => x.Brand)
            .Include(x => x.Category)
            .Include(x => x.Stock);

        var check = await itemcatalog.FirstOrDefaultAsync(x => x.Name == item.Name
                                                         && x.Article == item.Article);
        if (check is null)
        {
            throw new Exception("No such item");
        }

        if (check.Stock.Stock < item.Count)
        {
            throw new ArgumentException($"requested {item.Count} items, but got inly {check.Stock.Stock}");
        }

        return new Item
        {
            Article = item.Article,
            Brand = check.Brand.Name,
            Category = check.Category.Name,
            Count = item.Count,
            ItemId = check.ProductId,
            Name = item.Name
        };
    }

    public async Task<ICollection<Item>> CheckItems(ICollection<Item> items) {
        if (items.Any())
        {
            throw new ArgumentNullException("Items list is empty");
        }
        var res = new List<Item>();
        foreach(var item in items)
        {
            var itemcatalog = _context.Products
            .Include(x => x.Brand)
            .Include(x => x.Category)
            .Include(x => x.Stock);

            var check = await itemcatalog.FirstOrDefaultAsync(x => x.Name == item.Name
                                                             && x.Article == item.Article);
            if (check is null)
            {
                throw new Exception("No such item");
            }

            if (check.Stock.Stock < item.Count)
            {
                throw new ArgumentException($"requested {item.Count} items, but got inly {check.Stock.Stock}");
            }

            var toAdd = new Item
            {
                Article = item.Article,
                Brand = check.Brand.Name,
                Category = check.Category.Name,
                Count = item.Count,
                ItemId = check.ProductId,
                Name = item.Name
            };
            res.Add(toAdd);
        }

        return res;
    }

    public async Task<Item?> ItemById(int id)
    {
        var nav = _context.Products.Include(x => x.Category);
        var prod = await nav.FirstOrDefaultAsync(x => x.ProductId == id);
        if (prod == null)
        {
            throw new ArgumentException("Wrong Id");
        }

        var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductStockId == prod.ProductStockId);
        if (stock is null)
        {
            throw new Exception("Unable to check stocks");
        }
        var brand = await _context.Brands.FirstOrDefaultAsync(x => x.BrandId == prod.BrandId);
        if (brand is null)
        {
            throw new Exception("Unable to find brand");
        }
        
        return new Item
        {
            Brand = brand.Name, 
            Count = stock.Stock, 
            Name = prod.Name,
            ItemId = prod.ProductId,
            Category = prod.Category.Name,
            Article = prod.Article
        };
    }

    public async Task<Item?> ItemByName(string name)
    {
        var nav = _context.Products.Include(x => x.Category);
        var prod = await nav.FirstOrDefaultAsync(x => x.Name == name);
        if (prod == null)
        {
            throw new ArgumentException("Wrong Product Name");
        }

        var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductStockId == prod.ProductStockId);
        if (stock is null)
        {
            throw new Exception("Unable to check stocks");
        }
        var brand = await _context.Brands.FirstOrDefaultAsync(x => x.BrandId == prod.BrandId);
        if (brand is null)
        {
            throw new Exception("Unable to check brand");
        }
        
        var res = new Item
        {
            Brand = brand.Name, 
            Count = stock.Stock, 
            Name = prod.Name,
            ItemId = prod.ProductId,
            Category = prod.Category.Name,
            Article = prod.Article,
            Descr = prod.Description
        };

        return res;
    }

    public async Task<ICollection<Item>> ItemsByBrandName(string name)
    {
        var res = new List<Item>();
        var nav = _context.Brands
            .Include(x => x.Products)
            .ThenInclude(y => y.Category);
        var brand = await nav.FirstOrDefaultAsync(x => x.Name == name);
        if (brand == null)
        {
            throw new ArgumentException("Wrong Brand Name");
        }

        if (!brand.Products.Any())
            throw new ArgumentException("No matching Items");

        foreach (var product in brand.Products)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == product.ProductId);
            if (stock is null)
            {
                throw new Exception("Unable to check stocks");
            }
            var model = new Item
            {
                Brand = brand.Name, 
                Count = stock.Stock, 
                Name = product.Name,
                ItemId = product.ProductId,
                Category = product.Category.Name,
                Article = product.Article
            };
            res.Add(model);
        }

        return res;
    }

    public async Task NewItem(Item model)
    {
        var brand = await _context.Brands.FirstOrDefaultAsync(x => x.Name == model.Brand);
        if (brand == null)
            throw new ArgumentException("No such Brand");
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == model.Category);
        if (category == null)
            throw new ArgumentException("No such category");
        var prod = new Product
        {
            Brand = brand,
            BrandId = brand.BrandId,
            Article = model.Article,
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
        Brand result = new Brand
        {
            Description = model.Descr,
            Name = model.Name
        };
        _context.Brands.Add(result);
        _context.SaveChanges();
    }

    public async Task NewCategory(CategoryModel model)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == model.Name);
        if (category is not null)
            throw new Exception("Category already exists");
        Category res = new Category
        {
            Name = model.Name
        };

        _context.Categories.Add(res);
        _context.SaveChanges();
    }
}