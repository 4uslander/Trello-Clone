using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.ListServices;
using Trello.Application.Services.UserServices;
using Trello.Application.Utilities.Helper.JWT;
using Trello.Infrastructure.IRepositories;
using Trello.Infrastructure.Repositories;

namespace Trello.Application.Services
{
    public static class ModuleRegister
    {
        public static void ServiceRegister(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtHelper, JwtHelper>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<IListService, ListService>();
        }
    }
}

