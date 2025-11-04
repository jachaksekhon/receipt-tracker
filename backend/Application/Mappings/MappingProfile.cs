using AutoMapper;
using ReceiptTracker.Application.DTOs.Auth;
using ReceiptTracker.Application.DTOs.ReceiptItems;
using ReceiptTracker.Application.DTOs.Receipts;
using ReceiptTracker.Application.DTOs.Users;
using ReceiptTracker.Domain.Models;
using ReceiptTracker.Domain.Models.Receipts;

namespace ReceiptTracker.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserReadDto>();
        CreateMap<UserRegisterDto, User>();
        CreateMap<UserCreateDto, User>();

        CreateMap<Receipt, ReceiptReadDto>();
        CreateMap<ReceiptItem, ReceiptItemReadDto>();
        CreateMap<ReceiptItemCreateDto, ReceiptItem>();
        CreateMap<Receipt, ReceiptDashboardDto>();

        // Receipt <-> DTO
        CreateMap<Receipt, ReceiptReadDto>()
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ReverseMap();

        // ReceiptItem <-> DTO
        CreateMap<ReceiptItem, ReceiptItemReadDto>()
            .ForMember(dest => dest.ProductSku, opt => opt.MapFrom(src => src.ProductSku))
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemName))
            .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(src => src.OriginalPrice))
            .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount))
            .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => src.FinalPrice))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ReverseMap();
    }
}
