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

            if (int.TryParse(billType, out int billTypeInt))
            {
                return billTypeInt;
            }

            switch (billType)
            {
                case "Monthly": return 1;
                case "Quarterly": return 2;
                case "Half Yearly": return 3;
                case "Annually": return 4;
                default: return 0;
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
                    cmd.Parameters.AddWithValue("@DOB", model.DOB);
                    cmd.Parameters.AddWithValue("@AgeYear", model.AgeYear ?? 0);
                    cmd.Parameters.AddWithValue("@AgeMonth", model.AgeMonth ?? 0);
                    cmd.Parameters.AddWithValue("@NameOfDayCare", model.NameOfDayCare);
                    cmd.Parameters.AddWithValue("@AdmissionType", model.AdmissionType);
                    cmd.Parameters.AddWithValue("@AdmissionTypeOthers", model.AdmissionTypeOthers != null ? (object)model.AdmissionTypeOthers : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DayCareFee", model.DayCareFee ?? 0);
                    cmd.Parameters.AddWithValue("@BillType", (object)MapBillTypeToInt(model.BillType));
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
                    cmd.Parameters.AddWithValue("@DOB", model.DOB);
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
                    cmd.Parameters.AddWithValue("@BillType", (object)MapBillTypeToInt(model.BillType));
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
                                RID = reader["RID"] as int?,
                                DCID = reader["DCID"] as int?,
                                NameOfChild = reader["NameOfChild"] as string,
                                DOB = Convert.ToDateTime(reader["DOB"]),
                                AgeYear = reader["AgeYear"] as int?,
                                AgeMonth = reader["AgeMonth"] as int?,
                                NameOfDayCare = reader["NameOfDayCare"] as string,
                                AdmissionType = reader["AdmissionType"] as string,
                                AdmissionTypeOthers = reader["AdmissionTypeOthers"] as string,
                                DayCareFee = reader["DayCareFee"] as decimal?,
                                TermDuration = reader["TermDuration"] as string,
                                BillType = reader["BillType"] as string,
                                NoOfInvoice = reader["NoOfInvoice"] as int?,
                                InvoiceDate1 = reader["InvoiceDate1"] as DateTime?,
                                InvoiceDate2 = reader["InvoiceDate2"] as DateTime?,
                                InvoiceDate3 = reader["InvoiceDate3"] as DateTime?,
                                ModeOfPayment = reader["ModeOfPayment"] as string,
                                ModeOfPaymentOthers = reader["ModeOfPaymentOthers"] as string,
                                HardCopy = reader["HardCopy"] as bool?
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