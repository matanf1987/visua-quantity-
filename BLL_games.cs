using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Xml;

namespace TryWPF
{
    class BLL_games
    {
        //Global params
        public List<Game> _gamesList = new List<Game>();//The list hold the user answers for each single game
        DAL _dal = new DAL();


        //Insert statistic to the statistic table
        public bool InsertGameStatistics(string username, string gameName, string date, string title, string details)
        {
            return _dal.InsertGameStatistics(username, gameName, date, title, details);

        }

        //Update the game status, the status look like this (stage_level)
        public bool UpdateUserGameStatus(string userName, string gameName, string status)
        {
            return _dal.UpdateUserGameStatus(userName, gameName, status);

        }
        
        //Update the value in the table games_global_time of a game
        public bool UpdateGlobalGameTime(string userName, string gameName, string gameGlobalTime)
        {
            return _dal.UpdateGlobalGameTime(userName, gameName, gameGlobalTime);

        }

        //Return the global play time of a game
        public string GetGameGlobalTime(string userName, string gameName)
        {
            string gameGlobalTime = _dal.GetGameGlobalTime(userName, gameName);
            if (gameGlobalTime == "")
            {
                return "-1";
            }
            else
            {
                return gameGlobalTime;
            }
        }

        //Return the stage and the level of the user current geme
        public string GetStageAndLevel(string userName, string gameName)
        {
            return _dal.GetStageAndLevel(userName, gameName);
        }

        //Get the time that left to play of a game from the dates table
        public string GetGameTime(string userName, string gameName, string day)
        {
            return _dal.GetGameTime(userName, gameName, day);
        }

        //Update game time left to play of the current day of the choosen in the dates table
        public bool UpdateGameTime(string username, string gameName, string time, string day)
        {
            return _dal.UpdateGameTime(username, gameName, time,day);
        }


        //Get the number of rectangle the user pressed
        //Not in use for now
        public int GetTheRectangleNumber(double x, double y,int rawSize,int calSize,int rectangleWidth,int rectangleHeight)
        {
            try
            {
                int rectNum = 0;
                for (int i = 0; i < rawSize; i++)
                {
                    for (int j = 0; j < calSize; j++)
                    {
                        rectNum++;
                        if (x > rectangleWidth * j && x < rectangleWidth * (j + 1))
                            if (y > rectangleHeight * i && y < rectangleHeight * (i + 1))
                                return rectNum;
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("GetTheRectangleNumber failed", "שגיאה");
            }

            return 0;
        }


        //Return the param value from the config.txt file
        public string GetParamFromCfgFile(string paramName)
        {
            try
            {
                if (paramName == "")
                    return "";

                string[] lines = System.IO.File.ReadAllLines(@"config.txt");
                string[] paramArray;
                foreach (string line in lines)
                {
                    paramArray = line.Split('=');
                    if (paramArray[0] == paramName)
                        return paramArray[1];
                }
            }
            catch (Exception)
            {
                MessageBox.Show("GetParamFromCfgFile failed", "שגיאה");
            }
            return "";

        }


        //Generate a random number
        public int GenerateNumber(int maxNum)
        {
            try
            {
                Random rnd = new Random();
                int number = rnd.Next(1, maxNum + 1); // creates a number between 1 and the max number

                //Add a function that check if the number exists before.

                return number;
            }
            catch (Exception)
            {
                MessageBox.Show("GenerateNumber failed", "שגיאה");
            }

            return 0; /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////check ittttttttttttttttt

        }

        //Sky game
        //Check if the image was chosen
        public bool SpecialImageWasGenerate(int generatedNumber, List<int> specialImagesList)
        {
            try
            {
                foreach (int value in specialImagesList)
                {
                    if (generatedNumber == value)
                        return true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SpecialImageWasGenerate failed", "שגיאה");
            }
            return false;

        }


        //Check if the cell of a circle was chosen
        public bool SpecialCellWasGenerate(int generatedNumber, List<int> specialCellsList)
        {

            try
            {
                foreach (int value in specialCellsList)
                {
                    if (generatedNumber == value)
                        return true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("SpecialCellWasGenerate failed", "שגיאה");
            }
            return false;
        }


        
        //Generate a numbers of pictures and positions for the test stage
        public string GenerateTestVillageGame(int N, string gameName, int fribbleImagesNumber, List<int> specialCellsList, List<int> specialImagesList)
        {
            try
            {
                Random rnd = new Random();
                int number = rnd.Next(1, 3); // creates a number between 1 and the size of the board
                int cellNumber = 0;
                int imageNumber = 0;
                string result = "0";
                //number = 1;
                if (number == 1)//cell number or image number that were chosen N turns before
                {

                    result = "1";
                    if (gameName == "Village game location")
                    {
                        cellNumber = specialCellsList[specialCellsList.Count - N];
                        imageNumber = GenerateNumber(12);
                    }
                    else if (gameName == "Village game identity") 
                    {
                        imageNumber = specialImagesList[specialImagesList.Count - N];
                        cellNumber = GenerateNumber(12);

                    }

                }
                else//cell number or image number that were not chosen N turns before
                {
                    result = "2";
                    if (gameName == "Village game location")
                    {
                        do//cell number that chosen in the learning stage
                        {
                            cellNumber = GenerateNumber(12);
                            ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                        } while (cellNumber == specialCellsList[specialCellsList.Count - N]);
                        imageNumber = GenerateNumber(12);

                    }
                    else if (gameName == "Village game identity")
                    {
                        do//cell number that chosen in the learning stage
                        {
                            imageNumber = GenerateNumber(12);
                            ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                        } while (imageNumber == specialImagesList[specialImagesList.Count - N]);
                        cellNumber = GenerateNumber(12);

                    }
                }
                return "" + cellNumber + "_" + imageNumber + "_" + result;
            }
            catch (Exception)
            {
                MessageBox.Show("GenerateTestVillageGame failed", "שגיאה");
            }

            return "";////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////check it
        }
        
        //Generate a number for the test stage
        public string GenerateTestSkyGame(int boardSize,int fribbleImagesNumber,List<int> specialCellsList, List<int> specialImagesList)
        {
            try
            {
                Random rnd = new Random();
                int number = rnd.Next(1, 3); // creates a number between 1 and the size of the board
                int cellNumber = 0;
                int imageNumber = 0;
                int tempNumber = 0;
                if (number == 1)//cell number and image number that were chosen in the learning game
                {


                    do//cell number that chosen in the learning stage
                    {
                        cellNumber = GenerateNumber(boardSize);
                        ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                    } while (!SpecialCellWasGenerate(cellNumber, specialCellsList));
                    tempNumber = specialCellsList.IndexOf(cellNumber);
                    imageNumber = specialImagesList[tempNumber];

                }
                else//Cell number that didn't choose in the learning stage
                {

                    //There are 3 options
                    //1 - cell in place that preformed and picture that did not prefomed
                    //2 - cell in place that did not preformed and picture that prefomed
                    //3 - cell in place that did not preformed and picture that did not prefomed
                    number = rnd.Next(1, 4);

                    if (number == 1)
                    {
                        do//cell number that chosen in the learning stage
                        {
                            cellNumber = GenerateNumber(boardSize);
                            ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                        } while (!SpecialCellWasGenerate(cellNumber, specialCellsList));

                        do//image number that didn't choose in the learning stage
                        {
                            imageNumber = GenerateNumber(fribbleImagesNumber);

                            ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                        } while (SpecialImageWasGenerate(imageNumber, specialImagesList));

                    }
                    else if (number == 2)
                    {
                        do//cell number that didn't chosen in the learning stage
                        {
                            cellNumber = GenerateNumber(boardSize);
                            ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                        } while (SpecialCellWasGenerate(cellNumber, specialCellsList));

                        do//image number that choosen in the learning stage
                        {
                            imageNumber = GenerateNumber(fribbleImagesNumber);

                            ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                        } while (!SpecialImageWasGenerate(imageNumber, specialImagesList));
                    }
                    else
                    {
                        do//cell number that didn't chosen in the learning stage
                        {
                            cellNumber = GenerateNumber(boardSize);
                            ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                        } while (SpecialCellWasGenerate(cellNumber, specialCellsList));

                        do//image number that didn't choose in the learning stage
                        {
                            imageNumber = GenerateNumber(fribbleImagesNumber);

                            ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                        } while (SpecialImageWasGenerate(imageNumber, specialImagesList));

                    }
                }
                return "" + cellNumber + "_" + imageNumber;
            }
            catch (Exception)
            {
                 MessageBox.Show("GenerateTestSkyGame failed", "שגיאה");
            }

            return "";////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////check it
        }


        //Generate a number for the test stage
        //Make sure that the statistic of the times the circle preformed or not will be around 50 percent
        public int GenerateTest(int boardSize, List<int> specialCellsList)
        {
            try
            {
                Random rnd = new Random();
                int number = rnd.Next(1, 3); // creates a number between 1 and the size of the board

                if (number == 1)//cell number was chosen in the learning game
                {
                    do
                    {
                        number = GenerateNumber(boardSize);
                        ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                    } while (!SpecialCellWasGenerate(number, specialCellsList));
                }
                else//cell number wasn't chosen in the learning game
                {
                    do
                    {
                        number = GenerateNumber(boardSize);
                        ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                    } while (SpecialCellWasGenerate(number, specialCellsList));
                }
                return number;
            }
            catch (Exception)
            {
                MessageBox.Show("GenerateTest failed", "שגיאה");
            }
            return 0;////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////chcek it
        }

        //Return if the Circle preformed in the learning stage
        //value - the answer of the user
        //specialCellsList - the cells the pictures performed in
        //imagelCellsList - the number of the photos list(EXAMPLE - in the second cell the number of the picture there is the number of the picture that prefomed in the second time of learning time)
        //criclePerformed - the number of cell the image preformed in the test part
        //imagePerformed - the number of photo in the test part
        public bool PicturePerformedInLearningStageSkyGame(bool value, List<int> specialCellsList, List<int> imagelCellsList, int cirlePerformed, int imagePerformed)
        {
            try
            {
                int counter = 0;
                foreach (int cell in specialCellsList) // Loop through List with foreach
                {
                    if (cell == cirlePerformed && imagelCellsList[counter] == imagePerformed)//If the circle performed and the user said it preformed
                    {
                        if (value == true)
                            return true;
                        else
                            return false;
                    }
                    counter++;
                }

                if (value == true)//If the circle didn't perfomed and the user said it prefomed
                    return false;
                else //If the circle didn't perfomed and the user said it didn't perfomed
                    return true;
            }
            catch (Exception)
            {
                 throw;
            }

        }

        //Return if the Circle preformed in the learning stage
        public bool CirclePerformedInLearningStage(bool value, List<int> specialCellsList, int criclePerformed)
        {
            try
            {
                foreach (int cell in specialCellsList) // Loop through List with foreach
                {
                    if (cell == criclePerformed)//If the circle performed and the user said it preformed
                    {
                        if (value == true)
                            return true;
                        else
                            return false;
                    }
                }

                if (value == true)//If the circle didn't perfomed and the user said it prefomed
                    return false;
                else //If the circle didn't perfomed and the user said it didn't perfomed
                    return true;
            }
            catch (Exception)
            {
                throw;
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
            }

            return stage;//Return null object if stage does not exists ///////////////////////////////////////////////////////////////////////////////////////

        }

        //Return the grid raw size
        public int GetRawSize(int boardSize)
        {

            if (boardSize <= 12)
                return 3;
            else if (boardSize <= 20)
                return 4;
            else
                return 5;

        }

        //Return the grid cal size
        public int GetCalSize(int boardSize)
        {
            if (boardSize <= 9)
                return 3;
            else if (boardSize > 9 && boardSize <= 16)
                return 4;
            else
                return 5;

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
                 throw;
            }
        }


    }
}
