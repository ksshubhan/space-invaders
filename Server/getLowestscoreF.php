<?php
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_NAME', 'spaceinvaders');

$dbConnection = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

// SQL query to get the lowest score and corresponding player
$query = "SELECT name, MIN(points) AS lowest_score FROM users WHERE points = (SELECT MIN(points) FROM users)";

$result = executeQuery($dbConnection, $query);

if ($result->num_rows > 0) 
{
    $row = fetchLowestScore($result);

    // Display the lowest score and corresponding player
    $playerName = $row["name"];
    $lowestScore = $row["lowest_score"];
    echo "Lowest Score: $lowestScore | by Player: $playerName";
} 
else 
{
    echo "No records found";
}

// Close the database connection
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

    if (!$result) 
    {
        die("Query failed: " . $connection->error);
    }

    return $result;
}

// Function to fetch data from the result set and return as an associative array
function fetchLowestScore($result) 
{
    $row = $result->fetch_assoc();
    return $row;
}

// Function to close the database connection
function closeConnection($connection) 
{
    $connection->close();
}
?>
