<?php
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_NAME', 'spaceinvaders');

$conn = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

// Check if the user is logged in
if (isUserLoggedIn()) 
{
    logoutUser();

    // Redirect to the login page or any desired page
    header("Location: LoginF.php");
    exit();
} 
else 
{
    // If the user is not logged in, redirect to the login page
    header("Location: LoginF.php");
    exit();
}

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

// Function to check if the user is logged in
function isUserLoggedIn()
{
    return isset($_SESSION['id']) && isset($_SESSION['username']);
}

// Function to logout the user
function logoutUser()
{
    session_unset();

    session_destroy();

    echo "Logout successful";
}
?>
