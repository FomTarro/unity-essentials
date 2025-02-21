using System;
using UnityEngine;

namespace Skeletom.Essentials.Utils {

    public static class VersionUtils {
        /// <summary>
        /// Compares the current version to the remote version. Returns true if the remote is newer.
        /// </summary>
        /// <param name="remoteVersion"></param>
        /// <returns></returns>
        public static bool CompareVersion(VersionInfo remoteVersion) {
            string currentVersion = Application.version;
            return IsOlderThan(currentVersion, remoteVersion.version);
        }

        /// <summary>
        /// Compares the Version A to the Version B. Returns true if the VersionB is newer.
        /// </summary>
        /// <param name="versionA"></param>
        /// <param name="versionB"></param>
        /// <returns></returns>
        public static bool IsOlderThan(string versionA, string versionB) {
            return versionA == null || versionA.Length <= 0 || versionA.CompareTo(versionB) < 0;
        }

        /// <summary>
        /// Fetches version information from a remote URL via a GET request, 
        /// and automatically executes the corresponding callback depending on 
        /// if the remote version is newer or older than the current version.
        /// </summary>
        /// <param name="url">The URL to fetch from.</param>
        /// <param name="onRemoteNewer">Callback executed if the remote version is newer than the current version.</param>
        /// <param name="onRemoteOlder">Callback executed if the remote version is older/the same as the current version.</param>
        /// <param name="onError">Callback executed if an error occurs while making the GET request.</param>
        /// <param name="bearer">Optional bearer token for authentication.</param>
        public static void FetchAndCheckVersion(string url, Action<VersionInfo> onRemoteNewer, Action<VersionInfo> onRemoteOlder, Action<HttpUtils.HttpError> onError,  string bearer){
            HttpUtils.GetRequest(url,
            (error) => {  
                onError.Invoke(error);
            },
            (success) => {
                try{
                    VersionInfo info = JsonUtility.FromJson<VersionInfo>(success);
                    if (CompareVersion(info)){
                        onRemoteNewer.Invoke(info);
                    }else{
                        onRemoteOlder.Invoke(info);
                    }
                }catch(Exception e){
                    HttpUtils.HttpError error = new HttpUtils.HttpError(500, e.Message);
                    onError.Invoke(error);
                }
            },
            bearer);
        }

        [System.Serializable]
        public class VersionInfo {
            public string version;
            public string date;
            public string url;

            public override string ToString() {
                return JsonUtility.ToJson(this);
            }
        }
    }
}