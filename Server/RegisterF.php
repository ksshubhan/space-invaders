<?php
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_NAME', 'spaceinvaders');

// Establish a database connection
$dbConnection = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

// Get user ID from the Unity request
$userID = isset($_POST['userID']) ? $_POST['userID'] : null;

// Checking if the user owns all titles
$sqlCheckAllTitlesOwned = "SELECT COUNT(*) AS totalTitles FROM titles";
$stmtCheckAllTitlesOwned = $dbConnection->prepare($sqlCheckAllTitlesOwned);

if ($stmtCheckAllTitlesOwned === false) 
{
    echo "Error preparing statement: " . $dbConnection->error;
} 
else 
{
    $stmtCheckAllTitlesOwned->execute();
    $resultCheckAllTitlesOwned = $stmtCheckAllTitlesOwned->get_result();
    $rowTotalTitles = $resultCheckAllTitlesOwned->fetch_assoc();
    $totalTitles = $rowTotalTitles['totalTitles'];

    $stmtCheckAllTitlesOwned->close();

    $sqlCheckUserTitles = "SELECT COUNT(*) AS ownedTitles FROM userstitles WHERE userID = ?";
    $stmtCheckUserTitles = $dbConnection->prepare($sqlCheckUserTitles);

    if ($stmtCheckUserTitles === false) 
    {
        echo "Error preparing statement: " . $dbConnection->error;
    } 
    else 
    {
        $stmtCheckUserTitles->bind_param("i", $userID);
        $stmtCheckUserTitles->execute();
        $resultCheckUserTitles = $stmtCheckUserTitles->get_result();
        $rowOwnedTitles = $resultCheckUserTitles->fetch_assoc();
        $ownedTitles = $rowOwnedTitles['ownedTitles'];

        $stmtCheckUserTitles->close();

        if ($ownedTitles == $totalTitles)
        {
            echo "AllTitlesOwned";
        } 
        else 
        {
            echo "NotAllTitlesOwned";
        }
    }
}

closeConnection($dbConnection);

// Function to establish database connection
function establishConnection($server, $username, $password, $dbName) 
{
    $connection = new mysqli($server, $username, $password, $dbName);

    if ($connection->connect_error) 
    {
        die("Connection failed: " . $connection->connect_error);
    }

    return $connection;
}

// Function to close database connection
function closeConnection($connection) 
{
    $connection->close();
}
?>
