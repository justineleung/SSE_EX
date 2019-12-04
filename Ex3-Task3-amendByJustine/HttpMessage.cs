using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SSE
{
    public class HttpMessage
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
        public const string HEAD = "HEAD";
        public const string OPTIONS = "OPTIONS";
        public const string TRACE = "TRACE";

        public string Method = "";
        public string Host = "";
        public string Resource = "";
        public Dictionary<string, string> Headers = new Dictionary<string, string>();
        public string Content = "";
        public string StatusCode = "";
        public string StatusMessage = "";

        /// <summary>
        /// Construct an HTTP request.
        /// </summary>
        public HttpMessage(string method, string host, string resource, Dictionary<string, string> headers, string content)
        {
            this.Method = method;
            this.Host = host;
            this.Resource = resource;
            this.Headers = headers;
            if (Headers == null)
                Headers = new Dictionary<string, string>();
            this.Content = content;
            StatusCode = null;
            StatusMessage = null;
        }

        /// <summary>
        /// Construct an HTTP response.
        /// </summary>        
        public HttpMessage(string statusCode, string statusMessage, Dictionary<string, string> headers, string content)
        {
            this.Method = null;
            this.Host = null;
            this.Resource = null;
            this.Headers = headers;
            if (Headers == null)
                Headers = new Dictionary<string, string>();
            this.Content = content;
            StatusCode = statusCode;
            StatusMessage = statusMessage;
        }

        /// <summary>
        /// Constructs an HTTP message by parsing a (received) string.
        /// </summary>
        /// <param name="message"></param>
        public HttpMessage(string message)
        {

            // [done] TODO: parse HTTP request/response message
            // empty line -> end of header; the rest is content
            //string[] httpMessageArr0 = message.Split("\n\n"); 
            //if (httpMessageArr0.Length>1)
            //    Content = httpMessageArr0[1];

            //string[] httpMessageArr = httpMessageArr0[0].Split("\n");

            int endOfHeader = message.IndexOf("\n\n");
            string httpMessageArr0 = message.Substring(0, endOfHeader);
            string httpMessageArr1 = message.Substring(endOfHeader+2, message.Length - endOfHeader-2);

            string[] httpMessageArr = httpMessageArr0.Split("\n");

            if (httpMessageArr[0].StartsWith("HTTP"))
            {
                //httpResponse
                string[] httpResponseLine1Arr = httpMessageArr[0].Split(" ");
                StatusCode = httpResponseLine1Arr[1];
                StatusMessage = httpResponseLine1Arr[2];            
            }
            else
            {
                //httpRequest
                string[] httpRequestLine1Arr = httpMessageArr[0].Split(" ");
                Method = httpRequestLine1Arr[0];
                Resource = httpRequestLine1Arr[1];
                StatusCode = null;
                StatusMessage = null;
            }

            for (int i = 1; i < httpMessageArr.Length; i++)
            {
                if (httpMessageArr[i].IndexOf(": ") > -1)
                {
                    string[] httpResponseLineIArr = httpMessageArr[i].Split(": ");
                    if (httpResponseLineIArr[0].Equals("Host"))
                        Host = httpResponseLineIArr[1];
                    else
                        Headers.Add(httpResponseLineIArr[0].ToLower(), httpResponseLineIArr[1]);
                }
            }

            Content = httpMessageArr1;
            if (Headers.ContainsKey("transfer-encoding") && Headers["transfer-encoding"].Equals("chunked"))
            {
                Content = decodeChunked(Content);
            }

            if (StatusCode != null)
            {
                //httpResponse
                HttpMessage httpMessage = new HttpMessage(StatusCode, StatusMessage, Headers, Content);
            } 
            else
            {
                //httpRequest
                HttpMessage httpMessage = new HttpMessage(Method, Host, Resource, Headers, Content);
            }
        }

        /// <summary>
        /// Returns the string representation of the message.
        /// </summary>        
        public override string ToString()
        {
            string httpMessageStr = "";
            // [done] TODO: construct HTTP request/response message
            if(StatusCode != null)
            {
                //httpResponse
                httpMessageStr += "HTTP/1.1" + " " + StatusCode + " " + StatusMessage + "\n";
            } 
            else
            {
                //httpRequest
                httpMessageStr += this.Method + " " + this.Resource + " " + "HTTP/1.1" + "\n";
                httpMessageStr += "Host: " + this.Host + "\n";
            }

            foreach (string name in Headers.Keys)
            {
                string nameCase = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
                httpMessageStr += nameCase + ": " + Headers[name] + "\n";
            }

            string contentProcessed = "";
            if (Headers.ContainsKey("transfer-encoding") && Headers["transfer-encoding"].Equals("chunked"))
            {
                contentProcessed = encodeChunked(this.Content);
            } 
            else
            {
                contentProcessed = this.Content;
            }

            httpMessageStr += "\n" + contentProcessed;
            return httpMessageStr;
            //return "";
        }

        public string encodeChunked(string input)
        {
            // TODO: encode Chunked (ref: TestChunkedResponseMessage / TestChunkedResponseMessage2)
            return input;
        }

        public string decodeChunked(string input)
        {
            // TODO: decode Chunked (ref: TestChunkedResponseMessage / TestChunkedResponseMessage2)
            return input;
        }
    }
}
