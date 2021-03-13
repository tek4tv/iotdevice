using API_GGG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_GGG.Controllers
{
    [RoutePrefix("api/ggg/totalscenario")]
    public class TotalScenarioController : ApiController
    {
        DatabaseContext databaseContext = new DatabaseContext();
        [Route("info")]
        public HttpResponseMessage GetInfo()
        {
            try
            {
                var output = new TotalScenario();
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
                var items = databaseContext.TotalScenarios;
                var output = from item in items
                             select new
                             {
                                 item.ID,
                                 item.Name,
                                 
                                 item.Creator,
                                 item.CreatedAt,


                                 item.PlaylistTimeID
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("add")]
        public HttpResponseMessage PostTotalScenarios(TotalScenario totalScenario)
        {
            try
            {
                databaseContext.TotalScenarios.Add(totalScenario);
                databaseContext.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, totalScenario, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
