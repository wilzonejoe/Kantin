using AutoMapper;
using Kantin.Data.Models;
using Kantin.Models.Request;
using Kantin.Models.Response;
using System.Collections.Generic;
using System.Linq;

namespace Kantin.Models.Profiles
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItem, EditableMenuItemResponse>(MemberList.Source)
                .ForMember(dst => dst.AddOnItems, opt => opt.MapFrom(src => src.MenuAddOnItems.Select(m => m.AddOnItem)));

            CreateMap<EditableMenuItemRequest, MenuItem>(MemberList.Source)
                .ForMember(dst => dst.MenuAddOnItems, opt => opt.MapFrom(src => 
                    src.AddOnItemIds.Select(addOnId => new MenuAddOnItem { AddOnItemId = addOnId })));
        }
    }
}
