﻿using Core.Exceptions;
using Core.Exceptions.Models;
using Core.Extensions;
using Core.Helpers;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Service.Extensions;
using Kantin.Service.Models.Auth;
using Kantin.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class AccountProvider : GenericProvider<Account, KantinEntities>
    {
        public AccountProvider(KantinEntities context) : base(context) { }

        public async Task<LoginResult> Login(ITokenAuthorizationService tokenService, Login login)
        {
            login.Validate();

            var hashedPassword = PasswordHelper.GenerateHash(login.Password);

            var account = Context.Accounts.FirstOrDefault(account =>
                account.Username.ToLower() == login.Username.ToLower() && account.Password == hashedPassword);

            return await ProcessSession(tokenService, account);
        }

        public async Task<LoginResult> Register(ITokenAuthorizationService tokenService, Register register)
        {
            register.Validate();
            CheckRegisterEligibility(register);

            try
            {
                var account = new Account
                {
                    Id = Guid.NewGuid(),
                    Fullname = register.Fullname,
                    Username = register.Username,
                    Password = PasswordHelper.GenerateHash(register.Password),
                    IsArchived = false
                };

                var createdAccount = await Create(account);

                return await ProcessSession(tokenService, createdAccount);
            }
            catch
            {
                Context.Rollback();
                return GenerateLoginResult(null);
            }
        }

        private void CheckRegisterEligibility(Register register)
        {
            var propertyErrors = new List<PropertyErrorResult>();
            var errorMessageTemplate = "{0} with value of {1} has been taken";

            var usernameExisted = Context.Accounts.Any(a => a.Username.ToLower() == register.Username.ToLower());

            if (!usernameExisted)
                return;

            propertyErrors.Add(new PropertyErrorResult
            {
                FieldName = nameof(register.Username),
                FieldErrors = string.Format(errorMessageTemplate, nameof(register.Username), register.Username)
            });

            throw new ConflictException(propertyErrors);
        }

        private async Task<LoginResult> ProcessSession(ITokenAuthorizationService tokenService, Account account)
        {
            var loginResult = GenerateLoginResult(account);

            if (!loginResult.Success)
                return loginResult;

            await tokenService.SaveToken(loginResult.Token, account.Id);

            return loginResult;
        }

        private LoginResult GenerateLoginResult(Account account)
        {
            var success = account != null;
            var token = success ? JWTHelper.Instance.GenerateToken(account.ToJWTContainer()) : null;

            return new LoginResult()
            {
                Success = success,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Token = token
            };
        }
    }
}
