<?php
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_NAME', 'spaceinvaders');

$dbConnection = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

$username = $_POST['username']; 

// Use parameterized query to prevent SQL injection
$sql = "SELECT id FROM users WHERE username = ?";
$stmt = $dbConnection->prepare($sql);

if ($stmt === false) 
{
    echo "Error preparing statement: " . $dbConnection->error;
} 
else 
{
    $stmt->bind_param("s", $username);
    
    $stmt->execute();

    $result = $stmt->get_result();

    // Check if user is found
    if ($result->num_rows > 0) 
    {
        $userId = fetchUserId($result);
        echo $userId;
    } 
    else 
    {
        echo "User not found";
    }

    $stmt->close();
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
function fetchUserId($result) 
{
    $row = $result->fetch_assoc();
    return $row['id'];
}

// Function to close the database connection
function closeConnection($connection) 
{
    $connection->close();
}
?>
