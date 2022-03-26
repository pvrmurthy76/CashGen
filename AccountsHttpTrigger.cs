using AutoMapper;
using CashGen.DBContexts;
using CashGen.Entities;
using CashGen.Models;
using CashGen.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CashGen
{
    public class AccountsHttpTrigger
    {
        // Fields
        private readonly ICashGenRepository _cashGenRepository;
        private readonly CashGenContext _context;
        private readonly IMapper _mapper;

        // Methods
        public AccountsHttpTrigger(ICashGenRepository cashGenRepository, CashGenContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
            this._cashGenRepository = cashGenRepository;
        }

        [FunctionName("CreateTransaction")]
        public  async Task<IActionResult> CreateTransaction(
            [HttpTrigger(0, new string[] { "post"}, Route = "accounts")] HttpRequest req,
            ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            TransactionForCreationDto transaction = JsonConvert.DeserializeObject<TransactionForCreationDto>(await((TextReader)new StreamReader(req.Body)).ReadToEndAsync());
            List<Transaction> list1 = ((IQueryable<Transaction>)this._context.Transactions).Where<Transaction>((Expression<Func<Transaction, bool>>)(c => c.StoreId == transaction.StoreId)).ToList<Transaction>();
            if (transaction.DebitCredit == "debit" && transaction.Amount > 0M)
                transaction.Amount *= -1M;
            transaction.Balance = list1.Count != 0 ? 0M : transaction.Amount;
            Transaction transactionEntity = this._mapper.Map<Transaction>((object)transaction);
            transactionEntity.CreatedAt = DateTime.Now;
            transactionEntity.isManual = true;
            this._context.Transactions.Add(transactionEntity);
            this._cashGenRepository.Save();
            Transaction newBalance = ((IQueryable<Transaction>)this._context.Transactions).Where<Transaction>(c => c.StoreId == transactionEntity.StoreId && c.TransactionDate < transactionEntity.TransactionDate && !c.BlockPayment).OrderByDescending<Transaction, DateTime>(c => c.TransactionDate).ThenBy(c => c.Amount).FirstOrDefault<Transaction>();
            List<Transaction> list2 = ((IQueryable<Transaction>)this._context.Transactions).Where<Transaction>(c => c.StoreId == transactionEntity.StoreId && c.TransactionDate > newBalance.TransactionDate && !c.BlockPayment).OrderBy<Transaction, DateTime>(c => c.TransactionDate).ToList<Transaction>();
            System.Decimal num = 0M;
            if (newBalance != null)
                num = newBalance.Balance;
            foreach (Transaction transaction1 in list2)
            {
                num += transaction1.Amount;
                transaction1.Balance = num;
            }
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)true);
        }

        [FunctionName("DeleteTransaction")]
        public IActionResult DeleteTransaction(
           [HttpTrigger(0, new string[] { "delete" }, Route = "accounts/{id}")] HttpRequest req,int id,
           ILogger log)
        {
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            Transaction transaction = ((IQueryable<Transaction>)this._context.Transactions).Where<Transaction>(c => c.TransactionId == id).FirstOrDefault<Transaction>();
            this._context.Transactions.Remove(transaction);
            List<Transaction> transactions = ((IQueryable<Transaction>)this._context.Transactions).Where(c => c.PayoutId == id).ToList<Transaction>();
            foreach (Transaction transaction1 in transactions) { 
                transaction1.PayoutId = 0;
                transaction1.isPaid = false;
            }
            this._cashGenRepository.Save();
            Transaction newBalance = ((IQueryable<Transaction>)this._context.Transactions).Where<Transaction>(c => c.StoreId == transaction.StoreId && c.TransactionDate < transaction.TransactionDate && !c.BlockPayment).OrderByDescending<Transaction, DateTime>(c => c.TransactionDate).ThenByDescending<Transaction, int>(c => c.TransactionId).FirstOrDefault<Transaction>();
            List<Transaction> list = ((IQueryable<Transaction>)this._context.Transactions).Where<Transaction>(c => c.StoreId == transaction.StoreId && c.TransactionDate > newBalance.TransactionDate && !c.BlockPayment).OrderBy<Transaction, DateTime>(c => c.TransactionDate).ToList<Transaction>();
            System.Decimal num = 0M;
            if (newBalance != null)
                num = newBalance.Balance;
            foreach (Transaction transaction2 in list)
            {
                num += transaction2.Amount;
                transaction2.Balance = num;
            }
            this._cashGenRepository.Save();
            return (IActionResult)new OkObjectResult((object)true);
        }

        
        [FunctionName("GetAccount")]
        public IActionResult GetAccount(
            [HttpTrigger(0, new string[] { "get" }, Route = "accounts/{id}")] HttpRequest req,Guid id,
            ILogger log)
        {
            DateTime _start = DateTime.Now.AddDays(-7.0);
            DateTime _end = DateTime.Now;
            if (!string.IsNullOrEmpty(req.Query["start"]))
                _start = Convert.ToDateTime(req.Query["start"]);
            if (!string.IsNullOrEmpty(req.Query["end"]))
                _end = Convert.ToDateTime(req.Query["end"]);
            CashGen.Shared.Generic generic = new CashGen.Shared.Generic();
            _start = generic.ResetTimeToStartOfDay(_start);
            _end = generic.ResetTimeToEndOfDay(_end);
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            return (IActionResult)new OkObjectResult((object)((IQueryable<Transaction>)this._context.Transactions).Where<Transaction>(c => c.StoreId == id && c.TransactionDate >= _start && c.TransactionDate <= _end && !c.BlockPayment).OrderBy(c => c.TransactionDate).ThenBy<Transaction, int>(c => c.TransactionId).ToList<Transaction>());
        }

        [FunctionName("GetAccounts")]
        public IActionResult GetAccounts(
           [HttpTrigger(0, new string[] { "get" }, Route = "accounts")] HttpRequest req, 
           ILogger log)
        {
            DateTime _start = DateTime.Now.AddDays(-7.0);
            DateTime _end = DateTime.Now;
            if (!string.IsNullOrEmpty(req.Query["start"]))
                _start = Convert.ToDateTime(req.Query["start"]);
            if (!string.IsNullOrEmpty(req.Query["end"]))
                _end = Convert.ToDateTime(req.Query["end"]);
            CashGen.Shared.Generic generic = new CashGen.Shared.Generic();
            _start = generic.ResetTimeToStartOfDay(_start);
            _end = generic.ResetTimeToEndOfDay(_end);

            if(!new UsersHttpTrigger(this._cashGenRepository,this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            IEnumerable<Store> accounts = this._cashGenRepository.GetAccounts();
            List<AccountListDto> accountListDtoList = new List<AccountListDto>();
            foreach (Store store in accounts)
            {
                Store item = store;
                AccountListDto accountListDto = new AccountListDto();
                accountListDto.Id = item.Id;
                accountListDto.Title = item.Title;
                accountListDto.GroupName = item.GroupName;
                accountListDto.Balance = item.Balance;
                Transaction transaction1 = ((IQueryable<Transaction>)this._context.Transactions).Where<Transaction>(c => c.StoreId == item.Id && c.TransactionDate < _start && !c.BlockPayment).OrderByDescending(c => c.TransactionDate).ThenByDescending(c => c.TransactionId).FirstOrDefault<Transaction>();
                Transaction transaction2 = ((IQueryable<Transaction>)this._context.Transactions).Where<Transaction>(c => c.StoreId == item.Id && c.TransactionDate <= _end && !c.BlockPayment).OrderByDescending(c => c.TransactionDate).ThenByDescending(c => c.TransactionId).FirstOrDefault<Transaction>();
                accountListDto.OpeningBalance = transaction1 == null ? (transaction2 == null ? 0M : transaction2.Balance) : transaction1.Balance;
                accountListDto.ClosingBalance = transaction2 == null ? 0M : transaction2.Balance;
                accountListDtoList.Add(accountListDto);
            }
            return (IActionResult)new OkObjectResult((object)accountListDtoList);
        }

        [FunctionName("ExportAccounts")]
        public async Task<IActionResult> ExportAccounts(
            [HttpTrigger(0, new string[] { "post" }, Route = "accounts/export")] HttpRequest req,
            ILogger log)
        {
            CashGen.Shared.Generic daG = new CashGen.Shared.Generic();
            if (!new UsersHttpTrigger(this._cashGenRepository, this._context, this._mapper).authenticateUser(((IEnumerable<string>)(object)req.Headers["auth_token"]).FirstOrDefault<string>()).valid)
                return (IActionResult)new BadRequestResult();
            List<string> adminRecipients = new List<string>();
            List<User> users = ((IQueryable<User>)this._context.Users).Where(c => c.Accounting == true && c.UserLevel == "admin").ToList<User>();
            foreach (User user in users)
                adminRecipients.Add(user.Email);
            System.Decimal totalPayments = 0M;
            AccountsForExportDto payload = JsonConvert.DeserializeObject<AccountsForExportDto>(await((TextReader)new StreamReader(req.Body)).ReadToEndAsync());
            DateTime _start = DateTime.Now.AddDays(-7.0);
            DateTime _end = DateTime.Now;
            _start = daG.ResetTimeToStartOfDay(payload.start);
            _end = daG.ResetTimeToEndOfDay(payload.end);
            HSSFWorkbook workbook = new HSSFWorkbook();
            DocumentSummaryInformation summaryInformation1 = PropertySetFactory.CreateDocumentSummaryInformation();
            summaryInformation1.Company = "Cash Generator";
            ((POIDocument)workbook).DocumentSummaryInformation = summaryInformation1;
            SummaryInformation summaryInformation2 = PropertySetFactory.CreateSummaryInformation();
            summaryInformation2.Subject = "Cash Generator";
            ((POIDocument)workbook).SummaryInformation = summaryInformation2;
            ISheet sheet = workbook.CreateSheet("Payouts");
            IRow row1 = sheet.CreateRow(0);
            row1.CreateCell(0).SetCellValue("Group/Store Name");
            row1.CreateCell(1).SetCellValue("Store");
            row1.CreateCell(2).SetCellValue("Opening Balance");
            row1.CreateCell(3).SetCellValue("Closing Balance");
            IEnumerable<Store> accounts = this._cashGenRepository.GetAccounts();
            int count = 0;
            StringBuilder _xml = new StringBuilder();
            _xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            _xml.AppendLine("<Document xmlns=\"urn:iso:std:iso:20022:tech:xsd:pain.001.001.03\">");
            _xml.AppendLine("<CstmrCdtTrfInitn>");
            _xml.AppendLine("<GrpHdr>");
            _xml.AppendLine("<MsgId>COMMERCIAL BANKING ONLINE</MsgId>");
            _xml.AppendLine("<CreDtTm>" + DateTime.UtcNow.ToString("s") + ".000Z</CreDtTm>");
            _xml.AppendLine("<NbOfTxs>[NbOfTxs]</NbOfTxs>");
            _xml.AppendLine("<InitgPty/>");
            _xml.AppendLine("</GrpHdr>");
            int iTxCount = 0;
            foreach (Store store in accounts)
            {
                Store item = store;
                if (payload.stores.Contains(item.Id))
                {
                    Transaction transaction1 = ((IQueryable<Transaction>)this._context.Transactions).Where<Transaction>(c => c.StoreId == item.Id && c.TransactionDate < _start && !c.BlockPayment).OrderByDescending(c => c.TransactionDate).ThenBy<Transaction, int>(c => c.TransactionId).FirstOrDefault<Transaction>();
                    Transaction transaction2 = ((IQueryable<Transaction>)this._context.Transactions).Where<Transaction>(c => c.StoreId == item.Id && c.TransactionDate <= _end && !c.BlockPayment).OrderByDescending(c => c.TransactionDate).ThenBy<Transaction, int>(c => c.TransactionId).FirstOrDefault<Transaction>();
                    System.Decimal num1 = 0M;
                    if (transaction1 != null)
                        num1 = transaction1.Balance;
                    else if (transaction2 != null)
                        num1 = transaction2.Balance;
                    System.Decimal num2 = 0M;
                    if (transaction2 != null)
                        num2 = transaction2.Balance;
                    IRow row2 = sheet.CreateRow(count + 1);
                    if (!string.IsNullOrEmpty(item.GroupName))
                        row2.CreateCell(0).SetCellValue(item.GroupName);
                    else
                        row2.CreateCell(0).SetCellValue(item.Title);
                    if (!string.IsNullOrEmpty(item.GroupName))
                        row2.CreateCell(1).SetCellValue(item.Title);
                    row2.CreateCell(2).SetCellValue(num1.ToString("#.##"));
                    row2.CreateCell(3).SetCellValue(num2.ToString("#.##"));
                    ++count;
                }
            }

            ISheet sheet2 = workbook.CreateSheet("Transactions");
            IRow row3 = sheet2.CreateRow(0);
            row3.CreateCell(0).SetCellValue("Transaction Date");
            row3.CreateCell(1).SetCellValue("Store Name");
            row3.CreateCell(2).SetCellValue("Description");
            row3.CreateCell(3).SetCellValue("Debit/Credit");
            row3.CreateCell(4).SetCellValue("Amount");
            row3.CreateCell(5).SetCellValue("Balance");
            count = 0;

            foreach(Store store in accounts)    
            {
            Store _store = store;
            if (payload.stores.Contains(_store.Id))
            {
                Transaction transaction3 = ((IQueryable<Transaction>) this._context.Transactions).Where<Transaction>(c => c.StoreId == _store.Id && c.TransactionDate <= _end && !c.BlockPayment).OrderByDescending<Transaction, DateTime>(c => c.TransactionDate).ThenByDescending<Transaction, int>(c => c.TransactionId).FirstOrDefault<Transaction>();
                System.Decimal totalPayment = 0M;
                if (transaction3 != null)
                totalPayment = transaction3.Balance;
                if (totalPayment < 0M)
                totalPayment = 0M;
                Transaction trx = new Transaction();
                trx.StoreId = _store.Id;
                trx.Description = "Payout (Period: " + _start.ToString("dd/MM/yyyy") + " - " + _end.ToString("dd/MM/yyyy") + ")";
                trx.OrderId = new Guid("00000000-0000-0000-0000-000000000000");
                trx.ProductId = new Guid("00000000-0000-0000-0000-000000000000");
                trx.DebitCredit = "debit";
                trx.TransactionDate = _end;
                trx.PayoutId = 0;
                trx.isPayout = true;
                trx.isPaid = true;
                trx.isManual = true;
                trx.CreatedAt = DateTime.Now;
                this._context.Transactions.Add(trx);
                this._cashGenRepository.Save();
                List<Transaction> transactions = ((IQueryable<Transaction>) this._context.Transactions).Where<Transaction>(c => c.StoreId == _store.Id && c.TransactionDate >= _start && c.TransactionDate <= _end && c.isPaid == false && !c.BlockPayment).OrderBy<Transaction, DateTime>(c => c.TransactionDate).ThenBy<Transaction, int>(c => c.TransactionId).ToList<Transaction>();
                foreach (Transaction transaction in transactions)
                {
                    transaction.PayoutId = trx.TransactionId;
                    transaction.isPaid = true;
                    IRow row4 = sheet2.CreateRow(count + 1);
                    row4.CreateCell(0).SetCellValue(transaction.TransactionDate.ToString("dd/MM/yyyy HH:mm"));
                    row4.CreateCell(1).SetCellValue(_store.Title);
                    row4.CreateCell(2).SetCellValue(transaction.Description);
                    row4.CreateCell(3).SetCellValue(transaction.DebitCredit);
                    row4.CreateCell(4).SetCellValue(transaction.Amount.ToString("#.##"));
                    row4.CreateCell(5).SetCellValue(transaction.Balance.ToString("#.##"));
                    ++count;
                }
                if (totalPayment > 0M)
                {
                    ++iTxCount;
                    _xml.AppendLine("<PmtInf>");
                    _xml.AppendLine("<PmtInfId>" + iTxCount.ToString() + "</PmtInfId>");
                    _xml.AppendLine("<PmtMtd>TRA</PmtMtd>");
                    _xml.AppendLine("<PmtTpInf>");
                    _xml.AppendLine("<LclInstrm>");
                    _xml.AppendLine("<Prtry>UKBACS</Prtry>");
                    _xml.AppendLine("</LclInstrm>");
                    _xml.AppendLine("</PmtTpInf>");
                    _xml.AppendLine("<ReqdExctnDt>" + DateTime.Now.ToString("yyyy-MM-dd") + "</ReqdExctnDt>");
                    _xml.AppendLine("<Dbtr/>");
                    _xml.AppendLine("<DbtrAcct>");
                    _xml.AppendLine("<Id>");
                    _xml.AppendLine("<Othr>");
                    _xml.AppendLine("<Id>309697-59693668</Id>");
                    _xml.AppendLine("</Othr>");
                    _xml.AppendLine("</Id>");
                    _xml.AppendLine("</DbtrAcct>");
                    _xml.AppendLine("<DbtrAgt>");
                    _xml.AppendLine("<FinInstnId/>");
                    _xml.AppendLine("</DbtrAgt>");
                    _xml.AppendLine("<CdtTrfTxInf>");
                    _xml.AppendLine("<PmtId>");
                    _xml.AppendLine("<EndToEndId>" + trx.TransactionId.ToString() + "</EndToEndId>");
                    _xml.AppendLine("</PmtId>");
                    _xml.AppendLine("<Amt>");
                    _xml.AppendLine("<InstdAmt Ccy=\"GBP\">" + totalPayment.ToString("#.##") + "</InstdAmt>");
                    _xml.AppendLine("</Amt>");
                    _xml.AppendLine("<CdtrAgt>");
                    _xml.AppendLine("<FinInstnId>");
                    _xml.AppendLine("<ClrSysMmbId>");
                    _xml.AppendLine("<MmbId>" + _store.SortCode.Trim() + "</MmbId>");
                    _xml.AppendLine("</ClrSysMmbId>");
                    _xml.AppendLine("</FinInstnId>");
                    _xml.AppendLine("</CdtrAgt>");
                    _xml.AppendLine("<Cdtr>");
                    _xml.AppendLine("<Nm>" + _store.AccountName.Trim() + "</Nm>");
                    _xml.AppendLine("</Cdtr>");
                    _xml.AppendLine("<CdtrAcct>");
                    _xml.AppendLine("<Id>");
                    _xml.AppendLine("<Othr>");
                    _xml.AppendLine("<Id>" + _store.AccountNumber.Trim() + "</Id>");
                    _xml.AppendLine("</Othr>");
                    _xml.AppendLine("</Id>");
                    _xml.AppendLine("</CdtrAcct>");
                    _xml.AppendLine("</CdtTrfTxInf>");
                    _xml.AppendLine("</PmtInf>");
                }
                string filenamex = Guid.NewGuid().ToString() + ".pdf";
                string str1 = "";
                string str2 = "Cash Generator " + _store.Title;
                string line1 = _store.Line1;
                string line2 = _store.Line2;
                string town = _store.Town;
                string postCode = _store.PostCode;
                string str3 = "";
                string str4 = "";
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Document document = new Document())
                    {
                        PdfWriter.GetInstance(document, (Stream) ms);
                        document.Open();
                        Font font1 = (Font) FontFactory.GetFont("Helvetica", 18f, 1, new BaseColor(2, 66, 133));
                        Font font2 = (Font) FontFactory.GetFont("Helvetica", 11f, 0, new BaseColor(51, 51, 51));
                        FontFactory.GetFont("Helvetica", 11f, 1, new BaseColor(51, 51, 51));
                        FontFactory.GetFont("Helvetica", 14f, 1, new BaseColor(51, 51, 51));
                        Font font3 = (Font) FontFactory.GetFont("Helvetica", 9f, 0, new BaseColor(51, 51, 51));
                        Font font4 = (Font) FontFactory.GetFont("Helvetica", 9f, 1, new BaseColor(51, 51, 51));
                     /*   Font font5 = (Font) FontFactory.GetFont("Helvetica", 9f, 0, new BaseColor(51, 51, 51));
                        Font font6 = (Font) FontFactory.GetFont("Helvetica", 9f, 1, new BaseColor(51, 51, 51));
                     */   PdfPCell pdfPcell1 = new PdfPCell();
                        ((Rectangle) pdfPcell1).Border = 0;
                        pdfPcell1.Colspan = 2;
                        pdfPcell1.HorizontalAlignment = 1;
                        pdfPcell1.Phrase = new Phrase("STATEMENT OF ACCOUNT", font1);
                        iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance("https://cdn.shopify.com/s/files/1/0296/1185/1860/files/Cash_Generator_CMYK_ff39c41f-ea00-47f5-ab6a-f3db801c59cd_560x.png?v=1600861351");
                        PdfPCell pdfPcell3 = new PdfPCell();
                        ((Rectangle) pdfPcell3).Border = 0;
                        pdfPcell3.HorizontalAlignment = 1;
                        pdfPcell3.Image = instance;
                        pdfPcell3.PaddingLeft = 160f;
                        pdfPcell3.PaddingRight = 160f;
                        object obj1 = (object) new PdfPTable(1);
                       
                    }
                    byte[] byteArray = ms.ToArray();
                    CloudBlockBlob blockBlobx = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("StorageConnectionString")).CreateCloudBlobClient().GetContainerReference("reports").GetBlockBlobReference(filenamex);
                    ((CloudBlob) blockBlobx).Properties.ContentType = "application/pdf";
                    await blockBlobx.UploadFromByteArrayAsync(byteArray, 0, byteArray.Length);
                    string absoluteUri = ((CloudBlob) blockBlobx).Uri.AbsoluteUri;
                    List<string> stringList = new List<string>();
                    string accountsEmail = _store.AccountsEmail;
                    string[] separator = new string[3]
                    {
                        "\r\n",
                        "\n",
                        ","
                    };
                    foreach (string str7 in accountsEmail.Split(separator, StringSplitOptions.None))
                    {
                        string str8 = str7.Replace(";", "").Replace(",", "").Trim();
                        stringList.Add(str8);
                    }
                    new CashGen.Helpers.Mail().SendMail(new CashGen.Models.Mail()
                    {
                        to = stringList,
                        totalCost = totalPayment.ToString("F2"),
                        periodFrom = _start.ToString("dd/MM/yyyy"),
                        periodTo = _end.ToString("dd/MM/yyyy"),
                        templateId = 22456338,
                        attachment = filenamex,
                        file = byteArray
                    });
                    trx.Amount = totalPayment * -1M;
                    trx.url = absoluteUri;
                    this._cashGenRepository.Save();
                    totalPayments += totalPayment;
                    byteArray = (byte[]) null;
                    blockBlobx = (CloudBlockBlob) null;
                    }
                    Transaction transaction6 = ((IQueryable<Transaction>) this._context.Transactions).Where<Transaction>(c => c.StoreId == _store.Id && c.TransactionDate <= _end && !c.BlockPayment).OrderByDescending<Transaction, DateTime>(c => c.TransactionDate).ThenBy<Transaction, int>(c => c.TransactionId).FirstOrDefault<Transaction>();
                    List<Transaction> list2 = ((IQueryable<Transaction>) this._context.Transactions).Where<Transaction>(c => c.StoreId == _store.Id && c.TransactionDate > _end && !c.BlockPayment).OrderBy<Transaction, DateTime>(c => c.TransactionDate).ThenBy<Transaction, int>(c => c.TransactionId).ToList<Transaction>();
                    System.Decimal num = 0M;
                    if (transaction6 != null)
                        num = transaction6.Balance;
                    foreach (Transaction transaction7 in list2)
                    {
                        num += transaction7.Amount;
                        transaction7.Balance = num;
                    }
                    this._cashGenRepository.Save();
                    trx = (Transaction) null;
                    filenamex = (string) null;
                }
            }
            _xml.AppendLine("</CstmrCdtTrfInitn>");
            _xml.AppendLine("</Document>");
            _xml.Replace("[NbOfTxs]", iTxCount.ToString());
            CloudBlobContainer containerReference = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("StorageConnectionString")).CreateCloudBlobClient().GetContainerReference("reports");
            string filename = Guid.NewGuid().ToString() + ".xls";
            MemoryStream memoryStream = new MemoryStream();
            ((POIDocument)workbook).Write((Stream)memoryStream);
            byte[] result = memoryStream.ToArray();
            CloudBlockBlob blockBlob = containerReference.GetBlockBlobReference(filename);
            await blockBlob.UploadFromByteArrayAsync(result, 0, result.Length);
            Console.WriteLine("Absolute Uri: " + ((CloudBlob)blockBlob).Uri.AbsoluteUri);
            byte[] bytes = Encoding.UTF8.GetBytes(_xml.ToString());
            new CashGen.Helpers.Mail().SendMail(new CashGen.Models.Mail()
            {
                to = adminRecipients,
                totalCost = totalPayments.ToString("F2"),
                periodFrom = _start.ToString("dd/MM/yyyy"),
                periodTo = _end.ToString("dd/MM/yyyy"),
                templateId = 22472434,
                attachment = filename,
                file = result,
                attachment2 = "CashGeneratorPayments_" + _start.ToString("yyyyMMdd") + "_" + _end.ToString("yyyyMMdd") + ".xml",
                file2 = bytes
            });
            return (IActionResult)new OkObjectResult((object)new AccountsForExportResponseDto()
            {
                url = ((CloudBlob)blockBlob).Uri.AbsoluteUri
            });
        }
    
    }
}
