using Models;
using AutoMapper;

namespace Helpers
{
    public class EmailUserAutoMapperProfile : Profile
    {
        public EmailUserAutoMapperProfile()
        {
            CreateMap<RegisterModel, EmailUser>();
            CreateMap<AuthenticateModel, EmailUser>();
        }
    }
}