using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using DayCareApi.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DayCareApi.Repositories
{
    public class DayCareRepository : IDayCareRepository
    {
        private readonly string? _connectionString;

        public DayCareRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> AddChildAsync(DayCareReimbursement model, int initiatorEmpId)
        {
            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DayCareSupportReimbursement_InsertUpdateDataInChild", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var quarter = await GetQuarterAsync(DateTime.Now, DateTime.Now);
                    var year = (quarter == 4) ? DateTime.Now.Year - 1 : DateTime.Now.Year;

                    // Parameters based on documentation
                    cmd.Parameters.AddWithValue("@InstanceID", 0);
                    cmd.Parameters.AddWithValue("@InitiatorMEmpID", initiatorEmpId);
                    cmd.Parameters.AddWithValue("@GroupMGID", 0);
                    cmd.Parameters.AddWithValue("@TeamMGID", 0);
                    cmd.Parameters.AddWithValue("@WFStatus", 0);
                    cmd.Parameters.AddWithValue("@InitiatedOn", DateTime.Now);
                    cmd.Parameters.AddWithValue("@RID", 0);
                    cmd.Parameters.AddWithValue("@DCID", 0);
                    cmd.Parameters.AddWithValue("@NameOfChild", model.NameOfChild);
                    cmd.Parameters.AddWithValue("@DOB", model.DOB);
                    cmd.Parameters.AddWithValue("@AgeYear", model.AgeYear);
                    cmd.Parameters.AddWithValue("@AgeMonth", model.AgeMonth);
                    cmd.Parameters.AddWithValue("@NameOfDayCare", model.NameOfDayCare);
                    cmd.Parameters.AddWithValue("@AdmissionType", model.AdmissionType);
                    cmd.Parameters.AddWithValue("@AdmissionTypeOthers", model.AdmissionTypeOthers != null ? (object)model.AdmissionTypeOthers : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DayCareFee", model.DayCareFee);
                    cmd.Parameters.AddWithValue("@BillType", model.BillType);
                    cmd.Parameters.AddWithValue("@NoOfInvoice", model.NoOfInvoice);
                    cmd.Parameters.AddWithValue("@InvoiceDate1", model.InvoiceDate1.HasValue ? (object)model.InvoiceDate1.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@InvoiceDate2", model.InvoiceDate2.HasValue ? (object)model.InvoiceDate2.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@InvoiceDate3", model.InvoiceDate3.HasValue ? (object)model.InvoiceDate3.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@InvoiceDate4", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModeOfPayment", model.ModeOfPayment);
                    cmd.Parameters.AddWithValue("@ModeOfPaymentOthers", model.ModeOfPaymentOthers != null ? (object)model.ModeOfPaymentOthers : DBNull.Value);
                    cmd.Parameters.AddWithValue("@HardCopy", model.HardCopy);
                    cmd.Parameters.AddWithValue("@TermDuration", model.TermDuration);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@FileIndexID", "");
                    cmd.Parameters.AddWithValue("@FileName", "");
                    cmd.Parameters.AddWithValue("@FilePath", "");
                    cmd.Parameters.AddWithValue("@Quarter", quarter);
                    cmd.Parameters.AddWithValue("@FinYear", year);
                    cmd.Parameters.AddWithValue("@IsDraftable", 1);
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@Choice", 1);  // Insert

                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> UpdateChildAsync(DayCareReimbursement model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DayCareSupportReimbursement_InsertUpdateDataInChild", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var quarter = await GetQuarterAsync(DateTime.Now, DateTime.Now);
                    var year = (quarter == 4) ? DateTime.Now.Year - 1 : DateTime.Now.Year;

                    cmd.Parameters.AddWithValue("@RID", model.RID);
                    cmd.Parameters.AddWithValue("@DCID", model.DCID);
                    cmd.Parameters.AddWithValue("@NameOfChild", model.NameOfChild);
                    cmd.Parameters.AddWithValue("@DOB", model.DOB);
                    cmd.Parameters.AddWithValue("@AgeYear", model.AgeYear);
                    cmd.Parameters.AddWithValue("@AgeMonth", model.AgeMonth);
                    cmd.Parameters.AddWithValue("@NameOfDayCare", model.NameOfDayCare);
                    cmd.Parameters.AddWithValue("@AdmissionType", model.AdmissionType);
                    cmd.Parameters.AddWithValue("@AdmissionTypeOthers", model.AdmissionTypeOthers != null ? (object)model.AdmissionTypeOthers : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DayCareFee", model.DayCareFee);
                    cmd.Parameters.AddWithValue("@NoOfInvoice", model.NoOfInvoice);
                    cmd.Parameters.AddWithValue("@InvoiceDate1", model.InvoiceDate1.HasValue ? (object)model.InvoiceDate1.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@InvoiceDate2", model.InvoiceDate2.HasValue ? (object)model.InvoiceDate2.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@InvoiceDate3", model.InvoiceDate3.HasValue ? (object)model.InvoiceDate3.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModeOfPayment", model.ModeOfPayment);
                    cmd.Parameters.AddWithValue("@ModeOfPaymentOthers", model.ModeOfPaymentOthers != null ? (object)model.ModeOfPaymentOthers : DBNull.Value);
                    cmd.Parameters.AddWithValue("@HardCopy", model.HardCopy);
                    cmd.Parameters.AddWithValue("@TermDuration", model.TermDuration);
                    cmd.Parameters.AddWithValue("@BillType", model.BillType);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Quarter", quarter);
                    cmd.Parameters.AddWithValue("@FinYear", year);
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@Choice", 2);  // Update

                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> DeleteChildAsync(int rid, int dcid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DayCareSupportReimbursement_DeleteDataInChild", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RID", rid);
                    cmd.Parameters.AddWithValue("@DCID", dcid);

                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> UpdateDraftStatusAsync(int rid, int dcid, int choice)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DayCareSupportReimbursement_UpdateIsDraftStatus", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RID", rid);
                    cmd.Parameters.AddWithValue("@DCID", dcid);
                    cmd.Parameters.AddWithValue("@Choice", choice);

                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> GetQuarterAsync(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DayCareSupportReimbursement_GetQuarter", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    await connection.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }

        public async Task<IEnumerable<DayCareReimbursement>> GetByEmployeeIdAsync(int initiatorEmpId)
        {
            // Assumed implementation as the document does not specify a GET method
            await Task.CompletedTask;
            return new List<DayCareReimbursement>();
        }
    }
}
