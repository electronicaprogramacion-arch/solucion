using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.Reports.Infraestructure.APIRep.ViewModels
{
    public class QuoteReportViewModel
    {
        public string QuoteNumber { get; set; }
        public DateTime QuoteDate { get; set; }
        public string Subject { get; set; }
        
        // Customer Information
        public CustomerInfo Customer { get; set; }
        
        // Quote Items
        public List<QuoteItemInfo> Items { get; set; }
        
        // Totals
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string TotalAmountText { get; set; }
        
        // Company Information
        public CompanyInfo Company { get; set; }
        
        // Additional Information
        public string Notes { get; set; }
        public string ContactEmail { get; set; }
        public string BaseUrl { get; set; }
        
        public QuoteReportViewModel()
        {
            Items = new List<QuoteItemInfo>();
            Customer = new CustomerInfo();
            Company = new CompanyInfo();
            QuoteDate = DateTime.Now;
            Subject = "Service Request";
            ContactEmail = "Support@ThermoTemp.com";
        }
    }
    
    public class CustomerInfo
    {
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        
        public string FullAddress
        {
            get
            {
                var parts = new List<string>();
                if (!string.IsNullOrEmpty(Address)) parts.Add(Address);
                if (!string.IsNullOrEmpty(City)) parts.Add(City);
                if (!string.IsNullOrEmpty(State)) parts.Add(State);
                if (!string.IsNullOrEmpty(ZipCode)) parts.Add(ZipCode);
                return string.Join(", ", parts);
            }
        }
    }
    
    public class QuoteItemInfo
    {
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string EquipmentType { get; set; }
        public string SerialNumber { get; set; }
        public string EquipmentTypeGroupAndType { get; set; }

        public string FormattedUnitPrice => UnitPrice.ToString("C");
        public string FormattedTotalPrice => TotalPrice.ToString("C");
    }
    
    public class CompanyInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        
        public CompanyInfo()
        {
            Name = "ThermoTemp";
            Address = "813-A Woodcrest Drive • Houston, Texas • 77018-2127";
            Phone = "(713)695-1939";
            Fax = "(713)695-3001";
            Website = "www.thermotemp.com";
        }
    }
}
