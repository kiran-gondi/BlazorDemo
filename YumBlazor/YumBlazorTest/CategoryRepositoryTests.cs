using Microsoft.EntityFrameworkCore;
using YumBlazor.Data;
using YumBlazor.Repository.IRepository;
using EntityFrameworkCoreMock;
using YumBlazor.Repository;
using AutoFixture;
using FluentAssertions;

namespace YumBlazorTest
{
    public class CategoryRepositoryTests
    {
        private readonly IFixture _fixture;

        private readonly ICategoryRepository _categoryRepository;
        private IEnumerable<Category> categoryInitialData;
        private const int TestCategoryId = 1;

        public CategoryRepositoryTests()
        {
            _fixture = new Fixture();

            categoryInitialData = new List<Category>() { };

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
            new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Category, categoryInitialData);

            _categoryRepository = new CategoryRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task AdddCategory_Null()
        {
            Category category = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => 
            {
                await _categoryRepository.Create(category);
            });
        }

        [Fact]
        public async Task AddNewCategory_IsSuccess()
        {
            Category categoryToAdd = _fixture.Build<Category>()
                                            .With(x=>x.Id, TestCategoryId)
                                            .Create();

            await _categoryRepository.Create(categoryToAdd);

            Category categoryAdded = await _categoryRepository.Get(categoryToAdd.Id);

            categoryAdded.Id.Should().Be(TestCategoryId);
            categoryAdded.Name.Should().Be(categoryToAdd.Name);

            categoryAdded.Should().Be(categoryAdded);
        }

        [Fact]
        public async Task CategoryDelete_Valid()
        {
            Category categoryToAdd = _fixture.Build<Category>()
                                            .With(x => x.Id, TestCategoryId)
                                            .Create();

            await _categoryRepository.Create(categoryToAdd);

            bool isDeleted = await _categoryRepository.Delete(categoryToAdd.Id);

            isDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task CategoryDelete_InValid()
        {
            bool isDeleted = await _categoryRepository.Delete(TestCategoryId);
            isDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task CategoryGet_Valid()
        {
            Category categoryToAdd = _fixture.Build<Category>()
                                            .With(x => x.Id, TestCategoryId)
                                            .Create();

            await _categoryRepository.Create(categoryToAdd);

            Category categoryExists = await _categoryRepository.Get(categoryToAdd.Id);

            categoryExists.Should().NotBeNull();
            categoryExists.Id.Should().Be(TestCategoryId);
            categoryExists.Name.Should().Be(categoryToAdd.Name);
        }

        [Fact]
        public async Task CategoryGet_InValid()
        {
            Category categoryToAdd = _fixture.Build<Category>()
                                            .With(x => x.Id, TestCategoryId)
                                            .Create();

            await _categoryRepository.Create(categoryToAdd);

            Category categoryExists = await _categoryRepository.Get(123);

            categoryExists.Should().NotBeNull();
            categoryExists.Id.Should().Be(0);
            categoryExists.Name.Should().Be(null);
        }

        [Fact]
        public async Task CategoryGetAll_Valid()
        {
            Category categoryToAdd = _fixture.Build<Category>()
                                            .With(x => x.Id, TestCategoryId)
                                            .Create();

            await _categoryRepository.Create(categoryToAdd);

            IEnumerable<Category> categoryExists = await _categoryRepository.GetAll();

            categoryExists.Should().NotBeNull();
            categoryExists.Count().Should().Be(1);
        }

        [Fact]
        public async Task CategoryGetAll_InValid()
        {
            IEnumerable<Category> categoryExists = await _categoryRepository.GetAll();

            categoryExists.Should().BeEmpty();
            categoryExists.Count().Should().Be(0);
        }

        /*[Fact]
        public async Task UpdateCategory_InValid()
        {
            Category categoryToAdd = _fixture.Build<Category>()
                                           .With(x => x.Id, TestCategoryId)
                                           .Create();

            await _categoryRepository.Create(categoryToAdd);

            Category categoryExists = await _categoryRepository.Get(categoryToAdd.Id);

            categoryExists.Should().NotBeNull();
            categoryExists.Id.Should().Be(TestCategoryId);
            categoryExists.Name.Should().Be(categoryToAdd.Name);

            Category updatedCategory = await _categoryRepository.Update(categoryExists);

            updatedCategory.Should().NotBeNull();
            updatedCategory.Id.Should().Be(categoryExists.Id);
            updatedCategory.Name.Should().Be(categoryExists.Name);
        }*/

        [Fact]  
        public async Task UpdateCategory_Valid()
        {
            Category categoryToAdd = _fixture.Build<Category>()
                                           .With(x => x.Id, TestCategoryId)
                                           .Create();

            await _categoryRepository.Create(categoryToAdd);

            Category categoryExists = await _categoryRepository.Get(categoryToAdd.Id);

            categoryExists.Should().NotBeNull();
            categoryExists.Id.Should().Be(TestCategoryId);
            categoryExists.Name.Should().Be(categoryToAdd.Name);

            categoryExists.Id = 22;
            categoryExists.Name = "Test update";
            Category updatedCategory = await _categoryRepository.Update(categoryExists);

            updatedCategory.Should().NotBeNull();
            updatedCategory.Id.Should().Be(categoryExists.Id);
            updatedCategory.Name.Should().Be(categoryExists.Name);
        }
    }
}
