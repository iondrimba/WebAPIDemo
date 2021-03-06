﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    public class UserController : ApiController
    {
        private static IList<IAppUser> _dataSource = new List<IAppUser>();

        public static IList<IAppUser> DataSource
        {
            get { return UserController._dataSource; }
            set { UserController._dataSource = value; }
        }

        public UserController()
        {
            _dataSource = new List<IAppUser>();
            _dataSource.Add(new AppUserBase { Name = "User1", Email = "user1@demo.com", Id = 1 });
            _dataSource.Add(new AppUserBase { Name = "User2", Email = "user2@demo.com", Id = 2 });
            _dataSource.Add(new AppUserBase { Name = "User3", Email = "user3@demo.com", Id = 3 });

            UserController.DataSource = _dataSource;
        }

        // POST: api/User
        public async Task<IHttpActionResult> Post(IAppUser user)
        {
            await Task.Delay(1000);
            string location = "api/User/1";
            return Created(location, user);
        }

        // PUT: api/User/5
        public async Task<IHttpActionResult> Put(IAppUser user)
        {
            await Task.Delay(1000);

            return Ok(user);
        }

        // DELETE: api/User/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            await Task.Delay(1000);

            IAppUser user = _dataSource.Where(p => p.Id == id).FirstOrDefault();
            _dataSource.Remove(user);

            return Ok(_dataSource);
        }

        public async Task<IHttpActionResult> Get()
        {
            await Task.Delay(1000);

            return Ok(_dataSource);
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            await Task.Delay(1000);

            IAppUser user = _dataSource.Where(p => p.Id == id).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        public async Task<IHttpActionResult> GetBadRequest(int id)
        {
            await Task.Delay(1000);

            IAppUser user = _dataSource.Where(p => p.Id == id).FirstOrDefault();

            if (user == null)
            {
                string message = string.Format("Something went wrong on the request for user {0}", id);
                return BadRequest(message);
            }

            return Ok(user);
        }

        public async Task<HttpResponseMessage> GetPicture(int id)
        {
            await Task.Delay(1000);

            IAppUser user = _dataSource.Where(p => p.Id == id).FirstOrDefault();

            string path = "~/Upload/";
            string file = string.Format("user{0}.gif", user.Id);

            //string picturePath = HttpContext.Current.Server.MapPath(path);

            var baseDir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            var pa =baseDir.Replace(@"bin", "Upload");
            var testFile = Path.Combine(pa, file);

            var info = System.IO.File.GetAttributes(testFile);
            var result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StreamContent(new FileStream(testFile, FileMode.Open, FileAccess.Read));
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            result.Content.Headers.Add("x-filename", testFile);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = file;

            return result;
        }
    }
}