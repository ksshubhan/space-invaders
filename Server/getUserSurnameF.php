<?php

// Checking if User ID parameter is set 
if (isset($_POST['userID'])) 
{
    $userID = $_POST['userID'];

    define('DB_SERVER', 'localhost');
    define('DB_USERNAME', 'root');
    define('DB_PASSWORD', '');
    define('DB_NAME', 'spaceinvaders');

    $conn = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

    // Query to fetch user's surname based on their user ID
    $sql = "SELECT surname FROM users WHERE id = ?";
    
    $result = executeGetSurnameStatement($conn, $sql, 'i', $userID);

    // Returning surname as a JSON response
    if ($result) {
        $row = fetchResult($result);
        $surname = $row['surname'];
        $response = array('surname' => $surname);
        echo json_encode($response);
    } 
    else 
    {
        echo json_encode(array('surname' => 'User not found'));
    }

    closeConnection($conn);
} 
else 
{
    echo json_encode(array('surname' => 'Invalid request'));
}

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

// Function to execute a prepared statement for fetching user's surname
function executeGetSurnameStatement($connection, $query, $types, ...$params) 
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

// Function to fetch data from the result set and return it as an associative array
function fetchResult($result) 
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
