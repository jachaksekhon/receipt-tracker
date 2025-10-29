using AutoMapper;
using ReceiptTracker.DTOs.Auth;
using ReceiptTracker.DTOs.Users;
using ReceiptTracker.Models;

namespace ReceiptTracker.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserReadDto>();
        CreateMap<UserRegisterDto, User>();
        CreateMap<UserCreateDto, User>();
    }
}
