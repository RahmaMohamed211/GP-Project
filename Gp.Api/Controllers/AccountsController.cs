using AutoMapper;
using Gp.Api.Controllers;
using Gp.Api.Errors;
using GP.Core.Repositories;
using GP.Repository;
using GP.Repository.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using GP.APIs.Dtos;
using GP.core.Entities.identity;
using GP.core.Services;
using Gp.Api.Twilio;
using Twilio.TwiML.Messaging;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Caching.Memory;
using Gp.Api.Dtos;


namespace GP.APIs.Controllers
{

    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;
        private readonly ISmsMessage smsMessage;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IUserRepository accountRepo;

        public AccountsController(UserManager<AppUser> userManager,IMapper mapper ,IMemoryCache memoryCache, ISmsMessage smsMessage, SignInManager<AppUser> signInManager, ITokenService tokenService, IUserRepository AccountRepo)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
            this.smsMessage = smsMessage;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            accountRepo = AccountRepo;
        }

        [HttpPost("login")] //POST :api/accounts/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(401, "Invalid email or password.")); // Return message for invalid email

            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new ApiResponse(401, "Invalid email or password.")); // Return message for invalid password
            }

            //if (user.TwoFactorEnabled)
            //{
            // var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");

            // Generate a random 4-digit verification code
            var verificationCode = GenerateVerificationCode();

            // Compose the SMS message
            var sms = new Core.SMS
            {
                PhoneNumber = user.PhoneNumber,
                Body = $"Your verification code is: {verificationCode}"
            };

            // Send the SMS using SmsSetting
            var Sendresult = smsMessage.Send(sms);

            // Handle the result and return an appropriate response
            if (Sendresult != null && !string.IsNullOrEmpty(Sendresult.Sid))
            {
                memoryCache.Set(model.Email, verificationCode, TimeSpan.FromMinutes(5));
                return Ok(new
                {
                    user = new UserDto()
                    {
                        Id = user.Id,
                        DisplayName = user.DisplayName,
                        Email = user.Email,
                    verificationCode=verificationCode,
                        Token = await tokenService.CreateTokenAsyn(user, userManager)
                    },

                });


            }
            else
            {
                return BadRequest("Failed to send verification code");
            }


        }
        [HttpPost("verify")]
        [Authorize]
        public async Task<IActionResult> VerifyCode(VerificationDto model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

        

            // التحقق من صحة البريد الإلكتروني المستخدم
            if (!string.IsNullOrWhiteSpace(userEmail))
            {
                // استرجاع المستخدم من قاعدة البيانات باستخدام البريد الإلكتروني
                var user = await userManager.FindByEmailAsync(userEmail);

                // التأكد من وجود المستخدم
                if (user != null)
                {
                    // استرجاع رمز التحقق من MemoryCache باستخدام البريد الإلكتروني كمفتاح
                    if (memoryCache.TryGetValue(userEmail, out string storedCode))
                    {
                        if (model.VerificationCode == storedCode)
                        {
                            // تم التحقق بنجاح، قم بمسح رمز التحقق من MemoryCache
                            memoryCache.Remove(userEmail);
                            return Ok(" Verification successful");
                        }
                    }

                    return BadRequest("Invalid or expired verification code");
                }
            }

            // في حالة عدم وجود بريد إلكتروني مصرح به أو عدم وجود مستخدم
            return BadRequest("Authorized user or valid email not found.");
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (checkEmailExist(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "THis Email is Already Exist" }
                });

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,//Rahma.mohamed@gmail.com
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
                
            };
   
            var Result = await userManager.CreateAsync(user, model.Password);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));
            await userManager.SetTwoFactorEnabledAsync(user, true);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsyn(user, userManager),
                

            });
        }
        //[Authorize]
        //[HttpPost("login-2f")]
        //public async Task<IActionResult> VerifyCode(string verificationCode)
        //{
        //    // Get the currently authenticated user
        //    var email = User.FindFirstValue(ClaimTypes.Email);
        //    var user = await userManager.FindByEmailAsync(email);

        //    if (user == null)
        //    {
        //        // If user is not found, return unauthorized
        //        return Unauthorized();
        //    }

        //    // Check if the verification code provided by the user matches the one generated
        //    if (verificationCode == user.VerificationCode)
        //    {
        //        // If verification code is correct, log the user in
        //        await signInManager.SignInAsync(user, isPersistent: false);

        //        // Clear the verification code from the user entity
        //        user.VerificationCode = null;
        //        await userManager.UpdateAsync(user);

        //        // Return success message
        //        return Ok("Verification successful. User logged in.");
        //    }
        //    else
        //    {
        //        // If verification code is incorrect, return bad request
        //        return BadRequest("Incorrect verification code.");
        //    }
        //}

        private string GenerateVerificationCode()
        {
            var random = new Random();
            var verificationCode = random.Next(1000, 9999).ToString();
            return verificationCode;
        }

        [Authorize]
        [HttpGet("currentuser")] //get : api/accounts/currentuser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
       {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsyn(user, userManager)
            });

        }

        [HttpGet("checkEmail")]
        public async Task<ActionResult<bool>> checkEmailExist(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;//true





        }
        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok("Logged out successfully.");
        }
    }

}
