using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonChiquito
{
    [Serializable]
    public class Usuario
    {
        private int id;
        private string username;
        private string password;

        // Constructor por defecto
        public Usuario(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        // Getter y Setter para el atributo 'id'
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        // Getter y Setter para el atributo 'username'
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        // Getter y Setter para el atributo 'password'
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
