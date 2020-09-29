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
        [Route("sitemapid/{id}")]
        public HttpResponseMessage GetSitemapId(int id)
        {
            try
            {

                var items = dbContext.SiteMapGroups;
                var output = from item in items
                             where item.SiteMapID == id
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
        public HttpResponseMessage PostAdd(dynamic obj)
        {
            try
            {
                List<int> ListLiveGroupID = obj.ListLiveGroupID.ToObject<List<int>>();
                int siteMapID = (int)obj.SiteMapID;
                foreach (var id in ListLiveGroupID)
                {
                    var siteMapGroup = new SiteMapGroup();
                    siteMapGroup.GroupID = id;
                    siteMapGroup.SiteMapID = siteMapID;
                    dbContext.SiteMapGroups.Add(siteMapGroup);
                }
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
