using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace BetaFaceApi
{
    public class BetaFaceApi
    {
        private const int MAX_RETRIES = 10;

        private string mApiKey;
        private string mApiSecret;

        public BetaFaceApi(string apiKey, string apiSecret)
        {
            mApiKey = apiKey;
            mApiSecret = apiSecret;
        }

        public async Task<BetaFaceInfo> GetFaceInfoAsync(string url)
        {
            string apiUrl = "http://www.betafaceapi.com/service_json.svc/UploadNewImage_Url";
            var client = new HttpClient();
            var content = new BetaFaceUploadNewImageUrlRequest
            {
                api_key = mApiKey,
                api_secret = mApiSecret,
                detection_flags = "",
                image_url = url
            };
            var response = await client.PostAsJsonAsync(apiUrl, content);
            var responseObj = await response.Content.ReadAsAsync<BetaFaceUploadNewImageUrlResponse>();
            if (responseObj.int_response != 0)
            {
                // Some error occurred, probably limit has been reached.
                Debug.Print(responseObj.string_response);
                return new BetaFaceInfo();
            }

            apiUrl = "http://www.betafaceapi.com/service_json.svc/GetImageInfo";
            var content2 = new BetaFaceGetImageInfoRequest
            {
                api_key = mApiKey,
                api_secret = mApiSecret,
                img_uid = responseObj.img_uid
            };

            BetaFaceInfo retVal;
            int retries = 0;
            do
            {
                response = await client.PostAsJsonAsync(apiUrl, content2);
                retVal = await response.Content.ReadAsAsync<BetaFaceInfo>();
                if (retVal.int_response != 0)
                {
                    // Request in queue
                    await Task.Delay(1000);
                    retries++;
                }
                else
                {
                    return retVal;
                }
            } while (retries < MAX_RETRIES);
            return new BetaFaceInfo();
        }
    }

    public class BetaFaceInfo
    {
        public int int_response { get; set; }
        public List<BetaFaceFace> faces { get; set; }
    }

    public class BetaFaceFace
    {
        public List<BetaFaceTag> tags { get; set; } 
    }

    public class BetaFaceTag
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class BetaFaceGetImageInfoRequest
    {
        public string api_key { get; set; }
        public string api_secret { get; set; }
        public string img_uid { get; set; }
    }

    public class BetaFaceUploadNewImageUrlResponse
    {
        public int? int_response { get; set; }
        public string string_response { get; set; }
        public string img_uid { get; set; }
    }

    public class BetaFaceUploadNewImageUrlRequest
    {
        public string api_key { get; set; }
        public string api_secret { get; set; }
        public string detection_flags { get; set; }
        public string image_url { get; set; }
    }
}
