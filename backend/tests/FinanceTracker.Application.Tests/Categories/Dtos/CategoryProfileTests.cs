using AutoMapper;
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
}