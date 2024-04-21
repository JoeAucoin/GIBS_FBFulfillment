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

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Drawing;
using GIBS.Modules.GIBS_FBFoodOrder.Components;
using DotNetNuke.Common;
using DotNetNuke.Entities.Portals;
using System.Web.UI;

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
    public partial class View : GIBS_FBFulfillmentModuleBase, IActionable
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

                List<FBFInfo> items;
                FBFController controller = new FBFController();
                items = controller.GetOrdersByStatusCode(Int32.Parse(ddlStatus.SelectedValue.ToString()),txtVisitDate.Text.ToString());
                LabelOrderCount.Text = items.Count.ToString() + " Records";
                if (items == null || items.Count == 0)
                {
                    
                    LabelDebug.Visible = true;
                    LabelDebug.Text = "No Orders";
                }
                else
                {
                    LabelDebug.Visible = false;
                    
                    GridViewOrders.DataSource = items;
                    GridViewOrders.DataBind();
                }


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        


        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }

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


                if (e.CommandName == "Edit")
                {
                    int _visitID = Convert.ToInt32(e.CommandArgument);

                    List<FBFInfo> items;
                    FBFController controller = new FBFController();
                    items = controller.GetOrderDetails(_visitID);
                    HiddenFieldVisitID.Value = _visitID.ToString();
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

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
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

                    LabelDebug.Visible = false;
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
                                        + "Bags Allowed: " + fbfo.VisitNumBags.ToString();
                    LabelOrderDetails.Text = OrderDetails.ToString();
                    LabelorderDetails1.Text = OrderDetails.ToString();
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
            Response.Redirect(PortalSettings.ActiveTab.Url, true);

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


    }
}