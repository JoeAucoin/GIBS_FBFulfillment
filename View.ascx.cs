/*
' Copyright (c) 2024  GIBS.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using GIBS.Modules.GIBS_FBFulfillment.Components;
using GIBS.Modules.FBClients.Components;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Drawing;
using GIBS.Modules.GIBS_FBFoodOrder.Components;
using DotNetNuke.Common;
using DotNetNuke.Entities.Portals;
using System.Web.UI;
using System.Data;
using GIBS.FBClients.Components;
using System.Resources;
using System.Reflection;
using System.Linq;

namespace GIBS.Modules.GIBS_FBFulfillment
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from GIBS_FBFulfillmentModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : GIBS_FBFulfillmentModuleBase    //, IActionable
    {
        private GridViewHelper helper;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    txtVisitDate.Text =  DateTime.Now.ToShortDateString();
                    FillGrid();

                    if (Request.QueryString["enterorder"] != null)
                    {
                        string key = Request.QueryString["enterorder"].ToString();
                        char separator = '-'; // Space character
                        string[] keys = key.Split(separator); // returned array
                        int visitID = Int32.Parse(keys[0].ToString());
                        int clientID = Int32.Parse(keys[1].ToString());
                        FillOrderGrid(visitID, clientID);
                    }

                }

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        public void FillGrid()
        {

            try
            {

                List<FBFInfo> fbfitems;
                FBFController fbfcontroller = new FBFController();
                fbfitems = fbfcontroller.GetOrdersByStatusCode(Int32.Parse(ddlStatus.SelectedValue.ToString()),txtVisitDate.Text.ToString());
                LabelOrderCount.Text = fbfitems.Count.ToString() + " Records";
                if (fbfitems == null || fbfitems.Count == 0)
                {
                   
                    LabelDebug.Visible = true;
                    LabelDebug.Text = "No Orders";
                }
                else
                {
                    if (Int32.Parse(ddlStatus.SelectedValue.ToString()) != -1)
                    {
                        GridViewOrders.Columns[0].Visible = false;
                    }
                    else 
                    { 
                        GridViewOrders.Columns[0].Visible=true; 
                    }

                    LabelDebug.Visible = false;
                    
                    GridViewOrders.DataSource = fbfitems;
                    GridViewOrders.DataBind();
                }


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        


        //public ModuleActionCollection ModuleActions
        //{
        //    get
        //    {
        //        var actions = new ModuleActionCollection
        //            {
        //                {
        //                    GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
        //                    EditUrl(), false, SecurityAccessLevel.Edit, true, false
        //                }
        //            };
        //        return actions;
        //    }
        //}

        protected void GridViewOrders_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            FillGrid();
            GridViewOrders.PageIndex = e.NewPageIndex;
            GridViewOrders.DataBind();
        }

        protected void GridViewOrders_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            try
            {
                e.Cancel = true;

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void GridViewOrders_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EnterOrder")
                {
                    // get the ID of the clicked row
                    string key = e.CommandArgument.ToString();
                    char separator = '-'; // Space character
                    string[] keys = key.Split(separator); // returned array
                    int visitID = Int32.Parse(keys[0].ToString());
                    int clientID = Int32.Parse(keys[1].ToString());

                    FillOrderGrid(visitID, clientID);

                }

                ///Client-Manager/ctl/EditClient/mid/387/cid/3
                ///
                if (e.CommandName == "LinkToClient")
                {
                    string _clientID = e.CommandArgument.ToString();
                    string key = "";
                    if (Settings.Contains("foodBankClientModuleID"))
                    {
                        key = Settings["foodBankClientModuleID"].ToString();
                    }
                            
                    char separator = '-'; // Space character
                    string[] keys = key.Split(separator); // returned array
                    int tabID = Int32.Parse(keys[0].ToString());
                    int moduleID = Int32.Parse(keys[1].ToString());

                    Response.Redirect(EditUrl(tabID, "EditClient", true, "mid=" + moduleID, "cid="+ _clientID));

                }

                    if (e.CommandName == "SendText")
                {

                    string key = e.CommandArgument.ToString();
                    char separator = '-'; // Space character
                    string[] keys = key.Split(separator); // returned array
                    string _clientCell = keys[0].ToString();
                    int visitID = Int32.Parse(keys[1].ToString());
                    string _msg = Localization.GetString("TwilioPickupMessage", this.LocalResourceFile);
                    SendTwilioMessage(_msg.ToString(), _clientCell.ToString(), visitID);

                    FillGrid(); 

                }

                if (e.CommandName == "Edit")
                {
                    string key = e.CommandArgument.ToString();
                    char separator = '-'; // Space character
                    string[] keys = key.Split(separator); // returned array
                    string _clientCell = keys[0].ToString();
                    int _visitID = Int32.Parse(keys[1].ToString());

                    
                   // var cellphone = DataBinder.Eval(e.Row.DataItem, "ClientCellPhone");

                    GetOrder(_visitID);
                    HiddenFieldVisitID.Value = _visitID.ToString();
                    HiddenFieldClientCell.Value = _clientCell.ToString();

                    if (Settings.Contains("processOrderLayOut"))
                    {
                        if(Settings["processOrderLayOut"].ToString() == "1")
                        {
                            List<FBFInfo> items;
                            FBFController controller = new FBFController();
                            items = controller.GetOrderDetails(_visitID);
                            
                            GetOrder(_visitID);
                            if (items != null)
                            {
                                panelGrid.Visible = false;
                                PanelRecord.Visible = true;
                                GroupIt();
                                GridViewOrder.DataSource = items;
                                GridViewOrder.DataBind();

                                //   FillTranslationsGrid();
                            }
                            else
                            {
                                LabelOrderMessage.Text = "Order Error";
                            }
                        }
                        if (Settings["processOrderLayOut"].ToString() == "2")
                        {
                            FillRepeater(_visitID);
                        }
                    }





                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        private DataTable dtTopHalf;
        private DataTable dtBottomHalf;

        public void FillRepeater(int _visitID)
        {

            try
            {

                List<FBFInfo> items;
                FBFController controller = new FBFController();
                items = controller.GetOrderDetails(_visitID);

                if (items != null)
                {
                    panelGrid.Visible = false;
                    PanelRecord.Visible = true;
                }

                  DataTable dt = new DataTable();
                 dt = ToDataTable(items);
                int halfCount = dt.Rows.Count / 2;
                dtTopHalf = dt.AsEnumerable().Select(x => x).Take(halfCount).CopyToDataTable();
                dtBottomHalf = dt.AsEnumerable().Select(x => x).Skip(halfCount).CopyToDataTable();
                repeater1.DataSource = new List<bool>() { true, false };
                repeater1.DataBind();


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        protected void repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                bool isTopHalf = (bool)e.Item.DataItem;
                GridView gvHalf = e.Item.FindControl("gvHalf") as GridView;
                gvHalf.DataSource = isTopHalf ? dtTopHalf : dtBottomHalf;
                gvHalf.DataBind();
               
                if (Settings.Contains("showCategoryOnFulfillment"))
                {
                    gvHalf.Columns[0].Visible = Convert.ToBoolean(Settings["showCategoryOnFulfillment"].ToString());
                }
            }
            
        }


        protected void GridViewOrders_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {
                e.Cancel = true;

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void btnFillGrid_Click(object sender, EventArgs e)
        {
            try
            {
                FillGrid();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public void GroupIt()
        {

            try
            {

                helper = new GridViewHelper(this.GridViewOrder);
                helper.RegisterGroup("ProductCategory", true, true);

                helper.GroupHeader += new GroupEvent(helper_GroupHeader);
                helper.GroupSummary += new GroupEvent(helper_Bug);
                helper.ApplyGroupSort();

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        private void helper_GroupHeader(string groupName, object[] values, GridViewRow row)
        {
            row.BackColor = Color.FromArgb(236, 236, 236);
            row.Cells[0].Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + row.Cells[0].Text + "</b>";
        }

        private void helper_Bug(string groupName, object[] values, GridViewRow row)
        {
            if (groupName == null) return;

            row.BackColor = Color.LightCyan;
            row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            row.Cells[0].Text = values[0] + " Totals &nbsp;";
        }

        protected void GridViewOrder_Sorting(object sender, GridViewSortEventArgs e)
        {

        }


        public void FillOrderGrid(int visitID, int clientID)
        {

            try
            {

                List<FBFOInfo> items;
                Controller controller = new Controller();
                items = controller.GetOrderList(visitID, clientID);
                if (items == null || items.Count == 0)
                {
                    ButtonSaveOrder.Visible = false;
                    LabelDebug.Visible = true;
                    LabelDebug.Text = "This page is no longer valid.";
                }
                else
                {
                    GetOrder(visitID);
                    HiddenFieldVisitID.Value = visitID.ToString();
                    panelGrid.Visible = false;
                    
                    PanelEnterOrder.Visible = true;

                   // LabelDebug.Visible = false;
                    LabelRecordCount.Text = items.Count.ToString() + " Items";
                    ButtonSaveOrder.Visible = true;
                    GridViewOrderSheet.DataSource = items;
                    GridViewOrderSheet.DataBind();
                }


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public void GetOrder(int visitID)
        {

            try
            {
                Controller controller = new Controller();
                FBFOInfo fbfo = controller.GetOrder(visitID);

                if (fbfo != null)
                {
                    string OrderDetails = "Visit Date: " + fbfo.VisitDate.ToShortDateString() + "<br />"
                                        + fbfo.ClientName.ToString() + "<br />"
                                        + "Bags: " + fbfo.VisitNumBags.ToString() + "<br />"
                                        + "Household Total: " + fbfo.HouseholdTotal.ToString() + "<br />"
                                        + "Notes: " + fbfo.VisitNotes.ToString();
                    LabelOrderDetails.Text = OrderDetails.ToString();
                    LabelorderDetails1.Text = OrderDetails.ToString();

                    

                    switch (fbfo.HouseholdTotal)
                    {
                        case 1:
                            orderDetailsDiv.Attributes.Add("style", "background-color:#F9E770;padding:15px;"); // YELLOW
                            break;
                        case 2:
                            orderDetailsDiv.Attributes.Add("style", "background-color:#F9E770;padding:15px;"); // YELLOW
                            break;
                        case 3:
                            orderDetailsDiv.Attributes.Add("style", "background-color:#6ACD8D;padding:15px;"); //GREEN
                            break;
                        case 4:
                            orderDetailsDiv.Attributes.Add("style", "background-color:#6ACD8D;padding:15px;"); //GREEN
                            break;
                        case 5:
                            orderDetailsDiv.Attributes.Add("style", "background-color:#00BFFF;padding:15px;"); // BLUE
                            break;
                        case 6:
                            orderDetailsDiv.Attributes.Add("style", "background-color:#00BFFF;padding:15px;"); // BLUE
                            break;
                        case 7:
                            orderDetailsDiv.Attributes.Add("style", "background-color:#FA7C95;padding:15px;");  //PINK
                            break;
                        case 8:
                            orderDetailsDiv.Attributes.Add("style", "background-color:#FA7C95;padding:15px;");  //PINK
                            break;

                        default:
                            
                            break;
                    }


                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        protected void LinkButtonProcessOrder_Click(object sender, EventArgs e)
        {
            Controller controller = new Controller();
            FBFOInfo item = new FBFOInfo();
            item.OrderStatusCode = 3;
            item.VisitID = Int32.Parse(HiddenFieldVisitID.Value.ToString());
            controller.UpdateVisitOrderStatusCode(item);

            if(CheckBoxNotifyClientOrderReady.Checked && HiddenFieldClientCell.ToString().Length >10)
            {
                string _msg = Localization.GetString("TwilioPickupMessage", this.LocalResourceFile);
                string _clientcell = HiddenFieldClientCell.Value.ToString();
                int visitID = Int32.Parse(HiddenFieldVisitID.Value.ToString());
                SendTwilioMessage(_msg.ToString(), _clientcell.ToString(), visitID);

            }
            HiddenFieldClientCell.Value = "";
            HiddenFieldVisitID.Value = "";
            LabelOrderDetails.Text = "";
            LabelorderDetails1.Text = "";
            Response.Redirect(PortalSettings.ActiveTab.Url, true);
        }


        protected void GridViewOrderSheet_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

      


        //  private string previousFirstColumnValue = "";
        Color colorChoice = Color.LightGoldenrodYellow;
        protected void GridViewOrderSheet_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // Get the first column value of the current row
                string currentFirstColumnValue = e.Row.Cells[0].Text;

                // Check if it's not the first row
                if (e.Row.RowIndex != 0)
                {
                    // Get the first column value of the previous row
                    string previousFirstColumnValue = GridViewOrderSheet.Rows[e.Row.RowIndex - 1].Cells[0].Text;

                    // Check if the current first column value is different from the previous row's first column value
                    if (currentFirstColumnValue != previousFirstColumnValue)
                    {
                        // If the current row is the start of a new group, alternate between two colors
                        if (colorChoice == Color.LightGoldenrodYellow)
                        {
                            e.Row.Cells[0].BackColor = Color.AliceBlue;
                            colorChoice = Color.AliceBlue;
                        }
                        else
                        {
                            e.Row.Cells[0].BackColor = Color.LightGoldenrodYellow;
                            colorChoice = Color.LightGoldenrodYellow;
                        }
                    }
                    else
                    {
                        // Use the same color as the previous row if the first column value hasn't changed
                        e.Row.Cells[0].BackColor = colorChoice;
                    }
                }
                else
                {
                    // If it's the first row, color the first column cell with the initial color choice
                    e.Row.Cells[0].BackColor = Color.LightGoldenrodYellow;
                    colorChoice = Color.LightGoldenrodYellow;
                }

           

                int limit = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Limit"));
                DropDownList ddlQty = (DropDownList)e.Row.FindControl("DropDownListQty");

                for (int i = 1; i <= limit; i++)
                {
                    // do stuff
                    ListItem lst = new ListItem(i.ToString(), i.ToString());
                    ddlQty.Items.Add(lst); ;
                }



            }
        }

        protected void ButtonSaveOrder_Click(object sender, EventArgs e)
        {
            ButtonSaveOrder.Enabled = false;
            ProcessGrid();

           // GridViewOrderSheet.Visible = false;
           // ButtonSaveOrder.Visible = false;
            LabelDebug.Visible = true;
            LabelDebug.Text = "Order has Been Entered!";
            HiddenFieldVisitID.Value = "";
            LabelOrderDetails.Text = "";
            LabelorderDetails1.Text = "";
            //  Response.Redirect(PortalSettings.ActiveTab.Url, true);
            Response.Redirect(Globals.NavigateURL(), true);
        }

        public void ProcessGrid()
        {

            try
            {
                int visitID = Int32.Parse(HiddenFieldVisitID.Value.ToString());


                foreach (GridViewRow row in GridViewOrderSheet.Rows)
                {

                    //  object rows;
                    if ((DropDownList)row.FindControl("DropDownListQty") is DropDownList ddlQty1)
                    {
                        //if (ddlQty1.SelectedIndex > 0)
                        if (ddlQty1.SelectedValue.ToString() != "0")
                        {
                            //   DropDownList ddlQty = (DropDownList)row.FindControl("DropDownListQty");
                            HiddenField hidProductID = (HiddenField)row.FindControl("HiddenFieldProductID");
                            int productID = Convert.ToInt32(hidProductID.Value.ToString());
                            SaveOrderItem(visitID, productID, Int32.Parse(ddlQty1.SelectedValue.ToString()));
                        }
                    }
                    else
                    {

                    }
                    
                }


                Controller controller = new Controller();
                FBFOInfo item = new FBFOInfo();
                item.OrderStatusCode = 2;
                item.VisitID = visitID;
                controller.UpdateVisitOrderStatusCode(item);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        public void SaveOrderItem(int _VisitID, int _ProductID, int _Quantity)
        {
            try
            {
                Controller controller = new Controller();
                FBFOInfo item = new FBFOInfo();

                item.VisitID = _VisitID;
                item.ProductID = _ProductID;
                item.Quantity = _Quantity;

                controller.InsertVisitOrderItem(item);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        protected void GridViewOrders_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void GridViewOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton sndTextButton = (LinkButton)e.Row.FindControl("LinkButtonSendText");
                int orderStatusCode = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "OrderStatusCode"));
                var cellphone = DataBinder.Eval(e.Row.DataItem, "ClientCellPhone");


                if (orderStatusCode == 3 && cellphone.ToString().Length > 10)
                {
                    sndTextButton.Visible = true;
                }
            }
        }

        public void SendTwilioMessage(string _message, string _clientCellNumber, int _visitID)
        {
            string _TwilioAccountSid = "";
            string _TwilioAuthToken = "";
            string _TwilioPhoneNumber = "";
           

            if (Settings.Contains("twilioAccountSid"))
            {
               _TwilioAccountSid = Settings["twilioAccountSid"].ToString();
            }
            if (Settings.Contains("twilioAuthToken"))
            {
                _TwilioAuthToken = Settings["twilioAuthToken"].ToString();
            }
            if (Settings.Contains("twilioPhoneNumber"))
            {
                _TwilioPhoneNumber = Settings["twilioPhoneNumber"].ToString();
            }

            if (_TwilioAccountSid != string.Empty && _TwilioAuthToken != string.Empty && _TwilioPhoneNumber != string.Empty)
            {

                TwilioSMS twilioSMS = new TwilioSMS(_TwilioAccountSid.ToString(), _TwilioAuthToken.ToString(), _TwilioPhoneNumber.ToString());
                twilioSMS.SendSMS(_clientCellNumber.ToString(), _message.ToString());
            }

            // UPDATE THE OrderStatusCode to 4
            Controller controller = new Controller();
            FBFOInfo item = new FBFOInfo();
            item.OrderStatusCode = 4;
            item.VisitID = _visitID;
            controller.UpdateVisitOrderStatusCode(item);

            LabelDebug.Text += "<br />" + Localization.GetString("TwilioMessageSent", this.LocalResourceFile);
            LabelDebug.Visible = true;
        }

        
    }
}