using Microsoft.EntityFrameworkCore;
using YumBlazor.Data;
using YumBlazor.Repository.IRepository;

namespace YumBlazor.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Category> Create(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            _applicationDbContext.Add(category);
            await _applicationDbContext.SaveChangesAsync();
            return category;
        }

        public async Task<bool> Delete(int categoryId)
        {
            bool isDeleted = false;
            var categoryToDelete = _applicationDbContext.Category.FirstOrDefault(c => c.Id == categoryId);
            if (categoryToDelete != null)
            {
                _applicationDbContext.Remove(categoryToDelete);
                isDeleted = await _applicationDbContext.SaveChangesAsync() > 0;
            }
            return isDeleted;
        }

        public async Task<Category> Get(int categoryId)
        {
            Category category = await _applicationDbContext.Category.FirstOrDefaultAsync(x => x.Id == categoryId);
            if(category == null)
            {
                return new Category();
            }
            return category;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _applicationDbContext.Category.ToListAsync();
        }

        public async Task<Category> Update(Category categoryToUpdate)
        {
            Category existingCategory = await _applicationDbContext.Category.FirstOrDefaultAsync(x => x.Id == categoryToUpdate.Id);
            
            if (existingCategory != null)
            {
                existingCategory.Name = categoryToUpdate.Name;
                _applicationDbContext.Category.Update(existingCategory);
                await _applicationDbContext.SaveChangesAsync();
                return existingCategory;
            }

            return categoryToUpdate;
        }
    }
}
