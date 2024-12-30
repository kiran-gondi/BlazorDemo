using YumBlazor.Data;

namespace YumBlazor.Repository.IRepository
{
    public interface ICategoryRepository
    {
        public Task<Category> Create(Category category);
        public Task<Category> Update(Category category);
        public Task<bool> Delete(int categoryId);
        public Task<IEnumerable<Category>> GetAll();
        public Task<Category> Get(int categoryId);
    }
}
