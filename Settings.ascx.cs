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
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace GIBS.Modules.GIBS_FBFulfillment
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Settings class manages Module Settings
    /// 
    /// Typically your settings control would be used to manage settings for your module.
    /// There are two types of settings, ModuleSettings, and TabModuleSettings.
    /// 
    /// ModuleSettings apply to all "copies" of a module on a site, no matter which page the module is on. 
    /// 
    /// TabModuleSettings apply only to the current module on the current page, if you copy that module to
    /// another page the settings are not transferred.
    /// 
    /// If you happen to save both TabModuleSettings and ModuleSettings, TabModuleSettings overrides ModuleSettings.
    /// 
    /// Below we have some examples of how to access these settings but you will need to uncomment to use.
    /// 
    /// Because the control inherits from GIBS_FBFulfillmentSettingsBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Settings : GIBS_FBFulfillmentModuleSettingsBase
    {
        #region Base Method Implementations

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// LoadSettings loads the settings from the Database and displays them
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void LoadSettings()
        {
            try
            {
                if (Page.IsPostBack == false)
                {
                    BindModules();

                    //Check for existing settings and use those on this page
                    if (TwilioAccountSid != null)
                    {
                        txtTwilioAccountSid.Text = TwilioAccountSid;
                    }

                    if (TwilioAuthToken != null)
                    {
                        txtTwilioAuthToken.Text = TwilioAuthToken;
                    }

                    if (TwilioPhoneNumber != null)
                    {
                        txtTwilioPhoneNumber.Text = TwilioPhoneNumber;
                    }
                    if (ProcessOrderLayOut != null)
                    {
                        ddlProcessOrderLayOut.SelectedValue = ProcessOrderLayOut;
                       
                    }
                    if(ShowCategoryOnFulfillment != null)
                    {
                        cbxShowCategoryOnFulfillment.Checked = Convert.ToBoolean(ShowCategoryOnFulfillment.ToString());
                    }


                    //Settings["SettingName"]

                    /* uncomment to load saved settings in the text boxes
                    if(Settings.Contains("Setting1"))
                        txtSetting1.Text = Settings["Setting1"].ToString();
			
                    if (Settings.Contains("Setting2"))
                        txtSetting2.Text = Settings["Setting2"].ToString();

                    */

                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        private void BindModules()
        {

            DotNetNuke.Entities.Modules.ModuleController mc = new ModuleController();
            ArrayList existMods = mc.GetModulesByDefinition(this.PortalId, "GIBS Food Bank Client Manager");

            foreach (DotNetNuke.Entities.Modules.ModuleInfo mi in existMods)
            {
                if (!mi.IsDeleted)
                {

                    ListItem objListItemPage = new ListItem();
                    objListItemPage.Value = mi.TabID.ToString() + "-" + mi.ModuleID.ToString();
                    objListItemPage.Text = mi.ModuleTitle.ToString();

                    drpModuleID.Items.Add(objListItemPage);

                }
            }

            drpModuleID.Items.Insert(0, new ListItem(Localization.GetString("SelectModule", this.LocalResourceFile), "-1"));

            if (Settings.Contains("foodBankClientModuleID"))
            {
                drpModuleID.SelectedValue = Settings["foodBankClientModuleID"].ToString();
                //   LabelDebug.Text = Settings["foodBankClientModuleID"].ToString();

            }

        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            try
            {
                var modules = new ModuleController();
                ShowCategoryOnFulfillment = cbxShowCategoryOnFulfillment.Checked.ToString();
                TwilioAccountSid = txtTwilioAccountSid.Text.ToString();
                TwilioAuthToken = txtTwilioAuthToken.Text.ToString();
                TwilioPhoneNumber = txtTwilioPhoneNumber.Text.ToString();
                ProcessOrderLayOut = ddlProcessOrderLayOut.SelectedValue.ToString();
                FoodBankClientModuleID = drpModuleID.SelectedValue.ToString();
                //the following are two sample Module Settings, using the text boxes that are commented out in the ASCX file.
                //module settings
                //modules.UpdateModuleSetting(ModuleId, "Setting1", txtSetting1.Text);
                //modules.UpdateModuleSetting(ModuleId, "Setting2", txtSetting2.Text);

                //tab module settings
                //modules.UpdateTabModuleSetting(TabModuleId, "Setting1",  txtSetting1.Text);
                //modules.UpdateTabModuleSetting(TabModuleId, "Setting2",  txtSetting2.Text);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion
    }
}