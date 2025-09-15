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

        private int MapBillTypeToInt(string? billType)
        {
            if (string.IsNullOrEmpty(billType))
            {
                return 0;
            }

            // Check if the string is already a number
            if (int.TryParse(billType, out int billTypeInt))
            {
                return billTypeInt;
            }

            switch (billType.ToLower()) // Use ToLower() for case-insensitive matching
            {
                case "monthly": return 1;
                case "quarterly": return 2;
                case "half yearly": return 3;
                case "annually": return 4;
                default: return 0;
            }
        }

        private string MapBillTypeToString(int? billTypeInt)
        {
            if (!billTypeInt.HasValue)
            {
                return string.Empty;
            }

            switch (billTypeInt.Value)
            {
                case 1: return "Monthly";
                case 2: return "Quarterly";
                case 3: return "Half Yearly";
                case 4: return "Annually";
                default: return string.Empty;
            }
        }
        
        public async Task<int> AddChildAsync(DayCareReimbursement model, int initiatorEmpId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DayCareSupportReimbursement_InsertUpdateDataInChild", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var quarter = await GetQuarterAsync(DateTime.Now, DateTime.Now);
                    var year = (quarter == 4) ? DateTime.Now.Year - 1 : DateTime.Now.Year;

                    cmd.Parameters.AddWithValue("@InstanceID", 0);
                    cmd.Parameters.AddWithValue("@InitiatorMEmpID", initiatorEmpId);
                    cmd.Parameters.AddWithValue("@GroupMGID", 0);
                    cmd.Parameters.AddWithValue("@TeamMGID", 0);
                    cmd.Parameters.AddWithValue("@WFStatus", 0);
                    cmd.Parameters.AddWithValue("@InitiatedOn", DateTime.Now);
                    cmd.Parameters.AddWithValue("@RID", 0);
                    cmd.Parameters.AddWithValue("@DCID", 0);
                    cmd.Parameters.AddWithValue("@NameOfChild", model.NameOfChild);
                    cmd.Parameters.AddWithValue("@DOB", model.DOB.HasValue ? (object)model.DOB.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@AgeYear", model.AgeYear ?? 0);
                    cmd.Parameters.AddWithValue("@AgeMonth", model.AgeMonth ?? 0);
                    cmd.Parameters.AddWithValue("@NameOfDayCare", model.NameOfDayCare);
                    cmd.Parameters.AddWithValue("@AdmissionType", model.AdmissionType);
                    cmd.Parameters.AddWithValue("@AdmissionTypeOthers", model.AdmissionTypeOthers != null ? (object)model.AdmissionTypeOthers : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DayCareFee", model.DayCareFee ?? 0);
                    cmd.Parameters.AddWithValue("@BillType", MapBillTypeToInt(model.BillType));
                    cmd.Parameters.AddWithValue("@NoOfInvoice", model.NoOfInvoice ?? 0);
                    cmd.Parameters.AddWithValue("@InvoiceDate1", model.InvoiceDate1.HasValue ? (object)model.InvoiceDate1.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@InvoiceDate2", model.InvoiceDate2.HasValue ? (object)model.InvoiceDate2.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@InvoiceDate3", model.InvoiceDate3.HasValue ? (object)model.InvoiceDate3.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@InvoiceDate4", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModeOfPayment", model.ModeOfPayment);
                    cmd.Parameters.AddWithValue("@ModeOfPaymentOthers", model.ModeOfPaymentOthers != null ? (object)model.ModeOfPaymentOthers : DBNull.Value);
                    cmd.Parameters.AddWithValue("@HardCopy", model.HardCopy ?? false);
                    cmd.Parameters.AddWithValue("@TermDuration", model.TermDuration);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@FileIndexID", "");
                    cmd.Parameters.AddWithValue("@FileName", "");
                    cmd.Parameters.AddWithValue("@FilePath", "");
                    cmd.Parameters.AddWithValue("@Quarter", quarter);
                    cmd.Parameters.AddWithValue("@FinYear", year);
                    cmd.Parameters.AddWithValue("@IsDraftable", 1);
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@Choice", 1);
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
                    
                    cmd.Parameters.AddWithValue("@RID", model.RID.HasValue ? (object)model.RID.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DCID", model.DCID.HasValue ? (object)model.DCID.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@NameOfChild", model.NameOfChild);
                    cmd.Parameters.AddWithValue("@DOB", model.DOB.HasValue ? (object)model.DOB.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@AgeYear", model.AgeYear ?? 0);
                    cmd.Parameters.AddWithValue("@AgeMonth", model.AgeMonth ?? 0);
                    cmd.Parameters.AddWithValue("@NameOfDayCare", model.NameOfDayCare);
                    cmd.Parameters.AddWithValue("@AdmissionType", model.AdmissionType);
                    cmd.Parameters.AddWithValue("@AdmissionTypeOthers", model.AdmissionTypeOthers != null ? (object)model.AdmissionTypeOthers : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DayCareFee", model.DayCareFee ?? 0);
                    cmd.Parameters.AddWithValue("@NoOfInvoice", model.NoOfInvoice ?? 0);
                    cmd.Parameters.AddWithValue("@InvoiceDate1", model.InvoiceDate1.HasValue ? (object)model.InvoiceDate1.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@InvoiceDate2", model.InvoiceDate2.HasValue ? (object)model.InvoiceDate2.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@InvoiceDate3", model.InvoiceDate3.HasValue ? (object)model.InvoiceDate3.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModeOfPayment", model.ModeOfPayment);
                    cmd.Parameters.AddWithValue("@ModeOfPaymentOthers", model.ModeOfPaymentOthers != null ? (object)model.ModeOfPaymentOthers : DBNull.Value);
                    cmd.Parameters.AddWithValue("@HardCopy", model.HardCopy ?? false);
                    cmd.Parameters.AddWithValue("@TermDuration", model.TermDuration);
                    cmd.Parameters.AddWithValue("@BillType", MapBillTypeToInt(model.BillType));
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Quarter", quarter);
                    cmd.Parameters.AddWithValue("@FinYear", year);
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    cmd.Parameters.AddWithValue("@Choice", 2);
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

        public async Task<IEnumerable<DayCareReimbursement>> GetByEmployeeIdAsync(int initiatorEmpId)
        {
            var reimbursements = new List<DayCareReimbursement>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DayCareSupportReimbursement_GetDataByDayCareData", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmpID", initiatorEmpId); 
                    await connection.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            reimbursements.Add(new DayCareReimbursement
                            {
                                RID = reader.IsDBNull("RID") ? null : (int?)reader["RID"],
                                DCID = reader.IsDBNull("DCID") ? null : (int?)reader["DCID"],
                                NameOfChild = reader.IsDBNull("NameOfChild") ? null : reader.GetString("NameOfChild"),
                                DOB = reader.IsDBNull("DOB") ? null : (DateTime?)reader.GetDateTime("DOB"),
                                AgeYear = reader.IsDBNull("AgeYear") ? null : (int?)reader["AgeYear"],
                                AgeMonth = reader.IsDBNull("AgeMonth") ? null : (int?)reader["AgeMonth"],
                                NameOfDayCare = reader.IsDBNull("NameOfDayCare") ? null : reader.GetString("NameOfDayCare"),
                                AdmissionType = reader.IsDBNull("AdmissionType") ? null : reader.GetString("AdmissionType"),
                                AdmissionTypeOthers = reader.IsDBNull("AdmissionTypeOthers") ? null : reader.GetString("AdmissionTypeOthers"),
                                DayCareFee = reader.IsDBNull("DayCareFee") ? null : (decimal?)reader.GetDecimal("DayCareFee"),
                                TermDuration = reader.IsDBNull("TermDuration") ? null : reader.GetString("TermDuration"),
                                BillType = MapBillTypeToString(reader["BillType"] as int?), // Corrected line
                                NoOfInvoice = reader.IsDBNull("NoOfInvoice") ? null : (int?)reader["NoOfInvoice"],
                                InvoiceDate1 = reader.IsDBNull("InvoiceDate1") ? null : (DateTime?)reader.GetDateTime("InvoiceDate1"),
                                InvoiceDate2 = reader.IsDBNull("InvoiceDate2") ? null : (DateTime?)reader.GetDateTime("InvoiceDate2"),
                                InvoiceDate3 = reader.IsDBNull("InvoiceDate3") ? null : (DateTime?)reader.GetDateTime("InvoiceDate3"),
                                ModeOfPayment = reader.IsDBNull("ModeOfPayment") ? null : reader.GetString("ModeOfPayment"),
                                ModeOfPaymentOthers = reader.IsDBNull("ModeOfPaymentOthers") ? null : reader.GetString("ModeOfPaymentOthers"),
                                HardCopy = reader.IsDBNull("HardCopy") ? null : (bool?)reader.GetBoolean("HardCopy")
                            });
                        }
                    }
                }
            }
            return reimbursements;
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
        
        public Task<IEnumerable<string>> GetBillTypesAsync()
        {
            var billTypes = new List<string> { "Monthly", "Quarterly", "Half Yearly", "Annually" };
            return Task.FromResult<IEnumerable<string>>(billTypes);
        }
    }
}