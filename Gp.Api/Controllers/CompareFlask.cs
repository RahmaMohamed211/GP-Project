using AutoMapper;
using GP.core.Entities.identity;
using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using OpenCvSharp;
using System.Security.Claims;
using Twilio.TwiML.Messaging;

namespace Gp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompareFlask : ApiBaseController
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IFaceComparisonResultRepository faceComparisonResult;
        private readonly UserManager<AppUser> userManager;
        private readonly FaceComparisonService faceComparison;

        public CompareFlask(IConfiguration configuration, IMapper mapper, IFaceComparisonResultRepository faceComparisonResult, UserManager<AppUser> userManager, FaceComparisonService faceComparison)
        {
            this.configuration = configuration;
            this.mapper = mapper;
            this.faceComparisonResult = faceComparisonResult;
            this.userManager = userManager;
            this.faceComparison = faceComparison;
        }

        [Authorize]
        [HttpPost("compare-faces")]
        public async Task<IActionResult> CompareFaces(IFormFile image1, bool useCamera = true)
        {
            // Convert image1 to byte array
            byte[] bytes1;
            if (image1 == null)
            {

                return BadRequest(new { message = "Both images are required" });
            }
            using (var ms1 = new MemoryStream())
            {
                await image1.CopyToAsync(ms1);
                bytes1 = ms1.ToArray();
            }
            var email = User.FindFirstValue(ClaimTypes.Email);


            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                // التحقق من وجود المستخدم في قاعدة البيانات
                var userExistsInVerificationFaces = await faceComparisonResult.CheckUserExistsInVerificationFaces(existingUser.Id);

                if (userExistsInVerificationFaces)
                {
                    return BadRequest(new { message = "User already exists" });


                }

                byte[] bytes2 = null;
                if (!useCamera)
                {
                    // Capture image2 from the camera and convert it to byte array
                    bytes2 = await faceComparison.CaptureFrameFromCameraAsync();
                }

                // Call the CompareFaces method passing image1 and image2
                var result = await faceComparison.CompareFaces(bytes1, bytes2, useCamera);
                // إضافة معرف المستخدم إلى النتيجة
                result.userId = existingUser.Id;

                // حفظ النتيجة
                await faceComparisonResult.SaveComparisonResult(result);
                return Ok(result);
            }


            else
            {
                return BadRequest(new { message = "User not found" });
            }
        }
    }

                //public async Task<IActionResult> CompareFaces(IFormFile image1, bool useCamera = true)
                //{
                //    if (image1 == null)
                //    {
                //        return BadRequest("Image1 is required");
                //    }

                //    var email = User.FindFirstValue(ClaimTypes.Email);
                //    var existingUser = await userManager.FindByEmailAsync(email);

                //    if (existingUser != null)
                //    {
                //        byte[] bytes1;
                //        byte[] bytes2 = null;

                //        using (var ms1 = new MemoryStream())
                //        {
                //            await image1.CopyToAsync(ms1);
                //            bytes1 = ms1.ToArray();
                //        }

                //        if (useCamera)
                //        {
                //            // Call the face comparison service with camera image
                //            var result = await faceComparison.CompareFaces(bytes1, useCamera);
                //            // Add user ID to the result
                //            result.userId = existingUser.Id;
                //            // Save the comparison result
                //            await faceComparisonResult.SaveComparisonResult(result);
                //            return Ok(result);
                //        }
                //        else
                //        {
                //            // Call the face comparison service with both images
                //            // Handle the second image capture or retrieval here
                //            // For example, you can use another IFormFile parameter for image2
                //            // Convert image2 to byte array as you did for image1 and then call the service
                //            // var result = await faceComparisonService.CompareFaces(bytes1, bytes2);
                //            // Add user ID to the result
                //            // result.userId = existingUser.Id;
                //            // Save the comparison result
                //            // await faceComparisonResult.SaveComparisonResult(result);
                //            // return Ok(result);
                //            return BadRequest("Image2 is required when not using the camera");
                //        }
                //    }
                //    else
                //    {
                //        return BadRequest("User not found");
                //    }
                //}
                //[Authorize]
                //[HttpPost("compare-faces")]
                //public async Task<IActionResult> CompareFaces(IFormFile image1, IFormFile image2)
                //{
                //    if (image1 == null || image2 == null)
                //    {

                //        return BadRequest(new { message = "Both images are required" });
                //    }
                //    var email = User.FindFirstValue(ClaimTypes.Email);


                //    var existingUser = await userManager.FindByEmailAsync(email);

                //    if (existingUser != null)
                //    {
                //        // التحقق من وجود المستخدم في قاعدة البيانات
                //        var userExistsInVerificationFaces = await faceComparisonResult.CheckUserExistsInVerificationFaces(existingUser.Id);

                //        if (userExistsInVerificationFaces)
                //        {
                //            return BadRequest(new { message = "User already exists" });


                //        }

                //        using (var ms1 = new MemoryStream())
                //        using (var ms2 = new MemoryStream())
                //        {
                //            await image1.CopyToAsync(ms1);
                //            await image2.CopyToAsync(ms2);
                //            byte[] bytes1 = ms1.ToArray();
                //            byte[] bytes2 = ms2.ToArray();

                //            // إجراء المقارنة
                //            var result = await faceComparison.CompareFaces(bytes1, bytes2);

                //            // إضافة معرف المستخدم إلى النتيجة
                //            result.userId = existingUser.Id;

                //            // حفظ النتيجة
                //            await faceComparisonResult.SaveComparisonResult(result);

                //            return Ok(new { result });

                //        }

                //    }
                //    else
                //    {
                //        return BadRequest(new { message="User not found" });
                //    }
                //}



            }

