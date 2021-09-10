using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OlxWebsiteFyp.Models;
using System.Data;
using System.Data.SqlClient;


namespace OlxWebsiteFyp.Models
{
    public class OrderPage
    {
        public int re_id { get; set; }
        public string re_name { get; set; }

        public string re_gender { get; set; }

        public string re_Photo { get; set; }

        public string re_city { get; set; }

        public string i_name { get; set; }

        public string i_price { get; set; }

        public string i_image { get; set; }
        public string i_contact { get; set; }

        public string i_date { get; set; }
    }
}