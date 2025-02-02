using Microsoft.EntityFrameworkCore;
using YumBlazor.Data;
using YumBlazor.Repository.IRepository;
using EntityFrameworkCoreMock;
using YumBlazor.Repository;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace YumBlazorTest
{
    public class CategoryRepositoryTests
    {
        private readonly IFixture _fixture;

        private readonly ICategoryRepository _categoryRepository;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
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

            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _categoryRepository = _categoryRepositoryMock.Object;
            _categoryRepository = new CategoryRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task AdddCategory_Null()
        {
            Category category = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => 
            {
                await _categoryRepository.CreateAsync(category);
            });
        }

        [Fact]
        public async Task AddNewCategory_IsSuccess()
        {
            Category categoryToAdd = _fixture.Build<Category>()
                                            .With(x=>x.Id, TestCategoryId)
                                            .Create();

            _categoryRepositoryMock
                .Setup(temp => temp.CreateAsync(It.IsAny<Category>()))
                .ReturnsAsync(categoryToAdd);

            //await _categoryRepository.CreateAsync(categoryToAdd);

            _categoryRepositoryMock
                .Setup(temp => temp.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(categoryToAdd);

            categoryToAdd = await _categoryRepository.CreateAsync(categoryToAdd);

            //Category categoryAdded = await _categoryRepository.GetAsync(categoryToAdd.Id);

            categoryToAdd.Id.Should().Be(TestCategoryId);
            categoryToAdd.Name.Should().Be(categoryToAdd.Name);

            //categoryToAdd.Should().Be(categoryAdded);
        }

        [Fact]
        public async Task CategoryDelete_Valid()
        {
            Category categoryToAdd = _fixture.Build<Category>()
                                            .With(x => x.Id, TestCategoryId)
                                            .Create();

            await _categoryRepository.CreateAsync(categoryToAdd);

            bool isDeleted = await _categoryRepository.DeleteAsync(categoryToAdd.Id);

            isDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task CategoryDelete_InValid()
        {
            bool isDeleted = await _categoryRepository.DeleteAsync(TestCategoryId);
            isDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task CategoryGet_Valid()
        {
            Category categoryToAdd = _fixture.Build<Category>()
                                            .With(x => x.Id, TestCategoryId)
                                            .Create();

            await _categoryRepository.CreateAsync(categoryToAdd);

            Category categoryExists = await _categoryRepository.GetAsync(categoryToAdd.Id);

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

            await _categoryRepository.CreateAsync(categoryToAdd);

            Category categoryExists = await _categoryRepository.GetAsync(123);

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

            await _categoryRepository.CreateAsync(categoryToAdd);

            IEnumerable<Category> categoryExists = await _categoryRepository.GetAllAsync();

            categoryExists.Should().NotBeNull();
            categoryExists.Count().Should().Be(1);
        }

        [Fact]
        public async Task CategoryGetAll_InValid()
        {
            IEnumerable<Category> categoryExists = await _categoryRepository.GetAllAsync();

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

            await _categoryRepository.CreateAsync(categoryToAdd);

            Category categoryExists = await _categoryRepository.GetAsync(categoryToAdd.Id);

            categoryExists.Should().NotBeNull();
            categoryExists.Id.Should().Be(TestCategoryId);
            categoryExists.Name.Should().Be(categoryToAdd.Name);

            categoryExists.Id = 22;
            categoryExists.Name = "Test update";
            Category updatedCategory = await _categoryRepository.UpdateAsync(categoryExists);

            updatedCategory.Should().NotBeNull();
            updatedCategory.Id.Should().Be(categoryExists.Id);
            updatedCategory.Name.Should().Be(categoryExists.Name);
        }
    }
}
