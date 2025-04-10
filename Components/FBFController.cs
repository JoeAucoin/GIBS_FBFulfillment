using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GIBS.Modules.GIBS_FBFulfillment.Data;
using DotNetNuke.Common.Utilities;

namespace GIBS.Modules.GIBS_FBFulfillment.Components
{
    public class FBFController
    {
        public List<FBFInfo> GetOrdersByStatusCode(int orderStatusCode, string visitDate, string serviceLocation)
        {
            return CBO.FillCollection<FBFInfo>(DataProvider.Instance().GetOrdersByStatusCode(orderStatusCode, visitDate, serviceLocation));
        }

        public List<FBFInfo> GetOrderDetails(int visitID)
        {
            return CBO.FillCollection<FBFInfo>(DataProvider.Instance().GetOrderDetails(visitID));
        }

    }
}