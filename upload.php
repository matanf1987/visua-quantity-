<?php

$con = mysql_connect("localhost", "root", "")or die("cannot connect");
mysql_set_charset('utf8',$con);
mysql_select_db("visua_quantity")or die("cannot select DB");
date_default_timezone_set("Asia/Tashkent");//Offset for make the time like isreal with 12 hours plus offset
// die(date("Y/m/d",time()-(86400/2)));//Set the time to be like israel time

$function = $_POST["function"];

if($function == 'CheckUserCredentials')
{
	$username = $_POST["username"];
	$password = $_POST["password"];
	$sql    = "SELECT * FROM users WHERE username='$username' and password='$password'";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	echo $num_rows = mysql_num_rows($result);
	mysql_close($con);
	return;
}
if($function == 'UpdateUserGantt')
{
	$participantNumber = $_POST["participantNumber"];
	$cellsInfo = $_POST["cellsInfo"];
	$cellsInfoArray = explode("#", $cellsInfo);
	for ($x=0; $x<22; $x++) 
	{
		$dayArray = explode("_", $cellsInfoArray[$x]);
		$date = $dayArray[2];
		$day = $dayArray[3];
		$hour = $dayArray[4];
		$robotGameTimeTotal = $dayArray[5];
		$skyGameTimeTotal = $dayArray[6];
		$treasureMapGameTimeTotal = $dayArray[7];
		$treasureMapReverseGameTimeTotal = $dayArray[8];
		$castleGameTimeTotal = $dayArray[9];
		$villageGameLocationTimeTotal = $dayArray[10];
		$villageGameIdentityTimeTotal = $dayArray[11];
		$robotGameTimeLeft = $dayArray[12];
		$skyGameTimeLeft = $dayArray[13];
		$treasureMapGameTimeLeft = $dayArray[14];
		$treasureMapReverseGameTimeLeft = $dayArray[15];
		$castleGameTimeLeft = $dayArray[16];
		$villageGameLocationTimeLeft = $dayArray[17];
		$villageGameIdentityTimeLeft = $dayArray[18];
		
		
		$sql  = "UPDATE dates SET date='$date', hour='$hour' , robotGameTimeTotal='$robotGameTimeTotal', skyGameTimeTotal='$skyGameTimeTotal' ,
		treasureMapGameTimeTotal='$treasureMapGameTimeTotal', treasureMapReverseGameTimeTotal='$treasureMapReverseGameTimeTotal', castleGameTimeTotal='$castleGameTimeTotal',
		villageGameLocationTimeTotal='$villageGameLocationTimeTotal', villageGameIdentityTimeTotal='$villageGameIdentityTimeTotal',
		robotGameTimeLeft='$robotGameTimeLeft', skyGameTimeLeft='$skyGameTimeLeft', treasureMapGameTimeLeft='$treasureMapGameTimeLeft',
		treasureMapReverseGameTimeLeft='$treasureMapReverseGameTimeLeft', castleGameTimeLeft='$castleGameTimeLeft',
		villageGameLocationTimeLeft='$villageGameLocationTimeLeft', villageGameIdentityTimeLeft='$villageGameIdentityTimeLeft'
		WHERE participantNumber='$participantNumber' AND day='$day'";
		
		$result = mysql_query($sql, $con);

		if (!$result) {
			mysql_close($con);
			die("false");
		}
	}
	die("true");
}
else if($function == "CheckUserPrivileges")
{

	$username = $_POST["username"];
	$sql    = "SELECT * FROM users";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	while ($row = mysql_fetch_assoc($result)) {
	
		if($row['username'] == $username)
		{
			echo $row['privilege'];
			mysql_close($con);
			return;
		}	
	}
		//////////////////////////check ittttttt
		echo "";
		mysql_close($con);
		return;
}
else if($function == "InsertGameStatistics")
{

	$username = $_POST["username"];
	$gameName = $_POST["gameName"];
	$date = $_POST["date"];
	$title = $_POST["title"];
	$details = $_POST["details"];
	
	$sql  = "INSERT INTO statistics (username, gameName, date, title,details) VALUES('$username','$gameName','$date','$title','$details')";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	die("true");

}
else if($function == "AddUser")
{

	$participantNumber = $_POST["participantNumber"];
	$username = $_POST["username"];
	$password = $_POST["password"];
	$team = $_POST["team"];
	$privilege = $_POST["privilege"];
	$gender = $_POST["gender"];
	$dominantHand = $_POST["dominantHand"];
	$age = $_POST["age"];
	$experimentNumber = $_POST["experimentNumber"];
	
	$sql  = "INSERT INTO users (participantNumber, username, password, team, privilege,gender,dominantHand,age,experimentNumber) VALUES('$participantNumber','$username','$password','$team','$privilege','$gender','$dominantHand','$age','$experimentNumber')";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	die("true");

//participantNumber, username, password, team, privilege,gender,dominantHand,age,experimentNumber
}
else if($function == "UpdateUserGameStatus")
{

	$gamename = $_POST["gamename"];
	$username = $_POST["username"];
	$status = $_POST["status"];
		
	$sql  = "UPDATE games_status SET $gamename='$status'  WHERE username='$username'";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	die("true");
}
else if($function == "UpdateGlobalGameTime")
{

	$gamename = $_POST["gamename"];
	$username = $_POST["username"];
	$gameGlobalTime = $_POST["gameGlobalTime"];
		
	$sql  = "UPDATE games_global_time SET $gamename='$gameGlobalTime'  WHERE username='$username'";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	die("true");
}
else if($function == "AddUserGamesStatus")
{

	// $participantNumber = $_POST["participantNumber"];
	$username = $_POST["username"];
	$robotGame = $_POST["robotGame"];
	$skyGame = $_POST["skyGame"];
	$treasureMapGame = $_POST["treasureMapGame"];
	$treasureMapReverseGame = $_POST["treasureMapReverseGame"];
	$castleGame = $_POST["castleGame"];
	$villageGameLocation = $_POST["villageGameLocation"];
	$villageGameIdentity = $_POST["villageGameIdentity"];
	
	// die($username.$robotGame.$skyGame.$treasureMap.$treasureMapReverseGame.$castleGame);
	
	$sql = "INSERT INTO games_status (username ,robotGame ,skyGame ,treasureMapGame ,treasureMapReverseGame ,castleGame,villageGameLocation,villageGameIdentity) VALUES 
	 ('$username','$robotGame','$skyGame','$treasureMapGame','$treasureMapReverseGame','$castleGame','$villageGameLocation','$villageGameIdentity')";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	die("true");

}
else if($function == "AddUserGamesGlobalTime")
{

	$username = $_POST["username"];
	$sql = "INSERT INTO games_global_time (username ,robotGame ,skyGame ,treasureMapGame ,treasureMapReverseGame ,castleGame,villageGameLocation,villageGameIdentity)VALUES ('$username','0','0','0','0','0','0','0')";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	die("true");

}
else if($function == "AddUserGantt")
{

	$participantNumber = $_POST["participantNumber"];
	$username = $_POST["username"];
	
	// userName
	// participantNumber
	// date
	// day
	// hour
		// robotGameTimeTotal
		// skyGameTimeTotal
	// treasureMapGameTimeTotal
	// treasureMapReverseGameTimeTotal
	// castleGameTimeTotal
	// robotGameTimeLeft
	// skyGameTimeLeft
	// treasureMapGameTimeLeft
	// treasureMapReverseGameTimeLeft
	// castleGameTimeLeft
	
	

	$insertDate = date('Y-m-d',time());
	$day = "fMRI pre";
	$sql  = "INSERT INTO dates (userName, participantNumber, date,day ,hour ,robotGameTimeTotal,skyGameTimeTotal,treasureMapGameTimeTotal,treasureMapReverseGameTimeTotal,castleGameTimeTotal,
	villageGameLocationTimeTotal,villageGameIdentityTimeTotal,robotGameTimeLeft,skyGameTimeLeft,treasureMapGameTimeLeft,treasureMapReverseGameTimeLeft,castleGameTimeLeft,villageGameLocationTimeLeft,villageGameIdentityTimeLeft)
	VALUES('$username','$participantNumber','$insertDate','$day','13:00','0','0','0','0','0','0','0','0','0','0','0','0','0','0')";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("false");
	}
		
	for ($i=1; $i<=20; $i++) {
		$day = "Day".$i;
		$addDays = $i*86400;
		$insertDate = date('Y-m-d',time()+$addDays);
		$sql  = "INSERT INTO dates (userName, participantNumber, date,day ,hour ,robotGameTimeTotal,skyGameTimeTotal,treasureMapGameTimeTotal,treasureMapReverseGameTimeTotal,castleGameTimeTotal,
		villageGameLocationTimeTotal,villageGameIdentityTimeTotal,robotGameTimeLeft,skyGameTimeLeft,treasureMapGameTimeLeft,treasureMapReverseGameTimeLeft,castleGameTimeLeft,villageGameLocationTimeLeft,villageGameIdentityTimeLeft) 
		VALUES('$username','$participantNumber','$insertDate','$day','13:00','9','9','9','9','9','9','9','9','9','9','9','9','9','9')";
			$result = mysql_query($sql, $con);
	
		if (!$result) {
			mysql_close($con);
			die("false");
		}
	} 
	
	$addDays = 21*86400;
	$insertDate = date('Y-m-d',time()+$addDays);
	$day = "fMRI post";
	$sql  = "INSERT INTO dates (userName, participantNumber, date,day ,hour ,robotGameTimeTotal,skyGameTimeTotal,treasureMapGameTimeTotal,treasureMapReverseGameTimeTotal,castleGameTimeTotal,villageGameLocationTimeTotal,villageGameIdentityTimeTotal,
	robotGameTimeLeft,skyGameTimeLeft,treasureMapGameTimeLeft,treasureMapReverseGameTimeLeft,castleGameTimeLeft,villageGameLocationTimeLeft,villageGameIdentityTimeLeft)
	VALUES('$username','$participantNumber','$insertDate','$day','13:00','0','0','0','0','0','0','0','0','0','0','0','0','0','0')";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("false");
	}
	

	
	die("true");

//participantNumber, username, password, team, privilege,gender,dominantHand,age,experimentNumber
}
else if($function == "GetExamineeCurrentDay")
{
	$username = $_POST["username"];
	// $addDays = 21*86400;
	$todaysDate = date('Y-m-d',time()-(86400/2));
	$sql    = "SELECT * FROM dates WHERE username='$username'";
	$result = mysql_query($sql, $con);
	$res = "";
	// die($todaysDate);
	
	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	while ($row = mysql_fetch_assoc($result)) {
	
		if($row['date'] == $todaysDate)
		{
			if($row['day'] != "fMRI pre" && $row['day'] != "fMRI post")
			{
				mysql_close($con);
				die($row['day']);
			
			}
		
		}
	}
		//////////////////////////check ittttttt
		
		mysql_close($con);
		die("");

}
else if($function == "GetExamineeDailyGamesInformation")
{
	$username = $_POST["username"];
	$day = $_POST["day"];
	
	$sql    = "SELECT * FROM dates WHERE username='$username' AND  day='$day'";
	$result = mysql_query($sql, $con);
	$res = "";
	
	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	while ($row = mysql_fetch_assoc($result)) {
	
		$res.=$row['day']."=";
		$res.=$row['hour']."=";
		$res.=$row['robotGameTimeTotal']."=";
		$res.=$row['skyGameTimeTotal']."=";
		$res.=$row['treasureMapGameTimeTotal']."=";
		$res.=$row['treasureMapReverseGameTimeTotal']."=";
		$res.=$row['castleGameTimeTotal']."=";
		$res.=$row['villageGameLocationTimeTotal']."=";
		$res.=$row['villageGameIdentityTimeTotal']."=";
		$res.=$row['robotGameTimeLeft']."=";
		$res.=$row['skyGameTimeLeft']."=";
		$res.=$row['treasureMapGameTimeLeft']."=";
		$res.=$row['treasureMapReverseGameTimeLeft']."=";
		$res.=$row['castleGameTimeLeft']."=";
		$res.=$row['villageGameLocationTimeLeft']."=";
		$res.=$row['villageGameIdentityTimeLeft'];
		echo $res;
		mysql_close($con);
		return;
	}
		//////////////////////////check ittttttt
		echo $res;
		mysql_close($con);
		return;

}
else if($function == "GetExamineeGantt")
{
	$participantNumber = $_POST["participantNumber"];
	// $username = $_POST["username"];
	$sql    = "SELECT * FROM dates WHERE participantNumber='$participantNumber' ORDER BY date ASC";
	$result = mysql_query($sql, $con);
	$res = "";
	
	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	while ($row = mysql_fetch_assoc($result)) {
	
		$res.=$row['userName']."=";
		$res.=$row['participantNumber']."=";
		$res.=$row['date']."=";
		$res.=$row['day']."=";
		$res.=$row['hour']."=";
		$res.=$row['robotGameTimeTotal']."=";
		$res.=$row['skyGameTimeTotal']."=";
		$res.=$row['treasureMapGameTimeTotal']."=";
		$res.=$row['treasureMapReverseGameTimeTotal']."=";
		$res.=$row['castleGameTimeTotal']."=";
		$res.=$row['villageGameLocationTimeTotal']."=";
		$res.=$row['villageGameIdentityTimeTotal']."=";
		$res.=$row['robotGameTimeLeft']."=";
		$res.=$row['skyGameTimeLeft']."=";
		$res.=$row['treasureMapGameTimeLeft']."=";
		$res.=$row['treasureMapReverseGameTimeLeft']."=";
		$res.=$row['castleGameTimeLeft']."=";
		$res.=$row['villageGameLocationTimeLeft']."=";
		$res.=$row['villageGameIdentityTimeLeft'];
		$res.="#";
	}
		//////////////////////////check ittttttt
		echo $res;
		mysql_close($con);
		return;

}
else if($function == "GetStatistics")
{
	$sql    = "SELECT * FROM statistics";
	$result = mysql_query($sql, $con);
	$res = "";
	
	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	while ($row = mysql_fetch_assoc($result)) {
	
		$res.=$row['username']."=";
		$res.=$row['gamename']."=";
		$res.=$row['date']."=";
		$res.=$row['title']."=";
		$res.=$row['details']."=";
		$res.="#";
	}
		//////////////////////////check ittttttt
		echo $res;
		mysql_close($con);
		return;

}

else if($function == "GetStageAndLevel")
{
	$username = $_POST["username"];
	$gamename = $_POST["gamename"];
	// $username = $_POST["username"];
	
	$sql    = "SELECT $gamename FROM games_status WHERE username='$username'";
	$result = mysql_query($sql, $con);
	$res = "";
	
	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	while ($row = mysql_fetch_assoc($result)) {
	
		$res=$row[$gamename];
	
	}
		//////////////////////////check ittttttt
		echo $res;
		mysql_close($con);
		return;
}
else if($function == "GetGameTime")
{
	$username = $_POST["username"];
	$gamename = $_POST["gamename"];
	$day = $_POST["day"];
	// $username = $_POST["username"];
	
	$sql    = "SELECT * FROM dates WHERE username='$username' AND day='$day'";
	$result = mysql_query($sql, $con);
	$res = "";
	
	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	while ($row = mysql_fetch_assoc($result)) {
	
		$res=$row[$gamename."TimeLeft"];
	
	}
		//////////////////////////check ittttttt
		echo $res;
		mysql_close($con);
		return;
}
else if($function == "GetGameGlobalTime")
{
	$username = $_POST["username"];
	$gamename = $_POST["gamename"];
		
	$sql    = "SELECT $gamename FROM games_global_time WHERE username='$username'";
	$result = mysql_query($sql, $con);
	$res = "";
	
	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	while ($row = mysql_fetch_assoc($result)) {
	
		$res=$row[$gamename];
	
	}
		//////////////////////////check ittttttt
		echo $res;
		mysql_close($con);
		return;
}
else if($function == "UpdateGameTime")
{

	$gameName = $_POST["gameName"];
	$username = $_POST["username"];
	$time = $_POST["time"];
	$day = $_POST["day"];
	$timeLeftCol = $gameName.'TimeLeft';
	
	$sql  = "UPDATE dates SET $timeLeftCol='$time'  WHERE username='$username' AND day='$day'";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	die("true");

//participantNumber, username, password, team, privilege,gender,dominantHand,age,experimentNumber
}
else if($function == "EditUser")
{

	$participantNumber = $_POST["participantNumber"];
	$username = $_POST["username"];
	$password = $_POST["password"];
	$team = $_POST["team"];
	$privilege = $_POST["privilege"];
	$gender = $_POST["gender"];
	$dominantHand = $_POST["dominantHand"];
	$age = $_POST["age"];
	$experimentNumber = $_POST["experimentNumber"];
	
	// $sql  = "INSERT INTO users (participantNumber, username, password, team, privilege,gender,dominantHand,age,experimentNumber) VALUES('$participantNumber','$username','$password','$team','$privilege','$gender','$dominantHand','$age','$experimentNumber')";
	$sql  = "UPDATE users SET password='$password', team='$team' , privilege='$privilege', gender='$gender' , dominantHand='$dominantHand', age='$age', experimentNumber='$experimentNumber' WHERE participantNumber='$participantNumber'";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	die("true");

//participantNumber, username, password, team, privilege,gender,dominantHand,age,experimentNumber
}
else if($function == "CheckIfUserOrParticipantNumberExist")
{

	$username = $_POST["username"];
	$participantNumber = $_POST["participantNumber"];
	$sql    = "SELECT * FROM users";
	$result = mysql_query($sql, $con);

	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	while ($row = mysql_fetch_assoc($result)) {
	
		if($row['username'] == $username || $row['participantNumber'] == $participantNumber)
		{
			echo "true";
			mysql_close($con);
			return;
		}
	}
	
	echo "false";
	mysql_close($con);
	return;
}
else if($function == "GetExamineeProfileList")
{
	
	$sql    = "SELECT * FROM users";
	$result = mysql_query($sql, $con);
	$res = "";
	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	while ($row = mysql_fetch_assoc($result)) {
	
		$res.=$row['username']."=";
		$res.=$row['participantNumber'];
		$res.="#";
	}
		//////////////////////////check ittttttt
		echo $res;
		mysql_close($con);
		return;

}
else if($function == "GetExamineesGant")
{
	
	$sql    = "SELECT * FROM dates";
	$result = mysql_query($sql, $con);
	$res = "";
	
	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	while ($row = mysql_fetch_assoc($result)) {
	
		$res.=$row['userName']."=";
		$res.=$row['participantNumber']."=";
		$res.=$row['date']."=";
		$res.=$row['day']."=";
		$res.=$row['hour']."=";
		$res.=$row['robotGameTimeTotal']."=";
		$res.=$row['skyGameTimeTotal']."=";
		$res.=$row['treasureMapGameTimeTotal']."=";
		$res.=$row['treasureMapReverseGameTimeTotal']."=";
		$res.=$row['castleGameTimeTotal']."=";
		$res.=$row['villageGameLocationTimeTotal']."=";
		$res.=$row['villageGameIdentityTimeTotal']."=";
		$res.=$row['robotGameTimeLeft']."=";
		$res.=$row['skyGameTimeLeft']."=";
		$res.=$row['treasureMapGameTimeLeft']."=";
		$res.=$row['treasureMapReverseGameTimeLeft']."=";
		$res.=$row['castleGameTimeLeft']."=";
		$res.=$row['villageGameLocationTimeLeft']."=";
		$res.=$row['villageGameIdentityTimeLeft'];
		$res.="#";
	}
		//////////////////////////check ittttttt
		echo $res;
		mysql_close($con);
		return;

}
else if($function == "GetUserByParticipantNumber")
{
	// 
	$participantNumber = $_POST["participantNumber"];
	$sql    = "SELECT * FROM users Where participantNumber = '$participantNumber'";
	$result = mysql_query($sql, $con);
	$res = "";
	if (!$result) {
		mysql_close($con);
		die("");
	}
	
	while ($row = mysql_fetch_assoc($result)) {
	
		// echo $row;
		$res.=$row['participantNumber']."=";
		$res.=$row['privilege']."=";
		$res.=$row['age']."=";
		$res.=$row['username']."=";
		$res.=$row['password']."=";
		$res.=$row['gender']."=";
		$res.=$row['dominantHand']."=";
		$res.=$row['team']."=";
		$res.=$row['experimentNumber'];
		echo $res;
		mysql_close($con);
		return;

	}
	echo "";
	mysql_close($con);
	return;
		//////////////////////////check ittttttt
	
}
else if($function == "DeleteUser")
{
	$username = $_POST["username"];
	
	//Delete from users table
	$sql  = "DELETE FROM users WHERE username='$username'";
	$result = mysql_query($sql, $con);
	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	$sql  = "DELETE FROM dates WHERE username='$username'";
	$result = mysql_query($sql, $con);
	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	$sql  = "DELETE FROM games_global_time WHERE username='$username'";
	$result = mysql_query($sql, $con);
	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	$sql  = "DELETE FROM games_status WHERE username='$username'";
	$result = mysql_query($sql, $con);
	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	$sql  = "DELETE FROM statistics WHERE username='$username'";
	$result = mysql_query($sql, $con);
	if (!$result) {
		mysql_close($con);
		die("false");
	}
	
	
	
	die("true");
	
}


?>
