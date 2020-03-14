using AutoMapper;
using Kantin.Data.Models;
using Kantin.Models.Request;
using Kantin.Models.Response;
using System.Linq;

namespace Kantin.Models.Profiles
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<Menu, EditableMenuResponse>(MemberList.Source)
                .ForMember(dst => dst.MenuItems, opt => opt.MapFrom(src => src.MenuItemsOnMenus.Select(m => m.MenuItem)));

            CreateMap<EditableMenuRequest, Menu>(MemberList.Source)
                .ForMember(dst => dst.MenuItemsOnMenus, opt => opt.MapFrom(src => 
                    src.MenuItemIds.Select(menuItemId => new MenuItemOnMenu { MenuItemId = menuItemId })));
        }
    }
}
