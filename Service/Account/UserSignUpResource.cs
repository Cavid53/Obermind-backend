using AutoMapper;
using Domain.Entities;
using Service.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Service.Account
{
    public class UserSignUpResource:IResourceMapper
    {
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Password { get; set; }
     
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserSignUpResource, AppUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(ur => ur.Email))
                .ReverseMap();
        }

    }
}
