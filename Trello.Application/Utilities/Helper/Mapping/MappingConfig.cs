using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.Utilities.Helper.Mapping.MappingProfile;

namespace Trello.Application.Utilities.Helper.Mapping
{
    public static class MappingConfig
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new BoardProfile());
                mc.AddProfile(new ListProfile());
                mc.AddProfile(new CardProfile());
                mc.AddProfile(new BoardMemberProfile());
                mc.AddProfile(new RoleProfile());
                mc.AddProfile(new CardMemberProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

    }
}
