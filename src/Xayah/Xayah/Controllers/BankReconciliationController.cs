using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xayah.Data;
using Xayah.Models;
using Xayah.Util;

namespace Xayah.Controllers
{
    public class BankReconciliationController : Controller
    {
        private readonly XayahDbContext _context;
        private readonly IMapper _mapper;

        public BankReconciliationController(XayahDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var transactions = _context.Transactions.AsNoTracking().ToList();
            var transactionsOrdered = from row in transactions orderby row.DatePosted select row;
            var viewModel = _mapper.Map<IEnumerable<TransactionViewModel>>(transactionsOrdered);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ImportFiles()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(IEnumerable<TransactionViewModel> transactions)
        {
            _context.AddRange(_mapper.Map<IEnumerable<Transaction>>(transactions));
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ImportedRegisters(IFormFile[] files)
        {
            if (files == null || files.Length == 0)
                return Content("File(s) not selected");

            var transactions = new OFXReader().ReadTransactionsFromFiles(files);
            var transactionsOrdered = from row in transactions orderby row.DatePosted select row;
            var viewModel = _mapper.Map<IEnumerable<TransactionViewModel>>(transactionsOrdered);

            return View(viewModel);
        }
    }
}