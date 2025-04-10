<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="GIBS.Modules.GIBS_FBFulfillment.View" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/redmond/jquery-ui.css" />
<script type="text/javascript">

    $(function () {
        $("#txtVisitDate").datepicker({
            numberOfMonths: 1,
            showButtonPanel: false,
            showCurrentAtPos: 0
        });
        
    });

</script>

<asp:Panel runat="server" ID="panelGrid" Visible="True">
<div><asp:Label ID="LabelDebug" runat="server" Text="999" /></div>

<div style=" float:right">
<asp:Button ID="btnFillGrid" runat="server" Text="Button" ResourceKey="btnFillGrid" OnClick="btnFillGrid_Click"  CssClass="dnnPrimaryAction" /></div>
<div class="dnnForm" id="form-demo">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblVisitDate" runat="server" CssClass="dnnFormLabel" AssociatedControlID="txtVisitDate" Text="Visit Date" />
            <asp:TextBox ID="txtVisitDate" runat="server" ClientIDMode="Static" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblStatus" runat="server"  AssociatedControlID="ddlStatus" Text="Status" />
            <asp:DropDownList ID="ddlStatus" runat="server">
                <asp:ListItem Text="Show All" Value="-1"></asp:ListItem>
                <asp:ListItem Value="0" Text="Manual Order-Not Yet Entered"></asp:ListItem>
                 <asp:ListItem Value="1" Text="Text Sent - Not Yet Received"></asp:ListItem>
                 <asp:ListItem Value="2" Text="Ready to Fill"></asp:ListItem>
                 <asp:ListItem Value="3" Text="Order Filled"></asp:ListItem>
                <asp:ListItem Value="4" Text="Order Filled-Text Sent"></asp:ListItem>
            </asp:DropDownList>
        </div>	
                <div class="dnnFormItem">
		<dnn:Label runat="server" ID="lblLocation" ControlName="ddlLocations" ResourceKey="lblLocation" />
		<asp:DropDownList ID="ddlLocations" runat="server">
            </asp:DropDownList>
		</div>

    </fieldset>
</div>	
<div class="text-center">
    <asp:Label ID="LabelOrderCount" runat="server" Text=""></asp:Label>
</div>
<asp:GridView ID="GridViewOrders" runat="server" OnRowDataBound="GridViewOrders_RowDataBound" OnRowEditing="GridViewOrders_RowEditing"
     OnRowCommand="GridViewOrders_RowCommand" OnRowDeleting="GridViewOrders_RowDeleting" DataKeyNames="VisitID" OnSorting="GridViewOrders_Sorting" 
     HorizontalAlign="Center" AutoGenerateColumns="False" CssClass="table table-striped">


     <Columns>
<asp:BoundField HeaderText="#" DataField="OrderNumber" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="gridtext text-center" Visible="true"></asp:BoundField>
          <asp:TemplateField HeaderText="" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="170px">
         <ItemTemplate>
           <asp:LinkButton ID="LinkButtonEdit" CausesValidation="False" CssClass="btn btn-lg btn-default" Enabled='<%# DataBinder.Eval(Container.DataItem, "IsEnabled") %>'   
             CommandArgument='<%# Eval("ClientCellPhone") + "-" + Eval("VisitID") %>' 
             CommandName="Edit" runat="server">Process Order</asp:LinkButton>
         </ItemTemplate>
       </asp:TemplateField>
          <asp:TemplateField HeaderText="Send Text" ItemStyle-VerticalAlign="Top" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
         <ItemTemplate>
           <asp:LinkButton ID="LinkButtonSendText" CausesValidation="False"     
             CommandArgument='<%# Eval("ClientCellPhone") + "-" + Eval("VisitID") %>' Visible="false" CommandName="SendText" runat="server"><asp:image ID="imgSendOrderSheet" runat="server" imageurl="~/Icons/Sigma/Email_32x32_Standard.png" AlternateText="Send Order Sheet" /></asp:LinkButton>
             
         </ItemTemplate>
       </asp:TemplateField>
         
          <asp:TemplateField HeaderText="ID" ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
         <ItemTemplate>
             
           <asp:LinkButton ID="LinkButtonLinkToClient" CausesValidation="False"     
             CommandArgument='<%# Eval("ClientID") %>' CommandName="LinkToClient" runat="server">
               <%# Eval("ClientID") %></asp:LinkButton>
             
         </ItemTemplate>
       </asp:TemplateField>

         
         <asp:BoundField HeaderText="Client" DataField="ClientName" ItemStyle-CssClass="gridtext" Visible="true"></asp:BoundField>
        <asp:BoundField HeaderText="Time Entered" DataField="CreatedOnDate" DataFormatString="{0:hh:mm tt}" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="gridtext text-center" />
         <asp:BoundField HeaderText="Bags" DataField="VisitNumBags" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="gridtext text-center" Visible="true"></asp:BoundField>
         <asp:BoundField HeaderText="Status" DataField="OrderStatusCode" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="gridtext text-center" Visible="true"></asp:BoundField>

          <asp:TemplateField HeaderText="Manual Entry" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" ItemStyle-Width="170px">
         <ItemTemplate>
           <asp:LinkButton ID="LinkButtonEnterOrder" CausesValidation="False" CssClass="btn btn-lg btn-default" Visible='<%# DataBinder.Eval(Container.DataItem, "IsPaperOrder") %>'   
             CommandArgument='<%# Eval("VisitID") + "-" + Eval("ClientID")%>' 
             CommandName="EnterOrder" runat="server">Enter Order</asp:LinkButton>
         </ItemTemplate>
       </asp:TemplateField>
       
     </Columns>

</asp:GridView>

    <div>
        <b>OrderStatusCode</b>
        <ul>
        
		<li>0 -	OrderEntered //Paper Process</li>
		<li>1 -	OrderLinkSent  //Client Sent Order Link via Text</li>
		<li>2 -	OrderSubmitted  //Client Submitted Order via Text Web Link - Ready for Fulfillment</li>
		<li>3 -	OrderFilled  //Order Filled - Ready for Pickup</li>
		<li>4 -	OrderWaitingForPickup  //Link Sent to Client - Order filled, ready for pickup</li>
        </ul>
    </div>

</asp:Panel>
<asp:Panel ID="PanelRecord" runat="server" Visible="false">
<div style="width:50%; margin: auto;">
    <asp:Label ID="LabelOrderMessage" runat="server" Text=""></asp:Label>
    </div>
<div runat="server" id="orderDetailsDiv">
    <asp:Label ID="LabelOrderDetails" runat="server" CssClass="orderDetails" Text="Label"></asp:Label>
</div>
<div class="row">
    <asp:Repeater ID="repeater1" runat="server" OnItemDataBound="repeater1_ItemDataBound">

    <ItemTemplate>
        <div class="col-md-6">
            <asp:GridView ID="gvHalf" runat="server" AutoGenerateColumns="false" CssClass="table table-striped bigtext" Width="100%">
                <Columns>
         
         <asp:BoundField HeaderText="Category" DataField="ProductCategory" Visible="true"></asp:BoundField>
        <asp:BoundField HeaderText="Product" DataField="ProductName" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
        <asp:BoundField HeaderText="Quantity" DataField="Quantity" ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
        <asp:TemplateField ItemStyle-CssClass="text-center" HeaderText="Filled">
			<ItemTemplate>
			<asp:CheckBox ID="CheckBoxRepeater" runat="server" />
			</ItemTemplate>
		</asp:TemplateField>        
     </Columns>
            </asp:GridView>
        </div>
    </ItemTemplate>
    

</asp:Repeater>
</div>

<asp:GridView ID="GridViewOrder" runat="server" HorizontalAlign="Center" OnSorting="GridViewOrder_Sorting"
    
    AutoGenerateColumns="False" CssClass="table table-striped">
     <Columns>
         
         <asp:BoundField HeaderText="ProductCategory" DataField="ProductCategory" Visible="true"></asp:BoundField>
        <asp:BoundField HeaderText="Product" DataField="ProductName" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
        <asp:BoundField HeaderText="Quantity" DataField="Quantity" ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
        <asp:TemplateField ItemStyle-CssClass="text-center">                          
			<HeaderTemplate>
                <asp:CheckBox runat="server" ID="chkAll" Visible="false" />
            </HeaderTemplate>
			<ItemTemplate>
					
			<div class="custom-control-lg"><asp:CheckBox ID="CheckBox1" runat="server" Enabled='<%# DataBinder.Eval(Container.DataItem, "IsEnabled") %>'></asp:CheckBox></div>
					
			</ItemTemplate>
		</asp:TemplateField>
     </Columns>

</asp:GridView>
    <div>
        <asp:CheckBox ID="CheckBoxNotifyClientOrderReady" Text="&nbsp;&nbsp;Text Client Order Ready" TextAlign="Right" runat="server" />
    </div>
<div>
    <asp:LinkButton ID="LinkButtonProcessOrder" runat="server" OnClick="LinkButtonProcessOrder_Click" CssClass="btn btn-default btn-lg">Mark Order Filled</asp:LinkButton>
</div>
   
</asp:Panel>
 <asp:HiddenField ID="HiddenFieldVisitID" runat="server" />
<asp:HiddenField ID="HiddenFieldClientCell" runat="server" />

<asp:Panel ID="PanelEnterOrder" runat="server" Visible="false">

<h4>
    <asp:Label ID="LabelorderDetails1" runat="server" Text="Label"></asp:Label>
</h4>

<div>
    <asp:Label ID="LabelRecordCount" runat="server" Text="LabelRecordCount"></asp:Label>
</div>

<asp:GridView ID="GridViewOrderSheet" runat="server" HorizontalAlign="Center" OnSorting="GridViewOrderSheet_Sorting"
    AutoGenerateColumns="False" OnRowDataBound="GridViewOrderSheet_RowDataBound" CssClass="table table-striped">
     <Columns>
        
         <asp:BoundField HeaderText="Category" DataField="ProductCategory" Visible="true"></asp:BoundField>
        <asp:BoundField HeaderText="Product" DataField="ProductName" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
        <asp:BoundField HeaderText="Limit" DataField="Limit" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Order" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">                     
            
            <ItemTemplate>
                <asp:HiddenField ID="HiddenFieldProductID" Value='<%# Eval("ProductID") %>' runat="server" />
                <asp:DropDownList ID="DropDownListQty" runat="server"><asp:ListItem Text="---" Value="0" /></asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:BoundField HeaderText="ProductID" DataField="ProductID" Visible="false"></asp:BoundField>
     </Columns>

</asp:GridView>



<div>
    <asp:Button ID="ButtonSaveOrder" runat="server" Text="Save Order" OnClick="ButtonSaveOrder_Click" CssClass="btn btn-lg btn-default" Visible="false" />

</div>


</asp:Panel>


