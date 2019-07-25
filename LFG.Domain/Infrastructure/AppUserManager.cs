using LFG.Domain.Concrete;
using LFG.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace LFG.Domain.Infrastructure
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store)
            : base(store)
        { }

        // this method is called by Owin therefore this is the best place to configure your User Manager
        public static AppUserManager Create(
            IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            AppUserManager manager = new AppUserManager(
                new UserStore<AppUser>(context.Get<EFDbContext>()))
            {

                // optionally configure your manager
                UserValidator = new CustomUserValidator()
            };

            return manager;
        }
    }
    public class CustomUserValidator : IIdentityValidator<AppUser>
    {
        public async Task<IdentityResult> ValidateAsync(AppUser item)
        {
            List<string> errors = new List<string>();

            if (String.IsNullOrEmpty(item.UserName.Trim()))
                errors.Add("Вы указали пустое имя.");

            string userNamePattern = @"^[a-zA-Z0-9а-яА-Я]+$";

            if (!Regex.IsMatch(item.UserName, userNamePattern))
                errors.Add("В имени разрешается указывать буквы английского или русского языков, и цифры");

            if (errors.Count > 0)
                return IdentityResult.Failed(errors.ToArray());

            return IdentityResult.Success;
        }
    }
}