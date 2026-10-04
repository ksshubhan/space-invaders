<?php
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_NAME', 'spaceinvaders');

$dbConnection = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

// Fetch the number of players using COUNT
$sql = "SELECT COUNT(*) as playerCount FROM users";
$result = executeQuery($dbConnection, $sql);

if ($result->num_rows > 0) 
{
    // Fetch the result as an associative array
    $playerCount = fetchPlayerCount($result);

    // Return the result as JSON
    echo json_encode(array("playerCount" => $playerCount));
} 
else 
{
    // Handle the case when no records are found
    echo json_encode(array("playerCount" => 0));
}

closeConnection($dbConnection);

// Function to establish a database connection
function establishConnection($server, $username, $password, $dbName) 
{
    $connection = new mysqli($server, $username, $password, $dbName);

    if ($connection->connect_error) 
    {
        die("Connection failed: " . $connection->connect_error);
    }

    return $connection;
}

// Function to execute a SQL query and return the result
function executeQuery($connection, $query) 
{
    $result = $connection->query($query);

    if (!$result) {
        die("Query failed: " . $connection->error);
    }

    return $result;
}

// Function to fetch data from the result set and return as an associative array
function fetchPlayerCount($result) 
{
    $row = $result->fetch_assoc();
    return $row["playerCount"];
}

// Function to close the database connection
function closeConnection($connection) 
{
    $connection->close();
}

?>
