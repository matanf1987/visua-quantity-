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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Robot_game : Window
    {



        //Global params
        int _rectangleWidth = 0;//Each rectangel width(In the grid)
        int _rectangleHeight = 0;//Each rectangel Height(In the grid)
        int _rawSize;//Number of raws in the grid
        int _calSize;//Number of cals in the grid
        int _gameTime = 720;//Game time
        int _gameTimer = 0;//The remaining time of the game
        string _gameName = "Robot game";//The name of the game in the xml file
        string _gameMode = "experiment";//The mode of the game, can be experiment or tutorial
        int _stageNumber = 1;//Number of stage
        int _levelNumber = 2;//The number of board that need to be shown on the current stage
        string _robotImage = "";//The name of the robot image
        int _currentWinsInARaw = 0;//Number of wins in a raw
        int _currentlosesInARaw = 0;//Number of loses in a raw
        int _circleDisplayTime = 999;//The time the blue circle will preformed on the screen
        int _clearBoardDisplayTime = 500;//The time the board is clear between the blue circles
        Rectangle rectangle;//The rectangle object
        Ellipse ellipse;//The ellipse object
        SolidColorBrush myBrush;//The color object
        Boolean lockTheBoard = true;//Locking the screen while the learning stage is running
        List<int> _specialCellsList = new List<int>();//The list hold the special cells for each single game
        List<int> _userAnswersList = new List<int>();//The list hold the user answers for each single game
        List<Game> _gamesList;//List of games
        BackgroundWorker m_oWorker;//The main background worker
        Stage _currentStage = null;//Save the information of the current stage
        int _boardSize;//Keep the size of the current board
        int _winsInARow;//The number of wins in a raw, after x wins(The param is set in the configuration file) the user advance to the next stage
        int _firstLevel;//Keep the value of the first level of the stage
        int _lastLevel;//Keep the value of the last level of the stage
        int _criclePerformed = 0;
        System.Windows.Threading.DispatcherTimer Timer;
        BLL_games _bll;
        bool _puaseGame = false;
        bool _administratorMode = false;
        bool _problemHasOccurred = false;//True in case problem has occured, exit to the main window with an problem message
        bool _gameOver = false;//A flag, declare the game is over

        //Params for database information
        string _DBgameName = "robotGame";
        string _DBday = "";
        string _DBuserName = "";

        bool _m_oWorker_ready_to_use = true;//If the m_oWorker is ready to start again, use for the start new game after pause

        //The counstructor is not in use
        public Robot_game()
        {
            initGame();
            _bll = new BLL_games();
        }



        //Constructor for dministrator mode
        public Robot_game(string gameMode, int stageNumber, int levelNumber)
        {

            _administratorMode = true;
            _bll = new BLL_games();
            _stageNumber = stageNumber;
            _levelNumber = levelNumber;
            _gameMode = gameMode;//Can be experiment or tutorial
            initGame();
        }

        //Constructor for examineea mode
        public Robot_game(string username, string day)
        {
            _gameMode = "experiment";//Can be experiment or tutorial
            _administratorMode = false;
            _bll = new BLL_games();
            _DBgameName = "robotGame";
            _DBuserName = username;
            _DBday = day;


            string stageAndLevelFromDB = _bll.GetStageAndLevel(_DBuserName, _DBgameName);//Get the current satge and level of the user from the database
            //If the stage and level can't be found
            if (stageAndLevelFromDB == "")
            {
                problemHasOccurred("בשל בעיה בהבאת סטטוס השלב הינך מועבר חזרה לתפריט הראשי");
            }
            string[] stageAndLevelArray = stageAndLevelFromDB.Split('_');
            _stageNumber = Convert.ToInt32(stageAndLevelArray[0]);
            _levelNumber = Convert.ToInt32(stageAndLevelArray[1]);

            _gameTime = Convert.ToInt32(_bll.GetGameTime(_DBuserName, _DBgameName, _DBday)) * 60;//Init the game time
            initGame();
        }



        //Use in tutorial mode
        //Change the visibility of the unused items to hidden
        private void TutorialGameMode()
        {
            scoreGrid.Visibility = Visibility.Hidden;
            timeGrid.Visibility = Visibility.Hidden;
            gameNameLabel.Visibility = Visibility.Hidden;
            playPuaseImages.Visibility = Visibility.Hidden;
        }


        //Use to init the game, this function is called just in the constructors
        private void initGame()
        {
            InitializeComponent();

            if (_gameMode == "tutorial")
            {
                TutorialGameMode();
            }

            //try
            //{
                //Gets the value of the params from the config.txt file
                string param = _bll.GetParamFromCfgFile("RG_circleDisplayTime");
                if (param != "")
                    _circleDisplayTime = Convert.ToInt32(param);
                else
                    problemHasOccurred("הבאת המידע מהקובץ הקונפיג נכשלה פנה למנהל המערכת");

                param = _bll.GetParamFromCfgFile("RG_clearBoardDisplayTime");
                if (param != "")
                    _clearBoardDisplayTime = Convert.ToInt32(param);
                else
                    problemHasOccurred("הבאת המידע מהקובץ הקונפיג נכשלה פנה למנהל המערכת");

            //}
            //catch (Exception)
            //{
            //    problemHasOccurred("הבאת המידע מהקובץ הקונפיג נכשלה פנה למנהל המערכת");
            //}
            _gamesList = _bll._gamesList;//List of games
            //Timer init
            _gameTimer = _gameTime;
            try
            {
                Timer = new System.Windows.Threading.DispatcherTimer();
                Timer.Tick += new EventHandler(Timer_Tick);
                Timer.Interval = new TimeSpan(0, 0, 1);//Each second
                Timer.Start();
            }
            catch (Exception)
            {
                problemHasOccurred("אתחול השעון נכשל");
            }

            UpdtaeTimerLabel();



            try
            {
                //Set the background picture
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.UriSource = new Uri(@"images/Robot game/bg.jpg", UriKind.Relative);
                img.EndInit();
                backgroundImage.Source = img;
            }
            catch (Exception) { }

            try
            {
                _bll.ReadXml();//Read the xml file, use for the bll class
            }
            catch (Exception)
            {
                throw;
            }
            initBGWorker();
            initBoard();//Init the board


        }


        //Keyboard pressing listener
        public void keyDownEventHandler(object sender, KeyEventArgs e)
        {

            if (_administratorMode)
            {
                if (e.Key == Key.Escape)//If the user is administrator and He pressed on the escape button
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

            if (lockTheBoard)//If the board is lock return
                return;

            if (e.Key == Key.Q)//If the Q key was pressed
            {
                UserChoseYes();
            }
            else if (e.Key == Key.P)//If the P key was pressed
            {
                UserChoseNo();
            }


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
                MessageBox.Show("Game paused." + '\n' + "Press OK to continue.", "MESSAGE", MessageBoxButton.OK, MessageBoxImage.Warning);
                initBoard();
            }
            catch (Exception)
            {
                problemHasOccurred("imagePlayPuase_MouseUp failed");
            }
        }

        //Update the timer image
        public void updateTimeCircle()
        {
            if (_gameTimer < 0)//Prevent from the time circle to be under 0
                return;

            double x = (double)_gameTime / 78;
            double y = (double)_gameTimer / x;
            Rect myRect = new Rect(0, 78 - y, 88, 78);
            timeCircle.Rect = myRect;
        }


        //Handle the timer event, every second
        private void Timer_Tick(object sender, EventArgs e)
        {

            UpdtaeTimerLabel();
            if (_administratorMode == false)//The user is examinee
            {

                if (_problemHasOccurred == true)//If problem as occurred while the application tried to update the user status
                {

                    MessageBox.Show("חלה בעיה בעדכון הנתונים, פנה למנהל המערכת", "שגיאה");
                    Exam_menu admGM = new Exam_menu(_DBuserName, "problem has occurred");
                    this.Close();
                    admGM.Show();
                }

                if (_gameOver == true)//If the game is over
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

                //Update the statistic every minute
                if (_gameTimer % 60 == 0)
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
            else if (_administratorMode == true)//The user is administrator
            {

                if (_gameTimer <= 0)//IF the time of the game is 0 or less than it
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

            updateTimeCircle();//Update the time circle (The clock picture)
        }

        private void problemHasOccurred(string message)
        {
            MessageBox.Show(message, "שגיאה");
            throw new Exception("");
        }

        private void updateStatistics(int updateTime)
        {
            int examineeTotalGameTime = 0;
            //int minute = 60;
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

                //Update every 6 minutes the statistic in the database for the current examinee
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
                _userAnswersList.Clear();//Clear the list
                _boardSize = _currentStage._boardSize;
                _winsInARow = Convert.ToInt32(_currentStage._winsInARow); ;//The number of wins in a raw, after x wins(The param is set in the configuration file) the user advance to the next stage
                _firstLevel = Convert.ToInt32(_currentStage._firstLevel);
                _lastLevel = Convert.ToInt32(_currentStage._lastLevel);
                _robotImage = _currentStage._robotImage;
                _rawSize = _bll.GetRawSize(_boardSize);
                _calSize = _bll.GetCalSize(_boardSize);
                _rectangleWidth = (int)canvas.Width / _calSize;
                _rectangleHeight = (int)canvas.Height / _rawSize;

                //playPuaseImages.IsEnabled = true;
                Timer.Start();//Starts the timer event
                stageLabel.Content = "שלב: " + _stageNumber;
                statusLabel.Content = "זכור את מיקום" + '\n' + "הנקודות";
                statusLabel.Foreground = Brushes.DarkBlue;
                levelLabel.Content = "רמה: " + "" + _levelNumber;
                winsInARawLable.Content = "הצלחות ברצף: " + _currentWinsInARaw;
                yesImage.Visibility = Visibility.Hidden;
                noImage.Visibility = Visibility.Hidden;

                //Change the robot image
                System.Windows.Media.Imaging.BitmapImage newImage = new System.Windows.Media.Imaging.BitmapImage();
                newImage.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;
                newImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
                Uri urisource = new Uri(@"Images/Robot game/" + _robotImage, UriKind.Relative);
                newImage.BeginInit();
                newImage.UriSource = urisource;
                newImage.EndInit();
                robotImage.Source = newImage;

                //Start the backgroung worker 
                
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
        //Init the background worker properties
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
                m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(m_oWorker_RunWorkerCompleted);
            }
            catch (Exception)
            {
                problemHasOccurred("initBGWorker failed");
            }
            //Background worker - Handle the logic while the gui is running


        }

        //Draw the test stage grid
        public void DrawBoard_test()
        {
            try
            {

                _criclePerformed = _bll.GenerateTest(_boardSize, _specialCellsList);
                int cellCounter = 0;
                for (int i = 0; i < _rawSize; i++)
                {
                    for (int j = 0; j < _calSize; j++)
                    {
                        cellCounter++;
                        rectangle = new Rectangle();
                        rectangle.StrokeThickness = 2;
                        rectangle.Width = _rectangleWidth;
                        rectangle.Height = _rectangleHeight;
                        rectangle.Stroke = Brushes.Black;
                        if (cellCounter <= _boardSize)
                        {
                            myBrush = new SolidColorBrush(Colors.LightSkyBlue);
                        }
                        else
                        {
                            myBrush = new SolidColorBrush(Colors.Black);
                        }

                        rectangle.Fill = myBrush;
                        Canvas.SetTop(rectangle, _rectangleHeight * i);
                        Canvas.SetLeft(rectangle, _rectangleWidth * j);
                        canvas.Children.Add(rectangle);

                        if (_criclePerformed == cellCounter)
                        {
                            int offSet = 20;
                            ellipse = new Ellipse();
                            ellipse.Width = _rectangleWidth - offSet;
                            ellipse.Height = _rectangleHeight - offSet;
                            myBrush = new SolidColorBrush(Colors.Red);
                            ellipse.Fill = myBrush;
                            Canvas.SetTop(ellipse, _rectangleHeight * i + (offSet / 2));
                            Canvas.SetLeft(ellipse, _rectangleWidth * j + (offSet / 2));
                            canvas.Children.Add(ellipse);

                        }
                    }

                }
            }
            catch (Exception)
            {
                problemHasOccurred("ציור שלב המבחן נכשל");
            }

        }

        //Draw the learning stage grid
        private void DrawBoard_learning(Boolean drawSpecialCell)
        {
            try
            {

                int specialCellNumber = 0;
                int upperBound = 0;
                if (drawSpecialCell)
                {

                    do
                    {
                        specialCellNumber = _bll.GenerateNumber(_boardSize);
                        ///////////////Add a condition to check what happeneds if the lastLevel number is bigger than the size of the board
                    } while (_bll.SpecialCellWasGenerate(specialCellNumber, _specialCellsList));

                    _specialCellsList.Add(specialCellNumber);
                }


                int cellCounter = 0;


                for (int i = 0; i < _rawSize; i++)
                {
                    for (int j = 0; j < _calSize; j++)
                    {
                        cellCounter++;
                        rectangle = new Rectangle();
                        rectangle.StrokeThickness = 2;
                        rectangle.Width = _rectangleWidth;
                        rectangle.Height = _rectangleHeight;
                        rectangle.Stroke = Brushes.Black;
                        if (cellCounter <= _boardSize)
                        {
                            myBrush = new SolidColorBrush(Colors.LightSkyBlue);
                        }
                        else
                        {
                            myBrush = new SolidColorBrush(Colors.Black);
                        }
                        rectangle.Fill = myBrush;
                        Canvas.SetTop(rectangle, _rectangleHeight * i);
                        Canvas.SetLeft(rectangle, _rectangleWidth * j);
                        canvas.Children.Add(rectangle);

                        if (specialCellNumber == cellCounter && drawSpecialCell)
                        {
                            int offSet = 20;
                            ellipse = new Ellipse();
                            ellipse.Width = _rectangleWidth - offSet;
                            ellipse.Height = _rectangleHeight - offSet;
                            myBrush = new SolidColorBrush(Colors.DarkBlue);
                            ellipse.Fill = myBrush;
                            Canvas.SetTop(ellipse, _rectangleHeight * i + (offSet / 2));
                            Canvas.SetLeft(ellipse, _rectangleWidth * j + (offSet / 2));
                            canvas.Children.Add(ellipse);

                        }
                    }

                }
            }
            catch (Exception)
            {
                problemHasOccurred("ציור שלב הלמידה נכשל");
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
                if (m_oWorker.CancellationPending)
                {
                    m_oWorker.Dispose();
                    _puaseGame = true;
                    GC.Collect();
                    return;
                }
                if (e.ProgressPercentage == 0)//The stage before the learning stage starts, preforme the winning or losing label
                {
                    DrawBoard_learning(false);
                    if ((string)resultLabel.Content == "תוצאה")//Make sure that the label wont be visible in the first turn of the game(In that case there is no result)
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
                    DrawBoard_learning(true);
                    resultLabel.Visibility = Visibility.Hidden;
                }
                else if (e.ProgressPercentage == 2)
                {
                    DrawBoard_learning(false);
                }

                //The test stage
                else if (e.ProgressPercentage == 3)
                {
                    lockTheBoard = false;
                    //playPuaseImages.IsEnabled = false;
                    Timer.Stop();
                    DrawBoard_test();
                    statusLabel.Foreground = Brushes.DarkRed;
                    statusLabel.Content = "האם הנקודה" + '\n' + "הופיעה קודם" + '\n' + "באותו המקום" + "?";
                    yesImage.Visibility = Visibility.Visible;
                    noImage.Visibility = Visibility.Visible;
                    m_oWorker.CancelAsync();
                    m_oWorker.Dispose();
                    GC.Collect();
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

                
            try
            {
                if (_puaseGame == true)
                {
                    e.Cancel = true;
                    return;
                }

                string x = e.Argument as string;
                if (m_oWorker.CancellationPending)
                {
                    //m_oWorker_Dot_matrix.Dispose();
                    //m_oWorker_Dot_matrix = null;
                    //GC.Collect();
                    e.Cancel = true;
                    _puaseGame = true;
                    return;
                }
                lockTheBoard = true;
                m_oWorker.ReportProgress(0);

                Thread.Sleep(1000);

                for (int i = 0; i < (int)Int32.Parse(x); i++)
                {

                    if (m_oWorker.CancellationPending)
                    {
                        //m_oWorker_Dot_matrix.Dispose();
                        //m_oWorker_Dot_matrix = null;
                        //GC.Collect();
                        //e.Cancel = true;
                        e.Cancel = true;
                        _puaseGame = true;
                        return;
                    }
                    m_oWorker.ReportProgress(1);
                    Thread.Sleep(_circleDisplayTime);
                    m_oWorker.ReportProgress(2);
                    Thread.Sleep(_clearBoardDisplayTime);

                }
                if (m_oWorker.CancellationPending)
                {
                    //m_oWorker_Dot_matrix.Dispose();
                    //m_oWorker_Dot_matrix = null;
                    //GC.Collect();
                    e.Cancel = true;
                    _puaseGame = true;
                    return;
                }
                m_oWorker.ReportProgress(3);
            }
            catch (Exception)
            {
                problemHasOccurred("m_oWorker_DoWork failed");
            }

        }

        //If the user choose the yes answer
        private void UserChoseYes()
        {

            if (_gameMode == "tutorial")
            {
                initBoard();
                return;
            }

            try
            {
                if (_bll.CirclePerformedInLearningStage(true, _specialCellsList, _criclePerformed))//The circle was preformed in the learning stage
                {
                    _currentWinsInARaw++;
                    _currentlosesInARaw = 0;
                    resultLabel.Content = "כל הכבוד!!!";
                }
                else
                {
                    _currentWinsInARaw = 0;
                    _currentlosesInARaw++;
                    resultLabel.Content = "תשובה לא נכונה";
                }
            }
            catch (Exception)
            {
                ExitToMenu("בדיקת הופעת העיגול בשלב הלמידה נכשלה");
            }



            updateStageAndLevel();
            initBoard();
        }

        //If the user chose the no answer
        private void UserChoseNo()
        {
            if (_gameMode == "tutorial")
            {
                initBoard();
                return;
            }

            try
            {
                if (_bll.CirclePerformedInLearningStage(false, _specialCellsList, _criclePerformed))//The circle was not preformed in the learning stage
                {
                    _currentWinsInARaw++;
                    _currentlosesInARaw = 0;
                    resultLabel.Content = "כל הכבוד!!!";
                }
                else
                {
                    _currentWinsInARaw = 0;
                    _currentlosesInARaw++;
                    resultLabel.Content = "תשובה לא נכונה";
                }
            }
            catch (Exception)
            {
                ExitToMenu("בדיקת הופעת העיגול בשלב הלמידה נכשלה");
                
            }



            updateStageAndLevel();
            initBoard();
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

                else if (_currentlosesInARaw == _winsInARow)//If the user was wrong for _winsInARow (usualy 3 times in a row)
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
                problemHasOccurred("עדכון השלב והרמה נכשל");
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
