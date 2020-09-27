using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tek4TV.Devices.Models;

namespace Tek4TV.Devices.Apis
{
    [RoutePrefix("api/ReceiverTransmitter")]
    public class ReceiverTransmitterController : ApiController
    {
        DevicesContext dbContext = new DevicesContext();
        [Route("info")]
        public HttpResponseMessage GetInfo()
        {
            try
            {
                var output = new ReceiverTransmitter();
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
                var items = dbContext.receiverTransmitter;
                var output = from item in items
                             select new
                             {
                                 item.ID,
                                 item.Name,
                                 item.InputSource
                             };
                return Request.CreateResponse(HttpStatusCode.OK, items, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

    }
}
