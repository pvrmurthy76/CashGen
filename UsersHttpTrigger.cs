using AutoMapper;
using CashGen.DBContexts;
using CashGen.Entities;
using CashGen.Models;
using CashGen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashGen
{
    public class UsersHttpTrigger
    {
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        public UsersHttpTrigger(
          ICashGenRepository cashGenRepository,
          CashGenContext context,
          IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        public userAuth authenticateUser(string token)
        {
            userAuth userAuth = new userAuth();
            userAuth.valid = false;
            bool flag = false;
            if (new CashGen.Shared.Generic().isValidGuid(token))
            {
                if (((IQueryable<User>)this._context.Users).Where<User>((Expression<Func<User, bool>>)(c => c.Id == new Guid(token))).Count<User>() > 0)
                    flag = true;
            }
            if (flag)
            {
                userAuth.valid = true;
                userAuth.auth_token = new Guid(token);
            }
            return userAuth;
        }

        [FunctionName("GetUsers")]
        public IActionResult GetUsers([HttpTrigger] HttpRequest req, ILogger log) => this.authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid ? (IActionResult)new OkObjectResult((object)this._mapper.Map<IEnumerable<UserListDto>>((object)(IEnumerable<User>)this._context.Users)) : (IActionResult)new BadRequestResult();

        [FunctionName("GetUser")]
        public IActionResult GetUser([HttpTrigger] HttpRequest req, Guid id, ILogger log) => this.authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid ? (IActionResult)new OkObjectResult((object)this._mapper.Map<UserDto>((object)this._cashGenRepository.GetUser(id))) : (IActionResult)new BadRequestResult();

        [FunctionName("GetUserLogin")]
        public async Task<IActionResult> GetUserLogin([HttpTrigger] HttpRequest req, ILogger log)
        {
            UserForLoginDto userForLoginDto = JsonConvert.DeserializeObject<UserForLoginDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync());
            return (IActionResult)new OkObjectResult((object)this._mapper.Map<UserDto>((object)this._cashGenRepository.GetUserLogin(userForLoginDto.Email, userForLoginDto.Password)));
        }

        [FunctionName("CreateUser")]
        public async Task<IActionResult> CreateUser([HttpTrigger] HttpRequest req, ILogger log)
        {
            if (!this.authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            User user = this._mapper.Map<User>((object)JsonConvert.DeserializeObject<UserForCreationDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync()));
            this._cashGenRepository.AddUser(user);
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)this._mapper.Map<UserDto>((object)user));
        }

        [FunctionName("WelcomeUser")]
        public async Task<IActionResult> WelcomeUser(
          [HttpTrigger] HttpRequest req,
          Guid id,
          ILogger log)
        {
            User user = this._cashGenRepository.GetUser(id);
            this._mapper.Map<UserResetDto, User>(new UserResetDto()
            {
                Email = user.Email,
                ResetToken = Guid.NewGuid()
            }, user);
            this._cashGenRepository.UpdateUser(user);
            this._cashGenRepository.Save();
            CashGen.Models.Mail mail1 = new CashGen.Models.Mail();
            CashGen.Helpers.Mail mail2 = new CashGen.Helpers.Mail();
            mail1.to = new List<string>() { user.Email };
            mail1.name = user.FirstName;
            mail1.email = user.Email;
            mail1.password = user.Password;
            mail1.templateId = 20513235;
            mail1.resetToken = user.ResetToken.ToString();
            CashGen.Models.Mail msg = mail1;
            mail2.SendMail(msg);
            return (IActionResult)new OkObjectResult((object)true);
        }

        [FunctionName("UpdateUser")]
        public async Task<IActionResult> UpdateUser(
          [HttpTrigger] HttpRequest req,
          Guid id,
          ILogger log)
        {
            if (!this.authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            UserForUpdateDto userForUpdateDto = JsonConvert.DeserializeObject<UserForUpdateDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync());
            User user = this._cashGenRepository.GetUser(id);
            this._mapper.Map<UserForUpdateDto, User>(userForUpdateDto, user);
            this._cashGenRepository.UpdateUser(user);
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)this._mapper.Map<UserDto>((object)user));
        }

        [FunctionName("ResetPassword")]
        public async Task<IActionResult> ResetPassword([HttpTrigger] HttpRequest req, ILogger log)
        {
            if (!this.authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            UserResetDto userResetDto = JsonConvert.DeserializeObject<UserResetDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync());
            userResetDto.ResetToken = Guid.NewGuid();
            User userByEmail = this._cashGenRepository.GetUserByEmail(userResetDto.Email);
            this._mapper.Map<UserResetDto, User>(userResetDto, userByEmail);
            this._cashGenRepository.UpdateUser(userByEmail);
            this._cashGenRepository.Save();
            CashGen.Models.Mail mail1 = new CashGen.Models.Mail();
            CashGen.Helpers.Mail mail2 = new CashGen.Helpers.Mail();
            mail1.to = new List<string>() { userByEmail.Email };
            mail1.name = userByEmail.FirstName;
            mail1.email = userByEmail.Email;
            mail1.templateId = 20547547;
            mail1.resetToken = userByEmail.ResetToken.ToString();
            CashGen.Models.Mail msg = mail1;
            mail2.SendMail(msg);
            return (IActionResult)new OkObjectResult((object)true);
        }

        [FunctionName("SetPassword")]
        public async Task<IActionResult> SetPassword([HttpTrigger] HttpRequest req, ILogger log)
        {
            UserPasswordDto userPasswordDto = JsonConvert.DeserializeObject<UserPasswordDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync());
            User userByResetToken = this._cashGenRepository.GetUserByResetToken(userPasswordDto.ResetToken);
            userPasswordDto.ResetToken = Guid.Empty;
            this._mapper.Map<UserPasswordDto, User>(userPasswordDto, userByResetToken);
            this._cashGenRepository.UpdateUser(userByResetToken);
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)true);
        }

        [FunctionName("DeleteUser")]
        public IActionResult DeleteUser([HttpTrigger] HttpRequest req, Guid id, ILogger log)
        {
            if (!this.authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            this._cashGenRepository.DeleteUser(this._cashGenRepository.GetUser(id));
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)true);
        }

        [FunctionName("CreateStoreUser")]
        public async Task<IActionResult> CreateStoreUser([HttpTrigger] HttpRequest req, ILogger log)
        {
            if (!this.authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            this._cashGenRepository.AddStoreUser(this._mapper.Map<StoreUser>((object)JsonConvert.DeserializeObject<StoreUserForCreationDto>(await ((TextReader)new StreamReader(req.Body)).ReadToEndAsync())));
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)true);
        }

        [FunctionName("DeleteStoreUser")]
        public IActionResult DeleteStoreUser(
          [HttpTrigger] HttpRequest req,
          Guid user,
          Guid store,
          ILogger log)
        {
            if (!this.authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            this._cashGenRepository.DeleteStoreUser(((IQueryable<StoreUser>)this._context.StoreUsers).Where<StoreUser>((Expression<Func<StoreUser, bool>>)(c => c.UserId == user && c.StoreId == store)).FirstOrDefault<StoreUser>());
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)true);
        }

        [FunctionName("GetUserStores")]
        public IActionResult GetUserStores([HttpTrigger] HttpRequest req, Guid id, ILogger log) => this.authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid ? (IActionResult)new OkObjectResult((object)this._mapper.Map<IEnumerable<StoreListDto>>((object)this._cashGenRepository.GetUserStores(id))) : (IActionResult)new BadRequestResult();
    }
}
