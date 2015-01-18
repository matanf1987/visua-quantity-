using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace TryWPF
{
    class BLL_ms
    {

        DAL dal = new DAL();
        public List<Game> _gamesList = new List<Game>();//The list hold the user answers for each single game

        //Check if the user credentials are valid
        public bool CheckUserCredentials(string username, string password)
        {

            if (dal.CheckUserCredentials(username, password) == true)
                return true;
            else
                return false;
        }

        //Check the privilege of the user
        public string CheckUserPrivileges(string username)
        {
            return dal.CheckUserPrivileges(username);
        }

        //Add the 22 days of playing to the dates table for a user
        //THe dates will be from today till 21 days from today
        public bool AddUserGantt(string participantNumber, string userName)
        {
            return dal.AddUserGantt(participantNumber,userName);
        }

        //Insert a user to the table games_status the first possible stage for each game
        public bool AddUserGamesStatus(string userName, string participantNumber)
        {

            ReadXml();
            
            //GetFirstStageLevel(robotGame);
            string robotGame = "1_" + GetFirstStageLevel("Robot game"); ;
            string skyGame = "1_" + GetFirstStageLevel("Sky game");
            string treasureMapGame = "1_" + GetFirstStageLevel("Treasure map game");
            string treasureMapReverseGame = "1_" + GetFirstStageLevel("Treasure map reverse game");
            string castleGame = "1_" + GetFirstStageLevel("Castle game");
            string villageGameLocation = "1_0";
            string villageGameIdentity = "1_0";
            return dal.AddUserGamesStatus(userName, robotGame, skyGame, treasureMapGame, treasureMapReverseGame, castleGame,villageGameLocation,villageGameIdentity);

        }

        //Insert a user to the table games_global_time with 0 time(defalut)
        public bool AddUserGamesGlobalTime(string userName)
        {
            return dal.AddUserGamesGlobalTime(userName);
        }
        
        //Return the first stage and level for a game (the first posibble stage and level to play)
        public string GetFirstStageLevel(string gameName)
        {
            Stage stage = getStageDetails(gameName,1);
            if(stage == null)
            {
                throw new Exception("");
            }
            else
            {
                return "" + stage._firstLevel;
            }
          

        }

        //Return the stage object
        public Stage getStageDetails(string gameName, int stageNumber)
        {
      
            Stage stage = null;

            try
            {
                foreach (Game g2 in _gamesList) // Loop through List with foreach
                {
                    if (g2.getGameName() == gameName)
                    {
                        stage = g2.GetStage(stageNumber);
                        return stage;
                    }
                }
            }
            catch (Exception)
            {
                 MessageBox.Show("getStageDetails failed", "שגיאה");
                 throw;
            }

            return stage;//Return null object if stage does not exists////////////////////////////////////////////////////////////////////////////////////

        }
     

        //Read the games configuration form the xml file 
        public void ReadXml()
        {
            // string urlPath = "https://gdata.youtube.com/feeds/api/videos/" + id + "?v=2";
            //  string urlPath = "https://gdata.youtube.com/feeds/api/videos/94bGzWyHbu0?v=2";


            
        
            //XmlTextReader reader = new XmlTextReader(urlPath);
            try
            {
                XmlTextReader reader = new XmlTextReader("games.xml");
                string str = "";
                string gameName = "";
                int stageNumber = 0;
                int boardSize = 0;
                int winsInARow = 0;
                int firstStage = 0;
                int lastStage = 0;
                string imageRobot = "";
                while (reader.Read())
                {
                    // Do some work here on the data.
                    str = reader.Name;
                    if (str == "games")
                    {
                        while (reader.Read())
                        {
                            // Do some work here on the data.
                            str = reader.Name;
                            if (str == "games" && reader.NodeType == XmlNodeType.EndElement)
                            {
                                break;
                            }

                            if (str.Contains("game"))
                            {
                                string[] gameNameArray = str.Split('_');
                                gameName = "";
                                for (int i = 1; i < gameNameArray.Length; i++)
                                {
                                    if (i == 1)
                                        gameName += gameNameArray[i];
                                    else
                                        gameName += " " + gameNameArray[i];
                                }

                                Game game = new Game(gameName);
                                _gamesList.Add(game);
                                while (reader.Read())
                                {
                                    // Do some work here on the data.
                                    str = reader.Name;


                                    if (str.Contains("game") && reader.NodeType == XmlNodeType.EndElement)
                                    {
                                        break;
                                    }
                                    if (str == "stage")
                                    {
                                        while (reader.Read())
                                        {

                                            str = reader.Name;
                                            if (str == "stage" && reader.NodeType == XmlNodeType.EndElement)
                                            {
                                                Stage stage = new Stage(stageNumber, boardSize, winsInARow, firstStage, lastStage, imageRobot);
                                                game.addStage(stage);
                                                break;
                                            }
                                            else if (str == "stageNumber" && reader.NodeType == XmlNodeType.Element)
                                            {
                                                reader.Read();
                                                stageNumber = Convert.ToInt32(reader.Value.ToString());
                                            }
                                            else if (str == "boardSize" && reader.NodeType == XmlNodeType.Element)
                                            {
                                                reader.Read();
                                                boardSize = Convert.ToInt32(reader.Value.ToString());
                                            }
                                            else if (str == "winsInARow" && reader.NodeType == XmlNodeType.Element)
                                            {
                                                reader.Read();
                                                winsInARow = Convert.ToInt32(reader.Value.ToString());
                                            }
                                            else if (str == "firstLevel" && reader.NodeType == XmlNodeType.Element)
                                            {
                                                reader.Read();
                                                firstStage = Convert.ToInt32(reader.Value.ToString());
                                            }
                                            else if (str == "lastLevel" && reader.NodeType == XmlNodeType.Element)
                                            {
                                                reader.Read();
                                                lastStage = Convert.ToInt32(reader.Value.ToString());
                                            }
                                            else if (str == "image" && reader.NodeType == XmlNodeType.Element)
                                            {
                                                reader.Read();
                                                imageRobot = reader.Value.ToString();
                                            }
                                        }//end -   while (reader.Read())
                                    }//ends - if (str == "Stage")
                                }//while (reader.Read())
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ReadXML failed", "שגיאה");
            }
        }

        //Return Examinee Current Day
        //If the examinee does not have today a game to play in the gantt it will return empty string
        public string GetExamineeCurrentDay(string userName)
        {
            return dal.GetExamineeCurrentDay(userName);
        }

        //Return the examinee game inforamtion for a specific day
        public DailyGamesInformation GetExamineeDailyGamesInformation(string userName, string day)
        {
            return dal.GetExamineeDailyGamesInformation(userName,day);

        }

        //Return the users gantt
        public List<ExamineeGant> getExamineesGant()
        {
            return dal.GetExamineesGant();
        }

        //Return all the statistics from the statistics table
        public List<Statistics> GetStatistics()
        {
            return dal.GetStatistics();
        }

        //Update examinee gantt(update all the 22 days of the examinee)
        public bool UpdateUserGantt(string participantNumber, string cellsInfo)
        {
             return dal.UpdateUserGantt(participantNumber, cellsInfo);
        }

        //Return the gantt for specific user
        public List<ExamineeGant> GetExamineeGantt(string participantNumber)
        {
             return dal.GetExamineeGantt(participantNumber);
        }
         


        //Add user to the database
        public bool AddUser(string participantNumber, string username, string password, string team, string privilege, string gender, string dominantHand, string age, string experimentNumber)
        {
            if (participantNumber == "" || username == "" || password == "" || team == "" || privilege == "" || gender == "" || dominantHand == "" || age == "" || experimentNumber == "")
                return false;

            return dal.AddUser(participantNumber, username, password, team, privilege,gender,dominantHand,age,experimentNumber);
        }

        //Edit user and save the changes in the database
        public bool EditUser(string participantNumber, string username, string password, string team, string privilege, string gender, string dominantHand, string age, string experimentNumber)
        {
            return dal.EditUser(participantNumber, username, password, team, privilege, gender, dominantHand, age, experimentNumber);

        }

        //Return a list of all the examinees, not administrators
        public List<ExamineepProfile> GetExamineeProfileList()
        {
            return dal.GetExamineeProfileList();
        }

        //Delete user from the database
        public bool DeleteUser(string username)
        {

            if (dal.DeleteUser(username) == true)
                return true;
            else
                return false;
        }

        //Get the user by his participant number
        public User GetUserByParticipantNumber(string participantNumber)
        {
            return dal.GetUserByParticipantNumber(participantNumber);
        }

        //Check if the user name or participant number exsit in the users table
        public bool CheckIfUserExists(string name,string participantNumber)
        {

            if (dal.CheckIfUserOrParticipantNumberExist(name,participantNumber) == true)
                return true;
            else
                return false;
        }



    }
}
