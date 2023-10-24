using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using A2286.Models;

namespace A2286.Controllers
{
    public class CustomersController : Controller
    {
        private CustomerDBContext db = new CustomerDBContext();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }


        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Login user)
        {
            CustomerDBContext dbContext = new CustomerDBContext();
            var existingUser = dbContext.Customers.FirstOrDefault(u => u.Username == user.UsernameLogin && u.Password == user.PasswordLogin);

            if (existingUser != null)
            {
                // Đăng nhập thành công, thực hiện chuyển hướng đến trang chính
                return RedirectToAction("Index", "Customers");
            }
            else
            {
                // Đăng nhập thất bại, hiển thị thông báo lỗi
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                return View(user);
            }
        }
        // GET: Customers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,FullName,Birthday,Email,Phone,Avatar,Address,Gender")] Customers customers , HttpPostedFileBase AvatarFile )
        {
            if (ModelState.IsValid)
            {
                
                    if (AvatarFile.ContentLength > 0)
                    {
                        string rootFolder = Server.MapPath("/Uploads");
                        string pathAvatar = rootFolder + AvatarFile.FileName;
                        AvatarFile.SaveAs(pathAvatar);
                        customers.Avatar = "/Uploads/" + AvatarFile.FileName;

                    }
                    db.Customers.Add(customers);
                    db.SaveChanges();
                    return RedirectToAction("Index");
               
               
            }

            return View(customers);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,Password,FullName,Birthday,Email,Phone,Avatar,Address,Gender")] Customers customers, HttpPostedFileBase AvatarFile)
        {
            if (ModelState.IsValid)
            {
               
                    if (AvatarFile.ContentLength > 0)
                    {
                        string rootFolder = Server.MapPath("/Uploads");
                        string pathAvatar = rootFolder + AvatarFile.FileName;
                        AvatarFile.SaveAs(pathAvatar);
                        customers.Avatar = "/Uploads/" + AvatarFile.FileName;

                    }
                    db.Entry(customers).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
             
               
            }
            return View(customers);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Customers customers = db.Customers.Find(id);
            db.Customers.Remove(customers);
            db.SaveChanges();
            return RedirectToAction("Index");
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
