using AutoMapper;
using Kantin.Data.Models;
using Kantin.Models.Common;
using System.Linq;

namespace Kantin.Models.Profiles
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItem, EditableMenuItem>(MemberList.Source)
                .ForMember(dst => dst.AddOnItems, opt => opt.MapFrom(src => src.MenuAddOnItems.Select(m => m.AddOnItem)));

            CreateMap<EditableMenuItem, MenuItem>(MemberList.Source)
                .ForMember(dst => dst.MenuAddOnItems.Select(mad => mad.AddOnItemId), opt => opt.MapFrom( src => src.AddOnItems.Select(a => a.Id)))
                .ForMember(dst => dst.MenuAddOnItems.Select(mad => mad.MenuItemId), opt => opt.MapFrom( src => src.Id));
        }
    }
}
