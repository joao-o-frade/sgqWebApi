﻿using ProjectWebApi.DAOs;
using ProjectWebApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ProjectWebApi.Controllers
{
    public class DevManufController : ApiController
    {
        [HttpGet]
        [Route("devManuf/all")]
        [ResponseType(typeof(IList<IdName>))]
        public HttpResponseMessage all(HttpRequestMessage request)
        {
            var dao = new DevManufDAO();
            var list = dao.all();
            dao.Dispose();
            return request.CreateResponse(HttpStatusCode.OK, list);
        }

        [HttpGet]
        [Route("devManuf/allOfQueue")]
        [ResponseType(typeof(IList<IdName>))]
        public HttpResponseMessage allOfQueue(HttpRequestMessage request)
        {
            var dao = new DevManufDAO();
            var list = dao.allOfQueue();
            dao.Dispose();
            return request.CreateResponse(HttpStatusCode.OK, list);
        }

    }
}