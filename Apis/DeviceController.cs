using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tek4TV.Devices.Migrations;
using Tek4TV.Devices.Models;

namespace Tek4TV.Devices.Apis
{
    [RoutePrefix("api/device")]
    public class DeviceController : ApiController
    {
        DevicesContext dbContext = new DevicesContext();
        public bool checkIMEI(int id)
        {
            var items = dbContext.LiveDevices.Where(x => x.ID == id);
            var outputs = from item in items
                         select new
                         {
                             item.IMEI
                         };
            foreach (var output in outputs )
            {
                if (output.IMEI == null)
                {
                    return false;
                }
            }
            return true;
        }
        public bool checkExpDate(int id)
        {
            DateTime date =  DateTime.Now;

            var items = dbContext.LiveDevices.Where(x => x.ID == id);
            var outputs = from item in items
                          select new
                          {
                              item.ExpDate
                          };
            foreach (var output in outputs)
            {
                if (output.ExpDate < date)
                {
                    return false;
                }
            }
            return true;
        }
        [Route("info")]
        public HttpResponseMessage GetInfo()
        {
            try
            {
                var output = new LiveDevice();
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("all")]
        public HttpResponseMessage GetAllDevices()
        {
            try
            {
                var items = dbContext.LiveDevices;
                var output = from item in items
                             select new
                             {
                                 item.ID,
                                 item.IMEI,
                                 item.LinkStream,
                                 item.Name,
                                 item.Description,
                                 item.LiveCategoryID,
                                 item.ExpDate
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("search/{content}")]
        public HttpResponseMessage GetByContent(string content)
        {
            try
            {
                var items = dbContext.LiveDevices;
                var output = from item in items
                             select new
                             {
                                 item.ID,
                                 item.IMEI,
                                 item.LinkStream,
                                 item.Name,
                                 item.Description,
                                 item.LiveCategoryID,
                                 item.ExpDate
                             };
                if (!string.IsNullOrEmpty(content))
                {
                    output = output.Where(x => x.IMEI.Contains(content) || x.Name.Contains(content));
                }                   
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("category/{id}")]
        public HttpResponseMessage GetDevice(int Id)
        {
            try
            {
                var items = dbContext.LiveDevices;
                var output = from item in items
                             where item.LiveCategoryID == Id
                             select new
                             {
                                 item.ID,
                                 item.IMEI,
                                 item.LinkStream,
                                 item.Name,
                                 item.Description,
                                 item.LiveCategoryID,
                                 item.ExpDate
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("add")]
        public HttpResponseMessage PostDevice(LiveDevice liveDevice)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Bad request");
                }
                dbContext.LiveDevices.Add(liveDevice);
                dbContext.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Not Insert");
            }
        }
             
        [Route("{id}")]
        public HttpResponseMessage PutDevice(LiveDevice liveDevice, int Id)
        {
            try
            {
                var item = dbContext.LiveDevices.FirstOrDefault(m => m.ID == Id);
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Null");
                }
                else
                {
                    item.IMEI = liveDevice.IMEI;
                    item.LinkStream = liveDevice.LinkStream;
                    item.Name = liveDevice.Name;
                    item.Description = liveDevice.Description;
                    item.LiveCategoryID = liveDevice.LiveCategoryID;
                    item.ExpDate = liveDevice.ExpDate;
                    dbContext.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, item);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Not update");
            }
        }
       
        [Route("{id}")]
        public HttpResponseMessage DeleteDevice(int Id)
        {
            try
            {
                var item = dbContext.LiveDevices.FirstOrDefault(m => m.ID == Id);
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Item not find");
                }
                else
                {
                    dbContext.LiveDevices.Remove(item);
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
