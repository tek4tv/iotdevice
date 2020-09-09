using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tek4TV.Devices.Models;

namespace Tek4TV.Devices.Apis
{
    [RoutePrefix("api/playlist")]
    public class PlaylistController : ApiController
    {
        DevicesContext dbContext = new DevicesContext();
        [Route("info")]
        public HttpResponseMessage GetInfo()
        {
            try
            {
                var output = new LivePlaylist();
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("all")]
        public HttpResponseMessage GetAllPlaylists()
        {
            try
            {
                var items = dbContext.LivePlaylists;
                var output = from item in items
                             select new
                             {
                                 item.ID,
                                 item.Name,
                                 item.StartPlaylist,
                                 item.EndPlaylist,
                                 item.Playlist,
                                 item.IsPublish,
                                 item.IsDelete
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("{id}")]
        public HttpResponseMessage GetPlaylist(int Id)
        {
            try
            {
                var items = dbContext.LivePlaylists;
                var output = from item in items
                             where item.ID == Id
                             select new
                             {
                                 item.ID,
                                 item.Name,
                                 item.StartPlaylist,
                                 item.EndPlaylist,
                                 item.Playlist,
                                 item.IsPublish,
                                 item.IsDelete
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("add")]
        public HttpResponseMessage PostPlaylist(LivePlaylist livePlaylist)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not Value");
                }
                dbContext.LivePlaylists.Add(livePlaylist);
                dbContext.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "The add was not successful");
            }
        }
        [Route("{id}")]
        public HttpResponseMessage PutPlaylist(LivePlaylist livePlaylist, int Id)
        {
            try
            {
                var item = dbContext.LivePlaylists.FirstOrDefault(m => m.ID == Id);
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                }
                else
                {
                    item.Name = livePlaylist.Name;
                    item.StartPlaylist = livePlaylist.StartPlaylist;
                    item.EndPlaylist = livePlaylist.EndPlaylist;
                    item.Playlist = livePlaylist.Playlist;
                    item.IsPublish = livePlaylist.IsPublish;
                    item.IsDelete = livePlaylist.IsDelete;
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
        public HttpResponseMessage DeletePlaylist(int Id)
        {
            try
            {
                var item = dbContext.LivePlaylists.FirstOrDefault(m => m.ID == Id);
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not found");
                }
                else
                {
                    dbContext.LivePlaylists.Remove(item);
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
