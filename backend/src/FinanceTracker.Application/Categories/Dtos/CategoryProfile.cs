using AutoMapper;
using FinanceTracker.Application.Categories.Commands.CreateCategory;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Categories.Dtos;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryCommand, Category>();
        CreateMap<Category, CategoryDto>();
    }
}
