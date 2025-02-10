using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Networking;

namespace Skeletom.Essentials.Utils {

    public static class HttpUtils {
        /// <summary>
        /// Makes a GET request to the given URL, executing the corresponding callback on completion.
        /// </summary>
        /// <param name="url">The URL to call.</param>
        /// <param name="onError">The callback executed on an unsuccessful request.</param>
        /// <param name="onSuccess">The callback executed on a successful request.</param>
        /// <param name="bearer">Optional bearer token for authentication.</param>
        /// <returns></returns>
        public static IEnumerator GetRequest(string url, Action<HttpError> onError, Action<string> onSuccess, string bearer) {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            if (bearer != null) {
                webRequest.SetRequestHeader("Authorization", string.Format("Bearer {0}", bearer));
            }
            yield return MakeWebRequest(webRequest, onError, onSuccess);
        }

        /// <summary>
        /// Makes a POST request to the given URL with the given body, executing the corresponding callback on completion.
        /// </summary>
        /// <param name="url">The URL to call.</param>
        /// <param name="body">The body of the POST.</param>
        /// <param name="onError">The callback executed on an unsuccessful request.</param>
        /// <param name="onSuccess">The callback executed on a successful request.</param>
        /// <param name="bearer">Optional bearer token for authentication.</param>
        /// <returns></returns>
        public static IEnumerator PostRequest(string url, string body, Action<HttpError> onError, Action<string> onSuccess, string bearer) {
            UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            if (bearer != null) {
                webRequest.SetRequestHeader("Authorization", string.Format("Bearer {0}", bearer));
            }
            yield return MakeWebRequest(webRequest, onError, onSuccess);
        }

        private static IEnumerator MakeWebRequest(UnityWebRequest req, Action<HttpError> onError, Action<string> onSuccess){
            using (req) {
                // Request and wait for the desired page.
                yield return req.SendWebRequest();
                if (req.result != UnityWebRequest.Result.Success) {
                    string errorMessage = string.Format("Error making GET request to URL: {0} : {1}", req.url, req.error);
                    HttpError error = new HttpError(req.responseCode, req.error);
                    onError.Invoke(error);
                }
                else {
                    // Debug.Log("Received: " + webRequest.downloadHandler.text);
                    try {
                        onSuccess.Invoke(req.downloadHandler.text);
                    }
                    catch (Exception e) {
                        HttpError error = new HttpError(500, e.ToString());
                        onError.Invoke(error);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the local IPv4 address of this machine.
        /// </summary>
        /// <returns>The local IPv4.</returns>
        public static IPAddress GetLocalIPAddress() {

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint.Address;
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        /// <summary>
        /// Validates that a port is valid port number.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="defaultPort">The value to use if the provided value is invalid.</param>
        /// <returns>A valid port number.</returns>
        public static int ValidatePortValue(int value, int defaultPort) {
            int port = value;
            if (port <= 0 || port > 65535) {
                port = defaultPort;
            }
            return port;
        }

        public class HttpError {
            public long statusCode;
            public string message;

            public HttpError(long statusCode, string message) {
                this.statusCode = statusCode;
                this.message = message;
            }

            public override string ToString(){
                return this.statusCode + ": " + this.message;
            }
        }

        public class ConnectionStatus {
            public string message;
            public Status status;

            public enum Status {
                CONNECTING = 0,
                CONNECTED = 1,
                ERROR = 2,
                DISCONNECTED = 3,
            }
        }
    }
}