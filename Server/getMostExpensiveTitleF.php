<?php

define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_NAME', 'spaceinvaders');

// Checking if the userID parameter is set
if (isset($_POST['userID'])) 
{
    $userID = $_POST['userID'];

    $conn = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

    // Query to get the most expensive title for a user
    $query = "SELECT titles.title_name, titles.price
              FROM titles
              JOIN userstitles ON titles.ID = userstitles.itemID
              WHERE userstitles.userID = ?
              ORDER BY titles.price DESC
              LIMIT 1"; 

    $result = executePreparedStatement($conn, $query, 'i', $userID);

    // Checking if the user owns any titles
    // Returning most expensive title as a JSON response
    if ($result && $result->num_rows > 0) 
    {
        $row = fetchResult($result);
        $mostExpensiveTitle = new stdClass();
        $mostExpensiveTitle->title_name = $row['title_name'];
        $mostExpensiveTitle->price = $row['price'];
        echo json_encode($mostExpensiveTitle);
    } 
    else 
    {
        echo json_encode(array('error' => 'User does not own any titles.'));
    }

    closeConnection($conn);
} 
else 
{
    echo json_encode(array('error' => 'Invalid request. User ID parameter is missing.'));
}

// Function to establish a database connection
function establishConnection($server, $username, $password, $dbName)
{
    $conn = new mysqli($server, $username, $password, $dbName);

    if ($conn->connect_error) 
    {
        die("Connection failed: " . $conn->connect_error);
    }

    return $conn;
}

// Function to execute a prepared statement
function executePreparedStatement($connection, $query, $types, ...$params)
{
    $stmt = $connection->prepare($query);

    if (!$stmt) 
    {
        die("Prepare statement failed.");
    }

    $stmt->bind_param($types, ...$params);
    $stmt->execute();

    $result = $stmt->get_result();

    if (!$result) 
    {
        die("Execute statement failed.");
    }

    return $result;
}

// Function to fetch data from the result set
function fetchResult($result)
{
    return $result->fetch_assoc();
}

// Function to close the database connection
function closeConnection($connection)
{
    $connection->close();
}
?>
