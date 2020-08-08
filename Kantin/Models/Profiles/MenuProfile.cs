using AutoMapper;
using Kantin.Data.Models;
using Kantin.Models.Request;
using Kantin.Models.Response;
using System.Linq;

namespace Kantin.Models.Profiles
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Menu, EditableMenuResponse>(MemberList.Source)
                .ForMember(dst => dst.MenuItems, opt => opt.MapFrom(src => src.MenuItemMenus.Select(m => m.MenuItem)));

            CreateMap<EditableMenuRequest, Menu>(MemberList.Source)
                .ForMember(dst => dst.MenuItemMenus, opt => opt.MapFrom(src => 
                    src.MenuItemIds.Select(menuItemId => new MenuItemMenu { MenuItemId = menuItemId })));
        }
    }
}
