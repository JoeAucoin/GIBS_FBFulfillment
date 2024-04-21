/*
' Copyright (c) 2024 GIBS.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

//using System.Xml;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;
using System.Collections.Generic;

namespace GIBS.Modules.GIBS_FBFulfillment.Components
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for GIBS_FBFulfillment
    /// 
    /// The FeatureController class is defined as the BusinessController in the manifest file (.dnn)
    /// DotNetNuke will poll this class to find out which Interfaces the class implements. 
    /// 
    /// The IPortable interface is used to import/export content from a DNN module
    /// 
    /// The ISearchable interface is used by DNN to index the content of a module
    /// 
    /// The IUpgradeable interface allows module developers to execute code during the upgrade 
    /// process for a module.
    /// 
    /// Below you will find stubbed out implementations of each, uncomment and populate with your own data
    /// </summary>
    /// -----------------------------------------------------------------------------

    //uncomment the interfaces to add the support.
    public class FeatureController //: IPortable, ISearchable, IUpgradeable
    {


        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        //public string ExportModule(int ModuleID)
        //{
        //string strXML = "";

        //List<GIBS_FBFulfillmentInfo> colGIBS_FBFulfillments = GetGIBS_FBFulfillments(ModuleID);
        //if (colGIBS_FBFulfillments.Count != 0)
        //{
        //    strXML += "<GIBS_FBFulfillments>";

        //    foreach (GIBS_FBFulfillmentInfo objGIBS_FBFulfillment in colGIBS_FBFulfillments)
        //    {
        //        strXML += "<GIBS_FBFulfillment>";
        //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objGIBS_FBFulfillment.Content) + "</content>";
        //        strXML += "</GIBS_FBFulfillment>";
        //    }
        //    strXML += "</GIBS_FBFulfillments>";
        //}

        //return strXML;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        //public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        //{
        //XmlNode xmlGIBS_FBFulfillments = DotNetNuke.Common.Globals.GetContent(Content, "GIBS_FBFulfillments");
        //foreach (XmlNode xmlGIBS_FBFulfillment in xmlGIBS_FBFulfillments.SelectNodes("GIBS_FBFulfillment"))
        //{
        //    GIBS_FBFulfillmentInfo objGIBS_FBFulfillment = new GIBS_FBFulfillmentInfo();
        //    objGIBS_FBFulfillment.ModuleId = ModuleID;
        //    objGIBS_FBFulfillment.Content = xmlGIBS_FBFulfillment.SelectSingleNode("content").InnerText;
        //    objGIBS_FBFulfillment.CreatedByUser = UserID;
        //    AddGIBS_FBFulfillment(objGIBS_FBFulfillment);
        //}

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// -----------------------------------------------------------------------------
        //public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        //{
        //SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

        //List<GIBS_FBFulfillmentInfo> colGIBS_FBFulfillments = GetGIBS_FBFulfillments(ModInfo.ModuleID);

        //foreach (GIBS_FBFulfillmentInfo objGIBS_FBFulfillment in colGIBS_FBFulfillments)
        //{
        //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objGIBS_FBFulfillment.Content, objGIBS_FBFulfillment.CreatedByUser, objGIBS_FBFulfillment.CreatedDate, ModInfo.ModuleID, objGIBS_FBFulfillment.ItemId.ToString(), objGIBS_FBFulfillment.Content, "ItemId=" + objGIBS_FBFulfillment.ItemId.ToString());
        //    SearchItemCollection.Add(SearchItem);
        //}

        //return SearchItemCollection;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="Version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        //public string UpgradeModule(string Version)
        //{
        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        #endregion

    }

}
