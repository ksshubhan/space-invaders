<?php
session_start();

$servername = "localhost";
$username   = "root";
$password   = "";
$dbname     = "spaceinvaders";

// Establish a connection to the database
$conn = establishConnection($servername, $username, $password, $dbname);

// Retrieve user input from the POST request
$username = $_POST['username'];
$password = $_POST['password'];

// Trim and hash the entered password
$hashedEnteredPassword = trim(UnityHashPassword($password));

// Query the database for the user with the provided username
$result = queryDatabaseForUser($conn, $username);

// Check if a user with the provided username exists
if ($result->num_rows > 0) 
{
    $row = $result->fetch_assoc();

    $hashedStoredPassword = trim($row['password']); 

    authenticateUser($hashedStoredPassword, $hashedEnteredPassword, $row);
} 
else 
{
    echo "User not found";
}

// Close the database connection
closeConnection($conn);

// Function to establish a database connection
function establishConnection($servername, $username, $password, $dbname) 
{
    $conn = new mysqli($servername, $username, $password, $dbname);

    // Check for connection errors
    if ($conn->connect_error) {
        die("Connection failed: " . $conn->connect_error);
    }

    return $conn;
}

// Function to query the database for the user with the provided username
function queryDatabaseForUser($conn, $username) 
{
    $stmt = $conn->prepare("SELECT * FROM users WHERE username = ?");
    $stmt->bind_param("s", $username);
    $stmt->execute();
    return $stmt->get_result();
}

// Function to authenticate the user
function authenticateUser($hashedStoredPassword, $hashedEnteredPassword, $row) 
{
    if ($hashedStoredPassword === $hashedEnteredPassword) {
        // Set session variables for the authenticated user
        $_SESSION['id']       = $row['id'];
        $_SESSION['username'] = $row['username'];

        echo "Login successful";
    } else {
        echo "Invalid password";
    }
}

// Function to close the database connection
function closeConnection($conn) 
{
    $conn->close();
}

// Function to hash passwords (consider using PHP's password_hash() for better security)
function UnityHashPassword($password) 
{
    $hash = 0;

    if (empty($password)) {
        return $hash;
    }

    // Custom password hashing logic
    for ($i = 0; $i < strlen($password); $i++) {
        $currentChar = $password[$i];
        $hash = ($hash << 5) - $hash + ord($currentChar);
        $hash = $hash & $hash;
    }

    return $hash;
}
?>
