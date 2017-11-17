﻿using ProjectWebApi.Daos;
using ProjectWebApi.Models.Profile;
using ProjectWebApi.Models.User;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebApplication1.Controllers
{
    public class AuthController : ApiController
    {
        [HttpGet]
        [Route("auth/users")]
        public IList<User> getUsers()
        {
            var userDao = new UserDao();
            var user = userDao.getUsers();
            userDao.Dispose();
            return user;
        }

        [HttpPost]
        [Route("auth/userByCpf")]
        [ResponseType(typeof(IList<User>))]
        public HttpResponseMessage getUserByCpf(HttpRequestMessage request, User user) {
            var userDao = new UserDao();
            var result = userDao.getUserByCpf(user.login, user.cpf);
            userDao.Dispose();
            return request.CreateResponse(result != null ? HttpStatusCode.OK : HttpStatusCode.NotFound, result);
        }

        [HttpGet]
        [Route("auth/profilesByUser/{userId}")]
        [ResponseType(typeof(IList<Profile>))]
        public HttpResponseMessage getProfilesByUser(HttpRequestMessage request, int userId)
        {
            var userDao = new UserDao();
            var result = userDao.getProfilesByUser(userId);
            userDao.Dispose();
            return request.CreateResponse(result != null ? HttpStatusCode.OK : HttpStatusCode.NotFound, result);
        }

        [HttpGet]
        [Route("auth/userByPassword/{login}/{password}")]
        public User GetUserByPassword(string login, string password)
        {
            var userDao = new UserDao();
            var user = userDao.getUserByPassword(login, password);
            userDao.Dispose();
            return user;
        }
    }
}