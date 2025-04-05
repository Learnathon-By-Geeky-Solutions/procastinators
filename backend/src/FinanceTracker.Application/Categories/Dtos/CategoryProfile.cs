using AutoMapper;
using FinanceTracker.Application.Categories.Commands.CreateCategory;
using FinanceTracker.Application.Categories.Commands.UpdateCategory;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Categories.Dtos;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryCommand, Category>();
        CreateMap<UpdateCategoryCommand, Category>();
        CreateMap<Category, CategoryDto>();
    }
}
