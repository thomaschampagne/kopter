<?php

header('Content-type: application/json');

require_once 'Datasource.php';
require_once 'Track.php';
require_once 'TrackDao.php';
require_once 'Config.php';

$conn = new Datasource($dbHost, $dbName, $dbuser, $dbpasswd);
$trackDao = new TrackDao();

$r = "null";

// Init
switch ($_POST['method']) {

	case "newSession":

		$now =  date('Y-m-d H:i:s', time());
		$ipIn = $_SERVER['REMOTE_ADDR']; //
		$startDateIn = $now; //
		$endDateIn = null;
		$longitudeIn = 0;
		$latitudeIn = 0;
		$versionIn = $_POST['version']; //
		$unity3dversionIn =  $_POST['unityVersion'];
		$hostnameIn = $_POST['hostname'];
		$osIn = $_POST['os'];
		$platformIn = $_POST['platform']; //
		$resolutionIn = $_POST['resolution'];
		$graphicCardIn = $_POST['gc'];
		$cpuIn = $_POST['cpu'];
		$deviceModelIn = $_POST['devicemodel'];

		$sessionIdIn = $ipIn.''.$today.''.microtime();//.''.rand(0, time()).''.rand(0, time());
		$sessionIdIn = md5($sessionIdIn); //

		
		$t = new Track();
		$t->setAll($sessionIdIn, 
				$ipIn, 
				$startDateIn, 
				$endDateIn, 
				$longitudeIn, 
				$latitudeIn, 
				$versionIn, 
				$unity3dversionIn, 
				$hostnameIn,
				$osIn, 
				$platformIn, 
				$resolutionIn, 
				$graphicCardIn, 
				$cpuIn, 
				$deviceModelIn);
		
		/*
		$t->setAll($sessionIdIn,
				$ipIn,
				$startDateIn,
				$endDateIn,
				$geolocationIn,
				$versionIn,
				$unityVersionIn,
				$platformIn,
				$qualityIn);*/

		$trackDao->create($conn, $t);
		
		$r = "\"".$sessionIdIn."\"";

		break;
		
		
	case "endSession":
		
		$t = $trackDao->getObject($conn, $_POST['sessionId']);
		$now =  date('Y-m-d H:i:s', time());
		$t->setEndDate($now);
		$s = $trackDao->save($conn, $t);
		$r = ($s) ? true : false;
		break;

}

print "{\"r\":".$r."}";










?>