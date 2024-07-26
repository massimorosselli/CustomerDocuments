using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomerDocuments.Models;
using Microsoft.CodeAnalysis.CSharp;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iTextDocument = iText.Layout.Document;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CustomerDocuments.Controllers
{
    public class CustomerDocumentController : Controller
    {
        private readonly CustomerContext _context;

        public CustomerDocumentController(CustomerContext context)
        {
            _context = context;
        }

        // GET: CustomerDocument
        public IActionResult Index()
        {
            CustomerDocument customerDocument = new CustomerDocument()
            {
                Documents = _context.Documents.ToList(),
                Placeholders = _context.Placeholders.GroupBy(p => p.UserId).Select(g => g.First()).ToList()
            };

            return View(customerDocument);
        }

        [HttpPost]
        public IActionResult CreateDocument(string documentFileName, int placeholderUserId)
        {
            try
            {
                // Utilizza i valori selezionati nei dropdownlist
                var selectedDocument = _context.Documents.Where(d => d.FileName == documentFileName).Select(d => d.Content).FirstOrDefault();
                var placeholderList = _context.Placeholders.Where(p => p.UserId == placeholderUserId).ToDictionary(p => p.Name, p => p.Value);

                foreach (var place in placeholderList)
                {
                    selectedDocument = selectedDocument.Replace(place.Key, place.Value);
                }

                selectedDocument = selectedDocument.Replace("{Date}", DateTime.Today.ToString("yyyy-MM-dd"));          

                using (var memoryStream = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(memoryStream);
                    PdfDocument pdf = new PdfDocument(writer);
                    iTextDocument document = new iTextDocument(pdf);

                    document.Add(new Paragraph(selectedDocument));

                    document.Close();

                    UserTransaction transaction = new UserTransaction()
                    {
                        DocumentId = _context.Documents.Where(d => d.FileName == documentFileName).Select(d => d.DocumentId).FirstOrDefault(),
                        UserId = placeholderUserId,
                        TransactionDate = DateTime.Now
                    };

                    _context.Add(transaction);
                    _context.SaveChanges();

                    return File(memoryStream.ToArray(), "application/pdf", "document.pdf");

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
    }
}
