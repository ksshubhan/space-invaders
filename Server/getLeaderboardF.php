<?php
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_NAME', 'spaceinvaders');

$dbConnection = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

// SQL query to select top 5 scores
$sql = "SELECT * FROM users ORDER BY points DESC LIMIT 5";

$result = executePreparedStatement($dbConnection, $sql);

if ($result->num_rows > 0) 
{
    $leaderboardData = fetchLeaderboardData($result);

    // Output the leaderboard data as JSON
    echo json_encode($leaderboardData);
} 
else 
{
    echo "No scores yet.";
}

closeConnection($dbConnection);

// Function to establish a database connection
function establishConnection($server, $username, $password, $dbName) 
{
    $connection = new mysqli($server, $username, $password, $dbName);
    
    if ($connection->connect_error) {
        die("Connection failed: " . $connection->connect_error);
    }

    return $connection;
}

// Function to execute a prepared statement and return the result
function executePreparedStatement($connection, $sql) 
{
    $stmt = $connection->prepare($sql);

    if ($stmt === FALSE) {
        die("Error in preparing the statement: " . $connection->error);
    }

    $stmt->execute();
    return $stmt->get_result();
}

// Function to fetch data from the result set and return as an array
function fetchLeaderboardData($result) 
{
    $leaderboardData = array();
    
    while ($row = $result->fetch_assoc()) {
        $leaderboardData[] = array(
            'player' => $row['name'],
            'points' => $row['points']
        );
    }

    return $leaderboardData;
}

// Function to close the database connection
function closeConnection($connection) 
{
    $connection->close();
}
?>
