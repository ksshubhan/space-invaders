<?php
define('DB_SERVER', 'localhost');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', '');
define('DB_NAME', 'spaceinvaders');

$dbConnection = establishConnection(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);

// Get user ID and title ID from the Unity request
$userID = $_POST['userID'];
$titleID = $_POST['titleID'];

// Check if the user already owns the title
$sqlCheckOwnership = "SELECT * FROM userstitles WHERE userID = ? AND itemID = ?";
$stmtCheckOwnership = $dbConnection->prepare($sqlCheckOwnership);

if ($stmtCheckOwnership === false) 
{
    echo "Error preparing statement: " . $dbConnection->error;
} 
else 
{
    $stmtCheckOwnership->bind_param("ii", $userID, $titleID);

    $stmtCheckOwnership->execute();

    $resultCheckOwnership = $stmtCheckOwnership->get_result();

    if ($resultCheckOwnership->num_rows > 0) 
    {
        echo "You already own this title.";
    } 
    else 
    {
        // Get user's points
        $sqlGetPoints = "SELECT points FROM users WHERE id = ?";
        $stmtGetPoints = $dbConnection->prepare($sqlGetPoints);

        if ($stmtGetPoints === false) 
        {
            echo "Error preparing statement: " . $dbConnection->error;
        } 
        else 
        {
            $stmtGetPoints->bind_param("i", $userID);

            $stmtGetPoints->execute();

            $resultGetPoints = $stmtGetPoints->get_result();
            $row = $resultGetPoints->fetch_assoc();
            $userPoints = $row['points'];

            // Get title's details including price
            $sqlGetTitleDetails = "SELECT title_name, price FROM titles WHERE ID = ?";
            $stmtGetTitleDetails = $dbConnection->prepare($sqlGetTitleDetails);

            if ($stmtGetTitleDetails === false) 
            {
                echo "Error preparing statement: " . $dbConnection->error;
            } 
            else 
            {
                $stmtGetTitleDetails->bind_param("i", $titleID);
                
                $stmtGetTitleDetails->execute();

                $resultGetTitleDetails = $stmtGetTitleDetails->get_result();
                $rowTitleDetails = $resultGetTitleDetails->fetch_assoc();
                $titleName = $rowTitleDetails['title_name'];
                $titlePrice = $rowTitleDetails['price'];

                // Check if the user has enough points to buy the title
                if ($userPoints >= $titlePrice) 
                {
                    // Deduct points from the user
                    $newUserPoints = $userPoints - $titlePrice;
                    $sqlUpdatePoints = "UPDATE users SET points = ? WHERE id = ?";
                    $stmtUpdatePoints = $dbConnection->prepare($sqlUpdatePoints);

                    if ($stmtUpdatePoints === false) 
                    {
                        echo "Error preparing statement: " . $dbConnection->error;
                    } 
                    else 
                    {
                        $stmtUpdatePoints->bind_param("ii", $newUserPoints, $userID);

                        $stmtUpdatePoints->execute();

                        $sqlAddTitle = "INSERT INTO userstitles (userID, itemID) VALUES (?, ?)";
                        $stmtAddTitle = $dbConnection->prepare($sqlAddTitle);

                        if ($stmtAddTitle === false) 
                        {
                            echo "Error preparing statement: " . $dbConnection->error;
                        } 
                        else 
                        {
                            $stmtAddTitle->bind_param("ii", $userID, $titleID);

                            $stmtAddTitle->execute();

                            echo "Purchase successful. You now own the title: $titleName.";
                        }
                        $stmtAddTitle->close();
                    }
                    $stmtUpdatePoints->close();
                } 
                else 
                {
                    echo "Not enough points.";
                }
                $stmtGetTitleDetails->close();
            }
            $stmtGetPoints->close();
        }
    }
    $stmtCheckOwnership->close();
}

closeConnection($dbConnection);

// Function to establish a database connection
function establishConnection($server, $username, $password, $dbName) {
    $connection = new mysqli($server, $username, $password, $dbName);

    if ($connection->connect_error) {
        die("Connection failed: " . $connection->connect_error);
    }

    return $connection;
}

// Function to execute a SQL query and return the result
function executeQuery($connection, $query) {
    $result = $connection->query($query);

    if (!$result) {
        die("Query failed: " . $connection->error);
    }

    return $result;
}

// Function to close the database connection
function closeConnection($connection) 
{
    $connection->close();
}
?>
