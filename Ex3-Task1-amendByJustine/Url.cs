using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Task1
{
    /// <summary>
    /// A class for generating and parsing HTTP-URIs.
    /// </summary>
    public class Url
    {
        const string VALID_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$-_.~";

        public string Scheme = "";
        public string Host = "";
        public int Port = 80;
        public string Path = "";
        public string Query = "";
        public string FragmentId = "";


        /// <summary>
        /// Constructor for parsing URLs.
        /// </summary>
        public Url(string urlStr)
        {
            // TODO
            string regexPattern = @"^(?<s1>(?<s0>[^:/\?#]+):)?"
                                + @"(?<a1>//(?<a0>[^:?#]*):)?"
                                + @"(?<port>[^/\?#]*)?"
                                + @"(?<p0>[^\?#]*)"
                                + @"(?<q1>\?(?<q0>[^#]*))?"
                                + @"(?<f1>#(?<f0>.*))?";
            Regex regex = new Regex(regexPattern, RegexOptions.ExplicitCapture);
            Match match = regex.Match(urlStr);
            string PortStr = "";

            //Match match = Regex.Match(urlStr, @"^(http|https|ftp)://", RegexOptions.IgnorePatternWhitespace);
            if (match.Success)
            {
                this.Scheme = match.Groups["s0"].Value;
                this.Host = match.Groups["a0"].Value;
                //
                PortStr = match.Groups["port"].Value;
                if(PortStr!=null && PortStr.Length>0)
                    this.Port = Int32.Parse(PortStr);

                this.Path = Decode(match.Groups["p0"].Value);
                this.Query = match.Groups["q0"].Value;
                this.FragmentId = match.Groups["f0"].Value;
            }
            else
            {
                throw new FormatException("Could not parse URL: " + urlStr);
            }
        }

        /// <summary>
        /// Constructor for building URLs from their components.
        /// </summary>
        public Url(string scheme, string host, int port, string path, string query, string fragmentId)
        {
            this.Scheme = scheme;
            this.Host = host;
            this.Port = port;
            this.Path = path;
            this.Query = query;
            this.FragmentId = fragmentId;
        }

        /// <summary>
        /// Returns the string representation of the URL.
        /// </summary>
        public override string ToString()
        {
            // TODO
            string url = "";
            url = this.Scheme + "://" + this.Host; //scheme and host
            url += ":" + this.Port.ToString(); //port part
            url += Encode(this.Path);
            url += (this.Query != null && Query.Trim().Length > 0) ? "?" + this.Query : "";
            url += (this.FragmentId != null && FragmentId.Trim().Length > 0) ? "#" + this.FragmentId : "";

            return url;
        }

        /// <summary>
        /// Encodes any special characters in the URL with an escaping sequence.
        /// </summary>
        public static string Encode(string s)
        {
            string result = "";

            // TODO
            result = HttpUtility.UrlPathEncode(s);
            return result;
        }

        /// <summary>
        /// Decodes any escaping sequence in the URL with the corresponding characters.
        /// </summary>
        public static string Decode(string s)
        {
            // TODO
            s = HttpUtility.UrlDecode(s);
            return s;
        }
    }
}
