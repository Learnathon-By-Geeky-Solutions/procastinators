using AutoMapper;
using FinanceTracker.Application.Categories.Commands.CreateCategory;
using FinanceTracker.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FinanceTracker.Application.Categories.Dtos.Tests;

public class CategoryProfileTests
{
    [Fact()]
    public void CreateMap_ForCategoryToCategoryDto_MapsCorrectly()
    {
        // Arrange

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CategoryProfile>();
        });

        var mapper = configuration.CreateMapper();

        var category = new Category()
        {
            Id = 1,
            Title = "test",
            DefaultTransactionType = "Income",
        };

        // Act

        var categoryDto = mapper.Map<CategoryDto>(category);

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

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CategoryProfile>();
        });

        var mapper = configuration.CreateMapper();

        var command = new CreateCategoryCommand()
        {
            Title = "234",
            DefaultTransactionType = defaultTransactionTypes
        };

        // Act

        var category = mapper.Map<Category>(command);

        // Assert

        category.Should().NotBeNull();
        category.Title.Should().Be(category.Title);
        category.DefaultTransactionType.Should().Be(category.DefaultTransactionType);
    }
}