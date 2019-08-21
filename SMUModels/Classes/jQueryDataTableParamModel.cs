﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMUModels
{
    //public class jQueryDataTableParamModel
    //{
    //    /// <summary>
    //    /// Request sequence number sent by DataTable,
    //    /// same value must be returned in response
    //    /// </summary>       
    //    public string sEcho { get; set; }

    //    /// <summary>
    //    /// Text used for filtering
    //    /// </summary>
    //    public string sSearch { get; set; }

    //    /// <summary>
    //    /// Number of records that should be shown in table
    //    /// </summary>
    //    public int iDisplayLength { get; set; }

    //    /// <summary>
    //    /// First record that should be shown(used for paging)
    //    /// </summary>
    //    public int iDisplayStart { get; set; }

    //    /// <summary>
    //    /// Number of columns in table
    //    /// </summary>
    //    public int iColumns { get; set; }

    //    /// <summary>
    //    /// Number of columns that are used in sorting
    //    /// </summary>
    //    public int iSortingCols { get; set; }
    //    public int iSortCol_0 { get; set; }
    //    public string sSortDir_0 { get; set; }

    //    /// <summary>
    //    /// Comma separated list of column names
    //    /// </summary>
    //    public string sColumns { get; set; }
    //}


    public class jQueryDataTableParamModel
    {
        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }
        public int iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }

        /// <summary>
        /// ///////////////////////////////////////
        /// </summary>
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
    }
}