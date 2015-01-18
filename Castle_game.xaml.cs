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
using System.IO;


namespace TryWPF
{
    /// <summary>
    /// Interaction logic for Castle_game.xaml
    /// </summary>
    public partial class Castle_game : Window
    {
    
           //Global params
        int _rectangleWidth = 0;//Each rectangel width(In the grid)
        int _rectangleHeight = 0;//Each rectangel Height(In the grid)
        int _rawSize;//Number of raws in the grid
        int _calSize;//Number of cals in the grid
        int _numberOfGUessing = 0;//The number of times the user can/need to answer
        int _gameTime = 720;//Game time
        int _gameTimer = 0;//The remaining time of the game
        //bool _timerIsRunning = true;//If the timer is running
        string _gameName = "Castle game";//The name of the game for the xml file
        string _gameMode = "experiment";//The mode of the game, can be experiment or tutorial
        int _stageNumber = 7;//Number of stage
        int _levelNumber = 3;//The number of board that need to be shown on the current stage
        string _gameImage = "";//The name of the game image
        int _currentWinsInARaw = 0;//Number of wins in a raw
        int _currentlosesInARaw = 0;//Number of loses in a raw
        int _picturesDisplyTime = 1000;//The time the pictures display every turn, after it white rectangle will be disply
        string _picturesDisplyTimeOption = "false";//The option to display the pictures for a spasific time in each turn
        Rectangle rectangle;//The rectangle object
        Ellipse ellipse;//The ellipse object
        SolidColorBrush myBrush;//The color object
        Boolean lockTheBoard = true;//Locking the screen while the learning stage is running
        List<int> _specialCellsList = new List<int>();//The list hold the special cells that was chosen in each single game
        List<int> _specialImagesList = new List<int>();//The list hold the special images that was chosen in each single game
        List<int> _userLearningStageAnswersList = new List<int>();//The list hold the user answers for each single game
        List<int> _userTestStageAnswersList = new List<int>();//The list hold the user answers for each single game
        List<int> _rightAnswersList = new List<int>();//The list hold the user answers for each single game

        List<Game> _gamesList;
        BackgroundWorker m_oWorker;
   
        Stage _currentStage = null;
        int _boardSize;
        int _fribbleImagesNumber = 12;
        int _winsInARow;//The number of wins in a raw, after x wins(The param is set in the configuration file) the user advance to the next stage
        int _firstLevel;
        int _lastLevel;
        int _cellPerformed = 0;
        int _imagePerformed = 0;

        System.Windows.Threading.DispatcherTimer Timer;
        BLL_games _bll = new BLL_games();
        bool _puaseGame = false;

        //Drawing functions params
        int _specialCellNumber = 0;
        int _specialPicturesDirectoryNumber = 0;
        int _regularPicturesDirectoryNumber = 0;
        string[] _specialPicturesFilesArray = null;
        string[] _regularPicturesFilesArray = null;
        List<int> _regularPicturesNumberList = new List<int>();
        int _specialpictureFileNumber = 0;
        bool _administratorMode = false;

        //Params for database information
        string _DBgameName = "castleGame";
        string _DBday = "";
        string _DBuserName = "";
        bool _problemHasOccurred = false;//True in case problem has occured, exit to the main window with an problem message
        bool _gameOver = false;//A flag, declare the game is over
        bool _m_oWorker_ready_to_use = true;//If the m_oWorker is ready to start again, use for the start new game after pause

        public Castle_game()
        {
           initGame();
           //_bll = new BLL_games();
        }

        //Constructor for administrator mode
        public Castle_game(string gameMode, int stageNumber, int levelNumber)
        {

            _administratorMode = true;

            _stageNumber = stageNumber;
            _levelNumber = levelNumber;
            _gameMode = gameMode;//Can be experiment or tutorial
            initGame();
        }

        //Constructor for examinee mode
        public Castle_game(string username, string day)
        {

            _administratorMode = false;
            _DBgameName = "castleGame";
            _DBuserName = username;
            _DBday = day;

            string stageAndLevelFromDB = _bll.GetStageAndLevel(_DBuserName, _DBgameName);
            //If the stage and level can't be found
            if (stageAndLevelFromDB == "")
            {
                problemHasOccurred("בשל בעיה בהבאת סטטוס השלב הינך מועבר חזרה לתפריט הראשי");
            }
            string[] stageAndLevelArray = stageAndLevelFromDB.Split('_');
            _stageNumber = Convert.ToInt32(stageAndLevelArray[0]);
            _levelNumber = Convert.ToInt32(stageAndLevelArray[1]);
            _gameTime = Convert.ToInt32(_bll.GetGameTime(_DBuserName, _DBgameName, _DBday))*60;
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

            if (_gameMode == "tutorial")
            {
                TutorialGameMode();
            }

            //try
            //{
                string param = _bll.GetParamFromCfgFile("CG_picturesDisplyTime");
                if (param != "")
                    _picturesDisplyTime = Convert.ToInt32(param);
                else
                    problemHasOccurred("הבאת המידע מהקובץ הקונפיג נכשלה פנה למנהל המערכת");

                param = _bll.GetParamFromCfgFile("CG_picturesDisplyTimeOption");
                if (param != "")
                    _picturesDisplyTimeOption = param;
                else
                    problemHasOccurred("הבאת המידע מהקובץ הקונפיג נכשלה פנה למנהל המערכת");

            //}
            //catch (Exception)
            //{
            //    problemHasOccurred("הבאת המידע מהקובץ הקונפיג נכשלה פנה למנהל המערכת");
            //}


            //The bll object initialize
            _gamesList = _bll._gamesList;

            //Timer
            _gameTimer = _gameTime;

            try
            {
                Timer = new System.Windows.Threading.DispatcherTimer();
                Timer.Tick += new EventHandler(Timer_Tick);
                Timer.Interval = new TimeSpan(0, 0, 1);//Each second
                //Timer.Start();
                UpdtaeTimerLabel();
                canvas.Background = new SolidColorBrush(Colors.Transparent);
            }
            catch (Exception)
            {
                problemHasOccurred("אתחול השעון נכשל");
            }
            //



            try
            {
                ///Relative path to bg picture
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.UriSource = new Uri(@"images/Castle game/bg.jpg", UriKind.Relative);
                img.EndInit();
                backgroundImage.Source = img;
            }
            catch (Exception){}

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

        private void problemHasOccurred(string message)
        {
            MessageBox.Show(message, "שגיאה");
            throw new Exception("");
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

            if (lockTheBoard)//In learning stage return
                return;

            //if (e.Key == Key.Q)
            //    UserChoseYes();
            //else if (e.Key == Key.P)
            //    UserChoseNo();

        }

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
                _numberOfGUessing = 0;
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
            int minute = 60;
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
                //_gameTimer--;
                _gameTimer -= 1;
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
                //_numberOfGUessing = 0;
                _specialCellsList.Clear();//Clear the list
                _specialImagesList.Clear();

                _boardSize = _currentStage._boardSize;
                _winsInARow = Convert.ToInt32(_currentStage._winsInARow); ;//The number of wins in a raw, after x wins(The param is set in the configuration file) the user advance to the next stage
                _firstLevel = Convert.ToInt32(_currentStage._firstLevel);
                _lastLevel = Convert.ToInt32(_currentStage._lastLevel);
                _gameImage = _currentStage._robotImage;
                _rawSize = _bll.GetRawSize(_boardSize);
                _calSize = _bll.GetCalSize(_boardSize);
                _rectangleWidth = (int)canvas.Width / _calSize;
                _rectangleHeight = (int)canvas.Height / _rawSize;

                //playPuaseImages.IsEnabled = true;
                Timer.Start();//Starts the timer event
                
                //statusLabel.Content = "זכור את מיקום הדמויות וזהותם";
                //statusLabel.Foreground = Brushes.DarkBlue;
                stageLabel.Content = "שלב: " + _stageNumber;
                levelLabel.Content = "רמה: " + "" + _levelNumber;
                winsInARawLable.Content = "הצלחות ברצף: " + _currentWinsInARaw;
                yesImage.Visibility = Visibility.Hidden;
                noImage.Visibility = Visibility.Hidden;

                //Change the robot image
                System.Windows.Media.Imaging.BitmapImage newImage = new System.Windows.Media.Imaging.BitmapImage();
                newImage.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;
                newImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
                Uri urisource = new Uri(@"Images/Castle game/" + _gameImage, UriKind.Relative);
                newImage.BeginInit();
                newImage.UriSource = urisource;
                newImage.EndInit();
                gameImage.Source = newImage;


                _specialCellNumber = 0;
                _specialPicturesDirectoryNumber = 0;
                _regularPicturesDirectoryNumber = 0;
                _specialPicturesFilesArray = null;
                _regularPicturesFilesArray = null;
                _regularPicturesNumberList.Clear();
                _specialpictureFileNumber = 0;

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

        

        private void initBGWorker()
        {

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
                problemHasOccurred("אתחול העובד מאחורי הקלעים נכשל");
            }

        }

        //Draw the test stage grid
        //public void DrawBoard_test()
        //{

        //    try
        //    {
        //        int cellCounter = 0;
        //        int regularPictureIndexCounter = 0;

        //        for (int i = 0; i < _rawSize; i++)
        //        {
        //            for (int j = 0; j < _calSize; j++)
        //            {
        //                cellCounter++;

        //                if (cellCounter > _boardSize)
        //                    break;

        //                rectangle = new Rectangle();
        //                myBrush = new SolidColorBrush(Colors.White);
        //                rectangle.StrokeThickness = 2;
        //                rectangle.Width = _rectangleWidth;
        //                rectangle.Height = _rectangleHeight;
        //                rectangle.Stroke = Brushes.Black;
        //                rectangle.Fill = myBrush;
        //                Canvas.SetTop(rectangle, _rectangleHeight * i);
        //                Canvas.SetLeft(rectangle, _rectangleWidth * j);
        //                canvas.Children.Add(rectangle);

        //                BitmapImage img = new BitmapImage();
        //                img.BeginInit();
        //                img.CacheOption = BitmapCacheOption.OnLoad;
        //                int offSet = 10;

        //                if (_specialCellNumber == cellCounter)
        //                {

        //                    img.UriSource = new Uri(_specialPicturesFilesArray[_specialpictureFileNumber], UriKind.Relative);
        //                }
        //                else if (_specialCellNumber != cellCounter)//Not special picture
        //                {
        //                    img.UriSource = new Uri(_regularPicturesFilesArray[_regularPicturesNumberList[regularPictureIndexCounter]], UriKind.Relative);
        //                    regularPictureIndexCounter++;
        //                }
        //                img.EndInit();
        //                Image image = new Image
        //                {
        //                    Width = _rectangleWidth - offSet,
        //                    Height = _rectangleHeight - offSet,
        //                    Source = img
        //                };
        //                Canvas.SetTop(image, _rectangleHeight * i + (offSet / 2));
        //                Canvas.SetLeft(image, _rectangleWidth * j + (offSet / 2));
        //                canvas.Children.Add(image);

        //            }

        //        }
        //    }
        //    catch (Exception)
        //    {
        //         problemHasOccurred("ציור שלב המבחן נכשל");
        //    }
        //}

        //Draw the learning stage grid
        private void DrawBoard_learning(Boolean drawSpecialCell, Boolean displayRedBorder)
        {

            try
            {

                string[] picturesDirectoriesArray = null;
                string specialPicturesDirectoryPath = "";
                string regularPicturesDirectoryPath = "";

                int regularPicturesFileNumber = 0;


                //string[] scenesDirectoriesArray = Directory.GetDirectories(@"images\Castle game\scenes");
                string[] filesArray1;

                string[] filters = new[] { "*.jpg", "*.png", "*.gif" };

                if (drawSpecialCell)
                {
                    _specialCellNumber = _bll.GenerateNumber(_boardSize);
                    _rightAnswersList.Add(_specialCellNumber);
                    Random rnd = new Random();
                    int number = rnd.Next(1, 3); // creates a number between 1 and the size of the board

                    if (number == 1)//The picuters form the objects directory
                    {
                        picturesDirectoriesArray = Directory.GetDirectories(@"images\Castle game\objects");
                    }
                    else//The picuters form the scenes directory
                    {
                        picturesDirectoriesArray = Directory.GetDirectories(@"images\Castle game\scenes");
                    }


                    _specialPicturesDirectoryNumber = rnd.Next(1, picturesDirectoriesArray.Length); // creates a number between 1 and the size of the board
                    do
                    {
                        _regularPicturesDirectoryNumber = rnd.Next(1, picturesDirectoriesArray.Length); // creates a number between 1 and the size of the board

                        ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                    } while (_regularPicturesDirectoryNumber == _specialPicturesDirectoryNumber);


                    specialPicturesDirectoryPath = picturesDirectoriesArray[_specialPicturesDirectoryNumber];
                    regularPicturesDirectoryPath = picturesDirectoriesArray[_regularPicturesDirectoryNumber];


                    _specialPicturesFilesArray = filters.SelectMany(f => Directory.GetFiles(specialPicturesDirectoryPath, f)).ToArray();
                    _regularPicturesFilesArray = filters.SelectMany(f => Directory.GetFiles(regularPicturesDirectoryPath, f)).ToArray();
                    //_specialPicturesFilesArray = Directory.GetFiles(specialPicturesDirectoryPath);
                    //_regularPicturesFilesArray = Directory.GetFiles(regularPicturesDirectoryPath);

                    _specialpictureFileNumber = rnd.Next(1, _specialPicturesFilesArray.Length);
                    //regularPicturesFileNumber = rnd.Next(1, _regularPicturesFilesArray.Length);

                    for (int i = 0; i < _boardSize - 1; i++)
                    {
                        do
                        {
                            regularPicturesFileNumber = rnd.Next(1, _regularPicturesFilesArray.Length);

                            ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                        } while (_bll.SpecialImageWasGenerate(regularPicturesFileNumber, _regularPicturesNumberList));
                        _regularPicturesNumberList.Add(regularPicturesFileNumber);
                    }
                }



                int cellCounter = 0;

                int regularPictureIndexCounter = 0;
                for (int i = 0; i < _rawSize; i++)
                {
                    for (int j = 0; j < _calSize; j++)
                    {
                        cellCounter++;

                        if (cellCounter > _boardSize)
                            break;

                        rectangle = new Rectangle();
                        myBrush = new SolidColorBrush(Colors.White);
                        rectangle.StrokeThickness = 2;
                        rectangle.Width = _rectangleWidth;
                        rectangle.Height = _rectangleHeight;

                        if (displayRedBorder == true)
                            rectangle.Stroke = Brushes.Red;
                        else
                            rectangle.Stroke = Brushes.Black;

                        rectangle.Fill = myBrush;
                        Canvas.SetTop(rectangle, _rectangleHeight * i);
                        Canvas.SetLeft(rectangle, _rectangleWidth * j);
                        canvas.Children.Add(rectangle);

                        if (drawSpecialCell)
                        {
                            BitmapImage img = new BitmapImage();
                            img.BeginInit();
                            img.CacheOption = BitmapCacheOption.OnLoad;
                            int offSet = 10;

                            if (_specialCellNumber == cellCounter)
                            {

                                img.UriSource = new Uri(_specialPicturesFilesArray[_specialpictureFileNumber], UriKind.Relative);
                            }
                            else if (_specialCellNumber != cellCounter)//Not special picture
                            {
                                img.UriSource = new Uri(_regularPicturesFilesArray[_regularPicturesNumberList[regularPictureIndexCounter]], UriKind.Relative);
                                regularPictureIndexCounter++;
                            }
                            img.EndInit();
                            Image image = new Image
                            {
                                Width = _rectangleWidth - offSet,
                                Height = _rectangleHeight - offSet,
                                Source = img
                            };
                            Canvas.SetTop(image, _rectangleHeight * i + (offSet / 2));
                            Canvas.SetLeft(image, _rectangleWidth * j + (offSet / 2));
                            canvas.Children.Add(image);
                        }
                    }
                }
            }
            catch (Exception)
            {
                problemHasOccurred("ציור שלב הלמידה נכשל");
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
                problemHasOccurred("סימון ריבוע נכשל");
            }
        }
    
        private void endGame()
        {
            try
            {
                if (_gameMode == "experiment")
                {
                    if (_userLearningStageAnswersList.SequenceEqual(_userTestStageAnswersList))//Correct answer
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
                    updateStageAndLevel();

                }


                _numberOfGUessing = 0;
                _userLearningStageAnswersList.Clear();//Clear the list
                _rightAnswersList.Clear();//Clear the list
                _userTestStageAnswersList.Clear();
                initBoard();
            }
            catch (Exception)
            {
                problemHasOccurred("סיום משחק נכשל");
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


                if (rectangleNumber > 0 && rectangleNumber <= _boardSize)
                {


                    _numberOfGUessing++;
                    //initBGWorker();
                    if (_numberOfGUessing > _levelNumber)//If the user ended to choose the diffrent photo and now repet the order of the rectangles He choosen
                    {

                        lockTheBoard = true;
                        _userTestStageAnswersList.Add(rectangleNumber);
                        MarkRectangle(rectangleNumber);

                        if (_userTestStageAnswersList.Count == _levelNumber)//If the user finished to answer in the test stage
                        {
                            m_oWorker.RunWorkerAsync("final test - end playing");//If the user stil have to choose the rectangles he was chosen in the learning part(the diffrent photos)

                        }
                        else
                        {
                            m_oWorker.RunWorkerAsync("final test - still playing");//If the user finish to choose the rectangles he was chosen in the learning part(the diffrent photos)
                        }
                        return;
                    }

                    _userLearningStageAnswersList.Add(rectangleNumber);
                    if (_numberOfGUessing == _levelNumber)//If the user end the learning part
                    {
                        Timer.Stop();
                        statusLabel.Foreground = Brushes.DarkRed;
                        statusLabel.Content = "לחץ על מיקום התמונות" + '\n' + "השונות קודם";
                        DrawBoard_learning(false, true);
                    }
                    else
                    {
                        resultLabel.Content = "";
                        _puaseGame = false;
                        initBoard();
                    }


                    //m_oWorker_Dot_matrix.RunWorkerAsync("end game");

                }
            }
            catch (Exception)
            {
                problemHasOccurred("CanvasMouseDown failed");
            }

        }
        
        //
       
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

                    DrawBoard_learning(false, false);
                    if ((string)resultLabel.Content == "תוצאה" || (string)resultLabel.Content == "")//Make sure that the label wont be visible in the first turn of the game(In that case there is no result)
                    {
                        resultLabel.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        resultLabel.Visibility = Visibility.Visible;
                    }
                }
                //The learning stage 
                else if (e.ProgressPercentage == 1)
                {
                    //DrawBoard_learning_Dot_matrix(true);
                    //resultLabel.Visibility = Visibility.Hidden;
                }
                //The test stage
                else if (e.ProgressPercentage == 2)
                {
                    lockTheBoard = true;
                    resultLabel.Visibility = Visibility.Hidden;
                    //playPuaseImages.IsEnabled = false;

                    //DrawBoard_test_Dot_matrix();
                    DrawBoard_learning(true, false);
                    statusLabel.Foreground = Brushes.Blue;
                    statusLabel.Content = "לחץ על התמונה השונה";
                    //yesImage.Visibility = Visibility.Visible;
                    //noImage.Visibility = Visibility.Visible;

                    if (_picturesDisplyTimeOption == "false")
                    {
                        lockTheBoard = false;
                        m_oWorker.CancelAsync();
                        m_oWorker.Dispose();
                        GC.Collect();
                    }

                }
                else if (e.ProgressPercentage == 3)
                {
                    lockTheBoard = false;
                    resultLabel.Visibility = Visibility.Hidden;
                    //playPuaseImages.IsEnabled = false;

                    //DrawBoard_test_Dot_matrix();
                    DrawBoard_learning(false, false);
                    statusLabel.Foreground = Brushes.Blue;
                    statusLabel.Content = "לחץ על התמונה השונה";
                    //yesImage.Visibility = Visibility.Visible;
                    //noImage.Visibility = Visibility.Visible;
                    m_oWorker.CancelAsync();
                    m_oWorker.Dispose();
                    GC.Collect();
                }
                else if (e.ProgressPercentage == 4)
                {

                    DrawBoard_learning(false, true);
                    lockTheBoard = false;
                    m_oWorker.CancelAsync();
                    m_oWorker.Dispose();
                    GC.Collect();
                    //playPuaseImages.IsEnabled = false;
                    //Timer.Stop();
                    //DrawBoard_test_Dot_matrix();
                    //statusLabel.Foreground = Brushes.DarkRed;
                    //statusLabel.Content = "האם הדמות הופיעה באותו מקום" + '\n' + "ואותה זהות קודם?";
                    //yesImage.Visibility = Visibility.Visible;
                    //noImage.Visibility = Visibility.Visible;
                    //m_oWorker_Dot_matrix.CancelAsync();
                    //m_oWorker_Dot_matrix.Dispose();
                    //GC.Collect();
                }
                else if (e.ProgressPercentage == 5)
                {
                    lockTheBoard = false;
                    m_oWorker.CancelAsync();
                    m_oWorker.Dispose();
                    GC.Collect();
                    endGame();
                    ////playPuaseImages.IsEnabled = false;
                    //Timer.Stop();
                    //DrawBoard_test_Dot_matrix();
                    //statusLabel.Foreground = Brushes.DarkRed;
                    //statusLabel.Content = "האם הדמות הופיעה באותו מקום" + '\n' + "ואותה זהות קודם?";
                    ////yesImage.Visibility = Visibility.Visible;
                    ////noImage.Visibility = Visibility.Visible;
                    //m_oWorker_Dot_matrix.CancelAsync();
                    //m_oWorker_Dot_matrix.Dispose();
                    //GC.Collect();
                }
            }
            catch (Exception)
            {
                problemHasOccurred("m_oWorker_ProgressChanged failed");
            }
        }

        //m_oWorker_Statistics
        
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

                if (x == "final test - still playing")
                {
                    Thread.Sleep(300);
                    m_oWorker.ReportProgress(4);
                    return;
                }
                else if (x == "final test - end playing")
                {
                    Thread.Sleep(300);
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

                Thread.Sleep(500);
                //m_oWorker_Dot_matrix.ReportProgress(1);

                //Thread.Sleep(100);
                m_oWorker.ReportProgress(2);

                if (_picturesDisplyTimeOption == "true")
                {
                    Thread.Sleep(_picturesDisplyTime);
                    m_oWorker.ReportProgress(3);
                }

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