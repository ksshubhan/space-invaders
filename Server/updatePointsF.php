<?php
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_NAME', 'spaceinvaders');

$dbConnection = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

// Update user points
$id = $_POST['id'];
$points = $_POST['points'];

echo "Received points: " . $points . "\n"; 

// Use parameterized query to prevent SQL injection
$sql = "UPDATE users SET points = points + ? WHERE id = ?";
$paramTypes = "ii";
$params = [$points, $id];

if (executePreparedStatement($dbConnection, $sql, $paramTypes, $params)) 
{
    echo "Points updated successfully";
} 
else 
{
    echo "Error updating points.";
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

// Function to execute a prepared statement and return the result
function executePreparedStatement($connection, $sql, $paramTypes, $params) 
{
    $stmt = $connection->prepare($sql);

    if ($stmt === false) 
    {
        echo "Error preparing statement: " . $connection->error;
        return false;
    }

    $stmt->bind_param($paramTypes, ...$params);
    
    if ($stmt->execute()) 
    {
        return true;
    } 
    else 
    {
        echo "Error executing statement: " . $stmt->error;
        return false;
    }

    $stmt->close();
}

// Function to close the database connection
function closeConnection($connection) {
    $connection->close();
}

?>
