using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webproject_M_R.Models;
using Webproject_M_R.Models.DB;

namespace Webproject_M_R.Controllers
{
    public class ShopController : Controller
    {
        private IRepositoryUser rep;

        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult Registration(User user)
        {
            if (user == null)
            {
                return RedirectToAction("Registration");
            }

            CheckUserData(user);

            if (!ModelState.IsValid)
            {
                return View(user);
            }
            else
            {
                rep = new RepositoryUserDB();

                //eine Verbindung zum DB server herstellen 
                rep.Open();
                //versuchen, die Userdaten einzutragen
                if (rep.Insert(user))
                {
                    return View("Message", new Message("Registrierung", "Ihre Daten wurden erfolgreich abgespeichert"));
                }
                else
                {
                    //verbindung schließen
                    rep.Close();
                    return View("Message", new Message("Registrierung", "Ihre Daten konnten nicht gespeichert werden"));
                }

                //nun würden wir die Daten in einer DatenBank abspeichern

            }
        }

        public ActionResult Login()
        {
            return View(new UserLogin());
        }

        [HttpPost]
        public ActionResult Login(UserLogin user)
        {
            User userFromDB;
            rep = new RepositoryUserDB();
            rep.Open();
            userFromDB = rep.Login(user);
            if (userFromDB == null)
            {
                ModelState.AddModelError("Username", "Benutzername oder Passwort stimmen nicht übereine");
                return View(user);
            }
            else
            {
                Session["loggedinUser"] = userFromDB;
                return RedirectToAction("index", "home");
            }
        }


        private void CheckUserData(User user)
        {
            if (string.IsNullOrEmpty(user.Lastname.Trim()))
            {
                ModelState.AddModelError("Lastname", "Nachname ist ein Pflichtfeld");
            }

            if (user.Gender == Gender.notSpecified)
            {
                ModelState.AddModelError("Gender", "Bitte wählen sie das Geschlecht aus");
            }
            if (string.IsNullOrEmpty(user.Username.Trim()))
            {
                ModelState.AddModelError("Username", "Username ist ein Pflichtfeld");
            }
            //passwortfeld -> Richtlinien: mind. 8 Zeichen, mind. 1 klein & großbuchstabe; 1 sonderzeichen
            if (!CheckPassword(user.Password))
            {
                ModelState.AddModelError("Password",
                "Password muss mind. 8 Zeichen lang sein; mind. 1 Klein- & Großbuchstaben und ein Sonderzeichen enthalten.");
            }
            if (user.Password != user.Password2)
            {
                ModelState.AddModelError("Password2", "Passwort 2 muss mit PW1 übereinstimmen");
            }
        }

        private bool CheckPassword(string password)
        {
            string pwd = password.Trim();
            if (pwd.Length < 8)
            {
                return false;
            }
            if (!PasswordContainsLowercaseCharacters(pwd, 1))
            {
                return false;
            }
            if (!PasswordContainsUppercaseCharacters(pwd, 1))
            {
                return false;
            }
            if (!PasswordContainsSpecialCharacters(pwd, 1))
            {
                return false;
            }

            return true;

        }
        private bool PasswordContainsLowercaseCharacters(string text, int minCount)
        {
            int count = 0;
            foreach (char c in text)
            {
                if (char.IsLower(c))
                {
                    count++;
                }

            }
            return count >= minCount;
        }
        private bool PasswordContainsUppercaseCharacters(string text, int minCount)
        {
            int count = 0;
            foreach (char c in text)
            {
                if (char.IsUpper(c))
                {
                    count++;
                }

            }
            return count >= minCount;
        }
        private bool PasswordContainsSpecialCharacters(string text, int minCount)
        {
            string specialChars = "!\"()&%$§/=#+-?*'@€^°_.\\";
            int count = 0;
            foreach (char c in text)
            {
                if (specialChars.Contains(c))
                {
                    count++;
                }

            }
            return count >= minCount;
        }

    }
}