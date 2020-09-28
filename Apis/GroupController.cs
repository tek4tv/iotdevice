using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tek4TV.Devices.Models;

namespace Tek4TV.Devices.Apis
{
    
    [RoutePrefix("api/group")]
    public class GroupController : ApiController
    {
        DevicesContext dbContext = new DevicesContext();
        [Route("info")]
        public HttpResponseMessage GetInfo()
        {
            try
            {
                var output = new LiveGroup();           
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("all")]
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
                                 item.Icon,
                                 item.InputSource
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
                                 item.Icon,
                                 item.InputSource
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("add")]
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("device/{id}")]
        public HttpResponseMessage GetDeviceByGroup(int Id)
        {
            try
            {
                var output = from d in dbContext.LiveDevices
                             where d.LiveGroups.Any(g => g.ID == Id)
                             select new 
                             {
                                 d.ID,
                                 d.IMEI,
                                 d.LinkStream,
                                 d.Name,
                                 d.Description,
                                 d.LiveCategoryID,
                                 d.ExpDate
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);


            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("{idGroup}/device/{idDevice}")]
        public HttpResponseMessage DeleteDeviceByGroup(int idDevice, int idGroup)
        {
            try
            {             
                var group = dbContext.LiveGroups.Find(idGroup);
                var device = dbContext.LiveDevices.Find(idDevice);
                dbContext.Entry(group).Collection("LiveDevices").Load();
                group.LiveDevices.Remove(device);
                dbContext.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "true");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("{idGroup}/device/{idDevice}")]
        public HttpResponseMessage PostDeviceByGroup(int idDevice, int idGroup)
        {
            try
            {
                LiveDevice liveDevice = new LiveDevice { ID = idDevice };
                dbContext.LiveDevices.Add(liveDevice);
                dbContext.LiveDevices.Attach(liveDevice);

                LiveGroup liveGroup = new LiveGroup { ID = idGroup };
                dbContext.LiveGroups.Add(liveGroup);
                dbContext.LiveGroups.Attach(liveGroup);

                liveDevice.LiveGroups.Add(liveGroup);

                dbContext.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "true");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("playlist/{role}/{id}")]
        public HttpResponseMessage GetPlaylistByGroup(int Id, string role)
        {
            try
            {
                var output = from pl in dbContext.LivePlaylists
                             where pl.LiveGroups.Any(g => g.ID == Id)
                             where pl.role == role
                             select new
                             {
                                 pl.ID,
                                 pl.Name,
                                 pl.StartPlaylist,
                                 pl.EndPlaylist,
                                 pl.Playlist,
                                 pl.IsPublish,
                                 pl.IsDelete
                             };
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("{idGroup}/playlist/{idPlaylist}")]
        public HttpResponseMessage DeletePlaylistByGroup(int idPlaylist, int idGroup)
        {
            try
            {
                var group = dbContext.LiveGroups.Find(idGroup);
                var playlist = dbContext.LivePlaylists.Find(idPlaylist);
                dbContext.Entry(group).Collection("LivePlaylists").Load();
                group.LivePlaylists.Remove(playlist);
                dbContext.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "true");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("{idGroup}/playlist/{idPlaylist}")]
        public HttpResponseMessage PostPlaylistByGroup(int idPlaylist, int idGroup)
        {
            try
            {
                LivePlaylist livePlaylist = new LivePlaylist { ID = idPlaylist };
                dbContext.LivePlaylists.Add(livePlaylist);
                dbContext.LivePlaylists.Attach(livePlaylist);

                LiveGroup liveGroup = new LiveGroup { ID = idGroup };
                dbContext.LiveGroups.Add(liveGroup);
                dbContext.LiveGroups.Attach(liveGroup);
                livePlaylist.LiveGroups.Add(liveGroup);

                dbContext.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "true");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [Route("add/InputSource/{id}")]
        public HttpResponseMessage PutInputSource(LiveGroup liveGroup, int id)
        {
            try
            {
                var item = dbContext.LiveGroups.FirstOrDefault(m => m.ID == id);               
                    item.InputSource = liveGroup.InputSource;                 
                    dbContext.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, item);               
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
