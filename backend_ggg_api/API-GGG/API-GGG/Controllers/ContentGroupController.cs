using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_GGG.Models;
using Newtonsoft.Json;

namespace API_GGG.Controllers
{
    [RoutePrefix("api/ggg/contentgroup")]
    public class ContentGroupController : ApiController
    {
        DatabaseContext databaseContext = new DatabaseContext();
        [Route("info")]
        public HttpResponseMessage GetInfo()
        {
            try
            {
                var output = new ContentGroup();
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
                var items = databaseContext.ContentGroups;
                var output = from item in items
                             select new
                             {
                                 item.ID,
                                 item.Name    ,                           
                                 item.FileNumber,
                                 item.DataNumber,
                                 item.Creator,
                                 item.Playlists,                                
                                 
                                 
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("add")]
        public HttpResponseMessage PostContentGroup(ContentGroup contentGroup)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                }
                databaseContext.ContentGroups.Add(contentGroup);
                databaseContext.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, contentGroup, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
