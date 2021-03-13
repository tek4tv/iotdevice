using API_GGG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_GGG.Controllers
{
    [RoutePrefix("api/ggg/playlisttime")]
    public class PlaylistTimeController : ApiController
    {
        DatabaseContext databaseContext = new DatabaseContext();
        [Route("info")]
        public HttpResponseMessage GetInfo()
        {
            try
            {
                var output = new PlaylistTime();
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
                var items = databaseContext.PlaylistTimes;
                var output = from item in items
                             select new
                             {
                                 item.ID,
                                 item.Name,
                                 item.StartPlaylist,
                                 item.EndPlaylist,
                                 item.Creator,
                                 item.CreatedAt,
                                 
                                 item.ContenGroupID,
                                 item.AudioGroupID
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
                var items = databaseContext.PlaylistTimes;
                var output = from item in items where item.ID == id
                             select new
                             {
                                 item.ID,
                                 item.Name,
                                 item.StartPlaylist,
                                 item.EndPlaylist,
                                 item.Creator,
                                 item.CreatedAt,
                                
                                 item.ContenGroupID,
                                 item.AudioGroupID
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("add")]
        public HttpResponseMessage PostPlaylistTime(PlaylistTime playlistTime)
        {
            try
            {             
                databaseContext.PlaylistTimes.Add(playlistTime);
                databaseContext.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, playlistTime, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
