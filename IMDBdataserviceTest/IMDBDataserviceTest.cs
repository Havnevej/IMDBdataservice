﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace IMDBdataserviceTest
{
    public class IMDBdataserviceTest
    {
        private const string UserApi = "http://localhost:5001/api/users/";
        private const string TitleApi = "http://localhost:5001/api/titles";
        private const string PersonApi = "http://localhost:5001/api/person";


        /* /api/categories */

        [Fact]
        public void ApiUser_register()
        {
            var newuser = new
            {
                userid = "register_name6767",
                username = "testing_register6767",
                password = "amazing1234"
            };
            var (users, statusCode) = PostData(UserApi + "register", newuser);

            Assert.Equal(HttpStatusCode.Created, statusCode);
            //Assert.Equal(HttpStatusCode.OK, statusCode);
            //get test 

        }

        [Fact]
        public void ApiUser_getuser()
        {
            var (user, statusCode) = GetObject($"{UserApi}register_name6767");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("testing_register6767", user["name"]);
        }


        [Fact]
        public void ApiUser_DeleteUser()
        {
            var (data, statusCode) = DeleteData($"{UserApi}testing_register6767");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("Deleted User!", data["message"]);

        }


        (JObject, HttpStatusCode) PostData(string url, object content)
        {
            var client = new HttpClient();
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) GetObject(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) DeleteData(string url)
        {
            var client = new HttpClient();
            var response = client.DeleteAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        //Person Test *------------------------------------------------------------------------*

        [Fact]
        public void ApiPerson_AddPerson()
        {
            var newPerson = new
            {
                userid = "register_name6767",
                username = "testing_register6767",
                password = "amazing1234"
            };
            var (users, statusCode) = PostData(UserApi + "register", newuser);

            Assert.Equal(HttpStatusCode.Created, statusCode);
            //Assert.Equal(HttpStatusCode.OK, statusCode);
            //get test 

        }

        [Fact]
        public void ApiUser_getuser()
        {
            var (user, statusCode) = GetObject($"{UserApi}register_name6767");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("testing_register6767", user["name"]);
        }


        [Fact]
        public void ApiUser_DeleteUser()
        {
            var (data, statusCode) = DeleteData($"{UserApi}testing_register6767");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("Deleted User!", data["message"]);

        }


        (JObject, HttpStatusCode) PostData(string url, object content)
        {
            var client = new HttpClient();
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) GetObject(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) DeleteData(string url)
        {
            var client = new HttpClient();
            var response = client.DeleteAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }






    }
}
