﻿using Emgu.CV.Ocl;
using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.Repository
{
    public class FaceComparisonResultRepository : IFaceComparisonResultRepository
    {
        private readonly StoreContext dbContext;
        public FaceComparisonResultRepository(StoreContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveComparisonResult(FaceComparison result)
        {
            var entity = new verficationFaccess
            {
                MatchStatus = result.MatchStatus,
                accuracy = result.accuracy,
                userId = result.userId,
            };


            dbContext.verficationFaccess.Add(entity);

            await dbContext.SaveChangesAsync();

        }
        public async Task<bool> CheckUserExistsInVerificationFaces(string userId)
        {
            // تحقق من وجود المستخدم في قاعدة البيانات
            var user = await dbContext.verficationFaccess.FirstOrDefaultAsync(u => u.userId == userId);

            // إذا وجد المستخدم، يعود true
            return user != null;
        }
    }
}

