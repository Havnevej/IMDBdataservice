using System;
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
        private const string PersonApi = "http://localhost:5000/api/person/";


        /* /api/categories */

        [Fact]
        public void ApiUser_register()
        {
            var newuser = new
            {
                username = "testing_register6767",
                password = "amazing1234"
            };
            var (users, statusCode) = PostData(UserApi + "register", newuser);

            Assert.Equal(HttpStatusCode.Created, statusCode);
           

        }

        [Fact]
        public void ApiUser_getuser()
        {
            var (user, statusCode) = GetObject($"{UserApi+ "get"}/testing_register6767");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("testing_register6767", user["username"]);
        }

        [Fact]
        public void ApiUser_login()
        {
            var user_info = new
            {
                username = "testing_register6767",
                password = "amazing1234"
            };
            var (data, statusCode) = PostData($"{UserApi + "login"}", user_info);

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.NotNull(data["token"]);
        }

        [Fact]
        public void ApiUser_DeleteUser()
        {
            var user_info = new
            {
                username = "testing_register6767",
                password = "amazing1234"
            };

            var (data, statusCode) = PostData($"{UserApi + "login"}", user_info);
            var token = data["token"];

            using (var client = new HttpClient())
            {
                var url = $"{UserApi + "delete"}";
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                
                var requestContent = new StringContent(
                JsonConvert.SerializeObject(new {username = user_info.username}),
                Encoding.UTF8,
                "application/json");
                var response = client.PostAsync(url, requestContent).Result;
                var return_data = response.Content.ReadAsStringAsync().Result;
                var parsed_data = (JObject)JsonConvert.DeserializeObject(return_data);
                Assert.Equal("Deleted User!", parsed_data["message"]);
            }
        }

        //Person Test *------------------------------------------------------------------------*

        [Fact]
        public void ApiPerson_AddPerson()
        {
            var newPerson = new
            {
                personId = "nm00000019999",
                personName = "MickeyMouse",
                birthyear = "1986",
                deathyear = "1987"
            };
            var (person, statusCode) = PostData(PersonApi + "add", newPerson);

            Assert.Equal(HttpStatusCode.Created, statusCode);
            //Assert.Equal(HttpStatusCode.OK, statusCode);

        }
        
        [Fact]
        public void ApiPerson_getPerson()
        {
            var (person, statusCode) = GetObject($"{PersonApi}nm00000019999");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("MickeyMouse", person["personName"]);
        }
        

        [Fact]
        public void ApiUser_DeletePerson()
        {
            var (data, statusCode) = DeleteData($"{PersonApi}nm00000019999");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("Deleted User!", data["message"]);

        }

        [Fact]
        public void ApiPerson_UpdatePerson()
        {
            var (data, statusCode) = DeleteData($"{UserApi}nm00000019999");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("Deleted User!", data["message"]);

        }

        //Helpers *------------------------------------------------------------------------*

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
