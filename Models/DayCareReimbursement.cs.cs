using System;

namespace DayCareApi.Models
{
    /// <summary>
    /// Represents a single day care reimbursement entry for a child.
    /// Corrected to include all parameters listed in the documentation.
    /// </summary>
    public class DayCareReimbursement
    {
        // Properties for identifying existing records
        public int? RID { get; set; }
        public int? DCID { get; set; }

        // Child and Day Care Details
        public string? NameOfChild { get; set; }
        public DateTime DOB { get; set; }
        public int AgeYear { get; set; }
        public int AgeMonth { get; set; }
        public string? NameOfDayCare { get; set; }
        public string? AdmissionType { get; set; }
        public string? AdmissionTypeOthers { get; set; }
        public decimal DayCareFee { get; set; }
        public string? TermDuration { get; set; }

        // Billing Details
        public string? BillType { get; set; }
        public int NoOfInvoice { get; set; }
        public DateTime? InvoiceDate1 { get; set; }
        public DateTime? InvoiceDate2 { get; set; }
        public DateTime? InvoiceDate3 { get; set; }
        public string? ModeOfPayment { get; set; }
        public string? ModeOfPaymentOthers { get; set; }
        public bool HardCopy { get; set; }
    }
}
