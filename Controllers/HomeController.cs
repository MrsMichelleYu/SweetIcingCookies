using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSharpProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace CSharpProject.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")] //homepage
        public IActionResult Index()
        {
            if (HttpContext.Session.GetObjectFromJson<List<Item>>("Cart") == null)
            {
                HttpContext.Session.SetObjectAsJson("Cart", new List<Item>());
            }
            return View();
        }

        [HttpGet("admin")] //page to see Admin Login
        public IActionResult Admin()
        {
            return View();
        }

        [HttpPost("login")] //route to log into Admin account
        public IActionResult Login(AdminLogin admin)
        {
            if (ModelState.IsValid)
            {
                if (admin.AdminName == "admin" && admin.Password == "a1234")
                {
                    HttpContext.Session.SetString("UserName", "admin");
                    return RedirectToAction("Inventory");
                }
            }
            return View("Admin");
        }

        [HttpGet("inventory")] //page that shows inventory list once Admin logs in 
        public IActionResult Inventory()
        {
            string AdminName = HttpContext.Session.GetString("UserName");
            if (AdminName == null)
            {
                return View("Admin");
            }
            ViewBag.Admin = AdminName;
            List<Product> AllProducts = dbContext.Products.OrderBy(prod => prod.ProductId).ToList();
            return View(AllProducts);
        }

        [HttpGet("add")] //form for Admin to enter a new product into Inventory
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("add/inventory")] //route to add new product into Inventory
        public IActionResult AddInventory(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                dbContext.Products.Add(newProduct);
                dbContext.SaveChanges();
                return RedirectToAction("Inventory");
            }
            return View("Add");
        }

        [HttpGet("/edit/{productId}")] // form that allows Admin to edit a product on inventory
        public IActionResult EditProduct(int productId)
        {
            Product thisProduct = dbContext.Products.FirstOrDefault(product => product.ProductId == productId);
            return View("EditProduct", thisProduct);
        }

        [HttpPost("/update/{productId}")] //route that allows Admin to update a product on inventory
        public IActionResult UpdateProduct(Product formInfo, int productId)
        {
            Product thisProduct = dbContext.Products.FirstOrDefault(product => product.ProductId == productId);
            Console.WriteLine("Product data found");
            if (ModelState.IsValid)
            {
                thisProduct.Name = formInfo.Name;
                thisProduct.Price = formInfo.Price;
                thisProduct.CreatedAt = DateTime.Now;
                thisProduct.UpdatedAt = DateTime.Now;
                dbContext.SaveChanges();
                return RedirectToAction("Inventory");
            }
            return View("EditProduct", thisProduct);
        }

        [HttpGet("/delete/{productId}")] // route that allows Admin to delete product on Inventory
        public IActionResult Delete(int productId)
        {
            Product thisProduct = dbContext.Products.SingleOrDefault(product => product.ProductId == productId);
            dbContext.Products.Remove(thisProduct);
            dbContext.SaveChanges();
            return RedirectToAction("Inventory");
        }

        [HttpGet("logout")] //log out of Admin account 
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Admin");
        }

        [HttpGet("about")] //page that talks about Bakery 
        public IActionResult About()
        {
            return View();
        }

        [HttpGet("catering")] //page that talks about Catering Services 
        public IActionResult Catering()
        {
            return View();
        }

        [HttpGet("sugar")] //page that lists all of sugar cookies 
        public IActionResult Sugar()
        {
            return View();
        }


        [HttpPost("/add/{productId}")] // allows customer to add sugar cookie into Cart
        public IActionResult AddToCart(Item AddedItem, int productId) //add sugar cookies into cart
        {
            Product retrievedProduct = dbContext.Products.FirstOrDefault(p => p.ProductId == productId);
            AddedItem.Name = retrievedProduct.Name;
            AddedItem.Price = retrievedProduct.Price;
            AddedItem.TotalCost = (AddedItem.Quantity) * (retrievedProduct.Price);
            List<Item> ChosenItems = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            ChosenItems.Add(AddedItem);
            HttpContext.Session.SetObjectAsJson("Cart", ChosenItems);
            return RedirectToAction("Sugar");
        }

        [HttpPost("/add/traditional/{productId}")] //allows customer to add a traditional cookie into Cart
        public IActionResult AddCart(Item AddedItem, int productId)
        {
            Product retrievedProduct = dbContext.Products.FirstOrDefault(p => p.ProductId == productId);
            AddedItem.Name = retrievedProduct.Name;
            AddedItem.Price = retrievedProduct.Price;
            AddedItem.TotalCost = (AddedItem.Quantity) * (retrievedProduct.Price);
            List<Item> ChosenItems = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            ChosenItems.Add(AddedItem);
            HttpContext.Session.SetObjectAsJson("Cart", ChosenItems);
            return RedirectToAction("Traditional");
        }

        [HttpPost("/add/package/{productId}")] //allows customer to add package into Cart
        public IActionResult AddPackage(Item AddedItem, int productId)
        {
            Product retrievedProduct = dbContext.Products.FirstOrDefault(p => p.ProductId == productId);
            AddedItem.ProductId = productId;
            AddedItem.Name = retrievedProduct.Name;
            AddedItem.Price = retrievedProduct.Price;
            AddedItem.TotalCost = (AddedItem.Quantity) * (retrievedProduct.Price);
            List<Item> ChosenItems = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            ChosenItems.Add(AddedItem);
            HttpContext.Session.SetObjectAsJson("Cart", ChosenItems);
            return RedirectToAction("Packaging");
        }

        [HttpGet("cart")] // page that shows all of items in cart
        public IActionResult Cart()
        {
            List<Item> Retrieve = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            decimal SubTotal = 0;
            foreach (var product in Retrieve)
            {
                SubTotal += product.TotalCost;
            }
            int Subtotal = (int)SubTotal; //convert decimal SubTotal to an integer to save into session
            HttpContext.Session.SetInt32("Subtotal", Subtotal);
            ViewBag.SubTotal = SubTotal;
            return View(Retrieve);
        }

        [HttpGet("/edit/quantity/{productId}")] //page that shows customer form to edit Cart
        public IActionResult EditCart(Item formInfo, int productId)
        {
            List<Item> Retrieve = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            foreach (var item in Retrieve)
            {
                if (item.ProductId == productId)
                {
                    return View(item);
                }
            }
            return View("Cart");
        }

        [HttpPost("/update/cart/{productId}")] //route that allows customer to update their Cart
        public IActionResult UpdateCart(Item formInfo, int productId)
        {
            List<Item> Retrieve = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            foreach (var item in Retrieve)
            {
                if (item.ProductId == productId)
                {
                    item.Quantity = formInfo.Quantity;
                    item.TotalCost = (formInfo.Quantity * item.Price);
                    HttpContext.Session.SetObjectAsJson("Cart", Retrieve);
                }
            }
            return RedirectToAction("Cart", Retrieve);
        }

        [HttpGet("popsicle")]
        public IActionResult Popsicle()
        {
            return View();
        }

        [HttpGet("panda")]
        public IActionResult Panda()
        {
            return View();
        }

        [HttpGet("donut")]
        public IActionResult Donut()
        {
            return View();
        }

        [HttpGet("heart")]
        public IActionResult Heart()
        {
            return View();
        }

        [HttpGet("llama")]
        public IActionResult Llama()
        {
            return View();
        }

        [HttpGet("unicorn")]
        public IActionResult Unicorn()
        {
            return View();
        }

        [HttpGet("elephant")]
        public IActionResult Elephant()
        {
            return View();
        }

        [HttpGet("champagne")]
        public IActionResult Champagne()
        {
            return View();
        }

        [HttpGet("marble")]
        public IActionResult Marble()
        {
            return View();
        }

        [HttpGet("traditional")]

        public IActionResult Traditional()
        {
            return View();
        }

        [HttpGet("chocolate")]

        public IActionResult Chocolate()
        {
            return View();
        }

        [HttpGet("lavendar")]

        public IActionResult Lavendar()
        {
            return View();
        }

        [HttpGet("oatmeal")]
        public IActionResult Oatmeal()
        {
            return View();
        }

        [HttpGet("blueberry")]
        public IActionResult Blueberry()
        {
            return View();
        }

        [HttpGet("peanut")]
        public IActionResult Peanut()
        {
            return View();
        }

        [HttpGet("pistachio")]
        public IActionResult Pistachio()
        {
            return View();
        }

        [HttpGet("packaging")]
        public IActionResult Packaging()
        {
            return View();
        }

        [HttpGet("contact")]

        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet("/payment")] // shows page for customer to enter payment 
        public IActionResult Payment()
        {
            return View();
        }

        [HttpPost("/paymentinfo")] // route that allows customer to pay 
        public IActionResult PaymentInfo(Customer newCustomer)
        {
            if (ModelState.IsValid)
            {
                dbContext.Customers.Add(newCustomer);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("Customer", newCustomer.CustomerId);
                return RedirectToAction("Details");
            }
            return View("Payment");
        }

        [HttpGet("/details")] //page that shows details of the customer's order
        public IActionResult Details()
        {
            int CustomerId = (int)HttpContext.Session.GetInt32("Customer");
            Customer retrievedCustomer = dbContext.Customers.FirstOrDefault(c => c.CustomerId == CustomerId);
            List<Item> Retrieve = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            decimal SubTotal = (decimal)HttpContext.Session.GetInt32("Subtotal");
            ViewBag.SubTotal = SubTotal;
            ViewBag.Cart = Retrieve;
            decimal percentage = (decimal)0.0775;
            decimal Tax = Decimal.Multiply(SubTotal, percentage);
            var newTax = Math.Round(Tax, 2);
            ViewBag.Tax = newTax;
            decimal FinalTotal = newTax + SubTotal + 3;
            ViewBag.FinalTotal = FinalTotal;
            string stringTotal = FinalTotal.ToString();
            HttpContext.Session.SetString("Total", stringTotal);
            return View(retrievedCustomer);
        }

        [HttpPost("/process")] //route that finalizes order and enters customer's order into database
        public IActionResult Process(string Notes)
        {
            Order newOrder = new Order();
            newOrder.Notes = Notes;
            newOrder.CustomerId = (int)HttpContext.Session.GetInt32("Customer");
            var thisTotal = Convert.ToDecimal(HttpContext.Session.GetString("Total"));
            newOrder.TotalCost = thisTotal;
            dbContext.Orders.Add(newOrder);
            dbContext.SaveChanges();
            List<Item> CartItems = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            foreach (var Item in CartItems)
            {
                OrderedItem newItem = new OrderedItem();
                newItem.OrderId = newOrder.OrderId;
                newItem.Quantity = Item.Quantity;
                newItem.ProductId = Item.ProductId;
                dbContext.OrderedItems.Add(newItem);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Success");
        }

        [HttpGet("success")] // success page that shows that customer's order was successful and finalized
        public IActionResult Success()
        {
            int customerId = (int)HttpContext.Session.GetInt32("Customer");
            Customer thisCustomer = dbContext.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            return View("Success", thisCustomer);
        }

        [HttpGet("/clear")] //route to go back to homepage after making order, clears session
        public IActionResult Clear()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
