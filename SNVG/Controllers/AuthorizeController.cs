using SNVG.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SNVG.Controllers
{
    public class AuthorizeController : Controller
    {
        SNVGEntities dbmodel = new SNVGEntities();
        // GET: Authorize

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        [ActionName("SignIn")]
        public ActionResult SignIn2(string email, string password)
        {
            try
            {
                string message = "";
                var login_user = dbmodel.Accounts.Where(x => x.Email == email && x.Password == password).SingleOrDefault();
                if (login_user != null)
                {
                    Session["User_Id"] = login_user.Id.ToString();
                    //Session["UserName"] = login_user.Name.ToString();
                    return RedirectToAction("ProfileUser", "Authorize");
                }
                else
                {
                    message = "Invalid Credential Applied";
                    ViewBag.Err = message;
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.inco = "Something Invalid";
                return View();
            }
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ActionName("SignUp")]
        public ActionResult SignUp(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return RedirectToAction("SignUp", "Authorize");
                }
                bool Status = false;
                string message = "";

                // Model Validation 
                if (ModelState.IsValid)
                {
                    var isExist = IsEmailExist(email);
                    if (isExist)
                    {
                        ModelState.AddModelError("EmailExist", "Email already exist");
                        ViewBag.Error = "Email Already exist";
                        return View();
                    }
                    var account = new Account()
                    {
                        Email = email,
                        Password = password,
                    };

                    dbmodel.Accounts.Add(account);
                    dbmodel.SaveChanges();
                    var Latest = dbmodel.Accounts.OrderByDescending(x => x.Id).FirstOrDefault();
                    Session["Latest_UerID"] = Latest.Id;
                    //Send Email to User
                }
                else
                {
                    message = "Invalid Request";
                }
                ViewBag.Message = message;
                ViewBag.Status = Status;
                return RedirectToAction("NamePage", "Authorize");
            }
            catch (Exception ex)
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpGet]
        public ActionResult NamePage()
        {
            if (Session["Latest_UerID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }
        [HttpPost]
        [ActionName("NamePage")]
        public ActionResult NamePage(string FirstName, string LastName)
        {
            if (Session["Latest_UerID"] != null)
            {
                var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();

                account.Name = FirstName;
                account.LastName = LastName;

                dbmodel.Entry(account).State = EntityState.Modified;
                dbmodel.SaveChanges();

                return RedirectToAction("postCode", "Authorize");
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpGet]
        public ActionResult postCode()
        {
            if (Session["Latest_UerID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }
        [HttpPost]
        [ActionName("postCode")]
        public ActionResult postCode(string code)
        {
            if (Session["Latest_UerID"] != null)
            {
                var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();

                account.Postcode = code;

                dbmodel.Entry(account).State = EntityState.Modified;
                dbmodel.SaveChanges();

                return RedirectToAction("uk_mens_primary_objectives_component", "Authorize");
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpGet]
        public ActionResult uk_mens_primary_objectives_component()
        {
            if (Session["Latest_UerID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }
        [HttpPost]
        [ActionName("PrimaryObjectives")]
        public ActionResult uk_mens_primary_objectives_component(bool discovery, bool TryNew, bool exclusive_brands, bool expertadvice, bool savetime)
        {
            if (Session["Latest_UerID"] != null)
            {
                var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();

                var enter = new PrimaryComponent()
                {
                    AccesstoExclusiveBrands = exclusive_brands,
                    DiscoverNewTrend = discovery,
                    TryNew = TryNew,
                    ExpertAdvice = expertadvice,
                    SaveTime = savetime,
                    AccountId = accountId
                };
                dbmodel.PrimaryComponents.Add(enter);
                dbmodel.SaveChanges();

                return RedirectToAction("uk_height_component", "Authorize");
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpGet]
        public ActionResult uk_height_component()
        {
            if (Session["Latest_UerID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }
        [HttpPost]
        [ActionName("Height")]
        public ActionResult uk_height_component(string feet, string inches)
        {
            if (Session["Latest_UerID"] != null)
            {
                var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();

                var test = feet.ToString() + "." + inches.ToString();
                account.Height = test;

                dbmodel.Entry(account).State = EntityState.Modified;
                dbmodel.SaveChanges();

                return RedirectToAction("uk_weight_component", "Authorize");
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpGet]
        public ActionResult uk_weight_component()
        {
            if (Session["Latest_UerID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }
        [HttpPost]
        [ActionName("WeightComponent")]
        public ActionResult uk_weight_component(string weight)
        {
            try
            {
                if (Session["Latest_UerID"] != null)
                {
                    var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                    var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();

                    account.Weight = weight.ToString();

                    dbmodel.Entry(account).State = EntityState.Modified;
                    dbmodel.SaveChanges();

                    return RedirectToAction("uk_mens_size_component", "Authorize");
                }
                else
                {
                    return RedirectToAction("SignUp", "Authorize");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpGet]
        public ActionResult uk_mens_size_component()
        {
            if (Session["Latest_UerID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpPost]
        [ActionName("MenSize")]
        public ActionResult uk_mens_size_component(string uk_mens_onboarding_shirt_size_generic, string uk_mens_onboarding_size_collar, string uk_mens_onboarding_size_blazers,
                                                        string uk_mens_onboarding_size_bottoms, string uk_mens_onboarding_size_waist_numerical, string uk_mens_onboarding_size_inseam_numerical
                                                            , string uk_mens_onboarding_size_shoe_numerical)
        {
            try
            {
                if (Session["Latest_UerID"] != null)
                {
                    var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                    var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();

                    var size = new MenSize()
                    {
                        AccountId = account.Id,
                        Blazer = uk_mens_onboarding_size_blazers,
                        Bottom = uk_mens_onboarding_size_bottoms,
                        Collar = uk_mens_onboarding_size_collar,
                        InsideLeg = uk_mens_onboarding_size_inseam_numerical,
                        Shirt = uk_mens_onboarding_shirt_size_generic,
                        Waist = uk_mens_onboarding_size_waist_numerical,
                        Shoe = uk_mens_onboarding_size_shoe_numerical
                    };

                    dbmodel.MenSizes.Add(size);
                    dbmodel.SaveChanges();
                    return RedirectToAction("uk_mens_casual_shirts_style_component", "Authorize");
                }
                else
                {
                    return RedirectToAction("SignUp", "Authorize");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpGet]
        public ActionResult uk_mens_casual_shirts_style_component()
        {
            if (Session["Latest_UerID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpPost]
        [ActionName("ShirtStyle")]
        public ActionResult uk_mens_casual_shirts_style_components(string slim, string regular)
        {
            try
            {
                if (Session["Latest_UerID"] != null)
                {
                    var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                    var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();
                    var slimShirt = false;
                    var regularshirt = false;
                    if (!string.IsNullOrEmpty(slim))
                    {
                        slimShirt = true;
                    }

                    if (!string.IsNullOrEmpty(regular))
                    {
                        regularshirt = true;
                    }

                    var size = new ShirtStyle()
                    {
                        AccountId = account.Id,
                        Regulat = slimShirt,
                        Slim = regularshirt
                    };

                    dbmodel.ShirtStyles.Add(size);
                    dbmodel.SaveChanges();
                    return RedirectToAction("uk_mens_jeans_fit_component", "Authorize");
                }
                else
                {
                    return RedirectToAction("SignUp", "Authorize");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpGet]
        public ActionResult uk_mens_jeans_fit_component()
        {
            if (Session["Latest_UerID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }
        [HttpPost]
        [ActionName("JeansStyle")]
        public ActionResult uk_mens_jeans_fit_component(string skinny, string slim, string relaxed, string straight)
        {
            try
            {
                if (Session["Latest_UerID"] != null)
                {
                    var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                    var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();
                    var skinnyJeans = false; var slimJeans = false; var relaxedJeans = false; var straightJeans = false;

                    if (!string.IsNullOrEmpty(skinny))
                    {
                        skinnyJeans = true;
                    }
                    if (!string.IsNullOrEmpty(slim))
                    {
                        slimJeans = true;
                    }
                    if (!string.IsNullOrEmpty(relaxed))
                    {
                        relaxedJeans = true;
                    }
                    if (!string.IsNullOrEmpty(straight))
                    {
                        straightJeans = true;
                    }
                    var size = new JeansFit()
                    {
                        AccountId = account.Id,
                        Slim = slimJeans,
                        Relaxed = relaxedJeans,
                        Skinny = skinnyJeans,
                        Straight = straightJeans
                    };

                    dbmodel.JeansFits.Add(size);
                    dbmodel.SaveChanges();
                    return RedirectToAction("type", "Authorize");
                }
                else
                {
                    return RedirectToAction("SignUp", "Authorize");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("SignUp", "Authorize");
            };
        }

        [HttpGet]
        public ActionResult type()
        {
            if (Session["Latest_UerID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpPost]
        [ActionName("DesignStyle")]
        public ActionResult type(string st1, string st2, string st3, string st4, string st5, string st6, string st7, string st8, string st9,
            string st10, string st11, string st12, string st13, string st14, string st15, string st16)
        {
            try
            {
                if (Session["Latest_UerID"] != null)
                {
                    var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                    var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();

                    var size = new OutfitsDesign()
                    {
                        AccountId = account.Id,
                        Style1 = string.IsNullOrEmpty(st1),
                        Style2 = string.IsNullOrEmpty(st2),
                        Style3 = string.IsNullOrEmpty(st3),
                        Style4 = string.IsNullOrEmpty(st4),
                        Style5 = string.IsNullOrEmpty(st5),
                        Style6 = string.IsNullOrEmpty(st6),
                        Style7 = string.IsNullOrEmpty(st7),
                        Style8 = string.IsNullOrEmpty(st8),
                        Style9 = string.IsNullOrEmpty(st9),
                        Style10 = string.IsNullOrEmpty(st10),
                        Style11 = string.IsNullOrEmpty(st11),
                        Style12 = string.IsNullOrEmpty(st12),
                        Style13 = string.IsNullOrEmpty(st13),
                        Style14 = string.IsNullOrEmpty(st14),
                        Style15 = string.IsNullOrEmpty(st15),
                        Style16 = string.IsNullOrEmpty(st16)
                    };

                    dbmodel.OutfitsDesigns.Add(size);
                    dbmodel.SaveChanges();
                    return RedirectToAction("pricing", "Authorize");
                }
                else
                {
                    return RedirectToAction("SignUp", "Authorize");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("SignUp", "Authorize");
            };
        }

        [HttpGet]
        public ActionResult pricing()
        {
            if (Session["Latest_UerID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpPost]
        [ActionName("pricing")]
        public ActionResult pricing(string shirts, string tees, string jumpers, string jeans, string shorts, string jackets, string shoes, string coats)
        {
            try
            {
                if (Session["Latest_UerID"] != null)
                {
                    var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                    var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();

                    var size = new Pricing()
                    {
                        AccountId = account.Id,
                        BlazersAndJackets = jackets,
                        Coats = coats,
                        JumpersAndSweatShirts = jumpers,
                        Shirts = shirts,
                        Shoes = shoes,
                        Shorts = shorts,
                        TeesAndPolos = tees,
                        TrouserAndJeans = jeans
                    };

                    dbmodel.Pricings.Add(size);
                    dbmodel.SaveChanges();
                    return RedirectToAction("setDeliveryDate", "Authorize");
                }
                else
                {
                    return RedirectToAction("SignUp", "Authorize");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("SignUp", "Authorize");
            };
        }

        [HttpGet]
        public ActionResult setDeliveryDate()
        {
            ViewBag.DeliveryDate = DateTime.Now.AddDays(10);
            try
            {
                if (Session["Latest_UerID"] != null)
                {
                    var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                    var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();

                    account.DeliveryDate = DateTime.Now.AddDays(10);

                    dbmodel.Entry(account).State = EntityState.Modified;
                    dbmodel.SaveChanges();

                    return View();
                }
                else
                {
                    return RedirectToAction("SignUp", "Authorize");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpGet]
        public ActionResult paymentMethod()
        {
            if (Session["Latest_UerID"] != null)
            {
                Random _random = new Random();
                var check = _random.Next(50, 2000);
                ViewBag.NumberValue = check;
                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpPost]
        [ActionName("PaymentMethod")]
        public ActionResult paymentMethod(string COD, string CardName, string cvv, DateTime? ExpiryDate, string cardnumber, string Address, string PsotCode)
        {
            try
            {
                if (Session["Latest_UerID"] != null)
                {
                    var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                    var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();
                    var CODPayment = false;

                    if (!string.IsNullOrEmpty(COD))
                    {
                        CODPayment = true;
                    }

                    var size = new PaymentMethod()
                    {
                        AccountId = account.Id,
                        CardName = CardName,
                        CardNumber = cardnumber,
                        cvv = cvv,
                        ExpiryDate = ExpiryDate,
                        postcode = PsotCode
                    };

                    dbmodel.PaymentMethods.Add(size);
                    dbmodel.SaveChanges();
                    return RedirectToAction("thanks", "Authorize");
                }
                else
                {
                    return RedirectToAction("SignUp", "Authorize");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("SignUp", "Authorize");
            };
        }

        public ActionResult thanks()
        {
            if (Session["Latest_UerID"] != null)
            {
                var accountId = Convert.ToInt32(Session["Latest_UerID"].ToString());
                var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();

                SendEmail($"SNVG Order Confirmation", "<h1>Order Confirmation </h1> <h3> You Order is Confirmed and will be delivered in Given date </h3> ", "snvgtest1234@gmail.com", "Arain123@", account.Email);


                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [HttpGet]
        public ActionResult ProfileUser()
        {
            if (Session["User_Id"] != null)
            {
                var accountId = Convert.ToInt32(Session["User_Id"].ToString());
                var account = dbmodel.Accounts.Where(x => x.Id == accountId).FirstOrDefault();

                ViewBag.account = account;
                return View();
            }
            else
            {
                return RedirectToAction("SignUp", "Authorize");
            }
        }

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            var v = dbmodel.Accounts.Where(a => a.Email == emailID).FirstOrDefault();
            return v != null;
        }
        [NonAction]
        public void SendVerificationEmailLink(string emailid)
        {
            var fromEmail = new MailAddress("gareenhacker21@gmail.com", "malik taqi");
            var toEmail = new MailAddress(emailid);
            var fromEmailPassword = "taqveem123";
            string subject = "";
            string body = "";

            subject = "Your account is successfully created!";
            body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/>";


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        [NonAction]
        public static bool SendEmail(string subject, string body, string from, string password, string to)
        {
            try
            {
                bool res = false;

                string[] tempfrom = from.Split('@');

                MailMessage mail = new MailMessage();
                mail.To.Add(to);
                mail.From = new MailAddress(from, "SNVG", Encoding.UTF8);
                mail.Subject = subject;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.Body = body;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                SmtpClient client = new SmtpClient();
                //Add the Creddentials- use your own email id and password
                client.UseDefaultCredentials = false;

                client.Credentials = new System.Net.NetworkCredential(from, password);

                // Gmail works on this port
                if (tempfrom[1] == "gmail.com")
                {
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                }
                else if (tempfrom[1] == "yahoo.com")
                {
                    client.Port = 465;
                    client.Host = "smtp.mail.yahoo.com";
                }
                else
                {
                }
                client.EnableSsl = true; //Gmail works on Server Secured Layer
                client.Send(mail);
                res = true;
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}