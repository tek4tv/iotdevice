using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tek4TV.Devices.Models;

namespace Tek4TV.Devices.Apis
{
    [RoutePrefix("api/Group")]
    public class GroupController : ApiController
    {
        DevicesContext dbContext = new DevicesContext();
        [Route("All")]
        public HttpResponseMessage GetAllGroups()
        {
            try
            {
                var items = dbContext.LiveGroups;
                var output = from item in items
                             select new
                             {
                                 item.ID,
                                 item.Name,
                                 item.Description,
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
        public HttpResponseMessage GetGroup(int Id)
        {
            try
            {
                var items = dbContext.LiveGroups;
                var output = from item in items
                             where item.ID == Id
                             select new
                             {
                                 item.ID,
                                 item.Name,
                                 item.Description,
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
        public HttpResponseMessage PostGroup(LiveGroup liveGroup)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not Value");
                }
                dbContext.LiveGroups.Add(liveGroup);
                dbContext.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "The add was not successful");
            }
        }
        [Route("{id}")]
        public HttpResponseMessage PutGroup(LiveGroup liveGroup, int Id)
        {
            try
            {
                var item = dbContext.LiveGroups.FirstOrDefault(m => m.ID == Id);
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                }
                else
                {
                    item.Name = liveGroup.Name;
                    item.Description = liveGroup.Description;
                    item.ParentID = liveGroup.ParentID;
                    item.OrderID = liveGroup.OrderID;
                    item.IsShow = liveGroup.IsShow;
                    item.Icon = liveGroup.Icon;
                    dbContext.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, item);
                }

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "The update was not successful");
            }

        }
        [Route("{id}")]
        public HttpResponseMessage DeleteGroup(int Id)
        {
            try
            {
                var item = dbContext.LiveGroups.FirstOrDefault(m => m.ID == Id);
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                }
                else
                {
                    dbContext.LiveGroups.Remove(item);
                    dbContext.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, item);
                }

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "The delete was not successful");
            }
        }

    }
}
