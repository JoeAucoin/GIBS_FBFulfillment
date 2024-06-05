<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="GIBS.Modules.GIBS_FBFulfillment.Settings" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>


	<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>
	<fieldset>

	 <div class="dnnFormItem">
            <dnn:label id="lblShowCategoryOnFulfillment" runat="server" suffix=":" controlname="cbxShowCategoryOnFulfillment" />
           <asp:CheckBox ID="cbxShowCategoryOnFulfillment" runat="server" />
        </div>	

	 <div class="dnnFormItem">
            <dnn:label id="lblFoodBankClientModuleID" runat="server" suffix=":" controlname="drpModuleID" />
            <asp:dropdownlist id="drpModuleID" Runat="server" Width="325" 
				CssClass="NormalTextBox"></asp:dropdownlist>
        </div>	

        <div class="dnnFormItem">
            <dnn:Label ID="lblTwilioAccountSid" runat="server" controlname="txtTwilioAccountSid" suffix=":" /> 
 
            <asp:TextBox ID="txtTwilioAccountSid" runat="server" />
        </div>
	 <div class="dnnFormItem">
            <dnn:label ID="lblTwilioAuthToken" runat="server" controlname="txtTwilioAuthToken" suffix=":" />
            <asp:TextBox ID="txtTwilioAuthToken" runat="server" />
        </div>
		
      <div class="dnnFormItem">
            <dnn:label ID="lblTwilioPhoneNumber" runat="server" controlname="txtTwilioPhoneNumber" suffix=":" />
            <asp:TextBox ID="txtTwilioPhoneNumber" runat="server" />
        </div>
      <div class="dnnFormItem">
            <dnn:label ID="lblProcessOrderLayOut" runat="server" controlname="ddlProcessOrderLayOut" suffix=":" />
            <asp:DropDownList ID="ddlProcessOrderLayOut" runat="server">
			<asp:ListItem Text="One Column" Value="1"></asp:ListItem>
			<asp:ListItem Text="Two Column" Value="2"></asp:ListItem>
		</asp:DropDownList>				
        </div>

    </fieldset>


