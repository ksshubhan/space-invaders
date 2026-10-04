<?php
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_NAME', 'spaceinvaders');

if (isset($_POST['id'])) 
{
    $userID = $_POST['id'];

    // SQL query with INNER JOIN to retrieve titles for a specific user
    $query = "SELECT titles.title_name 
              FROM titles
              INNER JOIN userstitles ON titles.ID = userstitles.itemID
              WHERE userstitles.userID = $userID";

    $dbConnection = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

    $result = executeQuery($dbConnection, $query);

    $titles = fetchTitles($result);

    echo json_encode($titles);

    closeConnection($dbConnection);
} 
else 
{
    echo "Error: 'id' key not set in POST data.";
}

// Function to establish a database connection
function establishConnection($server, $username, $password, $dbName) 
{
    $connection = new mysqli($server, $username, $password, $dbName);
    
    if ($connection->connect_error) {
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

// Function to fetch data from the result set and return as an array
function fetchTitles($result) 
{
    $titles = array();
    
    while ($row = $result->fetch_assoc()) {
        $titles[] = $row['title_name'];
    }

    return $titles;
}

// Function to close the database connection
function closeConnection($connection) 
{
    $connection->close();
}
?>
