using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tek4TV.Devices.Models;

namespace Tek4TV.Devices.Apis
{
    [RoutePrefix("api/inputsource")]
    public class InputSourceController : ApiController
    {
        DevicesContext dbContext = new DevicesContext();
        [Route("info")]
        public HttpResponseMessage GetInfo()
        {
            try
            {
                var output = new LiveInputSource();
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("all")]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var item = dbContext.LiveInputSources.Select(i => new {
                    i.ID,
                    i.Name,
                    i.Type,
                    i.Param,
                    i.IsSchedule,
                    i.StartInput,
                    i.EndInput
                });
              
                return Request.CreateResponse(HttpStatusCode.OK, item, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("{id}")]
        public HttpResponseMessage GetItemById(int id)
        {
            try
            {
                var output = dbContext.LiveInputSources.Where(m => m.ID == id).FirstOrDefault();               
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("add")]
        public HttpResponseMessage PostElement(LiveInputSource liveInputSource)
        {
            try
            {
                dbContext.LiveInputSources.Add(liveInputSource);
                dbContext.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,"Added");
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("{id}")]
        public HttpResponseMessage PutElement(LiveInputSource liveInputSource, int id)
        {
            try
            {
                var item = dbContext.LiveInputSources.FirstOrDefault(m => m.ID == id);
                item.Name = liveInputSource.Name;
                item.Type = liveInputSource.Type;
                item.Param = liveInputSource.Param;
                item.IsSchedule = liveInputSource.IsSchedule;
                item.StartInput = liveInputSource.StartInput;
                item.EndInput = liveInputSource.EndInput;
                dbContext.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "updated");
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
                      
        }
        [Route("{id}")]
        public HttpResponseMessage DeleteElement (int id)
        {
            try
            {
                var item = dbContext.LiveInputSources.FirstOrDefault(m => m.ID == id);
                dbContext.LiveInputSources.Remove(item);
                dbContext.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "deleted");
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
