using EShop.ProductApi.Context;
using EShop.ProductApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.ProductApi.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Category>> GetCategories()
    {
        //Não é recomendávek usar ToListAsync pois ele retorna todas as categorias na memoria
        //estamos utilizando apenas por ter poucas categorias
        return await _context.Categories.ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetCategoriesProducts()
    {
        return await _context.Categories.Include(p => p.Products).ToListAsync();
    }

    public async Task<Category> GetById(int id)
    {
        return await _context.Categories.Where(c => c.CategoryId == id).FirstOrDefaultAsync();
    }

    public async Task<Category> Create(Category category)
    {
        //aqui modifica
        _context.Categories.Add(category);
        //aqui persirste as modificações no banco
        //Nesse caso seria interessante utilizar um UOF
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> Update(Category category)
    {
        _context.Entry(category).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> Delete(int id)
    {
        var category = await GetById(id);
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return category;
    }
}
