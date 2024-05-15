using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.Services;

namespace Trello.Application
{
    public static class DependencyInjection
    {
        public static void InfrastructureRegister(this IServiceCollection services)
        {
            services.ServiceRegister();
            
        }
    }
}
