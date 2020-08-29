using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tek4TV.Devices.Models;

namespace Tek4TV.Devices.Apis
{
    [RoutePrefix("api/DeviceCatagory")]
    public class DeviceCatagoryController : ApiController
    {
        DevicesContext dbContext = new DevicesContext();
        [Route("All")]
        public HttpResponseMessage GetAllCatagories()
        {
            try { 
                          
                var items = dbContext.liveDeviceCategories;
                var output = from item in items
                             select new
                             {
                                 item.ID,
                                 item.Name,
                                 item.Descripton,
                                 item.ParentID,
                                 item.OrderID,
                                 item.IsShow,
                                 item.Icon
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("{id}")]
        public HttpResponseMessage GetCatagory(int Id)
        {
            try
            {

                var items = dbContext.liveDeviceCategories;
                var output = from item in items
                             where item.ID == Id
                             select new
                             {
                                 item.ID,
                                 item.Name,
                                 item.Descripton,
                                 item.ParentID,
                                 item.OrderID,
                                 item.IsShow,
                                 item.Icon
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        public HttpResponseMessage PostCatagory(LiveDeviceCategory liveDeviceCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                }
                dbContext.liveDeviceCategories.Add(liveDeviceCategory);
                dbContext.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Value not insert");
            }                
        }
        [Route("{id}")]
        public HttpResponseMessage PutCatagory(LiveDeviceCategory liveDeviceCategory, int Id)
        {
            try
            {
                var item = dbContext.liveDeviceCategories.FirstOrDefault(m => m.ID == Id);
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                }
                else
                {
                    item.Name = liveDeviceCategory.Name;
                    item.Descripton = liveDeviceCategory.Descripton;
                    item.ParentID = liveDeviceCategory.ParentID;
                    item.OrderID = liveDeviceCategory.OrderID;
                    item.IsShow = liveDeviceCategory.IsShow;
                    item.Icon = liveDeviceCategory.Icon;
                    dbContext.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, item);

                }
            }catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "The update was not successful");
            }
        }
        [Route("{id}")]
        public HttpResponseMessage DeleteCatagory(int Id)
        {
            try
            {
                var item = dbContext.liveDeviceCategories.FirstOrDefault(m => m.ID == Id);
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                }
                else
                {
                    dbContext.liveDeviceCategories.Remove(item);
                    dbContext.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, item);
                }

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Bad request");
            }
        }


    }
}
