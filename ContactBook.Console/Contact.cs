using System;
using System.Linq;

namespace Phonebook.Console
{
    public delegate void ContactChange(string newphone, Contact contact);
    public class Contact
    {
        public event ContactChange TryChange;
        private string firstName;
        private string lastName;
        private DateTime birthDate;
        private string phone;
        private string email;

        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                if ((value != null) && (value != "") && (value.Count(c => c == ' ') != value.Length))
                {
                    firstName = value;
                }
                else
                {
                    throw new ArgumentException(String.Format("First name cannot be null or empty string"));
                }

            }
        }
        public string LastName
        {
            get
            {
                return lastName;
            }

            set
            {
                if ((value != null) && (value != "") && (value.Count(c => c == ' ') != value.Length))
                {
                    lastName = value;
                }
                else
                {
                    throw new ArgumentException(String.Format("Last name cannot be null or empty string"));
                }
            }
        }

        public DateTime BirthDate
        {
            get
            {
                return birthDate;
            }

            set
            {
                if ((value != null) && (value >= new DateTime(1900, 1, 1)))
                {
                    birthDate = value;
                }
                else
                {
                    throw new ArgumentException("Birthdate cannot be earlier than 01 Jan 1900");
                }
            }
        }

        public string Phone
        {
            get
            {
                return phone;
            }

            set
            {

                TryChange?.Invoke(value, this);
                if (value == null)
                {
                    phone = value;
                }
                else if ((value[0] == '+') && (value.Length > 4) && (value.Length <= 16) && (value.Length - 1 == value.Where(c => char.IsDigit(c)).Count()))
                {
                    phone = value;
                }
                else
                {
                    throw new ArgumentException("A phone number should begin with a “+”,followed by minimum 4 and maximum 15 digits");
                }
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                if (value == null)
                {
                    email = value;
                }
                else if ((value != "") && (value.Count(c => c == ' ') != value.Length))
                {
                    email = value.ToLower();
                }
                else
                {
                    throw new ArgumentException("E-mail cannot be an empty string");
                }
            }
        }

        public Contact(string FirstName, string LastName, DateTime BirthDate, string Phone, string Email)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.BirthDate = BirthDate;
            this.Phone = Phone;
            this.Email = Email;
        }
        public Contact(string FirstName, string LastName, DateTime BirthDate)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.BirthDate = BirthDate;
            email = null;
            phone = null;
        }
    }
}
