using FluentValidation;
using FluentValidation.AspNetCore;
using LoanSystem.Application.Interface;
using LoanSystem.Application.Services;
using LoanSystem.Application.Validations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddValidatorsFromAssemblyContaining(typeof(LoansValidationsRequest));
            services.AddFluentValidationAutoValidation();
            return services;
        }
    }
}
