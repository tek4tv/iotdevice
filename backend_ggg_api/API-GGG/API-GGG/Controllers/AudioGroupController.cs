using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_GGG.Models;

namespace API_GGG.Controllers
{
    [RoutePrefix("api/ggg/audiogroup")]
    public class AudioGroupController : ApiController
    {
        DatabaseContext databaseContext = new DatabaseContext();
        [Route("info")]
        public HttpResponseMessage GetInfo()
        {
            try
            {
                var output = new AudioGroup();
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
                var items = databaseContext.AudioGroups;
                var output = from item in items
                             select new
                             {
                                 item.ID,
                                 item.Name,
                                 item.FileNumber,
                                 item.DataNumber,
                                 item.Creator,
                                 item.CreatedAt


                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("{id}")]
        public HttpResponseMessage GetByID(int id)
        {
            try
            {
                var items = databaseContext.AudioGroups;
                var output = from item in items where item.ID == id
                             select new
                             {
                                 item.ID,
                                 item.Name,
                                 item.FileNumber,
                                 item.DataNumber,
                                 item.Creator,
                                 item.CreatedAt,
                                 item.AudioPlaylist


                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [Route("add")]
        public HttpResponseMessage PostAudioGroup(AudioGroup audioGroup)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                }
                databaseContext.AudioGroups.Add(audioGroup);
                databaseContext.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, audioGroup, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
