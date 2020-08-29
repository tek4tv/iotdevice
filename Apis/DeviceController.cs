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
    [RoutePrefix("api/Device")]
    public class DeviceController : ApiController
    {
        DevicesContext dbContext = new DevicesContext();
        [Route("All")]
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
        [Route("{id}")]
        public HttpResponseMessage GetDevice(int Id)
        {
            try
            {
                var items = dbContext.LiveDevices;
                var output = from item in items
                             where item.ID == Id
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
        [Route("{idDevice}/Group/{idGroup}")]
        public HttpResponseMessage PostToGroup(int idDevice, int idGroup)
        {          
            try
            {
                LiveDevice liveDevice = new LiveDevice { ID = idDevice };
                dbContext.LiveDevices.Add(liveDevice);
                dbContext.LiveDevices.Attach(liveDevice);

                LiveGroup liveGroup = new LiveGroup { ID = idGroup };
                dbContext.LiveGroups.Add(liveGroup);
                dbContext.LiveGroups.Attach(liveGroup);

                liveDevice.LiveGroups.Add(liveGroup);

                // remove duplicate
               /* LiveGroup removeduplicate = dbContext.LiveGroups.Find(idGroup);
                liveDevice.LiveGroups.Remove(removeduplicate);*/
                dbContext.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "true");
            }catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("{idDevice}/Group/{idGroup}")]
        public HttpResponseMessage PutToGroup(int idDevice, int idGroup)
        {
            try
            {
                LiveDevice liveDevice = dbContext.LiveDevices.Find(idDevice);
                LiveGroup idGroupChange = dbContext.LiveGroups.Find(idGroup);
                liveDevice.LiveGroups.Add(idGroupChange);
            
                // remove duplicate
                //LiveGroup removeduplicate = dbContext.LiveGroups.Find(idGroup);
                liveDevice.LiveGroups.Remove(idGroupChange);
                dbContext.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "true");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
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
        [Route("{idDevice}/Group/{idGroup}")]
        public HttpResponseMessage DeleteToGroup(int idDevice, int idGroup)
        {
            try
            {
                var divece = dbContext.LiveDevices.Find(idDevice);
                var group = dbContext.LiveGroups.Find(idGroup);
                dbContext.Entry(divece).Collection("LiveGroups").Load();
                divece.LiveGroups.Remove(group);
                dbContext.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "true");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

    }
}
