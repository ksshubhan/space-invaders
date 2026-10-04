<?php
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_NAME', 'spaceinvaders');

$conn = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

$usernameToCheck = $_POST['username'];

$usernameExists = doesUsernameExist($conn, $usernameToCheck);

// Return the result in JSON format
echo json_encode(array("usernameExists" => $usernameExists));

closeConnection($conn);

// Function to establish a database connection
function establishConnection($server, $username, $password, $dbname)
{
    $conn = new mysqli($server, $username, $password, $dbname);

    // Check for connection errors
    if ($conn->connect_error) {
        die("Connection failed: " . $conn->connect_error);
    }

    return $conn;
}

// Function to check if a username exists in the database
function doesUsernameExist($conn, $usernameToCheck)
{
    // Using prepared statement to prevent SQL injection
    $stmt = $conn->prepare("SELECT * FROM users WHERE username = ?");
    $stmt->bind_param("s", $usernameToCheck);
    $stmt->execute();

    $result = $stmt->get_result();
    return $result->num_rows > 0;
}

// Function to close the database connection
function closeConnection($conn)
{
    $conn->close();
}?>
