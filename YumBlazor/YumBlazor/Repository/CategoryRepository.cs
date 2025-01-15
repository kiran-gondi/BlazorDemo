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

        public async Task<Category> CreateAsync(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            await _applicationDbContext.AddAsync(category);
            await _applicationDbContext.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int categoryId)
        {
            bool isDeleted = false;
            var categoryToDelete = await _applicationDbContext.Category.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (categoryToDelete != null)
            {
                _applicationDbContext.Remove(categoryToDelete);
                isDeleted = (await _applicationDbContext.SaveChangesAsync()) > 0;
            }
            return isDeleted;
        }

        public async Task<Category> GetAsync(int categoryId)
        {
            Category category = await _applicationDbContext.Category.FirstOrDefaultAsync(x => x.Id == categoryId);
            if(category == null)
            {
                return new Category();
            }
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _applicationDbContext.Category.ToListAsync();
        }

        public async Task<Category> UpdateAsync(Category categoryToUpdate)
        {
            Category existingCategory = await _applicationDbContext.Category.FirstOrDefaultAsync(x => x.Id == categoryToUpdate.Id);
            
            if (existingCategory is not null)
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
