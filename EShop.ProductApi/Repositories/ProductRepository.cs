using EShop.ProductApi.Context;
using EShop.ProductApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.ProductApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Product>> GetAll()
    {
        //Não é recomendávek usar ToListAsync pois ele retorna todas as categorias na memoria
        //estamos utilizando apenas por ter poucas categorias
        return await _context.Products.Include(c =>c.Category).ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
        return await _context.Products.Include(c => c.Category).Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Product> Create(Product product)
    {
        //aqui modifica
        _context.Products.Add(product);
        //aqui persirste as modificações no banco
        //Nesse caso seria interessante utilizar um UOF
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Delete(int id)
    {
        var product = await GetById(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return product;
    }
}
