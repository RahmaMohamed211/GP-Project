using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace GP.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly StoreContext dbContext;

        public RequestRepository(StoreContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> DeleteRequestAsync(int RequestId)
        {
            var requestToDelete = await dbContext.Requests.FindAsync(RequestId);

            if (requestToDelete == null)
            {
                return false; // Request with the given ID doesn't exist
            }

            dbContext.Requests.Remove(requestToDelete);
            await dbContext.SaveChangesAsync();

            return true; // Request deleted successfully
        }


        public async Task<Request> GetRequestAsync(int requestId)
        {
            // استعلام للحصول على بيانات الطلب مع تفاصيل الشحنة والرحلة
            var request = await dbContext.Requests
                .Include(r => r.Shipment)
                .Include(r => r.Trip)
                .FirstOrDefaultAsync(r => r.RequestId == requestId);

            // تحديد خيارات التسلسل
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            // تسلسل كائن الطلب إلى سلسلة JSON باستخدام الخيارات المحددة
            var jsonString = JsonSerializer.Serialize(request, options);

            // فك تسلسل سلسلة JSON إلى كائن Request
            var deserializedRequest = JsonSerializer.Deserialize<Request>(jsonString, options);

            return deserializedRequest;
        }




        public async Task<Request> UpdateRequestAsync(Request request)
        {
            // التحقق مما إذا كانت القيمة TripId موجودة في جدول Trips
            var existingTrip = await dbContext.Trips.FindAsync(request.TripId);
           var exisitingShipment= await dbContext.shipments.FindAsync(request.ShipmentId);

           
            if (existingTrip == null)
            {
                // يمكنك تخصيص رسالة الخطأ حسب احتياجاتك
                throw new Exception("TripId does not exist in Trips table");
            }
            if (exisitingShipment == null)
            {
                // يمكنك تخصيص رسالة الخطأ حسب احتياجاتك
                throw new Exception("shipmentId does not exist in Trips table");
            }

            // القيمة موجودة، يتم إجراء الإدخال في جدول Requests
            var existingRequest = await dbContext.Requests.FindAsync(request.RequestId);

            // التحقق مما إذا كان سجل الطلب موجودًا بالفعل
            if (existingRequest == null)
            {
                // إذا كان السجل غير موجود، يتم إنشاء سجل جديد
                dbContext.Requests.Add(request);

                // حفظ التغييرات في قاعدة البيانات
                await dbContext.SaveChangesAsync();

                // إرجاع السجل الجديد الذي تمت إضافته
                return request;
            }

            // تحديث خصائص سجل الطلب الحالي بالقيم الجديدة المستلمة
            existingRequest.ShipmentId = request.ShipmentId;
            existingRequest.TripId = request.TripId;
            existingRequest.UserId = request.UserId;

            // حفظ التغييرات في قاعدة البيانات
            await dbContext.SaveChangesAsync();

            // إرجاع سجل الطلب المحدث
            return existingRequest;
        }
    }
}
