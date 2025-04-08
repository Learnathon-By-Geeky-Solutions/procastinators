using AutoMapper;
using FinanceTracker.Application.Categories.Commands.CreateCategory;
using FinanceTracker.Application.Categories.Commands.UpdateCategory;
using FinanceTracker.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FinanceTracker.Application.Categories.Dtos.Tests;

public class CategoryProfileTests
{
    private IMapper _mapper;

    public CategoryProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CategoryProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForCategoryToCategoryDto_MapsCorrectly()
    {
        // Arrange

        var category = new Category()
        {
            Id = 1,
            Title = "test",
            DefaultTransactionType = "Income",
        };

        // Act

        var categoryDto = _mapper.Map<CategoryDto>(category);

        // Assert

        categoryDto.Should().NotBeNull();
        categoryDto.Id.Should().Be(category.Id);
        categoryDto.Title.Should().Be(category.Title);
        categoryDto.DefaultTransactionType.Should().Be(category.DefaultTransactionType);
    }

    [Theory()]
    [InlineData("Income")]
    [InlineData("Expense")]
    public void CreateMap_ForCreateCategoryCommandToCategory_MapsCorrectly(string defaultTransactionTypes)
    {
        // Arrange

        var command = new CreateCategoryCommand()
        {
            Title = "234",
            DefaultTransactionType = defaultTransactionTypes
        };

        // Act

        var category = _mapper.Map<Category>(command);

        // Assert

        category.Should().NotBeNull();
        category.Title.Should().Be(command.Title);
        category.DefaultTransactionType.Should().Be(command.DefaultTransactionType);
    }

    [Theory()]
    [InlineData("Income")]
    [InlineData("Expense")]
    public void CreateMap_ForUpdateCategoryCommandToCategory_MapsCorrectly(string defaultTransactionTypes)
    {
        // Arrange

        var command = new UpdateCategoryCommand()
        {
            Title = "234",
            DefaultTransactionType = defaultTransactionTypes
        };

        // Act

        var category = _mapper.Map<Category>(command);

        // Assert

        category.Should().NotBeNull();
        category.Title.Should().Be(command.Title);
        category.DefaultTransactionType.Should().Be(command.DefaultTransactionType);
    }
}