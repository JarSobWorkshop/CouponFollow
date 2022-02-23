using InterviewCouponFollow.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace InterviewCouponFollow
{
    public class APITests
    {

        private RestClient client = new RestClient("https://couponfollow.com/api/extension/trendingOffers");
        private string resource = "https://couponfollow.com/api/extension/trendingOffers";

        [Test]
        public void FirstScenario()
        {
            IRestRequest restRequest = new RestRequest(resource, Method.GET)
                .AddHeader("catc-version", "5.0.0");

            IRestResponse restResponse = client.Execute(restRequest);
            Assert.Multiple(() =>
            {
                Assert.That(restResponse.IsSuccessful, Is.True, "Response finished unsuccesfuly");
                Assert.That(restResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Incorrect response status - actual result: {restResponse.StatusCode} expected result: {HttpStatusCode.OK}");
                Assert.That(JsonConvert.DeserializeObject<List<OfferModel>>(restResponse.Content), Is.Not.Null, "Response content is null");
            });
        }


        [Test]
        public void SecondScenario()
        {
            IRestRequest restRequest = new RestRequest(resource, Method.GET);

            IRestResponse restResponse = client.Execute(restRequest);
            Assert.That(restResponse.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden), $"Incorrect response status - actual result: {restResponse.StatusCode} expected result: {HttpStatusCode.Forbidden}");
        }

        [Test]
        public void ThirdScenario()
        {
            IRestRequest restRequest = new RestRequest(resource, Method.GET)
                .AddHeader("catc-version", "5.0.0");

            IRestResponse restResponse = client.Execute(restRequest);
            List<OfferModel> offers = JsonConvert.DeserializeObject<List<OfferModel>>(restResponse.Content);
            Assert.That(offers.Count, !Is.GreaterThan(20), $"Incorrect number of coupons actual result: {offers} expected result: not higher than 20");
        }

        [Test]
        public void FourthScenario()
        {
            IRestRequest restRequest = new RestRequest(resource, Method.GET)
                .AddHeader("catc-version", "5.0.0");

            IRestResponse restResponse = client.Execute(restRequest);
            List<OfferModel> offers = JsonConvert.DeserializeObject<List<OfferModel>>(restResponse.Content);
            var uniqueDomain = offers.Select(o => o.DomainName).Distinct().Count(); //select objects with unique domain
            Assert.That(uniqueDomain, Is.EqualTo(offers.Count), $"Number of unique domain should be the same as all offert from response actual result: {uniqueDomain} expected result: {offers.Count}"); // compare number of unique objects with number of all objects from list
        }
    }
}
