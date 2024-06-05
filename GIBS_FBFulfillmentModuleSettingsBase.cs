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

namespace GIBS.Modules.GIBS_FBFulfillment
{
    public class GIBS_FBFulfillmentModuleSettingsBase : ModuleSettingsBase
    {

        public string ShowCategoryOnFulfillment
        {
            get
            {
                if (Settings.Contains("showCategoryOnFulfillment"))
                    return Settings["showCategoryOnFulfillment"].ToString();
                return "True";

            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(this.TabModuleId, "showCategoryOnFulfillment", value.ToString());
            }

        }
        public string FoodBankClientModuleID
        {
            get
            {
                if (Settings.Contains("foodBankClientModuleID"))
                    return Settings["foodBankClientModuleID"].ToString();
                return "";

            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(this.TabModuleId, "foodBankClientModuleID", value.ToString());
            }

        }

        public string TwilioAccountSid
        {
            get
            {
                if (Settings.Contains("twilioAccountSid"))
                    return Settings["twilioAccountSid"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "twilioAccountSid", value.ToString());
            }
        }

        public string TwilioAuthToken
        {
            get
            {
                if (Settings.Contains("twilioAuthToken"))
                    return Settings["twilioAuthToken"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "twilioAuthToken", value.ToString());
            }
        }

        public string TwilioPhoneNumber
        {
            get
            {
                if (Settings.Contains("twilioPhoneNumber"))
                    return Settings["twilioPhoneNumber"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "twilioPhoneNumber", value.ToString());
            }
        }
        //ProcessOrderLayOut
        public string ProcessOrderLayOut
        {
            get
            {
                if (Settings.Contains("processOrderLayOut"))
                    return Settings["processOrderLayOut"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "processOrderLayOut", value.ToString());
            }
        }
    }
}