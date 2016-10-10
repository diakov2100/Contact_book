using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Phonebook.Console
{

    public class ContactBook
    {
        private List<Contact> allContacts = new List<Contact>();
        private Dictionary<string, Contact> allContactsDictionary = new Dictionary<string, Contact>();

        public List<Contact> AllContacts
        {
            get
            {
                return allContacts;
            }
        }
        public void AddContact(Contact contact)
        {

            if (((contact.Phone != null) && (allContactsDictionary.ContainsKey(contact.Phone))) || ((contact.Email != null) && (allContacts.Exists(c => contact.Email == c.Email))))
            {
                throw new ArgumentException(String.Format("Contact with such data has been already added"));
            }
            else
            {
                allContacts.Add(contact);
                contact.TryChange += TryChange;
                if (contact.Phone != null)
                {
                    allContactsDictionary[contact.Phone] = contact;
                }
            }

        }

        public List<Contact> SearchByName(string expression)
        {
            return allContacts.FindAll(c =>
               ((Regex.IsMatch(expression, c.FirstName, RegexOptions.IgnoreCase)) && !(Regex.IsMatch(expression, c.LastName, RegexOptions.IgnoreCase)))
            || ((Regex.IsMatch(expression, c.LastName, RegexOptions.IgnoreCase)) && !(Regex.IsMatch(expression, c.FirstName, RegexOptions.IgnoreCase)))
            || ((Regex.IsMatch(expression, c.FirstName, RegexOptions.IgnoreCase)) && (Regex.IsMatch(expression, c.LastName, RegexOptions.IgnoreCase)))
            );
        }

        public Contact SearchByEmail(string email)
        {
            return allContacts.Find(c => email == c.Email);
        }

        public Contact SearchByPhone(string phone)
        {
            if (allContactsDictionary.ContainsKey(phone))
            {
                return allContactsDictionary[phone];
            }
            else
            {
                return null;
            }
        }
        public void TryChange(string newphone, Contact contact)
        {
            if ((newphone != null) && (allContacts.Find(c => (newphone == c.Phone)&&(c != contact)) != null))
            {
                throw new Exception("Contact with such number has been already added");
            }
            
            if ((contact.Phone == null) && (newphone != null))
            {
                allContactsDictionary[newphone] = contact;
            }
            else if (((contact.Phone != null) && (newphone == null)))
            {
                allContactsDictionary.Remove(contact.Phone);
            }
            else if (((contact.Phone != null) && (newphone != null)))
            {
                allContactsDictionary.Remove(contact.Phone);
                allContactsDictionary[newphone] = contact;
            }

        }
        public void RemoveAll(Func<Contact, bool> check)
        {
            foreach (Contact currentContact in allContacts)
            {
                if (check(currentContact))
                {
                    currentContact.TryChange -= TryChange;
                    if (currentContact.Phone != null) { allContactsDictionary.Remove(currentContact.Phone); }
                }
            }
            allContacts.RemoveAll(c => check(c));
        }
    }
}
