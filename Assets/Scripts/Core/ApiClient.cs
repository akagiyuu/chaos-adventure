using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiClient
{
    private static readonly string baseUrl = "https://chaos-adventure-api.akagiyuu.dev";

#nullable enable
    private static string? accessToken;
    private static readonly SemaphoreSlim _authLock = new(1, 1);

    [Serializable] public class LoginData { public string username = ""; public string password = ""; }
    [Serializable] public class RegisterData { public string username = ""; public string password = ""; }
    [Serializable] public class Account { public string id = ""; public string username = ""; }
    [Serializable] public class CreateRecordData { public double time; }
    [Serializable] public class Record { public string? username; public double time; public string? createdAt; }

    [Serializable]
    public class HTTPError
    {
        public string? type;
        public string? title;
        public int status;
        public string? detail;
        public string? instance;
    }

    public class ApiException : Exception
    {
        public long StatusCode { get; }
        public HTTPError? Error { get; }

        public ApiException(long statusCode, HTTPError? err, string raw)
            : base(err != null ? $"{err.title ?? err.detail}" : $"HTTP {statusCode}: {raw}")
        {
            StatusCode = statusCode;
            Error = err;
        }
    }

    public static async Task<string> LoginAsync(LoginData data, CancellationToken ct = default)
    {
        await _authLock.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            var json = JsonUtility.ToJson(data);
            var text = await SendRequestAsync("POST", "/auth/login", json, requiresAuth: false, ct).ConfigureAwait(false);
            var token = UnwrapJsonString(text);
            if (!string.IsNullOrEmpty(token)) SetToken(token);
            return token;
        }
        finally { _authLock.Release(); }
    }

    public static async Task<string> RegisterAsync(RegisterData data, CancellationToken ct = default)
    {
        var json = JsonUtility.ToJson(data);
        var text = await SendRequestAsync("POST", "/auth/register", json, requiresAuth: false, ct).ConfigureAwait(false);
        var token = UnwrapJsonString(text);
        if (!string.IsNullOrEmpty(token)) SetToken(token);
        return token;
    }

    public static async Task<Account> GetSelfAsync(CancellationToken ct = default)
    {
        await EnsureTokenAsync(ct).ConfigureAwait(false);
        var text = await SendRequestAsync("GET", "/auth/self", null, requiresAuth: true, ct).ConfigureAwait(false);
        return JsonUtility.FromJson<Account>(text);
    }

    public static async Task<List<Record>> GetAllRecordsAsync(CancellationToken ct = default)
    {
        var text = await SendRequestAsync("GET", "/record/", null, requiresAuth: false, ct).ConfigureAwait(false);
        var arr = JsonArrayHelper.FromJson<Record>(text);
        return new List<Record>(arr);
    }

    public static async Task<string> CreateRecordAsync(CreateRecordData body, CancellationToken ct = default)
    {
        await EnsureTokenAsync(ct).ConfigureAwait(false);
        var json = JsonUtility.ToJson(body);
        var text = await SendRequestAsync("POST", "/record/", json, requiresAuth: true, ct).ConfigureAwait(false);
        return text;
    }

    public static void Logout()
    {
        accessToken = null;
    }

    private static void SetToken(string token)
    {
        accessToken = token;
    }

    private static async Task EnsureTokenAsync(CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(accessToken))
            throw new InvalidOperationException("No access token. Call LoginAsync or RegisterAsync first.");
        await Task.CompletedTask;
    }

    private static string UnwrapJsonString(string json)
    {
        if (string.IsNullOrEmpty(json)) return "";
        json = json.Trim();
        if (json.Length >= 2 && json[0] == '"' && json[^1] == '"')
            return json[1..^1].Replace("\\\"", "\"").Replace("\\\\", "\\");
        return json;
    }

    private static Task<string> SendRequestAsync(string method, string path, string? bodyJson, bool requiresAuth, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

        var url = baseUrl + path;

        UnityWebRequest req;
        if (method.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            req = UnityWebRequest.Get(url);
        }
        else
        {
            var bytes = bodyJson != null ? Encoding.UTF8.GetBytes(bodyJson) : Array.Empty<byte>();
            req = new UnityWebRequest(url, method)
            {
                uploadHandler = new UploadHandlerRaw(bytes),
                downloadHandler = new DownloadHandlerBuffer()
            };
            req.SetRequestHeader("Content-Type", "application/json");
        }

        req.SetRequestHeader("Accept", "application/json");
        if (requiresAuth)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                tcs.TrySetException(new InvalidOperationException("No access token. Call LoginAsync/RegisterAsync first."));
                req.Dispose();
                return tcs.Task;
            }
            req.SetRequestHeader("Authorization", "Bearer " + accessToken);
        }

        var op = req.SendWebRequest();

        if (ct != CancellationToken.None)
        {
            ct.Register(() =>
            {
                try { req.Abort(); } catch { }
                tcs.TrySetCanceled();
            });
        }

        op.completed += _ =>
        {
            try
            {
                var failed = req.result != UnityWebRequest.Result.Success;
                var text = req.downloadHandler?.text ?? "";

                if (failed)
                {
                    HTTPError? err = null;
                    try { err = JsonUtility.FromJson<HTTPError>(text); } catch { /* ignore */ }
                    tcs.TrySetException(new ApiException(req.responseCode, err, text));
                }
                else
                {
                    if (path.Equals("/auth/login", StringComparison.OrdinalIgnoreCase) ||
                         path.Equals("/auth/register", StringComparison.OrdinalIgnoreCase))
                    {
                        var token = UnwrapJsonString(text);
                        if (!string.IsNullOrEmpty(token)) SetToken(token);
                    }

                    tcs.TrySetResult(text);
                }
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
            finally
            {
                req.Dispose();
            }
        };

        return tcs.Task;
    }

    static class JsonArrayHelper
    {
        [Serializable]
        private class Wrapper<T> { public T[] Items = {}; }

        public static T[] FromJson<T>(string json)
        {
            var wrapped = "{\"Items\":" + json + "}";
            var w = JsonUtility.FromJson<Wrapper<T>>(wrapped);
            return w?.Items ?? Array.Empty<T>();
        }
    }
}