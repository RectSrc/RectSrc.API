using System;
using System.Net;

namespace RectSrc.API
{
    public static class RsA
    {
        private static void Main(string[] args)
        {
            RsAClient RsAClient = new RsAClient();
            RsAClient.apikey = "testKey";
            RsAReturn ret = RsAClient.GetKey("khhs", "hildinggrandprix");
            Console.WriteLine(ret.ToString());
        }

        public class RsAClient
        {
            public string apikey = "";
            public RsAReturn GetKey(string username, string password)
            {
                WebClient webClient = new WebClient();
                string state = webClient.DownloadString($"http://api.rectpm.tk/?key={this.apikey}&username={username}&password={password}");
                string[] data = state.Split(";", StringSplitOptions.RemoveEmptyEntries);
                RsAState rsaState = 0;
                switch (data[0]) {
                    case "OK":
                        rsaState = RsAState.OK;
                        break;
                    case "ERROR":
                        switch (data[1])
                        {
                            case "INVALIDINFO":
                                rsaState = RsAState.INVALIDINFO;
                                break;
                            case "INVALIDKEY":
                                rsaState = RsAState.INVALIDKEY;
                                break;
                            case "CONNECTIONERROR":
                                rsaState = RsAState.CONNECTIONERROR;
                                break;
                        }
                        break;
                }
                string key = "";
                if (rsaState == RsAState.OK)
                    key = data[1];
                RsAReturn rsateturn = new RsAReturn(rsaState, key);
                return rsateturn;
            }
        }

        public class RsAReturn
        {
            public RsAState rsastate;
            public string key;

            public RsAReturn(RsAState rsastate, string key)
            {
                this.rsastate = rsastate;
                this.key = key;
            }

            public override string ToString()
            {
                return "rsa.RsAReturn(" + rsastate + ", " + key + ")";
            }
        }
        public enum RsAState
        {
            OK = 1,
            INVALIDINFO = 2,
            INVALIDKEY = 3,
            CONNECTIONERROR = 4
        }
    }
}
