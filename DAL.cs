using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System;
using System.Windows;

namespace TryWPF
{
    class DAL
    {
        //Global variables
        private MySqlConnection cn;
        private string server;
        private string database;
        private string uid;
        private string pass;
        //private string _url = "http://localhost/upload.php";
        private string _url = "http://visuaquantity.esy.es/upload.php";
        //private string _url = "http://visuaquantity.web44.net/upload_webhost.php";

        public DAL()
        {

            //AddUserGantt("4", "matanf");


            //server = "localhost";
            //database = "visua_quantity";
            //uid = "root";
            //pass = "";
            //server = "mysql1.000webhost.com";
            //database = "a2532781_visua";
            //uid = "a2532781_visua";
            //pass = "server5287";
            //server = "mysql.hostinger.co.il";
            //database = "u969015456_visua";
            //uid = "u969015456_visua";
            //pass = "Server5287";
            //string connectionString;
            //connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            //database + ";" + "UID=" + uid + ";" + "PASSWORD=" + pass + ";" + "CharSet = utf8";

            //cn = new MySqlConnection(connectionString);
            //GetAllIssues();
            //cn.Open();
        }

        //Return the stage and the level of the user current geme
        public string GetStageAndLevel(string userName, string gameName)
        {


            try
            {
                var data = new NameValueCollection();
                data["function"] = "GetStageAndLevel";
                data["username"] = userName;
                data["gamename"] = gameName;
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                return result;
            }
            catch (Exception)
            {
                MessageBox.Show("xxx failed", "שגיאה");
            }

            return "";

        }

        //Update game time left to play of the current day of the choosen in the dates table
        public bool UpdateGameTime(string username, string gameName, string time, string day)
        {
            
            try
            {
                var data = new NameValueCollection();
                data["function"] = "UpdateGameTime";
                data["gameName"] = gameName;
                data["username"] = username;
                data["time"] = time;
                data["day"] = day;
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("UpdateGameTime failed", "שגיאה");
                throw;
            }

        }

        //Insert statistic to the statistic table
        public bool InsertGameStatistics(string username, string gameName, string date, string title, string details)
        {
            try
            {
                var data = new NameValueCollection();
                data["function"] = "InsertGameStatistics";
                data["username"] = username;
                data["gameName"] = gameName;
                data["date"] = date;
                data["title"] = title;
                data["details"] = details;
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("InsertGameStatistics failed", "שגיאה");
                throw;
            }
        }

        //Get the time that left to play of a game from the dates table
        public string GetGameTime(string userName, string gameName , string day)
        {
            //VillageGameLocationTimeLeft
          
            try
            {
                var data = new NameValueCollection();
                data["function"] = "GetGameTime";
                data["username"] = userName;
                data["gamename"] = gameName;
                //data["gamename"] = "villageGameLocation";            
                data["day"] = day;

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                return result;
            }
            catch (Exception)
            {
                MessageBox.Show("GetGameTime failed", "שגיאה");
                return "";
            }
        }

        //Add the 22 days of playing to the dates table for a user
        //THe dates will be from today till 21 days from today
        public bool AddUserGantt(string participantNumber, string userName)
        {
            try
            {
                var data = new NameValueCollection();
                data["function"] = "AddUserGantt";
                data["participantNumber"] = participantNumber;
                data["username"] = userName;
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("AddUserGantt failed", "שגיאה");
                return false;
            }
        }

        //Insert a user to the table games_global_time with 0 time(defalut)
        public bool AddUserGamesGlobalTime(string userName)
        {
            try
            {
                var data = new NameValueCollection();
                data["function"] = "AddUserGamesGlobalTime";
                data["username"] = userName;
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("AddUserGamesGlobalTime failed", "שגיאה");
                return false;
            }

        }

        //Insert a user to the table games_status the first possible stage for each game
        public bool AddUserGamesStatus(string userName, string robotGame, string skyGame, string treasureMapGame, string treasureMapReverseGame, string castleGame, string villageGameLocation, string villageGameIdentity)
        {
            try
            {
                var data = new NameValueCollection();
                data["function"] = "AddUserGamesStatus";
                //data["participantNumber"] = participantNumber;
                data["username"] = userName;
                data["robotGame"] = robotGame;
                data["skyGame"] = skyGame;
                data["treasureMapGame"] = treasureMapGame;
                data["treasureMapReverseGame"] = treasureMapReverseGame;
                data["castleGame"] = castleGame;
                data["villageGameLocation"] = villageGameLocation;
                data["villageGameIdentity"] = villageGameIdentity;

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
      
                if (result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("AddUserGamesStatus failed", "שגיאה");
                return false;
            }
        }

        //Return the global play time of a game
        public string GetGameGlobalTime(string userName, string gameName)
        {
            
            try
            {
                var data = new NameValueCollection();
                data["function"] = "GetGameGlobalTime";
                data["username"] = userName;
                data["gamename"] = gameName;

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);

                return result;
            }
            catch (Exception)
            {
                MessageBox.Show("GetGameGlobalTime failed", "שגיאה");
                throw;
            }
        }

        //Return Examinee Current Day
        //If the examinee does not have today a game to play in the gantt it will return empty string
        public string GetExamineeCurrentDay(string userName)
        {
            try
            {
                var data = new NameValueCollection();
                data["function"] = "GetExamineeCurrentDay";
                data["username"] = userName;


                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);

                return result;
            }
            catch (Exception)
            {
                MessageBox.Show("GetExamineeCurrentDay failed", "שגיאה");
                return "";
            }
       

        }

        //Update the value in the table games_global_time of a game
        public bool UpdateGlobalGameTime(string userName, string gameName, string gameGlobalTime)
        {
            try
            {
                var data = new NameValueCollection();
                data["function"] = "UpdateGlobalGameTime";
                data["username"] = userName;
                data["gamename"] = gameName;
                data["gameGlobalTime"] = gameGlobalTime;

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("UpdateGlobalGameTime failed", "שגיאה");
                throw;
            }

        }

        //Update the game status, the status look like this (stage_level)
        public bool UpdateUserGameStatus(string userName, string gameName, string status)
        {
            try
            {
                var data = new NameValueCollection();
                data["function"] = "UpdateUserGameStatus";
                data["username"] = userName;
                data["gamename"] = gameName;
                data["status"] = status;

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("UpdateUserGameStatus failed", "שגיאה");
                throw;
            }
        }

        //Return all the statistics from the statistics table
        public List<Statistics> GetStatistics()
        {

            try
            {

                var data = new NameValueCollection();
                data["function"] = "GetStatistics";
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result == "")
                {
                    return null;
                }


                List<Statistics> sList = new List<Statistics>();
                Statistics stat = new Statistics();


                //string currentUserName = "";
                string[] statArray = result.Split('#');
                //Array.Sort(daysArray);
                foreach (var item in statArray)
                {

                    if (item != "")
                    {

                        stat = new Statistics();
                        string[] dayArray = item.Split('=');
                        stat.username = dayArray[0];
                        stat.gameName = dayArray[1];
                        stat.dateTime = dayArray[2];
                        stat.title = dayArray[3];
                        string[] details = dayArray[4].Split('_');
                        stat.details = details[0] + "." + details[1];
                        sList.Add(stat);
                    }

                }

                return sList;
            }
            catch (Exception)
            {
                MessageBox.Show("GetStatistics failed", "שגיאה");
                return null;
            }
        }

        //Return the examinee game inforamtion for a specific day
        public DailyGamesInformation GetExamineeDailyGamesInformation(string userName, string day)
        {
            try
            {
                var data = new NameValueCollection();
                data["function"] = "GetExamineeDailyGamesInformation";
                data["username"] = userName;
                data["day"] = day;


                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if(result == "")
                {
                    return null;
                }
                DailyGamesInformation dgi = new DailyGamesInformation();
                //return result;
                string[] dayArray = result.Split('=');
                dgi.day = dayArray[0];
                dgi.hour = dayArray[1];
                dgi.robotGameTimeTotal = dayArray[2];
                dgi.skyGameTimeTotal = dayArray[3];
                dgi.treasureMapGameTimeTotal = dayArray[4];
                dgi.treasureMapReverseGameTimeTotal = dayArray[5];
                dgi.castleGameTimeTotal = dayArray[6];
                dgi.villageGameLocationTimeTotal = dayArray[7];
                dgi.villageGameIdentityTimeTotal = dayArray[8];
                dgi.robotGameTimeLeft = dayArray[9];
                dgi.skyGameTimeLeft = dayArray[10];
                dgi.treasureMapGameTimeLeft = dayArray[11];
                dgi.treasureMapReverseGameTimeLeft = dayArray[12];
                dgi.castleGameTimeLeft = dayArray[13];
                dgi.villageGameLocationTimeLeft = dayArray[14];
                dgi.villageGameIdentityTimeLeft = dayArray[15];
                return dgi;
            }
            catch (Exception)
            {
                MessageBox.Show("GetExamineeDailyGamesInformation failed", "שגיאה");
                return null;
            }
    
        }

        //Return the users gantt
        public List<ExamineeGant> GetExamineesGant()
        {

            try
            {
                var data = new NameValueCollection();
                data["function"] = "GetExamineesGant";
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if(result == "")
                {
                    return null;
                }


                List<ExamineeGant> egList = new List<ExamineeGant>();
                ExamineeGant eg = new ExamineeGant();
     
                string currentUserName = "";
                string[] daysArray = result.Split('#');
                Array.Sort(daysArray);
                foreach (var item in daysArray)
                {

                    if (item != "")
                    {
                        string[] dayArray = item.Split('=');

                        if (currentUserName != dayArray[0])
                        {
                            if (currentUserName != "")
                            {
                                egList.Add(eg);
                                eg = new ExamineeGant();
                            }

                            currentUserName = dayArray[0];
                        }

                        eg.userName = dayArray[0];
                        eg.participantNumber = dayArray[1];
                        eg.AddDayToDictionary(dayArray[2], dayArray[3], dayArray[4], dayArray[5], dayArray[6], dayArray[7], dayArray[8], dayArray[9], dayArray[10], dayArray[11], dayArray[12], dayArray[13], dayArray[14], dayArray[15], dayArray[16], dayArray[17], dayArray[18]);
                    }

                }

                egList.Add(eg);
                return egList;
            }
            catch (Exception)
            {
                MessageBox.Show("GetExamineesGant failed", "שגיאה");
                return null;
            }
        }


        //Return the gantt for specific user
        public List<ExamineeGant> GetExamineeGantt(string participantNumber)
        {

            try
            {
                var data = new NameValueCollection();
                data["function"] = "GetExamineeGantt";
                data["participantNumber"] = participantNumber;
                //data["username"] = username;
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if(result == "")
                {
                    return null;
                }


                List<ExamineeGant> egList = new List<ExamineeGant>();
                ExamineeGant eg = new ExamineeGant();


                //string currentUserName = "";
                string[] daysArray = result.Split('#');
                //Array.Sort(daysArray);
                foreach (var item in daysArray)
                {

                    if (item != "")
                    {
                        string[] dayArray = item.Split('=');

                        eg.userName = dayArray[0];
                        eg.participantNumber = dayArray[1];
                        eg.date = dayArray[2];
                        eg.day = dayArray[3];
                        eg.hour = dayArray[4];
                        eg.robotGameTimeTotal = dayArray[5];
                        eg.skyGameTimeTotal = dayArray[6];
                        eg.treasureMapGameTimeTotal = dayArray[7];
                        eg.treasureMapReverseGameTimeTotal = dayArray[8];
                        eg.castleGameTimeTotal = dayArray[9];
                        eg.villageGameLocationTimeTotal = dayArray[10];
                        eg.villageGameIdentityTimeTotal = dayArray[11];
                        eg.robotGameTimeLeft = dayArray[12];
                        eg.skyGameTimeLeft = dayArray[13];
                        eg.treasureMapGameTimeLeft = dayArray[14];
                        eg.treasureMapReverseGameTimeLeft = dayArray[15];
                        eg.castleGameTimeLeft = dayArray[16];
                        eg.villageGameLocationTimeLeft = dayArray[17];
                        eg.villageGameIdentityTimeLeft = dayArray[18];
                        //eg.AddDayToDictionary(dayArray[2], dayArray[3], dayArray[4], dayArray[5], dayArray[6], dayArray[7], dayArray[8], dayArray[9], dayArray[10], dayArray[11], dayArray[12], dayArray[13], dayArray[14]);

                        egList.Add(eg);
                        eg = new ExamineeGant();
                    }

                }

                if (result == "true")
                {
                    //return true;
                }
                else
                {
                    //return false;
                }
                return egList;
            }
            catch (Exception)
            {
                MessageBox.Show("GetExamineeGantt failed", "שגיאה");
                return null;
            }
        }

        //Update examinee gantt(update all the 22 days of the examinee)
        public bool UpdateUserGantt(string participantNumber, string cellsInfo)
        {

            try
            {
                var data = new NameValueCollection();
                data["function"] = "UpdateUserGantt";
                data["participantNumber"] = participantNumber;
                //data["username"] = userName;
                //data["cellsInfo"] = "maty100-0000-0fM-RIre13_000000000#00#0#";
                data["cellsInfo"] = cellsInfo;

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception)
            {
                MessageBox.Show("UpdateUserGantt failed", "שגיאה");
                return false;
            }



        }

        //Insert user to the users table
        public bool AddUser(string participantNumber, string username, string password, string team, string privilege, string gender, string dominantHand, string age, string experimentNumber)
        {

            try
            {
                var data = new NameValueCollection();
                data["function"] = "AddUser";
                data["participantNumber"] = participantNumber;
                data["username"] = username;
                data["password"] = password;
                data["team"] = team;
                data["privilege"] = privilege;
                data["gender"] = gender;
                data["dominantHand"] = dominantHand;
                data["age"] = age;
                data["experimentNumber"] = experimentNumber;

                //participantNumber, username, password, team, privilege,gender,dominantHand,age,experimentNumber
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("AddUser failed", "שגיאה");
                return false;
            }
                
        }

        //Edit user and save the changes in the database
        public bool EditUser(string participantNumber, string username, string password, string team, string privilege, string gender, string dominantHand, string age, string experimentNumber)
        {

            try
            {

                var data = new NameValueCollection();
                data["function"] = "EditUser";
                data["participantNumber"] = participantNumber;
                data["username"] = username;
                data["password"] = password;
                data["team"] = team;
                data["privilege"] = privilege;
                data["gender"] = gender;
                data["dominantHand"] = dominantHand;
                data["age"] = age;
                data["experimentNumber"] = experimentNumber;

                //participantNumber, username, password, team, privilege,gender,dominantHand,age,experimentNumber
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("EditUser failed", "שגיאה");
                return false;
            }
            

        }


        //Delete user from the database
        public bool DeleteUser(string username)
        {
            try
            {
                var data = new NameValueCollection();
                data["function"] = "DeleteUser";
                data["username"] = username;
                //participantNumber, username, password, team, privilege,gender,dominantHand,age,experimentNumber
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("DeleteUser failed", "שגיאה");
                return false;
            }
        }

        //Check if the user name or participant number exsit in the users table
        public bool CheckIfUserOrParticipantNumberExist(string username , string participantNumber)
        {
            try
            {

                var data = new NameValueCollection();
                data["function"] = "CheckIfUserOrParticipantNumberExist";
                data["participantNumber"] = participantNumber;
                data["username"] = username;

                //participantNumber, username, password, team, privilege,gender,dominantHand,age,experimentNumber
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("CheckIfUserOrParticipantNumberExist failed", "שגיאה");
                return false;
            }

        }

        //Get user params by participant number
        public User GetUserByParticipantNumber(string participantNumber)
        {


            try
            {
                User user = new User();
                var data = new NameValueCollection();
                data["function"] = "GetUserByParticipantNumber";
                data["participantNumber"] = participantNumber;

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result != "")
                {
                    string[] userArray = result.Split('=');
                    user._participantNumber = userArray[0];
                    user._privilege = userArray[1];
                    user._age = userArray[2];
                    user._username = userArray[3];
                    user._password = userArray[4];
                    user._gender = userArray[5];
                    user._dominantHand = userArray[6];
                    user._team = userArray[7];
                    user._experimentNumber = userArray[8];
                    //foreach (string participantNumber in participantNumbersArray)
                    //{
                    //    if (participantNumber != "")
                    //        participantNumbersList.Add(participantNumber);
                    //}
                    return user;
                }
                return null;

            }
            catch (Exception)
            {
                MessageBox.Show("GetUserByParticipantNumber failed", "שגיאה");
                return null;
            }

        }

        //Check if the user credentials are valid
        public bool CheckUserCredentials(string username, string password)
        {
            try
            {
                var data = new NameValueCollection();
                data["function"] = "CheckUserCredentials";
                data["username"] = username;
                data["password"] = password;

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result != "")
                {
                    int count = Convert.ToInt32(result);
                    if (count > 0)
                        return true;
                    else
                        return false;
                }

                return false;
            }
            catch (Exception)
            {
                MessageBox.Show("CheckUserCredentials failed", "שגיאה");
                return false;
            }
            
        }


        //Return a list of all the examinees, not administrators
        public List<ExamineepProfile> GetExamineeProfileList()
        {

            try
            {
                List<ExamineepProfile> examineeProfileList = new List<ExamineepProfile>();
                ExamineepProfile ep = new ExamineepProfile();
                var data = new NameValueCollection();
                data["function"] = "GetExamineeProfileList";

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                if (result != "")
                {
                    string[] examineesProfileArray = result.Split('#');
                    foreach (string examineeProfile in examineesProfileArray)
                    {

                        if (examineeProfile != "")
                        {
                            string[] examineeProfileArray = examineeProfile.Split('=');
                            if (examineeProfileArray[1] != "0")//The administrators have participantNumber with the value 0(its prevent editting the adm users)
                            {
                                ep.userName = examineeProfileArray[0];
                                ep.participantNumber = examineeProfileArray[1];
                                examineeProfileList.Add(ep);
                                ep = new ExamineepProfile();

                            }

                        }

                    }
                }
                return examineeProfileList;
            }
            catch (Exception)
            {
                MessageBox.Show("GetExamineeProfileList failed", "שגיאה");
                return null;
            }
          
        }

        //Check the privilege of the user
        public string CheckUserPrivileges(string username)
        {
            try
            {

                var data = new NameValueCollection();
                data["function"] = "CheckUserPrivileges";
                data["username"] = username;

                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                byte[] response = client.UploadValues(_url, "POST", data);
                var result = Encoding.UTF8.GetString(response);
                return result;
            }
            catch (Exception)
            {
                MessageBox.Show("CheckUserPrivileges failed", "שגיאה");
                return "";
            }

           

        }

    }
}

