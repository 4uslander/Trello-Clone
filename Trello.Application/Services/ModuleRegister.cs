using Microsoft.Extensions.DependencyInjection;
using Trello.Application.Services.BoardMemberServices;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.CardMemberServices;
using Trello.Application.Services.CardServices;
using Trello.Application.Services.CommentServices;
using Trello.Application.Services.ListServices;
using Trello.Application.Services.RoleServices;
using Trello.Application.Services.ToDoServices;
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
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IBoardMemberService, BoardMemberService>();
            services.AddScoped<ICardMemberService, CardMemberService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IToDoService, ToDoService>();
        }
    }
}

