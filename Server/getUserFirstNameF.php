<?php
// Checking if the userID parameter is set
if (isset($_POST['userID'])) 
{
    // Sanitising and getting the user ID
    $userID = htmlspecialchars($_POST['userID']);

    define('DB_SERVER', 'localhost');
    define('DB_USERNAME', 'root');
    define('DB_PASSWORD', '');
    define('DB_NAME', 'spaceinvaders');

    $conn = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

    // Query to fetch user's first name based on their user ID
    $query = "SELECT name FROM users WHERE id = ?";
    $result = executePreparedStatement($conn, $query, 'i', $userID);

    // Returning first name as a JSON response
    if ($result) 
    {
        $row = fetchResult($result);
        $firstName = $row['name'];
        $response = array('firstName' => $firstName);
        echo json_encode($response);
    } 
    else 
    {
        echo "Error executing query.";
    }
    closeConnection($conn);
} 
else 
{
    echo "Error: userID parameter is missing.";
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

// Function to execute prepared statement and return the result
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
