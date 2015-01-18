using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;
using System.Xml;

namespace TryWPF
{
    /// <summary>
    /// Interaction logic for Treasure_map.xaml
    /// </summary>
    /// 

    public partial class Treasure_map : Window
    {

        //Global params
        int _rectangleWidth = 0;//Each rectangel width(In the grid)
        int _rectangleHeight = 0;//Each rectangel Height(In the grid)
        int _rawSize = 9;//Number of raws in the grid
        int _calSize = 9;//Number of cals in the grid
        int _numberOfGUessing = 0;//The number of times the user can/need to answer
        int _gameTime = 720;//Game time
        int _gameTimer = 0;//The remaining time of the game
        //bool _timerIsRunning = true;//If the timer is running
        //String _gameName = "Treasure map game";//The name of the game
        string _gameName = "Treasure map reverse game";//The name of the game for the xml file
        string _gameMode = "experiment";//The mode of the game, can be experiment or tutorial
        int _stageNumber = 1;//Number of stage
        int _levelNumber = 2;//The number of board that need to be shown on the current stage
        string _gameImage = "";//The name of the game image
        int _currentWinsInARaw = 0;//Number of wins in a raw
        int _currentlosesInARaw = 0;//Number of loses in a raw
        int _rectangleDisplayTime = 999;//The time the rectangle will be display on the screen
        int _clearBoardDisplayTime = 500;//The time the board is clear between two rectangles
        Rectangle rectangle;//The rectangle object
        Ellipse ellipse;//The ellipse object
        SolidColorBrush myBrush;//The color object
        Boolean lockTheBoard = true;//Locking the screen while the learning stage is running
        List<int> _specialCellsList = new List<int>();//The list hold the special cells for each single game
        List<int> _markedRectangles = new List<int>();//The list hold the special cells for each single game that were marked
        List<int> _userAnswersList = new List<int>();//The list hold the user answers for each single game
        List<Game> _gamesList;
        BackgroundWorker m_oWorker;
        Stage _currentStage = null;
        int _boardSize;
        int _winsInARow;//The number of wins in a raw, after x wins(The param is set in the configuration file) the user advance to the next stage
        int _firstLevel;
        int _lastLevel;
        int _criclePerformed = 0;
        System.Windows.Threading.DispatcherTimer Timer;
        BLL_games _bll;
        bool _puaseGame = false;
        int _drawBoardTime = 0;//The number of time the board is draw
        bool _administratorMode = false;
        bool _problemHasOccurred = false;//True in case problem has occured, exit to the main window with an problem message
        bool _gameOver = false;//A flag, declare the game is over
        bool _m_oWorker_ready_to_use = true;//If the m_oWorker is ready to start again, use for the start new game after pause

        //Params for database information
        string _DBgameName = "";
        string _DBday = "";
        string _DBuserName = "";

        public Treasure_map()
        {
             initGame();
             _bll = new BLL_games();
        }

        //Constructor for administrator mode
        public Treasure_map(string gameMode, string gameName, int stageNumber, int levelNumber)
        {
            _administratorMode = true;
            _bll = new BLL_games();
            _gameName = gameName;
            _stageNumber = stageNumber;
            _levelNumber = levelNumber;
            _gameMode = gameMode;//Can be experiment or tutorial

            initGame();
        }

        //Constructor for examinee mode
        public Treasure_map(string gameName, string username, string day)
        {

            _administratorMode = false;
            _bll = new BLL_games();
            _gameName = gameName;
   
            if (_gameName == "Treasure map game")
            {
                _DBgameName = "treasureMapGame";
            }
            else if (_gameName == "Treasure map reverse game")
            {
                _DBgameName = "treasureMapReverseGame";
            }

            _DBuserName = username;
            _DBday = day;

            string stageAndLevelFromDB = _bll.GetStageAndLevel(_DBuserName,_DBgameName );
            //If the stage and level can't be found
            if (stageAndLevelFromDB == "")
            {
                problemHasOccurred("בשל בעיה בהבאת סטטוס השלב הינך מועבר חזרה לתפריט הראשי");
            }
            string[] stageAndLevelArray = stageAndLevelFromDB.Split('_');
            _stageNumber = Convert.ToInt32(stageAndLevelArray[0]);
            _levelNumber = Convert.ToInt32(stageAndLevelArray[1]);
            _gameTime = Convert.ToInt32(_bll.GetGameTime(_DBuserName, _DBgameName, _DBday)) * 60;
            _gameMode = "experiment";//Can be experiment or tutorial

            initGame();
        }

           
        private void TutorialGameMode()
        {
            scoreGrid.Visibility = Visibility.Hidden;
            timeGrid.Visibility = Visibility.Hidden;
            gameNameLabel.Visibility = Visibility.Hidden;
            playPuaseImages.Visibility = Visibility.Hidden;
        }


        private void initGame()
        {

            InitializeComponent();
            //The bll object initialize 

            if (_gameMode == "tutorial")
            {
                TutorialGameMode();
            }

          
            string param1 = "";
            string param2 = "";
            //string param3 = "";
            if (_gameName == "Treasure map game")
            {
                param1 = _bll.GetParamFromCfgFile("TM_rectangleDisplayTime");
                param2 = _bll.GetParamFromCfgFile("TM_clearBoardDisplayTime");
                //param3 = _bll.GetParamFromCfgFile("TM_gameTime");
            }
            else if (_gameName == "Treasure map reverse game")
            {
                param1 = _bll.GetParamFromCfgFile("TMR_rectangleDisplayTime");
                param2 = _bll.GetParamFromCfgFile("TMR_clearBoardDisplayTime");
                //param3 = _bll.GetParamFromCfgFile("TMR_gameTime");
            }

            if (param1 != "")
                _rectangleDisplayTime = Convert.ToInt32(param1);
            else
                problemHasOccurred("הבאת המידע מהקובץ הקונפיג נכשלה פנה למנהל המערכת");

            if (param2 != "")
                _clearBoardDisplayTime = Convert.ToInt32(param2);
            else
                problemHasOccurred("הבאת המידע מהקובץ הקונפיג נכשלה פנה למנהל המערכת");


            _gamesList = _bll._gamesList;

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;



            try
            {
                if (_gameName == "Treasure map game")
                {
                    gameNameLabel.Content = "משחק מפת האוצר";
                    img.UriSource = new Uri(@"images/Treasure map/bg.jpg", UriKind.Relative);
                }
                else if (_gameName == "Treasure map reverse game")
                {
                    gameNameLabel.Content = "משחק מפת האוצר ההפוך";
                    img.UriSource = new Uri(@"images/Treasure map/bg_r.jpg", UriKind.Relative);
                }
                img.EndInit();
                backgroundImage.Source = img;
            }
            catch (Exception){}
            
            
     

            //Timer
            _gameTimer = _gameTime;
            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 1);//Each second
            Timer.Start();
            UpdtaeTimerLabel();
            canvas.Background = new SolidColorBrush(Colors.Transparent);




            try
            {
                _bll.ReadXml();//Read the xml file, use for the bll class
            }
            catch (Exception)
            {
                throw;
            }

            //Start the backgroung worker 
            initBGWorker();
            initBoard();
        }

        //Keyboard pressing listener
        public void keyDownEventHandler(object sender, KeyEventArgs e)
        {


            if (_administratorMode)
            {
                if (e.Key == Key.Escape)
                {
                    MessageBoxResult result = MessageBox.Show("האם ברצונך לצאת מהמשחק?", "אזהרה", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //Return to the main menu
                        ADM_Games_Menu menu = new ADM_Games_Menu();
                        this.Close();
                        menu.Show();
                    }

                }
            }
 
        }

        private void problemHasOccurred(string message)
        {
            MessageBox.Show(message, "שגיאה");
            throw new Exception("");
        }

        //Pressed on the play pause image listener change the mode betwewn play and puase
        //private void imagePlayPuase_MouseUp(object sender, MouseButtonEventArgs e)
        //{

        //    try
        //    {
        //        Timer.Stop();
        //        m_oWorker.CancelAsync();
        //        MessageBox.Show("Game paused." + '\n' + "Press OK to continue.", "MESSAGE", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        initBoard();
        //    }
        //    catch (Exception)
        //    {
        //        problemHasOccurred("imagePlayPuase_MouseUp failed");
        //    }

        //    //System.Windows.Media.Imaging.BitmapImage newImage = new System.Windows.Media.Imaging.BitmapImage();
        //    //newImage.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;
        //    //newImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
        //    //Uri urisource = null;
        //    //if (_timerIsRunning == true)//If the timer is running(toggle to pause mode)
        //    //{
        //    //    Timer.Stop();
        //    //    //urisource = new Uri(@"Images/Dot matrix/play.png", UriKind.Relative);
        //    //    _timerIsRunning = false;

        //    //}
        //    //else//If the timer is not running(toggle to play mode)
        //    //{
        //    //    Timer.Start();
        //    //    //urisource = new Uri(@"Images/Dot matrix/pause.png", UriKind.Relative);
        //    //    _timerIsRunning = true;
        //    //}

        //    //newImage.BeginInit();
        //    //newImage.UriSource = urisource;
        //    //newImage.EndInit();
        //    //playPuaseImages.Source = newImage;

        //}

        //Pressed on the play pause image listener change the mode betwewn play and puase
        private void imagePlayPuase_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                playPuaseImages.IsEnabled = false;
                _puaseGame = true;
                Timer.Stop();
                m_oWorker.CancelAsync();
                _m_oWorker_ready_to_use = false;
                Thread.Sleep(3000);
                resultLabel.Content = "תור חדש";
                MessageBox.Show("Game paused." + '\n' + "Press OK to continue.", "MESSAGE", MessageBoxButton.OK, MessageBoxImage.Warning);
                initBoard();
            }
            catch (Exception)
            {
                problemHasOccurred("imagePlayPuase_MouseUp failed");
            }
        }

        private void ExitToMenu(string message)
        {
            MessageBox.Show(message, "שגיאה");
            if (_administratorMode == true)
            {
                ADM_Games_Menu menu = new ADM_Games_Menu();
                this.Close();
                menu.Show();
            }
            else
            {
                Exam_menu menu = new Exam_menu(_DBuserName, "problem has occurred");
                this.Close();
                menu.Show();
            }
        }

        //Update the timer image
        public void updateTimeCircle()
        {
            if (_gameTimer < 0)//Prevent from the time circle to be under 0
                return;

            double x = (double)_gameTime / 60;
            double y = (double)_gameTimer / x;
            Rect myRect = new Rect(0, 60 - y, 66, 60);
            timeCircle.Rect = myRect;
        }

        //Handle the timer event, every second
        private void Timer_Tick(object sender, EventArgs e)
        {

            UpdtaeTimerLabel();

            // _bll.UpdateUserGameStatus("yona", "robotGame", "5_3");//Update the stage and level in the database(games_status table)
            //_bll.UpdateGlobalGameTime("yona", "robotGame", "3");
            //string gameGlobalTime = _bll.GetGameGlobalTime("yona", "robotGame");
            //int increaseGameGlobalTimeByOne = Convert.ToInt32(gameGlobalTime) + 1;
            //gameGlobalTime = "" + increaseGameGlobalTimeByOne;
            if (_administratorMode == false)//The user is examinee
            {

                if (_problemHasOccurred == true)
                {
                    MessageBox.Show("חלה בעיה בעדכון הנתונים, פנה למנהל המערכת");
                    Exam_menu admGM = new Exam_menu(_DBuserName, "problem has occurred");
                    this.Close();
                    admGM.Show();
                }

                if (_gameOver == true)
                {
                    Timer.Stop();
                    if (_gameMode == "experiment")
                    {
                        MessageBox.Show("Game over");
                        Exam_menu admGM = new Exam_menu(_DBuserName, "game complete");
                        this.Close();
                        admGM.Show();
                    }
                }

                if (_gameTimer % 60 == 0)//If 
                {
                    int updateTime = (int)(_gameTimer / 60);
                    try
                    {
                        Thread thread = new Thread(() => updateStatistics(updateTime));
                        thread.Start();
                    }
                    catch (Exception)
                    {
                        problemHasOccurred("עדכון הסטטיסטיקות נכשל");
                    }
                }

            }
            else if (_administratorMode == true)//The user is examinee
            {

                if (_gameTimer <= 0)
                {
                    Timer.Stop();
                    if (_gameMode == "experiment")
                    {
                        MessageBox.Show("Game over");
                        ADM_Games_Menu admGM = new ADM_Games_Menu();
                        this.Close();
                        admGM.Show();
                    }
                }

            }

            updateTimeCircle();
        }

        private void updateStatistics(int updateTime)
        {
            int examineeTotalGameTime = 0;
            bool updatesValid = true;

            try
            {

                if (_bll.UpdateGameTime(_DBuserName, _DBgameName, "" + updateTime, _DBday) == false)
                {
                    updatesValid = false;
                }
                if (_bll.UpdateUserGameStatus(_DBuserName, _DBgameName, "" + _stageNumber + "_" + _levelNumber) == false)//Update the stage and level in the database(games_status table)
                {
                    updatesValid = false;
                }

                int gameGlobalTime = Convert.ToInt32(_bll.GetGameGlobalTime(_DBuserName, _DBgameName));
                if (gameGlobalTime == -1)
                {
                    updatesValid = false;
                }
                examineeTotalGameTime = gameGlobalTime + 1;

                if (_bll.UpdateGlobalGameTime(_DBuserName, _DBgameName, "" + examineeTotalGameTime) == false)
                {
                    updatesValid = false;
                }


                string dt = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
                //Update the 4 minutes statistic in the database for the current examinee
                if (examineeTotalGameTime == 4)
                {
                    if (_bll.InsertGameStatistics(_DBuserName, _DBgameName, dt, "First 4 minutes", "" + _stageNumber + "_" + _levelNumber) == false)
                    {
                        updatesValid = false;
                    }
                }

                //Update the 4 minutes statistic in the database for the current examinee
                if (examineeTotalGameTime != 0 && examineeTotalGameTime % 6 == 0)///////GAME TOTOAL TIME
                {
                    if (_bll.InsertGameStatistics(_DBuserName, _DBgameName, dt, "6 minutes", "" + _stageNumber + "_" + _levelNumber) == false)
                    {
                        updatesValid = false;
                    }
                }
            }
            catch (Exception)
            {
                _problemHasOccurred = true;
            }

            if (updatesValid == false)
            {
                _problemHasOccurred = true;
            }

            if (updateTime == 0)
            {
                _gameOver = true;
                // if (_gameTimer <= 0)//Prevent from the time to be under 00:00 in the clock;
                //return;
            }
        }


        //Updates the timer label
        public void UpdtaeTimerLabel()
        {
            try
            {
                if (_gameTimer < 0)//Prevent from the time to be under 00:00 in the clock;
                    return;

                TimeSpan t = TimeSpan.FromSeconds(_gameTimer);
                string answer = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
                _gameTimer -= 1;
                //_gameTimer--;
                timerLable.Content = "שעון: " + answer;
            }
            catch (Exception)
            {
                 problemHasOccurred("UpdtaeTimerLabel failed");
            }
        }



        //Init the board in each game
        private void initBoard()
        {
            try
            {
                _currentStage = _bll.getStageDetails(_gameName, _stageNumber);//Return the stage details
                _numberOfGUessing = 0;
                _specialCellsList.Clear();//Clear the list
                _userAnswersList.Clear();//Clear the list
                _markedRectangles.Clear();//Clear the list
                _boardSize = _currentStage._boardSize;
                _winsInARow = Convert.ToInt32(_currentStage._winsInARow); ;//The number of wins in a raw, after x wins(The param is set in the configuration file) the user advance to the next stage
                _firstLevel = Convert.ToInt32(_currentStage._firstLevel);
                _lastLevel = Convert.ToInt32(_currentStage._lastLevel);
                _gameImage = _currentStage._robotImage;
                //_rawSize = _bll.GetRawSize(_boardSize);
                //_calSize = _bll.GetCalSize(_boardSize);
                _rectangleWidth = (int)canvas.Width / _calSize;
                _rectangleHeight = (int)canvas.Height / _rawSize;

                //playPuaseImages.IsEnabled = true;
                Timer.Start();//Starts the timer event
                stageLabel.Content = "שלב: " + _stageNumber;
                statusLabel.Content = "זכור את מיקום הריבועים";
                statusLabel.Foreground = Brushes.DarkBlue;
                levelLabel.Content = "רמה: " + "" + _levelNumber;
                winsInARawLable.Content = "הצלחות ברצף: " + _currentWinsInARaw;


                //Change game image
                System.Windows.Media.Imaging.BitmapImage newImage = new System.Windows.Media.Imaging.BitmapImage();
                newImage.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;
                newImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
                Uri urisource = new Uri(@"Images/Treasure map game/" + _gameImage, UriKind.Relative);
                newImage.BeginInit();
                newImage.UriSource = urisource;
                newImage.EndInit();
                gameImage.Source = newImage;
                CreateSpecialCellsList();//Create the game board preformed cells
                _drawBoardTime = 0;


                //Start the backgroung worker 
                initBGWorker();
                _puaseGame = false;
                playPuaseImages.IsEnabled = true;
                if (_m_oWorker_ready_to_use == false && m_oWorker.IsBusy == true)
                {
                    ExitToMenu("אתחול הלוח נכשל");
                }
                m_oWorker.RunWorkerAsync("" + _levelNumber);
            }
            catch (Exception)
            {
                ExitToMenu("אתחול הלוח נכשל");
            }
        }

        //Create the game board preformed cells
        private void CreateSpecialCellsList()
        {
            try
            {
                int specialCellNumber = 0;
                for (int i = 0; i < _boardSize; i++)
                {
                    do
                    {
                        specialCellNumber = _bll.GenerateNumber(_rawSize * _calSize);

                    } while (_bll.SpecialCellWasGenerate(specialCellNumber, _specialCellsList) || checkIfTwoRectanglesNextToEachOther(specialCellNumber));
                    _specialCellsList.Add(specialCellNumber);
                }
            }
            catch (Exception)
            {
               problemHasOccurred("CreateSpecialCellsList failed");
            }

        }

        private bool checkIfTwoRectanglesNextToEachOther(int specialCellNumber)
        {

            try
            {
                if (_specialCellsList.IndexOf(specialCellNumber - 10) != -1 || _specialCellsList.IndexOf(specialCellNumber + 10) != -1 ||
                          _specialCellsList.IndexOf(specialCellNumber - 1) != -1 || _specialCellsList.IndexOf(specialCellNumber + 1) != -1 ||
                               _specialCellsList.IndexOf(specialCellNumber - 9) != -1 || _specialCellsList.IndexOf(specialCellNumber + 9) != -1 ||
                                   _specialCellsList.IndexOf(specialCellNumber - 8) != -1 || _specialCellsList.IndexOf(specialCellNumber + 8) != -1)
                    return true;

                return false;
            }
            catch (Exception)
            {
                problemHasOccurred("checkIfTwoRectabglesNextToEachOther failed");
            }

            return false;
        }
        private void initBGWorker()
        {
            //Background worker - Handle the logic while the gui is running
            try
            {
                m_oWorker = null;
                m_oWorker = new BackgroundWorker();
                m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
                m_oWorker.ProgressChanged += new ProgressChangedEventHandler(m_oWorker_ProgressChanged);
                m_oWorker.WorkerReportsProgress = true;
                m_oWorker.WorkerSupportsCancellation = true;
            }
            catch (Exception)
            {
                problemHasOccurred("initBGWorker failed");
            }

        }

        private void CanvasMouseDown(object sender, MouseButtonEventArgs e)
        {

            try
            {
                if (lockTheBoard == true)//Prevent from pressing the board while the computer shows the game boards("Computer turn")
                    return;

                Point p = Mouse.GetPosition(canvas);

                int rectangleNumber = _bll.GetTheRectangleNumber(p.X, p.Y, _rawSize, _calSize, _rectangleWidth, _rectangleHeight);
                //MessageBox.Show(""+rectangleNumber); 
                if (_specialCellsList.IndexOf(rectangleNumber) != -1)//If the chosen rectangle preformed on the board as black rectable
                {
                    if (_userAnswersList.IndexOf(rectangleNumber) == -1)//If the answer did not choose before in this current game
                    {
                        _numberOfGUessing++;
                        MarkRectangle(rectangleNumber);
                        _userAnswersList.Add(rectangleNumber);
                    }
                }

                if (_numberOfGUessing == _levelNumber)
                {


                    if (_gameMode == "experiment")
                    {
                        if (_gameName == "Treasure map reverse game")
                        {
                            _userAnswersList.Reverse();//Reverse the answer of the user
                        }

                        if (_markedRectangles.SequenceEqual(_userAnswersList))//Correct answer
                        {
                            _currentWinsInARaw++;
                            _currentlosesInARaw = 0;
                            resultLabel.Content = "כל הכבוד!!!";
                        }
                        else//Wrong answer
                        {
                            _currentWinsInARaw = 0;
                            _currentlosesInARaw++;
                            resultLabel.Content = "תשובה לא נכונה";
                        }
                    }


                    initBGWorker();
                    _puaseGame = false;
                    m_oWorker.RunWorkerAsync("end game");
                }
            }
            catch (Exception)
            {
                  problemHasOccurred("CanvasMouseDown failed");
            }
            
        }

        private void MarkRectangle(int rectangleNumber)
        {
            try
            {
                int counter = 0;
                //canvas.Children.Clear();
                for (int i = 0; i < _rawSize; i++)
                {
                    for (int j = 0; j < _calSize; j++)
                    {

                        counter++;
                        if (counter == rectangleNumber)
                        {
                            rectangle = new Rectangle();
                            rectangle.StrokeThickness = 2;
                            rectangle.Width = _rectangleWidth;
                            rectangle.Height = _rectangleHeight;
                            rectangle.Stroke = Brushes.White;
                            myBrush = new SolidColorBrush(Colors.Blue);
                            rectangle.Fill = myBrush;
                            Canvas.SetTop(rectangle, _rectangleHeight * i);
                            Canvas.SetLeft(rectangle, _rectangleWidth * j);
                            canvas.Children.Add(rectangle);

                        }
                    }
                }
            }
            catch (Exception)
            {
                 problemHasOccurred("MarkRectangle failed");
            }
        }
    




        //Draw the test stage grid
        public void DrawBoard_test()
        {
            //_criclePerformed = _bll.GenerateTest(_boardSize, _specialCellsList);
            try
            {
                int cellCounter = 0;
                for (int i = 0; i < _rawSize; i++)
                {
                    for (int j = 0; j < _calSize; j++)
                    {
                        cellCounter++;
                        if (_specialCellsList.IndexOf(cellCounter) != -1)
                        {
                            rectangle = new Rectangle();
                            rectangle.StrokeThickness = 2;
                            rectangle.Width = _rectangleWidth;
                            rectangle.Height = _rectangleHeight;
                            rectangle.Stroke = Brushes.Black;
                            myBrush = new SolidColorBrush(Colors.Black);
                            rectangle.Fill = myBrush;
                            Canvas.SetTop(rectangle, _rectangleHeight * i);
                            Canvas.SetLeft(rectangle, _rectangleWidth * j);
                            canvas.Children.Add(rectangle);
                        }


                    }

                }
            }
            catch (Exception)
            {
                 problemHasOccurred("DrawBoard_test failed");
            }

        }

        //Draw the learning stage grid
        private void DrawBoard_learning(Boolean drawSpecialCell)
        {

            try
            {
                int cellCounter = 0;
                for (int i = 0; i < _rawSize; i++)
                {
                    for (int j = 0; j < _calSize; j++)
                    {
                        cellCounter++;

                        if (_specialCellsList[_drawBoardTime] == cellCounter && drawSpecialCell)
                        {
                            rectangle = new Rectangle();
                            rectangle.StrokeThickness = 2;
                            rectangle.Width = _rectangleWidth;
                            rectangle.Height = _rectangleHeight;
                            rectangle.Stroke = Brushes.Black;
                            myBrush = new SolidColorBrush(Colors.Lime);
                            rectangle.Fill = myBrush;
                            Canvas.SetTop(rectangle, _rectangleHeight * i);
                            Canvas.SetLeft(rectangle, _rectangleWidth * j);
                            canvas.Children.Add(rectangle);

                            _markedRectangles.Add(cellCounter);//Add to the list the number of rectangle that was marked
                        }
                        else if (_specialCellsList.IndexOf(cellCounter) != -1)
                        {
                            rectangle = new Rectangle();
                            rectangle.StrokeThickness = 2;
                            rectangle.Width = _rectangleWidth;
                            rectangle.Height = _rectangleHeight;
                            rectangle.Stroke = Brushes.Black;
                            myBrush = new SolidColorBrush(Colors.Black);
                            rectangle.Fill = myBrush;
                            Canvas.SetTop(rectangle, _rectangleHeight * i);
                            Canvas.SetLeft(rectangle, _rectangleWidth * j);
                            canvas.Children.Add(rectangle);
                        }
                    }
                }

                if (drawSpecialCell == true)//To prevent to increase the param when the board is draw without showing marked rectangle in the learning stage
                    _drawBoardTime++;
            }
            catch (Exception)
            {
                problemHasOccurred("DrawBoard_learning failed");
            }

        }

        //The backgroundworker event, will be calling with the code m_oWorker_Dot_matrix.ReportProgress
        void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            try
            {
                if (_puaseGame == true)
                    return;

                canvas.Children.Clear();
                // MessageBox.Show("" + e.ProgressPercentage)
                if (m_oWorker.CancellationPending)
                {
                    m_oWorker.Dispose();
                    _puaseGame = true;
                    GC.Collect();
                    return;
                }
                if (e.ProgressPercentage == 0)//The stage before the learning stage starts, preforme the winning or losing label
                {
                    //DrawBoard_learning_Dot_matrix(false);
                    if ((string)resultLabel.Content == "תוצאה")//Make sure that the label wont be visible in the first turn of the game(In that case there is no result)
                    {
                        resultLabel.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        resultLabel.Visibility = Visibility.Visible;
                    }
                }
                else if (e.ProgressPercentage == 1)//The stage before the learning stage starts
                {
                    DrawBoard_learning(false);
                    resultLabel.Visibility = Visibility.Hidden;
                }
                //The learning stage 
                else if (e.ProgressPercentage == 2)
                {
                    DrawBoard_learning(true);
                    resultLabel.Visibility = Visibility.Hidden;
                }
                else if (e.ProgressPercentage == 3)
                {
                    DrawBoard_learning(false);
                    //resultLabel.Visibility = Visibility.Hidden;
                }
                //The test stage
                else if (e.ProgressPercentage == 4)
                {
                    lockTheBoard = false;
                    //playPuaseImages.IsEnabled = false;
                    Timer.Stop();
                    DrawBoard_test();
                    statusLabel.Foreground = Brushes.DarkRed;
                    if (_gameName == "Treasure map game")
                    {
                        statusLabel.Content = "חזור על מיקום הריבועים" + '\n' + "על פי סדר הופעתם";
                    }
                    else if (_gameName == "Treasure map reverse game")
                    {
                        statusLabel.Content = "חזור על מיקום הריבועים" + '\n' + "מהסוף להתחלה";
                    }
                    //yesImage.Visibility = Visibility.Visible;
                    //noImage.Visibility = Visibility.Visible;
                    m_oWorker.CancelAsync();
                    m_oWorker.Dispose();
                    GC.Collect();
                }
                else if (e.ProgressPercentage == 5)
                {
                    updateStageAndLevel();
                    initBoard();
                }
            }
            catch (Exception)
            {
                 problemHasOccurred("m_oWorker_ProgressChanged failed");
            }
        }

        //The backgroundworker lisener start with the code m_oWorker_Dot_matrix.RunWorkerAsync("" + _levelNumber);
        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.
            //NOTE : Never play with the UI thread here...

            try
            {
                if (_puaseGame == true)
                    return;

                string x = e.Argument as string;
                if (x == "end game")
                {
                    Thread.Sleep(200);
                    m_oWorker.ReportProgress(5);
                    return;
                }

                if (m_oWorker.CancellationPending)
                {
                    //m_oWorker_Dot_matrix.Dispose();
                    //m_oWorker_Dot_matrix = null;
                    //GC.Collect();
                    _puaseGame = true;
                    return;
                }
                lockTheBoard = true;
                m_oWorker.ReportProgress(0);
                Thread.Sleep(1500);
                m_oWorker.ReportProgress(1);
                Thread.Sleep(2000);

                for (int i = 0; i < (int)Int32.Parse(x); i++)
                {

                    if (m_oWorker.CancellationPending)
                    {
                        //m_oWorker_Dot_matrix.Dispose();
                        //m_oWorker_Dot_matrix = null;
                        //GC.Collect();
                        //e.Cancel = true;
                        _puaseGame = true;
                        return;
                    }
                    m_oWorker.ReportProgress(2);
                    Thread.Sleep(_rectangleDisplayTime);
                    m_oWorker.ReportProgress(3);
                    Thread.Sleep(_clearBoardDisplayTime);

                }
                if (m_oWorker.CancellationPending)
                {
                    //m_oWorker_Dot_matrix.Dispose();
                    //m_oWorker_Dot_matrix = null;
                    //GC.Collect();
                    _puaseGame = true;
                    return;
                }
                m_oWorker.ReportProgress(4);
            }
            catch (Exception)
            {
                problemHasOccurred("m_oWorker_DoWork failed");
            }
        }

        //Update the stage and level
        public void updateStageAndLevel()
        {
            try
            {
                if (_currentWinsInARaw == _winsInARow)
                {
                    if (_levelNumber < _lastLevel)
                    {
                        _levelNumber++;
                    }
                    else
                    {
                        Stage s = _bll.getStageDetails(_gameName, _stageNumber + 1);
                        if (s != null)//If the stage exists
                        {
                            _stageNumber++;
                            _levelNumber = s._firstLevel;//Place the first level
                        }

                        ////////////////Update the level number to
                    }
                    _currentWinsInARaw = 0;
                    _currentlosesInARaw = 0;

                }

                else if (_currentlosesInARaw == _winsInARow)
                {
                    if (_levelNumber > _firstLevel)
                    {
                        _levelNumber--;
                    }
                    else
                    {
                        Stage s = _bll.getStageDetails(_gameName, _stageNumber - 1);
                        if (s != null)//If the stage exists
                        {
                            _stageNumber--;
                            _levelNumber = s._lastLevel;//Place the first level
                            _currentWinsInARaw = 0;
                            _currentlosesInARaw = 0;
                        }

                        ////////////////Update the level number to
                    }
                    _currentWinsInARaw = 0;
                    _currentlosesInARaw = 0;
                }
            }
            catch (Exception)
            {
                problemHasOccurred("updateStageAndLevel failed");
            }
        }

        //use for cases the m_oWorker is cancelled(pause game)
        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Cancelled)
            {
                _m_oWorker_ready_to_use = true;
            }
            //MessageBox.Show("Worker cancelled");

            //if (e.Cancelled)
            //{
            //    //lblStatus.Text = "Task Cancelled.";
            //}

            // Check to see if an error occurred in the background process.

            //else if (e.Error != null)
            //{
            //    //lblStatus.Text = "Error while performing background operation.";
            //}
            //else
            //{

            //    // Everything completed normally.
            //    //lblStatus.Text = "Task Completed...";

            //}
            //m_oWorker_Dot_matrix.RunWorkerAsync();
        }
    }
}
