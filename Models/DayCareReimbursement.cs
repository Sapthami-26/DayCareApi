using System;

namespace DayCareApi.Models
{
    public class DayCareReimbursement
    {
        public int? RID { get; set; }
        public int? DCID { get; set; }
        public string? NameOfChild { get; set; }
        public DateTime? DOB { get; set; }
        public int? AgeYear { get; set; }
        public int? AgeMonth { get; set; }
        public string? NameOfDayCare { get; set; }
        public string? AdmissionType { get; set; }
        public string? AdmissionTypeOthers { get; set; }
        public decimal? DayCareFee { get; set; }
        public string? BillType { get; set; }
        public int? NoOfInvoice { get; set; }
        public DateTime? InvoiceDate1 { get; set; }
        public DateTime? InvoiceDate2 { get; set; }
        public DateTime? InvoiceDate3 { get; set; }
        public string? ModeOfPayment { get; set; }
        public string? ModeOfPaymentOthers { get; set; }
        public bool? HardCopy { get; set; }
        public string? TermDuration { get; set; }
    }
}