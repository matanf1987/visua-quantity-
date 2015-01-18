using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryWPF
{
    class User
    {
        public string _participantNumber;
        public string _privilege;
        public string _age;
        public string _username;
        public string _password;
        public string _gender;
        public string _dominantHand;
        public string _team;
        public string _experimentNumber;


        public User()
        {

        }

        public User(string participantNumber, string privilege, string age, string username, string password, string gender, string dominantHand, string team, string experimentNumber)
        {
            _participantNumber = participantNumber;
            _privilege = privilege;
            _age = age;
            _username = username;
            _password = password;
            _gender = gender;
            _dominantHand = dominantHand;
            _team = team;
            _experimentNumber = experimentNumber;
        }

    }
}
