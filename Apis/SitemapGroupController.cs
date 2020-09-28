using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tek4TV.Devices.Models;

namespace Tek4TV.Devices.Apis
{
    [RoutePrefix("api/stiemapGroup")]
    public class SitemapGroupController : ApiController
    {
        DevicesContext dbContext = new DevicesContext();
        [Route("all")]
        public HttpResponseMessage GetAll()
        {
            try
            {

                var items = dbContext.SiteMapGroups;
                var output = from item in items
                             select new
                             {
                                 item.ID,
                                 item.GroupID,
                                 item.SiteMapID
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("add")]
        public HttpResponseMessage PostElement(SiteMapGroup siteMapGroup)
        {
            try
            {              
                dbContext.SiteMapGroups.Add(siteMapGroup);
                dbContext.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
