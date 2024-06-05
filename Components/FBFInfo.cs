using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GIBS.Modules.GIBS_FBFulfillment.Components
{
    public class FBFInfo
    {
        private int productID;
        private string productName;
        private string productCategory;
        private int limit;
        private int visitID;
        private int quantity;
        private int clientID;
        private int orderStatusCode;
        //      OrderStatusCode
        //0	OrderEntered //Paper Process
        //1	OrderLinkSent  //Client Sent Order Link via Text
        //2	OrderSubmitted  //Client Submitted Order via Text Web Link
        //3	Order Currently Being Processed  //Order Ready in process of Fulfillment
        //4	OrderFilled  //Order filled, ready for pickup
        private string clientName;
        private string clientLanguage;
        private DateTime visitDate;
        private int visitNumBags;
        private bool isEnabled;
        private bool isPaperOrder;
        private DateTime createdOnDate;
        private string clientCellPhone;
        private int orderNumber;
        private string visitNotes;

        public FBFInfo()
        {

        }

        public string VisitNotes
        {
            get { return visitNotes; }
            set { visitNotes = value; }
        }

        public int OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
        }
        public string ClientCellPhone
        {
            get { return clientCellPhone; }
            set { clientCellPhone = value; }
        }

        public DateTime CreatedOnDate
        {
            get { return createdOnDate; }
            set { createdOnDate = value; }
        }

        public bool IsPaperOrder
        { 
            get { return isPaperOrder; } 
            set { isPaperOrder = value; }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
        public string ClientName
        {
            get { return clientName; }
            set { clientName = value; }
        }
        public string ClientLanguage
        {
            get { return clientLanguage; }
            set { clientLanguage = value; }
        }
        public DateTime VisitDate
        {
            get { return visitDate; }
            set { visitDate = value; }
        }

        public int VisitNumBags
        {
            get { return visitNumBags; }
            set { visitNumBags = value; }
        }
        public int OrderStatusCode
        {
            get { return orderStatusCode; }
            set { orderStatusCode = value; }
        }

        public int ClientID
        {
            get { return clientID; }
            set { clientID = value; }
        }
        public int VisitID
        {
            get { return visitID; }
            set { visitID = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        public string ProductCategory
        {
            get { return productCategory; }
            set { productCategory = value; }
        }
        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }
    }
}