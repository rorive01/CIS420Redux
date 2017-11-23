﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIS420Redux.Models;
using CIS420Redux.Models.ViewModels.Advisor;

namespace CIS420Redux.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Dashboard()
        {
            return View();
        }


        public ActionResult Search()
        {
            return View();
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult StudentTodos()
        {
            return View(db.Events.ToList());
        }

        public ActionResult ComplianceDocs()
        {
            var clinicalCompliances = db.ClincalCompliances.ToList();
            //var clinicalCompliances = db.ClincalCompliances.Where(c => c.Student.Id == student.Id).ToList();

            //var isStudentCompliant = db.ClincalCompliances.Any(cc => cc.Student.Id == student.Id && cc.IsCompliant == false);
            var ccdocidList = clinicalCompliances.Select(d => d.DocumentId).ToList();
            var documents = db.Documents.Where(d => ccdocidList.Contains(d.Id)).ToList();

            var viewModel = clinicalCompliances.Select(c => new AdvisorCCIndexViewModel
            {
                ExpirationDate = c.ExpirationDate,
                DocumentId = c.DocumentId,
                IsComplaint = c.IsCompliant,
                ID = c.ID,
                Type = c.Type,
                StudentNumber = c.Student.StudentNumber,
                FirstName = c.Student.FirstName,
                LastName = c.Student.LastName,
                IsExpired = c.ExpirationDate <= DateTime.Today ? true : false
            });

            foreach (var doc in documents)
            {
                viewModel.FirstOrDefault(v => v.DocumentId == doc.Id).Document = doc;
            }


            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
