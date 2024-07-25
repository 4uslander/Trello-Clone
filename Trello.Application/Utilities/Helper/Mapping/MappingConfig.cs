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
                mc.AddProfile(new RoleProfile());
                mc.AddProfile(new CardProfile());
                mc.AddProfile(new BoardMemberProfile());
                mc.AddProfile(new CardMemberProfile());
                mc.AddProfile(new CommentProfile());
                mc.AddProfile(new ToDoProfile());
                mc.AddProfile(new TaskProfile());
                mc.AddProfile(new LabelProfile());
                mc.AddProfile(new CardLabelProfile());
                mc.AddProfile(new CardActivityProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
