using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chambers.Models;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.CompilerServices;

namespace Chambers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        public const int MaxDocSize = 5000000;//in Bytes

        private readonly DocumentContext _context;

        public DocumentsController(DocumentContext context)
        {
            _context = context;
        }

        // GET: api/Documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetDocumentObj>>> GetDocuments()
        {
            List<GetDocumentObj> getDocList = new List<GetDocumentObj>();
            List<Document> docs = await _context.Documents.ToListAsync();
            foreach(var doc in docs)
            {
                GetDocumentObj getDoc = new GetDocumentObj
                {
                    Name = doc.Name,
                    Location = doc.Location,
                    FileSize = doc.Data.Length
                };
                getDocList.Add(getDoc);
            }
             
            return getDocList;
        }

        // GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            var document = await _context.Documents.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            return document;
        }
        
        // PUT: api/Documents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument(int id, Document document)
        {
            if (id != document.Id)
            {
                return BadRequest();
            }

            _context.Entry(document).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Documents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Error>> PostDocument(Document document)
        {
            Error error = new Error
            {
                Status = ErrorStatus.UnknownError,
                Message = UserMessages.UnknowError
            };

            //Check if the doc is  pdf
            if(!document.Name.EndsWith(".pdf"))
            {
                error.Status = ErrorStatus.Nonpdf;
                error.Message = UserMessages.NonPdf;
                return error;
            }

            // Check if the doc is pdf size is less than max size
            if (document.Data.Length > MaxDocSize)
            {
                error.Status = ErrorStatus.MaxPdfSize;
                error.Message = UserMessages.MaxPdfSize;
                return error;
            }

            if (!DocumentExists(document.Name))
            {
                _context.Documents.Add(document);
                await _context.SaveChangesAsync();

                error.Status = ErrorStatus.UploadSuccess;
                error.Message = UserMessages.UploadSuccess;
            }
            else
            {
                error.Status = ErrorStatus.DocExists;
                error.Message = UserMessages.DocExists;
            }
            return error;
        }

        // DELETE: api/Documents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Error>> DeleteDocument(int id)
        {
            Error error = new Error
            {
                Status = ErrorStatus.UnknownError,
                Message = UserMessages.UnknowError
            };

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                error.Status = ErrorStatus.FileNotExistsToDelete;
                error.Message = UserMessages.FileNotExistsToDelete;

                return error;
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();


            error.Status = ErrorStatus.Deleted;
            error.Message = UserMessages.DeleteSuccess;

            return error;
        }

       
        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
        private bool DocumentExists(string filename)
        {
            return _context.Documents.Any(e => e.Name == filename);
        }
    }
}
