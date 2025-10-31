using AutoMapper;
using ReceiptTracker.Application.DTOs.Auth;
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
    }
}
